describe('Verify Customer - ContactProviders', function() {

    'use strict';

    var pages={};
    var testData = require('../resources/testData.json');
    pages.Home = require('../pages/Home.js');
    pages.Customers = require('../pages/Customers.js');
    pages.ContactProviders = require('../pages/ContactProviders.js');
    var homePage = new pages.Home();
    var customerPage = new pages.Customers();
    var contactProvidersPage = new pages.ContactProviders();

    beforeEach(function(){
        homePage.openSearchPage(testData.search.homeUrl);
          homePage.isPageLoaded();
          expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
    })

    it('Verify new Contact Provider Added', function(){
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnContactProvidersIcon();
        contactProvidersPage.isContactProvidersPageDisplayed();
        if(testData.User.Role ==="Admin"|| testData.User.Role === "SuperAdmin")
        	{
        contactProvidersPage.clickOnAddNewContactProvider();
        contactProvidersPage.isAddNewContactProviderPageDisplayed();
        contactProvidersPage.enterAddNewContactProvider();
            }
        else if (testData.User.Role ==="User") 
            {
            contactProvidersPage.isAddNewContactProviderAvailable(); 
            }

    })

    // it('Verify filters in Contact Providers', function() {
    //     homePage.navigateToMappingTablesPage();
    //     customerPage.isCustomerPageLoaded();
    //     customerPage.clickOnContactProvidersIcon();
    //     contactProvidersPage.isContactProvidersPageDisplayed();
    //     contactProvidersPage.filterByFieldName("ProviderID");
    //     contactProvidersPage.clearFilter("ProviderID");
    //     contactProvidersPage.filterByFieldName("CustomerName");
    //     contactProvidersPage.clearFilter("CustomerName");
    //     contactProvidersPage.filterByFieldName("LocationName");
    //     contactProvidersPage.clearFilter("LocationName");
    //     contactProvidersPage.filterByFieldName("ContactID");
    //     contactProvidersPage.clearFilter("ContactID");
    //     contactProvidersPage.filterByFieldName("Tenant");
    //     contactProvidersPage.clearFilter("Tenant");

    // })

    // it('Verify update for Contact Providers', function() {
    //     homePage.navigateToMappingTablesPage();
    //     customerPage.isCustomerPageLoaded();
    //     customerPage.clickOnContactProvidersIcon();
    //     contactProvidersPage.isContactProvidersPageDisplayed();
    //     contactProvidersPage.openNewContactProvider();
    //     contactProvidersPage.updateContactProvider();
    // })
    
})