using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class FASTWebOrdersTests
    {
        [TestMethod]
        public void FastWeb_PageValidationAndAddOffice()
        {
            TowerAutomationTests.TestProtractormethod("FASTWebOrdersTests", "Add Forward To Office popup Verification");
        }

        [TestMethod]
        public void FastWeb_ForwardToListVerification()
        {
            TowerAutomationTests.TestProtractormethod("FASTWebOrdersTests", "Fast Web ForwardToOffice verification");
        }

        [TestMethod]
        public void FastWeb_popupverification()
        {
            TowerAutomationTests.TestProtractormethod("FASTWebOrdersTests", "Fast Web Order Details popup Verification");
        }

        [TestMethod]
        public void FastWeb_BorrowerNameVerification()
        {
            TowerAutomationTests.TestProtractormethod("FASTWebOrdersTests", "Search by BorrowerName Verification");
        }

        [TestMethod]
        public void FastWeb_FastWebOrderNumberVerification()
        {
            TowerAutomationTests.TestProtractormethod("FASTWebOrdersTests", "Search by FastWebOrder Number Verification");
        }

        [TestMethod]
        public void FastWeb_PropertyAddressVerification()
        {
            TowerAutomationTests.TestProtractormethod("FASTWebOrdersTests", "Search by Property Address Verification");
        }

        [TestMethod]
        public void FastWeb_ReloadButtonInFASTWebPage()
        {
            TowerAutomationTests.TestProtractormethod("FASTWebOrdersTests", "Verification of Reload Button");
        }

        [TestMethod]
        public void FastWeb_NoNullValueInForwardToOffice()
        {
            TowerAutomationTests.TestProtractormethod("FASTWebOrdersTests", "Fast Web ForwardToOffice verification");
        }
    }
}
