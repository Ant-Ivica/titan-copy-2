using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_ODR

{   [TestClass]
    public class FASTTaskmapsTests
    {
        [TestMethod]
        public void FASTTaskmapsTests_AddNewTaskMap()
        {
            TowerAutomationTests.TestProtractormethod("FASTTaskmapsTests", "Verify AddNew FastTaskmap ");
        }

        [TestMethod]
        public void FASTTaskmapsTests_DeleteTaskMap()
        {
            TowerAutomationTests.TestProtractormethod("FASTTaskmapsTests", "Verify Delete FastTaskmap ");
        }

        [TestMethod]
        public void FASTTaskmapsTests_UpdateTaskMap()
        {
            TowerAutomationTests.TestProtractormethod("FASTTaskmapsTests", "Verify updation of FastTaskmap ");
        }

        [TestMethod]
        public void FASTTaskmapsTests_GridFiltersTaskMap()
        {
            TowerAutomationTests.TestProtractormethod("FASTTaskmapsTests", "Verify filter is working successfully");
        }



    }
}
