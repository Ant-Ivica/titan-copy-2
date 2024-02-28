module.exports = function () {

    'use Strict';
    var pages = {};
    // Mapping Tables - Customers
    var objRepo = require('../resources/BusinessExceptionQueuesObjRepo.json');
    var reportingRep = require('../resources/towerReportingPageRepository.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    // pages.bEQPage = require('../pages/BusinessExceptionQueues.js');
    var reportingPage = require('../pages/reportingTower.js');
    //common - objects
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
    // var reportingPage = new pages.reportingPage();

    var tEQTab = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.tEQTab);
    var bEQTab = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.bEQTab);
    var bEQHeader = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.bEQHeader);
    var bEQFirstRecord = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.bEQFirstRecord);
    // var filterExceptionType = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.filterExceptionType);
    var filterByTextExceptionType = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.filterByTextExceptionType);
    var exceptionInfo = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.exceptionInfo);
    var messageInfo = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.messageInfo);
    var resubmitButton = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.resubmitButton);
    var emailButton = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.emailButton);
    var sendButton = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.send);
    
    var save = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.save);
    var confirmPopUp = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.confirmPopUp);
    var outboundEmailPopUp = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.outboundEmailPopUp);
    
    var confirmOk = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.confirmOk);
    var resubmitConfMsg = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.resubmitConfMsg);
    var emailConfMsg = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.emailConfMsg);
    
    var statusResolved = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.statusResolved);
    var updateFASTInfo = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.updateFASTInfo);
    var updateFASTInfoHeader = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.updateFASTInfoHeader);
    var update = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.update);
    var updateSuccessMsg = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.updateSuccessMsg);
    var dateFilter = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.dateFilter);
    var search = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.search);
    var spinner = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.spinner);
    var reportingTab = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.reportingTab);
    var terminalLogTab = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.terminalLogTab);
    var FirstRow = objLocator.findLocator(reportingRep.searchPage.FirstEntry);
    //var FFN = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo)
    var updatedTID;
    var FFN;
    var intRefNumber = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.intRefNumber);
    var cancel = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.cancel);
    var cancelPop = objLocator.findLocator(objRepo.businessExceptionQueuesObjRepo.cancelPop);

    this.clickOnTEQTab = function () {
        waitActions.waitForElementIsDisplayed(tEQTab);
        buttonActions.click(tEQTab);
        return this;
    }

    this.clickOnBEQTab = function () {
        waitActions.waitForElementIsDisplayed(bEQTab);
        buttonActions.click(bEQTab);
        return this;
    }

    this.isBEQPageDisplayed = function () {
        waitActions.waitForElementIsDisplayed(bEQHeader);
    }

    this.defaultDateFilter = function () {

        expect(dateFilter.$('option:checked').getText()).toBe("Last 15 Days");
    }

    this.dateFilter = function () {

        expect(dateFilter.$('option:checked').getText()).toBe("Last 15 Days");
    }

    this.isFirstRowDisplayed = function () {

        waitActions.waitForElementIsDisplayed(bEQFirstRecord);
    }

    this.bEQUpdateFASTInfo = function () {
        dropDownActions.select(dateFilter, "Last 365 Days");
        buttonActions.click(search);
        waitActions.waitForElementIsDisplayed(spinner);
        waitActions.waitForElementIsDisplayed(search);
        // browser.wait(function () {
        //     return spinner.isDisplayed().then(function (result) { return !result });
        // }, 1000);
        inputBoxActions.typeAndEnter(filterByTextExceptionType, "Duplicate_Order_Source");
        if (waitActions.waitForElementIsDisplayed(bEQFirstRecord) != 0) {
            browser.actions().doubleClick(bEQFirstRecord).perform();
            browser.actions().doubleClick(bEQFirstRecord).perform();
            waitActions.waitForElementIsDisplayed(exceptionInfo);
            browser.sleep(8000);
            //   mouseActions.mouseDown(updateFASTInfo);
            buttonActions.click(updateFASTInfo);
            waitActions.waitForElementIsDisplayed(updateFASTInfoHeader);
            intRefNumber.getAttribute('class').then(function (text) {
                console.log(text);
                var myJSON = JSON.stringify(text);
                console.log(myJSON);
                if (myJSON.includes('ng-not-empty')) {
                    buttonActions.click(update);
                    waitActions.waitForElementIsDisplayed(updateSuccessMsg);
                    updateSuccessMsg.getText().then(function (text) {
                        console.log(text);
                        var textSplit = text.split(' ');
                        updatedTID = textSplit[3];
                        FFN = textSplit[8];
                        console.log(updatedTID);
                        console.log(FFN);
                        browser.sleep(8000);
                        screenshots.takeScreenshot('updateSuccessMsg');
                        console.log(browser.getCurrentUrl);
                        return FFN, updatedTID;
                    })
                    buttonActions.click(reportingTab);

                }
                else {
                    screenshots.takeScreenshot('No Fast File Number available for this record');
                }
            })
        }
        else {
            screenshots.takeScreenshot('No Duplicate Order Source records exist!');
        }

        //return this;
    };

    this.clickOnTerminalLogTab = function () {
        buttonActions.click(terminalLogTab);
    }

    this.ClickonFirstRow = function () {
        var until = protractor.ExpectedConditions;
        browser.wait(until.presenceOf(bEQFirstRecord), 2400000, 'Element taking too long to appear in the DOM');


        browser.actions().doubleClick(bEQFirstRecord).perform();
        browser.actions().doubleClick(bEQFirstRecord).perform();
        return this;
    };

    this.resubmitException = function () {

        waitActions.waitForElementIsDisplayed(exceptionInfo);
        //buttonActions.click(messageInfo);
        browser.sleep(3000);
        buttonActions.click(resubmitButton);
        waitActions.waitForElementIsDisplayed(confirmPopUp);
        buttonActions.click(confirmOk);
        browser.sleep(10000);
        resubmitConfMsg.getText().then(function (text) {
            expect(text).toBe('Exception was resubmitted successfully')
        })
        //buttonActions.click(closeExceptionDetails);
        //browser.sleep(3000);
        buttonActions.click(bEQHeader);
        //waitActions.waitForElementIsDisplayed(tEQConfMsg);
        // browser.sleep(10000);
        // waitActions.waitForElementIsDisplayed(statusResolved);
        // statusResolved.getText().then(function(text){
        // expect(text).toBe("Resolved");
        // })

        return this;
    }

    
    this.emailException = function () {

        waitActions.waitForElementIsDisplayed(exceptionInfo);
        //buttonActions.click(messageInfo);
        browser.sleep(3000);
        buttonActions.click(emailButton);
        waitActions.waitForElementIsDisplayed(outboundEmailPopUp);
        buttonActions.click(sendButton);
        browser.sleep(5000);
        emailConfMsg.getText().then(function (text) {
            expect(text).toBe('Email sent to Escrow Officer')
        })    
        return this;
    }


    this.saveBEQException = function () {
        waitActions.waitForElementIsDisplayed(exceptionInfo);
        buttonActions.click(messageInfo);
        browser.sleep(3000);
        buttonActions.click(save);
        browser.sleep(10000);
        resubmitConfMsg.getText().then(function (text) {
            expect(text).toBe('Exception information was saved successfully')
        })

        return this;
    }
}

