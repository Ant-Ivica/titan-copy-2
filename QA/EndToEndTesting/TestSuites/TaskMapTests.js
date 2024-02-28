describe("Test Execution on TOWER MappingFASTTask map Page", function() {
  "use strict";

  var testData = require("../resources/testData.json");
  var TaskMap = require("../pages/TaskMap.js");
  var taskMapPage = new TaskMap();
  var DBpage = require("../utils/DBUtils.js");
  var DBUtil = new DBpage();

  it("Verify filter is working successfully", function() {
    taskMapPage.openTaskMapPage(testData.search.homeUrl);
    taskMapPage.isPageLoaded();
    taskMapPage.verifyTaskDescriptioonTextBox();
    taskMapPage.verifyTaskStatusTextBox();
    taskMapPage.verifyRegionIDTextBox();
    taskMapPage.verifyMessageTypeTextBox();
    taskMapPage.verifyServiceTextBox();
    taskMapPage.verifyExternalApplicationTextBox();
    taskMapPage.verifyCustomerNameTextBox();
    taskMapPage.verifyTenantTextBox();
  });

  it("Verify AddNew FastTaskmap ", function() {
    taskMapPage.openTaskMapPage(testData.search.homeUrl);
    taskMapPage.isPageLoaded();
    if (testData.User.Role !== "User") {
      taskMapPage.addNewTaskmap();
    } else {
      taskMapPage.isAddNewTaskMapAvailable();
    }
  });

  it("Verify Delete FastTaskmap ", async () => {
    taskMapPage.openTaskMapPage(testData.search.homeUrl);
    taskMapPage.isPageLoaded();

    if (testData.User.Role === "SuperAdmin") {
      taskMapPage.deletetaskmap();
    } else await taskMapPage.TestTaskPresence();
  });

  it("Verify updation of FastTaskmap ", function() {
    taskMapPage.openTaskMapPage(testData.search.homeUrl);
    taskMapPage.isPageLoaded();
    if (testData.User.Role !== "User") {
      taskMapPage.editTaskmap();
    } else {
      taskMapPage.editTaskMapUserRole();
    }
  });
});
