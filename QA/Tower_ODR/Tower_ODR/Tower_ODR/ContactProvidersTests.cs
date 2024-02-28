using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class ContactProvidersTests
    {

        [TestMethod]
        public void ContactProviders_AddNewContactProvider()
        {
            TowerAutomationTests.TestProtractormethod("ContactProvidersTests", "Verify new Contact Provider Added");
        }
                

        [TestMethod]
        public void ContactProviders_FiltersContactProvider()
        {
            TowerAutomationTests.TestProtractormethod("ContactProvidersTests", "Verify filters in Contact Providers");
        }

       
        [TestMethod]
        public void ContactProviders_UpdateContactProvider()
        {
            TowerAutomationTests.TestProtractormethod("ContactProvidersTests", "Verify update for Contact Providers");
        }

              
    }
}
