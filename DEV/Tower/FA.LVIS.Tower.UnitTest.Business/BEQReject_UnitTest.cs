using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.BEQRejectProcess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FA.LVIS.Tower.UnitTests.Business
{
    [TestClass]
    public class BEQReject_UnitTest
    {
        [TestMethod]
        public void UnitTest_GetDefaultNotes_NO_GOOD_MATCH_success()
        {
            string NO_GOOD_MATCH_expected = "Thank you for the order.  We are rejecting this order - for now - because our Escrow/Closing/Settlement Officer has not opened the order within our system yet.  All purchase orders must be received by our local office first from the Real Estate Agent/Broker or Attorney, before we can accept and connect your order to it.  Please contact the local Escrow/Closing/Settlement Office and confirm the order is opened. Then, you are welcome to resubmit the order.";
            BEQRejectProcess.BEQRejectProcess brp = new BEQRejectProcess.BEQRejectProcess();
            string NO_GOOD_MATCH_actual = brp.getDefaultNotes_UT(52);
            Assert.AreEqual(NO_GOOD_MATCH_expected, NO_GOOD_MATCH_actual);
        }
        [TestMethod]
        public void UnitTest_GetDefaultNotes_Mismatch_Lender_success()
        {
            string MISMATCH_LENDER_expected = "Thank you for the order.  We are rejecting this order - for now - because we do not show you as the new lender on the file.  Please contact the local Escrow/Closing/Settlement Office and confirm that the buyer has communicated to them that you are the new lender on the file.  Then, you are welcome to resubmit the order.";
            BEQRejectProcess.BEQRejectProcess brp = new BEQRejectProcess.BEQRejectProcess();
            string MISMATCH_LENDER_actual = brp.getDefaultNotes_UT(54);
            Assert.AreEqual(MISMATCH_LENDER_expected, MISMATCH_LENDER_actual);
        }


    }
}
