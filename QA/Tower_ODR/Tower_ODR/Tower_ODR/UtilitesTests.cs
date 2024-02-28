using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class UtilitesTests
    {

        [TestMethod]
        public void UtilitesTests_ManageServiceRequestUpdate()
        {
            TowerAutomationTests.TestProtractormethod("UtilitesTests", "Utilities: Manage Service Request - Update");
        }

        [TestMethod]
        public void UtilitesTests_ManageServiceRequestReset()
        {
            TowerAutomationTests.TestProtractormethod("UtilitesTests", "Utilities: Manage Service Request - Reset");
        }

        [TestMethod]
        public void UtilitesTests_ConfirmServiceRequestUpdate()
        {
            TowerAutomationTests.TestProtractormethod("UtilitesTests", "Utilities: Confirm Service Request - ConfirmService");
        }

   
        [TestMethod]
        public void UtilitesTests_ConfirmServiceRequestReset()
        {
            TowerAutomationTests.TestProtractormethod("UtilitesTests", "Utilities: Confirm Service Request - Reset");
        }

        [TestMethod]
        public void UtilitesTests_EndPointAccess()
        {
            TowerAutomationTests.TestProtractormethod("UtilitesTests", "Utilities: EndPoint Access");
        }

        //[TestMethod]
        //public void UtilitesTests_ManageServiceRequest_UpdateInFast()
        //{
        //    TowerAutomationTests.TestProtractormethod("UtilitesTests", "Utilities: Manage Service Request - Update In Fast");
        //}

        [TestMethod]
        public void UtilitesTests_ManageServiceRequest_UniqueIDandExtRefNum_UpdateInFastNotInLVIS()
        {
            TowerAutomationTests.TestProtractormethod("UtilitesTests", "Utilities: Manage Service Request - UniqueID ExternalRefNum Update In FAST");
        }

        [TestMethod]
        public void UtilitesTests_ManageServiceRequest_UpdateUniqueIDInFAST()
        {
            TowerAutomationTests.TestProtractormethod("UtilitesTests", "Utilities: Manage Service Request - Update UniqueID In FAST");
        }

        [TestMethod]
        public void UtilitiesTests_ManageServiceRequest_UpdateExternalRefNumInFAST()
        {
            TowerAutomationTests.TestProtractormethod("UtilitesTests", "Utilities: Manage Service Request - Update ExternalRefNum In FAST");
        }
    }
}
