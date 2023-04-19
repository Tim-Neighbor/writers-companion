using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWritersCompanion
{
    /// <summary>
    /// Interface that specifies the behavior for all StorageManagers
    /// Author: Tim Neighbor
    /// </summary>
    public interface IStorageManager
    {
        #region DirectoryMethods

        /// <summary>
        /// Returns the outcome of the project build.
        /// </summary>
        /// <returns>The outcome of the project build</returns>
        OutcomeCode OutcomeOfProjectBuild();

        #endregion

        #region CategoryMethods

        /// <summary>
        /// Determines if the Category, specified by name, exists.
        /// </summary>
        /// <param name="categoryName">Name of the Category</param>
        /// <returns>True if Category exists, otherwise false</returns>
        bool CategoryExists(string categoryName);

        /// <summary>
        /// Creates a new Category object, and adds a folder to the main
        /// directory.
        /// </summary>
        /// <param name="categoryName">Name of the Category to create</param>
        /// <returns>Outcome of Category creation; returns: SUCCESS or 
        /// CATEGORY_ALREADY_EXISTS</returns>
        OutcomeCode CreateCategory(string categoryName);

        /// <summary>
        /// Removes the Category, specified by name, and all notes within that
        /// category, from the List and persistent storage.
        /// </summary>
        /// <param name="categoryName">category that is being removed along with
        /// its notes</param>
        /// <returns>Outcome of the removal process; returns: SUCCESS or
        /// CATEGORY_DOES_NOT_EXIST</returns>
        OutcomeCode RemoveCategory(string categoryName);

        /// <summary>
        /// Returns the Category specified by name. Returns null if no such
        /// Category exists
        /// </summary>
        /// <param name="categoryName">Name of the Category</param>
        /// <returns>The Category specified by name; null if category does
        /// not exist</returns>
        Category GetCategory(string categoryName);

        /// <summary>
        /// Returns a list of all categories in the current project
        /// </summary>
        /// <returns>list of all categories</returns>
        List<Category> GetAllCategories();

        #endregion

        #region NoteMethods

        /// <summary>
        /// Determines if the Note, specified by Note name and Category name,
        /// exists.
        /// </summary>
        /// <param name="noteName">Name of the Note</param>
        /// <param name="categoryName">Name of the Note's Category</param>
        /// <returns></returns>
        bool NoteExists(string noteName, string categoryName);

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
        OutcomeCode CreateNote(string noteName, string categoryName);

        /// <summary>
        /// Removes the Note, specified by note name and category name, from
        /// the system.
        /// </summary>
        /// <param name="noteName">Name of the Note to remove</param>
        /// <param name="categoryName">Name of the Category that the Note is in
        /// </param>
        /// <returns>Outcome of Note removal; returns: SUCCESS or 
        /// NOTE_DOES_NOT_EXIST</returns>
        OutcomeCode RemoveNote(string noteName, string categoryName);

        /// <summary>
        /// Returns the Note specified by Note name and Category name. Returns null if
        /// no such Note exists.
        /// </summary>
        /// <param name="noteName">Name of Note to be returned</param>
        /// <param name="categoryName">Name of the Category that the Note is in
        /// </param>
        /// <returns>The Note specified by Note name and Category name; null if note does
        /// not exist</returns>
        Note GetNote(string noteName, string categoryName);

        /// <summary>
        /// Saves the note, specified by note name and category name, to the
        /// local project directory.
        /// </summary>
        /// <param name="noteName">Name of the note to save</param>
        /// <param name="categoryName">Name of the note's category</param>
        /// <returns>Outcome of the save process; returns: SUCCESS or
        /// NOTE_DOES_NOT_EXIST</returns>
        OutcomeCode SaveNote(string noteName, string categoryName);

        /// <summary>
        /// Saves all notes to the local project directory.
        /// </summary>
        /// <returns>Outcome of the saving process; returns: SUCCESS</returns>
        OutcomeCode SaveAllNotes();

        /// <summary>
        /// Returns a list of all Note objects for the current project
        /// </summary>
        /// <returns> a list of all Notes for the current project</returns>
        List<Note> GetAllNotes();

        /// <summary>
        /// Returns a list of Notes that are within the category specified. If
        /// the category does not exist, null is returned.
        /// </summary>
        /// <param name="categoryName">The name of the category</param>
        /// <returns>A List of Notes that are in the specified category,
        /// null if category does not exist</returns>
        List<Note> GetNotesFromCategory(string categoryName);

        #endregion
    }
}
