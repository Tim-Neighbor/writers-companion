using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWritersCompanion
{
    /// <summary>
    /// Defines abstract functionality of a IProjectLoader
    /// </summary>
    /// <remarks>
    /// Rudy Fisher
    /// </remarks>
    public interface IProjectLoader
    {
        /// <summary>
        /// Loads the entire project at the given projectDirectory
        /// </summary>
        /// <param name="categories">The list of categories to populate
        ///                          as List of Category objects</param>
        /// <param name="notes">The list of notes to populate
        ///                     as List of Note objects</param>
        /// <returns>The outcome of loading the project, whether successfull
        ///          or the specific failure that occured</returns>
        OutcomeCode LoadProject(List<Category> categories, List<Note> notes);

        /// <summary>
        /// Loads the project's categories
        /// </summary>
        /// <param name="categories">The list of categories to populate
        ///                          as List of Category objects</param>
        /// <returns>The outcome of loading the categories, whether successfull
        ///          or the specific failure that occured</returns>
        OutcomeCode LoadCategories(List<Category> categories);

        /// <summary>
        /// Loads the notes within the given category directory
        /// </summary>
        /// <param name="category">The category</param>
        /// <param name="notesInCategory">The list of notes to populate
        /// <returns>The outcome of loading the notes, whether successfull
        ///          or the specific failure that occured</returns>
        OutcomeCode LoadNotesInCategory(Category category, 
                                        List<Note> notesInCategory);
    }
}
