describe('Test Execution on Exceptions - Technical Exception Queue', function () {

    'use strict';

    var pages={};
    pages.TechnicalExceptionQueues = require('../pages/TechnicalExceptionQueues.js');
    pages.BusinessExceptionQueues = require('../pages/BusinessExceptionQueues.js');
    pages.Home = require('../pages/Home.js');

    var testData = require('../resources/testData.json');
    var homePage = new pages.Home();
    var bEQPage = new pages.BusinessExceptionQueues();
    var tEQPage = new pages.TechnicalExceptionQueues();

    // var Search = require('../pages/TechnicalExceptionQueues.js')   
    // var searchPage = new Search(); 

    
    // beforeEach(function(){
    //     homePage.openSearchPage(testData.search.homeUrl);
    //       homePage.isPageLoaded();
    //       expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
    // })

    it('Verify TEQ - Exception window disappears post successfull resubmission', function () {
      homePage.openSearchPage(testData.search.homeUrl);

        homePage.isPageLoaded();    
        homePage.navigateToExceptionsPage();
        bEQPage.clickOnTEQTab();
        tEQPage.verifyDefaultDate();
        tEQPage.ClickonFirstRow();
        tEQPage.resubmitException();     
  
        
    })

    
    it('Verify TEQ - Exception window disappears post saving the Exception', function () {
      homePage.openSearchPage(testData.search.homeUrl);

        homePage.isPageLoaded();    
        homePage.navigateToExceptionsPage();
        bEQPage.clickOnTEQTab();
        tEQPage.verifyDefaultDate();
        tEQPage.ClickonFirstRow();
        tEQPage.saveTEQException();      
              
    })
    
    


    it('Verify Multiple resubmissions from TEQ', function() {
      homePage.openSearchPage(testData.search.homeUrl);
      homePage.isPageLoaded();   
      homePage.navigateToExceptionsPage();
        bEQPage.clickOnTEQTab();
        tEQPage.bulkResubmit();
    })

    it('Verify TEQ - Search by Date', function () { 
      homePage.openSearchPage(testData.search.homeUrl);
      homePage.isPageLoaded();    
      homePage.navigateToExceptionsPage();
      bEQPage.clickOnTEQTab();    
      tEQPage.ClickonDateSearchToggle();
      tEQPage.ClickonFirstRow();
      tEQPage.isExceptionDetailsDisplayed();
      tEQPage.closeExceptionDetails();
    });

     var testParams =["ExternalRefNum", "Internalrefnum", "Customerrefnumber"];
     testParams.forEach(function(testSpec) {
    it('Tower TEQ - Search by '+testSpec.toString(), async () =>{ 

      await tEQPage.getFastExternalRef();
      homePage.openSearchPage(testData.search.homeUrl);
      homePage.navigateToExceptionsPage();
      bEQPage.clickOnTEQTab();      
      tEQPage.ClickonToggle(testParams.indexOf(testSpec)+1);
      tEQPage.ClickonFirstRow();
      tEQPage.isExceptionDetailsLoaded(testParams.indexOf(testSpec)+1);

    });

    it('Verify TEQ - Reject', function() {
      homePage.openSearchPage(testData.search.homeUrl);
      homePage.isPageLoaded();    
      homePage.navigateToExceptionsPage();
      bEQPage.clickOnTEQTab();
      tEQPage.ClickonFirstRowOfRejectPage(); //works only if record found
      tEQPage.RejectTEQException();
    });

    it('Verify TEQ - Save and Resubmit', function() {
      homePage.openSearchPage(testData.search.homeUrl);
      homePage.isPageLoaded();    
      homePage.navigateToExceptionsPage();
      bEQPage.clickOnTEQTab();
      tEQPage.ClickonFirstRow();
      tEQPage.saveTEQExceptionWithNotes();
      tEQPage.ClickonFirstRow();
      tEQPage.resubmitException();   
    });

 });


})