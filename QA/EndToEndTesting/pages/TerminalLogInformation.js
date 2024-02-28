module.exports = function() {
    'use strict';
    var objRepo = require('../resources/TerminalLogInformationObjRepo.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var utilspage = require('../utils/objectLocator.js'); 
    var buttonActions = require('../commons/buttonActions.js');
    var waitActions = require('../commons/waitActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var dropDownActions = require('../commons/dropDownActions');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var objLocator = new utilspage(); 
    var gridFilterActions = new gridFilterActions();
    var buttonActions = new buttonActions(); 
    var waitActions = new waitActions();
    var dropDownActions = new dropDownActions();
    var inputBoxActions = new inputBoxActions();

    var terminalLogTab = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.terminalLogTab);
    var terminalLogHeader = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.terminalLogHeader);
    var startTime = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.startTime);
    var endTime = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.endTime);
    var search = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.search);
    var spinner = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.spinner);
    var logIdFilter = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.logIdFilter);
    var dateFilter = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.dateFilter);
    var levelFilter = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.levelFilter);
    var loggerFilter = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.loggerFilter);
    var messageFilter = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.messageFilter);
    var hostNameFilter = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.hostNameFilter);
    var logIdClear = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.logIdClear);
    var dateClear = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.dateClear);
    var levelClear = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.levelClear);
    var loggerClear = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.loggerClear);
    var messageClear = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.messageClear);
    var hostNameClear = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.hostNameClear);
    var firstRow = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.firstRow);
    var resultrow_LogID = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.resultrow_LogID);
    var resultrow_Date = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.resultrow_Date);
    var resultrow_Level = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.resultrow_Level);
    var resultrow_Logger = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.resultrow_Logger);
    var resultrow_Message = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.resultrow_Message);
    var resultrow_HostName = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.resultrow_HostName);
    var date = objLocator.findLocator(objRepo.terminalLogInfoObjRepo.date);

    this.isTerminalLogRecordsDisplayed = function() {
        waitActions.waitForElementIsDisplayed(terminalLogHeader);
        console.log("111");
        inputBoxActions.type(date, "11/26/2019" );
        dropDownActions.selectDropdownbyNum(startTime, 1);
        buttonActions.click(search);
        //waitActions.waitForElementIsDisplayed(firstRow);
        browser.sleep(25000);
        firstRow.isDisplayed().then(function(text) {
        if(text)
        {
            console.log("Records available");
        }        
        else
        {
             console.log("No Records to Display");
        }
        
    })
}

    // this.verifyTerminalLogFilters = function() {
    //     console.log("In function");
    //     waitActions.waitForElementIsDisplayed(search);
    //     console.log("found search");
    //     dropDownActions.selectDropdownbyNum(startTime, 1);
    //     buttonActions.click(search);
    //     waitActions.waitForElementIsDisplayed(firstRow);
    //     inputBoxActions.type(logIdFilter, '12');
    //     buttonActions.click(logIdClear);
    //     waitActions.waitForElementIsDisplayed(firstRow);
    //     inputBoxActions.type(dateFilter, '10');
    //     buttonActions.click(dateClear);
    //     waitActions.waitForElementIsDisplayed(firstRow);
    //     inputBoxActions.type(levelFilter, 'INFO');
    //     buttonActions.click(levelClear);
    //     waitActions.waitForElementIsDisplayed(firstRow);
    //     inputBoxActions.type(loggerFilter, 'LVIS');
    //     buttonActions.click(loggerClear);
    //     waitActions.waitForElementIsDisplayed(firstRow);
    //     inputBoxActions.type(messageFilter, 'Heartbeat');
    //     buttonActions.click(messageClear);
    //     waitActions.waitForElementIsDisplayed(firstRow);
    //     inputBoxActions.type(hostNameFilter, 'SNA');
    //     browser.actions().mouseMove(hostNameClear).perform();
    //     browser.actions().click().perform();
    //     waitActions.waitForElementIsDisplayed(firstRow);

    // }

    this.gridFilterByFieldName = function (FilterBy) {
        if (FilterBy == "LogID") {
            gridFilterActions.filter(logIdFilter, "12", resultrow_LogID);
        }
        else if (FilterBy == "Date") {
            gridFilterActions.filter(dateFilter, "2019", resultrow_Date);
        }
        else if (FilterBy == "Level") {
            gridFilterActions.filter(levelFilter, "INFO", resultrow_Level);
        }
        else if (FilterBy == "Logger") {
            gridFilterActions.filter(loggerFilter, "LVIS.Services.Windows", resultrow_Logger);
        }
        else if (FilterBy == "Message") {
            gridFilterActions.filter(messageFilter, "Heartbeat: ", resultrow_Message);
        }
        else if (FilterBy == "HostName") {
            gridFilterActions.filter(hostNameFilter, "SNAVNAPPLVIS", resultrow_HostName);
        }
    }

    this.clearFilter = function (Clear) {
        if (Clear == "LogID")
            buttonActions.click(logIdClear);
        else if (Clear == "Date")
            buttonActions.click(dateClear);
        else if (Clear == "Level")
            buttonActions.click(levelClear);
        else if (Clear == "Logger")
            buttonActions.click(loggerClear);
        else if (Clear == "Message")
            buttonActions.click(messageClear);
        else if (Clear == "HostName") {
            browser.actions().mouseMove(hostNameClear).perform();
            browser.actions().click().perform();
        }

    }

}