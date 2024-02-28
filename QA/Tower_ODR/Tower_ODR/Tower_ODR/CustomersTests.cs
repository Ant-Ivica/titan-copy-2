using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tower_ODR
{
    [TestClass]
    public class CustomersTests
    {

        [TestMethod]
        public void Customers_AddNewCustomer()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify Add New Customer");
        }


        [TestMethod]
        public void Customers_FiltersCustomerPage()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify filters are working in Customers Page");
        }

        [TestMethod]
        public void Customers_DeleteNewCustomer()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify Delete new customer");
        }

        [TestMethod]
        public void Customers_UpdateNewCustomer()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify Update new customer");
        }
        [TestMethod]
        public void Webhook_VerifyWebhookSubscriptionFields()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify Webhook Subscription Fields");
        }
        [TestMethod]
        public void Webhook_VerifyAddNewWebhook()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify Add New Webhook");
        }
        [TestMethod]
        public void Webhook_VerifyEditWebhook()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify Edit Webhook");
        }
        [TestMethod]
        public void Webhook_VerifyDeleteWebhook()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify Delete Webhook");
        }
        [TestMethod]
        public void Webhook_VerifyCancelNewWebhook()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify Cancel New Webhook");
        }
        [TestMethod]
        public void Webhook_VerifyCancelEditWebhook()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify Cancel Edit Webhook");
        }

        [TestMethod]
        public void OpenAPI_VerifyCreateCredential()
        {
            TowerAutomationTests.TestProtractormethod("CustomersTests", "Verify OpenApi Create Credential");
        }
    }
}
