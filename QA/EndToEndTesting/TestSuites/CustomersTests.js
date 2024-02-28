describe('Test Execution on Mapping Tables - Customers', function () {

    'use strict';
    var pages = {};
    var testData = require('../resources/testData.json');
    pages.Home = require('../pages/Home.js');
    var homePage = new pages.Home();
    pages.Customers = require('../pages/Customers.js');
    var customerPage = new pages.Customers();

    beforeEach(function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
    })

     it('Verify Add New Customer', function () {
         homePage.openSearchPage(testData.search.homeUrl);
         homePage.isPageLoaded();
         expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
         homePage.navigateToMappingTablesPage();
         customerPage.isCustomerPageLoaded();
         if (testData.User.Role === "Admin" || testData.User.Role === "SuperAdmin") {
             customerPage.clickOnAddNewCustomer();
             customerPage.enterAddNewCustomerData();
         }
         else if (testData.User.Role === "User")
             customerPage.isAddNewCustomerAvailable();

     });
      
     it('Verify filters are working in Customers Page', function () {
         homePage.navigateToMappingTablesPage();
         customerPage.isCustomerPageLoaded();
         customerPage.filterByFieldName("CustomerID");
         customerPage.clearFilter("CustomerID");
         customerPage.filterByFieldName("CustomerName");
         customerPage.clearFilter("CustomerName");
         customerPage.filterByFieldName("Category");
         customerPage.clearFilter("Category");
         customerPage.filterByFieldName("Application");
         customerPage.clearFilter("Application");
         customerPage.filterByFieldName("Tenant");
         customerPage.clearFilter("Tenant");
         customerPage.filterByInvalidData("CustomerID");
     })


     it('Verify Update new Customer', function () {
         homePage.navigateToMappingTablesPage();
         customerPage.isCustomerPageLoaded();
         customerPage.openNewRecord();
         customerPage.updateNewRecord();

     })

     it('Verify Delete new customer', function () {
         homePage.navigateToMappingTablesPage();
         customerPage.isCustomerPageLoaded();
         customerPage.openNewRecord();
         customerPage.deleteNewRecord();
     })

    it('Verify Webhook Subscription Fields', function () {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.openWebhookPage();
        customerPage.verifyWebhookSubscriptionFiledsDisplayed();
    })

    it('Verify Add New Webhook', function () {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.openWebhookPage();
        customerPage.addNewWebhook();
    })

    it('Verify Edit Webhook', function () {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.openWebhookPage();
        customerPage.editWebhook();
    })

    it('Verify Delete Webhook', function () {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.openWebhookPage();
        customerPage.deleteWebhook();
    })

    it('Verify Cancel New Webhook', function () {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.openWebhookPage();
        customerPage.cancelNewWebhook();
    })

    it('Verify Cancel Edit Webhook', function () {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.openWebhookPage();
        customerPage.cancelEditWebhook();
    })
    
    it('Verify OpenApi Create Credential', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        if (testData.User.Role === "Admin" || testData.User.Role === "SuperAdmin") {
            customerPage.clickOnAddNewCustomer();
            customerPage.openAPICustomerAddCredential();
        }
        else if (testData.User.Role === "User")
            customerPage.isAddNewCustomerAvailable();

    });

})

