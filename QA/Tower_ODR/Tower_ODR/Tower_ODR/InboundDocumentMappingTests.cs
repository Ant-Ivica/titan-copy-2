using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class InboundDocumentMappingTests
    {

        [TestMethod]
        public void InboundDocumentMappingTestsLoadIsSuccessfull()
        {
            TowerAutomationTests.TestProtractormethod("InboundDocumentMappingTests", "Verify InboundDocumentMapping Screen Loaded properly");
        }

        [TestMethod]
        public void InboundDocumentMappingAddDeleteDocMapping()
        {
            TowerAutomationTests.TestProtractormethod("InboundDocumentMappingTests", "Verify Add and delete inbound document mapping");
        }


    }
}
