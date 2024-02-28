using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class FASTGABMapTests
    {

        [TestMethod]
        public void FASTGABMap_AddNewFASTGABMap()
        {
            TowerAutomationTests.TestProtractormethod("FASTGABMapTests", "Verify Add New Customer");
        }
                

        [TestMethod]
        public void FASTGABMap_FiltersFASTGABMapPage()
        {
            TowerAutomationTests.TestProtractormethod("FASTGABMapTests", "Verify filters are working in Customers Page");
        }

        [TestMethod]
        public void FASTGABMap_DeleteNewFASTGABMap()
        {
            TowerAutomationTests.TestProtractormethod("FASTGABMapTests", "Verify Delete new customer");
        }

        [TestMethod]
        public void FASTGABMap_UpdateNewFASTGABMap()
        {
            TowerAutomationTests.TestProtractormethod("FASTGABMapTests", "Verify Update new customer");
        }

              
    }
}
