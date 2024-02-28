using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class SecurityProfilesTests
    {

        [TestMethod]
        public void SecurityProfilesVerification()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests");
        }

        [TestMethod]
        public void SecurityProfileLoad()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests", "Verify Security Profile Page is loaded Successfully");
        }

        

        [TestMethod]
        public void SecurityProfilesTestsControlsAsSuperAdmin()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests", "Verify Update Controls of SuperAdminUser");
        }

        [TestMethod]
        public void SecurityProfilesTestsControlsAsAdmin()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests", "Verify Update Controls of AdminUser");
        }

        [TestMethod]
        public void SecurityProfilesTestsControlsAsUser()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests", "Verify Update Controls of User");
        }

        [TestMethod]
        public void SecurityProfilesUserRightFunc()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests", "Verify User Right Functionality");
        }

        [TestMethod]
        public void SecurityProfilesAddNewUser_INTL_Corp()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests", "Verify Add New User Functionality_Super Admin Only");
        }

        [TestMethod]
        public void SecurityProfilesFilters()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests", "Verify Filter Options on Security Page");
        }

        [TestMethod]
        public void SecurityProfilesActivateAndInactiveUser()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests", "Verify Activate and Inactivate Functionality");
        }

        [TestMethod]
      
        public void SecurityProfilesTenancySwitch()
        {
            TowerAutomationTests.TestProtractormethod("SecurityProfilesTests", "Verify User can switch between Tenancy");
        }
    }
}
