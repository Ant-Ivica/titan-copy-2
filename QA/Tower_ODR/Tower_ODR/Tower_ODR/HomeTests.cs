using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class HomeTests
    {

        [TestMethod]
        public void HomePageVerification()
        {
            TowerAutomationTests.TestProtractormethod("1_HomeTests");
        }

        [TestMethod]
        public void HomeDashboardPageLoad()
        {
            TowerAutomationTests.TestProtractormethod("1_HomeTests", "Verify DashBoard Page is Displayed Successfully");
        }

        [TestMethod]
        public void TowerReleaseNotification()
        {
            TowerAutomationTests.TestProtractormethod("1_HomeTests", "On initial Load after deployment");
        }

        [TestMethod]
        public void HomePageTEQExceptions()
        {
            TowerAutomationTests.TestProtractormethod("1_HomeTests", "Verify Exception Values of Tiles TEQ");
        }

        [TestMethod]
        public void HomePageBEQExceptions()
        {
            TowerAutomationTests.TestProtractormethod("1_HomeTests", "Verify Exception Values of Tiles BEQ");
        }

        [TestMethod]
        public void HomePageTEQExceptionsCountAndValue()
        {
            TowerAutomationTests.TestProtractormethod("1_HomeTests", "Verify Results in TEQ Exceptions");
        }

        [TestMethod]
        public void HomePageBEQExceptionsCountAndValue()
        {
            TowerAutomationTests.TestProtractormethod("1_HomeTests", "Verify Results in BEQ Exceptions");
        }
        [TestMethod]
        public void HomePageTEQDisplayResolvedExceptions()
        {
            TowerAutomationTests.TestProtractormethod("1_HomeTests", "Verify Exception Values of Tiles TEQ Resolved");
        }

    }
}
