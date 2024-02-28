describe('Test Execution on FASTWebOrders Page', function () {
    'use strict';
    var pages = {};
    pages.Home = require('../pages/Home.js');
    var homePage = new pages.Home();
    var testData = require('../resources/testData.json');
    var fASTWebOrders = require('../pages/FASTWebOrders.js');
    var fASTWebOrdersPage = new fASTWebOrders();

    beforeEach(function () {
        // Load 'Home' page
        homePage.openSearchPage(testData.search.homeUrl);
        // After loading 'Home' page, check whether it properly loaded or not by verify element in that page
        //homePage.isPageLoaded();
        // Verify the URL loaded properly or not
        expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
        // Navigate to 'Exceptions' page by clicking on 'Exceptions' tab
        homePage.navigateToExceptionsPage();
        console.log(testData.User.Role);
        if (testData.User.Role == "SuperAdmin") {
            // Load 'FastWeb Orders' page by clicking on 'FastWeb Orders' tab               
            fASTWebOrdersPage.clickOnFASTWebTab();
            // After loading 'FastWeb Orders' page, check whether it properly loaded or not
            fASTWebOrdersPage.isPageLoaded();
        }
        else {
            console.log("enters else block");
            fASTWebOrdersPage.verifyFastWebOrdersPagePresent();
        }
        

    });

    it('Search by BorrowerName Verification', function () {
        // Verify all UI data loaded or not(Like buttons, header, dropdown etc) 
        if (testData.User.Role == "SuperAdmin") {
            fASTWebOrdersPage.filterByBorrowerName();
            fASTWebOrdersPage.validateSearchResult();
        }
        else {
            console.log("enters else block");
            fASTWebOrdersPage.verifyFastWebOrdersPagePresent();
        }
    });

    it('Search by FastWebOrder Number Verification', function () {
        // Verify all UI data loaded or not(Like buttons, header, dropdown etc) 
        if (testData.User.Role == "SuperAdmin") {
            fASTWebOrdersPage.filterByFastWebOrderNum();
            fASTWebOrdersPage.validateSearchResult();
        }
        else {
            console.log("enters else block");
            fASTWebOrdersPage.verifyFastWebOrdersPagePresent();
        }
    });

    it('Search by Property Address Verification', function () {
        // Verify all UI data loaded or not(Like buttons, header, dropdown etc) 
        if (testData.User.Role == "SuperAdmin") {
            fASTWebOrdersPage.filterByPropertyAddress();
            fASTWebOrdersPage.validateSearchResult();
        }
        else {
            console.log("enters else block");
            fASTWebOrdersPage.verifyFastWebOrdersPagePresent();
        }
    });

    it('Fast Web Order Details popup Verification', function () {
        // Verify all UI data loaded or not(Like buttons, header, dropdown etc)
        if (testData.User.Role == "SuperAdmin") {
            fASTWebOrdersPage.areElementsPresent();
            browser.sleep(1000);
            fASTWebOrdersPage.clickOnOrderDetails();
            browser.sleep(1000);
            fASTWebOrdersPage.isOrderDetailsLoaded();
            browser.sleep(1000);
            fASTWebOrdersPage.clickOnOrderDetailsCancel()
        }
        else {
            console.log("enters else block");
            fASTWebOrdersPage.verifyFastWebOrdersPagePresent();
        }
    });

    it('Add Forward To Office popup Verification', function () {
        // Verify all UI data loaded or not(Like buttons, header, dropdown etc)
        if (testData.User.Role == "SuperAdmin") {
            fASTWebOrdersPage.areElementsPresent();
            //fASTWebOrdersPage.areElementsPresentInOrderDetails();
            fASTWebOrdersPage.clickAddOfficeButton();
            fASTWebOrdersPage.isaddForwardToOffice();
            fASTWebOrdersPage.passInputvalues();
            //browser.sleep(1000000);
            //fASTWebOrdersPage.clickOnOAddOfficeClose();
            fASTWebOrdersPage.clickOnOAddOfficeSubmit();
        }
        else {
            console.log("enters else block");
            fASTWebOrdersPage.verifyFastWebOrdersPagePresent();
        }

    });

    it('Fast Web ForwardToOffice verification', function () {
        //Verify all UI data loaded or not(Like buttons, header, dropdown etc)
        if (testData.User.Role == "SuperAdmin") {
            fASTWebOrdersPage.areElementsPresent();
            // Load 'FastWeb Orders' page by clicking on 'FastWeb Orders' tab
            //fASTWebOrdersPage.clickOnFASTWebTab();
            // After loading 'FastWeb Orders' page, check whether it properly loaded or not
            fASTWebOrdersPage.isPageLoaded();
            // Verify all UI data loaded or not(Like buttons, header, dropdown etc)
            fASTWebOrdersPage.areElementsPresent();
            // Go to Indivisual row details by double clicking on particular row in FastWeb page(Grid)
            fASTWebOrdersPage.doubleClickOnFirstRow();
            // Select Dropdown and Sumbit ForwardToOffice
            fASTWebOrdersPage.selectDropAndSubmit();
        }
        else {
            console.log("enters else block");
            fASTWebOrdersPage.verifyFastWebOrdersPagePresent();
        }
    });

    it('Verification of Reload Button', function () {
        //Verify all UI data loaded or not(Like buttons, header, dropdown etc)
        if (testData.User.Role == "SuperAdmin") {
            fASTWebOrdersPage.isPageLoaded();
            fASTWebOrdersPage.areElementsPresent();
            fASTWebOrdersPage.verifyReloadButton();
        }
        else {
            console.log("enters else block");
            fASTWebOrdersPage.verifyFastWebOrdersPagePresent();
        }
    });

    it('Verification of status of Submit button in Forward To page', function () {
        //Verify all UI data loaded or not(Like buttons, header, dropdown etc)
        if (testData.User.Role == "SuperAdmin") {
            fASTWebOrdersPage.isPageLoaded();
            fASTWebOrdersPage.areElementsPresent();
            fASTWebOrdersPage.verifyStatusOfSubmitButton_ForwardToOfficePage();
        }
        else {
            console.log("enters else block");
            fASTWebOrdersPage.verifyFastWebOrdersPagePresent();
        }
    });

});