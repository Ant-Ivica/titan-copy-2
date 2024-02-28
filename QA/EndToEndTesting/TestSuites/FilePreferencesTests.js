describe('Test Execution on - Mapping Tables - FAST - FilePrefence', function () {
    'use strict';

    var testDataSrc = require('../resources/testData.json');

    var pages = {};
    pages.Home = require('../pages/Home.js');
    pages.FilePreferences = require('../pages/FilePreferences.js');
    pages.Customers = require('../pages/Customers.js');

    var homePage = new pages.Home();
    var filePreference = new pages.FilePreferences();
    var customerPage = new pages.Customers();
    //ar SearchPage = new pages.reportingTower();

    it('Fast FilePreference: Add File Preference Test', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        filePreference.openFastFilePreference();
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin') {
            filePreference.openAddFilePrefrence();
            filePreference.enterAddFilePreference();
        }
        else if (testDataSrc.User.Role === 'User') {
            filePreference.verifyAddPrefernceButtonAvailable();
        }
    });

    it('Fast FilePreference: Add/Update File Preference Test', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        filePreference.openFastFilePreference();
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin') {
            filePreference.UpdateFilePreference();
            
        }
        else if (testDataSrc.User.Role === 'User') {
            filePreference.verifyAddPrefernceButtonAvailable();
        }
    });


    it('Fast FilePreference: File Preference GridFilters Test', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        filePreference.openFastFilePreference();
        filePreference.addCustomerNameColumn();
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin' || testDataSrc.User.Role === 'User') {
            filePreference.gridFilterByFieldName("Name");
            filePreference.clearFilter("Name");
            filePreference.gridFilterByFieldName("ProgramType");
            filePreference.clearFilter("ProgramType");
            filePreference.gridFilterByFieldName("SearchType");
            filePreference.clearFilter("SearchType");
            filePreference.gridFilterByFieldName("LoanPurpose");
            filePreference.clearFilter("LoanPurpose");
            filePreference.gridFilterByFieldName("CustomerLocation");
            filePreference.clearFilter("CustomerLocation");
            filePreference.gridFilterByFieldName("CustomerName");
            filePreference.clearFilter("CustomerName");
            filePreference.gridFilterByFieldName("Tenant");
            filePreference.clearFilter("Tenant");
        }
    });

    it('Fast FilePreference: File Preference Filter Test', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        filePreference.openFastFilePreference();
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin' || testDataSrc.User.Role === 'User') {
            filePreference.filterByRegion();
            filePreference.filterByState();
            //filePreference.filterByCounty();
            //The Loan amount filter is not working
            filePreference.addLoanAmount();
            filePreference.validateSearchResult();
        }
    });

    it('Fast FilePreference: File Prefernce Reset Filter test', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        filePreference.openFastFilePreference();
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin' || testDataSrc.User.Role === 'User') {
            filePreference.filterByRegion();
            filePreference.filterByState();
            filePreference.addLoanAmount();
            filePreference.resetSearchResult();
        }
    });

    it('Fast FilePreference: Update File Preferernce', function () {
        homePage.openSearchPage(testDataSrc.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        filePreference.openFastFilePreference();
        filePreference.updateFilePreference();
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin') {
            filePreference.verifyFilePreferenceUpdateEnabled();
        }
        else if (testDataSrc.User.Role === 'User') {
            filePreference.verifyFilePreferenceUpdateDisabled();
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
        else if (testDataSrc.User.Role === 'Admin' || testDataSrc.User.Role === 'User') {
            filePreference.verifyFilePreferenceDeleteDisabled();
        }
    });
});
