using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParatureAPI;
using ParatureAPI.ParaObjects;
using Download = ParatureAPI.ParaObjects.Download;

namespace ParatureAPITests
{
    [TestClass]
    public class DownloadTests
    {
        /// <summary>
        /// Schema should indicate that only one folder is allowed
        /// </summary>
        [TestMethod]
        public void DownloadXmlParserSingleFolder()
        {
            var creds = TestCredentials.Credentials;
            creds.Departmentid = 45021;

            var download = ParatureAPI.ApiHandler.Download.DownloadSchema(creds);

            bool FlagSetCorrectly = !download.MultipleFolders;

            Assert.IsTrue(FlagSetCorrectly);
        }

        /// <summary>
        /// Confirm that XML parser correctly sets the flag and that this file has multiple folders
        /// </summary>
        [TestMethod]
        public void DownloadXmlParserMultipleFolders()
        {
            var creds = TestCredentials.Credentials;
            creds.Departmentid = 45001;

            var Download = ParatureAPI.ApiHandler.Download.DownloadGetDetails(1, creds);

            bool FlagSetCorrectly = Download.MultipleFolders;

            Assert.IsTrue(FlagSetCorrectly && (Download.Folders.Count > 1));
        }

        /// <summary>
        /// Tests that adding multiple folders to a Download file that does not allow multiple folders throws the correct exception
        /// </summary>
        [TestMethod]
        public void TestFoldersException()
        {
            var download = new Download(false);
            download.Folders.Add(new DownloadFolder());
            download.Folders.Add(new DownloadFolder());

            bool caughtException = false;

            try
            {
                ParatureAPI.ApiHandler.Download.DownloadInsert(download, TestCredentials.Credentials);
            }
            catch (ArgumentOutOfRangeException e)
            {
                caughtException = true;
            }

            Assert.IsTrue(caughtException);
        }

    }
}
