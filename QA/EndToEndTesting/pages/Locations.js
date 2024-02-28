module.exports = function () {

    'use Strict';
    var testData = require('../resources/testData.json');
    //var objRepo = require('../resources/CustomersObjRepo.json');
    //var custObjRepo = require('../resources/CustomersObjRepo.json');
    var objRepo = require('../resources/LocationsObjRepo.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    //common - objects
    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var verifyAction = require('../commons/verifyActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var verifyAction = new verifyAction();
    var inputBoxActions = new inputBoxActions();
    var dropDownActions = new dropDownActions();
    //Locations - Objects
    //var locationIcon = objLocator.findLocator(objRepo.customersObjRepo.Locations);
    var addNewLocation = objLocator.findLocator(objRepo.locationsObjRepo.addNewLocation);
    var externalID = objLocator.findLocator(objRepo.locationsObjRepo.externalID);
    var locationName = objLocator.findLocator(objRepo.locationsObjRepo.locationName);
    var servicePreference = objLocator.findLocator(objRepo.locationsObjRepo.servicePreference);
    var servicePreferenceOption = objLocator.findLocator(objRepo.locationsObjRepo.servicePreferenceOption);
    var defaultFastFileNotes = objLocator.findLocator(objRepo.locationsObjRepo.defaultFastFileNotes);
    var successMsgNewLocation = objLocator.findLocator(objRepo.locationsObjRepo.successMsgNewLocation);
    var fASTGABMapIcon = objLocator.findLocator(objRepo.locationsObjRepo.fASTGABMapIcon);
    var save = objLocator.findLocator(objRepo.locationsObjRepo.save);
    var contacts = objLocator.findLocator(objRepo.locationsObjRepo.contacts);
    var filterByCustomerName = objLocator.findLocator(objRepo.locationsObjRepo.filterByCustomerName);
    var filterByExternalID = objLocator.findLocator(objRepo.locationsObjRepo.filterByExternalID);
    var filterByLocationName = objLocator.findLocator(objRepo.locationsObjRepo.filterByLocationName);
    var filterByTenant = objLocator.findLocator(objRepo.locationsObjRepo.filterByTenant);
    var resultRowCustName = objLocator.findLocator(objRepo.locationsObjRepo.resultRowCustName);
    var resultRowExtID = objLocator.findLocator(objRepo.locationsObjRepo.resultRowExtID);
    var resultRowLocationName = objLocator.findLocator(objRepo.locationsObjRepo.resultRowLocationName);
    var resultRowTenant = objLocator.findLocator(objRepo.locationsObjRepo.resultRowTenant);
    var customerNameCancel = objLocator.findLocator(objRepo.locationsObjRepo.customerNameCancel);
    var externalIDCancel = objLocator.findLocator(objRepo.locationsObjRepo.externalIDCancel);
    var locationNameCancel = objLocator.findLocator(objRepo.locationsObjRepo.locationNameCancel);
    var tenantCancel = objLocator.findLocator(objRepo.locationsObjRepo.tenantCancel);
    var editLocationHeader = objLocator.findLocator(objRepo.locationsObjRepo.editLocationHeader);
    var deleteLoc = objLocator.findLocator(objRepo.locationsObjRepo.deleteLoc);
    var cannotDeletePopup = objLocator.findLocator(objRepo.locationsObjRepo.cannotDeletePopup);
    var ok = objLocator.findLocator(objRepo.locationsObjRepo.ok);
    var editLocationSuccess = objLocator.findLocator(objRepo.locationsObjRepo.editLocationSuccess);

    this.isLocationPageDisplayed = function () {

        waitActions.waitForElementIsDisplayed(fASTGABMapIcon);
    }

    this.clickOnAddNewLocation = function () {
        buttonActions.click(addNewLocation);
        return this;
    }

    this.clickOnContacts = function () {
        buttonActions.click(contacts);
        return this;

    }


    this.isAddNewLocationPageDisplayed = function () {
        waitActions.waitForElementIsDisplayed(externalID);
        return this;
    }

    this.enterAddNewLocationData = function () {
        inputBoxActions.type(externalID, '12345');
        inputBoxActions.type(locationName, "TestLocation");
        inputBoxActions.type(defaultFastFileNotes, "Test Notes-SK");
        buttonActions.click(save);
        successMsgNewLocation.getText().then(function (text) {
            console.log(text);
            screenshots.takeScreenshot('successMsgNewLocation');
        });

    }

    this.isAddNewLocationDisplayed = function () {
            expect(addNewLocation.isPresent()).toEqual(false);
        } 

    this.clickOnFASTGABMap = function () {
        buttonActions.click(fASTGABMapIcon);
        return this;
    }

    this.updateLocationData = function() {
        browser.actions().doubleClick(resultRowCustName).perform();        
        browser.actions().doubleClick(resultRowCustName).perform();
        waitActions.waitForElementIsDisplayed(editLocationHeader);
        dropDownActions.select(servicePreferenceOption);
        if(testData.User.Role==="Admin"||testData.User.Role==="SuperAdmin")
        {
           buttonActions.click(save);
           editLocationSuccess.getText().then(function(text) {
           expect(text).toContain("updated successfully");
           })
        }
        else if(testData.User.Role==="User") {
            save.getAttribute('disabled').then(function(text) {
                expect(text).toBe('true');
            })
        }
        
    }


    this.isDeleteLocationNotAvailable = function() {
        expect(deleteLoc.isPresent()).toBe(false);
    };
            
    this.isDeleteLocationDisabled = function() {
        waitActions.waitForElementIsDisplayed(deleteLoc);
        deleteLoc.getAttribute('disabled').then(function(text) {
            expect(text).toBe('true');
        })
    }
    this.filterByFieldName = function(FilterBy) {
        if(FilterBy=="CustomerName")
        {
         inputBoxActions.type(filterByCustomerName, "LVIS Test");
         waitActions.waitForElementIsDisplayed(resultRowCustName);
         this.validateGirdData(resultRowCustName, "LVIS Test");
         return this;  
        }
         
        else if(FilterBy=="ExternalID")
        {
         inputBoxActions.type(filterByExternalID, "12345");
         waitActions.waitForElementIsDisplayed(resultRowExtID);
         this.validateGirdData(resultRowExtID, "12345");
         return this;  
        }
        else if(FilterBy=="LocationName")

        {
         inputBoxActions.type(filterByLocationName, "TestLocation");
         waitActions.waitForElementIsDisplayed(resultRowLocationName);
         this.validateGirdData(resultRowLocationName, "TestLocation");
         return this;  
        }
        else if(FilterBy=="Tenant")
        {
         inputBoxActions.type(filterByTenant, "LVIS");
         waitActions.waitForElementIsDisplayed(resultRowTenant);
         this.validateGirdData(resultRowTenant, "LVIS");
         return this; 
        }
     }

     this.clearFilter = function(Cancel) {
        if(Cancel=="CustomerName")
        buttonActions.click(customerNameCancel);
        else if(Cancel=="ExternalID")
        buttonActions.click(externalIDCancel);
        else if (Cancel=="LocationName")
        buttonActions.click(locationNameCancel);
        else if(Cancel=="Tenant")
        {
        browser.actions().mouseMove(tenantCancel).perform();
        browser.actions().click().perform();
        }

     }

     this.validateGirdData = function (resultRow, dataValidate) {
        resultRow.getText().then(function _onSuccess(text) {
            console.log(text);
            expect(text).toContain(dataValidate);
        }
        ).catch(function _onFailure(err) {
            console.log(err);
        })
        screenshots.takeScreenshot('Hello Screen shot_' + dataValidate);
    }

    this.openNewLocation = function() {
        browser.actions().doubleClick(resultRowCustName).perform();        
        browser.actions().doubleClick(resultRowCustName).perform();
        waitActions.waitForElementIsDisplayed(editLocationHeader);
        return this; 

        }

    this.deleteNewLocation = function() {
        if(testData.User.Role==="SuperAdmin")
        {
            buttonActions.click(deleteLoc);
            waitActions.waitForElementIsDisplayed(cannotDeletePopup);
            buttonActions.click(ok);
        }
        else if(testData.User.Role==="Admin"||testData.User.Role==="User")
        {
            this.isDeleteLocationDisabled();
        }
                
    return this; 
    }

}