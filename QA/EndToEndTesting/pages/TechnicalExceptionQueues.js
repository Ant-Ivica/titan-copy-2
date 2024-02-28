module.exports = function () {

    'use Strict';
    // Mapping Tables - Customers
    var objRepo = require('../resources/TechnicalExceptionQueuesObjRepo.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var reportingTower = require('../pages/reportingTower');
    var tEQPage = require('../pages/TechnicalExceptionQueues');
    //common - objects
    var utilspage = require('../utils/objectLocator.js'); 
    var buttonActions = require('../commons/buttonActions.js');
    var waitActions = require('../commons/waitActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var inputBoxActions = require('../commons/inputBoxActions');
    var inputBoxActions = new inputBoxActions();
    var Dropdownactions = new dropDownActions();
    var objLocator = new utilspage(); 
    var gridFilterActions = new gridFilterActions();
    var buttonActions = new buttonActions(); 
    var waitActions = new waitActions();
    var tEQFirstRecord = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.tEQFirstRecord);
    var exceptionInfo = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.exceptionInfo);
    var messageInfo = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.messageInfo);
    var resubmitButton = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.resubmitButton);
    var confirmPopUp = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.confirmPopUp);
    var confirmOk = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.confirmOk);
    var resubmitConfMsg = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.resubmitConfMsg);
    var saveConfMsg = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.saveConfMsg);
    var closeExceptionDetails=objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.closeExceptionDetails);
    var tEQConfMsg = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.tEQConfMsg);
    var statusResolved = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.statusResolved);
    var tEQRecord1 = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.tEQRecord1);
    var tEQRecord2 = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.tEQRecord2);
    var tEQRecord3 = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.tEQRecord3);
    var resubmitSelected = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.resubmitSelected);
    var bulkConfirmOk = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.bulkConfirmOk);
    var selectedResubmit = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.selectedResubmit);
    var successfullResubmit = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.successfullResubmit);
    var nonSucessfullResubmit = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.nonSucessfullResubmit);
    var bulkConfirmMsgClose = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.bulkConfirmMsgClose);
    var confirmationPopUp = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.confirmationPopUp);
    var save= objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.save);
    var reject= objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.reject);
    var tEQHeader= objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.tEQHeader);
    var TogglebyDate = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.TogglebyDate);
    var dropdownDate = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.dropdownDate);
    var searchDate = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.searchDate);
    var spinner = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.spinner);
    var toggleButton = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.toggleButton);
    //var closeButton = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.closeButton);
    var dropdown = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.dropdown);
    var textSearch = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.textSearch);
    var searchButton = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.searchButton);
    //var serviceRequest  = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.serviceRequest);
   // var toggleButton = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.toggleButton);
    var closeExceptionInfo = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.closeExceptionInfo);
    var resultExtNum = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.resultExtNum);
    var cancel = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.cancel);
    var MessageTypeSearch = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.MessageTypeSearch);
    var ExceptionInfoDetailsNotes = objLocator.findLocator(objRepo.technicalExceptionQueuesObjRepo.ExceptionInfoDetailsNotes);
    var extNum = "13476";
    var internalrefnum = "349";
    var custrefnum = "234";
    var DBpage = require('../utils/DBUtilsNew.js');
    var DBUtilNew = new DBpage();
    this.getFastExternalRef=  async () =>  {

        var Results=   await DBUtilNew.ConnectDBAsync("select top 1 * from servicerequest  where InternalRefNum is not null and statustypecodeid = 857 order by CreatedDate desc")
       
        if(Results != undefined)
        {
            console.log(Results.recordset);
            extNum =Results.recordset[0].ExternalRefNum;
            internalrefnum=Results.recordset[0].InternalRefNum;
            custrefnum=Results.recordset[0].CustomerRefNum;
        }

    }
        
        this.isTEQPageLoaded = function() {
            waitActions.waitForElementIsDisplayed(tEQFirstRecord);
            return this;
        };

        this.verifyDefaultDate = function() {

            expect(dropdownDate.$('option:checked').getText()).toBe("24 hrs");
        }
        
        this.ClickonFirstRow = function () {
            var until = protractor.ExpectedConditions;
              browser.wait(until.presenceOf(tEQFirstRecord), 2400000, 'Element taking too long to appear in the DOM');

          
           browser.actions().doubleClick(tEQFirstRecord).perform();
            browser.actions().doubleClick(tEQFirstRecord).perform();
            return this;
        };

        this.isExceptionDetailsLoaded = function (option) {
            browser.sleep(3000);
            waitActions.waitForElementIsDisplayed(exceptionInfo);  
            if(option===1) {
                waitActions.waitForElementIsAvailable(resultExtNum);
                resultExtNum.getText().then(function(text) {
                    console.log(text);
                    console.log(extNum);
                    expect(resultExtNum).toContain(extNum);
                    buttonActions.click(closeExceptionInfo);
                })
            }
            else if (option===2) { 
                browser.sleep(3000);
                waitActions.waitForElementIsDisplayed(cancel);
                buttonActions.click(cancel);
                }
            else if (option===3) {
                browser.sleep(3000);
                waitActions.waitForElementIsDisplayed(cancel);  
                buttonActions.click(cancel);
                }
    
            return this;
        };
        
        this.isExceptionDetailsDisplayed = function() {
            waitActions.waitForElementIsDisplayed(exceptionInfo);
            return this;
        }

        this.resubmitException = function() {
         
            waitActions.waitForElementIsDisplayed(exceptionInfo);  
            browser.actions().mouseMove(exceptionInfo).perform();            
            browser.actions().click(exceptionInfo).perform();
            browser.sleep(10000);
      
            browser.actions().mouseMove(messageInfo).perform();
            browser.actions().click(messageInfo).perform();

            buttonActions.click(resubmitButton);

            waitActions.waitForElementIsDisplayed(confirmPopUp);
            buttonActions.click(confirmOk);
            browser.sleep(10000);
            resubmitConfMsg.getText().then(function(text){
                expect(text).toBe('Exception was resubmitted successfully')
            })

            buttonActions.click(closeExceptionDetails);
      
            waitActions.waitForElementIsAvailable(tEQHeader);
           
            return this;
        }

        this.saveTEQException = function() { 
            waitActions.waitForElementIsDisplayed(exceptionInfo);  
            browser.actions().mouseMove(exceptionInfo).perform();            
            browser.actions().click(exceptionInfo).perform();
            browser.sleep(10000);
      
            browser.actions().mouseMove(messageInfo).perform();
            browser.actions().click(messageInfo).perform();

            buttonActions.click(resubmitButton);  
           
            buttonActions.click(save);
            browser.sleep(10000);
            saveConfMsg.getText().then(function(text){
                expect(text).toBe('Exception information was saved successfully')
            })
            waitActions.waitForElementIsAvailable(tEQHeader);             
            return this;
        }

        this.bulkResubmit = function() {
            var until = protractor.ExpectedConditions;
            browser.wait(until.presenceOf(tEQFirstRecord), 2400000, 'Element taking too long to appear in the DOM');
            buttonActions.click(tEQRecord1);
            buttonActions.click(tEQRecord2);
            buttonActions.click(tEQRecord3);
            buttonActions.click(resubmitSelected);
            browser.sleep(10000);
            buttonActions.click(bulkConfirmOk);
            waitActions.waitForElementIsDisplayed(confirmationPopUp);
            selectedResubmit.getText().then(function(text){
                var selCount = text.split(':');
                console.log(selCount[1]);
            })
            successfullResubmit.getText().then(function(text){
                var selCount = text.split(':');
                console.log(selCount[1]);
            })
            nonSucessfullResubmit.getText().then(function(text){
                var selCount = text.split(':');
                console.log(selCount[1]);
            })

            buttonActions.click(bulkConfirmMsgClose);

        }

        this.ClickonDateSearchToggle = function () {

            browser.actions().click(TogglebyDate).perform();
            waitActions.waitForElementIsDisplayed(dropdownDate);
            Dropdownactions.selectDropdownbyNum(dropdownDate, 7);
            buttonActions.click(searchDate);    
            this.ClickonFirstRow();

        }

        this.closeExceptionDetails = function() {
            buttonActions.click(closeExceptionDetails);
        }

        this.ClickonToggle = function (option) {

            waitActions.waitForElementIsDisplayed(toggleButton);
         
            browser.actions().mouseMove(toggleButton).click().perform();
            waitActions.waitForElementIsDisplayed(textSearch);
            Dropdownactions.selectDropdownbyNum(dropdown, option)
            if (option == 1)
                inputBoxActions.type(textSearch, extNum);
            else if (option == 2)
            inputBoxActions.type(textSearch, internalrefnum);
            else if (option == 3)
            inputBoxActions.type(textSearch, custrefnum);
            buttonActions.click(searchButton);
            browser.wait(function () {
                return spinner.isDisplayed().then(function (result) { return !result });
            }, 20000);
    
            return this;
        };

        this.ClickonFirstRowOfRejectPage  = function(){
            var until = protractor.ExpectedConditions;
            inputBoxActions.type(MessageTypeSearch, "Service Request"); //Reject implemented on Service Request message type
            browser.wait(until.presenceOf(tEQFirstRecord), 2400000, 'Element taking too long to appear in the DOM');
            browser.actions().doubleClick(tEQFirstRecord).perform();
        }

        this.RejectTEQException = function() { 
            waitActions.waitForElementIsDisplayed(exceptionInfo);  
            browser.actions().mouseMove(exceptionInfo).perform();            
            browser.actions().click(exceptionInfo).perform();
            browser.sleep(10000);
      
            browser.actions().mouseMove(messageInfo).perform();
            browser.actions().click(messageInfo).perform();

            buttonActions.click(reject);
            browser.sleep(10000);
            inputBoxActions.type(AddNotes, "Reject with &.");
            buttonActions.click(RejectOrder);
            rejectConfMsg.getText().then(function(text){
                expect(text).toBe('Rejected successfully.')
            })
            waitActions.waitForElementIsAvailable(tEQHeader);             
            return this;
        }

        this.saveTEQExceptionWithNotes = function() { 
            waitActions.waitForElementIsDisplayed(exceptionInfo);
            inputBoxActions.type(ExceptionInfoDetailsNotes, "Test Notes &.");           
            buttonActions.click(save);
            browser.sleep(10000);
            saveConfMsg.getText().then(function(text){
                expect(text).toBe('Exception information was saved successfully')
            })
            waitActions.waitForElementIsAvailable(tEQHeader);             
            return this;
        }
}