using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class CategoriesTests
    {

        [TestMethod]
        public void Categories_Filters()
        {
            TowerAutomationTests.TestProtractormethod("CategoriesTests", "Verify filter is working successfully");
        }

        [TestMethod]
        public void Categories_AddDeleteCategory()
        {
            TowerAutomationTests.TestProtractormethod("CategoriesTests", "Verify AddNewCateogory and delete");
        }

        [TestMethod]
        public void Categories_AddDeleteOutboundDocument()
        {
            TowerAutomationTests.TestProtractormethod("CategoriesTests", "Verify Addand deleteoutbounddocument");
        }

        [TestMethod]
        public void Categories_AddDeleteSubscriptions()
        {
            TowerAutomationTests.TestProtractormethod("CategoriesTests", "Verify Add and delete subscriptions");
        }
        [TestMethod]
        public void Categories_MessageTypeNameFilter()
        {
            TowerAutomationTests.TestProtractormethod("CategoriesTests", "VVerify MessageTypeNameTextBox filter");
        }
        [TestMethod]
        public void Categories_MessageTypeDecriptionFilter()
        {
            TowerAutomationTests.TestProtractormethod("CategoriesTests", "Verify MessageTypeDescriptionTextBox filter");
        }
        [TestMethod]
        public void Categories_TenantTextboxFilter()
        {
            TowerAutomationTests.TestProtractormethod("CategoriesTests", "Verify TenantTextBox filter");
        }

    }
}
