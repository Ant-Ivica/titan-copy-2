describe('Test Execution - Dashboard to TEQ', function () {

    'use strict';
  
  
    var pages = {};
    var testData = require('../resources/testData.json');
    pages.Home=require('../pages/Home.js');
  
    var homePage = new pages.Home();
  
  
    it('Verify DashBoard To TEQ Page is Displayed Successfully', function () {
      homePage.openSearchPage(testData.search.homeUrl);
      homePage.isPageLoaded();
      expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
      homePage.verifyNavigateToTEQSection();
      expect(homePage.Details).not.toContain();
  
    }, 100000); 

    it('Verify DashBoard To TEQ Page navigated', function () {       
        homePage.NavigateToLinks();
    
      }, 900000); 
 

  
  });
  
  