describe('Test Execution on OfficeMap Page', function () {
    'use strict';
    var pages = {};
    var testData = require('../resources/testData.json');
    pages.Home = require('../pages/Home.js');
    pages.fastOfficeMap = require('../pages/OfficeMap.js');
    pages.Customers = require('../pages/Customers.js');

    var homePage = new pages.Home();
    var fastOfficeMap = new pages.fastOfficeMap();
    var customerPage = new pages.Customers();

    it('Verify Fast Office Map Profile Page is loaded Successfully', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        fastOfficeMap.navigateToOfficeMapPage();
        fastOfficeMap.isPageLoaded();
    });

    it('OfficeMap_Add New Office Map Test', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        fastOfficeMap.navigateToOfficeMapPage();
        fastOfficeMap.isPageLoaded();
        if (testData.User.Role === 'SuperAdmin' || testData.User.Role === 'Admin') {
            fastOfficeMap.addNewOfficeMap();
        }
        else if (testData.User.Role === 'User') {
            fastOfficeMap.addOfficeMapavailable();
        }
    });

    it('OfficeMap_Update Office Map test', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        fastOfficeMap.navigateToOfficeMapPage();
        fastOfficeMap.isPageLoaded();
        fastOfficeMap.openExistingDocument();
        if (testData.User.Role === 'SuperAdmin' || testData.User.Role === 'Admin') {
            fastOfficeMap.editOfficeMapEnabled();
        }
        else if (testData.User.Role === 'User') {
            fastOfficeMap.editOfficeMapDisabled();
        }
    });

    it('OfficeMap_OfficeMap Filter Test', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        fastOfficeMap.navigateToOfficeMapPage();
        fastOfficeMap.isPageLoaded();
        fastOfficeMap.stateAndCountyFiltertest();
    });

    it('OfficeMap_OfficeMap Grid Filter Test', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        fastOfficeMap.navigateToOfficeMapPage();
        fastOfficeMap.isPageLoaded();
        //  fastOfficeMap.gridFilterByFieldName("providerId", "433822");
        fastOfficeMap.gridFilterByFieldName("providerId", "OpenAPI");
        fastOfficeMap.clearFilter("providerId");
        fastOfficeMap.gridFilterByFieldName("externalId", "90001");
        fastOfficeMap.clearFilter("externalId");
        fastOfficeMap.gridFilterByFieldName("regionID", "Cali");
        fastOfficeMap.clearFilter("regionID");
        fastOfficeMap.gridFilterByFieldName("titleOfficeId", "CA");
        fastOfficeMap.clearFilter("titleOfficeId");
        fastOfficeMap.gridFilterByFieldName("escrowOfficeId", "CA");
        fastOfficeMap.clearFilter("escrowOfficeId");
        fastOfficeMap.gridFilterByFieldName("titleOfficer", "MAN");
        fastOfficeMap.clearFilter("titleOfficer");
        fastOfficeMap.gridFilterByFieldName("escrowOfficer", "MAN");
        fastOfficeMap.clearFilter("escrowOfficer");
        // fastOfficeMap.gridFilterByFieldName("location", "433822 (3333)");
        // fastOfficeMap.clearFilter("location");
        fastOfficeMap.gridFilterByFieldName("description", "FOM");
        fastOfficeMap.clearFilter("description");
        fastOfficeMap.gridFilterByFieldName("tenant", "LVIS");
        fastOfficeMap.clearFilter("tenant");
    });

    it('OfficeMap_Delete Office Map test', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        fastOfficeMap.navigateToOfficeMapPage();
        fastOfficeMap.isPageLoaded();
        fastOfficeMap.openExistingDocument();
        if (testData.User.Role === 'SuperAdmin') {
            fastOfficeMap.deleteOfficeMapEnabled();
        }
        else if (testData.User.Role === 'Admin' || testData.User.Role === 'User') {
            fastOfficeMap.deleteOfficeMapDisabled();
        }
    });
})