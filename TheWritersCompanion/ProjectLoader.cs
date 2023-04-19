using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWritersCompanion
{
    /// <summary>
    /// Defines ProjectLoader objects
    /// </summary>
    /// <remarks>
    /// Rudy Fisher
    /// </remarks>
    public class ProjectLoader : IProjectLoader
    {
        private const char ESCAPE = '\\';
        private const string TXT_EXTENSION = ".txt";
        private const string EMPTY_STRING = "";
        public string ProjectDirectory { set; get; }

        /// <summary>
        /// Creates ProjectLoader objects with specified project directory
        /// </summary>
        /// <param name="projectDirectory">The project's directory
        ///                                 as string</param>
        public ProjectLoader(string projectDirectory = "")
        {
            ProjectDirectory = projectDirectory;
        }

        /// <summary>
        /// Loads the project's categories
        /// </summary>
        /// <param name="categories">The list of categories to populate
        ///                          as List of Category objects</param>
        /// <returns>The outcome of loading the categories, whether successfull
        ///          or the specific failure that occured</returns>
        public OutcomeCode LoadCategories(List<Category> categories)
        {
            OutcomeCode outcome = OutcomeCode.SUCCESS;
            string[] categoryPaths = Directory.GetDirectories(ProjectDirectory);

            foreach (string path in categoryPaths)
            {
                string namePart = path.Substring(path.LastIndexOf(ESCAPE) + 1);

                categories.Add(new Category(namePart, path.Replace(ESCAPE + namePart, EMPTY_STRING)));
            }

            return outcome;
        }

        /// <summary>
        /// Loads the notes within the given category directory
        /// </summary>
        /// <param name="category">The category</param>
        /// <param name="notesInCategory">The list of notes to populate
        /// <returns>The outcome of loading the notes, whether successfull
        ///          or the specific failure that occured</returns>
        public OutcomeCode LoadNotesInCategory(Category category,
                                                List<Note> notesInCategory)
        {
            OutcomeCode outcome = OutcomeCode.SUCCESS;
            string[] notePaths = Directory.GetFiles(category.FolderPath);

            foreach (string path in notePaths)
            {
                string noteNamePart = Path.GetFileNameWithoutExtension(path);

                string rtfContents = File.ReadAllText(path);

                if (rtfContents == null)
                {
                    rtfContents = string.Empty;
                }

                notesInCategory.Add(new Note(noteNamePart, category, rtfContents, 
                    path.Replace(ESCAPE + noteNamePart + TXT_EXTENSION, 
                                EMPTY_STRING)));
            }

            return outcome;
        }

        /// <summary>
        /// Loads the entire project at the given projectDirectory
        /// </summary>
        /// <param name="categories">The list of categories to populate
        ///                          as List of Category objects</param>
        /// <param name="notes">The list of notes to populate
        ///                     as List of Note objects</param>
        /// <returns>The outcome of loading the project, whether successfull
        ///          or the specific failure that occured</returns>
        public OutcomeCode LoadProject(List<Category> categories, List<Note> notes)
        {
            OutcomeCode outcome = LoadCategories(categories);

            int i = 0;
            while (outcome == OutcomeCode.SUCCESS &&
                    i < categories.Count)
            {
                outcome = LoadNotesInCategory(categories[i], notes);
                i++;
            }

            return outcome;
        }
    }
}
