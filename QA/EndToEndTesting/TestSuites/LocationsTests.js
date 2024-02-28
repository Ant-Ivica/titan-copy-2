describe('Verify Customer - Locations', function() {

    'use strict';
    var pages={};
    var testData = require('../resources/testData.json');
    pages.Home = require('../pages/Home.js');
    pages.Customers = require('../pages/Customers.js')
    pages.Locations = require('../pages/Locations.js')
    var homePage = new pages.Home();
    var customerPage = new pages.Customers();
    var locationPage = new pages.Locations();

    beforeEach(function(){
        homePage.openSearchPage(testData.search.homeUrl);
          homePage.isPageLoaded();
          expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
    })

    it('Verify Add New Location', function(){
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.isLocationPageDisplayed();
        if(testData.User.Role==="SuperAdmin"||testData.User.Role==="Admin")
        {
            locationPage.clickOnAddNewLocation();
            locationPage.isAddNewLocationPageDisplayed();
            locationPage.enterAddNewLocationData();
        }
        else if(testData.User.Role==="User")
        {
            locationPage.isAddNewLocationDisplayed();
        }
        
    })

    it('Verify filters are working in Locations', function(){
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.isLocationPageDisplayed();
        locationPage.filterByFieldName("CustomerName");
        locationPage.clearFilter("CustomerName");
        locationPage.filterByFieldName("ExternalID");
        locationPage.clearFilter("ExternalID");
        locationPage.filterByFieldName("LocationName");
        locationPage.clearFilter("LocationName");
    })

    it('Verify Location Update', function() {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.isLocationPageDisplayed();
        locationPage.filterByFieldName("CustomerName");
        locationPage.updateLocationData();
        
    })

    it('Verify Delete Location', function() {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.isLocationPageDisplayed();
        locationPage.filterByFieldName("CustomerName");
        locationPage.openNewLocation();
        locationPage.deleteNewLocation();

    })

    
    
})