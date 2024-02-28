module.exports = function () {
    'use Strict';

    var objRepo = require('../resources/LVISToFASTDocumentsObjRepo.json');
    var taskMapObjRepo = require('../resources/TaskMapObjRepo.json');
    var utilspage = require('../utils/objectLocator.js');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var constantsFile = require('../resources/constantsTower.json');
    var buttonActions = require('../commons/buttonActions.js');
    var waitActions = require('../commons/waitActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var verifyActions = require('../commons/verifyActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var gridFilterActions = new gridFilterActions();
    var buttonActions = new buttonActions();
    var verifyActions = new verifyActions();
    var inputBoxActions = new inputBoxActions();
    var dropDownActions = new dropDownActions();

    //xpath
    var pagePath = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.pageLabel);
    var fast = objLocator.findLocator(taskMapObjRepo.TaskMapObjRepo.fastDropDown); 
    var lvisToFastLink = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.lvisToFastLink);
    var addNewDocLink = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.addNewDocLink);
    var addNewDocPopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.addNewDocinPopup);
    var docDescriptionPopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.docDescriptioninPopup);
    var lvisDocTypePopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.lvisDocTypeinPopup);
    var serviceinPopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.serviceinPopup);
    var fastDocTypeinPopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.fastDocTypeinPopup);
    var saveButtoninPopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.saveButtoninPopup);
    var successMessage = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.successMessage);
    var lvisDocType_result = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.lvisDocType_result);
    var lvisDocDes_result = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.lvisDocDes_result);
    var service_result = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.service_result);
    var fastDocType_result = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.fastDocType_result);
    var tenant_result = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.tenant_result);
    var lvisDocType_txt = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.lvisDocType_txt);
    var lvisDocDes_txt = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.lvisDocDes_txt);
    var service_txt = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.service_txt);
    var fastDocType_txt = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.fastDocType_txt);
    var tenant_txt = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.tenant_txt);
    var result_File = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.result_File);
    var editDocinPopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.editDocinPopup);
    var deleteButtoninPopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.deleteButtoninPopup);
    var yesinPopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.yesinPopup);
    var lvisDocType_close = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.lvisDocType_close);
    var lvisDocDes_close = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.lvisDocDes_close);
    var service_close = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.service_close);
    var fastDocType_close = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.fastDocType_close);
    var tenant_close = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.tenant_close);
    var cancelButtoninPopup = objLocator.findLocator(objRepo.fastToLvisDocObjRepo.cancelButtoninPopup);
    //Var Declarations
    var random; 

    this.navigateToLvisToFastDocPage = function(){

        buttonActions.click(fast);
        buttonActions.click(lvisToFastLink);
        waitActions.waitForElementIsDisplayed(pagePath);
        }

    this.isPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(pagePath);
        return this;
    };

    this.gridFilterByFieldName = function (FilterBy, Value) {
        if (FilterBy == "LvisDocType") {
            gridFilterActions.filter(lvisDocType_txt, Value, lvisDocType_result);
            screenshots.takeScreenshot('LvisDocType');
        }
        else if (FilterBy == "LvisDocDescription") {
            gridFilterActions.filter(lvisDocDes_txt, Value, lvisDocDes_result);
            screenshots.takeScreenshot('LvisDocDescription');

        }
        else if (FilterBy == "Service") {
            gridFilterActions.filter(service_txt, Value, service_result);
            screenshots.takeScreenshot('Service');
        }
        else if (FilterBy == "FastDocType") {
            gridFilterActions.filter(fastDocType_txt, Value, fastDocType_result);
            screenshots.takeScreenshot('FastDocType');
        }
        else if (FilterBy == "Tenant") {
            gridFilterActions.filter(tenant_txt, Value, tenant_result);
            screenshots.takeScreenshot('Tenant');
        }
    }

    this.clearFilter = function (Clear) {
        if (Clear == "LvisDocType") {
            buttonActions.click(lvisDocType_close);

        }
        else if (Clear == "LvisDocDescription") {
            buttonActions.click(lvisDocDes_close);
        }
        else if (Clear == "Service") {
            buttonActions.click(service_close);

        }
        else if (Clear == "FastDocType") {
           buttonActions.click(fastDocType_close);

        }
        else if (Clear == "Tenant") {
            browser.actions().mouseMove(tenant_close).perform();
            browser.actions().click().perform();


        }
        

    }

    getRandomNum = function(min, max){
        return parseInt(Math.random() * (max - min) + min);
    };

    this.addNewLvisToFastDocument = function(){
        buttonActions.click(addNewDocLink);
        screenshots.takeScreenshot("Add New Document Popup");
        waitActions.waitForElementIsDisplayed(addNewDocPopup);
        dropDownActions.selectDropdownbyValue(lvisDocTypePopup, "ALTA SETTLEMENT STATEMENT");
        random = getRandomNum(1,100);
        inputBoxActions.type(docDescriptionPopup, "Test"+random);
        console.log("Test"+random);
        dropDownActions.selectDropdownbyValue(serviceinPopup, "Escrow");
        dropDownActions.selectDropdownbyValue(fastDocTypeinPopup, "ALTA Settlement Buyer Only");
        buttonActions.click(saveButtoninPopup);
        
        successMessage.getText().then(function (successMsg) {
            expect(successMsg).toContain("added successfully");
        });
        screenshots.takeScreenshot("Added New Document Successfully");
    }


    this.verifyAddNewRecordOptionIsNotAvailable = function(){
        expect(addNewDocLink.isPresent()).toBe(false);

    }

    this.openExistingDocument = function(){
        console.log(random);
       this.gridFilterByFieldName("LvisDocDescription","Test"+random);
        browser
            .actions()
            .doubleClick(result_File)
            .perform();

        result_File.isDisplayed().then(function (res) {
            if (res) {

                browser.actions().doubleClick(result_File).perform();
            }
        });
        waitActions.waitForElementIsAvailable(editDocinPopup);


    }

    this.editExistingDocument=function(){
      
        dropDownActions.selectDropdownbyValue(serviceinPopup, "Title");
        buttonActions.click(saveButtoninPopup);
        
        successMessage.getText().then(function (successMsg) {
            expect(successMsg).toContain("updated successfully");
        });
        screenshots.takeScreenshot("Document Updated Successfully");

    }

    this.verifyEditDocIsDisabled = function(){
        buttonActions.click(cancelButtoninPopup);
        openExistingDocument();
        dropDownActions.selectDropdownbyValue(serviceinPopup, "Title");
        expect(element(by.xpath(saveButtoninPopup)).getAttribute('disabled')).toBe('true');
        buttonActions.click(cancelButtoninPopup);
    }

    this.verifyDeleteIsDisabled = function(){
        
        expect(element(deleteButtoninPopup).getAttribute('disabled')).toBe('true');
        buttonActions.click(cancelButtoninPopup);
    }

    this.verifyDeleteIsDisabledForUser = function(){
        this.gridFilterByFieldName("LvisDocDescription","QATowerTest_DontEdit");
        browser
            .actions()
            .doubleClick(result_File)
            .perform();

        result_File.isDisplayed().then(function (res) {
            if (res) {

                browser.actions().doubleClick(result_File).perform();
            }
        });
        waitActions.waitForElementIsAvailable(editDocinPopup);
        expect(element(deleteButtoninPopup).getAttribute('disabled')).toBe('true');
        buttonActions.click(cancelButtoninPopup);
    }
    

    this.deleteExistingDocument = function(){
        buttonActions.click(deleteButtoninPopup);
        buttonActions.click(yesinPopup);        
        successMessage.getText().then(function (successMsg) {
            expect(successMsg).toContain("deleted successfully");
        });
        screenshots.takeScreenshot("Document Deleted Successfully");
    }
}