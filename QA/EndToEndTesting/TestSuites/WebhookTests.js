const waitActions = require('../commons/waitActions');

describe('Test Execution on -  [Open API] Webhook Configuration ', function () {
    'use strict';
        var testData = require('../resources/testData.json');
        // var homePage = new pages.Home();
        // const { Home } = require('pages/Home.js');
    
        var homePage = new pages.Home();
        var customerPage = new pages.Customers();
        var webhookPage = new pages.Webhook();
        var waitActions = new commons.waitActions();
        var helperObject = require('../resources/WebhooksObjRepo.json');

                it('Tower_OpenAPI: Verify Add New Webhook',function(){
                    webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
                    isAddNewWebhookPageDisplayed(webhookPage);
                    webhookPage.verifyWebhookPageFields();
                    webhookPage.enterAddNewWebhookDataAndVerify();
                
                });

                // it('Tower_OpenAPI: Verify Webhook fields and data displayed as per access', function () {
                //     webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
                //     isAddNewWebhookPageDisplayed(webhookPage);
                //     webhookPage.clickOnCancelButton();
                // });

                // it('Tower_OpenAPI: Verify Edit pop up for webhooks',function(){
                // webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
                //     editWebhookAndSave(webhookPage);
                // });

                // it('Tower_OpenAPI: Verify Cancel button working on edit webhook pop up',function(){
                //     webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
                //     webhookPage.isEditWebhookPopLoaded();
                //     webhookPage.editWebhookURL();
                //     webhookPage.clickOnCancelButton();
                //     webhookPage.verifyDataEntryRemainSame();
                //     webhookPage.deleteTestingWebhookEntry();
                //     webhookPage.isWebhookPageLoaded();
                // });

                // it('Tower_OpenAPI: Verify all webhook fields are displayed after add/update through API(POST)',function(){
                //     navigateToOpenAPI(webhookPage, testData, waitActions);
                //     webhookPage.postOrderThroughAPI(1);
                //         browser.sleep(3000);
                //         webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
                //         editWebhookAndSave(webhookPage);
                //     webhookPage.deleteTestingWebhookEntry();
                //     webhookPage.isWebhookPageLoaded();
                // });
            });

// describe('OpenAPI: Verify webhook retry count field only allows 1-5  insert/update from API and Tower UI', function () {
//             'use strict';
//         var request = require("request");
//         var testData = require('../resources/testData.json');
        // var homePage = new pages.Home();
//         var customerPage = new pages.Customers();
//         var webhookPage = new pages.Webhook();
//         var waitActions = new commons.waitActions();
//                 it('Tower_OpenAPI:Verify With Invalid Retry Count(POST)',function(){
//                         navigateToOpenAPI(webhookPage, testData, waitActions);
//                         webhookPage.postOrderThroughAPI(2,1);
//                         webhookPage.postOrderThroughAPI(1,1);
//                             browser.sleep(3000);
//                                 webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
//                                 isAddNewWebhookPageDisplayed(webhookPage);
//                         webhookPage.verifyWebhookPageFields();
//                         webhookPage.verifyMaximumRetryCounts();//YTI
//                 });
//                 it('Tower_OpenAPI:Verify Without Retry Count(POST)',function(){
//                     navigateToOpenAPI(webhookPage, testData, waitActions);
//                     webhookPage.postOrderThroughAPI(1,2);
//                         browser.sleep(3000);
//                             webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
//                             isAddNewWebhookPageDisplayed(webhookPage);
//                     webhookPage.verifyWebhookPageFields();
//                     //Update in openAPi using PUT request
//                     webhookPage.deleteTestingWebhookEntry();
//                     webhookPage.isWebhookPageLoaded();

//             });
            
//             it('Tower_OpenAPI:Verify Updating(PUT) Webhook through OpenAPi',function(){
//                 navigateToOpenAPI(webhookPage, testData, waitActions);
//                 webhookPage.postOrderThroughAPI(1);
//                     browser.sleep(3000);
//                         webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
//                         isAddNewWebhookPageDisplayed(webhookPage);
//                 webhookPage.verifyWebhookPageFields();
//                 webhookPage.verifyMaximumRetryCounts();//YTI
//         });
//     });  
 

//     describe('[Open API] Validate pre-approved domain for Webhook - Part 2', function () {
//         'use strict';
//             var testData = require('../resources/testData.json');
            // var homePage = new pages.Home();
//             var customerPage = new pages.Customers();
//             var webhookPage = new pages.Webhook();
//             var waitActions = new commons.waitActions();
//             var helperObject = require('../resources/WebhooksObjRepo.json');

//                     it('Tower - Webhook Domain Verify Add/Edit/Delete',function(){
//                         webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
//                         isAddNewWebhookPageDisplayed(webhookPage);
//                         webhookPage.verifyWebhookPageFields();
//                         webhookPage.enterAddNewWebhookDataAndVerify();
//                         isAddNewEditWebhookDomainsPageDisplayed(webhookPage);
//                         webhookPage.addNewWebhookDomain();
//                         webhookPage.editWebhookDomain();
//                         webhookPage.deleteWebhookDomain();
//                         webhookPage.clickOnCancelButton();
//                         webhookPage.isWebhookPageLoaded();
//                         webhookPage.deleteTestingWebhookEntry();
//                         webhookPage.isWebhookPageLoaded();
//                     });

//                     // it('Tower - Add/Edit Webhook, verify Webhook Domain exists for customer',function(){
//                     //     webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject);
//                     //     isAddNewWebhookPageDisplayed(webhookPage);
//                     //     webhookPage.verifyWebhookPageFields();
//                     //     webhookPage.enterAddNewWebhookDataAndVerify();
//                     //     //add new customer
//                     //     isAddNewEditWebhookDomainsPageDisplayed(webhookPage);
//                     //     webhookPage.addNewWebhookDomain();//Search for success message and then try below
//                     //     webhookPage.addWebhookWithNonExistingDomain();//Search for error message
//                     //     webhookPage.clickOnCancelButton();
//                     //     webhookPage.isWebhookPageLoaded();
//                     //     webhookPage.deleteTestingWebhookEntry();
//                     //     webhookPage.isWebhookPageLoaded();
//                     // });
    
//                 });      




function editWebhookAndSave(webhookPage) {
    webhookPage.isEditWebhookPopLoaded();
    webhookPage.editFieldsAndClickSave();
}

function isAddNewWebhookPageDisplayed(webhookPage) {
    webhookPage.clickOnAddWebhookButton();
    webhookPage.isAddNewWebhookPageDisplayed();
}

function isAddNewEditWebhookDomainsPageDisplayed(webhookPage)
{
    webhookPage.clickOnAddWebhookDomainsButton();
    webhookPage.isAddNewEditWebhookDomainsPageDisplayed();
}

function navigateToOpenAPI(webhookPage, testData, waitActions) {
    webhookPage.openOpenApiPage(testData.search.webhookWebApi);
    waitActions.wait(4000);
    webhookPage.isOpenApiPageLoaded();
    expect(browser.getCurrentUrl()).toContain(testData.search.webhookWebApi);
    waitActions.wait(4000);
}

function webhookPageLoadedForTestingCustomer(waitActions, homePage, testData, customerPage, webhookPage, helperObject) {
    waitActions.wait(2000);
    homePage.openSearchPage(testData.search.homeUrl);
    homePage.isPageLoaded();
    expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
    waitActions.wait(4000);
    homePage.navigateToMappingTablesPage();
    customerPage.isCustomerPageLoaded();
    webhookPage.searchForATCTenant();
    webhookPage.searchForTestCustomer(helperObject.helperVariables.webhookCustomerForTesting);
    webhookPage.isWebhookIconDisplayed();
    webhookPage.clickOnWebhookIcon();
    webhookPage.isWebhookPageLoaded();
}