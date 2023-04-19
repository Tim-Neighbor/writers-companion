using System;
using TheWritersCompanion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace TheWritersCompanionUnitTests
{
    /// <summary>
    /// Tests IDataSyncer interface
    /// </summary>
    [TestClass]
    public class DatabaseSyncerAndProjectImporterTester
    {
        private const string USERNAME = "team1";
        private const string PASSWORD = "sql112358";

        private const int NUMBER_CATEGORIES = 8;
        private const string ESCAPE = "\\";
        private const string PROJECT_NAME = "Test Project";
        private const string CATEGORY_NAME = "Test Category";
        private const string NOTE_NAME_PART = "Test Note";
        private const string TXT_EXTENSION = ".txt";

        private string testProjectDirectory = @"C:\";

        private List<Category> expectedCategories;
        private List<Note> expectedNotes;

        private IDataSyncer dataSyncer;
        private ProjectImporter importer;
        private ProjectCreator creator;

        private List<string> expectedCategoryNames;
        private List<string> expectedNoteNames;
        private List<string> expectedNoteContent;
        private List<string> expectedCategoryPaths;
        private List<string> noteParentPaths;

        [TestInitialize]
        public void Initialize()
        {
            dataSyncer = new DatabaseSyncer();
            importer = new ProjectImporter();
            creator = new ProjectCreator();

            expectedCategories = new List<Category>();
            expectedNotes = new List<Note>();

            #region Create a test project 
            expectedCategoryNames = new List<string>();
            expectedNoteNames = new List<string>();
            expectedNoteContent = new List<string>();
            expectedCategoryPaths = new List<string>();
            noteParentPaths = new List<string>();

            List<string> noteFullPaths = new List<string>();

            creator = new ProjectCreator();

            creator.CreateProjectFolder(testProjectDirectory);

            for (int i = 0; i < NUMBER_CATEGORIES; i++)
            {
                string categoryname = i + " " + CATEGORY_NAME;
                expectedCategoryNames.Add(categoryname);

                expectedCategories.Add(new Category(categoryname, testProjectDirectory));
            }

            creator.CreateNoteCategories(testProjectDirectory,
                expectedCategoryNames, expectedCategoryPaths);

            foreach (Category category in expectedCategories)
            {
                noteParentPaths.Add(category.FolderPath);
                for (int i = 0; i < NUMBER_CATEGORIES; i++)
                {
                    expectedNoteNames.Add(i + " " + NOTE_NAME_PART);
                    expectedNoteContent.Add(i +
                        " testing atlkjhdfsakjfh  \n\n\n\n ksdjfhghe \t "
                        + NOTE_NAME_PART);
                    noteFullPaths.Add(category + ESCAPE + i + " " + NOTE_NAME_PART +
                                        TXT_EXTENSION);

                    expectedNotes.Add(new Note(i + " " + NOTE_NAME_PART, category,
                        i + " testing atlkjhdfsakjfh  \n\n\n\n ksdjfhghe \t "
                        + NOTE_NAME_PART, category.FolderPath));
                }
            }

            for (int i = 0; i < noteParentPaths.Count; i++)
            {
                creator.CreateTestNotes(noteParentPaths[i], expectedNoteNames,
                    expectedNoteContent, new List<string>());
            }
            #endregion
        }

        [TestCleanup]
        public void Cleanup()
        {
            creator.RecursivelyDeleteProject(testProjectDirectory);
        }

        [TestMethod]
        public void SyncDataSuccess()
        {
            Assert.AreEqual(OutcomeCode.SUCCESS, 
                dataSyncer.SyncData(testProjectDirectory + ESCAPE + 
                    PROJECT_NAME, USERNAME, PASSWORD));
        }

        [TestMethod]
        public void ImportProjectSuccess()
        {
            Assert.AreEqual(OutcomeCode.SUCCESS, 
                importer.ImportProject(PROJECT_NAME, testProjectDirectory, 
                                        USERNAME, PASSWORD));
        }

        [TestMethod]
        public void SyncDataWrongCredentials()
        {
            Assert.AreEqual(OutcomeCode.UNABLE_TO_INSERT_PROJECT_INTO_DATABASE,
                dataSyncer.SyncData(testProjectDirectory + ESCAPE + PROJECT_NAME, 
                USERNAME + "4", PASSWORD + "h"));
        }

        [TestMethod]
        public void ImportProjectWrongCredentials()
        {
            Assert.AreEqual(OutcomeCode.FAILURE,
                importer.ImportProject(PROJECT_NAME, testProjectDirectory,
                                        USERNAME + "4", PASSWORD + "h"));
        }

        [TestMethod]
        public void SyncDataAndImportProjectDataIsAccurate()
        {
            dataSyncer.SyncData(testProjectDirectory + ESCAPE + PROJECT_NAME, 
                USERNAME, PASSWORD);

            ProjectImporter projectImporter = new ProjectImporter();
            projectImporter.ImportProject(PROJECT_NAME, testProjectDirectory, 
                USERNAME, PASSWORD);


            ProjectLoader loader = new ProjectLoader(testProjectDirectory + 
                                                ESCAPE + PROJECT_NAME);
            List<Category> categories = new List<Category>();
            List<Note> notes = new List<Note>();
            loader.LoadProject(categories, notes);

            categories.Sort();
            notes.Sort();

            expectedCategories.Sort();
            expectedNotes.Sort();

            for (int i = 0; i < categories.Count; i++)
            {
                Assert.AreEqual(expectedCategories[i], categories[i]);
            }

            for (int i = 0; i < notes.Count; i++)
            {
                Assert.AreEqual(expectedNotes[i], notes[i]);
            }
        }
    }
}
