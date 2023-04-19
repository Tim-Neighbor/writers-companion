using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Documents;
using System.Windows.Data;
using System.Windows;

namespace TheWritersCompanion
{
    /// <summary>
    /// Manages the directory and files for a project of TheWritersCompanion
    /// Author: Tim Neighbor
    /// </summary>
    public class DirectoryManager : IStorageManager
    {
        #region Fields
        // what the outcome of building the project was
        private OutcomeCode outcomeOfProjectBuild;

        // stores the current project's main directory
        private string currentDirectory;

        // List of all Notes
        private List<Note> notes;

        // List of all Categories
        private List<Category> categories;

        // extension used for all Note files
        private const string FILE_EXTENSION = ".txt";

        // name of config file used to identify a directory as a valid project
        private string NAME_OF_CONFIG_FILE = "TWCconfig";

        // content that each config file should contain
        private string CONTENT_OF_CONFIG_FILE = "DO NOT DELETE THIS FILE - " +
            "It is necessary to identify projects of The Writers Companion";

        // forbidden characters
        private string[] charsToRemove = new string[] { "\r", "\n" };

        #endregion

        #region Contructors

        /// <summary>
        /// Constructor that initializes a DirectoryManager and all of its 
        /// fields by accessing the relevant files and folders within the given
        /// directory.
        /// </summary>
        /// <param name="currentDirectory">the current project's main 
        /// directory</param>
        public DirectoryManager(string currentDirectory, bool isNewProject)
        {
            this.currentDirectory = currentDirectory;

            InitializeOutcomeOfProjectBuild(isNewProject);

            if (outcomeOfProjectBuild == OutcomeCode.SUCCESS)
            {

                notes = new List<Note>();
                categories = new List<Category>();

                // all folders in the main directory are categories
                List<string> folderPaths =
                    Directory.GetDirectories(currentDirectory).ToList<string>();

                // keep track of all note files and their paths
                Dictionary<string, Category> notePaths =
                    new Dictionary<string, Category>();

                // create Category objects, and add to List
                InitializeCategories(folderPaths, notePaths);

                // create note objects and add them to the List
                InitializeNotes(notePaths);
            }

        }

        #endregion

        #region ConstructorHelpers
        /// <summary>
        /// Helper method that initializes the outcome of project build based
        /// on the current directory and whether or not the project is new.
        /// </summary>
        /// <param name="isNewProject">Whether or not the project is new
        /// </param>
        private void InitializeOutcomeOfProjectBuild(bool isNewProject)
        {
            string pathToConfig = currentDirectory + "\\" +
                NAME_OF_CONFIG_FILE + FILE_EXTENSION;

            if (!isNewProject && !File.Exists(pathToConfig))
            {
                outcomeOfProjectBuild = OutcomeCode.PROJECT_DOES_NOT_EXIST;
            } 
            else if (isNewProject && 
                Directory.EnumerateFileSystemEntries(currentDirectory).Any())
            {
                outcomeOfProjectBuild = OutcomeCode.DIRECTORY_NOT_EMPTY;
            }
            else
            {
                if (isNewProject)
                {
                    File.WriteAllText(pathToConfig, CONTENT_OF_CONFIG_FILE);
                }
                outcomeOfProjectBuild = OutcomeCode.SUCCESS;
            }
        }

        /// <summary>
        /// Helper method that initializes the Categories from the file system
        /// while also retrieving the paths for the note files  from the file 
        /// system.
        /// </summary>
        /// <param name="folderPaths">A list of paths to all folders, from 
        /// which the Categories are created</param>
        /// <param name="notePaths">A dictionary to store the paths to all note
        /// files which are mapped to the appropriate Category</param>
        private void InitializeCategories(List<string> folderPaths,
            Dictionary<string, Category> notePaths)
        {
            foreach (string folderPath in folderPaths)
            {
                Category category = new Category();

                int lastIndexofBackslash = folderPath.LastIndexOf('\\');
                category.Name = folderPath.Substring(lastIndexofBackslash + 1);
                category.FolderPath = folderPath;
                categories.Add(category);

                // check each folder for files, save the files paths
                List<string> tempNotePaths =
                    Directory.GetFiles(folderPath).ToList<string>();
                foreach (string tempNotePath in tempNotePaths)
                {
                    notePaths.Add(tempNotePath, category);
                }
            }
        }

        /// <summary>
        /// Helper method that initializes the Notes from the file system.
        /// </summary>
        /// <param name="notePaths">A dictionary to store the paths to all note
        /// files which are mapped to the appropriate Category</param>
        private void InitializeNotes(Dictionary<string, Category> notePaths)
        {
            foreach (KeyValuePair<string, Category> entry in notePaths)
            {
                Note note = new Note();

                // isolate the name of the Note with no file extension
                int lastIndexofBackslash = entry.Key.LastIndexOf('\\');
                string fileName =
                    entry.Key.Substring(lastIndexofBackslash + 1);
                int indexOfPeriod = fileName.IndexOf('.');

                note.Name = fileName.Substring(0, indexOfPeriod);
                note.Category = entry.Value;
                // add the current Note to its Category's List of Notes
                note.Category.notesInCategory.Add(note);
                note.FilePath = entry.Key;
                note.Content = File.ReadAllText(entry.Key);

                notes.Add(note);
            }
        }


        #endregion

        #region DirectoryMethods

        /// <summary>
        /// Returns the outcome of the project build.
        /// </summary>
        /// <returns>The outcome of the project build</returns>
        public OutcomeCode OutcomeOfProjectBuild()
        {
            return outcomeOfProjectBuild;
        }

        #endregion

        #region StringParsingMethods

        /// <summary>
        /// Returns a copy of the given string, but with all of the
        /// specified characters removed from it.
        /// </summary>
        /// <param name="str">The string to remove characters from</param>
        /// <param name="charsToRemove">An array of characters that are to be
        /// removed from the string</param>
        /// <returns>A copy of the string with the specified characters removed
        /// </returns>
        private string RemoveForbiddenCharacters(string str,
            string[] charsToRemove)
        {
            string newStr = str;

            foreach (string character in charsToRemove)
            {
                newStr = newStr.Replace(character, string.Empty);
            }

            return newStr;
        }

        #endregion

        #region CategoryMethods

        /// <summary>
        /// Determines if the Category, specified by name, exists.
        /// </summary>
        /// <param name="categoryName">Name of the Category</param>
        /// <returns>True if Category exists, otherwise false</returns>
        public bool CategoryExists(string categoryName)
        {
            return categories.Exists(
                category => category.Name.Equals(categoryName));
        }



        /// <summary>
        /// Creates a new Category object, and adds a folder to the main
        /// directory.
        /// </summary>
        /// <param name="categoryName">Name of the Category to create</param>
        /// <returns>Outcome of Category creation; returns: SUCCESS or 
        /// CATEGORY_ALREADY_EXISTS</returns>
        public OutcomeCode CreateCategory(string categoryName)
        {
            string newCategoryName = RemoveForbiddenCharacters(categoryName, charsToRemove);

            if (!CategoryExists(newCategoryName))
            {
                Category category = new Category(categoryName, currentDirectory);
                Directory.CreateDirectory(category.FolderPath);
                categories.Add(category);

                return OutcomeCode.SUCCESS;
            }
            else
            {
                return OutcomeCode.CATEGORY_ALREADY_EXISTS;
            }
        }



        /// <summary>
        /// Removes the Category, specified by name, and all notes within that
        /// category, from the List and persistent storage.
        /// </summary>
        /// <param name="categoryName">Name of the category that is being
        /// removed along with its notes</param>
        /// <returns>Outcome of the removal process; returns: SUCCESS or
        /// CATEGORY_DOES_NOT_EXIST</returns>
        public OutcomeCode RemoveCategory(string categoryName)
        {
            if (CategoryExists(categoryName))
            {
                Category category = GetCategory(categoryName);

                Directory.Delete(category.FolderPath, true);
                categories.Remove(category);
                foreach (Note note in category.notesInCategory)
                {
                    notes.Remove(note);
                }
                // CORRECT OUTCOME CODE
                return OutcomeCode.SUCCESS;
            }
            else
            {
                return OutcomeCode.CATEGORY_DOES_NOT_EXIST;
            }
        }



        /// <summary>
        /// Returns the Category specified by name. Returns null if no such
        /// Category exists
        /// </summary>
        /// <param name="categoryName">Name of the Category</param>
        /// <returns>The Category specified by name; null if category does
        /// not exist</returns>
        public Category GetCategory(string categoryName)
        {
            foreach (Category category in categories)
            {
                if (category.Name.Equals(categoryName))
                {
                    return category;
                }
            }
            // that category was not found, return null
            return null;
        }



        /// <summary>
        /// Returns a list of all categories in the current project
        /// </summary>
        /// <returns>list of all categories</returns>
        public List<Category> GetAllCategories()
        {
            return categories;
        }

        #endregion

        #region NoteMethods

        /// <summary>
        /// Determines if the Note, specified by Note name and Category name,
        /// exists.
        /// </summary>
        /// <param name="notename">Name of the Note</param>
        /// <param name="categoryName">Name of the Note's Category</param>
        /// <returns></returns>
        public bool NoteExists(string notename, string categoryName)
        {
            return notes.Exists(note => note.Name.Equals(notename) &&
                note.Category.Name.Equals(categoryName));
        }



        /// <summary>
        /// Creates a Note with the specified fields and adds it to the List
        /// of Notes stored by the DirectoryManager but does not save the Note
        /// to persistent storage. The specified category MUST exist prior to 
        /// this method being called.
        /// </summary>
        /// <param name="noteName">Name of the note to be created</param>
        /// <param name="categoryName">Name of the Category to add the Note to
        /// </param>
        /// <returns>Outcome of the Note creation; returns: SUCCESS,
        /// NOTE_ALREADY_EXISTS, or CATEGORY_DOES_NOT_EXIST</returns>
        public OutcomeCode CreateNote(string noteName, string categoryName)
        {
            if (CategoryExists(categoryName) && !NoteExists(noteName, categoryName))
            {
                Category category = GetCategory(categoryName);
                Note note = new Note();

                note.Name = noteName;
                note.Category = category;
                note.FilePath = category.FolderPath + '\\' + noteName + FILE_EXTENSION;
                note.Content = "";

                category.notesInCategory.Add(note);
                notes.Add(note);

                SaveNote(note.Name, note.Category.Name);

                return OutcomeCode.SUCCESS;
            }
            else if (NoteExists(noteName, categoryName))
            {
                return OutcomeCode.NOTE_ALREADY_EXISTS;
            }
            else
            {
                return OutcomeCode.CATEGORY_DOES_NOT_EXIST;
            }
        }



        /// <summary>
        /// Removes the Note, specified by note name and category name, from
        /// the system.
        /// </summary>
        /// <param name="noteName">Name of the Note to remove</param>
        /// <param name="categoryName">Name of the Category that the Note is in
        /// </param>
        /// <returns>Outcome of Note removal; returns: SUCCESS or 
        /// NOTE_DOES_NOT_EXIST</returns>
        public OutcomeCode RemoveNote(string noteName, string categoryName)
        {
            if (NoteExists(noteName, categoryName))
            {
                Note note = GetNote(noteName, categoryName);
                File.Delete(note.FilePath);
                notes.Remove(note);
                note.Category.notesInCategory.Remove(note);
                return OutcomeCode.SUCCESS;
            }
            else
            {
                return OutcomeCode.NOTE_DOES_NOT_EXIST;
            }
        }



        /// <summary>
        /// Returns the Note specified by Note name and Category name. Returns
        /// null if no such Note exists.
        /// </summary>
        /// <param name="noteName">Name of Note to be returned</param>
        /// <param name="categoryName">Name of the Category that the Note is in
        /// </param>
        /// <returns>The Note specified by Note name and Category name; null if
        /// note does not exist</returns>
        public Note GetNote(string noteName, string categoryName)
        {
            foreach (Note note in notes)
            {
                if (note.Name.Equals(noteName) &&
                    note.Category.Name.Equals(categoryName))
                {
                    return note;
                }
            }
            // that note was not found, return null
            return null;
        }



        /// <summary>
        /// Saves the note, specified by note name and category name, to the
        /// local project directory.
        /// </summary>
        /// <param name="noteName">Name of the note to save</param>
        /// <param name="categoryName">Name of the note's category</param>
        /// <returns>Outcome of the save process; returns: SUCCESS or
        /// NOTE_DOES_NOT_EXIST</returns>
        public OutcomeCode SaveNote(string noteName, string categoryName)
        {
            if (NoteExists(noteName, categoryName))
            {
                Note note = GetNote(noteName, categoryName);
                File.WriteAllText(note.FilePath, note.Content);
                return OutcomeCode.SUCCESS;
            }
            else
            {
                return OutcomeCode.NOTE_DOES_NOT_EXIST;
            }
        }



        /// <summary>
        /// Saves all notes to the local project directory.
        /// </summary>
        /// <returns>Outcome of the saving process; returns: SUCCESS</returns>
        public OutcomeCode SaveAllNotes()
        {
            foreach (Note note in notes)
            {
                SaveNote(note.Name, note.Category.Name);
            }
            return OutcomeCode.SUCCESS;
        }



        /// <summary>
        /// Returns a list of all Note objects for the current project
        /// </summary>
        /// <returns> a list of all Notes for the current project</returns>
        public List<Note> GetAllNotes()
        {
            return notes;
        }



        /// <summary>
        /// Returns a list of Notes that are within the category specified. If
        /// the category does not exist, null is returned.
        /// </summary>
        /// <param name="categoryName">The name of the category</param>
        /// <returns>A List of Notes that are in the specified category,
        /// null if category does not exist</returns>
        public List<Note> GetNotesFromCategory(string categoryName)
        {
            if (CategoryExists(categoryName))
            {
                return GetCategory(categoryName).notesInCategory;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
