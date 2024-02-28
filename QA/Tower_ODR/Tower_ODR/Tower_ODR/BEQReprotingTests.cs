using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_ODR
{
    [TestClass]
    public class BEQReprotingTests
    {
        [TestMethod]
        public void BEQReprotingTests_MessageLogDisplay()
        {
            TowerAutomationTests.TestProtractormethod("BEQReprotingTests", "Verify MessageLog being displayed");

        }

        [TestMethod]
        public void BEQReprotingTests_GroupMessageLogDisplay()
        {
            TowerAutomationTests.TestProtractormethod("BEQReprotingTests", "Verify Grouped MessageLog being displayed");

        }
    }
}
