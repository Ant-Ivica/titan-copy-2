module.exports = function() {

    'use Strict';

    //var custobjRepo = require('../resources/CustomersObjRepo.json');
   // var locObjRepo = require('../resources/LocationsObjRepo.json');
    var objRepo = require('../resources/FASTGABMapObjRepo.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    //common - objects
    var testData = require('../resources/testData.json');
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
    //FAST GAB Map - Objects
  //  var fASTGABMapIcon = objLocator.findLocator(locObjRepo.LocationsObjRepo.FASTGABMap);
    var FASTGABMappings = objLocator.findLocator(objRepo.fASTGABMapObjRepo.FASTGABMappings);
    var locationID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.locationID); 
    var addFASTGABMap = objLocator.findLocator(objRepo.fASTGABMapObjRepo.addFASTGABMap);
    //var locationName = objLocator.findLocator(objRepo.fASTGABMapObjRepo.locationName);
    var description =  objLocator.findLocator(objRepo.fASTGABMapObjRepo.description);
    var regionID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.regionID);
    var loanType = objLocator.findLocator(objRepo.fASTGABMapObjRepo.loanType);
    var state = objLocator.findLocator(objRepo.fASTGABMapObjRepo.state);
    var county = objLocator.findLocator(objRepo.fASTGABMapObjRepo.county);
    var businessSourceABEID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.businessSourceABEID);
    var newLenderABEID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.newLenderABEID);
    var save = objLocator.findLocator(objRepo.fASTGABMapObjRepo.save);
    var successMsgNewFASTGABMap = objLocator.findLocator(objRepo.fASTGABMapObjRepo.successMsgNewFASTGABMap);
    var filterByLocationID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.filterByLocationID);
    var filterByLocationName = objLocator.findLocator(objRepo.fASTGABMapObjRepo.filterByLocationName);
    var filterByRegionID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.filterByRegionID);
    var filterByBusinessSourceABEID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.filterByBusinessSourceABEID);
    var filterByNewLenderABEID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.filterByNewLenderABEID);
    var filterByLoanType = objLocator.findLocator(objRepo.fASTGABMapObjRepo.filterByLoanType);
    var filterByDescription = objLocator.findLocator(objRepo.fASTGABMapObjRepo.filterByDescription);
    var locationIDCancel = objLocator.findLocator(objRepo.fASTGABMapObjRepo.locationIDCancel);
    var locationNameCancel = objLocator.findLocator(objRepo.fASTGABMapObjRepo.locationNameCancel);
    var regionIDCancel = objLocator.findLocator(objRepo.fASTGABMapObjRepo.regionIDCancel);
    var businessSourceABEIDCancel = objLocator.findLocator(objRepo.fASTGABMapObjRepo.businessSourceABEIDCancel);
    var newLenderABEIDCancel = objLocator.findLocator(objRepo.fASTGABMapObjRepo.newLenderABEIDCancel);
    var loanTypeCancel = objLocator.findLocator(objRepo.fASTGABMapObjRepo.loanTypeCancel);
    var descriptionCancel = objLocator.findLocator(objRepo.fASTGABMapObjRepo.descriptionCancel);
    var resultRowLocationID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.resultRowLocationID);
    var resultRowLocationName = objLocator.findLocator(objRepo.fASTGABMapObjRepo.resultRowLocationName);
    var resultRowRegionID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.resultRowRegionID);
    var resultRowBusinessSourceABEID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.resultRowBusinessSourceABEID);
    var resultRowNewLenderABEID = objLocator.findLocator(objRepo.fASTGABMapObjRepo.resultRowNewLenderABEID);
    var resultRowLoanType = objLocator.findLocator(objRepo.fASTGABMapObjRepo.resultRowLoanType);
    var resultRowDescription = objLocator.findLocator(objRepo.fASTGABMapObjRepo.resultRowDescription);
    var Delete = objLocator.findLocator(objRepo.fASTGABMapObjRepo.Delete);
    var yesBtn = objLocator.findLocator(objRepo.fASTGABMapObjRepo.yesBtn);
    var confirmationMessage = objLocator.findLocator(objRepo.fASTGABMapObjRepo.confirmationMessage);
    var editFGMHeader = objLocator.findLocator(objRepo.fASTGABMapObjRepo.editFGMHeader);



    

        this.isFASTGABMapPageDisplayed = function() {
            waitActions.waitForElementIsDisplayed(FASTGABMappings);
            return this;
        }

        this.clickOnAddNewFASTGABMap = function() {
            buttonActions.click(addFASTGABMap);
            waitActions.waitForElementIsDisplayed(locationID);
            return this;
        }
        // this.isAddNewGABMapPageDisplayed = function() {
        //     waitActions.waitForElementIsDisplayed(locationID);
        //     return this;
        // }
    
        getRandomNum = function(min, max){
            return parseInt(Math.random() * (max - min) + min);
        };

        this.openNewRecord = function() {
            inputBoxActions.type(filterByLocationID, 1001);
            waitActions.waitForElementIsDisplayed(resultRowLocationID);
            browser.actions().doubleClick(resultRowLocationID).perform();        
            browser.actions().doubleClick(resultRowLocationID).perform();
            return this; 
    
            }

        this.isDeleteFASTGABMapDisabled = function() {
            waitActions.waitForElementIsDisplayed(Delete);
            Delete.getAttribute('disabled').then(function(text) {
                expect(text).toBe('true');
                })
        }

        this.isAddNewFASTGABMapAvailable = function() {
            expect(addFASTGABMap.isPresent()).toBe(false);
            }

        this.AddNewFASTGABMap = function() {
            waitActions.waitForElementIsDisplayed(locationID);
            inputBoxActions.type(description, "TestGAB");
            dropDownActions.selectDropdownbyNum(regionID, 6);
            dropDownActions.selectDropdownbyNum(loanType, 1);
            dropDownActions.selectDropdownbyNum(state, 6);
            //dropDownActions.selectDropdownbyNum(county, 3);
            var random = getRandomNum(1,100);
            inputBoxActions.type(businessSourceABEID, "test"+random);
            inputBoxActions.type(newLenderABEID, "test"+random);  
            buttonActions.click(save);
            browser.sleep(500);
            waitActions.waitForElementIsDisplayed(successMsgNewFASTGABMap);
            successMsgNewFASTGABMap.getText().then(function(text){
                console.log("test"+random);
                console.log(text);  
                screenshots.takeScreenshot('successMsgNewFASTGABMap');
            })

        }

        this.filterByFieldName = function(FilterBy) {
            if(FilterBy=="LocationID")
            {
            inputBoxActions.type(filterByLocationID, 1001);
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
            else if(FilterBy=="RegionID")
            {
            inputBoxActions.type(filterByRegionID, "California (410)");
            waitActions.waitForElementIsDisplayed(resultRowRegionID);
            browser.sleep(3000);
            this.validateGirdData(resultRowRegionID, "California (410)");
            return this;  
            }
            else if (FilterBy=="BusinessSourceABEID")
            {
            inputBoxActions.type(filterByBusinessSourceABEID, "test");
            waitActions.waitForElementIsDisplayed(resultRowBusinessSourceABEID);
            this.validateGirdData(resultRowBusinessSourceABEID, "test");
            return this; 
            }
            else if(FilterBy=="NewLenderABEID")
            {
            inputBoxActions.type(filterByNewLenderABEID, "test");
            waitActions.waitForElementIsDisplayed(resultRowNewLenderABEID);
            this.validateGirdData(resultRowNewLenderABEID, "test");
            return this; 
            }
            else if(FilterBy=="LoanType")
            {
            inputBoxActions.type(filterByLoanType, "Conventional");
            waitActions.waitForElementIsDisplayed(resultRowLoanType);
            this.validateGirdData(resultRowLoanType, "Conventional");
            return this; 
            }
            else if(FilterBy=="Description")
            {
            inputBoxActions.type(filterByDescription, "TestGAB");
            waitActions.waitForElementIsDisplayed(resultRowDescription);
            this.validateGirdData(resultRowDescription, "TestGAB");
            return this; 
            }
        }

        this.clearFilter = function(Cancel) {
            if(Cancel=="LocationID")
            buttonActions.click(locationIDCancel);
            else if(Cancel=="LocationName")
            buttonActions.click(locationNameCancel);
            else if(Cancel=="RegionID")
            buttonActions.click(regionIDCancel);
            else if (Cancel=="BusinessSourceABEID")
            buttonActions.click(businessSourceABEIDCancel);
            else if(Cancel=="NewLenderABEID")
            buttonActions.click(newLenderABEIDCancel);
            else if(Cancel=="LoanType")
            buttonActions.click(loanTypeCancel);
            else if(Cancel=="Description")
            {
                browser.actions().mouseMove(descriptionCancel).perform();
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

    this.deleteNewRecord = function() {
        inputBoxActions.type(filterByDescription, "TestGAB");
        waitActions.waitForElementIsDisplayed(resultRowDescription);
        browser.actions().doubleClick(resultRowDescription).perform();        
        browser.actions().doubleClick(resultRowDescription).perform();
        waitActions.waitForElementIsDisplayed(Delete);
        buttonActions.click(Delete);
        browser.sleep(3000);
        waitActions.waitForElementIsDisplayed(yesBtn);
        buttonActions.click(yesBtn);
        waitActions.waitForElementIsDisplayed(confirmationMessage);
        screenshots.takeScreenshot('confirmationMessage');
        return this; 

        }

        this.updateFASTGABMap = function() {
            waitActions.waitForElementIsDisplayed(editFGMHeader);
            dropDownActions.selectDropdownbyNum(regionID, 8);
            if(testData.User.Role ==="Admin"|| testData.User.Role === "SuperAdmin")
        	{
                buttonActions.click(save);
            }
            else if (testData.User.Role ==="User")
            {
                save.getAttribute('disabled').then(function(text) {
                    expect(text).toBe('true');
                })
            }
        }
    }