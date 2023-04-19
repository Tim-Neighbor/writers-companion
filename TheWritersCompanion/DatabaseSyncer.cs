using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Configuration;

namespace TheWritersCompanion
{
    /// <summary>
    /// Defines DatabaseSyncer Objects
    /// </summary>
    /// <remarks>
    /// Rudy Fisher
    /// </remarks>
    public class DatabaseSyncer : IDataSyncer
    {
        #region MySql Constants
        public const string SAFE_INSERT_PROJECT_QUERY =
            "INSERT IGNORE INTO Project (name) VALUES (@projectName);";
        public const string SAFE_INSERT_CATEGORY_QUERY =
            "INSERT IGNORE INTO Category (name, projectName)" +
                " VALUES (@categoryName, @projectName);";
        public const string SAFE_INSERT_NOTE_QUERY =
            "INSERT INTO Note (name, categoryName, projectName, content)" +
                " VALUES (@noteName, @categoryName, @projectName, @noteContent)" +
                    "ON DUPLICATE KEY UPDATE content = @noteContent;";

        private const string NOTE_NAME_PARAMETER_VALUE = "@noteName";
        private const string PROJECT_NAME_PARAMETER_VALUE = "@projectName";
        private const string CATEGORY_NAME_PARAMETER_VALUE = "@categoryName";
        private const string NOTE_CONTENT_PARAMETER_VALUE = "@noteContent";
        private const string NO_PARAMETER_VALUE = "";
        #endregion

        private const char ESCAPE = '\\';

        private IProjectLoader projectLoader;

        public DatabaseSyncer()
        {
        }

        /// <summary>
        /// Syncs the current project to the database for backup storage
        /// </summary>
        /// <param name="projectDirectoryPath">The project's current directory
        ///                                as string</param>
        /// <param name="userName">The username credential to connect to the 
        ///                        database as string</param>
        /// <param name="password">The password credential to connect to the 
        ///                        database as string</param>
        /// <returns>The outcome of the importation process, i.e. whether it 
        ///          was a success or a meaningful description of the error 
        ///          that occured if it failed as OutcomeCode</returns>
        public OutcomeCode SyncData(string projectDirectoryPath, string userName,
                                    string password)
        {
            string connectionString = "server=127.0.0.1;user=" + userName +
              ";password=" + password + ";database=" + userName + ";port=3306";

            OutcomeCode outcome = OutcomeCode.SUCCESS;
            List<Category> categories = new List<Category>();
            List<Note> notes = new List<Note>();

            int index = projectDirectoryPath.LastIndexOf(ESCAPE) + 1;
            string projectName = projectDirectoryPath.Substring(index);

            LoadProject(projectDirectoryPath, categories, notes);

            try
            {
                using (MySqlConnection connection =
                new MySqlConnection(connectionString))
                {
                    // Delete previous synced data for this project
                    DeleteProject(projectName, connection);

                    // Sync project
                    outcome = InsertRow(connection, SAFE_INSERT_PROJECT_QUERY,
                            OutcomeCode.UNABLE_TO_INSERT_PROJECT_INTO_DATABASE,
                            projectName);

                    if (outcome == OutcomeCode.SUCCESS)
                    {
                        // Sync categories
                        int i = 0;
                        while (i < categories.Count && outcome == OutcomeCode.SUCCESS)
                        {
                            outcome = InsertRow(connection, SAFE_INSERT_CATEGORY_QUERY,
                                OutcomeCode.UNABLE_TO_INSERT_CATEGORY_INTO_DATABASE,
                                                projectName, categories[i].Name);
                            i++;
                        }

                        if (outcome == OutcomeCode.SUCCESS)
                        {
                            // Sync notes
                            i = 0;
                            while (i < notes.Count && outcome == OutcomeCode.SUCCESS)
                            {
                                outcome = InsertRow(connection, SAFE_INSERT_NOTE_QUERY,
                                OutcomeCode.UNABLE_TO_INSERT_NOTE_INTO_DATABASE,
                                projectName, notes[i].Category.Name, notes[i].Name,
                                notes[i].Content);
                                i++;
                            }
                        }

                    }
                }
            }
            catch (MySqlException e)
            {
                outcome = OutcomeCode.FAILURE;
                Debug.WriteLine(e.Message);
            }

            return outcome;
        }

        /// <summary>
        /// Deletes the synced data for the project from the database
        /// </summary>
        /// <param name="projectName">The name of the project to delete
        ///                           from the project as string</param>
        /// <param name="connection">The connection to the database
        ///                          as MySqlConnection</param>
        public void DeleteProject(string projectName, MySqlConnection connection)
        {
            string query = "DELETE FROM Project WHERE name = @projectName;";
            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue(PROJECT_NAME_PARAMETER_VALUE,
                                                projectName);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (MySqlException e)
            {
            }
        }


        /// <summary>
        /// Inserts a row into a table of the database
        /// </summary>
        /// <param name="connection">The connection to the database 
        ///                          as MySQLConnection</param>
        /// <param name="projectName">The name of the project as string</param>
        /// <param name="categoryName">The name of the category 
        ///                            as string</param>
        /// <param name="noteName">The name of the note as string</param>
        /// <param name="noteContents">The note's contents as string</param>
        /// <remarks>
        /// Optional parameters are ordered by precedence of existance,
        /// e.g. a projectName must be given before a categoryName, etc.
        /// </remarks>
        public OutcomeCode InsertRow(MySqlConnection connection, string query,
                                      OutcomeCode failedOutcome =
                              OutcomeCode.UNABLE_TO_INSERT_ROW_INTO_DATABASE,
                                      string projectName = NO_PARAMETER_VALUE,
                                      string categoryName = NO_PARAMETER_VALUE,
                                      string noteName = NO_PARAMETER_VALUE,
                                      string noteContents = NO_PARAMETER_VALUE)
        {
            OutcomeCode outcome = OutcomeCode.SUCCESS;

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);

                if (projectName != NO_PARAMETER_VALUE)
                {
                    cmd.Parameters.AddWithValue(PROJECT_NAME_PARAMETER_VALUE,
                                                projectName);

                    if (categoryName != NO_PARAMETER_VALUE)
                    {
                        cmd.Parameters.AddWithValue(CATEGORY_NAME_PARAMETER_VALUE,
                                                    categoryName);

                        if (noteName != NO_PARAMETER_VALUE)
                        {
                            cmd.Parameters.AddWithValue(NOTE_NAME_PARAMETER_VALUE,
                                                        noteName);
                            cmd.Parameters.AddWithValue(NOTE_CONTENT_PARAMETER_VALUE,
                                                        noteContents);
                        }
                    }
                }

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message + " projname:" +
                                projectName + " catname:" +
                               categoryName + " notename:" +
                                   noteName + " notecont:" +
                                   noteContents);
                outcome = failedOutcome;
            }

            return outcome;
        }

        /// <summary>
        /// Loads the project
        /// </summary>
        /// <param name="categories">The categories to populate
        ///                          as a List of Category objects</param>
        /// <param name="notes">The notes to populate
        ///                          as a List of Note objects</param>
        public void LoadProject(string projectDirectory,
            List<Category> categories, List<Note> notes)
        {
            projectLoader = new ProjectLoader(projectDirectory);
            projectLoader.LoadProject(categories, notes);
        }
    }
}
