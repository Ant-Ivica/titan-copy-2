module.exports = function () {
    'use strict';
    // Categories Page

    browser.waitForAngularEnabled(true);

    var cRepo = require('../resources/CategoriesObjRepo.json');


    var testData = require('../resources/testData.json');
    var objRepo = require('../resources/HomeObjRepo.json');
    var teqObjRepo = require('../resources/TechnicalExceptionQueuesObjRepo.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var constantsFile = require('../resources/constantsTower.json');
    //pages.Home = require('../pages/Home.js');    
    var DBpage = require('../utils/DBUtils.js');
    var DBUtil = new DBpage();
    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var verifyAction = require('../commons/verifyActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var mouseActions = require('../commons/mouseActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var gridActions = require('../commons/gridFilterActions.js');

    //var homePage = new pages.Home();
    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var verifyAction = new verifyAction();
    var inputBoxActions = new inputBoxActions();
    var mouseActions = new mouseActions();
    var dropDownActions = new dropDownActions();
    var gridActions = new gridActions();

    var categoryName_TB = objLocator.findLocator(cRepo.CategoriesObjRepo.categoryName_TB);
    var application_TB = objLocator.findLocator(cRepo.CategoriesObjRepo.application_TB);
    var tenant_Tb = objLocator.findLocator(cRepo.CategoriesObjRepo.tenant_Tb);
    var customers_Header = objLocator.findLocator(cRepo.CategoriesObjRepo.customers_Header);
    var categories_button = objLocator.findLocator(cRepo.CategoriesObjRepo.categories_button);
    var Mappingsheader = objLocator.findLocator(cRepo.CategoriesObjRepo.Mappings);
    var template = objLocator.findLocator(cRepo.CategoriesObjRepo.template);
    var appName = objLocator.findLocator(cRepo.CategoriesObjRepo.appName);
    var closeIcon1 = objLocator.findLocator(cRepo.CategoriesObjRepo.closeIcon1);
    var closeIcon2 = objLocator.findLocator(cRepo.CategoriesObjRepo.closeIcon2);
    var closeIcon3 = objLocator.findLocator(cRepo.CategoriesObjRepo.closeIcon3);
    var TenantName = objLocator.findLocator(cRepo.CategoriesObjRepo.TenantName);
    var addNewCategory = objLocator.findLocator(cRepo.CategoriesObjRepo.addNewCategory_Btn);
    var categoryName_Tb_Popup = objLocator.findLocator(cRepo.CategoriesObjRepo.categoryName_Tb_Popup);
    var save_btn = objLocator.findLocator(cRepo.CategoriesObjRepo.save_btn);
    var addNewCategory_Header = objLocator.findLocator(cRepo.CategoriesObjRepo.addNewCategory_Header);
    var createcategory = objLocator.findLocator(cRepo.CategoriesObjRepo.createcategory);
    var Delete = objLocator.findLocator(cRepo.CategoriesObjRepo.Delete);
    var defaultText = objLocator.findLocator(cRepo.CategoriesObjRepo.defaultText);
    var yes = objLocator.findLocator(cRepo.CategoriesObjRepo.yes);
    var confirmationMessage = objLocator.findLocator(cRepo.CategoriesObjRepo.confirmationMessage);
    var obdocIcon = objLocator.findLocator(cRepo.CategoriesObjRepo.obdocIcon);
    var addNewOBDButton = objLocator.findLocator(cRepo.CategoriesObjRepo.addNewOBDButton);
    var LVISDocumentType = objLocator.findLocator(cRepo.CategoriesObjRepo.LVISDocumentType);
    var Application = objLocator.findLocator(cRepo.CategoriesObjRepo.Application);
    var Service = objLocator.findLocator(cRepo.CategoriesObjRepo.Service);
    var ExternalDoctype = objLocator.findLocator(cRepo.CategoriesObjRepo.ExternalDoctype);
    var subscripIcon = objLocator.findLocator(cRepo.CategoriesObjRepo.subscripIcon);
    var addNewSubscriptionButton = objLocator.findLocator(cRepo.CategoriesObjRepo.addNewSubscriptionButton);
    var Application_Sub = objLocator.findLocator(cRepo.CategoriesObjRepo.Application_Sub);
    var MessageType_Sub = objLocator.findLocator(cRepo.CategoriesObjRepo.MessageType_Sub);
    var Save_Subs = objLocator.findLocator(cRepo.CategoriesObjRepo.Save_Subs);
    var confirmationMessage_subs = objLocator.findLocator(cRepo.CategoriesObjRepo.confirmationMessage_subs);
    var messageType_TB = objLocator.findLocator(cRepo.CategoriesObjRepo.messageType_TB);
    var Result = objLocator.findLocator(cRepo.CategoriesObjRepo.Result);
    var Result1 = objLocator.findLocator(cRepo.CategoriesObjRepo.Result1);
    var messageTypedesc_TB = objLocator.findLocator(cRepo.CategoriesObjRepo.messageTypedesc_TB);
    var subsTenant_TB = objLocator.findLocator(cRepo.CategoriesObjRepo.subsTenant_TB);
    var ExternalMessageType = objLocator.findLocator(cRepo.CategoriesObjRepo.ExternalMessageType);
    var AddSelectedMessageType = objLocator.findLocator(cRepo.CategoriesObjRepo.AddSelectedMessageType);
    var Result3 = objLocator.findLocator(cRepo.CategoriesObjRepo.Result3);
    var Save1 = objLocator.findLocator(cRepo.CategoriesObjRepo.Save1);
    var ConfMessage1 = objLocator.findLocator(cRepo.CategoriesObjRepo.ConfMessage1);
    var serviceTB = objLocator.findLocator(cRepo.CategoriesObjRepo.serviceTB);
    var selectedRow = objLocator.findLocator(cRepo.CategoriesObjRepo.selectedRow);
    var selectedRow1 = objLocator.findLocator(cRepo.CategoriesObjRepo.selectedRow1);
    var NotdeleteMessage = objLocator.findLocator(cRepo.CategoriesObjRepo.NotdeleteMessage);
    var ok = objLocator.findLocator(cRepo.CategoriesObjRepo.ok);
    var question = objLocator.findLocator(cRepo.CategoriesObjRepo.question);
    var ConfMessage2 = objLocator.findLocator(cRepo.CategoriesObjRepo.ConfMessage2);
    var EditSubscriptionDesc = objLocator.findLocator(cRepo.CategoriesObjRepo.EditSubscriptionDesc);
    var successMessage = objLocator.findLocator(cRepo.CategoriesObjRepo.successMessage);

    var testData = require('../resources/testData.json');

    this.openSearchPage = function (path) {
        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.isPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(Mappingsheader);
        buttonActions.click(Mappingsheader);
        waitActions.waitForElementIsDisplayed(customers_Header);
        buttonActions.click(categories_button);
        waitActions.waitForElementIsDisplayed(categoryName_TB);
        return this;
    };

    this.verifyFilterCategoryNameTextBox = function () {
        waitActions.waitForElementIsDisplayed(categoryName_TB);
        inputBoxActions.type(categoryName_TB, "keystone Template");
        waitActions.waitForElementIsDisplayed(template);
        return this;
    };

    this.verifyFilterApplicationTextBox = function () {
        waitActions.waitForElementIsDisplayed(template);
        buttonActions.click(closeIcon1);
        inputBoxActions.type(application_TB, "Elite");
        waitActions.waitForElementIsDisplayed(appName);
        return this;
    };

    this.verifyFilterTenantTextBox = function () {
        waitActions.waitForElementIsDisplayed(appName);
        buttonActions.click(closeIcon2);
        inputBoxActions.type(tenant_Tb, "Agency");
        buttonActions.click(closeIcon3);
        waitActions.waitForElementIsDisplayed(TenantName);
        return this;
    };

    this.verifyAddNewCategory = function () {
        if (testData.User.Role == "User")
            expect(addNewCategory.isPresent()).toBe(false);
        else {
            buttonActions.click(addNewCategory);
            waitActions.waitForElementIsDisplayed(addNewCategory_Header);
            inputBoxActions.type(categoryName_Tb_Popup, "BCategory");
            buttonActions.click(save_btn);
            inputBoxActions.type(categoryName_TB, "BCategory");
            waitActions.waitForElementIsDisplayed(createcategory);
            waitActions.waitForElementIsDisplayed(successMessage);
            successMessage.getText(function _onSuccess(text) {
                expect(text).toContain("added successfully");
            })
            return this;
        }
    };

    this.verfiyDeleteCategory = function () {
        inputBoxActions.type(categoryName_TB, "BCategory")
        browser.actions().doubleClick(createcategory).perform();
        browser.actions().doubleClick(createcategory).perform();
        waitActions.waitForElementIsDisplayed(Delete);
        if (testData.User.Role == "SuperAdmin") {
            buttonActions.click(Delete);
            waitActions.waitForElementIsDisplayed(yes);
            buttonActions.click(yes);
            waitActions.waitForElementIsDisplayed(confirmationMessage);
            confirmationMessage.getText(function _onSuccess(text) {
                expect(text).toContain("successfully");
            })
        }
        else
            expect(Delete.isEnabled()).toBe(false);
    };


    this.verifyOutboundDocumentIcon = function () {
        inputBoxActions.type(categoryName_TB, "BCategory")
        buttonActions.click(obdocIcon);
        if (testData.User.Role == "User")
            expect(addNewOBDButton.isPresent()).toBe(false);
        else {
            waitActions.waitForElementIsDisplayed(addNewOBDButton);
            buttonActions.click(addNewOBDButton);
            dropDownActions.selectDropdownbyNum(LVISDocumentType, 3);
            dropDownActions.selectDropdownbyNum(Application, 15);
            dropDownActions.selectDropdownbyNum(Service, 1);
            dropDownActions.selectDropdownbyNum(ExternalDoctype, 1);
            dropDownActions.selectDropdownbyNum(ExternalMessageType, 1);
            buttonActions.click(AddSelectedMessageType);
            waitActions.waitForElementIsDisplayed(Result3);
            buttonActions.click(Save1);
            inputBoxActions.type(serviceTB, "escrow");
            waitActions.waitForElementIsDisplayed(selectedRow);
            browser.actions().doubleClick(selectedRow).perform();
            browser.actions().doubleClick(selectedRow).perform();
            if (testData.User.Role == "SuperAdmin") {
                waitActions.waitForElementIsDisplayed(Delete);
                buttonActions.click(Delete);
                waitActions.waitForElementIsDisplayed(defaultText);
                buttonActions.click(yes);
                waitActions.waitForElementIsDisplayed(NotdeleteMessage);
                buttonActions.click(ok);
            }
            else
                expect(Delete.isEnabled()).toBe(false);
        }
        return this;
    };

    this.verifySubscriptionsIcon = function () {
        var desc;
        inputBoxActions.type(categoryName_TB, "BCategory")
        buttonActions.click(subscripIcon);
        if (testData.User.Role == "User")
            expect(addNewSubscriptionButton.isPresent()).toBe(false);
        else {
            waitActions.waitForElementIsDisplayed(addNewSubscriptionButton);
            buttonActions.click(addNewSubscriptionButton);
            dropDownActions.selectDropdownbyNum(Application_Sub, 5);
            dropDownActions.selectDropdownbyNum(MessageType_Sub, 1);
            screenshots.takeScreenshot("Save Subscriptions");
            buttonActions.click(Save_Subs);
            waitActions.waitForElementIsDisplayed(confirmationMessage_subs);
            confirmationMessage_subs.getText().then(function _onSuccess(text) {
                expect(text).toContain("added successfully");
            })
            inputBoxActions.type(messageTypedesc_TB, desc);
            screenshots.takeScreenshot("Filter Test");
            browser.actions().doubleClick(selectedRow1).perform();
            browser.actions().doubleClick(selectedRow1).perform();
            if (testData.User.Role == "SuperAdmin") {
                waitActions.waitForElementIsDisplayed(Delete);
                buttonActions.click(Delete);
                waitActions.waitForElementIsDisplayed(question);
                buttonActions.click(yes);
                waitActions.waitForElementIsDisplayed(ConfMessage2);
            }
            else
                expect(Delete.isEnabled()).toBe(false);
        }
        return this;
    };

    this.verifyMessageTypeNameTextBox = function () {
        buttonActions.click(subscripIcon);
        waitActions.waitForElementIsDisplayed(messageType_TB);
        inputBoxActions.type(messageType_TB, "comment");
        waitActions.waitForElementIsDisplayed(Result);
        return this;
    };

    this.verifyMessageTypeDescriptionTextBox = function () {
        buttonActions.click(subscripIcon);
        waitActions.waitForElementIsDisplayed(messageTypedesc_TB);
        inputBoxActions.type(messageTypedesc_TB, "comment");
        waitActions.waitForElementIsDisplayed(Result);
        return this;
    };

    this.verifyTenantTextBox = function () {
        buttonActions.click(subscripIcon);
        waitActions.waitForElementIsDisplayed(subsTenant_TB);
        inputBoxActions.type(subsTenant_TB, "lvis");
        waitActions.waitForElementIsDisplayed(Result1);
        return this;
    };

};

    // this.verify = function (
    // waitActions.waitForElementIsDisplayed(addedValue);   
    // )
    // //browser.actions().click().click(createcategory).perform().then(function(){
    // waitActions.waitForElementIsDisplayed(addedValue)