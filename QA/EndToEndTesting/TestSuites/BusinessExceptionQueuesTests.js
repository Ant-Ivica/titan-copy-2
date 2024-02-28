describe('Test Execution on Exceptions - Business Exception Queue', function () {

    'use strict';

    var pages = {};
    pages.Home = require('../pages/Home.js');
    pages.BusinessExceptionQueues = require('../pages/BusinessExceptionQueues.js');
    pages.bEQPage = require('../pages/BusinessExceptionQueues.js');
    var bEQPage = new pages.BusinessExceptionQueues();
    var testData = require('../resources/testData.json');
    var Search = require('../pages/BusinessExceptionQueues.js');
    var homePage = new pages.Home();
    var reportingPage = require('../pages/reportingTower.js');
    var reportingPage = new reportingPage();


    it('verify BEQ - Default Date', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
        homePage.navigateToExceptionsPage();
        bEQPage.isBEQPageDisplayed();
        bEQPage.defaultDateFilter();
    })


    it('Verify BEQ - Exception window disappears post successfull resubmission', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
        homePage.navigateToExceptionsPage();
        //bEQPage.clickon();
        bEQPage.isBEQPageDisplayed();
        bEQPage.defaultDateFilter();
        bEQPage.ClickonFirstRow();
        bEQPage.resubmitException();
    })

    it('Verify BEQ - Exception window disappears post saving the Exception', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToExceptionsPage();
        bEQPage.isBEQPageDisplayed();
        bEQPage.defaultDateFilter();
        bEQPage.ClickonFirstRow();
        bEQPage.saveBEQException();

    })

    it('verify BEQ - Update FAST Info', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
        homePage.navigateToExceptionsPage();
        bEQPage.isBEQPageDisplayed();
        bEQPage.bEQUpdateFASTInfo();
    })

    it('Verify BEQ - Emailing Out of BEQ Pop-Up Closing', function () {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
        browser.sleep(3000);//To Close LVIS update popup manually while debugging can remove post debugging
        homePage.navigateToExceptionsPage();
        bEQPage.isBEQPageDisplayed();
        bEQPage.defaultDateFilter();
        bEQPage.ClickonFirstRow();
        bEQPage.emailException();
    })

})