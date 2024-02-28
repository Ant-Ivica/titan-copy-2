using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class TowerAutomationTests
    {
        [TestMethod]
        public void OrderReporting()
        {
            TestProtractormethod("OrderSummaryTests");
        }

        public void RFOrderSummaryTests()
        {
            TestProtractormethod("RFOrderSummaryTests");
        }

        [TestMethod]
        public void HomeTests()
        {
            TestProtractormethod("1_HomeTests");
        }

        [TestMethod]
        public void AuditingSummaryTests()
        {
            TestProtractormethod("AuditingSummaryTests");
        }

        [TestMethod]
        public void BEQReportingTests()
        {
            TestProtractormethod("BEQReportingTests");
        }

        [TestMethod]
        public void BusinessExceptionQueuesTest_1()
        {
            TestProtractormethod("BEQTests");
        }

        [TestMethod]
        public void BusinessExceptionQueuesTest_UpdateFastinfo()
        {
            TestProtractormethod("BusinessExceptionQueuesTests");
        }
        
     [TestMethod]
        public void CategoriesTests()
        {
            TestProtractormethod("CategoriesTests");
        }

        [TestMethod]
        public void ContactProvidersTests()
        {
            TestProtractormethod("ContactProvidersTests");
        }
        [TestMethod]
        public void ContactsTests()
        {
            TestProtractormethod("ContactsTests");
        }

        [TestMethod]
        public void CustomersTests()
        {
            TestProtractormethod("CustomersTests");
        }

        [TestMethod]
        public void FASTGABMapTests()
        {
            TestProtractormethod("FASTGABMapTests");
        }


        [TestMethod]
        public void FASTToLVISDocumentsTests()
        {
            TestProtractormethod("FASTToLVISDocumentsTests");
        }

        [TestMethod]
        public void FASTWebOrdersTests()
        {
            TestProtractormethod("FASTWebOrdersTests");
        }

        [TestMethod]
        public void FilePreferencesTests()
        {
            TestProtractormethod("FilePreferencesTests");
        }


        [TestMethod]
        public void InboundDocumentMappingTests()
        {
            TestProtractormethod("InboundDocumentMappingTests");
        }



        [TestMethod]
        public void LocationsTests()
        {
            TestProtractormethod("LocationsTests");
        }




        [TestMethod]
        public void LVISToFASTDocumentsTests()
        {
            TestProtractormethod("LVISToFASTDocumentsTests");
        }





        [TestMethod]
        public void ProcessTriggersTests()
        {
            TestProtractormethod("ProcessTriggersTests");
        }

        [TestMethod]
        public void SecurityProfilesTests()
        {
            TestProtractormethod("SecurityProfilesTests");
        }
          

        [TestMethod]
        public void FastTaskMap()
        {
            TestProtractormethod("TaskMapTests");
        }

        [TestMethod]
        public void TechnicalExceptionQueuesTests()
        {
            TestProtractormethod("TechnicalExceptionQueuesTests");
        }


        [TestMethod]
        public void TerminalLogInformationTests()
        {
            TestProtractormethod("TerminalLogInformationTests");
        }

        [TestMethod]
        public void UtilitesTests()
        {
            TestProtractormethod("UtilitesTests");
        }

        [TestMethod]
        public void Z_HelpTests()
        {
            TestProtractormethod("Z_HelpTests");
        }

        [TestMethod]
        public void TEQToDashBoard()
        {
            TestProtractormethod("TEQToDashBoard");
        }

        [TestMethod]
        public void ProvidersTests()
        {
            TestProtractormethod("ProvidersTests");
        }

        [TestMethod]
        public void OfficeMapTests()
        {
            TestProtractormethod("OfficeMapTests");
        }

        [TestMethod]
        public void AccessRequestTests()
        {
            TestProtractormethod("AccessRequestTests");
        }

        public static void TestProtractormethod(string TestSuite)
        {
            int intExitCode = 99;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C protractor chrome.conf.js --specs=TestSuites/" + TestSuite + ".js";
            startInfo.WorkingDirectory = @"C:\Test\EndToEndTesting";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            Assert.IsTrue(process.ExitCode <= 0);

        }


        public static void TestProtractormethod(string TestSuite, string Testcase)
        {
            Testcase = Regex.Replace(Testcase, " ", @"\s");

            Console.WriteLine("/C protractor chrome.conf.js --specs=TestSuites/" + TestSuite + ".js --grep=" + Testcase);

             int intExitCode = 99;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C protractor chrome.conf.js --specs=TestSuites/" + TestSuite + ".js --grep=" + Testcase;
            startInfo.WorkingDirectory = @"C:\Test\EndToEndTesting";
            process.StartInfo = startInfo;
            
            process.Start();
            process.WaitForExit();

            Assert.IsTrue(process.ExitCode <= 0);

        }
    }
}
