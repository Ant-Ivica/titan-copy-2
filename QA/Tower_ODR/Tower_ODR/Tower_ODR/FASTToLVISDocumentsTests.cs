using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class FASTToLVISDocumentsTests
    {

        [TestMethod]
        public void FASTToLVISDocuments_AddNewFASTToLVISDocuments()
        {
            TowerAutomationTests.TestProtractormethod("FASTToLVISDocumentsTests", "FASTTOLVISDocuments: Add New FAST To LVIS Documents");
        }
                

        [TestMethod]
        public void FASTToLVISDocuments_GirdFilterFASTToLVISDocuments()
        {
            TowerAutomationTests.TestProtractormethod("FASTToLVISDocumentsTests", "FASTTOLVISDocuments: Filter Test");
        }

        [TestMethod]
        public void FASTToLVISDocuments_UpdateFastToLVISDocuments()
        {
            TowerAutomationTests.TestProtractormethod("FASTToLVISDocumentsTests", "FASTTOLVISDocuments: Update FAST To LVIS Documents");
        }

   
        [TestMethod]
        public void FASTToLVISDocuments_DeleteFastToLVISDocuments()
        {
            TowerAutomationTests.TestProtractormethod("FASTToLVISDocumentsTests", "FASTTOLVISDocuments: Delete FAST To LVIS Documents");
        }

    }
}
