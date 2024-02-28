using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class ContactsTests
    {

        [TestMethod]
        public void Contacts_AddNewContact()
        {
            TowerAutomationTests.TestProtractormethod("ContactsTests", "Verify new Contact Added");
        }                

        [TestMethod]
        public void Contacts_FiltersContactPage()
        {
            TowerAutomationTests.TestProtractormethod("ContactsTests", "Verify filters are working in Contacts");
        }

        [TestMethod]
        public void Contacts_InvalidFiltersContactPage()
        {
            TowerAutomationTests.TestProtractormethod("ContactsTests", "verify filter with invalid data");
        }

        [TestMethod]
        public void Contacts_UpdateNewContact()
        {
            TowerAutomationTests.TestProtractormethod("ContactsTests", "Verify update contacts");
        }

              
    }
}
