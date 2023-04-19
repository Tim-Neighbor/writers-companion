
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace TheWritersCompanion
{
    /// <summary>
    /// Defines ProjectImporter Objects, which import projects from databases
    /// </summary>
    /// <remarks>
    /// Rudy Fisher
    /// </remarks>
    public class ProjectImporter : IProjectImporter
    {
        #region MySql Constants
        private const string SELECT_PROJECT_QUERY =
            "SELECT name FROM Project WHERE name = @projectName;";
        private const string SELECT_CATEGORIES_QUERY =
            "SELECT * FROM Category WHERE projectName = @projectName;";
        private const string SELECT_NOTES_QUERY =
            "SELECT * FROM Note WHERE projectName = @projectName;";

        private const string NOTE_NAME_PARAMETER = "@noteName";
        private const string PROJECT_NAME_PARAMETER = "@projectName";
        private const string CATEGORY_NAME_PARAMETER = "@categoryName";
        private const string NOTE_CONTENT_PARAMETER = "@noteContent";

        private const int CATEGORY_NAME_COLUMN_INDEX = 0;
        private const int NOTE_NAME_COLUMN_INDEX = 0;
        private const int NOTE_CATEGORY_COLUMN_INDEX = 1;
        private const int NOTE_CONTENT_COLUMN_INDEX = 3;
        #endregion

        private const string ESCAPE = "\\";

        private string FILE_EXTENSION = GlobalConstants.FILE_EXTENSION;

        private string NAME_OF_CONFIG_FILE = GlobalConstants.NAME_OF_CONFIG_FILE;

        private string CONTENT_OF_CONFIG_FILE = GlobalConstants.CONTENT_OF_CONFIG_FILE;

        /// <summary>
        /// Imports the given project from the database to the given directory
        /// </summary>
        /// <param name="projectName">The name of the project to import 
        ///                           as string</param>
        /// <param name="userName">The username credential to connect to the 
        ///                        database as string</param>
        /// <param name="password">The password credential to connect to the 
        ///                        database as string</param>
        /// <param name="directory">The directory path to import the project to
        ///                         as string</param>
        /// <returns>The outcome of the importation process, i.e. whether it 
        ///          was a success or a meaningful description of the error 
        ///          that occured if it failed as OutcomeCode</returns>
        public OutcomeCode ImportProject(string projectName, string directory,
                                         string userName, string password)
        {
            string connectionString = "server=127.0.0.1;user=" + userName +
              ";password=" + password + ";database=" + userName + ";port=3306";

            OutcomeCode outcome = OutcomeCode.SUCCESS;
            List<Category> categories = new List<Category>();
            List<Note> notes = new List<Note>();

            try
            {
                using (MySqlConnection connection =
                   new MySqlConnection(connectionString))
                {
                    if (CheckIfProjectExists(connection, projectName))
                    {
                        string fullPath = directory + ESCAPE + projectName;
                        outcome = RetrieveProjectFromDatabase(connection, projectName,
                                              fullPath, categories, notes);

                        if (outcome == OutcomeCode.SUCCESS)
                        {
                            BuildProject(projectName, fullPath, categories,
                                            notes);
                        }

                        #region Debug
                        /*
                        foreach (Category category in categories)
                        {
                            Console.WriteLine(category.ToString());
                        }

                        foreach (Note note in notes)
                        {
                            Console.WriteLine(note.ToString());
                        }
                        */
                        #endregion
                    }
                    else
                    {
                        outcome = OutcomeCode.PROJECT_DOES_NOT_EXIST_IN_DATABASE;
                    }
                }
            }
            catch (Exception)
            {
                outcome = OutcomeCode.FAILURE;
            }

            return outcome;
        }

        /// TODO: see RetrieveNotes
        /// <summary>
        /// Retrieves the full project from the database
        /// </summary>
        /// <param name="connection">The connection to the database
        ///                          as MySqlConnection</param>
        /// <param name="projectName">The name of the project
        ///                           as string</param>
        /// <param name="categories">The project's categories, to populate
        ///                           as List of Category objects</param>
        /// <param name="notes">The project's notes, to populate as List of 
        ///                     Note objects</param>
        /// <exception cref="MySqlException"/>
        public OutcomeCode RetrieveProjectFromDatabase(
            MySqlConnection connection, string projectName,
            string projectDirectory, List<Category> categories,
            List<Note> notes)
        {
            OutcomeCode outcome = OutcomeCode.SUCCESS;
            outcome = RetrieveCategories(connection, projectName,
                                        projectDirectory, categories) ?
                      OutcomeCode.SUCCESS :
                      OutcomeCode.UNABLE_TO_RETRIEVE_CATEGORIES;

            outcome = RetrieveNotes(connection, projectName,
                                    projectDirectory, notes) ?
                                    OutcomeCode.SUCCESS :
                                    OutcomeCode.UNABLE_TO_RETRIEVE_NOTES;

            return outcome;
        }

        /// <summary>
        /// Checks if the project exists in the database
        /// </summary>
        /// <param name="connection">The connection to the database
        ///                          as MySqlConnection</param>
        /// <param name="projectName">The name of the project
        ///                           as string</param>
        /// <returns>True if project exists, otherwise false</returns>
        /// <exception cref="MySqlException"/>
        public bool CheckIfProjectExists(MySqlConnection connection,
                                            string projectName)
        {
            bool projectExists = false;

            MySqlCommand command = new MySqlCommand(SELECT_PROJECT_QUERY,
                                                        connection);

            command.Parameters.AddWithValue(PROJECT_NAME_PARAMETER,
                                                    projectName);

            command.Connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                projectExists = reader.HasRows;
            }

            command.Connection.Close();

            return projectExists;
        }

        /// <summary>
        /// Retrieves the project's categories from the database
        /// </summary>
        /// <param name="connection">The connection to the database
        ///                          as MySqlConnection</param>
        /// <param name="projectName">The name of the project
        ///                           as string</param>
        /// <param name="projectDirectory">The project's directory
        ///                         as a string</param>
        /// <param name="categories">The project's categories, to populate
        ///                           as List of Category objects</param>
        /// <returns>True if all categories were successfully put
        ///         into List, otherwise false</returns>
        /// <exception cref="MySqlException"/>
        public bool RetrieveCategories(MySqlConnection connection,
                        string projectName, string projectDirectory,
                        List<Category> categories)
        {
            bool isSuccessfullyRetrievingColumns = true;
            MySqlCommand command = new MySqlCommand(SELECT_CATEGORIES_QUERY,
                                                        connection);

            command.Parameters.AddWithValue(PROJECT_NAME_PARAMETER,
                                                    projectName);

            command.Connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read() && isSuccessfullyRetrievingColumns)
                    {
                        object categoryNameObject =
                               reader.GetValue(CATEGORY_NAME_COLUMN_INDEX);

                        string categoryName;
                        isSuccessfullyRetrievingColumns =
                            GetColumnValue<string>(reader,
                            CATEGORY_NAME_COLUMN_INDEX, out categoryName);

                        if (isSuccessfullyRetrievingColumns)
                        {
                            categories.Add(new Category(categoryName,
                                                    projectDirectory));
                        }
                    }
                }
            }

            command.Connection.Close();

            return isSuccessfullyRetrievingColumns;
        }

        /// TODO: finish populating note list when content is changed
        ///       from string[] to string
        /// <summary>
        /// Retrieves the project's notes from the database
        /// </summary>
        /// <param name="connection">The connection to the database
        ///                          as MySqlConnection</param>
        /// <param name="projectName">The name of the project
        ///                           as string</param>
        /// <param name="projectDirectory">The project's directory
        ///                         as a string</param>
        /// <param name="notes">The list of notes to populate
        ///                 as a List of Note objects</param>
        /// <returns>True if all notes were successfully put
        ///         into List, otherwise false</returns>
        /// <exception cref="MySqlException"/>
        public bool RetrieveNotes(MySqlConnection connection,
            string projectName, string projectDirectory, List<Note> notes)
        {
            bool isSuccessfullyRetrievingColumns = true;
            MySqlCommand command = new MySqlCommand(SELECT_NOTES_QUERY,
                                                        connection);

            command.Parameters.AddWithValue(PROJECT_NAME_PARAMETER,
                                                    projectName);

            command.Connection.Open();

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read() && isSuccessfullyRetrievingColumns)
                    {
                        string noteName;
                        string noteCategoryName;
                        string noteContent;

                        isSuccessfullyRetrievingColumns =
                            GetColumnValue<string>(reader,
                            NOTE_NAME_COLUMN_INDEX, out noteName);

                        if (isSuccessfullyRetrievingColumns)
                        {
                            isSuccessfullyRetrievingColumns =
                                GetColumnValue<string>(reader,
                                NOTE_CATEGORY_COLUMN_INDEX, out noteCategoryName);

                            if (isSuccessfullyRetrievingColumns)
                            {
                                isSuccessfullyRetrievingColumns =
                                    GetColumnValue<string>(reader,
                                    NOTE_CONTENT_COLUMN_INDEX,
                                    out noteContent);

                                notes.Add(new Note(noteName, new Category(noteCategoryName),
                                    noteContent, projectDirectory + ESCAPE +
                                            noteCategoryName));

                            }
                        }
                    }
                }
            }

            command.Connection.Close();

            return isSuccessfullyRetrievingColumns;
        }


        /// <summary>
        /// Gets the value at the column index of the reader's current row
        /// </summary>
        /// <typeparam name="T">The type to cast the value
        ///                     to</typeparam>
        /// <param name="reader">The reader with the table data from the
        ///                     database as MySqlDataReader</param>
        /// <param name="columnIndex">The index of the desired value
        ///                         as int</param>
        /// <param name="columnValue">The value to get from the row
        ///                 as an out parameter of type T</param>
        /// <returns>True if value was of type T, otherwise,
        ///          false</returns>
        /// <exception cref="MySqlException"/>
        public bool GetColumnValue<T>(MySqlDataReader reader, int columnIndex,
                                        out T columnValue)
        {
            object columnObject = reader.GetValue(columnIndex);
            bool isSuccessfullyRetrievingColumns = true;

            columnValue = default;
            if (columnObject is T)
            {
                columnValue = (T)columnObject;
            }
            else
            {
                isSuccessfullyRetrievingColumns = false;
                Debug.WriteLine("Column value is NOT string!");
            }

            return isSuccessfullyRetrievingColumns;
        }

        /// <summary>
        /// Builds the project from the database at the specified parent
        /// directory
        /// </summary>
        /// <param name="projectName">The project's name to build
        ///                         as a string</param>
        /// <param name="fullProjectPath">The project's desired
        ///         parent directory as string</param>
        /// <param name="categories">The categories contained
        ///         within the project as a List of Category
        ///         objects</param>
        /// <param name="notes">The notes contained
        ///         within the project as a List of Note
        ///         objects</param>
        public OutcomeCode BuildProject(string projectName, string fullProjectPath,
                                   List<Category> categories, List<Note> notes)
        {
            OutcomeCode outcome = OutcomeCode.SUCCESS;

            try
            {
                DirectoryInfo info = Directory.CreateDirectory(fullProjectPath);
                if (!info.Exists)
                {
                    outcome = OutcomeCode.UNABLE_TO_CREATE_PROJECT_ROOT_FOLDER;
                }
            }
            catch (Exception e)
            {
                outcome = OutcomeCode.UNABLE_TO_CREATE_PROJECT_ROOT_FOLDER;
            }

            if (outcome == OutcomeCode.SUCCESS)
            {
                try
                {
                    string configPath = fullProjectPath + ESCAPE +
                        NAME_OF_CONFIG_FILE + FILE_EXTENSION;
                    File.Create(configPath).Close();

                    if (!File.Exists(configPath))
                    {
                        outcome = OutcomeCode.UNABLE_TO_CREATE_PROJECT_CONFIG_FILE;
                    }
                    else
                    {
                        File.WriteAllText(configPath, CONTENT_OF_CONFIG_FILE);
                    }
                }
                catch (Exception e)
                {
                    outcome = OutcomeCode.UNABLE_TO_CREATE_PROJECT_CONFIG_FILE;
                }
            }

            if (outcome == OutcomeCode.SUCCESS)
            {
                foreach (Category category in categories)
                {
                    try
                    {
                        DirectoryInfo info = 
                            Directory.CreateDirectory(category.FolderPath);

                        if (!info.Exists)
                        {
                            outcome = OutcomeCode.UNABLE_TO_CREATE_CATEGORY;
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        outcome = OutcomeCode.UNABLE_TO_CREATE_CATEGORY;
                    }
                }
            }

            if (outcome == OutcomeCode.SUCCESS)
            {
                foreach (Note note in notes)
                {
                    try
                    {
                        File.Create(note.FilePath).Close();

                        if (!File.Exists(note.FilePath))
                        {
                            outcome = OutcomeCode.UNABLE_TO_CREATE_NOTE;
                            break;
                        }
                        else
                        {
                            File.WriteAllText(note.FilePath, note.Content);
                        }
                    }
                    catch (Exception e)
                    {
                        outcome = OutcomeCode.UNABLE_TO_CREATE_NOTE;
                    }
                }
            }

            return outcome;
        }
    }
}
