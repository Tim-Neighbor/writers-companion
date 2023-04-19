using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Ryan Overbeck
/// </summary>
namespace TheWritersCompanion
{
    /// <summary>
    /// <see cref="IController"/>
    /// </summary>
    public class Controller : IController
    {
        /// <summary>
        /// Local storage.
        /// </summary>
        IStorageManager directoryManager;

        /// <summary>
        /// Remote storage.
        /// </summary>
        IDataSyncer databaseSyncer = new DatabaseSyncer();

        /// <summary>
        /// Contains the full path of the current directory (for local storage).
        /// </summary>
        private string directory;

        /// <summary>
        /// Getter for <see cref="Controller.directory"/>
        /// </summary>
        /// <returns>The current directory string.</returns>
        public string GetDirectory()
        {
            return directory;
        }

        /// <summary>
        /// <see cref="IController.SetDirectory(string)"/>
        /// </summary>
        public OutcomeCode SetDirectory(string directory, bool isNewProject)
        {
            try
            {
                if (Directory.Exists(directory))
                {
                    this.directory = Path.GetFullPath(directory);

                    directoryManager = new DirectoryManager(this.directory, isNewProject);

                    return directoryManager.OutcomeOfProjectBuild();
                }
                else
                {
                    return OutcomeCode.DIRECTORY_DOES_NOT_EXIST;
                }
            }
            catch (ArgumentException e)
            {
                return OutcomeCode.DIRECTORY_PATH_EMPTY_OR_INVALID_CHARACTERS;
            }
            catch (SecurityException e)
            {
                return OutcomeCode.DIRECTORY_PATH_INVALID_PERMISSIONS;
            }
            catch (PathTooLongException e)
            {
                return OutcomeCode.DIRECTORY_PATH_TOO_LONG;
            }
            catch (Exception e)
            {
                return OutcomeCode.DIRECTORY_PATH_UNSPECIFIED_ERROR;
            }
        }

        /// <summary>
        /// Credentials[0] = Username,
        /// Credentials[1] = Password
        /// </summary>
        private string[] credentials = new string[] { "username", "password" };

        /// <summary>
        /// Getter for <see cref="Controller.credentials"/>
        /// </summary>
        /// <returns>The current credentials array.</returns>
        public string[] GetCredentials()
        {
            return credentials;
        }

        /// <summary>
        /// <see cref="IController.SetCredentials(string, string)"/>
        /// </summary>
        public OutcomeCode SetCredentials(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                credentials[0] = username;
                credentials[1] = password;
                
                return OutcomeCode.SUCCESS;
            }
            else
            {
                return OutcomeCode.FAILURE;
            }
        }

        /// <summary>
        /// Determines validity of file system entry.
        /// </summary>
        /// <param name="fileSystemEntry">A file or folder name.</param>
        /// <returns>True if the name is legal and false otherwise.</returns>
        private bool validFileSystemEntry(string fileSystemEntry)
        {
            return !string.IsNullOrEmpty(fileSystemEntry) &&
              fileSystemEntry.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }

        /// <summary>
        /// <see cref="IController.GetNote(string, string)"/>
        /// </summary>
        public Note GetNote(string noteName, string categoryName)
        {
            if (validFileSystemEntry(noteName) && validFileSystemEntry(categoryName))
            {
                return directoryManager.GetNote(noteName, categoryName);
            } 
            else
            {
                return null; // Unable to retrieve specified note
            }
        }

        /// <summary>
        /// <see cref="IController.GetAllNotes"/>
        /// </summary>
        public List<Note> GetAllNotes()
        {
            return directoryManager.GetAllNotes();
        }

        /// <summary>
        /// <see cref="IController.SaveNote(string, string)"/>
        /// </summary>
        public OutcomeCode SaveNote(string noteName, string categoryName,
            string newNoteContent)
        {
            if (validFileSystemEntry(noteName) && validFileSystemEntry(categoryName))
            {
                Note noteToSaveTo = directoryManager.GetNote(noteName, categoryName);

                if (noteToSaveTo == null)
                {
                    return OutcomeCode.NOTE_DOES_NOT_EXIST;
                }
                else
                {
                    noteToSaveTo.Content = newNoteContent;

                    return directoryManager.SaveNote(noteName, categoryName);
                }
            }
            else
            {
                return OutcomeCode.INVALID_FILE_ENTRY;
            }
        }

        /// <summary>
        /// <see cref="IController.SaveAllNotes"/>
        /// </summary>
        public OutcomeCode SaveAllNotes()
        {
            return directoryManager.SaveAllNotes();
        }

        /// <summary>
        /// <see cref="IController.CreateNote(string, string)"/>
        /// </summary>
        public OutcomeCode CreateNote(string noteName, string categoryName)
        {
            if (validFileSystemEntry(noteName) && validFileSystemEntry(categoryName))
            {
                return directoryManager.CreateNote(noteName, categoryName);
            }
            else
            {
                if (validFileSystemEntry(categoryName)) {
                    return OutcomeCode.INVALID_NOTE_NAME;
                }
                else
                {
                    return OutcomeCode.CATEGORY_NOT_SELECTED;
                }
            }
        }

        /// <summary>
        /// <see cref="IController.RemoveNote(string, string)"/>
        /// </summary>
        public OutcomeCode RemoveNote(string noteName, string categoryName)
        {
            if (validFileSystemEntry(noteName) && validFileSystemEntry(categoryName))
            {
                return directoryManager.RemoveNote(noteName, categoryName);
            }
            else
            {
                if(noteName.Length == 0)
                {
                    return OutcomeCode.NOTE_NOT_SELECTED;
                }
                return OutcomeCode.INVALID_FILE_ENTRY;
            }
        }

        /// <summary>
        /// <see cref="IController.GetAllCategories"/>
        /// </summary>
        public List<Category> GetAllCategories()
        {
            return directoryManager.GetAllCategories();
        }

        /// <summary>
        /// <see cref="IController.CreateCategory(string)"/>
        /// </summary>
        public OutcomeCode CreateCategory(string categoryName)
        {
            if (validFileSystemEntry(categoryName))
            {
                return directoryManager.CreateCategory(categoryName);
            }
            else
            {
                return OutcomeCode.INVALID_CATEGORY_NAME;
            }
        }

        /// <summary>
        /// <see cref="IController.RemoveCategory(string)"/>
        /// </summary>
        public OutcomeCode RemoveCategory(string categoryName)
        {
            if (validFileSystemEntry(categoryName))
            {
                return directoryManager.RemoveCategory(categoryName);
            }
            else
            {
                if(categoryName.Length == 0)
                {
                    return OutcomeCode.CATEGORY_NOT_SELECTED;
                }
                return OutcomeCode.INVALID_FILE_ENTRY;
            }
        }

        /// <summary>
        /// <see cref="IController.SyncToDatabase"/>
        /// </summary>
        public OutcomeCode SyncToDatabase(string directory, string username, string password)
        {
            return databaseSyncer.SyncData(directory, credentials[0], credentials[1]);
        }
    }
}
