using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class LVISToFASTDocumentsTests
    {

        [TestMethod]
        public void LvisToFastVerification()
        {
            TowerAutomationTests.TestProtractormethod("LVISToFASTDocumentsTests");
        }

        [TestMethod]
        public void LvisToFastAddDoc()
        {
           // TowerAutomationTests.TestProtractormethod("LVISToFASTDocumentsTests", "LVIS To Fast Documents: Add Document Test");
        }



        [TestMethod]
        public void LvisToFastEditDoc()
        {
            //TowerAutomationTests.TestProtractormethod("LVISToFASTDocumentsTests", "LVIS To Fast Documents: Edit Document Test");
        }

        [TestMethod]
        public void LvisToFastFilterOptions()
        {
            TowerAutomationTests.TestProtractormethod("LVISToFASTDocumentsTests", "Verify Filter Options on LvisToFastDoc Page");
        }

        [TestMethod]
        public void LvisToFastDeleteDoc()
        {
          //  TowerAutomationTests.TestProtractormethod("LVISToFASTDocumentsTests", "LVIS To Fast Documents: Delete Document Test");
        }



    }
}
