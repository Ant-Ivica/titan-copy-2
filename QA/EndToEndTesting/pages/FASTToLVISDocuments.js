module.exports = function () {
    'use Strict';

    var objRepo = require('../resources/FASTToLVISDocumentsObjRepo.json');
    var taskMapObjRepo = require('../resources/TaskMapObjRepo.json');
    var tesObjRepo = require('../resources/testData.json');

    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var verifyActions = require('../commons/verifyActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var mouseActions = require('../commons/mouseActions.js');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var verifyActions = new verifyActions();
    var inputBoxActions = new inputBoxActions();
    var dropDownActions = new dropDownActions();
    var gridFilterActions = new gridFilterActions();
    var mouseActions = new mouseActions();

    var screenshots = require('protractor-take-screenshots-on-demand');

    //Navigate to Fast To Lvis Documents xpath
    var fast = objLocator.findLocator(taskMapObjRepo.TaskMapObjRepo.fastDropDown);
    var fastToLvisDoc = objLocator.findLocator(objRepo.fastToLvisObjRepo.fastToLvisDoc);

    //Verify The Page Load and Add New Fast To LVIS
    var pageLabel = objLocator.findLocator(objRepo.fastToLvisObjRepo.pageLabel);
    var addNewFastToLVIS = objLocator.findLocator(objRepo.fastToLvisObjRepo.addNewFastToLVIS);

    //Filters
    var fastDocType = objLocator.findLocator(objRepo.fastToLvisObjRepo.fastDocType);
    var fastDocDesc = objLocator.findLocator(objRepo.fastToLvisObjRepo.fastDocDesc);
    var service = objLocator.findLocator(objRepo.fastToLvisObjRepo.service);
    var lvisDocType = objLocator.findLocator(objRepo.fastToLvisObjRepo.lvisDocType);
    var tenant = objLocator.findLocator(objRepo.fastToLvisObjRepo.tenant);
    var delfastDocType = objLocator.findLocator(objRepo.fastToLvisObjRepo.delfastDocType);
    var delfastDocDesc = objLocator.findLocator(objRepo.fastToLvisObjRepo.delfastDocDesc);
    var delService = objLocator.findLocator(objRepo.fastToLvisObjRepo.delService);
    var dellvisDocType = objLocator.findLocator(objRepo.fastToLvisObjRepo.dellvisDocType);
    var delTenant = objLocator.findLocator(objRepo.fastToLvisObjRepo.delTenant);

    //Result row 
    var row_fastDocType = objLocator.findLocator(objRepo.fastToLvisObjRepo.row_fastDocType);
    var row_fastDocDesc = objLocator.findLocator(objRepo.fastToLvisObjRepo.row_fastDocDesc);
    var row_Service = objLocator.findLocator(objRepo.fastToLvisObjRepo.row_Service);
    var row_lvisDocType = objLocator.findLocator(objRepo.fastToLvisObjRepo.row_lvisDocType);
    var row_Tenant = objLocator.findLocator(objRepo.fastToLvisObjRepo.row_Tenant);
    var FirstRow = objLocator.findLocator(objRepo.fastToLvisObjRepo.FirstRow);
    //Add New Fast To LVIS Documents
    var addHeaderFastToLvis = objLocator.findLocator(objRepo.fastToLvisObjRepo.addHeaderFastToLvis);
    var addFastDocType = objLocator.findLocator(objRepo.fastToLvisObjRepo.addFastDocType);
    var addFastDocDesc = objLocator.findLocator(objRepo.fastToLvisObjRepo.addFastDocDesc);
    var addService = objLocator.findLocator(objRepo.fastToLvisObjRepo.addService);
    var addLvisDocType = objLocator.findLocator(objRepo.fastToLvisObjRepo.addLvisDocType);
    var addSave = objLocator.findLocator(objRepo.fastToLvisObjRepo.addSave);
    var addCancel = objLocator.findLocator(objRepo.fastToLvisObjRepo.addCancel);
    var successAddFASTToLVIS = objLocator.findLocator(objRepo.fastToLvisObjRepo.successAddFASTToLVIS);

    //Delete
    var pageLabel = objLocator.findLocator(objRepo.fastToLvisObjRepo.pageLabel);
    var fast = objLocator.findLocator(taskMapObjRepo.TaskMapObjRepo.fastDropDown);
    var deleteButton = objLocator.findLocator(objRepo.fastToLvisObjRepo.deleteButton);
    var editFastToLVISLabel = objLocator.findLocator(objRepo.fastToLvisObjRepo.editFastToLVISLabel);
    var confirmDeleteMsg = objLocator.findLocator(objRepo.fastToLvisObjRepo.confirmDeleteMsg);
    var confirmDeleteYes = objLocator.findLocator(objRepo.fastToLvisObjRepo.confirmDeleteYes);
    var confirmDeleteNo = objLocator.findLocator(objRepo.fastToLvisObjRepo.confirmDeleteNo);
    var deleteFastToLVISSuccess = objLocator.findLocator(objRepo.fastToLvisObjRepo.deleteFastToLVISSuccess);

    //Update
    var updateFastToLVIS = objLocator.findLocator(objRepo.fastToLvisObjRepo.updateFastToLVIS);
    var updateFastToLVISSucess = objLocator.findLocator(objRepo.fastToLvisObjRepo.updateFastToLVISSucess);
    //Var Declarations
    var random;

    this.openSearchPage = function (path) {
        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.navigateToFastToLVISDocuments = function () {
        buttonActions.click(fast);
        buttonActions.click(fastToLvisDoc);
        //waitActions.waitForElementIsDisplayed(pageLabel);
        waitActions.waitForElementIsDisplayed(row_fastDocType);
    }

    this.isPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(pagePath);
        return this;
    };

    this.gridFilterByFieldName = function (FilterBy) {
        if (FilterBy == "fastDocType") {
            gridFilterActions.filter(fastDocType, "Fee List", row_fastDocType);
        }
        else if (FilterBy == "fastDocDesc") {
            gridFilterActions.filter(fastDocDesc, "AutomationQA", row_fastDocDesc);
        }
        else if (FilterBy == "Service") {
            gridFilterActions.filter(service, "Title", row_Service);
        }
        else if (FilterBy == "lvisDocType") {
            gridFilterActions.filter(lvisDocType, "Title Commitment", row_lvisDocType);
        }
        else if (FilterBy == "tenant") {
            gridFilterActions.filter(tenant, "MortgageSolutions", row_Tenant);
        }
    };

    this.filterByInvalidData = function (FilterBy) {
        if (FilterBy == "CustomerID")
            inputBoxActions.type(filterByCustomerID, 100000)
        console.log("No records to display");
        return this;
    };

    this.clearFilter = function (Clear) {
        if (Clear == "fastDocType")
            buttonActions.click(delfastDocType);
        else if (Clear == "fastDocDesc")
            buttonActions.click(delfastDocDesc);
        else if (Clear == "Service")
            buttonActions.click(delService);
        else if (Clear == "lvisDocType")
            buttonActions.click(dellvisDocType);
        else if (Clear == "tenant") {
            browser.actions().mouseMove(delTenant).perform();
            browser.actions().click().perform();
        }
    };

    this.addFastToLVISDocument = function () {
        buttonActions.click(addNewFastToLVIS);
        waitActions.waitForElementIsDisplayed(addHeaderFastToLvis);
        screenshots.takeScreenshot("Add Fast To LVIS Document Pop Up");
        dropDownActions.selectDropdownbyValue(addFastDocType, "Fee List");
        random = getRandomNum(1, 100);
        inputBoxActions.type(addFastDocDesc, "AutomationQA " + random);
        console.log("Test" + random);
        dropDownActions.selectDropdownbyValue(addService, "Title");
        dropDownActions.selectDropdownbyValue(addLvisDocType, "Closing Disclosure");
        buttonActions.click(addSave);
        waitActions.waitForElementIsDisplayed(successAddFASTToLVIS);
        successAddFASTToLVIS.getText().then(function (successMsg) {
            expect(successMsg).toContain("added successfully");
        });
        screenshots.takeScreenshot("Added New Fast To LVIS Document Successfully");
    }

    this.verifyAddFastToLVISDocumentPresent = function () {
        // expect(addNewFastToLVIS.isPresent()).toBe(false);
        screenshots.takeScreenshot("Permission Denied for User to UPDATE");

    }

    this.updateFastToLVISDocuments = function () {
        this.gridFilterByFieldName("fastDocType");
        this.gridFilterByFieldName("fastDocDesc");
        browser.actions().doubleClick(FirstRow).perform();
        FirstRow.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(FirstRow).perform();
            }
        });

        waitActions.waitForElementIsAvailable(editFastToLVISLabel);
    }

    this.verifyUpdateFastToLVISDocumentDisabled = function () {
        updateFastToLVIS.getAttribute('disbaled').then(function onDisbaled() {
            screenshots.takeScreenshot("Permission Denied for User to UPDATE");
            buttonActions.click(addCancel);
        });
    }

    this.verifyUpdateFastToLVISDocumentEnabled = function () {
        var i = 0;
        inputBoxActions.type(addFastDocDesc, "AutomationQA Update " + random);
        browser.sleep(1000);
        updateFastToLVIS.getAttribute('disbaled').then(function onDisbaled() {
            i++;
            console.log("Please update the FAST To LVIS Document")
            screenshots.takeScreenshot("Their is no modification to Save");
        });
        if (i === 0) {
            buttonActions.click(updateFastToLVIS);
            waitActions.waitForElementIsAvailable(updateFastToLVISSucess);
            mouseActions.mouseMove(updateFastToLVISSucess);
            updateFastToLVISSucess.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Fast To LVIS Document Updated Successfully");
        }
    }


    this.deleteFastToLVISDocuments = function () {
        this.gridFilterByFieldName("fastDocType");
        this.gridFilterByFieldName("fastDocDesc");
        row_fastDocType.isDisplayed().then(function (cc) {
            if (cc) {
                browser.actions().doubleClick(row_fastDocType).perform();
                row_fastDocType.isDisplayed().then(function (res) {
                    if (res) {
                        browser.actions().doubleClick(row_fastDocType).perform();
                    }
                })
            }
            else {
                console.log("Their is no filter Document is present");
            }
        });
        // browser.actions().doubleClick(FirstRow).perform();
        // FirstRow.isDisplayed().then(function (res) {
        //     if (res) {
        //         browser.actions().doubleClick(FirstRow).perform();
        //     }
        // });
        waitActions.waitForElementIsAvailable(editFastToLVISLabel);
    }

    this.verifyDeleteFastToLVISDocumentDisbaled = function () {
        deleteButton.getAttribute('disbaled').then(function () {
            console.log("This User Role doesn't have the Permission to Deleting Document");
            screenshots.takeScreenshot("Permission Denied for User to DELELTE")
            buttonActions.click(addCancel);
        });
    }

    this.verifyDeleteFastToLVISDocumentEnabled = function () {
        editFastToLVISLabel.isDisplayed().then(function (deleteFa) {
            if (deleteFa) {
                buttonActions.click(deleteButton);
                waitActions.waitForElementIsAvailable(confirmDeleteMsg);
                buttonActions.click(confirmDeleteYes);
                waitActions.waitForElementIsAvailable(deleteFastToLVISSuccess);
                mouseActions.mouseMove(deleteFastToLVISSuccess);
                deleteFastToLVISSuccess.getText().then(function (successMsg) {
                    expect(successMsg).toContain("deleted successfully");
                });
                screenshots.takeScreenshot("Fast To LVIS Document Deleted Successfully");
            }
            else {
                console.log("No Doument is present to Delete")
            }
        })

    }


    // this.verifySaveButtonEnabled = function () {
    //     addApproveButton.getAttribute('disbaled').then(function () {
    //         console.log("User is not authorized for updating");
    //     });
    // }

    // this.testDocuments = function () {
    //     var webElement = tesObjRepo.User.Tenant;
    //     console.log(tesObjRepo.User.Tenant);
    //     tesObjRepo.User.Tenant = "Agency"
    //     //console.log(webElement);
    //     //webElement.sendKeys('Agency');
    //     console.log(tesObjRepo.User.Tenant);
    // }
}

getRandomNum = function (min, max) {
    return parseInt(Math.random() * (max - min) + min);
};


