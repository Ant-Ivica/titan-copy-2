module.exports = function () {
    'use Strict';

    // Utilities
    var homeObj = require('../resources/HomeObjRepo.json');
    var objAuditing = require('../resources/AuditingSummaryObjRepo.json');
    var taskMapObjRepo = require('../resources/TaskMapObjRepo.json');

    var screenshots = require('protractor-take-screenshots-on-demand');

    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var gridActions = require('../commons/gridFilterActions.js');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var dropDownActions = new dropDownActions();
    var inputBoxActions = new inputBoxActions();
    var gridActions = new gridActions();

    var checkUserName;

    var auditingtab = objLocator.findLocator(homeObj.homeObjRepo.auditingMenu);
    var auditingHeader = objLocator.findLocator(objAuditing.AuditingObjRepo.auditingheader);
    var dateFilter = objLocator.findLocator(objAuditing.AuditingObjRepo.dateFilter);
    var search = objLocator.findLocator(objAuditing.AuditingObjRepo.search);
var auditingmenu =objLocator.findLocator(objAuditing.AuditingObjRepo.auditingmenu);

    //Grid Filter Text Box
    var userName = objLocator.findLocator(objAuditing.AuditingObjRepo.userName);
    var acitivtyType = objLocator.findLocator(objAuditing.AuditingObjRepo.acitivtyType);
    var date = objLocator.findLocator(objAuditing.AuditingObjRepo.date);
    var section = objLocator.findLocator(objAuditing.AuditingObjRepo.section);
    var record = objLocator.findLocator(objAuditing.AuditingObjRepo.record);
    var property = objLocator.findLocator(objAuditing.AuditingObjRepo.property);
    var orignalValue = objLocator.findLocator(objAuditing.AuditingObjRepo.orignalValue);
    var newValue = objLocator.findLocator(objAuditing.AuditingObjRepo.newValue);


    //Grid 
    var rowDataUser = objLocator.findLocator(objAuditing.AuditingObjRepo.rowDataUser);
    var rowDataDate = objLocator.findLocator(objAuditing.AuditingObjRepo.rowDataDate);
    var rowDataActivityType = objLocator.findLocator(objAuditing.AuditingObjRepo.rowDataActivityType);
    var rowDataSection = objLocator.findLocator(objAuditing.AuditingObjRepo.rowDataSection);
    var rowDataRecord = objLocator.findLocator(objAuditing.AuditingObjRepo.rowDataRecord);
    var rowDataProperty = objLocator.findLocator(objAuditing.AuditingObjRepo.rowDataProperty);
    var rowDataOrignalValue = objLocator.findLocator(objAuditing.AuditingObjRepo.rowDataOrignalValue);
    var rowDataNewValue = objLocator.findLocator(objAuditing.AuditingObjRepo.rowDataNewValue);

    //Button
    var btnUser = objLocator.findLocator(objAuditing.AuditingObjRepo.btnUser);
    var btnDate = objLocator.findLocator(objAuditing.AuditingObjRepo.btnDate);
    var btnActivityType = objLocator.findLocator(objAuditing.AuditingObjRepo.btnActivityType);
    var btnSection = objLocator.findLocator(objAuditing.AuditingObjRepo.btnSection);
    var btnRecord = objLocator.findLocator(objAuditing.AuditingObjRepo.btnRecord);
    var btnProperty = objLocator.findLocator(objAuditing.AuditingObjRepo.btnProperty);
    var btnOriginal = objLocator.findLocator(objAuditing.AuditingObjRepo.btnOriginal);
    var btnNew = objLocator.findLocator(objAuditing.AuditingObjRepo.btnNew);

    //Popup
    var popupHeader = objLocator.findLocator(objAuditing.AuditingObjRepo.popupHeader);
    var popupUserName = objLocator.findLocator(objAuditing.AuditingObjRepo.popupUserName);
    var FirstRow = objLocator.findLocator(objAuditing.AuditingObjRepo.FirstEntry);
    var popupCloseButton = objLocator.findLocator(objAuditing.AuditingObjRepo.popupCloseButton);
    var popupActivityType = objLocator.findLocator(objAuditing.AuditingObjRepo.popupActivityType);
    var chktext;


    this.openSearchPage = function (path) {
        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.navigateToAuditingMenu = function () {
        
        waitActions.waitForElementIsDisplayed(auditingmenu);

        waitActions.waitForElementIsDisplayed(auditingtab);

        buttonActions.click(auditingtab);
        waitActions.waitForElementIsDisplayed(auditingHeader);
        browser.sleep(500);
        waitActions.waitForElementIsDisplayed(rowDataUser);
        screenshots.takeScreenshot('Auditing- Success Loading');
    }

    this.filterTests = function () {
        dropDownActions.selectDropdownbyNum(dateFilter, 2);
        buttonActions.click(search);
        waitActions.waitForElementIsDisplayed(search);
        screenshots.takeScreenshot('Auditing- Success Filter Test for 60 Days');
        //gridActions.filter(userName, "INTL\\BDanti", rowDataUser);
        //waitActions.waitForElementIsDisplayed(rowDataUser);
    }


    this.gridFilterByFieldName = function (FilterBy) {
        if (FilterBy == "User") {
            gridActions.filter(userName, "INTL\\BDanti", rowDataUser);
        }
        else if (FilterBy == "Date") {
            var todayDate = new Date().add({ days: -1 });
            gridActions.filter(date, todayDate.getMonth() + 1 + '/' + todayDate.getDate() + '/' + todayDate.getFullYear(), rowDataDate);
        }
        else if (FilterBy == "ActivityType") {
            gridActions.filter(acitivtyType, "LogIn", rowDataActivityType);
        }
        else if (FilterBy == "Section") {
            gridActions.filter(section, "Security", rowDataSection);
        }
        else if (FilterBy == "Record") {
            gridActions.filter(record, "INTL\\BDanti", rowDataRecord);
        }
        else if (FilterBy == "Property") {
            gridActions.filter(property, "Authorize", rowDataProperty);
        }
        else if (FilterBy == "OrignalValue") {
            gridActions.filter(orignalValue, "SuperAdmin", rowDataOrignalValue);
        }
        else if (FilterBy == "NewValue") {
            gridActions.filter(newValue, "Admin", rowDataNewValue);
        }
    }

    this.clearFilter = function (Clear) {
        if (Clear == "User")
            buttonActions.click(btnUser);
        else if (Clear == "Date")
            buttonActions.click(btnDate);
        else if (Clear == "ActivityType")
            buttonActions.click(btnActivityType);
        else if (Clear == "Section")
            buttonActions.click(btnSection);
        else if (Clear == "Record")
            buttonActions.click(btnRecord);
        else if (Clear == "Property")
            buttonActions.click(btnProperty);
        else if (Clear == "OrignalValue")
            buttonActions.click(btnOriginal);
        else if (Clear == "NewValue")
            buttonActions.click(btnNew);
    }

    this.validatedata = function () {
        console.log("Validate Data");
        gridActions.filter(userName, "INTL\\BDanti", rowDataUser);
        browser.actions().doubleClick(FirstRow).perform();
        browser.actions().doubleClick(FirstRow).perform();
        waitActions.waitForElementIsDisplayed(popupHeader);
        checkUserName = popupUserName.getText().then(function _onSuccess(text) {
            chktext = text.toString().search("BDanti");
            console.log(chktext);
            return text;
        });
        screenshots.takeScreenshot('Auditing: PopUp test');
        buttonActions.click(popupCloseButton);
        //this.clearFilter("User");
        return this;
    }
}