using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class BEQTests
    {
        [TestMethod]
        public void BEQ_HomePageToBEQ()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify Home page to BEQ screen");
        }
        [TestMethod]
        public void BEQ_BEQPage()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify BEQ Screen Loaded properly");
        }

        [TestMethod]
        public void BEQ_BEQExceptionDetails()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify the Exception details view on BEQ screen");
        }
        [TestMethod]
        public void BEQ_BEQDateFilter()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify the datefilter is getting enable when selecting custom");
        }
        [TestMethod]
        public void BEQ_Bind()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify the bind for potential match found exception");
        }
        [TestMethod]
        public void BEQ_Bind_ATC()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify the bind for multiple match bound exception for ATC tenant");
        }
        [TestMethod]
        public void BEQ_Unbind_ATC()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify UNBIND in Tower BEQ screen for ATC tenant");
        }
        [TestMethod]
        public void BEQ_BindWithInternalRefID_ATC()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify the bind functinality by searching InternalRefID for SuperAdmin user for ATC tenant");
        }
        [TestMethod]
        public void BEQ_Resubmit_ATC()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify BEQ - Resubmit With ATC tenant");
        }
        [TestMethod]
        public void BEQ_Unbind()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify validate message for Unbound function");
        }
        [TestMethod]
        public void BEQ_Reject()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify the reject function in BEQ");
        }
        
        [TestMethod]
        public void BEQ_NewOrder_ATC()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify New Order");
        }

        [TestMethod]
        public void BEQ_Delete_ATC()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify Delete Button in BEQ Page for ATC");
        }

        [TestMethod]
        public void BEQ_Cancel_ATC()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify Cancel Button in BEQ Page for ATC");
        }

        [TestMethod]
        public void BEQ_UpdateFASTInfo_UpdateAndReject_ATC()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify BEQ - UpdateAndReject Button in UpdateFAST Info for ATC");
        }

        [TestMethod]
        public void BEQ_UpdateFASTInfo_Update_ATC()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify BEQ - Update FAST Info Update Button for ATC");
        }

        [TestMethod]
        public void BEQ_BindWithInternalRefID()
        {
            TowerAutomationTests.TestProtractormethod("BEQTests", "Verify the bind functinality by searching InternalRefID for SuperAdmin user with tenant as LVIS");
        }
    }
}
