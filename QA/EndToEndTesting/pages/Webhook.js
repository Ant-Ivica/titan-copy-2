        // const { isExportDeclaration, getEffectiveTypeParameterDeclarations } = require('typescript');
        const objectLocator = require('../utils/objectLocator');

        module.exports = function () {

            'use Strict';
            var objRepo1 = require('../resources/WebhooksObjRepo.json');
            var objRepo = require('../resources/CustomersObjRepo.json');
            var screenshots = require('protractor-take-screenshots-on-demand');
            var testData = require('../resources/testData.json');
            
            //common - objects
            var objLocator = new utils.objectLocator();
            var inputBoxActions = new commons.inputBoxActions();
            var buttonActions = new commons.buttonActions();
            var waitActions = new commons.waitActions();
            var dropDownActions = new commons.dropDownActions();

            var randomizedID;
            var randomizedXAPIKey;
            var randomizedSecret;
            var webhookURLForEdit =  objLocator.findLocator(objRepo1.helperVariables.webhookURIForEdit);
            var tenantName = objLocator.findLocator(objRepo.customersObjRepo.tenantText); 
            var secretText = objLocator.findLocator(objRepo1.webhooksObjRepo.secretText);
            var searchURL = objLocator.findLocator(objRepo1.webhooksObjRepo.searchURL);
            var customerName = objLocator.findLocator(objRepo.customersObjRepo.customerNameText);
            var resultRowTenant = objLocator.findLocator(objRepo.customersObjRepo.resultRowTenant);
            var webhookIcon = objLocator.findLocator(objRepo.customersObjRepo.customersWebhooks);
            var save = objLocator.findLocator(objRepo.customersObjRepo.save);
            var addWebhookButton = objLocator.findLocator(objRepo1.webhooksObjRepo.addWebhook);
            var addWebhookDomainButton = objLocator.findLocator(objRepo1.webhooksObjRepo.addWebhookDomain);
            var addWebhookNewDomain = objLocator.findLocator(objRepo1.webhooksObjRepo.addWebhookNewDomain);
            var domainTextbox = objLocator.findLocator(objRepo1.webhooksObjRepo.addDomainText);
            var webhookURL = objLocator.findLocator(objRepo1.webhooksObjRepo.webhookURL);
            var secret = objLocator.findLocator(objRepo1.webhooksObjRepo.secret);
            var xAPIKey = objLocator.findLocator(objRepo1.webhooksObjRepo.xAPIKey);
            var actionType= objLocator.findLocator(objRepo1.webhooksObjRepo.actionType);
            var maxAttempts =objLocator.findLocator(objRepo1.webhooksObjRepo.maxAttempts);
            var isActive = objLocator.findLocator(objRepo1.webhooksObjRepo.isActive);
            var successMsgNewWebhook = objLocator.findLocator(objRepo1.webhooksObjRepo.successMsgNewWebhook);
            var cancel = objLocator.findLocator(objRepo1.webhooksObjRepo.cancel);
            var deleteWebhookButton = objLocator.findLocator(objRepo1.webhooksObjRepo.delete);
            var testWebhookAutomationURLEntry = objLocator.findLocator(objRepo1.webhooksObjRepo.testWebhookAutomationURLEntry);
            var testWebhookDomainAutomationURLEntry = objLocator.findLocator(objRepo1.webhooksObjRepo.testWebhookDomainAutomationURLEntry);
            
            var webHookTiles = objLocator.findLocator(objRepo1.webhooksObjRepo.webHookTiles);
            var msgOfSuccessEdit = objLocator.findLocator(objRepo1.webhooksObjRepo.msgOfSuccessEdit);
            var openApiTitle = objLocator.findLocator(objRepo1.webhooksObjRepo.openApiTitle);
            var openApiWebRequest= objLocator.findLocator(objRepo1.webhooksObjRepo.openApiWebRequest);
            var postRequestAuthorizationKey = objLocator.findLocator(objRepo1.webhooksObjRepo.postRequestAuthorizationKey);
            var postButton =objLocator.findLocator(objRepo1.webhooksObjRepo.postButton);
            var openApiMessageForRetryCount =objLocator.findLocator(objRepo1.webhooksObjRepo.openApiMessageForRetryCount);
            var openApiMessageWithoutRetryCount =objLocator.findLocator(objRepo1.webhooksObjRepo.openApiMessageWithoutRetryCount);

            var searchXAPI_KEY =objLocator.findLocator(objRepo1.webhooksObjRepo.searchXAPIKey);
            var newEditURL="";

            this.searchForATCTenant = function () {
                inputBoxActions.typeAndEnter(tenantName, "Air Traffic Control");
                return this;
            };

            this.searchForTestCustomer=function(customerNametoTest){
                inputBoxActions.typeAndEnter(customerName, customerNametoTest);
            return this;
            };

            this.isWebhookIconDisplayed = function () {
                waitActions.waitForElementIsDisplayed(webhookIcon);
                return this;
            };

            this.clickOnWebhookIcon = function () {
                buttonActions.click(webhookIcon);
                return this;
            };

            this.isWebhookPageLoaded = function () {
                waitActions.waitForElementIsDisplayed(addWebhookButton);
                return this;
            };

            this.clickOnAddWebhookButton =function(){
                buttonActions.click(addWebhookButton);
                return this;
            };

            this.clickOnAddWebhookDomainsButton =function(){
                buttonActions.click(addWebhookDomainButton);
                return this;
            };


            this.isAddNewWebhookPageDisplayed =function(){
                waitActions.waitForElementIsDisplayed(save);
                return this;
            };

            this.isAddNewEditWebhookDomainsPageDisplayed =function(){
                waitActions.waitForElementIsDisplayed(cancel);
                return this;
            };

            this.addNewWebhookDomain=function(){
            //     buttonActions.click(addWebhookNewDomain);
            //     waitActions.waitForElementIsDisplayed(cancel);
                inputBoxActions.type(domainTextbox,"www.abc.com");
                buttonActions.click(save);
                waitActions.waitForElementIsDisplayed("Success Mesg");
                return this;
            };

            this.editWebhookDomain=function(){
                buttonActions.click(testWebhookDomainAutomationURLEntry);//add xpath for particular element/domain
                browser
                    .actions()
                    .doubleClick(testWebhookDomainAutomationURLEntry)
                    .perform();
                    waitActions.wait(3000); 
                waitActions.waitForElementIsDisplayed(save);
                waitActions.waitForElementIsDisplayed(cancel);
                inputBoxActions.type(domainTextbox,"www.abcEditeDomain.com");
                buttonActions.click(save);
                waitActions.waitForElementIsDisplayed("Success Edit Mesg");
                waitActions.waitForElementIsDisplayed(testWebhookDomainAutomationURLEntry);
                return this;
            };

            this.deleteWebhookDomain=function(){
                buttonActions.click(testWebhookDomainAutomationURLEntry);
                browser
                    .actions()
                    .doubleClick(testWebhookDomainAutomationURLEntry)
                    .perform();
                    waitActions.wait(3000); 
                waitActions.waitForElementIsDisplayed(deleteWebhookButton);
                waitActions.waitForElementIsDisplayed(cancel);
                buttonActions.click(deleteWebhookButton);
                waitActions.waitForElementIsDisplayed("Success Delete Mesg");
                waitActions.waitForElementIsDisplayed(cancel);
                return this;
            };
            
            this.addWebhookWithNonExistingDomain=function(){

                return this;
            };

            this.verifyMaximumRetryCounts = function()
            {
                //YTI
            }

            this.enterAddNewWebhookDataAndVerify= function(){
                var random = getRandomNum(1,100);
                var secretKeyToTest="TestingSecretKey"+random;
                var webhookNewURL = objRepo1.helperVariables.webhookAddNewEntryURI;
                inputBoxActions.type(webhookURL,webhookNewURL);
                inputBoxActions.type(secret,secretKeyToTest);
                inputBoxActions.type(xAPIKey,"XAPIKEYTEST");
                dropDownActions.selectDropdownbyNum(actionType, 1);
                waitActions.wait(6000);
                dropDownActions.selectDropdownbyNum(maxAttempts,1);
                buttonActions.click(isActive);
                waitActions.waitForElementIsEnabled(save);
                waitActions.wait(3000);
                buttonActions.click(save);
                waitActions.wait(3000);
                successMsgNewWebhook.getText().then(function(text){
                    console.log(text);
                    screenshots.takeScreenshot('successMsgNewWebhook');
                });
                inputBoxActions.typeAndEnter(secretText,secretKeyToTest);
                waitActions.waitForElementIsDisplayed(webHookTiles);
                waitActions.wait(2000);
                screenshots.takeScreenshot('webHookTiles');
                return this;
            };

            this.clickOnCancelButton =function(){
                buttonActions.click(cancel);
                return this;
            };

            this.isEditWebhookPopLoaded = function(){
                waitActions.wait(6000);
                waitActions.waitForElementIsDisplayed(testWebhookAutomationURLEntry);
                buttonActions.click(testWebhookAutomationURLEntry);
                browser
                    .actions()
                    .doubleClick(testWebhookAutomationURLEntry)
                    .perform();
                    waitActions.wait(6000); 
                waitActions.waitForElementIsDisplayed(deleteWebhookButton);
                return this;
            };

            this.editFieldsAndClickSave=function()
            {
                var random = getRandomNum(1,100);
                var newXAPIKey = "XAPIKEYTEST" +random;
                inputBoxActions.typeAndEnter(xAPIKey,newXAPIKey);
                dropDownActions.selectDropdownbyNum(actionType, 2);
                waitActions.wait(3000);
                waitActions.waitForElementIsDisplayed(save);
                buttonActions.click(save);
                msgOfSuccessEdit.getText().then(function(text){
                    console.log(text);
                    screenshots.takeScreenshot('msgOfSuccessEdit');
                    inputBoxActions.type(searchXAPI_KEY,newXAPIKey);
                    waitActions.wait(2000);
                    waitActions.waitForElementIsDisplayed(webHookTiles);
                });
                waitActions.wait(2000);
                return this;  
            };

            this.openOpenApiPage = function (path) {
                if (typeof path === 'undefined') {
                    path = '';
                }
                browser.get(path);
                return this;
            };

            this.isOpenApiPageLoaded = function () {
                waitActions.waitForElementIsDisplayed(openApiTitle);
                return this;
            };

            this.searchWebhookForEdit = function()
            {
                inputBoxActions.typeAndEnter(searchURL,webhookURLForEdit);
                return this;
            }

            this.isCustomerExist = function(){
                waitActions.waitForElementIsDisplayed(resultRowTenant);
                resultRowTenant.getText().then(function(text){
                    console.log(text);
                    screenshots.takeScreenshot('resultRowTenant')
                });
                return this;
            };

            this.clickOnDeleteButton= function () {
                buttonActions.click(deleteWebhookButton);
                return this;
            };

            this.verifyWebhookPageFields=function(){
                waitActions.waitForElementIsDisplayed(webhookURL);
                waitActions.waitForElementIsDisplayed(secret);
                waitActions.waitForElementIsDisplayed(xAPIKey);
                waitActions.waitForElementIsDisplayed(actionType);
                waitActions.waitForElementIsDisplayed(maxAttempts);
                return this;
            };

            this.isCancelButtonExist=function(){
                waitActions.waitForElementIsDisplayed(cancel);
                return this;
            };

            this.deleteTestingWebhookEntry=function(){
                buttonActions.click(testWebhookAutomationURLEntry);
                browser
                    .actions()
                    .doubleClick(testWebhookAutomationURLEntry)
                    .perform();
                    waitActions.wait(6000); 
                waitActions.waitForElementIsDisplayed(deleteWebhookButton);
                buttonActions.click(deleteWebhookButton);
                return this;
            };

            this.editWebhookURL=function(){
                newEditURL="http://www.testforCancelButtonCheck.com/";
                inputBoxActions.type(webhookURL,newEditURL);
                return this;
            };

            getRandomNum = function(min, max){
                return parseInt(Math.random() * (max - min) + min+10);
            };
           
            this.validateGirdData = function (resultRow, dataValidate) {
                resultRow.getText().then(function _onSuccess(text) {
                    console.log(text);
                    expect(text).toBe(dataValidate);
                }
                ).catch(function _onFailure(err) {
                    console.log(err);
                })
                screenshots.takeScreenshot('New entry validated' + dataValidate);
                return this;
            }

            this.verifyDataEntryRemainSame=function(){
                 waitActions.wait(4000);
                waitActions.waitForElementIsDisplayed(testWebhookAutomationURLEntry);
                return this;
            };

            this.postOrderThroughAPI=function(singleCaseOrMultiCase,isReEntryCount)
            {
                var random = getRandomNum(1,1000);
                randomizedID="19963d2b-a067-42dc-82de-89890g2956f76"+random;
                var Uri="https://www.testdomain1.com";
                randomizedXAPIKey="Jaishankar"+random;
                randomizedSecret="Jaishankar"+random;
                var retryList=[-1,10,"ten", 1];
                var IsPaused=true;
                var Filter ="MessageAdded";
                var openApiMessageToCheckForRetryCount = "\"Please provide a valid count for retryCount(1-5)!\"";
                var openApiMessageToCheckWithoutRetryCount = "\"Please provide a valid count for retryCount(1-5)!\"";
                
                    if(isReEntryCount===1)
                    {
                        for(var i=0;i <4; i++)
                        {  
                                    if(singleCaseOrMultiCase===1 || i===3)
                                    {
                                        i=i+3;
                                        if(singleCaseOrMultiCase===2)
                                        {   
                                                break;
                                        }
                                    }
                                    inputBoxActions.type(openApiWebRequest,"{\"Uri\":\"" +Uri+
                                    "\",\n\"Id\":\""+randomizedID+
                                    "\",\n\"Secret\":\""+randomizedSecret+
                                    "\",\n\"IsPaused\":\""+IsPaused+
                                    "\",\n\"Filter\":\""+Filter+
                                    "\",\n\"Headers\":{\n\"x-api-key\":\""+randomizedXAPIKey+"\"},\n\"Properties\":{\n\"retryCount\":"+retryList[i]+"}}"
                                );
                                inputBoxActions.type(postRequestAuthorizationKey,"TFZPUDY3NzQ6M0NFODEkdCVl");
                                waitActions.wait(3000);
                                buttonActions.click(postButton);
                                waitActions.wait(4000);
                                if(i!=3)
                                {
                                    waitActions.waitForElementIsDisplayed(openApiMessageForRetryCount);
                                    openApiMessageForRetryCount.getText().then(function(text){
                                        console.log(text);
                                        expect(text).toContain(openApiMessageToCheckForRetryCount);
                                });
                                }
                        };
                    }
                    else
                    {
                                inputBoxActions.type(openApiWebRequest,"{\"Uri\":\"" +Uri+
                                "\",\n\"Id\":\""+randomizedID+
                                "\",\n\"Secret\":\""+randomizedSecret+
                                "\",\n\"IsPaused\":\""+IsPaused+
                                "\",\n\"Filter\":\""+Filter+
                                "\",\n\"Headers\":{\n\"x-api-key\":\""+randomizedXAPIKey+"\"}}"
                            );
                            inputBoxActions.type(postRequestAuthorizationKey,"TFZPUDY3NzQ6M0NFODEkdCVl");
                            waitActions.wait(3000);
                            buttonActions.click(postButton);
                            waitActions.wait(4000);
                        
                                waitActions.waitForElementIsDisplayed(openApiMessageWithoutRetryCount);
                                openApiMessageWithoutRetryCount.getText().then(function(text){
                                    console.log(text);
                                    expect(text).toContain(openApiMessageToCheckWithoutRetryCount);
                            });
                            //update this entry Using OpenApi
                    }
                    return this;
                };

            this.verifyNewWebhookEntryInGrid=function(){
                waitActions.waitForElementIsDisplayed(testWebhookAutomationURLEntry);
                return this;
            };

            this.validateGirdData = function (resultRow, dataValidate) {
                resultRow.getText().then(function _onSuccess(text) {
                    console.log(text);
                    expect(text).toBe(dataValidate);
                }
                ).catch(function _onFailure(err) {
                    console.log(err);
                })
                screenshots.takeScreenshot('New entry validated' + dataValidate);
                return this;
            };
        };
    