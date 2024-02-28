describe('Test Execution on BusinessExceptionQueue', function () {

    'use strict';
    var pages = {};

    var testData = require('../resources/testData.json');

    pages.BEQPage = require('../pages/BEQ.js');
    var BEQPage = new pages.BEQPage();



    //var beqPage = require('../pages/BEQ.js');


    var beQPageExc = require('../pages/BusinessExceptionQueues.js');
    var bEQPageExc = new beQPageExc();

    var Search = require('../pages/reportingTower.js');
    var searchPage = new Search();
    pages.Home = require('../pages/Home.js');
    var homePage = new pages.Home();

    var utilspage = require('../utils/objectLocator.js');
    var buttonActions = require('../commons/buttonActions.js');
    var waitActions = require('../commons/waitActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var mouseActions = require('../commons/mouseActions.js');

    var dropDownActions = require('../commons/dropDownActions');
    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var gridFilterActions = new gridFilterActions();
    var buttonActions = new buttonActions();
    var inputBoxActions = new inputBoxActions();
    var mouseActions = new mouseActions();
    var dropDownActions = new dropDownActions();
    var internalRefID;

     it('Verify Home page to BEQ screen', function () {
         BEQPage.openSearchPage(testData.search.homeUrl);
         BEQPage.navigateToExceptionsPage();
     });

     it('Verify BEQ Screen Loaded properly', function () {
         BEQPage.openSearchPage(testData.search.homeUrl);
         BEQPage.isPageLoaded();
         BEQPage.exceptiondetailsView();
     });

     it('Verify the datefilter is getting enable when selecting custom', function () {
         BEQPage.openSearchPage(testData.search.homeUrl);
         BEQPage.isPageLoaded();
         BEQPage.selectingCustom();
     });

     it('Verify the bind for potential match found exception', function () {
         BEQPage.openSearchPage(testData.search.homeUrl);
         BEQPage.filterException();
     });

    it('Verify the bind for multiple match bound exception for ATC tenant', function () {
        BEQPage.openSearchPage(testData.search.homeUrl);
        BEQPage.openSearchPage(testData.search.businessException);
        BEQPage.routeExceptionAndBindForATC();
    });

    it('Verify UNBIND in Tower BEQ screen for ATC tenant', function () {
        BEQPage.openSearchPage(testData.search.homeUrl);
        BEQPage.openSearchPage(testData.search.businessException);
        browser.sleep(2000);
        var internalRefNum = BEQPage.routeExceptionAndBindForATC();
        BEQPage.openSearchPage(testData.search.homeUrl);
        BEQPage.openSearchPage(testData.search.businessException);
        BEQPage.UnbindWithRegionID();
    });

    browser.sleep(10000);
    it('Verify the bind functinality by searching InternalRefID for SuperAdmin user for ATC tenant', function () {
        if (testData.User.Role == "SuperAdmin") {
            BEQPage.openSearchPage(testData.search.homeUrl);
            expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
            homePage.isPageLoaded();
            homePage.clickOnReportingTab();
            searchPage.isReportingPageDisplayed();
            internalRefID = BEQPage.FASTWebFFIDforATC();
            console.log(internalRefID);
            BEQPage.openSearchPage(testData.search.homeUrl);
            homePage.isPageLoaded();
            homePage.clickOnReportingTab();
            searchPage.ClickonDateSearchToggle();
            BEQPage.verifyBindingInReportingPage();
            BEQPage.openSearchPage(testData.search.homeUrl);
            BEQPage.UnbindWithRegionID();
        }
        else {
            console.log("enters else block");
            bEQPageExc.isBEQPageDisplayed();
        }

    });

    browser.sleep(10000);

     it('Verify the bind functinality by searching InternalRefID for SuperAdmin user with tenant as LVIS', function () {
         if (testData.User.Role == "SuperAdmin") {
             BEQPage.openSearchPage(testData.search.homeUrl);
             expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
             homePage.isPageLoaded();
             homePage.clickOnReportingTab();
             searchPage.isReportingPageDisplayed();
             internalRefID = BEQPage.FASTWebFFID();
             console.log(internalRefID);
             var externalRefNumber = ""; //BEQPage.forceBindNoGoodMatchOrders(internalRefID);
             // console.log("Binding done");
             BEQPage.openSearchPage(testData.search.homeUrl);
             homePage.isPageLoaded();
             homePage.clickOnReportingTab();
             searchPage.ClickonDateSearchToggle();
             BEQPage.verifyBindingInReportingPage();
             BEQPage.openSearchPage(testData.search.homeUrl);
             BEQPage.Unbind();
             //BEQPage.unbindTheBindedOrder();
         }
         else {
             console.log("enters else block");
             bEQPageExc.isBEQPageDisplayed();
         }

     });

     it('Verify validate message for Unbound function', async () => {
         await BEQPage.getFastFileNum();
         BEQPage.openSearchPage(testData.search.homeUrl);
         BEQPage.Unbind();
     });

     it('Verify New Order', function () {
         BEQPage.openSearchPage(testData.search.homeUrl);
         BEQPage.NewOrder();
     })

     it('Verify Delete Button in BEQ Page for ATC', function () {
        BEQPage.openSearchPage(testData.search.homeUrl);
        BEQPage.openSearchPage(testData.search.businessException);
        BEQPage.Delete();
    })

    it('Verify Cancel Button in BEQ Page for ATC', function () {
        BEQPage.openSearchPage(testData.search.homeUrl);
        BEQPage.openSearchPage(testData.search.businessException);
        BEQPage.Cancel();
    })

    it('Verify the reject function in BEQ', function () {
        BEQPage.openSearchPage(testData.search.homeUrl);
        var externalRefNumber = BEQPage.reject();
        console.log(externalRefNumber);
        homePage.clickOnReportingTab();
        searchPage.isReportingPageDisplayed();
        searchPage.VerifyOrderRejectInReportingTab(externalRefNumber);
    })


     it('Verify BEQ - Update FAST Info Page', function() {
         if (testData.User.Role == "SuperAdmin") {
             BEQPage.openSearchPage(testData.search.homeUrl);
             BEQPage.ValidateUpdateFASTInfoPage();
             BEQPage.ValidateCancelInUpdateFASTInfoPage();
         }
     })

     it('Verify BEQ - UpdateAndReject Button in UpdateFAST Info for ATC', function() {
         if (testData.User.Role == "SuperAdmin") {
            BEQPage.openSearchPage(testData.search.homeUrl);
            BEQPage.openSearchPage(testData.search.businessException);
            var externalRefNumber = BEQPage.ValidateUpdateFASTInfoPage_UpdateReject();
            console.log(externalRefNumber);
            homePage.clickOnReportingTab();
            searchPage.isReportingPageDisplayed();
            searchPage.VerifyOrderStatusInReportingTab(externalRefNumber);
         }
     })
ATC
     it('Verify BEQ - Update FAST Info Update Button for ATC', function() {
         if (testData.User.Role == "SuperAdmin") {
             BEQPage.openSearchPage(testData.search.homeUrl);
             BEQPage.openSearchPage(testData.search.businessException);
             var externalRefNumber = BEQPage.ValidateUpdateFASTInfoPage_Update();
             console.log(externalRefNumber);
             homePage.clickOnReportingTab();
             searchPage.isReportingPageDisplayed();
             searchPage.VerifyOrderStatusInReportingTab(externalRefNumber);
         }
     })

     it('Verify BEQ - Update FAST Info With RegionId For NonLenderServices', function() {
         if (testData.User.Role == "SuperAdmin") {
             BEQPage.openSearchPage(testData.search.homeUrl);
             BEQPage.ValidateUpdateFASTInfoPage();
         }
     })

     it('Verify BEQ - Resubmit With ATC tenant', function() {
        BEQPage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        BEQPage.openSearchPage(testData.search.businessException);
        BEQPage.ResubmitBEQexception();
        BEQPage.openSearchPage(testData.search.businessException);
        browser.sleep(2000);
        BEQPage.ValidateBEQResubmit();
     })

});


