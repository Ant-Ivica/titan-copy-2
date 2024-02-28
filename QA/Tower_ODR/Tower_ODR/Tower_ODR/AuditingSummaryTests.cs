using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class AuditingSummaryTests
    {

        [TestMethod]
        public void AuditingSummary_ValidateFilterTest()
        {
            TowerAutomationTests.TestProtractormethod("AuditingSummaryTests", "Auditing: Validate the Filters");
        }

        [TestMethod]
        public void AuditingSummary_ValidGridFilters()
        {
            TowerAutomationTests.TestProtractormethod("AuditingSummaryTests", "Auditing: Validate Gird Filters");
        }

        [TestMethod]
        public void AuditingSummary_ValidateGridData()
        {
            TowerAutomationTests.TestProtractormethod("AuditingSummaryTests", "Auditing: Validate Gird Data");
        }
    }
}
