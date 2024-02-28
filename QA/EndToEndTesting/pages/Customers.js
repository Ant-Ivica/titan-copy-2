module.exports = function () {

    'use Strict';
    // Mapping Tables - Customers
    var objRepo = require('../resources/CustomersObjRepo.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    //common - objects
    var testData = require('../resources/testData.json');
    var utilspage = require('../utils/objectLocator.js');
    var objLocator = new utilspage();
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    //var verifyAction = require('../commons/verifyActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var checkBoxActions = require('../commons/checkBoxActions.js');
    var inputBoxActions = new inputBoxActions();
    var buttonActions = new buttonActions();
    var waitActions = new waitActions();
    var dropDownActions = new dropDownActions();
    var checkBoxActions = new checkBoxActions();
    //Customers - Objects
    //var customersTitle = objLocator.findLocator(objRepo.customersObjRepo.customersTitle);
    //var customerGrid = objLocator.findLocator(objRepo.customersObjRepo.customerGrid);car
    var homeTab = objLocator.findLocator(objRepo.customersObjRepo.homeTab);
    //var customerPage = objLocator.findLocator(objRepo.customersObjRepo.mappingTables);
    //var mappingTables = objLocator.findLocator(objRepo.customersObjRepo.mappingTables);
    var locationIcon = objLocator.findLocator(objRepo.customersObjRepo.locations);
    var addNewCustomer = objLocator.findLocator(objRepo.customersObjRepo.addNewCustomer);
    var customerPreferences = objLocator.findLocator(objRepo.customersObjRepo.customerPreferences);
    //var servicePreference = objLocator.findLocator(objRepo.customersObjRepo.servicePreference);
    var customerName = objLocator.findLocator(objRepo.customersObjRepo.customerName);
    var category = objLocator.findLocator(objRepo.customersObjRepo.category);
    var application = objLocator.findLocator(objRepo.customersObjRepo.application);
    var save = objLocator.findLocator(objRepo.customersObjRepo.save);
    var successMsgNewCustomer = objLocator.findLocator(objRepo.customersObjRepo.successMsgNewCustomer);
    var resultRowCustID = objLocator.findLocator(objRepo.customersObjRepo.resultRowCustID);
    var resultRowCustName = objLocator.findLocator(objRepo.customersObjRepo.resultRowCustName);
    var resultRowCategory = objLocator.findLocator(objRepo.customersObjRepo.resultRowCategory);
    var resultRowApplication = objLocator.findLocator(objRepo.customersObjRepo.resultRowApplication);
    var resultRowTenant = objLocator.findLocator(objRepo.customersObjRepo.resultRowTenant);
    var resultRow = objLocator.findLocator(objRepo.customersObjRepo.resultRow);
    var filterByCustomerID = objLocator.findLocator(objRepo.customersObjRepo.filterByCustomerID);
    var filterByCustomerName = objLocator.findLocator(objRepo.customersObjRepo.filterByCustomerName);
    var filterByCategory = objLocator.findLocator(objRepo.customersObjRepo.filterByCategory);
    var filterByApplication = objLocator.findLocator(objRepo.customersObjRepo.filterByApplication);
    var filterByTenant = objLocator.findLocator(objRepo.customersObjRepo.filterByTenant);
    var customerIDCancel = objLocator.findLocator(objRepo.customersObjRepo.customerIDCancel);
    var customerNameCancel = objLocator.findLocator(objRepo.customersObjRepo.customerNameCancel);
    var categoryCancel = objLocator.findLocator(objRepo.customersObjRepo.categoryCancel);
    var applicationCancel = objLocator.findLocator(objRepo.customersObjRepo.applicationCancel);
    var tenantCancel = objLocator.findLocator(objRepo.customersObjRepo.tenantCancel);
    var customersContactProvider = objLocator.findLocator(objRepo.customersObjRepo.customersContactProvider);
    var newCustomer = objLocator.findLocator(objRepo.customersObjRepo.newCustomer);
    var Delete = objLocator.findLocator(objRepo.customersObjRepo.Delete);
    var yesBtn = objLocator.findLocator(objRepo.customersObjRepo.yesBtn);
    var confirmationMessage = objLocator.findLocator(objRepo.customersObjRepo.confirmationMessage);
    var confirmDelete = objLocator.findLocator(objRepo.customersObjRepo.confirmDelete);
    var editCustomer = objLocator.findLocator(objRepo.customersObjRepo.editCustomer);
    var editNewCustomerSuccess = objLocator.findLocator(objRepo.customersObjRepo.editNewCustomerSuccess);
    var cancel = objLocator.findLocator(objRepo.customersObjRepo.cancel);
    var webhookSubscriptionsButton = objLocator.findLocator(objRepo.customersObjRepo.webhookSubscriptions);
    var addwebhookButton = objLocator.findLocator(objRepo.customersObjRepo.addwebhook);
    var webhooksHeader = objLocator.findLocator(objRepo.customersObjRepo.webhooksHeader)
    var addwebhookcloseButton = objLocator.findLocator(objRepo.customersObjRepo.close);
    var addwebhookDeleteButton = objLocator.findLocator(objRepo.customersObjRepo.Delete);
    var addwebhookSaveButton = objLocator.findLocator(objRepo.customersObjRepo.addwebhook);
    var userIdColumn = objLocator.findLocator(objRepo.customersObjRepo.userIdColumn);
    var urlColumn = objLocator.findLocator(objRepo.customersObjRepo.urlColumn);
    var secretColumn = objLocator.findLocator(objRepo.customersObjRepo.secretColumn);
    var actionTypeColumn = objLocator.findLocator(objRepo.customersObjRepo.actionTypeColumn);
    var activeColumn = objLocator.findLocator(objRepo.customersObjRepo.activeColumn);
    var maxAttempsColumn = objLocator.findLocator(objRepo.customersObjRepo.maxAttempsColumn);
    var addNewWebhookHeader = objLocator.findLocator(objRepo.customersObjRepo.addNewWebhookHeader);
    var editWebhookHeader = objLocator.findLocator(objRepo.customersObjRepo.editWebhookHeader);
    var userIdTextBox = objLocator.findLocator(objRepo.customersObjRepo.userIdTextBox);
    var urlTextBox = objLocator.findLocator(objRepo.customersObjRepo.urlTextBox);
    var secretTextBox = objLocator.findLocator(objRepo.customersObjRepo.secretTextBox);
    var actionTypeDropdown = objLocator.findLocator(objRepo.customersObjRepo.actionTypeDropdown);
    var activeCheckbox = objLocator.findLocator(objRepo.customersObjRepo.activeCheckbox);
    var maxAttempsDropdown = objLocator.findLocator(objRepo.customersObjRepo.maxAttempsDropdown);
    var successMsgNewWebhook = objLocator.findLocator(objRepo.customersObjRepo.sucessMsgAddWebhook);
    var sucessMsgeditWebhook = objLocator.findLocator(objRepo.customersObjRepo.sucessMsgeditWebhook);
    var sucessMsgdeleteWebhook = objLocator.findLocator(objRepo.customersObjRepo.sucessMsgdeleteWebhook);
    var webhookSubscriptionGridelements = [userIdColumn, urlColumn, secretColumn, actionTypeColumn, activeColumn, maxAttempsColumn];
    var webhookSubscriptionAddelements = [addNewWebhookHeader, userIdTextBox, urlTextBox, secretTextBox, actionTypeDropdown, activeCheckbox, maxAttempsDropdown,
        addwebhookSaveButton, cancel, addwebhookcloseButton];
    var createCredential = objLocator.findLocator(objRepo.customersObjRepo.createCredential);
    var CustomerUserId = objLocator.findLocator(objRepo.customersObjRepo.CustomerUserId);

    this.openSearchPage = function (path) {

        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    }

    this.clickOnLocationIcon = function () {
        buttonActions.click(locationIcon);

        return this;
    }

    this.clickOnContactProvidersIcon = function () {
        buttonActions.click(customersContactProvider);
        return this;
    }

    this.isCustomerPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(locationIcon);
        return this;
    }

    this.clickOnHomeTab = function () {
        buttonActions.click(homeTab);
        return this;

    }

    this.clickOnAddNewCustomer = function () {
        buttonActions.click(addNewCustomer);
        waitActions.waitForElementIsDisplayed(customerPreferences);
        return this;
    }

    this.isAddNewCustomerAvailable = function () {
        expect(addNewCustomer.isPresent()).toBe(false);
    }


    this.isDeleteCustomerDisabled = function () {
        waitActions.waitForElementIsDisplayed(Delete);
        Delete.getAttribute('disabled').then(function (text) {
            expect(text).toBe('true');
            browser.actions().mouseMove(save).perform();
            screenshots.takeScreenshot();
        })
    }
    this.enterAddNewCustomerData = function () {
        inputBoxActions.type(customerName, "ProcCustomer");
        dropDownActions.selectDropdownbyNum(category, 1);
        dropDownActions.selectDropdownbyNum(application, 10);
        buttonActions.click(save);
        successMsgNewCustomer.getText().then(function (text) {
            console.log(text);
            screenshots.takeScreenshot('successMsgNewCustomer');
        });
    }

    this.openAPICustomerAddCredential = function () {            
        inputBoxActions.type(customerName, "ProcCustomer");
        dropDownActions.selectDropdownbyNum(category, 1);
        browser.sleep(3000);
        dropDownActions.selectDropdownbyNum(application, 10);
        checkBoxActions.click(createCredential);
        var userCredential = CustomerUserId.getAttribute('value')
        console.log(userCredential); 
        screenshots.takeScreenshot('createCredential');
    }

    this.filterByFieldName = function (FilterBy) {
        if (FilterBy == "CustomerID") {
            inputBoxActions.type(filterByCustomerID, 10001);
            waitActions.waitForElementIsDisplayed(resultRowCustID);
            return this;
        }

        else if (FilterBy == "CustomerName") {
            inputBoxActions.type(filterByCustomerName, "ProcCustomer");
            waitActions.waitForElementIsDisplayed(resultRowCustName);
            return this;
        }
        else if (FilterBy == "Category") {
            inputBoxActions.type(filterByCategory, 3234234);
            waitActions.waitForElementIsDisplayed(resultRowCategory);
            return this;
        }
        else if (FilterBy == "Application") {
            inputBoxActions.type(filterByApplication, "RealEC");
            waitActions.waitForElementIsDisplayed(resultRowApplication);
            return this;
        }
        else if (FilterBy == "Tenant") {
            inputBoxActions.type(filterByTenant, "RLA");
            waitActions.waitForElementIsDisplayed(resultRowTenant);
            return this;
        }
    }
    this.filterByInvalidData = function (FilterBy) {
        if (FilterBy == "CustomerID")
            inputBoxActions.type(filterByCustomerID, 100000)
        console.log(resultRowTenant);
        console.log("No records to display");
        return this;
    }

    this.clearFilter = function (Cancel) {
        if (Cancel == "CustomerID")
            buttonActions.click(customerIDCancel);
        else if (Cancel == "CustomerName")
            buttonActions.click(customerNameCancel);
        else if (Cancel == "Category")
            buttonActions.click(categoryCancel);
        else if (Cancel == "Application")
            buttonActions.click(applicationCancel);
        else if (Cancel == "Tenant")
            buttonActions.click(tenantCancel);

    }

    this.openNewRecord = function () {
        inputBoxActions.type(filterByCustomerName, "ProcCustomer");
        waitActions.waitForElementIsDisplayed(newCustomer);
        browser.actions().doubleClick(newCustomer).perform();
        browser.actions().doubleClick(newCustomer).perform();
        newCustomer.isDisplayed().then(function (text) {
            if (text) {
                browser.actions().doubleClick(newCustomer).perform();
            }
        })

        return this;

    }

    this.deleteNewRecord = function () {
        waitActions.waitForElementIsDisplayed(Delete);
        if (testData.User.Role === "SuperAdmin") {
            buttonActions.click(Delete);
            browser.sleep(1000);
            waitActions.waitForElementIsDisplayed(confirmDelete);
            buttonActions.click(yesBtn);
            waitActions.waitForElementIsDisplayed(confirmationMessage);
        }
        else if (testData.User.Role === "Admin" || testData.User.Role === "User") {
            Delete.getAttribute('disabled').then(function (text) {
                expect(text).toBe('true');
                browser.actions().mouseMove(cancel).perform();
            })
        }

        return this;
    }

    this.updateNewRecord = function () {
        waitActions.waitForElementIsDisplayed(editCustomer);
        dropDownActions.selectDropdownbyNum(application, 10);
        if (testData.User.Role === "Admin" || testData.User.Role === "SuperAdmin") {
            buttonActions.click(save);
            editNewCustomerSuccess.getText().then(function (text) {
                console.log(text);
                screenshots.takeScreenshot('editNewCustomerSuccess');
            });
        }
        else if (testData.User.Role === "User") {
            save.getAttribute('disabled').then(function (text) {
                expect(text).toBe('true');
            })
        }


    }

    this.areWebhookGridElementsPresent = function () {
        webhookSubscriptionGridelements.forEach(Element => {
            this.isElementPresent(Element);
            return this;
        });
    }

    this.areWebhookpageElementsPresent = function () {
        webhookSubscriptionAddelements.forEach(Element => {
            this.isElementPresent(Element);
            return this;
        });
    }

    this.openWebhookPage = function openWebhookPage() {
        inputBoxActions.type(filterByTenant, "Air Traffic Control");
        inputBoxActions.type(filterByCustomerName, "TestCustomer");
        waitActions.waitForElementIsDisplayed(resultRowTenant);
        buttonActions.click(webhookSubscriptionsButton);
        waitActions.waitForElementIsDisplayed(webhooksHeader);
    }

    this.verifyWebhookSubscriptionFiledsDisplayed = function verifyWebhookSubscriptionFiledsDisplayed() {
        if (testData.User.Role === "Admin" || testData.User.Role === "SuperAdmin") {
            //Verify Add New webhook Button displayed
            expect(addwebhookButton.isDisplayed()).toBe(true);
            // this.areWebhookGridElementsPresent();
            // //Click on Add New webhook Button
            // buttonActions.click(addwebhookButton);
            // //Verify all fields displayed
            // this.areWebhookPageElementsPresent();
            // //Click on Cancel Button
            // buttonActions.click(cancel);
            // //Verify pop up closed
            // expect(addNewWebhookHeader.isDisplayed()).toBe(false);
            // browser.actions().doubleClick(resultRowTenant).perform();
            // //Verify all fields displayed
            // waitActions.waitForElementIsDisplayed(addwebhookDeleteButton);
            // //Click on Cancel Button
            // buttonActions.click(addwebhookcloseButton);
            // //Verify pop up closed
            // expect(editWebhookHeader.isDisplayed()).toBe(false);
        }

        else if (testData.User.Role === "User") {
            //Verify Add New webhook Button not displayed
            addwebhookButton.isDisplayed().expect(false);
            this.areWebhookGridElementsPresent();
        }
    }

    this.addNewWebhook = function addNewWebhook() {
        buttonActions.click(addwebhookButton);
        inputBoxActions.type(userIdTextBox, "TestUserAuto");
        inputBoxActions.type(urlTextBox, "http://TestUrlAuto.com/");
        inputBoxActions.type(secretTextBox, "TestSecretAuto1234");
        dropDownActions.selectDropdownbyValue(actionTypeDropdown, "OrderCreated");
        //dropDownActions.selectDropdownbyValue(maxAttempsDropdown, "2");
        checkBoxActions.click(activeCheckbox);
        buttonActions.click(save);
        successMsgNewWebhook.getText().then(function (text) {
            console.log(text);
            screenshots.takeScreenshot('successMsgNewWebhook');
        });
    }

    this.editWebhook = function editWebhook() {
        browser.actions().doubleClick(resultRow).perform();
        inputBoxActions.type(userIdTextBox, "TestUserAutoUpdate");
        inputBoxActions.type(urlTextBox, "http://TestUrlAutoUpdate.com/");
        inputBoxActions.type(secretTextBox, "TestSecretAutoUpdate4567");
        dropDownActions.selectDropdownbyValue(actionTypeDropdown, "CurativeInfoPending");
        //dropDownActions.selectDropdownbyValue(maxAttempsDropdown, "3");
        checkBoxActions.click(activeCheckbox);
        buttonActions.click(save);
        sucessMsgeditWebhook.getText().then(function (text) {
            console.log(text);
            screenshots.takeScreenshot('sucessMsgeditWebhook');
        });
    }
    this.deleteWebhook = function deleteWebhook() {
        browser.actions().doubleClick(resultRow).perform();
        buttonActions.click(Delete);
        sucessMsgdeleteWebhook.getText().then(function (text) {
            console.log(text);
            screenshots.takeScreenshot('sucessMsgdeleteWebhook');
        });
    }

    this.cancelNewWebhook = function cancelNewWebhook() {
        buttonActions.click(addwebhookButton);
        buttonActions.click(cancel);
        expect(addNewWebhookHeader.isDisplayed()).toBe(true);
    }

    this.cancelEditWebhook = function cancelEditWebhook() {
        browser.actions().doubleClick(resultRow).perform();
        buttonActions.click(cancel);
        expect(editWebhookHeader.isDisplayed()).toBe(true);
    }

}
