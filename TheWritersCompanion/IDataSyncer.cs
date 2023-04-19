

namespace TheWritersCompanion
{
    /// <summary>
    /// Defines the public abstract interface of IDataSyncer Objects
    /// </summary>
    /// <remarks>
    /// Rudy Fisher
    /// </remarks>
    public interface IDataSyncer
    {
        /// <summary>
        /// Syncs the current project to the database for backup storage
        /// </summary>
        /// <param name="currentDirectory">The project's current directory
        ///                                as string</param>
        /// <param name="userName">The username credential to connect to the 
        ///                        database as string</param>
        /// <param name="password">The password credential to connect to the 
        ///                        database as string</param>
        /// <returns>The outcome of the importation process, i.e. whether it 
        ///          was a success or a meaningful description of the error 
        ///          that occured if it failed as OutcomeCode</returns>
        OutcomeCode SyncData(string currentDirectory, string userName, 
                             string password);
    }
}
