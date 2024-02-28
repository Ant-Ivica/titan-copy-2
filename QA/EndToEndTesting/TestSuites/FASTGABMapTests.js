describe('Verify Customer - Locations - FAST GAB Mappings', function() {

    'use strict';
    var pages={};
    var testData = require('../resources/testData.json');
    pages.Home = require('../pages/Home.js');
    pages.Customers = require('../pages/Customers.js')
    pages.Locations = require('../pages/Locations.js')
    pages.FASTGABMap = require('../pages/FASTGABMap.js')
    var homePage = new pages.Home();
    var customerPage = new pages.Customers();
    var locationPage = new pages.Locations();
    var fASTGABMapPage = new pages.FASTGABMap();

    beforeEach(function(){
        homePage.openSearchPage(testData.search.homeUrl);
          homePage.isPageLoaded();
          expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
    })

    it('Verify new FAST GAB Map Added', function(){
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.clickOnFASTGABMap();
        fASTGABMapPage.isFASTGABMapPageDisplayed();
        if(testData.User.Role ==="Admin"|| testData.User.Role === "SuperAdmin")
        	{
        	fASTGABMapPage.clickOnAddNewFASTGABMap();
        	fASTGABMapPage.AddNewFASTGABMap();
		    }
	    else if (testData.User.Role ==="User")
        fASTGABMapPage.isAddNewFASTGABMapAvailable();
    });

    it('Verify filters are working in FAST GAB Mappings', function() {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.clickOnFASTGABMap();
        fASTGABMapPage.isFASTGABMapPageDisplayed();
        fASTGABMapPage.filterByFieldName("LocationID");
        fASTGABMapPage.clearFilter("LocationID");
        fASTGABMapPage.filterByFieldName("LocationName");
        fASTGABMapPage.clearFilter("LocationName");
        fASTGABMapPage.filterByFieldName("RegionID");
        fASTGABMapPage.clearFilter("RegionID");
        fASTGABMapPage.filterByFieldName("BusinessSourceABEID");
        fASTGABMapPage.clearFilter("BusinessSourceABEID");
        fASTGABMapPage.filterByFieldName("NewLenderABEID");
        fASTGABMapPage.clearFilter("NewLenderABEID");
        fASTGABMapPage.filterByFieldName("LoanType");
        fASTGABMapPage.clearFilter("LoanType");
        fASTGABMapPage.filterByFieldName("Description");
        fASTGABMapPage.clearFilter("Description");

    });

    it('Verify Update FASTGABMAP', function() {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.clickOnFASTGABMap();
        fASTGABMapPage.isFASTGABMapPageDisplayed();
        fASTGABMapPage.openNewRecord();
        fASTGABMapPage.updateFASTGABMap();
    })
    it('Verify Delete new FastGABMap', function() {
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        customerPage.clickOnLocationIcon();
        locationPage.clickOnFASTGABMap();
        fASTGABMapPage.isFASTGABMapPageDisplayed();
        fASTGABMapPage.openNewRecord();
        if(testData.User.Role ==="SuperAdmin") 
        {
            fASTGABMapPage.deleteNewRecord();
        }
        else if (testData.User.Role === "Admin"||testData.User.Role==="User")
        {
            fASTGABMapPage.isDeleteFASTGABMapDisabled();
        }
    })

        
});