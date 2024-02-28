describe('Test Execution to Update User Role to Admin', function () {
    'use strict';

    var pages = {};
    pages.Home = require('../pages/Home.js');
    pages.Customers = require('../pages/Customers.js');
    pages.FastToLVISDocuments = require('../pages/FASTToLVISDocuments.js');
    pages.FilePreferences = require('../pages/FilePreferences.js');
    pages.fastOfficeMap = require('../pages/OfficeMap');


    var homePage = new pages.Home();
    var customerPage = new pages.Customers();
    var FastToLVISDocuments = new pages.FastToLVISDocuments();
    var filePreference = new pages.FilePreferences();
    var fastOfficeMap = new pages.fastOfficeMap();

    var testDataSrc = require('../resources/testData.json');

    it('FASTTOLVISDocuments: Delete FAST To LVIS Documents ', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        FastToLVISDocuments.navigateToFastToLVISDocuments();
        //FastToLVISDocuments.testDocuments();
        FastToLVISDocuments.deleteFastToLVISDocuments();
        if (testDataSrc.User.Role === 'SuperAdmin') {
            FastToLVISDocuments.verifyDeleteFastToLVISDocumentEnabled();
        }
    });

    it('Fast FilePreference: Delete File Preferernce', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        filePreference.openFastFilePreference();
        filePreference.deletFilePreference();
        if (testDataSrc.User.Role === 'SuperAdmin') {
            filePreference.verifyFilePreferenceDeleteEnabled();
        }
    });


    it('OfficeMap_Delete Office Map test', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        fastOfficeMap.navigateToOfficeMapPage();
        fastOfficeMap.isPageLoaded();
        fastOfficeMap.openExistingDocument();
        if (testDataSrc.User.Role === 'SuperAdmin') {
            fastOfficeMap.deleteOfficeMapEnabled();
        }
    });

})