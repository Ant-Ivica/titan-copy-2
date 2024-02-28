module.exports = function() {

    'use Strict';

    var testData = require('../resources/testData.json');
    var objRepo = require('../resources/ContactsObjRepo.json');
    var utilspage = require('../utils/objectLocator.js');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var buttonActions = require('../commons/buttonActions.js');
    var waitActions = require('../commons/waitActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var verifyActions = require('../commons/verifyActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var gridFilterActions = new gridFilterActions();
    var buttonActions = new buttonActions();
    var verifyActions = new verifyActions();
    var inputBoxActions = new inputBoxActions();

    
    //Contacts - Objects
    var contactID = objLocator.findLocator(objRepo.contactsObjRepo.contactID);
    var addNewContact = objLocator.findLocator(objRepo.contactsObjRepo.addNewContact);
    var newContactID = objLocator.findLocator(objRepo.contactsObjRepo.newContactID);
    var servicePreference = objLocator.findLocator(objRepo.contactsObjRepo.servicePreference);
    //var servicePreference = objLocator.findLocator(objLocator.contactsObjRepo.servicePreference);
    var selectAllServicePreference = objLocator.findLocator(objRepo.contactsObjRepo.selectAllServicePreference);
    var save = objLocator.findLocator(objRepo.contactsObjRepo.save);
    var successMsgNewContact = objLocator.findLocator(objRepo.contactsObjRepo.successMsgNewContact);
    var locationID = objLocator.findLocator(objRepo.contactsObjRepo.locationID);
    var filterByLocationID= objLocator.findLocator(objRepo.contactsObjRepo.filterByLocationID);
    var filterByLocationName = objLocator.findLocator(objRepo.contactsObjRepo.filterByLocationName);
    var filterByContactID= objLocator.findLocator(objRepo.contactsObjRepo.filterByContactID);
    var filterByActive= objLocator.findLocator(objRepo.contactsObjRepo.filterByActive);
    var resultRowLocationID = objLocator.findLocator(objRepo.contactsObjRepo.resultRowLocationID);
    var resultRowLocationName= objLocator.findLocator(objRepo.contactsObjRepo.resultRowLocationName);
    var resultRowContactID= objLocator.findLocator(objRepo.contactsObjRepo.resultRowContactID);
    var resultRowActive= objLocator.findLocator(objRepo.contactsObjRepo.resultRowActive);
    var locationIDCancel = objLocator.findLocator(objRepo.contactsObjRepo.locationIDCancel);
    var contactIDCancel= objLocator.findLocator(objRepo.contactsObjRepo.contactIDCancel);
    var locationNameCancel= objLocator.findLocator(objRepo.contactsObjRepo.locationNameCancel);
    var activeCancel= objLocator.findLocator(objRepo.contactsObjRepo.activeCancel);
    var editContactHeader = objLocator.findLocator(objRepo.contactsObjRepo.editContactHeader);
    var updateSuccessMessage = objLocator.findLocator(objRepo.contactsObjRepo.editContactHeader);

    this.isContactPageDisplayed = function() {
        waitActions.waitForElementIsDisplayed(contactID);
        return this;
    }

    this.clickOnAddNewContact = function() {
        buttonActions.click(addNewContact);
        waitActions.waitForElementIsDisplayed(newContactID);
        return this;
    }
    this.isAddNewContactPageDisplayed = function() {
        waitActions.waitForElementIsDisplayed(newContactID);
        return this;
    }

    this.isAddNewContactAvailable = function() {
        expect(addNewContact.isPresent()).toBe(false);
    }
   
    this.enterAddNewContacts = function() {
        waitActions.waitForElementIsDisplayed(newContactID);
        inputBoxActions.type(newContactID, "abc@abc.com");

        browser.actions().click(save).perform();
        successMsgNewContact.getText().then(function(text){
            console.log(text);  
         //   screenshots.takeScreenshot('successMsgNewContact');
        })

    }

    this.filterByFieldName = function(FilterBy) {
        if(FilterBy=="LocationID")
        {
            inputBoxActions.type(filterByLocationID, "1001");
            waitActions.waitForElementIsDisplayed(resultRowLocationID);
            this.validateGirdData(resultRowLocationID, "1001");
            return this;  
        }
            
        else if(FilterBy=="LocationName")
        {
            inputBoxActions.type(filterByLocationName, "LVIS Test Office");
            waitActions.waitForElementIsDisplayed(resultRowLocationName);
            this.validateGirdData(resultRowLocationName, "LVIS Test Office");
            return this;  
        }
        else if(FilterBy=="ContactID")

        {
            inputBoxActions.type(filterByContactID, "abc@abc.com");
            waitActions.waitForElementIsDisplayed(resultRowContactID);
            this.validateGirdData(resultRowContactID, "abc@abc.com");
            return this;  
        }
        
        else if(FilterBy=="Active")
        {
            inputBoxActions.type(filterByActive, "Yes");
            waitActions.waitForElementIsDisplayed(resultRowActive);
            this.validateGirdData(resultRowActive, "Yes");
            return this; 
        }
    }
     this.filterByInvalidData = function(FilterBy){
        if(FilterBy=="LocationID")
        inputBoxActions.type(filterByLocationID, 10000);
         expect(resultRowLocationID.isPresent()).toBe(false);  
        screenshots.takeScreenshot('No results for invalid data filter')
        return this;
     }

        this.clearFilter = function(Cancel) {
        if(Cancel=="LocationID")
        buttonActions.click(locationIDCancel);
        else if(Cancel=="LocationName")
        buttonActions.click(locationNameCancel);
        else if(Cancel=="ContactID")
        buttonActions.click(contactIDCancel);
        else if(Cancel=="Active")
        {
            browser.actions().mouseMove(activeCancel).perform();
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

        this.openNewContact = function() {
            inputBoxActions.type(filterByContactID, "abc@abc.com");
            browser.actions().doubleClick(resultRowContactID).perform();        
            browser.actions().doubleClick(resultRowContactID).perform();
            waitActions.waitForElementIsDisplayed(editContactHeader);
        }

        this.updateContact = function() {

            waitActions.waitForElementIsDisplayed(editContactHeader);
            inputBoxActions.type(newContactID, "test@test.com");
            if(testData.User.Role ==="Admin"|| testData.User.Role === "SuperAdmin")
        	{ 
                save.getAttribute('disabled').then(function(text) {
                expect(text).toBe('false');
            });
            
            }
            else if (testData.User.Role ==="User")
            {
                save.getAttribute('disabled').then(function(text) {
                    expect(text).toBe('true');
                })
            }

        }

    }
