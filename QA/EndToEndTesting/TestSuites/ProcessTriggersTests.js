describe("Test Execution on TOWER  Process Triggers Page", function() {
  "use strict";

  var testData = require("../resources/testData.json");
  var ProcessTriggers = require("../pages/ProcessTriggers.js");
  var ProcessTriggersPage = new ProcessTriggers();
  var DBpage = require("../utils/DBUtils.js");
  var DBUtil = new DBpage();

  it("Verify filter is working successfully", function() {
    ProcessTriggersPage.openProcessTriggersPage(testData.search.homeUrl); //
    ProcessTriggersPage.isPageLoaded(); //
    ProcessTriggersPage.verifyMessageTypeTextBox(); //
    ProcessTriggersPage.verifyTaskEventIDTextBox();
    ProcessTriggersPage.verifyServiceTextBox(); //
    ProcessTriggersPage.verifyDescriptionTextBox(); //
    ProcessTriggersPage.verifyCustomerTextBox(); //
    ProcessTriggersPage.verifyTenantTextBox(); //
  });

  it("Verify AddNew Process Trigger ", function() {
    ProcessTriggersPage.openProcessTriggersPage(testData.search.homeUrl);
    ProcessTriggersPage.isPageLoaded();
    if (testData.User.Role !== "User") {
      ProcessTriggersPage.addNewProcessTrigger();
    } else ProcessTriggersPage.isAddNewProcessTriggerAvailable();
  });
  it("Verify deletion of Process Trigger", async () => {
    ProcessTriggersPage.openProcessTriggersPage(testData.search.homeUrl);
    ProcessTriggersPage.isPageLoaded();
    if (testData.User.Role === "SuperAdmin") {
      ProcessTriggersPage.DeleteNewProcessTrigger();
    } else await ProcessTriggersPage.ProcessTriggerDeletion();
  });
});
