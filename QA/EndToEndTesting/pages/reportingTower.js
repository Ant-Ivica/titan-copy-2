module.exports = function () {
    'use strict';
    var objRepo = require('../resources/towerReportingPageRepository.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var Dropdownactions = new dropDownActions();
    var inputactions = new inputBoxActions();
    var testData = require('../resources/testData.json');
    var objRepo1 = require('../resources/HomeObjRepo.json');
    var objRepo2 = require('../resources/BusinessExceptionQueuesObjRepo.json');

    var gridActions = require('../commons/gridFilterActions.js');
    var gridActions = new gridActions();

    var Status = objLocator.findLocator(objRepo.searchPage.txtStatus);
    var userAccountName = objLocator.findLocator(objRepo1.homeObjRepo.userAccountName);
    var FirstRow = objLocator.findLocator(objRepo.searchPage.FirstEntry);
    var orderAtFirstRow = objLocator.findLocator(objRepo.searchPage.orderAtFirstRow);
    var Servicerequest = objLocator.findLocator(objRepo.searchPage.Servicerequest);
    var faiLogo = objLocator.findLocator(objRepo.searchPage.faiLogo);
    var orderSummary = objLocator.findLocator(objRepo.searchPage.orderSummary);
    var ToggleButton = objLocator.findLocator(objRepo.searchPage.Toggle);
    var TogglebyDate = objLocator.findLocator(objRepo.searchPage.TogglebyDate);

    var ExternalRefNum = objLocator.findLocator(objRepo.searchPage.ExternalRefNum);
    var extRefNumTextBox = objLocator.findLocator(objRepo.searchPage.extRefNumTextBox);
    var closeButton = objLocator.findLocator(objRepo.searchPage.closeButton);
    var Dropdown = objLocator.findLocator(objRepo.searchPage.Dropdown);
    var DropdownDate = objLocator.findLocator(objRepo.searchPage.DropdownDate);
    var txtStatus = objLocator.findLocator(objRepo.searchPage.txtStatus);
    var Status_Rejected = objLocator.findLocator(objRepo.searchPage.Status_Rejected);
    var Status_Open = objLocator.findLocator(objRepo.searchPage.Status_Open);
    var txtExternalApp = objLocator.findLocator(objRepo.searchPage.txtExternalApp);
    var txtTenant = objLocator.findLocator(objRepo.searchPage.txtTenant);
    var OrderActivityPage = objLocator.findLocator(objRepo.searchPage.OrderActivityPage);
    var internalRefID = objLocator.findLocator(objRepo.searchPage.internalRefID);
    var internalRefNum_OrderActivity = objLocator.findLocator(objRepo.searchPage.internalRefNum_OrderActivity);
    var TextSearch = objLocator.findLocator(objRepo.searchPage.TextSearch);
    var searchButton = objLocator.findLocator(objRepo.searchPage.searchButton);
    var Tenantcolumn = objLocator.findLocator(objRepo.searchPage.TenantColumn);
    var Tenantdet = objLocator.findLocator(objRepo.searchPage.TenantDet);
    var RFOrdersummary = objLocator.findLocator(objRepo.searchPage.RFOrdersummary);
    var Reportingtab = objLocator.findLocator(objRepo.searchPage.Reportingtab);
    var searchDate = objLocator.findLocator(objRepo.searchPage.searchDate);
    var Spinner = objLocator.findLocator(objRepo.searchPage.Spinner);
    var Attachments = objLocator.findLocator(objRepo.searchPage.Attachments);
    var popupcontent = objLocator.findLocator(objRepo.searchPage.popupcontent);
    var More = objLocator.findLocator(objRepo.searchPage.More); //"(//li[@ng-show='false']//a[contains(.,'+ More')])";
    //var Attachments="(//a[contains(@data-toggle,'modal')])";
    var extnum;
    var internalrefnum;
    var custrefnum;
    var internalref_id;
    var resultrow_InternalRef = objLocator.findLocator(objRepo.searchPage.resultrow_InternalRef);
    //var FFN = objLocator.findLocator(objRepo2.businessExceptionQueuesObjRepo.FFN)

    this.openSearchPage = function (path) {
        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.isReportingPageDisplayed = function () {
        waitActions.waitForElementIsDisplayed(orderSummary);
    }


    this.ClickonFirstRow = function () {
        waitActions.waitForElementIsDisplayed(FirstRow);
        gridActions.filter(Status, 'Open');
      
        if (testData.User.Tenant == "LVIS") {

            waitActions.waitForElementIsDisplayed(Tenantcolumn);
        }
        else {

            expect(Tenantcolumn).toContain(undefined);

        }
        browser.actions().doubleClick(FirstRow).perform();
        screenshots.takeScreenshot('FirstRow')

        return this;
    };


    this.isPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(faiLogo);

        waitActions.waitForElementIsDisplayed(Reportingtab);
        buttonActions.click(Reportingtab);
        return this;
    };

    this.isRFOrderSummary = function () {
        waitActions.waitForElementIsDisplayed(RFOrdersummary);

        buttonActions.click(RFOrdersummary);
        waitActions.waitForElementIsDisplayed(FirstRow);
        browser.actions().doubleClick(FirstRow).perform();
        screenshots.takeScreenshot('FirstRow')

        return this;
    };

    this.PopupisPageLoaded = function (CheckDetails) {
        waitActions.waitForElementIsDisplayed(Servicerequest);
        var list = objLocator.findLocator(objRepo.searchPage.DivMessage);
        var detailes = objLocator.findLocator(objRepo.searchPage.DivMessageFastfile);

        screenshots.takeScreenshot('Servicerequest Searchpage')

        list.getText().then(function (text) {
            var n = text.toString().search("External Reference Number:");
            var n1 = text.toString().search("Customer Reference Number:");
            extnum = text.toString().substring(n, n1).replace("External Reference Number:", "");
            if (CheckDetails)
                expect(text.toString()).toContain(extnum.toString());
            custrefnum = text.toString().substring(n1).replace("Customer Reference Number:", "");
            if (CheckDetails)
                expect(text.toString()).toContain(custrefnum.toString());

        }
        );

        detailes.getText().then(function (text) {

            var n = text.toString().search("Internal Reference Number:");
            var n1 = text.toString().search("Internal Reference Id:");
            internalrefnum = text.toString().substring(n, n1).replace("Internal Reference Number:", "");
            if (CheckDetails)
                expect(text.toString()).toContain(internalrefnum.toString());

            internalref_id = text.toString().substring(n1).replace("Internal Reference Id:", "");
            if (CheckDetails)
                expect(text.toString()).toContain(internalref_id.toString());
        }
        );

        buttonActions.click(More);

        buttonActions.click(Attachments);
        waitActions.waitForElementIsDisplayed(popupcontent);
        buttonActions.click(closeButton);



        return this;
    };

    this.Popupclose = function () {


        screenshots.takeScreenshot('Popupclose -' + ExternalRefNum)
        buttonActions.click(closeButton);

        return this;
    };

    this.ClickonDateSearchToggle = function () {

        browser.actions().click(TogglebyDate).perform();
        waitActions.waitForElementIsDisplayed(DropdownDate);
        //DropdownDate
        Dropdownactions.selectDropdownbyNum(DropdownDate, 3)
        buttonActions.click(searchDate);

        browser.wait(function () {
            return Spinner.isDisplayed().then(function (result) { return !result });
        }, 20000);
    }

    this.verifyBindingInReportingPage = function (fastFN, fastFID) {

        browser.actions().click(TogglebyDate).perform();
        waitActions.waitForElementIsDisplayed(DropdownDate);
        //DropdownDate
        Dropdownactions.selectDropdownbyNum(DropdownDate, 3)
        buttonActions.click(searchDate);
        waitActions.waitForElementIsDisplayed(FirstRow);
        inputactions.type(extRefNumTextBox, fastFN);
        waitActions.waitForElementIsDisplayed(FirstRow);
        browser.actions().click(orderAtFirstRow).perform();
        browser.sleep(1000);
        browser.actions().doubleClick(orderAtFirstRow).perform();
        waitActions.waitForElementIsDisplayed(OrderActivityPage);
        var FFN = internalRefNum_OrderActivity.getText().then(function _onSuccess(text) {
            console.log(text);
        });
        expect(fastFN).toContain(FFN);
        var FFID = internalRefID.getText().then(function _onSuccess(text) {
            console.log(text);
        });
        expect(fastFID).toContain(FFID);
        console.log("Fast File Number and Fast File ID are same. Binding Successful");
        browser.actions().click(closeButton).perform();
        waitActions.waitForElementIsDisplayed(faiLogo);
    }

    this.FASTWebFFID = function () {

        inputactions.type(txtStatus, "Open");
        inputactions.type(txtExternalApp, "FastWeb");
        inputactions.type(txtTenant, "Agency");
        waitActions.waitForElementIsDisplayed(FirstRow);
        console.log("Searched FASTWeb order");
        browser.actions().click(orderAtFirstRow).perform();
        browser.sleep(1000);
        browser.actions().doubleClick(orderAtFirstRow).perform();
        screenshots.takeScreenshot('FirstRow')
        waitActions.waitForElementIsDisplayed(OrderActivityPage);
        var FFID = internalRefID.getText().then(function _onSuccess(text) {
            console.log(text);
        });
        browser.actions().click(closeButton).perform();
        waitActions.waitForElementIsDisplayed(faiLogo);
        console.log("FFID received");
        return FFID;

    }

    this.VerifyOrderStatusInReportingTab = function (TID) {

        inputactions.type(extRefNumTextBox, TID);
        waitActions.waitForElementIsDisplayed(FirstRow);
        console.log("Searched for the order and check order status");
        Status_Open.getText().then(function (text) {
            expect(text).toContain('Open');
        })
    }

    this.VerifyOrderRejectInReportingTab = function (TID) {

        inputactions.type(extRefNumTextBox, TID);
        waitActions.waitForElementIsDisplayed(FirstRow);
        console.log("Searched for the order and check order status");
        Status_Rejected.getText().then(function (text) {
            expect(text).toContain('Rejected');
        })
    }

    //      waitActions.waitForElementIsDisplayed(ExternalRefNum);     
    //     Dropdownactions.selectDropdownbyNum(Dropdown, option)
    //     if(option ==1 )
    //         inputactions.type(TextSearch,extnum);             
    //     else if(option ==2 )
    //         inputactions.type(TextSearch,internalrefnum);
    //   else if(option ==3 )
    //         inputactions.type(TextSearch,custrefnum);

    //     screenshots.takeScreenshot('TextSearch -'+ option.toString())
    //     buttonActions.click(searchButton);

    //     return this;
    // };


    this.ClickonToggle = function (option) {

        if (option == 1)
        {
            browser.actions().mouseMove(ToggleButton).click().perform();
            buttonActions.click(ToggleButton);
        }

            waitActions.waitForElementIsDisplayed(ExternalRefNum);


        Dropdownactions.selectDropdownbyNum(Dropdown, option)
        if (option == 1)
            inputactions.type(TextSearch, extnum);
        else if (option == 2)
            inputactions.type(TextSearch, internalrefnum);
        else if (option == 3)
            inputactions.type(TextSearch, custrefnum);
        else if (option == 4)
            inputactions.type(TextSearch, internalref_id);
        screenshots.takeScreenshot('TextSearch -' + option.toString())
        buttonActions.click(searchButton);
        browser.wait(function () {
            return Spinner.isDisplayed().then(function (result) { return !result });
        }, 20000);

        return this;
    };

    this.searchByExternalRefNumber = function (option, value) {
        waitActions.waitForElementIsDisplayed(ExternalRefNum);
        Dropdownactions.selectDropdownbyNum(Dropdown, option)
        if (option == 1)
            console.log("Testlog");
        inputactions.type(TextSearch, value);
        buttonActions.click(searchButton);
        browser.wait(function () {
            return Spinner.isDisplayed().then(function (result) { return !result });
        }, 20000);

        return this;
    }

    this.getFFNResultRow = function () {
        waitActions.waitForElementIsDisplayed(FirstRow);
        resultrow_InternalRef.getText().then(function (text) {
            var FullFFN = text.split('-');
            var FFN = FullFFN[0];
            console.log(FFN);
            return FFN;
        })
    }

    this.toggle = function (value) {
        buttonActions.click(ToggleButton);
    }

    this.verifyOrderUpdateFastInfo = function (FFN, updatedTID) {
        this.toggle(1);
        this.searchByExternalRefNumber(1, updatedTID);
        waitActions.waitForElementIsDisplayed(FirstRow);
        var actualFFN = this.getFFNResultRow();
        if (FFN === actualFFN) {
            screenshots.takeScreenshot('actualFFN');
        }
        else {
            buttonActions.click(FirstRow);
            screenshots.takeScreenshot("BEQ resubmission Error - UpdateExternalRefNumFromBEQ");
        }
    }

};