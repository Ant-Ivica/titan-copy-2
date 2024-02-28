using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class FilePreferencesTests
    {

        [TestMethod]
        public void FilePreference_AddNewFilePreference()
        {
            TowerAutomationTests.TestProtractormethod("FilePreferencesTests", "Fast FilePreference: Add File Preference Test");
        }
                

        [TestMethod]
        public void FilePreference_GirdFilterFilePreference()
        {
            TowerAutomationTests.TestProtractormethod("FilePreferencesTests", "Fast FilePreference: File Preference GridFilters Test");
        }

        [TestMethod]
        public void FilePreference_FilterFilePreference()
        {
            TowerAutomationTests.TestProtractormethod("FilePreferencesTests", "Fast FilePreference: File Preference Filter Test");
        }

        [TestMethod]
        public void FilePreference_ResetFilterTest()
        {
            TowerAutomationTests.TestProtractormethod("FilePreferencesTests", "Fast FilePreference: File Prefernce Reset Filter test");
        }

        [TestMethod]
        public void FilePreference_UpdateFilePreference()
        {
            TowerAutomationTests.TestProtractormethod("FilePreferencesTests", "Fast FilePreference: Update File Preferernce");
        }

        [TestMethod]
        public void FilePreference_DeleteFilePreference()
        {
            TowerAutomationTests.TestProtractormethod("FilePreferencesTests", "Fast FilePreference: Delete File Preferernce");
        }

        [TestMethod]
        public void FilePreference_UpdateFilePreference_COOPLien()
        {
            TowerAutomationTests.TestProtractormethod("FilePreferencesTests", "Fast FilePreference: Add/Update File Preference Test");
        }

    }
}
