describe('Test Execution on Tower Help Page', function () {

    'use strict';
  
  
    var testData = require('../resources/testData.json');
  

    var Search = require('../pages/Help.js');    

    var Help = new Search();  

    var Home = require('../pages/Home.js');    
  
    var homePage = new Home();
   
    it('Verify Help Page is Displayed Successfully', function () {
      homePage.openSearchPage(testData.search.homeUrl);
      
      homePage.isPageLoaded();
      expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
     
      Help.isHelpPageLoaded();
    });
  
})