describe("Test Execution on Business Exception Queues", function() {
  "use strict";


  var pages = {};
  var testData = require("../resources/testData.json");
  pages.BEQReporting = require("../pages/BEQReporting.js");
  var BEQReportingPage = new pages.BEQReporting();
  var DBpage = require("../utils/DBUtils.js");
  var DBUtil = new DBpage();

  it("Verify MessageLog being displayed", function() {
    BEQReportingPage.openBEQReportingPage(testData.search.homeUrl); //
    BEQReportingPage.isPageLoaded(); //
    BEQReportingPage.verifyMessageLogButtonDisplayed(); //
  });

  it("Verify Grouped MessageLog being displayed", function() {
    BEQReportingPage.openBEQReportingPage(testData.search.homeUrl); //
    BEQReportingPage.isPageLoaded(); //
    BEQReportingPage.verifyGroupedMessageLogDisplayed(); //
  });
});
