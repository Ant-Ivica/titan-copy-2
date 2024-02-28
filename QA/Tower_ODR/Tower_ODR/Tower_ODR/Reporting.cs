using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class Reporting
    {
        [TestMethod]
        public void OrderReporting()
        {
            TowerAutomationTests.TestProtractormethod("OrderSummaryTests", "Open Tower Reporting page");
        }

        [TestMethod]
        public void OrderReporting_SearchBy()
        {
            TowerAutomationTests.TestProtractormethod("OrderSummaryTests");
        }


        [TestMethod]
        public void OrderReporting_SearchByLast30()
        {
            TowerAutomationTests.TestProtractormethod("OrderSummaryTests", "Tower Reporting Test - Search by Date Last30 days");
        }

        [TestMethod]
        public void RFOrderReporting()
        {
            TowerAutomationTests.TestProtractormethod("RFOrderSummaryTests");
        }



    }
}
