

namespace TheWritersCompanion
{
    /// <summary>
    /// Defines public abstract interface of IProjectImporter Objects
    /// </summary>
    /// <remarks>
    /// Rudy Fisher
    /// </remarks>
    public interface IProjectImporter
    {
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
        OutcomeCode ImportProject(string projectName, string directory, 
                                  string userName, string password);
    }
}
