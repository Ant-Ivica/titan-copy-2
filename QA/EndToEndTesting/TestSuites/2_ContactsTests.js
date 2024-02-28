describe('Verify Customer - Locations - Contacts', function() {

    'use strict';
    var pages={};
    var testData = require('../resources/testData.json');
    pages.Home = require('../pages/Home.js');
    pages.Customers = require('../pages/Customers.js');
    pages.Locations = require('../pages/Locations.js');
    pages.Contacts = require('../pages/Contacts.js');
    var homePage = new pages.Home();
    var customerPage = new pages.Customers();
    var locationPage = new pages.Locations();
    var contactPage = new pages.Contacts();

   

    beforeEach(function(){
        homePage.openSearchPage(testData.search.homeUrl);
          homePage.isPageLoaded();
          expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
    })

    it('Verify new Contact Added', function(){
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.isLocationPageDisplayed();
        locationPage.clickOnContacts();
        contactPage.isContactPageDisplayed();
        if(testData.User.Role ==="Admin"|| testData.User.Role === "SuperAdmin")
        	{
            contactPage.clickOnAddNewContact();
            contactPage.isAddNewContactPageDisplayed();
            contactPage.enterAddNewContacts();
            }
        else if (testData.User.Role ==="User")
        {
            contactPage.isAddNewContactAvailable();
        }
        
    })

    it('Verify filters are working in Contacts', function() {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.isLocationPageDisplayed();
        locationPage.clickOnContacts();
        contactPage.isContactPageDisplayed();
        contactPage.filterByFieldName("LocationID");
        contactPage.clearFilter("LocationID");
        contactPage.filterByFieldName("LocationName");
        contactPage.clearFilter("LocationName");
        contactPage.filterByFieldName("ContactID");
        contactPage.clearFilter("ContactID");
        contactPage.filterByFieldName("Active");
        contactPage.clearFilter("Active");

    })

    it('verify filter with invalid data', function() {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.isLocationPageDisplayed();
        locationPage.clickOnContacts();
        contactPage.isContactPageDisplayed();
        contactPage.filterByInvalidData("LocationID");
    })

    it('Verify update contacts', function() {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.isLocationPageDisplayed();
        locationPage.clickOnContacts();
        contactPage.isContactPageDisplayed();
        contactPage.openNewContact();
        contactPage.updateContact();
    })
    
})