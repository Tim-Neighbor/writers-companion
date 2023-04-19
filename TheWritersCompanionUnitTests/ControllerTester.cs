using System;
using TheWritersCompanion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Ryan Overbeck
/// </summary>
namespace TheWritersCompanionUnitTests
{
    /// <summary>
    /// Handles all the testing for the Controller classes.
    /// </summary>
    [TestClass]
    public class ControllerTester
    {
        /// <summary>
        /// Controller instance shared by test methods.
        /// </summary>
        Controller controller = new Controller();

        /// <summary>
        /// Tests <see cref="IController.SetDirectory(string)"/>
        /// </summary>
        [TestMethod]
        public void TestSetDirectory()
        {
            // Valid
            string validTestPath = System.IO.Directory.GetCurrentDirectory();

            Assert.AreEqual(controller.SetDirectory(validTestPath), OutcomeCode.SUCCESS);
            Assert.AreEqual(controller.GetDirectory(), validTestPath);

            // Invalid
            string nullPath = null;

            Assert.AreEqual(controller.SetDirectory(nullPath), OutcomeCode.DIRECTORY_PATH_EMPTY_OR_INVALID_CHARACTERS);
            Assert.AreNotEqual(controller.GetDirectory(), nullPath);

            string emptyPath = @"";

            Assert.AreEqual(controller.SetDirectory(emptyPath), OutcomeCode.DIRECTORY_PATH_EMPTY_OR_INVALID_CHARACTERS);
            Assert.AreNotEqual(controller.GetDirectory(), emptyPath);

            string invalidPathCharacters = @"C:\Temp\?*?";

            Assert.AreEqual(controller.SetDirectory(invalidPathCharacters), OutcomeCode.DIRECTORY_PATH_EMPTY_OR_INVALID_CHARACTERS);
            Assert.AreNotEqual(controller.GetDirectory(), invalidPathCharacters);

            string pathTooLong = @"C:\wita6lo33awl3ob666udx5kr8l549yti1lv7fnrj6aslkc2s0vnlg4xcvrq2g0fmds2ey8sudhrg62r8wfgfxrjshpblklpyhy0rk7ugmhnmw3jdkv6ky8tenbjh9vfk02m9cbmurefl6wtlbxbyxpyxbq8rpc4g0d4eu6mx46az01p7646nmw67l1v6628a4c9ylpk1zh67n0ribw5aj1avjc76gckowabvy3se86zi8u8jqi76zn0mrofvwb9xqyg760fiwy0vitxjxismdyg7x0kxf32wdk999kvx6hvc";

            Assert.AreEqual(controller.SetDirectory(pathTooLong), OutcomeCode.DIRECTORY_PATH_TOO_LONG);
            Assert.AreNotEqual(controller.GetDirectory(), pathTooLong);

            string badPath = @"C:::::::::::::::::::::::\";

            Assert.AreEqual(controller.SetDirectory(badPath), OutcomeCode.DIRECTORY_PATH_UNSPECIFIED_ERROR);
            Assert.AreNotEqual(controller.GetDirectory(), badPath);
        }

        /// <summary>
        /// Tests <see cref="IController.SetCredentials(string, string)"/>
        /// </summary>
        [TestMethod]
        public void TestSetCredentials()
        {
            // Valid
            string[] validCredentials = new string[] { "smithj25", "Pass1234" }; 

            Assert.AreEqual(controller.SetCredentials(validCredentials[0], validCredentials[1]), OutcomeCode.SUCCESS);
            Assert.AreEqual(controller.GetCredentials()[0], validCredentials[0]); // username
            Assert.AreEqual(controller.GetCredentials()[1], validCredentials[1]); // password

            // Invalid
            //string[] emptyUsername = new 
        }
    
    }
}
