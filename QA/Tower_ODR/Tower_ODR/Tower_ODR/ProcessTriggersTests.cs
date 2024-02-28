using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_ODR

{   [TestClass]
    public class ProcessTriggersTests
    {
        [TestMethod]
        public void ProcessTriggersTests_AddNewProcessTrigger()
        {
            TowerAutomationTests.TestProtractormethod("ProcessTriggersTests", "Verify AddNew Process Trigger ");
        }

        [TestMethod]
        public void ProcessTriggersTests_DeleteProcessTrigger()
        {
            TowerAutomationTests.TestProtractormethod("ProcessTriggersTests", "Verify deletion of Process Trigger ");
        }


        [TestMethod]
        public void ProcessTriggersTests_GridFiltersProcessTriggers()
        {
            TowerAutomationTests.TestProtractormethod("ProcessTriggersTests", "Verify filter is working successfully");
        }


    }
}
