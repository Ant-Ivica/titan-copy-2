using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class AccessRequestTests
    {

        [TestMethod]
        public void AccessRequest_AddNew()
        {
            TowerAutomationTests.TestProtractormethod("AccessRequestTests", "AccessRequest Add Access Request Test");
        }
                

        [TestMethod]
        public void AccessRequest_FilterTest()
        {
            TowerAutomationTests.TestProtractormethod("AccessRequestTests", "AccessRequest Access Request Filter Test");
        }

        [TestMethod]
        public void AccessRequest_Active()
        {
            TowerAutomationTests.TestProtractormethod("AccessRequestTests", "AccessRequest Access Request Activate Test");
        }

        [TestMethod]
        public void AccessRequest_Approve()
        {
            TowerAutomationTests.TestProtractormethod("AccessRequestTests", "AccessRequest Access Request Approve Test");
        }

        [TestMethod]
        public void AccessRequest_Decline()
        {
            TowerAutomationTests.TestProtractormethod("AccessRequestTests", "AccessRequest Access Request Decline Test");
        }

        [TestMethod]
        public void AccessRequest_Deactivate()
        {
            TowerAutomationTests.TestProtractormethod("AccessRequestTests", "AccessRequest Access Request Deactivate Test");
        }

    }
}
