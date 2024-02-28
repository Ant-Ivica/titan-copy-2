describe('Test Execution on FAST TO LVIS Document Mapping', function () {

    'use strict';
    var pages = {};
    pages.Home = require('../pages/Home.js');
    pages.Customers = require('../pages/Customers.js');
    pages.FastToLVISDocuments = require('../pages/FASTToLVISDocuments.js');

    var testDataSrc = require('../resources/testData.json');
    var homePage = new pages.Home();
    var customerPage = new pages.Customers();
    var FastToLVISDocuments = new pages.FastToLVISDocuments();


    it('FASTTOLVISDocuments: Add New FAST To LVIS Documents ', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        FastToLVISDocuments.navigateToFastToLVISDocuments();
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin') {
            FastToLVISDocuments.addFastToLVISDocument();
        }
        else if (testDataSrc.User.Role === 'User') {
            FastToLVISDocuments.verifyAddFastToLVISDocumentPresent();
        }
    });

    it('FASTTOLVISDocuments: Filter Test ', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin' || testDataSrc.User.Role === 'User') {
            FastToLVISDocuments.navigateToFastToLVISDocuments();
            FastToLVISDocuments.gridFilterByFieldName("fastDocType");
            FastToLVISDocuments.clearFilter("fastDocType");
            FastToLVISDocuments.gridFilterByFieldName("fastDocDesc");
            FastToLVISDocuments.clearFilter("fastDocDesc");
            FastToLVISDocuments.gridFilterByFieldName("Service");
            FastToLVISDocuments.clearFilter("Service");
            FastToLVISDocuments.gridFilterByFieldName("lvisDocType");
            FastToLVISDocuments.clearFilter("lvisDocType");
            FastToLVISDocuments.gridFilterByFieldName("tenant");
            FastToLVISDocuments.clearFilter("tenant");
        }
    });

    it('FASTTOLVISDocuments: Update FAST To LVIS Documents ', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        FastToLVISDocuments.navigateToFastToLVISDocuments();
        FastToLVISDocuments.updateFastToLVISDocuments();
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin') {
            FastToLVISDocuments.verifyUpdateFastToLVISDocumentEnabled();
        }
        else if (testDataSrc.User.Role === 'User') {
            FastToLVISDocuments.verifyUpdateFastToLVISDocumentDisabled()

        }
    });

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
        else if (testDataSrc.User.Role === 'Admin' || testDataSrc.User.Role === 'User') {
            FastToLVISDocuments.verifyDeleteFastToLVISDocumentDisbaled();
        }
    });
});
