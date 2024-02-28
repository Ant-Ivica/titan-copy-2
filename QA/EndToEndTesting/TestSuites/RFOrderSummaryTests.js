describe('Tower Reporting Search Tests execution on RFOrderSummary', function () {

    'use strict';

    var testData = require('../resources/testData.json');


    var Search = require('../pages/reportingTower.js');    
    var home = require('../pages/Home.js');

var searchPage = new Search();
  
    var homePage = new home();

  
    it('Open Tower Reporting page RFOrderSummary', function () {
        searchPage.openSearchPage(testData.search.homeUrl);
        searchPage.isPageLoaded();
        searchPage.isRFOrderSummary(); 
       
    });

      
     

});
