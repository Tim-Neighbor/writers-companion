using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheWritersCompanion;

namespace TheWritersCompanionUnitTests
{
    [TestClass]
    public class ProjectLoaderTester
    {
        private const int NUMBER_CATEGORIES = 8;
        private const string ESCAPE = "\\";
        private const string PROJECT_NAME = "Test Project";
        private const string CATEGORY_NAME = "Test Category";
        private const string NOTE_NAME_PART = "Test Note";
        private const string TXT_EXTENSION = ".txt";

        private string testProjectDirectory =
                "C:\\" + PROJECT_NAME;

        private ProjectLoader loader;
        private ProjectCreator creator;

        private List<Category> categories;
        private List<Note> notes;

        private List<Note> expectedNotes;
        private List<Category> expectedCategories;


        private List<string> expectedCategoryNames;
        private List<string> expectedNoteNames;
        private List<string> expectedNoteContent;
        private List<string> expectedCategoryPaths;
        private List<string> noteParentPaths;

        [TestInitialize]
        public void Initialize()
        {
            expectedNotes = new List<Note>();
            expectedCategories = new List<Category>();

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


            loader = new ProjectLoader(testProjectDirectory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            creator.RecursivelyDeleteProject(testProjectDirectory);
        }

        #region Category Tests
        [TestMethod]
        public void TestLoadCategoriesNamesAreAsExpected()
        {
            categories = new List<Category>();
            loader.LoadCategories(categories);

            List<string> names = new List<string>();

            foreach (Category category in categories)
            {
                names.Add(category.Name);
            }

            names.Sort();
            expectedCategoryNames.Sort();

            for (int i = 0; i < categories.Count; i++)
            {
                Assert.AreEqual(expectedCategoryNames[i], names[i]);
            }
        }


        [TestMethod]
        public void TestLoadCategoriesPathsAreAsExpected()
        {
            categories = new List<Category>();
            loader.LoadCategories(categories);

            List<string> paths = new List<string>();

            foreach (Category category in categories)
            {
                paths.Add(category.FolderPath);
            }

            paths.Sort();
            expectedCategoryPaths.Sort();

            for (int i = 0; i < categories.Count; i++)
            {
                Assert.AreEqual(expectedCategoryPaths[i], paths[i]);
            }
        }


        [TestMethod]
        public void TestLoadCategoriesSuccess()
        {
            categories = new List<Category>();
            Assert.AreEqual(OutcomeCode.SUCCESS, loader.LoadCategories(categories));
        }
        #endregion

        #region Note Tests
        [TestMethod]
        public void TestLoadNotesNamesAreAsExpected()
        {
            categories = new List<Category>();
            loader.LoadCategories(categories);

            notes = new List<Note>();
            List<string> paths = new List<string>();

            foreach (Category category in categories)
            {
                loader.LoadNotesInCategory(category, notes);
            }

            notes.Sort();
            expectedNotes.Sort();

            for (int i = 0; i < notes.Count; i++)
            {
                Assert.AreEqual(expectedNotes[i].Name, notes[i].Name);
            }
        }


        [TestMethod]
        public void TestLoadNotesPathsAreAsExpected()
        {
            categories = new List<Category>();
            loader.LoadCategories(categories);

            notes = new List<Note>();
            List<string> paths = new List<string>();

            foreach (Category category in categories)
            {
                loader.LoadNotesInCategory(category, notes);
            }

            notes.Sort();
            expectedNotes.Sort();

            for (int i = 0; i < notes.Count; i++)
            {
                Assert.AreEqual(expectedNotes[i].FilePath, notes[i].FilePath);
            }
        }


        [TestMethod]
        public void TestLoadNotesContentAreAsExpected()
        {
            categories = new List<Category>();
            loader.LoadCategories(categories);

            notes = new List<Note>();
            List<string> paths = new List<string>();

            foreach (Category category in categories)
            {
                loader.LoadNotesInCategory(category, notes);
            }

            notes.Sort();
            expectedNotes.Sort();

            for (int i = 0; i < notes.Count; i++)
            {
                Assert.AreEqual(expectedNotes[i].Content, notes[i].Content);
            }
        }


        [TestMethod]
        public void TestLoadNotesSuccess()
        {
            categories = new List<Category>();
            loader.LoadCategories(categories);

            foreach (Category category in categories)
            {
                Assert.AreEqual(OutcomeCode.SUCCESS, loader.LoadNotesInCategory(category, new List<Note>()));
            }
        }
        #endregion


        [TestMethod]
        public void TestLoadProjectSuccess()
        {
            categories = new List<Category>();
            loader.LoadCategories(categories);

            foreach (Category category in categories)
            {
                Assert.AreEqual(OutcomeCode.SUCCESS, loader.LoadProject(new List<Category>(), new List<Note>()));
            }
        }
    }
}
