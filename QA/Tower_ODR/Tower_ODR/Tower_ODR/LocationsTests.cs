using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class LocationsTests
    {

        [TestMethod]
        public void Locations_AddNewLocation()
        {
            TowerAutomationTests.TestProtractormethod("LocationsTests", "Verify Add New Location");
        }
                

        [TestMethod]
        public void Locations_FiltersLocationsPage()
        {
            TowerAutomationTests.TestProtractormethod("LocationsTests", "Verify filters are working in Locations");
        }

        [TestMethod]
        public void Locations_DeleteNewLocation()
        {
            TowerAutomationTests.TestProtractormethod("LocationsTests", "Verify Delete Location");
        }

        [TestMethod]
        public void Locations_UpdateNewLocation()
        {
            TowerAutomationTests.TestProtractormethod("LocationsTests", "Verify Location Update");
        }

              
    }
}
