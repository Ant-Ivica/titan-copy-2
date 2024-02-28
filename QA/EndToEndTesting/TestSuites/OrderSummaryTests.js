describe('Tower Reporting Search Tests execution', function () {

    'use strict';

    var testData = require('../resources/testData.json');


    var Search = require('../pages/reportingTower.js');    

     var searchPage = new Search();  

    var externalnum ;
    it('Open Tower Reporting page', function () {
       searchPage.openSearchPage(testData.search.homeUrl);
        searchPage.isPageLoaded();       
        searchPage.ClickonFirstRow();
        searchPage.PopupisPageLoaded(false);
        searchPage.Popupclose();
    });
    
    var testParams =["ExternalRefNum", "Internalrefnum", "Customerrefnumber","InternalReferenceId"];
    testParams.forEach(function(testSpec) {
      it('Tower Reporting Test - Search by '+testSpec.toString(), function () {        
        searchPage.ClickonToggle(testParams.indexOf(testSpec)+1);
        searchPage.ClickonFirstRow();
        searchPage.PopupisPageLoaded(true);
        searchPage.Popupclose();
      });

    });
  

      it('Tower Reporting Test - Search by Date Last30 days', function () {        
        searchPage.ClickonDateSearchToggle();
        searchPage.ClickonFirstRow();
        searchPage.PopupisPageLoaded(false);
        searchPage.Popupclose();
      });


});
