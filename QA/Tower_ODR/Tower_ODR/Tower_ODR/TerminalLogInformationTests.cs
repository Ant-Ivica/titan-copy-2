using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class TerminalLogInformationTests
    {

        [TestMethod]
        public void TerminalLogInfo_FiltersTerminalLog()
        {
            TowerAutomationTests.TestProtractormethod("TerminalLogInformationTests", "Verify filters in Terminal Log");
        }
              
    }
}
