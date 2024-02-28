module.exports = function () {

    'use strict';

    browser.waitForAngularEnabled(true);

    var DBpage = require('../utils/DBUtilsNew.js');
    var DBUtilNew = new DBpage();

    var BEQRepo = require('../resources/BEQObjRepo.json');
    var objRepo = require('../resources/towerReportingPageRepository.json');

    var Search = require('../pages/reportingTower.js');
    var Home = require('../pages/Home.js');
    var screenshots = require('protractor-take-screenshots-on-demand');

    var searchPage = new Search();
    var homePage = new Home();

    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var gridActions = require('../commons/gridFilterActions.js');
    var mouseActions = require('../commons/mouseActions.js');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var dropDownActions = new dropDownActions();
    var inputBoxActions = new inputBoxActions();
    var gridActions = new gridActions();
    var mouseActions = new mouseActions();

    //Variable Decalrations
    var result;
    var canonical_Order;
    var externalRef;
    var internalRef;
    var searchRegionId;
    var validatedInternalRef;

    var externalRef_unbind;
    var internalRef_unbind;
    var region_unbind;

    var ExceptionHeader = objLocator.findLocator(BEQRepo.BEQObjRepo.ExceptionHeader);
    var BusinessExceptionQueues = objLocator.findLocator(BEQRepo.BEQObjRepo.BusinessExceptionQueues);
    var GridAvail = objLocator.findLocator(BEQRepo.BEQObjRepo.GridAvail);
    var statusFilter = objLocator.findLocator(BEQRepo.BEQObjRepo.statusFilter);
    var dateFilter = objLocator.findLocator(BEQRepo.BEQObjRepo.dateFilter);
    var exceptionType = objLocator.findLocator(BEQRepo.BEQObjRepo.exceptionType);
    var tenantColumn = objLocator.findLocator(BEQRepo.BEQObjRepo.tenantColumn);
    var extApplicationColumn = objLocator.findLocator(BEQRepo.BEQObjRepo.extApplicationColumn);
    var ExternalRef = objLocator.findLocator(BEQRepo.BEQObjRepo.ExternalRef);


    var searchIcon = objLocator.findLocator(BEQRepo.BEQObjRepo.searchIcon);
    var includeResolvedCheckBox = objLocator.findLocator(BEQRepo.BEQObjRepo.includeResolvedCheckBox);
    var resultRow = objLocator.findLocator(BEQRepo.BEQObjRepo.resultRow);
    var bindButton = objLocator.findLocator(BEQRepo.BEQObjRepo.bindButton);
    var addNotes_lbl = objLocator.findLocator(BEQRepo.BEQObjRepo.addNotes_lbl);
    var addNotes_Text = objLocator.findLocator(BEQRepo.BEQObjRepo.addNotes_Text);
    var bindAction = objLocator.findLocator(BEQRepo.BEQObjRepo.bindAction);
    var bind_confMessage = objLocator.findLocator(BEQRepo.BEQObjRepo.bind_confMessage);
    var beqGridhome = objLocator.findLocator(BEQRepo.BEQObjRepo.beqGridhome);
    var homeTab = objLocator.findLocator(BEQRepo.BEQObjRepo.homeTab);
    var firstRow = objLocator.findLocator(BEQRepo.BEQObjRepo.firstRow);
    var firstRowDuplicateOrderSource = objLocator.findLocator(BEQRepo.BEQObjRepo.firstRow_DuplicateOrderSource);
    var firstRowMulMatchFound = objLocator.findLocator(BEQRepo.BEQObjRepo.multiplematchVerify);
    var firstRowGrid = objLocator.findLocator(BEQRepo.BEQObjRepo.firstRowGrid);
    var firstRowVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.firstRowVerify);
    var homeButton = objLocator.findLocator(BEQRepo.BEQObjRepo.homeButton);
    var secondRow = objLocator.findLocator(BEQRepo.BEQObjRepo.secondRow);
    var secondRowVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.secondRowVerify);
    var thirdRowVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.thirdRowVerify);
    var thirdRow = objLocator.findLocator(BEQRepo.BEQObjRepo.thirdRow);
    var fourthRowVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.fourthRowVerify);
    var fourthRow = objLocator.findLocator(BEQRepo.BEQObjRepo.fourthRow);
    var pageExceptionDetailsHeader = objLocator.findLocator(BEQRepo.BEQObjRepo.pageExceptionDetailsHeader);
    var searchHeader = objLocator.findLocator(BEQRepo.BEQObjRepo.searchHeader);
    var fastFileIDTextBox = objLocator.findLocator(BEQRepo.BEQObjRepo.fastFileIDTextBox);
    var fromDate = objLocator.findLocator(BEQRepo.BEQObjRepo.fromDate);
    var unhandledRow = objLocator.findLocator(BEQRepo.BEQObjRepo.unhandledRow);
    var unhandledVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.unhandledVerify);
    var unboundRow = objLocator.findLocator(BEQRepo.BEQObjRepo.unboundRow);
    var unboundVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.unboundVerify);
    var multiplematchRow = objLocator.findLocator(BEQRepo.BEQObjRepo.multiplematchRow);
    var multiplematchVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.multiplematchVerify);
    var potentialRow = objLocator.findLocator(BEQRepo.BEQObjRepo.potentialRow);
    var potentialVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.potentialVerify);
    var toDate = objLocator.findLocator(BEQRepo.BEQObjRepo.toDate);
    var newserviceRow = objLocator.findLocator(BEQRepo.BEQObjRepo.newserviceRow);
    var newserviceVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.newserviceVerify);
    var searchGrid = objLocator.findLocator(BEQRepo.BEQObjRepo.searchGrid);
    var loanexceedsRow = objLocator.findLocator(BEQRepo.BEQObjRepo.loanexceedsRow);
    var loanexceedsVerify = objLocator.findLocator(BEQRepo.BEQObjRepo.loanexceedsVerify);
    var unbind = objLocator.findLocator(BEQRepo.BEQObjRepo.unbind);
    var unbindMatch = objLocator.findLocator(BEQRepo.BEQObjRepo.unbindMatch);

    var OKButton = objLocator.findLocator(BEQRepo.BEQObjRepo.OKButton);
    var externalRefNum_BEQ = objLocator.findLocator(BEQRepo.BEQObjRepo.externalRefNum);
    var searchBtn = objLocator.findLocator(BEQRepo.BEQObjRepo.searchBtn);
    var fastFiletxtbox = objLocator.findLocator(BEQRepo.BEQObjRepo.fastFiletxtbox);
    var regionID = objLocator.findLocator(BEQRepo.BEQObjRepo.regionID);
    var regionbutton = objLocator.findLocator(BEQRepo.BEQObjRepo.regionbutton);
    var validateMsg = objLocator.findLocator(BEQRepo.BEQObjRepo.validateMsg);
    var email = objLocator.findLocator(BEQRepo.BEQObjRepo.email);
    var resultRow1 = objLocator.findLocator(BEQRepo.BEQObjRepo.resultRow1);
    var rejectButton = objLocator.findLocator(BEQRepo.BEQObjRepo.rejectButton);
    var addNotes_lbl1 = objLocator.findLocator(BEQRepo.BEQObjRepo.addNotes_lbl1);
    var addNotes_Text1 = objLocator.findLocator(BEQRepo.BEQObjRepo.addNotes_Text1);
    var reject = objLocator.findLocator(BEQRepo.BEQObjRepo.reject);
    var confMsg = objLocator.findLocator(BEQRepo.BEQObjRepo.confMsg);
    var result = objLocator.findLocator(BEQRepo.BEQObjRepo.result);
    var bind = objLocator.findLocator(BEQRepo.BEQObjRepo.bind);
    var btn_Resubmit = objLocator.findLocator(BEQRepo.BEQObjRepo.btn_Resubmit);
    var btn_NewOrder = objLocator.findLocator(BEQRepo.BEQObjRepo.btn_NewOrder);
    var btn_NewOrder1 = objLocator.findLocator(BEQRepo.BEQObjRepo.btn_NewOrder1);
    var btn_Delete = objLocator.findLocator(BEQRepo.BEQObjRepo.btn_Delete);
    var PopUp_Delete = objLocator.findLocator(BEQRepo.BEQObjRepo.PopUp_Delete);
    var btn_Cancel = objLocator.findLocator(BEQRepo.BEQObjRepo.btn_Cancel);
    var btn_updateFastInfo_Update = objLocator.findLocator(BEQRepo.BEQObjRepo.updateFastInfo_Update);
    var btn_updateFastInfo_UpdateReject = objLocator.findLocator(BEQRepo.BEQObjRepo.updateFastInfo_UpdateReject);
    var addNotes_NewOrderText = objLocator.findLocator(BEQRepo.BEQObjRepo.addNotes_NewOrderText);



    var reportingtab = objLocator.findLocator(objRepo.searchPage.Reportingtab);
    var txtExternalRef = objLocator.findLocator(objRepo.searchPage.txtExternalRef);

    var ServiceReq = objLocator.findLocator(objRepo.searchPage.txtServiceReq);
    var Status = objLocator.findLocator(objRepo.searchPage.txtStatus);
    var Servicerequest_popup = objLocator.findLocator(objRepo.searchPage.Servicerequest);
    var txtStatus = objLocator.findLocator(objRepo.searchPage.txtStatus);
    var txtExternalApp = objLocator.findLocator(objRepo.searchPage.txtExternalApp);
    var txtTenant = objLocator.findLocator(objRepo.searchPage.txtTenant);
    var Servicerequest = objLocator.findLocator(objRepo.searchPage.Servicerequest);
    //Grid Result values
    var result_serviceReq = objLocator.findLocator(objRepo.searchPage.resultrow_ServiceReqId);
    var resultrow_ExternalRef = objLocator.findLocator(objRepo.searchPage.resultrow_ExternalRef);
    var FirstRow = objLocator.findLocator(objRepo.searchPage.FirstEntry);
    var result_status = objLocator.findLocator(objRepo.searchPage.resultrow_Status);
    var FirstRow = objLocator.findLocator(objRepo.searchPage.FirstEntry);
    var closeButton = objLocator.findLocator(objRepo.searchPage.closeButton);
    var bindInternalRefNum = objLocator.findLocator(BEQRepo.BEQObjRepo.bindInternalRefNum);
    var bindRegionId = objLocator.findLocator(BEQRepo.BEQObjRepo.bindRegionId);
    var Attachments = objLocator.findLocator(objRepo.searchPage.Attachments);
    var popupcontent = objLocator.findLocator(objRepo.searchPage.popupcontent);
    var attachContent = objLocator.findLocator(objRepo.searchPage.attachContent);
    var ToggleButton = objLocator.findLocator(objRepo.searchPage.Toggle);
    var ExternalRefNum = objLocator.findLocator(objRepo.searchPage.ExternalRefNum);
    var Dropdown = objLocator.findLocator(objRepo.searchPage.Dropdown);
    var TextSearch = objLocator.findLocator(objRepo.searchPage.TextSearch);
    var searchButton = objLocator.findLocator(objRepo.searchPage.searchButton);
    var Spinner = objLocator.findLocator(objRepo.searchPage.Spinner);
    var externalApp = objLocator.findLocator(objRepo.searchPage.externalApp);
    var TogglebyDate = objLocator.findLocator(objRepo.searchPage.TogglebyDate);
    var searchDate = objLocator.findLocator(objRepo.searchPage.searchDate);
    var DropdownDate = objLocator.findLocator(objRepo.searchPage.DropdownDate);
    var extRefNumTextBox = objLocator.findLocator(objRepo.searchPage.extRefNumTextBox);
    var OrderActivityPage = objLocator.findLocator(objRepo.searchPage.OrderActivityPage);
    var faiLogo = objLocator.findLocator(objRepo.searchPage.faiLogo);
    var updateFastInfoTitle = objLocator.findLocator(BEQRepo.BEQObjRepo.updateFastInfoTitle);
    var updateFastInfo = objLocator.findLocator(BEQRepo.BEQObjRepo.updateFastInfo);
    var updateFastInfo_ExternalRefNum = objLocator.findLocator(BEQRepo.BEQObjRepo.updateFastInfo_ExternalRefNum);
    var updateFastInfo_InternalRefNum = objLocator.findLocator(BEQRepo.BEQObjRepo.updateFastInfo_InternalRefNum);
    var updateFastInfo_RegionId = objLocator.findLocator(BEQRepo.BEQObjRepo.updateFastInfo_RegionId);
    var updateFastInfo_Cancel = objLocator.findLocator(BEQRepo.BEQObjRepo.updateFastInfo_Cancel);
    var internalRefID;

    this.openSearchPage = function (path) {
        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.isPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(ExceptionHeader);
        buttonActions.click(ExceptionHeader);
        waitActions.waitForElementIsDisplayed(BusinessExceptionQueues);
        buttonActions.click(BusinessExceptionQueues);
        waitActions.waitForElementIsDisplayed(GridAvail);
        return this;
    };

    this.navigateToExceptionsPage = function () {
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(firstRow);
        waitActions.waitForElementIsDisplayed(firstRowVerify);
        buttonActions.click(homeButton);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(secondRow);
        waitActions.waitForElementIsDisplayed(secondRowVerify);
        buttonActions.click(homeButton);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(thirdRow);
        waitActions.waitForElementIsDisplayed(thirdRowVerify);
        buttonActions.click(homeButton);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(fourthRow);
        waitActions.waitForElementIsDisplayed(fourthRowVerify);
        buttonActions.click(homeButton);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(unhandledRow);
        waitActions.waitForElementIsDisplayed(unhandledVerify);
        buttonActions.click(homeButton);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(unboundRow);
        waitActions.waitForElementIsDisplayed(unboundVerify);
        buttonActions.click(homeButton);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(multiplematchRow);
        waitActions.waitForElementIsDisplayed(multiplematchVerify);
        buttonActions.click(homeButton);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(potentialRow);
        waitActions.waitForElementIsDisplayed(potentialVerify);
        buttonActions.click(homeButton);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(newserviceRow);
        waitActions.waitForElementIsDisplayed(newserviceVerify);
        buttonActions.click(homeButton);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        buttonActions.click(loanexceedsRow);
        waitActions.waitForElementIsDisplayed(loanexceedsVerify);
        buttonActions.click(homeButton);
        browser.sleep(1000);
        return this;
    };


    this.selectingCustom = function () {
        waitActions.waitForElementIsDisplayed(GridAvail);
        dropDownActions.select(dateFilter, "Custom");
        fromDate.clear();
        inputBoxActions.type(fromDate, "10/01/2019");
        toDate.clear();
        inputBoxActions.type(toDate, "10/02/2019");
        buttonActions.click(searchIcon);
        waitActions.waitForElementIsDisplayed(searchGrid);
        browser.sleep(1000);
        return this;
    };

    this.exceptiondetailsView = function () {
        waitActions.waitForElementIsDisplayed(GridAvail);
        browser.actions().doubleClick(GridAvail).perform();
        browser.actions().doubleClick(GridAvail).perform();
        waitActions.waitForElementIsDisplayed(email);
        browser.sleep(1000);
        return this;
    };

    this.filterException = function () {
        waitActions.waitForElementIsDisplayed(beqGridhome);
        // inputBoxActions.type(externalApp, "RealEC");
        inputBoxActions.type(exceptionType, "Potential");
        //buttonActions.click(potentialRow);
        //waitActions.waitForElementIsDisplayed(potentialVerify);
        buttonActions.click(firstRow);
        waitActions.waitForElementIsDisplayed(resultRow);
        browser.actions().doubleClick(resultRow).perform();
        browser.actions().doubleClick(resultRow).perform();
        waitActions.waitForElementIsDisplayed(bindButton);
        browser.sleep(1000);
        buttonActions.click(bindButton);
        internalRef = bindInternalRefNum.getText().then(function _onSuccess(text) {
            console.log(text);
            return text;
        })
        browser.sleep(1000);
        waitActions.waitForElementIsDisplayed(addNotes_lbl);
        inputBoxActions.type(addNotes_Text, "Test&Test.com<*");
        buttonActions.click(bindAction);
        browser.sleep(2000);
        waitActions.waitForElementIsDisplayed(bind_confMessage);
        bind_confMessage.getText().then(function (text) {
            expect(text).toContain('bound successfully')
        })
        screenshots.takeScreenshot("Binded Successfully ");
        externalRef = bind_confMessage.getText().then(function _onSuccess(text) {
            return (text.substring(25, text.indexOf("was")).trim());
        });
        browser.sleep(1000);
        this.validateInReportingTower(externalRef, internalRef);
        // return this;
    };

    this.routeExceptionAndBindForATC = function () {
        waitActions.waitForElementIsDisplayed(beqGridhome);
        inputBoxActions.type(exceptionType, "Multiple_Match_Found");
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        buttonActions.click(firstRowMulMatchFound);
        waitActions.waitForElementIsDisplayed(firstRowMulMatchFound);
        browser.actions().doubleClick(firstRowMulMatchFound).perform();
        browser.actions().doubleClick(firstRowMulMatchFound).perform();
        waitActions.waitForElementIsDisplayed(bindButton);
        browser.sleep(1000);
        buttonActions.click(bindButton);
        internalRef = bindInternalRefNum.getText().then(function _onSuccess(text) {
            console.log(text);
            return text;
        })
        searchRegionId = bindRegionId.getText().then(function _onSuccess(text) {
            console.log(text);
            return text;
        })

        browser.sleep(1000);
        waitActions.waitForElementIsDisplayed(addNotes_lbl);
        inputBoxActions.type(addNotes_Text, "Testing bind scenario for ATC");
        buttonActions.click(bindAction);
        browser.sleep(2000);
        waitActions.waitForElementIsDisplayed(bind_confMessage);
        bind_confMessage.getText().then(function (text) {
            expect(text).toContain('bound successfully')
        })
        screenshots.takeScreenshot("Binded Successfully ");
        externalRef = bind_confMessage.getText().then(function _onSuccess(text) {
            return (text.substring(25, text.indexOf("was")).trim());
        });
        browser.sleep(3000);
        console.log(externalRef);
        this.validateInReportingTower(externalRef, internalRef);

        return internalRef;
    };

    this.FASTWebFFID = function () {
        inputBoxActions.type(txtExternalApp, "FastWeb");
        inputBoxActions.type(txtStatus, "Open");
        inputBoxActions.type(txtTenant, "Agency");
        waitActions.waitForElementIsDisplayed(FirstRow);
        console.log("Searched FASTWeb order");
        browser.actions().doubleClick(FirstRow).perform();
        waitActions.waitForElementIsDisplayed(Servicerequest);
        var details = objLocator.findLocator(objRepo.searchPage.DivMessageFastfile);
        screenshots.takeScreenshot('BEQ Exceptions');
        internalRefID = details.getText().then(function (text) {
            var n1 = text.toString().search("Internal Reference Id:");
            return (text.toString().substring(n1).replace("Internal Reference Id:", "").trim());
        });
        browser.sleep(1000);
        console.log(internalRefID);
        this.Popupclose();
        homePage.navigateToExceptionsPage();
        this.isPageLoaded();
        this.forceBindNoGoodMatchOrders(internalRefID);
    };

    this.FASTWebFFIDforATC = function () {
        inputBoxActions.type(txtExternalApp, "RealEC");
        inputBoxActions.type(txtStatus, "Open");
        inputBoxActions.type(txtTenant, "Air Traffic Control");
        waitActions.waitForElementIsDisplayed(FirstRow);
        console.log("Searched FASTWeb order");
        browser.actions().doubleClick(FirstRow).perform();
        waitActions.waitForElementIsDisplayed(Servicerequest);
        var details = objLocator.findLocator(objRepo.searchPage.DivMessageFastfile);
        screenshots.takeScreenshot('BEQ Exceptions');
        internalRefID = details.getText().then(function (text) {
            var n1 = text.toString().search("Internal Reference Id:");
            return (text.toString().substring(n1).replace("Internal Reference Id:", "").trim());
        });
        browser.sleep(1000);
        console.log(internalRefID);
        this.Popupclose();
        homePage.navigateToExceptionsPage();
        this.isPageLoaded();
        this.forceBindNoGoodMatchForATCtenantOrder(internalRefID);
    };

    this.Popupclose = function () {
        screenshots.takeScreenshot('Popupclose -' + ExternalRefNum)
        buttonActions.click(closeButton);
        return this;
    };

    this.forceBindNoGoodMatchForATCtenantOrder = function (internalRefID) {
        console.log(internalRefID);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        console.log(internalRefID);
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        inputBoxActions.type(exceptionType, "No_Good_Match");
        waitActions.waitForElementIsDisplayed(firstRowGrid);
        browser.actions().click(fourthRow).perform();
        browser.actions().doubleClick(fourthRow).perform();
        waitActions.waitForElementIsDisplayed(pageExceptionDetailsHeader);
        buttonActions.click(searchHeader);
        waitActions.waitForElementIsDisplayed(fastFileIDTextBox);
        inputBoxActions.type(fastFileIDTextBox, internalRefID);
        buttonActions.click(searchBtn);
        waitActions.waitForElementIsDisplayed(bindButton);
        browser.sleep(1000);
        searchRegionId = bindRegionId.getText().then(function _onSuccess(text) {
            console.log(text);
            return text;
        })
        buttonActions.click(bindButton);
        browser.sleep(1000);
        waitActions.waitForElementIsDisplayed(addNotes_lbl);
        inputBoxActions.type(addNotes_Text, "Testing for bind scenario for ATC");
        buttonActions.click(bindAction);
        browser.sleep(2000);
        //Verify the below part
        waitActions.waitForElementIsDisplayed(bind_confMessage);
        mouseActions.mouseMove(bind_confMessage);
        bind_confMessage.getText().then(function (text) {
            console.log(text);
            expect(text).toContain('bound successfully')
        })
        screenshots.takeScreenshot('Bind Successfully');
        externalRef = bind_confMessage.getText().then(function _onSuccess(text) {
            console.log(text.substring(25, text.indexOf("was")).trim());
            return (text.substring(25, text.indexOf("was")).trim());
        });
        browser.sleep(1000);
    };

    this.forceBindNoGoodMatchOrders = function (internalRefID) {
        console.log(internalRefID);
        waitActions.waitForElementIsDisplayed(beqGridhome);
        console.log(internalRefID);
        inputBoxActions.type(tenantColumn, "Agency");
        inputBoxActions.type(extApplicationColumn, "RealEC");
        inputBoxActions.type(exceptionType, "No_Good_Match");
        //buttonActions.click(potentialRow);
        waitActions.waitForElementIsDisplayed(firstRowGrid);
        browser.actions().click(fourthRow).perform();
        browser.actions().doubleClick(fourthRow).perform();
        waitActions.waitForElementIsDisplayed(pageExceptionDetailsHeader);
        buttonActions.click(searchHeader);
        waitActions.waitForElementIsDisplayed(fastFileIDTextBox);
        inputBoxActions.type(fastFileIDTextBox, internalRefID);
        buttonActions.click(searchBtn);
        waitActions.waitForElementIsDisplayed(bindButton);
        browser.sleep(1000);
        buttonActions.click(bindButton);
        browser.sleep(1000);
        waitActions.waitForElementIsDisplayed(addNotes_lbl);
        inputBoxActions.type(addNotes_Text, "Test&Test.com<*");
        buttonActions.click(bindAction);
        browser.sleep(2000);
        //Verify the below part
        waitActions.waitForElementIsDisplayed(bind_confMessage);
        mouseActions.mouseMove(bind_confMessage);
        bind_confMessage.getText().then(function (text) {
            console.log(text);
            expect(text).toContain('bound successfully')
        })
        screenshots.takeScreenshot('Bind Successfully');
        externalRef = bind_confMessage.getText().then(function _onSuccess(text) {
            console.log(text.substring(25, text.indexOf("was")).trim());
            return (text.substring(25, text.indexOf("was")).trim());
        });
        browser.sleep(1000);
        //  buttonActions.click(closeButton);
        // waitActions.waitForElementIsDisplayed(homeTab);
        // mouseActions.mouseMove(homeTab);
        // buttonActions.click(homeTab);
        // return externalRef;
    };

    this.verifyBindingInReportingPage = function () {
        browser.actions().click(TogglebyDate).perform();
        waitActions.waitForElementIsDisplayed(DropdownDate);
        //DropdownDate
        dropDownActions.selectDropdownbyNum(DropdownDate, 3)
        inputBoxActions.type(extRefNumTextBox, externalRef);
        buttonActions.click(searchDate);
        waitActions.waitForElementIsDisplayed(FirstRow);
        browser.actions().doubleClick(FirstRow).perform();
        waitActions.waitForElementIsDisplayed(OrderActivityPage);
        var details = objLocator.findLocator(objRepo.searchPage.DivMessageFastfile);
        details.getText().then(function (text) {
            var n1 = text.toString().search("Internal Reference Id:");
            var internalId = text.toString().substring(n1).replace("Internal Reference Id:", "").trim();
            expect(internalId.toContain(internalRefID));
        });
        screenshots.takeScreenshot("Order Got Successfully Binded");
        //console.log("Fast File Number and Fast File ID are same. Binding Successful");
        this.Popupclose();
        waitActions.waitForElementIsDisplayed(faiLogo);
    }

    this.validateUnboundOrder = function (externalRef) {
        buttonActions.click(searchIcon);
        waitActions.waitForElementIsDisplayed(GridAvail);
        inputBoxActions.type(exceptionType, "Unbound");
        inputBoxActions.type(ExternalRef, externalRef);
        waitActions.waitForElementIsDisplayed(resultRow);
    }

    this.validateInReport = function (externalRef) {

        waitActions.waitForElementIsDisplayed(reportingtab);
        buttonActions.click(reportingtab);
        waitActions.waitForElementIsDisplayed(result_serviceReq);
        this.ClickonToggle("1", externalRef);
        waitActions.waitForElementIsDisplayed(result_serviceReq);
        browser.actions().doubleClick(FirstRow).perform();
        browser.sleep(5000);
        var list = objLocator.findLocator(objRepo.searchPage.DivMessage);
        var detailes = objLocator.findLocator(objRepo.searchPage.DivMessageFastfile);

        detailes.getText().then(function (text) {

            var n = text.toString().search("Internal Reference Number:");
            var n1 = text.toString().search("Internal Reference Id:");
            var internalrefnum = text.toString().substring(n, n1).replace("Internal Reference Number:", "");
            expect(internalrefnum.length > 0).toBe(true);
        }
        );

        return this;
    };

    this.validateInReportingTower = function (externalRef, internalRef) {

        waitActions.waitForElementIsDisplayed(reportingtab);
        buttonActions.click(reportingtab);
        waitActions.waitForElementIsDisplayed(result_serviceReq);
        this.ClickonToggle("1", externalRef);
        waitActions.waitForElementIsDisplayed(result_serviceReq);
        browser.actions().doubleClick(FirstRow).perform();
        browser.sleep(5000);
        buttonActions.click(Attachments);
        waitActions.waitForElementIsDisplayed(attachContent);
        canonical_Order = attachContent.getText().then(function _onSuccess(text) {
            console.log(text);
            expect(text).toContain(internalRef);
        })
        browser.sleep(1000);
        buttonActions.click(closeButton);

        browser.sleep(1000);
        buttonActions.click(closeButton);
    }

    this.ClickonToggle = function (option, extnum) {
        if (option == 1)
            browser.actions().mouseMove(ToggleButton).click().perform();
        waitActions.waitForElementIsDisplayed(ExternalRefNum);
        dropDownActions.selectDropdownbyNum(Dropdown, option)
        if (option == 1)
            inputBoxActions.type(TextSearch, extnum);
        else if (option == 2)
            inputBoxActions.type(TextSearch, internalrefnum);
        else if (option == 3)
            inputBoxActions.type(TextSearch, custrefnum);
        //screenshots.takeScreenshot('TextSearch -' + option.toString())
        buttonActions.click(searchButton);
        browser.wait(function () {
            return Spinner.isDisplayed().then(function (result) { return !result });
        }, 20000);
        return this;
    };

    this.bindException = function () {
        dropDownActions.selectDropdownbyNum(dateFilter, "3");
        buttonActions.click(searchIcon);
        waitActions.waitForElementIsDisplayed(GridAvail);
        inputBoxActions.type(exceptionType, "Multiple");
        waitActions.waitForElementIsDisplayed(result);
        browser.actions().doubleClick(result).perform();
        browser.actions().doubleClick(result).perform();
        waitActions.waitForElementIsDisplayed(bind);
        buttonActions.click(bind);
        waitActions.waitForElementIsDisplayed(addNotes_lbl);
        buttonActions.click(bindAction);
        waitActions.waitForElementIsDisplayed(bind_confMessage);
        return this;
    };


    this.getFastFileNum = async () => {
        var Results = await DBUtilNew.ConnectDBAsync("select top 1 * from servicerequest  where  isfilecreated =0 and InternalRefNum is not null order by CreatedDate desc")
        if (Results != undefined) {
            console.log(Results.recordset);
            externalRef_unbind = Results.recordset[0].ExternalRefNum;
            var internalRefnum = Results.recordset[0].InternalRefNum;
            var index = internalRefnum.lastIndexOf("-");
            internalRef_unbind = internalRefnum.substring(0, index);
            region_unbind = internalRefnum.substring(index + 1);
        }
    }

    this.Unbind = function () {
        waitActions.waitForElementIsDisplayed(ExceptionHeader);
        buttonActions.click(ExceptionHeader);
        waitActions.waitForElementIsDisplayed(BusinessExceptionQueues);
        buttonActions.click(BusinessExceptionQueues);
        buttonActions.click(unbind);
        waitActions.waitForElementIsDisplayed(searchBtn);
        inputBoxActions.type(fastFiletxtbox, internalRefID);
        inputBoxActions.type(regionID, 410);
        buttonActions.click(regionbutton);
        buttonActions.click(searchBtn);
        waitActions.waitForElementIsDisplayed(unbindMatch);
        buttonActions.click(unbindMatch);
        waitActions.waitForElementIsDisplayed(OKButton);
        buttonActions.click(OKButton);
        buttonActions.click(closeButton);
        this.validateInReportingTower(externalRef_unbind, "");
        this.isPageLoaded();
        this.validateUnboundOrder(externalRef_unbind);
        return this;
    };

    this.UnbindWithRegionID = function () {
        waitActions.waitForElementIsDisplayed(ExceptionHeader);
        buttonActions.click(ExceptionHeader);
        waitActions.waitForElementIsDisplayed(BusinessExceptionQueues);
        buttonActions.click(BusinessExceptionQueues);
        buttonActions.click(unbind);
        waitActions.waitForElementIsDisplayed(searchBtn);
        inputBoxActions.type(fastFiletxtbox, internalRef);
        browser.sleep(3000);
        inputBoxActions.type(regionID, searchRegionId);
        // buttonActions.click(regionbutton);
        buttonActions.click(searchBtn);
        waitActions.waitForElementIsDisplayed(unbindMatch);
        buttonActions.click(unbindMatch);
        waitActions.waitForElementIsDisplayed(OKButton);
        buttonActions.click(OKButton);
        buttonActions.click(closeButton);
        this.validateInReportingTower(externalRef_unbind, "");
        this.isPageLoaded();
        this.validateUnboundOrder(externalRef_unbind);
        return this;
    };

    this.unbindTheBindedOrder = function (FFN) {
        waitActions.waitForElementIsDisplayed(ExceptionHeader);
        buttonActions.click(ExceptionHeader);
        waitActions.waitForElementIsDisplayed(BusinessExceptionQueues);
        buttonActions.click(BusinessExceptionQueues);
        buttonActions.click(unbind);
        waitActions.waitForElementIsDisplayed(searchBtn);
        inputBoxActions.type(fastFiletxtbox, FFN);
        inputBoxActions.type(regionID, "410");
        buttonActions.click(regionbutton);
        buttonActions.click(searchBtn);
        waitActions.waitForElementIsDisplayed(unbindMatch);
        buttonActions.click(unbindMatch);
        waitActions.waitForElementIsDisplayed(OKButton);
        buttonActions.click(OKButton);
        buttonActions.click(closeButton);
    };

    this.reject = function () {
        buttonActions.click(ExceptionHeader);
        waitActions.waitForElementIsDisplayed(BusinessExceptionQueues);
        dropDownActions.selectDropdownbyNum(dateFilter, "5");
        browser.sleep(15000);
        waitActions.waitForElementIsDisplayed(GridAvail);
        inputBoxActions.type(externalApp, "RealEC");
        inputBoxActions.type(exceptionType, "No_Good_Match");
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        buttonActions.click(firstRow);
        waitActions.waitForElementIsDisplayed(firstRowVerify);
        browser.actions().doubleClick(firstRowVerify).perform();
        browser.actions().doubleClick(firstRowVerify).perform();
        waitActions.waitForElementIsDisplayed(rejectButton);
        buttonActions.click(rejectButton);
        waitActions.waitForElementIsDisplayed(addNotes_lbl1);
        waitActions.waitForElementIsDisplayed(addNotes_Text1);
        inputBoxActions.type(addNotes_Text1, "Test&Test.com>*");
        buttonActions.click(reject);
        waitActions.waitForElementIsDisplayed(confMsg);
        confMsg.getText().then(function (text) {
            expect(text).toContain('was Rejected successfully');
        })
        browser.sleep(10000);
        externalRef = confMsg.getText().then(function _onSuccess(text) {
            return (text.substring(25, text.indexOf("was")).trim());
        });
        browser.sleep(3000);
        console.log(externalRef);
        return externalRef;
    };

    this.NewOrder = function () {
        buttonActions.click(ExceptionHeader);
        waitActions.waitForElementIsDisplayed(BusinessExceptionQueues);
        dropDownActions.selectDropdownbyNum(dateFilter, "5");
        browser.sleep(15000);
        waitActions.waitForElementIsDisplayed(GridAvail);
        inputBoxActions.type(externalApp, "RealEC");
        buttonActions.click(firstRow);
        waitActions.waitForElementIsDisplayed(firstRowVerify);
        waitActions.waitForElementIsDisplayed(firstRowVerify);
        browser.actions().doubleClick(firstRowVerify).perform();
        browser.actions().doubleClick(firstRowVerify).perform();
        waitActions.waitForElementIsDisplayed(btn_NewOrder);
        buttonActions.click(btn_NewOrder);
        waitActions.waitForElementIsDisplayed(addNotes_lbl);
        waitActions.waitForElementIsDisplayed(addNotes_NewOrderText);
        inputBoxActions.type(addNotes_NewOrderText, "Test&Test.com>*");
        buttonActions.click(btn_NewOrder1);
        waitActions.waitForElementIsDisplayed(confMsg);
        confMsg.getText().then(function (text) {
            expect(text).toContain('created successfully');
        })

        externalRef = confMsg.getText().then(function _onSuccess(text) {
            return (text.substring(12, text.indexOf("was")).trim());
        });
        browser.sleep(10000);
        this.validateInReport(externalRef);

    }

    this.Delete = function () {
        buttonActions.click(ExceptionHeader);
        waitActions.waitForElementIsDisplayed(BusinessExceptionQueues);
        dropDownActions.selectDropdownbyNum(dateFilter, "5");
        browser.sleep(15000);
        waitActions.waitForElementIsDisplayed(GridAvail);
        inputBoxActions.type(extApplicationColumn, "RealEC");
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        inputBoxActions.type(exceptionType, "No_Good_Match");
        buttonActions.click(firstRow);
        waitActions.waitForElementIsDisplayed(firstRow);
        browser.actions().doubleClick(firstRow).perform();
        browser.actions().doubleClick(firstRow).perform();
        waitActions.waitForElementIsDisplayed(email);
        waitActions.waitForElementIsDisplayed(btn_Delete);
        buttonActions.click(btn_Delete);
        waitActions.waitForElementIsDisplayed(PopUp_Delete);
        buttonActions.click(PopUp_Delete);
        waitActions.waitForElementIsDisplayed(confMsg);
        confMsg.getText().then(function (text) {
            expect(text).toContain('was Deleted successfully');
        })
    }

    this.Cancel = function () {
        waitActions.waitForElementIsDisplayed(GridAvail);
        inputBoxActions.type(extApplicationColumn, "RealEC");
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        inputBoxActions.type(exceptionType, "No_Good_Match");
        buttonActions.click(firstRow);
        waitActions.waitForElementIsDisplayed(firstRow);
        browser.actions().doubleClick(firstRow).perform();
        browser.actions().doubleClick(firstRow).perform();
        waitActions.waitForElementIsDisplayed(email);
        waitActions.waitForElementIsDisplayed(btn_Cancel);
        buttonActions.click(btn_Cancel);
        waitActions.waitForElementIsDisplayed(GridAvail);
    }

    this.ValidateUpdateFASTInfoPage = function()
    {
        waitActions.waitForElementIsDisplayed(beqGridhome);
        inputBoxActions.type(exceptionType, "Duplicate_Order_Source");
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        buttonActions.click(firstRowDuplicateOrderSource);
        waitActions.waitForElementIsDisplayed(firstRowDuplicateOrderSource);
        browser.actions().doubleClick(firstRowDuplicateOrderSource).perform();
        browser.actions().doubleClick(firstRowDuplicateOrderSource).perform();
        waitActions.waitForElementIsDisplayed(bindButton);
        browser.sleep(10000);
        waitActions.waitForElementIsDisplayed(btn_Resubmit);

        externalRef = externalRefNum_BEQ.getText().then(function _onSuccess(text) {
            console.log(text);
            return text;
        })

        buttonActions.click(updateFastInfo);
        waitActions.waitForElementIsDisplayed(updateFastInfoTitle);
        expect(updateFastInfo_ExternalRefNum.isDisplayed()).toBe(true);
        expect(updateFastInfo_InternalRefNum.isDisplayed()).toBe(true);
    }

    this.ValidateUpdateFASTInfoPage_Update = function()
    {
        waitActions.waitForElementIsDisplayed(beqGridhome);
        inputBoxActions.type(exceptionType, "Duplicate_Order_Source");
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        //buttonActions.click(firstRowDuplicateOrderSource);
        waitActions.waitForElementIsDisplayed(firstRowDuplicateOrderSource);
        browser.actions().doubleClick(firstRowDuplicateOrderSource).perform();
        browser.actions().doubleClick(firstRowDuplicateOrderSource).perform();
        waitActions.waitForElementIsDisplayed(bindButton);
        browser.sleep(10000);
        waitActions.waitForElementIsDisplayed(btn_Resubmit);

        externalRef = externalRefNum_BEQ.getText().then(function _onSuccess(text) {
            console.log(text);
            return text;
        })

        buttonActions.click(updateFastInfo);
        waitActions.waitForElementIsDisplayed(updateFastInfoTitle);
        expect(updateFastInfo_ExternalRefNum.isDisplayed()).toBe(true);
        expect(updateFastInfo_InternalRefNum.isDisplayed()).toBe(true);
        buttonActions.click(btn_updateFastInfo_Update);
        waitActions.waitForElementIsDisplayed(confMsg);
        confMsg.getText().then(function (text) {
            expect(text).toContain('was updated successfully to');
        })
        return externalRef;
    }

    //Update Reject

    this.ValidateUpdateFASTInfoPage_UpdateReject = function()
    {
        waitActions.waitForElementIsDisplayed(beqGridhome);
        inputBoxActions.type(exceptionType, "Duplicate_Order_Source");
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        //buttonActions.click(firstRowDuplicateOrderSource);
        waitActions.waitForElementIsDisplayed(firstRowDuplicateOrderSource);
        browser.actions().doubleClick(firstRowDuplicateOrderSource).perform();
        browser.actions().doubleClick(firstRowDuplicateOrderSource).perform();
        waitActions.waitForElementIsDisplayed(bindButton);
        browser.sleep(10000);
        waitActions.waitForElementIsDisplayed(btn_Resubmit);

        externalRef = externalRefNum_BEQ.getText().then(function _onSuccess(text) {
            console.log(text);
            return text;
        })

        buttonActions.click(updateFastInfo);
        waitActions.waitForElementIsDisplayed(updateFastInfoTitle);
        expect(updateFastInfo_ExternalRefNum.isDisplayed()).toBe(true);
        expect(updateFastInfo_InternalRefNum.isDisplayed()).toBe(true);
        buttonActions.click(btn_updateFastInfo_UpdateReject);
        browser.sleep(5000);
        waitActions.waitForElementIsDisplayed(confMsg);
        confMsg.getText().then(function (text) {
            expect(text).toContain('was updated successfully to');
        })
        return externalRef;
    }

    this.ResubmitBEQexception = function () {
        waitActions.waitForElementIsDisplayed(beqGridhome);
        inputBoxActions.type(exceptionType, "Multiple_Match_Found");
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        buttonActions.click(firstRowMulMatchFound);
        waitActions.waitForElementIsDisplayed(firstRowMulMatchFound);
        browser.actions().doubleClick(firstRowMulMatchFound).perform();
        browser.actions().doubleClick(firstRowMulMatchFound).perform();
        browser.sleep(1000);
        waitActions.waitForElementIsDisplayed(btn_Resubmit);
        externalRef = externalRefNum_BEQ.getText().then(function _onSuccess(text) {
            console.log(text);
            return text;
        })
        buttonActions.click(btn_Resubmit);
        buttonActions.click(OKButton);
        waitActions.waitForElementIsDisplayed(confMsg);
        confMsg.getText().then(function (text) {
            expect(text).toContain('Exception was resubmitted successfully');
        })
    }

    this.ValidateBEQResubmit = function () {
        waitActions.waitForElementIsDisplayed(beqGridhome);
        inputBoxActions.type(exceptionType, "Multiple_Match_Found");
        inputBoxActions.type(tenantColumn, "Air Traffic Control");
        buttonActions.click(includeResolvedCheckBox);
        buttonActions.click(searchIcon);
        inputBoxActions.type(ExternalRef, externalRef);
        buttonActions.click(searchIcon);
        dropDownActions.select(statusFilter, "New");
        waitActions.waitForElementIsDisplayed(firstRowGrid);
        dropDownActions.select(statusFilter, "Resolved");
        waitActions.waitForElementIsDisplayed(firstRowGrid);
    }

    this.ValidateCancelInUpdateFASTInfoPage = function()
    {
        buttonActions.click(updateFastInfo_Cancel);
        waitActions.waitForElementIsDisplayed();
    }
};
