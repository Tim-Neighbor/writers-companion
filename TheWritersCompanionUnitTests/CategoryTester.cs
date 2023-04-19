using System;
using TheWritersCompanion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheWritersCompanionUnitTests
{
    /// <summary>
    /// Tester class for the DirectoryManager class
    /// Author: Tim Neighbor
    /// </summary>
    [TestClass]
    public class CategoryTester
    {
        #region Methods

        /// <summary>
        /// Tests Category constructor that takes no arguments.
        /// </summary>
        [TestMethod]
        public void TestEmptyConstructor()
        {
            Category empty = new Category();

            Assert.AreEqual(empty.Name, "");
            Assert.AreEqual(empty.FolderPath, "");
        }



        /// <summary>
        /// Tests Category constructor that takes one argument: the category 
        /// name.
        /// </summary>
        [TestMethod]
        public void TestNameContructor()
        {
            string categoryName = "characters";
            Category characters = new Category(categoryName);

            Assert.AreEqual(characters.Name, categoryName);
            Assert.AreEqual(characters.FolderPath, "");
        }



        /// <summary>
        /// Tests Category constructor that takes two arguments: the category
        /// name and the category path.
        /// </summary>
        [TestMethod]
        public void TestFullConstructor()
        {
            string categoryName = "setting";
            string currentDirectory = @"C:\Users\Tim\Desktop\Horn\Fall 2020";
            Category category = new Category(categoryName, currentDirectory);

            Assert.AreEqual(category.Name, categoryName);
            Assert.AreEqual(category.FolderPath, currentDirectory + @"\" + categoryName);
        }



        /// <summary>
        /// Tests Category property "FolderPath" after using full constructor.
        /// </summary>
        [TestMethod]
        public void TestFolderPath()
        {
            string categoryName = "setting";
            string currentDirectory = @"C:\Test Project";
            Category category = new Category(categoryName, currentDirectory);

            Assert.AreEqual(category.FolderPath, currentDirectory + @"\" + categoryName);
        }

        #endregion
    }
}
