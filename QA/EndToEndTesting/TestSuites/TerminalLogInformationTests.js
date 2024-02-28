describe('Test Execution on Exceptions - Terminal Log', function () {

    'use strict';

    var pages={};
    pages.Home = require('../pages/Home.js');
    pages.BusinessExceptionQueues = require('../pages/BusinessExceptionQueues.js');
    pages.bEQPage = require('../pages/BusinessExceptionQueues.js');
    pages.terminalLogPage = require('../pages/TerminalLogInformation.js');
    var bEQPage = new pages.BusinessExceptionQueues();
    var testData = require('../resources/testData.json');
    var homePage = new pages.Home();
    var terminalLogPage = new pages.terminalLogPage();
    

    
    it('Verify filters in Terminal Log', function() {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToExceptionsPage();
        bEQPage.isBEQPageDisplayed();
        bEQPage.clickOnTerminalLogTab();
	    terminalLogPage.isTerminalLogRecordsDisplayed();
	    terminalLogPage.gridFilterByFieldName("LogID");
        terminalLogPage.clearFilter("LogID");
        terminalLogPage.gridFilterByFieldName("Date");
        terminalLogPage.clearFilter("Date");
        terminalLogPage.gridFilterByFieldName("Level");
        terminalLogPage.clearFilter("Level");
        terminalLogPage.gridFilterByFieldName("Logger");
        terminalLogPage.clearFilter("Logger");
        terminalLogPage.gridFilterByFieldName("Message");
        terminalLogPage.clearFilter("Message");
        terminalLogPage.gridFilterByFieldName("HostName");
        terminalLogPage.clearFilter("HostName");


    })
})