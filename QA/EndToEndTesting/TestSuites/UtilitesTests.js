describe('Test Execution on - Utilites', function () {
    'use strict';

    var testDataSrc = require('../resources/testData.json');

    var pages = {};
    pages.Home = require('../pages/Home.js');
    pages.Utilites = require('../pages/Utilites');
    var homePage = new pages.Home();
    var utilites = new pages.Utilites();

    it('Utilities: Manage Service Request - Update', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        if (testDataSrc.User.Role == "SuperAdmin") {
            utilites.navigateToReportingOrderSummary();
            utilites.getOrderDetails("New", "Update");
        }
        else {
            utilites.isUtilityPresent();
        }
    });

    it('Utilities: Manage Service Request - UniqueID ExternalRefNum Update In FAST', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        if (testDataSrc.User.Role == "SuperAdmin") {
            utilites.navigateToReportingOrderSummary();
            utilites.getOrderDetails("Open", "NotToUpdateInLVIS");
        }
        else {
            utilites.isUtilityPresent();
        }
    });

    it('Utilities: Manage Service Request - Update UniqueID In FAST', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        if (testDataSrc.User.Role == "SuperAdmin") {
            utilites.navigateToReportingOrderSummary();
            utilites.getOrderDetails("Open", "UniqueIDInFAST");
        }
        else {
            utilites.isUtilityPresent();
        }
    });

    it('Utilities: Manage Service Request - Update ExternalRefNum In FAST', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        if (testDataSrc.User.Role == "SuperAdmin") {
            utilites.navigateToReportingOrderSummary();
            utilites.getOrderDetails("Open", "ExternalRefNumInFAST");
        }
        else {
            utilites.isUtilityPresent();
        }
    });


    it('Utilities: Manage Service Request - Reset', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        if (testDataSrc.User.Role == "SuperAdmin") {
            utilites.navigateToUtilitesManageSR();
            utilites.validateResetServiceRequest();
        }
        else {
            utilites.isUtilityPresent();
        }
    });
    //  The Confirm Service Request will work for following service request
    //TEQ, BEQ, Cancelled, Open In Error, Rejected, Invalidated.
    it('Utilities: Confirm Service Request - ConfirmService', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        if (testDataSrc.User.Role == "SuperAdmin") {

            utilites.navigateToReportingOrderSummary();
            utilites.getOrderDetails("In TEQ", "Confirm");
        }
        else {
            utilites.isUtilityPresent();
        }
    });

    it('Utilities: Confirm Service Request - Reset', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        if (testDataSrc.User.Role == "SuperAdmin") {
            utilites.navigateToUtilitesConfirmSR();
            utilites.getTestData();
            utilites.validateResetServiceRequestForCSR();
        }
        else {
            utilites.isUtilityPresent();
        }
    });

    it('Utilities: EndPoint Access', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        if (testDataSrc.User.Role == "SuperAdmin") {
            utilites.navigateToUtilitesEndPointAccess();
            utilites.validateAvailableApplications();
            utilites.validateEndpointRequest();
        }
        else {
            utilites.isUtilityPresent();
        }
    });
});