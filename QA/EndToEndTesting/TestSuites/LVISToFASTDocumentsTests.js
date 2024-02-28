describe('Test Execution on - Mapping Tables - LVIS To Fast Documents', function () {
    'use strict';

    var testData = require('../resources/testData.json');

    var pages = {};
    pages.Home = require('../pages/Home.js');
    pages.lVISToFASTDocuments = require('../pages/LVISToFASTDocuments.js');
    pages.Customers = require('../pages/Customers.js');

    var homePage = new pages.Home();
    var lVISToFASTDocuments = new pages.lVISToFASTDocuments();
    var customerPage = new pages.Customers();
   

    // it('LVIS To Fast Documents: Add Document Test', function () {
        
    //     homePage.openSearchPage(testData.search.homeUrl);
    //     homePage.isPageLoaded();
    //     homePage.navigateToMappingTablesPage();
    //     customerPage.isCustomerPageLoaded();
    //     lVISToFASTDocuments.navigateToLvisToFastDocPage();
    //     lVISToFASTDocuments.isPageLoaded();
    //     if(testData.User.Role ==='SuperAdmin')
    //     {
    //     lVISToFASTDocuments.addNewLvisToFastDocument();
    //            }
    //     else if(testData.User.Role === 'Admin')
    //     {
    //     lVISToFASTDocuments.addNewLvisToFastDocument();

    //     }
    //     else if(testData.User.Role === 'User')
    //     {
    //         lVISToFASTDocuments.verifyAddNewRecordOptionIsNotAvailable();
    //     }
        
    // });

    // it('LVIS To Fast Documents: Edit Document Test', function () {
    //     homePage.openSearchPage(testData.search.homeUrl);
    //     homePage.isPageLoaded();
    //     homePage.navigateToMappingTablesPage();
    //     customerPage.isCustomerPageLoaded();
    //     lVISToFASTDocuments.navigateToLvisToFastDocPage();
    //     lVISToFASTDocuments.isPageLoaded();
    //      if(testData.User.Role ==='SuperAdmin')
    //     {
    //     lVISToFASTDocuments.openExistingDocument();
    //     lVISToFASTDocuments.editExistingDocument();
    //     }
    //     else if(testData.User.Role === 'Admin')
    //     {
       
    //     lVISToFASTDocuments.openExistingDocument();
    //     lVISToFASTDocuments.editExistingDocument();
       
    //     // lVISToFASTDocuments.verifyDeleteIsDisabled();
    //     }
    //     else if(testData.User.Role === 'User')
    //     {
    //         lVISToFASTDocuments.verifyAddNewRecordOptionIsNotAvailable();
    //     }
   

        
    // });


    it('Verify Filter Options on LvisToFastDoc Page', function()
    {
        if(testData.User.Role ==='SuperAdmin' || testData.User.Role === 'Admin' || testData.User.Role ==='User')
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToMappingTablesPage();
        customerPage.isCustomerPageLoaded();
        lVISToFASTDocuments.navigateToLvisToFastDocPage();
        lVISToFASTDocuments.isPageLoaded();
      lVISToFASTDocuments.gridFilterByFieldName("LvisDocType","Death Certificate");
      lVISToFASTDocuments.clearFilter("LvisDocType");
      lVISToFASTDocuments.gridFilterByFieldName("LvisDocDescription", "QATowerTest_DontEdit");
      lVISToFASTDocuments.clearFilter("LvisDocDescription");
      lVISToFASTDocuments.gridFilterByFieldName("Service","Signing");
      lVISToFASTDocuments.clearFilter("Service");
      lVISToFASTDocuments.gridFilterByFieldName("FastDocType","Closing Buyer Only");
      lVISToFASTDocuments.clearFilter("FastDocType");
      lVISToFASTDocuments.gridFilterByFieldName("Tenant","LVIS");
     
       
      })

    // it('LVIS To Fast Documents: Delete Document Test', function () {
    //     homePage.openSearchPage(testData.search.homeUrl);
    //     homePage.isPageLoaded();
    //     homePage.navigateToMappingTablesPage();
    //     customerPage.isCustomerPageLoaded();
    //     lVISToFASTDocuments.navigateToLvisToFastDocPage();
    //     lVISToFASTDocuments.isPageLoaded();
    //   if(testData.User.Role ==='SuperAdmin')
    // {
    //     lVISToFASTDocuments.openExistingDocument();
    //     lVISToFASTDocuments.deleteExistingDocument();
    // }
    // else if(testData.User.Role === 'Admin')
    // {
    
    // lVISToFASTDocuments.openExistingDocument();
    // lVISToFASTDocuments.verifyDeleteIsDisabled();
    // }
    // else if(testData.User.Role === 'User')
    // {
    //     lVISToFASTDocuments.verifyDeleteIsDisabledForUser();
    //     //lVISToFASTDocuments.verifyAddNewRecordOptionIsNotAvailable();
    // }

    // });

});