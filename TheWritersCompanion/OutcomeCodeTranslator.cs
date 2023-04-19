using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWritersCompanion
{
    /// <summary>
    /// Stores user-friendly error message translations
    /// for OutcomeCodes to provide upon request.
    /// In effect, translates OutcomeCodes into
    /// nice strings.
    /// Author: Dylan Schulz
    /// </summary>
    public static class OutcomeCodeTranslator
    {
        // A dictionary to store translations from OutcomeCode to
        // user friendly error string
        private static Dictionary<OutcomeCode, string> translationDict = new Dictionary<OutcomeCode, string>
        {
            // GENERIC
            {OutcomeCode.SUCCESS,
                "The operation was successful!"},
            {OutcomeCode.FAILURE,
                "An unspecified error occurred."},

            // NOTE
            {OutcomeCode.NOTE_ALREADY_EXISTS,
                "Note already exists."},
            {OutcomeCode.NOTE_DOES_NOT_EXIST,
                "Note does not exist."},
            {OutcomeCode.INVALID_NOTE_NAME,
                "Invalid note name."},
            {OutcomeCode.UNABLE_TO_CREATE_NOTE,
                "Unable to create note."},
            {OutcomeCode.UNABLE_TO_SAVE_NOTE,
                "Unable to save note."},
            {OutcomeCode.UNABLE_TO_DELETE_NOTE,
                "Unable to delete note."},
            {OutcomeCode.NOTE_NOT_SELECTED,
                "Note not selected."},

            // CATEGORY
            {OutcomeCode.CATEGORY_ALREADY_EXISTS,
                "Category already exists."},
            {OutcomeCode.CATEGORY_DOES_NOT_EXIST,
                "Category does not exist."},
            {OutcomeCode.INVALID_CATEGORY_NAME,
                "Invalid category name."},
            {OutcomeCode.UNABLE_TO_CREATE_CATEGORY,
                "Unable to create category."},
            {OutcomeCode.UNABLE_TO_SAVE_CATEGORY,
                "Unable to save category."},
            {OutcomeCode.UNABLE_TO_DELETE_CATEGORY,
                "Unable to delete category."},
            {OutcomeCode.CATEGORY_NOT_SELECTED,
                "Category not selected."},

            // REMOTE STORAGE
            {OutcomeCode.INAVLID_USER_CREDENTIALS,
                "Invalid user credentials."},
            {OutcomeCode.UNABLE_TO_CONNECT_TO_DATABASE,
                "Unable to connect to database."},
            {OutcomeCode.UNABLE_TO_SYNC_DATABASE,
                "Unable to sync to database."},
            {OutcomeCode.UNABLE_TO_INSERT_ROW_INTO_DATABASE,
                "Unable to insert row into database."},
            {OutcomeCode.PROJECT_DOES_NOT_EXIST_IN_DATABASE,
                "Project does not exist in database."},
            {OutcomeCode.UNABLE_TO_RETRIEVE_CATEGORIES,
                "Unable to retrieve categories from database."},
            {OutcomeCode.UNABLE_TO_RETRIEVE_NOTES,
                "Unable to retrieve notes from database."},
            {OutcomeCode.UNABLE_TO_INSERT_NOTE_INTO_DATABASE,
                "Unable to insert note into database."},
            {OutcomeCode.UNABLE_TO_INSERT_CATEGORY_INTO_DATABASE,
                "Unable to insert category into database."},
            {OutcomeCode.UNABLE_TO_INSERT_PROJECT_INTO_DATABASE,
                "Unable to insert project into database."},

            // DIRECTORY
            {OutcomeCode.INVALID_FILE_ENTRY,
                "Invalid file entry string given."},
            {OutcomeCode.DIRECTORY_PATH_EMPTY_OR_INVALID_CHARACTERS,
                "Given directory path is empty or has invalid characters."},
            {OutcomeCode.DIRECTORY_PATH_INVALID_PERMISSIONS,
                "Invalid permissions to access directory path."},
            {OutcomeCode.DIRECTORY_PATH_TOO_LONG,
                "Given directory path is too long."},
            {OutcomeCode.DIRECTORY_PATH_UNSPECIFIED_ERROR,
                "Unspecified error with directory path."},
            {OutcomeCode.DIRECTORY_NOT_EMPTY,
                "Directory must be empty."},
            {OutcomeCode.DIRECTORY_DOES_NOT_EXIST,
                "The given directory does not exist."},

            // PROJECT EXISTENCE CONFLICT
            {OutcomeCode.PROJECT_DOES_NOT_EXIST,
                "That project does not exist."},
            {OutcomeCode.PROJECT_ALREADY_EXISTS,
                "That project already exists."},
            {OutcomeCode.UNABLE_TO_CREATE_PROJECT_ROOT_FOLDER,
                "Unable to create the project root folder."},

            // CONFIG FILE
            {OutcomeCode.UNABLE_TO_CREATE_PROJECT_CONFIG_FILE,
                "Unable to create the config file for the project."},

            // UI FORMATTING ERRORS
            {OutcomeCode.MULTIPLE_FONTS_SELECTED,
                "Only one font may be selected."},
            {OutcomeCode.ZOOM_FACTOR_OUT_OF_RANGE,
                "That zoom factor is too small or too large."},
            {OutcomeCode.ZOOM_FACTOR_MALFORMATTED,
                "That zoom factor does not follow the proper format."}
        };

        // String to use when a error that is not
        // in the dictionary occurs
        private const string OTHER_ERROR_MSG = "An error occurred: ";

        /// <summary>
        /// Returns a user friendly error message translation
        /// for the given OutcomeCode.
        /// If no translation is known, returns
        /// a message containing a statement plus the given OutcomeCode.
        /// </summary>
        /// <param name="outcomeCode">OutcomeCode to translate</param>
        /// <returns>User friendly error message</returns>
        public static string GetUserFriendlyErrorMessage(OutcomeCode outcomeCode)
        {
            if (translationDict.TryGetValue(outcomeCode, out string errorMessage))
            {
                return errorMessage;
            }
            else
            {
                return OTHER_ERROR_MSG + outcomeCode.ToString();
            }
        }
    }
}
