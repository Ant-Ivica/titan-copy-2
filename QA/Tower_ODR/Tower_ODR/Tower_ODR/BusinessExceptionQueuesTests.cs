using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit.Sdk;

namespace Tower_ODR
{
    [TestClass]
    public class BusinessExceptionQueuesTests
    {

        [TestMethod]
        public void BusinessExceptionQueues_DefaultDateFilter()
        {
            TowerAutomationTests.TestProtractormethod("BusinessExceptionQueuesTests", "verify BEQ - Default Date");
        }
        [TestMethod]
        public void BusinessExceptionQueues_UpdateFastInfoPage()
        {
            TowerAutomationTests.TestProtractormethod("BusinessExceptionQueuesTests", "Verify BEQ - Update FAST Info Page");
        }
        [TestMethod]
        public void BusinessExceptionQueues_UpdateFastInfoUpdateAndReject()
        {
            TowerAutomationTests.TestProtractormethod("BusinessExceptionQueuesTests", "Verify BEQ - Update FAST Info UpdateAndReject");
        }
        [TestMethod]
        public void BusinessExceptionQueues_UpdateFastInfoUpdate()
        {
            TowerAutomationTests.TestProtractormethod("BusinessExceptionQueuesTests", "Verify BEQ - Update FAST Info Update");
        }
        [TestMethod]
        public void BusinessExceptionQueues_UpdateFastInfoWithRegionIdForNonLenderServices()
        {
            TowerAutomationTests.TestProtractormethod("BusinessExceptionQueuesTests", "Verify BEQ - Update FAST Info With RegionId For NonLenderServices");
        }
        [TestMethod]
        public void BusinessExceptionQueues_UpdateFASTInfo()
        {
            TowerAutomationTests.TestProtractormethod("BusinessExceptionQueuesTests", "verify BEQ - Update FAST Info");
        }
        [TestMethod]
        public void BusinessExceptionQueues_EmailingOutOfBEQPopUpNotAutoClosing()
        {
            TowerAutomationTests.TestProtractormethod("BusinessExceptionQueuesTests", "Verify BEQ - Emailing Out of BEQ Pop-Up Closing");
        }
    }
}
