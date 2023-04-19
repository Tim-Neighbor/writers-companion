using System;
using TheWritersCompanion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace TheWritersCompanionUnitTests
{
    /// <summary>
    /// Tester class for the DirectoryManager class (INCOMPLETE).
    /// Author: Tim Neighbor
    /// </summary>
    [TestClass]
    public class DirectoryManagerTester
    {
        #region Fields

        IStorageManager directoryManager;

        const string PROJECT_DIRECTORY = "C:\\MockProjectTesting";

        const int NUM_INITIAL_CATEGORIES = 2;
        const int NUM_INITIAl_NOTES = 5;

        #endregion

        #region Methods

        /// <summary>
        /// Constructor that creates the directoryManager to be used in 
        /// testing.
        /// </summary>
        public DirectoryManagerTester()
        {
            directoryManager = new DirectoryManager(PROJECT_DIRECTORY, true);
        }



        /// <summary>
        /// Tests the constructor of a DirectoryManager.
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            // I think this will only work on my machine unless you create the
            // same directory

            List<Note> notes= directoryManager.GetAllNotes();
            List<Category> categories = directoryManager.GetAllCategories();

            Assert.AreEqual(NUM_INITIAl_NOTES, notes.Count);
            Assert.AreEqual(NUM_INITIAL_CATEGORIES, categories.Count);
        }



        /// <summary>
        /// Tests the Category methods of a DirectoryManager. Should be
        /// factored out into individual method tests, but I couldn't figure
        /// out a good way to do so.
        /// </summary>
        [TestMethod]
        public void TestCategoryMethods()
        {
            OutcomeCode result;
            List<Category> categories;

            string categoryName = "Example";
            // create Category named Example
            result = directoryManager.CreateCategory(categoryName);
            categories = directoryManager.GetAllCategories();

            Assert.AreEqual(OutcomeCode.SUCCESS, result);
            Assert.AreEqual(true, directoryManager.CategoryExists(categoryName));
            Assert.AreEqual(1, categories.Count);
            Assert.AreEqual(categoryName, categories[0].Name);

            // create Category named Example again
            result = directoryManager.CreateCategory(categoryName);
            categories = directoryManager.GetAllCategories();

            Assert.AreEqual(OutcomeCode.CATEGORY_ALREADY_EXISTS, result);
            Assert.AreEqual(true, directoryManager.CategoryExists(categoryName));
            Assert.AreEqual(1, categories.Count);
            Assert.AreEqual(categoryName, categories[0].Name);

            // get Category named Example
            Category exampleCategory = directoryManager.GetCategory(categoryName);

            Assert.AreNotEqual(null, exampleCategory);
            Assert.AreEqual(categoryName, exampleCategory.Name);


            // remove Category named Example
            result = directoryManager.RemoveCategory(categoryName);
            categories = directoryManager.GetAllCategories();

            Assert.AreEqual(OutcomeCode.SUCCESS, result);
            Assert.AreEqual(false, directoryManager.CategoryExists(categoryName));
            Assert.AreEqual(0, categories.Count);

            // remove Category named Example again
            result = directoryManager.RemoveCategory(categoryName);
            categories = directoryManager.GetAllCategories();

            Assert.AreEqual(OutcomeCode.CATEGORY_DOES_NOT_EXIST, result);
            Assert.AreEqual(false, directoryManager.CategoryExists(categoryName));
            Assert.AreEqual(0, categories.Count);
        }

        #endregion
    }
}
