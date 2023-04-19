using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWritersCompanion
{
    /// <summary>
    /// An interface to represent the primary
    /// methods for a UI for this project.
    /// Author: Dylan Schulz
    /// </summary>
    interface IUserInterface
    {
        /// <summary>
        /// Ask the user for a Category and an input string name
        /// to create a new Note.
        /// </summary>
        /// <returns>The Note that was created,
        /// or null if no note was created</returns>
        Note CreateNote();

        /// <summary>
        /// Saves the current content of the editor rich text box
        /// as the given note in the given category using controller.
        /// If the user can edit the note, the user is notified
        /// of saving status through the saveStatusLabel.
        /// </summary>
        /// <param name="noteName">The note name to save to</param>
        /// <param name="categoryName">The category name to save to</param>
        void SaveNote(string noteName, string categoryName);

        /// <summary>
        /// Asks the user if they want to remove the current note
        /// and does so if the user answers yes.
        /// </summary>
        /// <returns>true if a note was removed successfully,
        /// false otherwise</returns>
        bool RemoveNote();

        /// <summary>
        /// Ask the user for an input string name to create a new category.
        /// </summary>
        /// <returns>The Category that was created,
        /// or null if no category was created/</returns>
        Category CreateCategory();

        /// <summary>
        /// Asks the user if they want to remove the current category
        /// and does so if the user answers yes.
        /// All notes in the category are also deleted.
        /// </summary>
        /// <returns>true if a category was removed successfuly,
        /// false otherwise</returns>
        bool RemoveCategory();

        /// <summary>
        /// Ty Larson wrote this method. Minor modifications by Dylan Schulz.
        /// Syncs the project to the database.
        /// </summary>
        /// <param name="directory">Directory string of project</param>
        /// <param name="username">Username for database</param>
        /// <param name="password">Password for database</param>
        /// <returns>OutcomeCode for whether it succeeded or how it failed</returns>
        OutcomeCode SyncToDatabase(string directory, string username, string password);
    }
}
