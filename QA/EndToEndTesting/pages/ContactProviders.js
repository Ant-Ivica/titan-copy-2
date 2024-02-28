module.exports = function() {

    'use Strict';

    var objRepo = require('../resources/ContactProvidersObjRepo.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var utilspage = require('../utils/objectLocator.js');
    
    //common - objects
    var testData = require('../resources/testData.json');
    var objLocator = new utilspage();
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var dropDownActions = require('../commons/dropDownActions');
    var inputBoxActions = new inputBoxActions();
    var buttonActions = new buttonActions();
    var waitActions = new waitActions();
    var dropDownActions = new dropDownActions();
    //Contacts - Objects
    var tenant = objLocator.findLocator(objRepo.contactProvidersObjRepo.tenant);
    var addNewContactProvider = objLocator.findLocator(objRepo.contactProvidersObjRepo.addNewContactProvider);
    //var customerName = objLocator.findLocator(objRepo.contactProvidersObjRepo.customerName);
    var locationName = objLocator.findLocator(objRepo.contactProvidersObjRepo.locationName);
    var contactId = objLocator.findLocator(objRepo.contactProvidersObjRepo.contactId);
    var save = objLocator.findLocator(objRepo.contactProvidersObjRepo.save);
    var providerName = objLocator.findLocator(objRepo.contactProvidersObjRepo.providerName);
    var successMsgNewContactProvider = objLocator.findLocator(objRepo.contactProvidersObjRepo.successMsgNewContactProvider);
    var filterByProviderID = objLocator.findLocator(objRepo.contactProvidersObjRepo.filterByProviderID);
    var filterByCustomerName = objLocator.findLocator(objRepo.contactProvidersObjRepo.filterByCustomerName);
    var filterByLocationName = objLocator.findLocator(objRepo.contactProvidersObjRepo.filterByLocationName);
    var filterByContactID = objLocator.findLocator(objRepo.contactProvidersObjRepo.filterByContactID);
    var filterByTenant = objLocator.findLocator(objRepo.contactProvidersObjRepo.filterByTenant);
    var resultRowProviderID = objLocator.findLocator(objRepo.contactProvidersObjRepo.resultRowProviderID);
    var resultRowCustomerName = objLocator.findLocator(objRepo.contactProvidersObjRepo.resultRowCustomerName);
    var resultRowLocationName = objLocator.findLocator(objRepo.contactProvidersObjRepo.resultRowLocationName);
    var resultRowContactID = objLocator.findLocator(objRepo.contactProvidersObjRepo.resultRowContactID);
    var resultRowTenant = objLocator.findLocator(objRepo.contactProvidersObjRepo.resultRowTenant);
    var providerIDCancel = objLocator.findLocator(objRepo.contactProvidersObjRepo.providerIDCancel);
    var customerNameCancel = objLocator.findLocator(objRepo.contactProvidersObjRepo.customerNameCancel);
    var locationNameCancel = objLocator.findLocator(objRepo.contactProvidersObjRepo.locationNameCancel);
    var contactIDCancel = objLocator.findLocator(objRepo.contactProvidersObjRepo.contactIDCancel);
    var tenantCancel = objLocator.findLocator(objRepo.contactProvidersObjRepo.tenantCancel);
    var resultRow = objLocator.findLocator(objRepo.contactProvidersObjRepo.resultRow);
    var editContactProvider = objLocator.findLocator(objRepo.contactProvidersObjRepo.editContactProvider);
    var updateSuccessMsg  = objLocator.findLocator(objRepo.contactProvidersObjRepo.updateSuccessMsg);

    this.isContactProvidersPageDisplayed = function() {
        waitActions.waitForElementIsDisplayed(addNewContactProvider);
        return this;
    }

    
    this.clickOnAddNewContactProvider = function() {
        buttonActions.click(addNewContactProvider);
        return this;
    }
    this.isAddNewContactProviderPageDisplayed = function() {
        waitActions.waitForElementIsDisplayed(tenant);
        return this;
    }
   
    this.enterAddNewContactProvider = function() {
        
       dropDownActions.select(locationName, "LVIS Test Office(1001)");
        dropDownActions.select(contactId, 'abc@abc.com');
       dropDownActions.select(providerName, 'testprovider');
        screenshots.takeScreenshot(); 
        buttonActions.click(save);
        successMsgNewContactProvider.getText().then(function(text){
            
          if(text.includes('A similar records already')||text.includes('A new record for Customer'))  
            console.log(text); 
            screenshots.takeScreenshot('successMsgNewContactProvider');
            return this;
        })
    }

    this.isAddNewContactProviderAvailable = function() {
        expect(addNewContactProvider.isPresent()).toBe(false);
    }

    this.filterByFieldName = function(FilterBy) {
        if(FilterBy=="ProviderID")
        {
            inputBoxActions.type(filterByProviderID, "3");
            waitActions.waitForElementIsDisplayed(resultRowProviderID);
            this.validateGirdData(resultRowProviderID, "3");
            return this;  
        }
            
        else if(FilterBy=="CustomerName")
        {
            inputBoxActions.type(filterByCustomerName, "LVIS Test2");
            waitActions.waitForElementIsDisplayed(resultRowCustomerName);
            this.validateGirdData(resultRowCustomerName, "LVIS Test2");
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
        else if(FilterBy=="Tenant")
        {
            inputBoxActions.type(filterByTenant, "MortgageSolutions");
            waitActions.waitForElementIsDisplayed(resultRowTenant);
            this.validateGirdData(resultRowTenant, "MortgageSolutions");
            return this; 
        }
        
    }

    this.clearFilter = function(Cancel) {
        if(Cancel=="ProviderID")
        buttonActions.click(providerIDCancel);
        else if(Cancel=="CustomerName")
        buttonActions.click(customerNameCancel);
        else if(Cancel=="LocationName")
        buttonActions.click(locationNameCancel);
        else if(Cancel=="ContactID")
        buttonActions.click(contactIDCancel);
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

    this.openNewContactProvider = function() {
        inputBoxActions.type(filterByContactID, "abc@abc.com");
        browser.actions().doubleClick(resultRow).perform();        
        browser.actions().doubleClick(resultRow).perform();
        waitActions.waitForElementIsDisplayed(editContactProvider);

    }

    this.updateContactProvider = function() {
        waitActions.waitForElementIsDisplayed(editContactProvider);
        dropDownActions.selectDropdownbyNum(contactId, 2);
        if(testData.User.Role ==="Admin"|| testData.User.Role === "SuperAdmin")
        	{
                buttonActions.click(save);
                updateSuccessMsg.getText().then(function (text) {
                    console.log(text);
                    screenshots.takeScreenshot('updateSuccessMsg');
                })
            }
            else if (testData.User.Role ==="User")
            {
                save.getAttribute('disabled').then(function(text) {
                    expect(text).toBe('true');
                })
            }
    }

}