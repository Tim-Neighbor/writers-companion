using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Ryan Overbeck
/// </summary>
namespace TheWritersCompanion
{
    /// <summary>
    /// Handles application logic and serves as an intermediary between UI and data storage.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Setup local storage by specifying a valid directory path.
        /// </summary>
        /// <param name="directory">Path of desired directory for local storage, either in part or in full.</param>
        /// <returns>Relevant outcome code.</returns>
        OutcomeCode SetDirectory(string directory, bool isNewProject);

        /// <summary>
        /// Stores credentials for remote access.
        /// </summary>
        /// <param name="username">Remote storage username.</param>
        /// <param name="password">Remote storage password.</param>
        /// <returns>Relevant outcome code.</returns>
        OutcomeCode SetCredentials(string username, string password);

        /// <summary>
        /// <see cref="DirectoryManager.GetNote(string, string)"/>
        /// </summary>
        Note GetNote(string noteName, string categoryName);

        /// <summary>
        /// <see cref="DirectoryManager.GetAllNotes"/>
        /// </summary>
        List<Note> GetAllNotes();

        /// <summary>
        /// <see cref="DirectoryManager.SaveNote(string, string)"/>
        /// </summary>
        OutcomeCode SaveNote(string noteName, string categoryName,
            string newNoteContent);

        /// <summary>
        /// <see cref="DirectoryManager.SaveAllNotes"/>
        /// </summary>
        OutcomeCode SaveAllNotes();

        /// <summary>
        /// <see cref="DirectoryManager.CreateNote(string, string)"/>
        /// </summary>
        OutcomeCode CreateNote(string noteName, string categoryName);

        /// <summary>
        /// <see cref="DirectoryManager.RemoveNote(string)"/>
        /// </summary>
        OutcomeCode RemoveNote(string noteName, string categoryName);

        /// <summary>
        /// <see cref="DirectoryManager.GetAllCategories"/>
        /// </summary>
        List<Category> GetAllCategories();

        /// <summary>
        /// <see cref="DirectoryManager.CreateCategory(string)"/>
        /// </summary>
        OutcomeCode CreateCategory(string categoryName);

        /// <summary>
        /// <see cref="DirectoryManager.RemoveCategory(string)"/>
        /// </summary>
        OutcomeCode RemoveCategory(string categoryName);

        /// <summary>
        /// <see cref="DatabaseSyncer.SyncData(string, string, string)"/>
        /// </summary>
        OutcomeCode SyncToDatabase(string directory, string username, string password);
    }
}
