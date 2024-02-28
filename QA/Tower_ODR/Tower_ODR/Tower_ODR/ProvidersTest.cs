using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_ODR
{
    [TestClass]
    public class ProvidersTest
    {
        [TestMethod]
        public void ProvidersTest_Filters()
        {
            TowerAutomationTests.TestProtractormethod("ProvidersTest", "Verify filter is working successfully");
        }

        [TestMethod]
        public void ProvidersTest_AddNewProvider()
        {
            TowerAutomationTests.TestProtractormethod("ProvidersTest", "Verify Add new Provider");
        }

        [TestMethod]
        public void ProvidersTest_ServiceProviderIDTitlePort()
        {
            TowerAutomationTests.TestProtractormethod("ProvidersTest", "Verify if ServiceProviderID is visible For only TitlePort ");
        }
        [TestMethod]
        public void ProvidersTest_FASTOfficeNavigation()
        {
            TowerAutomationTests.TestProtractormethod("ProvidersTest", "Verify navigation to FASTOfficeMap from Provider screen");
        }
        
    }
}
