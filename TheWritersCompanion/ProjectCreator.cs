using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWritersCompanion
{
    /// <summary>
    /// Defines ProjectCreator objects
    /// </summary>
    /// <remarks>
    /// This is a convenience utility class used for testing
    /// Author: Rudy Fisher
    /// </remarks>
    public class ProjectCreator
    {
        private const char ESCAPE = '\\';
        private const string TXT = ".txt";
        
        /// <summary>
        /// Creates the Project's root folder
        /// </summary>
        /// <param name="projectDirectoryPath">The directory path to create</param>
        public void CreateProjectFolder(string projectDirectoryPath)
        {
            Directory.CreateDirectory(projectDirectoryPath);
        }

        /// <summary>
        /// Creates the categories to put the project's notes into
        /// </summary>
        /// <param name="projectDirectoryPath">The path of the project's root folder</param>
        /// <param name="categoryNames">The List of the names of the categories to make</param>
        /// <param name="outCatagoryPaths">The resulting full paths of the created categories</param>
        public void CreateNoteCategories(string projectDirectoryPath, List<string> categoryNames, 
                                         List<string> outCatagoryPaths)
        {
            foreach (string categoryName in categoryNames)
            {
                outCatagoryPaths.Add(projectDirectoryPath + ESCAPE + categoryName);
                Directory.CreateDirectory(projectDirectoryPath + ESCAPE + categoryName);
            }
        }

        /// <summary>
        /// Creates the notes for the specified category
        /// </summary>
        /// <param name="categoryPath">The path of the category</param>
        /// <param name="noteNames">The names of the notes to make</param>
        /// <param name="noteContents">The contents to put into the notes,
        ///             correlated by index</param>
        /// <param name="outNotePaths">The resulting paths of the made notes</param>
        public void CreateTestNotes(string categoryPath, List<string> noteNames, 
                                    List<string> noteContents, List<string> outNotePaths)
        {
            for (int i = 0; i < noteNames.Count; i++)
            {
                string path = categoryPath + ESCAPE + noteNames[i] + TXT;
                outNotePaths.Add(path);
                File.Create(path).Close();
                File.WriteAllText(path, noteContents[i]);
            }
        }

        /// <summary>
        /// Recursively deletes a project at the directory path
        /// </summary>
        /// <param name="directoryPath">The path of the directory to delete</param>
        /// <remarks>
        /// Also deletes the directory at the given directoryPath
        /// </remarks>
        public void RecursivelyDeleteProject(string directoryPath)
        {

            // Recursively delete subfolders
            string[] subDirectories = Directory.GetDirectories(directoryPath);
            foreach (string subDirectory in subDirectories)
            {
                RecursivelyDeleteProject(subDirectory);
            }

            // Delete files in current folder
            string[] files = Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                File.Delete(file);
            }

            // Delete this folder
            Directory.Delete(directoryPath);
        }
    }
}
