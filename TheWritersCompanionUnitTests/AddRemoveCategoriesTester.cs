//Ty Larson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWritersCompanion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TheWritersCompanionUnitTests
{
    /// <summary>
    /// Ty Larson
    /// Tests the Creation and Deletion of categories
    /// </summary>
    [TestClass]
    public class AddRemoveCategoriesTester
    {
        private const int NUMBER_CATEGORIES = 8;
        private const string ESCAPE = "\\";
        private const string PROJECT_NAME = "Test Project";
        private const string CATEGORY_NAME = "Test Category";
        private const string NOTE_NAME_PART = "Test Note";
        private const string TXT_EXTENSION = ".txt";

        private string testProjectDirectory =
                @"C:\Test Project";

        private const string FILE_EXTENSION = ".txt";

        private string NAME_OF_CONFIG_FILE = "TWCconfig";

        private string CONTENT_OF_CONFIG_FILE = "DO NOT DELETE THIS FILE - " +
            "It is necessary to identify projects of The Writers Companion";

        private List<Category> expectedCategories;
        private List<Note> expectedNotes;

        private ProjectCreator creator;
        private Controller controller;

        private List<string> expectedCategoryNames;
        private List<string> expectedNoteNames;
        private List<string> expectedNoteContent;
        private List<string> expectedCategoryPaths;
        private List<string> noteParentPaths;

        /// <summary>
        /// Creates a mock project to be tested
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            creator = new ProjectCreator();

            expectedCategories = new List<Category>();
            expectedNotes = new List<Note>();

            // Create a test project 
            expectedCategoryNames = new List<string>();
            expectedNoteNames = new List<string>();
            expectedNoteContent = new List<string>();
            expectedCategoryPaths = new List<string>();
            noteParentPaths = new List<string>();

            List<string> noteFullPaths = new List<string>();

            creator = new ProjectCreator();

            creator.CreateProjectFolder(testProjectDirectory);

            string pathToConfig = testProjectDirectory + "\\" +
                NAME_OF_CONFIG_FILE + FILE_EXTENSION;

            File.WriteAllText(pathToConfig, CONTENT_OF_CONFIG_FILE);

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

            controller = new Controller();
            controller.SetDirectory(testProjectDirectory, false);
        }

        /// <summary>
        /// Deletes the mock project that was created on initialization
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            creator.RecursivelyDeleteProject(testProjectDirectory);
        }

        /// <summary>
        /// Tests adding a valid Category
        /// </summary>
        [TestMethod]
        public void AddCategorySuccess()
        {
            Assert.AreEqual(OutcomeCode.SUCCESS, controller.CreateCategory("FindThis"));

            bool found = false;

            Category findThisCategory = new Category("FindThis", testProjectDirectory);

            List<Category> categories = controller.GetAllCategories();

            foreach (Category category in categories)
            {
                if(category.Equals(findThisCategory)) {
                    found = true;
                }
            }

            Assert.AreEqual(true, found);
        }

        /// <summary>
        /// Tests adding a Category without a name
        /// </summary>
        [TestMethod]
        public void AddCategoryNoName()
        {
            Assert.AreEqual(OutcomeCode.INVALID_CATEGORY_NAME, controller.CreateCategory(""));
        }

        /// <summary>
        /// Tests Removing a Valid Category
        /// </summary>
        [TestMethod]
        public void RemoveCategorySuccess()
        {
            int i = 3;
            Assert.AreEqual(OutcomeCode.SUCCESS, controller.RemoveCategory(i + " " + CATEGORY_NAME));

            bool found = false;

            Category findThisCategory = new Category(i + " " + CATEGORY_NAME);

            List<Category> categories = controller.GetAllCategories();

            foreach (Category category in categories)
            {
                if(category.Equals(findThisCategory))
                {
                    found = true;
                }
            }

            Assert.AreEqual(false, found);
        }

        /// <summary>
        /// Tests removing a Category without selecting one
        /// </summary>
        [TestMethod]
        public void RemoveCategoryNoSelection()
        {
            Assert.AreEqual(OutcomeCode.CATEGORY_NOT_SELECTED, controller.RemoveCategory(""));
        }
    }
}
