using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class OfficeMapTests
    {

        [TestMethod]
        public void OfficeMap_AddNewOfficeMapTest()
        {
            TowerAutomationTests.TestProtractormethod("OfficeMapTests", "OfficeMap_Add New Office Map Test");
        }

        [TestMethod]
        public void OfficeMap_UpdateOfficeMapTest()
        {
            TowerAutomationTests.TestProtractormethod("OfficeMapTests", "OfficeMap_Update Office Map test");
        }

        [TestMethod]
        public void OfficeMap_FilterOfficeMapTest()
        {
            TowerAutomationTests.TestProtractormethod("OfficeMapTests", "OfficeMap_OfficeMap Filter Test");
        }

        [TestMethod]
        public void OfficeMap_GridFilterOfficeMapTest()
        {
            TowerAutomationTests.TestProtractormethod("OfficeMapTests", "OfficeMap_OfficeMap Grid Filter Test");
        }

        [TestMethod]
        public void OfficeMap_DeleteOfficeMapTest()
        {
            TowerAutomationTests.TestProtractormethod("OfficeMapTests", "OfficeMap_Delete Office Map test");
        }

    }
}
