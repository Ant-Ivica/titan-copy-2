using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class TechnicalExceptionQueuesTests
    {

        [TestMethod]
        public void TEQ_ResubmitException()
        {
            TowerAutomationTests.TestProtractormethod("TechnicalExceptionQueuesTests", "Verify TEQ - Exception resubmitted");
        }
                

        [TestMethod]
        public void TEQ_MultipleResubmissions()
        {
            TowerAutomationTests.TestProtractormethod("TechnicalExceptionQueuesTests", "Verify Multiple resubmissions from TEQ");
        }

       
        [TestMethod]
        public void TEQ_SearchByDate()
        {
            TowerAutomationTests.TestProtractormethod("TechnicalExceptionQueuesTests", "Verify TEQ - Search by Date");
        }

        [TestMethod]
        public void TEQ_SearchByOptions()
        {
            TowerAutomationTests.TestProtractormethod("TechnicalExceptionQueuesTests", "Verify TEQ - Search by Options");
        }

        [TestMethod]
        public void TEQ_RejectException()
        {
            TowerAutomationTests.TestProtractormethod("TechnicalExceptionQueuesTests", "Verify TEQ - Reject");
        }

        [TestMethod]
        public void TEQ_SaveAndResumitException()
        {
            TowerAutomationTests.TestProtractormethod("TechnicalExceptionQueuesTests", "Verify TEQ - Save and Resubmit");
        }
    }
}
