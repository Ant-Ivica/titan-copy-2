module.exports = function() {
  "use strict";
  var objRepo = require("../resources/TaskMapObjRepo.json");
  var screenshots = require("protractor-take-screenshots-on-demand");
  var utilspage = require("../utils/objectLocator.js");
  var waitActions = require("../commons/waitActions.js");
  var dropDownActions = require("../commons/dropDownActions.js");
  var inputBoxActions = require("../commons/inputBoxActions.js");
  var buttonActions = require("../commons/buttonActions.js");
  var testData = require("../resources/testData.json");

  var objLocator = new utilspage();
  var waitActions = new waitActions();
  var Dropdownactions = new dropDownActions();
  var buttonActions = new buttonActions();
  var inputBoxActions = new inputBoxActions();

  var DBpage = require("../utils/DBUtilsNew.js");
  var DBUtil = new DBpage();
  var objRepo1 = require("../resources/HomeObjRepo.json");

  var userAccountName = objLocator.findLocator(
    objRepo1.homeObjRepo.userAccountName
  );
  var fasTDropDown = objLocator.findLocator(
    objRepo.TaskMapObjRepo.fastDropDown
  );
  var taskMapDropDown = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskMapDropDown
  );

  var addNewTaskMapBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.addNewTaskMapBtn
  );
  var taskDescriptionSearch = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskDescriptionSearch
  );
  var taskDescSearchResult = objLocator.findLocator(
    objRepo.TaskMapObjRepo.TaskDescSearchResult
  );
  var taskDescSearchResultCancelBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.TaskDescSearchResultCancelBtn
  );
  var taskStatusSearch = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskStatusSearch
  );
  var taskStatusSearchResult = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskStatusSearchResult
  );
  var taskStatusSearchResultCancelBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskStatusSearchResultCancelBtn
  );
  var regionIDSearch = objLocator.findLocator(
    objRepo.TaskMapObjRepo.regionIDSearch
  );
  var regionIDSearchResult = objLocator.findLocator(
    objRepo.TaskMapObjRepo.regionIDSearchResult
  );
  var regionIDSearchResultCancelBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.regionIDSearchResultCancelBtn
  );
  var messageTypeSearch = objLocator.findLocator(
    objRepo.TaskMapObjRepo.messageTypeSearch
  );
  var messageTypeSearchResult = objLocator.findLocator(
    objRepo.TaskMapObjRepo.messageTypeSearchResult
  );
  var messageTypeSearchResultCancelBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.messageTypeSearchResultCancelBtn
  );
  var serviceSearch = objLocator.findLocator(
    objRepo.TaskMapObjRepo.serviceSearch
  );
  var serviceSearchResult = objLocator.findLocator(
    objRepo.TaskMapObjRepo.serviceSearchResult
  );
  var serviceSearchResultCancelBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.serviceSearchResultCancelBtn
  );
  var externalApplicationSearch = objLocator.findLocator(
    objRepo.TaskMapObjRepo.externalApplicationSearch
  );
  var externalApplicationSearchResult = objLocator.findLocator(
    objRepo.TaskMapObjRepo.externalApplicationSearchResult
  );
  var externalApplicationSearchResultCancelBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.externalApplicationSearchResultCancelBtn
  );

  var customerNameSearch = objLocator.findLocator(
    objRepo.TaskMapObjRepo.customerNameSearch
  );
  var customerNameSearchResult = objLocator.findLocator(
    objRepo.TaskMapObjRepo.customerNameSearchResult
  );
  var customerNameSearchResultCancelBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.customerNameSearchResultCancelBtn
  );
  var tenantSearch = objLocator.findLocator(
    objRepo.TaskMapObjRepo.tenantSearch
  );
  var tenantSearchResult = objLocator.findLocator(
    objRepo.TaskMapObjRepo.tenantSearchResult
  );

  var tenantSearchResultCancelBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.tenantSearchResultCancelBtn
  );
  var taskDescriptionField = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskDescriptionField
  );
  var taskStatusDropDown = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskStatusDropDown
  );
  var taskStatusDropDownStarted = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskStatusDropDownStarted
  );
  var taskStatusDropDownWaived = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskStatusDropDownWaived
  );
  var MessageTypeDropDown = objLocator.findLocator(
    objRepo.TaskMapObjRepo.MessageTypeDropDown
  );
  var MessageTypeDropDownAdditionalInfo = objLocator.findLocator(
    objRepo.TaskMapObjRepo.MessageTypeDropDownAdditionalInfo
  );
  var serviceDropDown = objLocator.findLocator(
    objRepo.TaskMapObjRepo.serviceDropDown
  );
  var serviceDropDownEscrow = objLocator.findLocator(
    objRepo.TaskMapObjRepo.serviceDropDownEscrow
  );
  var saveNewTaskMapbtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.saveNewTaskMapbtn
  );
  var mappingTableTab = objLocator.findLocator(
    objRepo.TaskMapObjRepo.mappingTableTab
  );

  var addTaskmapSuccessMsg = objLocator.findLocator(
    objRepo.TaskMapObjRepo.addTaskmapSuccessMsg
  );
  var successMsgCloseButton = objLocator.findLocator(
    objRepo.TaskMapObjRepo.successMsgCloseButton
  );
  var deleteTaskMapbtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.deleteTaskMapbtn
  );
  var testTaskAutomation = objLocator.findLocator(
    objRepo.TaskMapObjRepo.testTaskAutomation
  );
  var testTaskAutomationFocused = objLocator.findLocator(
    objRepo.TaskMapObjRepo.testTaskAutomationFocused
  );
  var deleteTaskmapSuccessMsg = objLocator.findLocator(
    objRepo.TaskMapObjRepo.deleteTaskmapSuccessMsg
  );
  var deleteConfirmationBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.deleteConfirmationBtn
  );
  var editTaskmapSuccessMsg = objLocator.findLocator(
    objRepo.TaskMapObjRepo.editTaskmapSuccessMsg
  );
  var taskDescriptionFieldSearchResultCancelBtn = objLocator.findLocator(
    objRepo.TaskMapObjRepo.taskDescriptionFieldSearchResultCancelBtn
  );
  var saveNewTaskMapbtnDisabled = objLocator.findLocator(
    objRepo.TaskMapObjRepo.saveNewTaskMapbtnDisabled
  );
  var deleteTaskMapbtnDisabled = objLocator.findLocator(
    objRepo.TaskMapObjRepo.deleteTaskMapbtnDisabled
  );

  var testc827 = objLocator.findLocator(objRepo.TaskMapObjRepo.testc827);
  var faiLogo = objLocator.findLocator(objRepo1.homeObjRepo.faiLogo);
  var extnum;
  var internalrefnum;
  var custrefnum;

  this.openTaskMapPage = function(path) {
    if (typeof path === "undefined") {
      path = "";
    }
    browser.get(path);
    return this;
  };

  this.isAddNewTaskMapAvailable = function() {
    expect(addNewTaskMapBtn.isPresent()).toBe(false);
  };

  this.ClickonAddNewFASTTaskMap = function() {
    waitActions.waitForElementIsDisplayed(addNewTaskMapBtn);

    browser
      .actions()
      .doubleClick(addNewTaskMapBtn)
      .perform();
    screenshots.takeScreenshot("Add New Task Map");

    return this;
  };

  this.ClickonAddNewFASTTaskMap = function() {
    waitActions.waitForElementIsDisplayed(addNewTaskMapBtn);

    browser
      .actions()
      .doubleClick(addNewTaskMapBtn)
      .perform();
    screenshots.takeScreenshot("Add New Task Map");

    return this;
  };
  this.isPageLoaded = function() {
    waitActions.waitForElementIsDisplayed(faiLogo);
    waitActions.waitForElementIsDisplayed(mappingTableTab);
    browser
      .actions()
      .click(mappingTableTab)
      .perform();
    waitActions.waitForElementIsDisplayed(fasTDropDown);
    browser
      .actions()
      .click(fasTDropDown)
      .perform();
    waitActions.waitForElementIsDisplayed(taskMapDropDown);
    browser
      .actions()
      .mouseMove(taskMapDropDown)
      .perform();
    browser
      .actions()
      .click(taskMapDropDown)
      .perform();
    waitActions.waitForElementIsDisplayed(taskDescriptionSearch);
    return this;
  };

  this.verifyTaskDescriptioonTextBox = function() {
    waitActions.waitForElementIsEnabled(taskDescriptionSearch);
    inputBoxActions.type(taskDescriptionSearch, "cur ");
    waitActions.waitForElementIsDisplayed(taskDescSearchResult);
    buttonActions.click(taskDescSearchResultCancelBtn);

    return this;
  };
  this.verifyTaskStatusTextBox = function() {
    waitActions.waitForElementIsEnabled(taskStatusSearch);
    inputBoxActions.type(taskStatusSearch, "Star");
    waitActions.waitForElementIsDisplayed(taskStatusSearchResult);
    buttonActions.click(taskStatusSearchResultCancelBtn);

    return this;
  };
  this.verifyRegionIDTextBox = function() {
    waitActions.waitForElementIsEnabled(regionIDSearch);
    inputBoxActions.type(regionIDSearch, "mort");
    waitActions.waitForElementIsDisplayed(regionIDSearchResult);
    buttonActions.click(regionIDSearchResultCancelBtn);

    return this;
  };
  this.verifyMessageTypeTextBox = function() {
    waitActions.waitForElementIsEnabled(messageTypeSearch);
    inputBoxActions.type(messageTypeSearch, "comment");
    waitActions.waitForElementIsDisplayed(messageTypeSearchResult);
    buttonActions.click(messageTypeSearchResultCancelBtn);

    return this;
  };
  this.verifyServiceTextBox = function() {
    waitActions.waitForElementIsEnabled(serviceSearch);
    inputBoxActions.type(serviceSearch, "Escrow");
    waitActions.waitForElementIsDisplayed(serviceSearchResult);
    buttonActions.click(serviceSearchResultCancelBtn);

    return this;
  };
  this.verifyExternalApplicationTextBox = function() {
    waitActions.waitForElementIsEnabled(externalApplicationSearch);
    inputBoxActions.type(externalApplicationSearch, "TitlePort");
    waitActions.waitForElementIsDisplayed(externalApplicationSearchResult);
    buttonActions.click(externalApplicationSearchResultCancelBtn);

    return this;
  };
  this.verifyCustomerNameTextBox = function() {
    waitActions.waitForElementIsEnabled(customerNameSearch);
    inputBoxActions.type(customerNameSearch, "Bank");
    waitActions.waitForElementIsDisplayed(customerNameSearchResult);
    buttonActions.click(customerNameSearchResultCancelBtn);

    return this;
  };
  this.verifyTenantTextBox = function() {
    waitActions.waitForElementIsEnabled(tenantSearch);
    inputBoxActions.type(tenantSearch, "air");
    waitActions.waitForElementIsDisplayed(tenantSearchResult);

    return this;
  };

  this.addNewTaskmap = function() {
    //Adding task this specific task must not be
    //Prsent in the environment
    this.isPageLoaded();
    waitActions.waitForElementIsEnabled(addNewTaskMapBtn);
    buttonActions.click(addNewTaskMapBtn);
    waitActions.waitForElementIsEnabled(taskDescriptionField);
    inputBoxActions.type(taskDescriptionField, "TestTaskAutomation");
    waitActions.waitForElementIsEnabled(taskStatusDropDown);
    buttonActions.click(taskStatusDropDown);
    waitActions.waitForElementIsEnabled(taskStatusDropDownStarted);
    buttonActions.click(taskStatusDropDownStarted);
    waitActions.waitForElementIsEnabled(MessageTypeDropDown);
    buttonActions.click(MessageTypeDropDown);
    waitActions.waitForElementIsEnabled(MessageTypeDropDownAdditionalInfo);
    buttonActions.click(MessageTypeDropDownAdditionalInfo);
    waitActions.waitForElementIsEnabled(serviceDropDown);
    buttonActions.click(serviceDropDown);
    waitActions.waitForElementIsEnabled(serviceDropDownEscrow);
    buttonActions.click(serviceDropDownEscrow);
    waitActions.waitForElementIsEnabled(saveNewTaskMapbtn);
    buttonActions.click(saveNewTaskMapbtn);
    waitActions.waitForElementIsEnabled(addTaskmapSuccessMsg);
    buttonActions.click(successMsgCloseButton);
    return this;
  };

  this.deletetaskmap = function() {
    /*Deleting the task*/
    this.isPageLoaded();
    waitActions.waitForElementIsEnabled(taskDescriptionSearch);
    inputBoxActions.type(taskDescriptionSearch, "TestTaskAutomation");
    waitActions.waitForElementIsDisplayed(testTaskAutomation);
    buttonActions.click(testTaskAutomation);
    browser
      .actions()
      .doubleClick(testTaskAutomationFocused)
      .perform();

    waitActions.waitForElementIsEnabled(deleteTaskMapbtn);
    buttonActions.click(deleteTaskMapbtn);
    waitActions.waitForElementIsEnabled(deleteConfirmationBtn);
    buttonActions.click(deleteConfirmationBtn);
    waitActions.waitForElementIsEnabled(deleteTaskmapSuccessMsg);
    buttonActions.click(successMsgCloseButton);

    return this;
  };

  this.deletetaskmapNonSuperAdmin = function() {
    /*Deleting the task*/
    this.isPageLoaded();
    waitActions.waitForElementIsEnabled(taskDescriptionSearch);
    inputBoxActions.type(taskDescriptionSearch, "TestTaskAutomation");
    waitActions.waitForElementIsDisplayed(testTaskAutomation);
    buttonActions.click(testTaskAutomation);
    browser
      .actions()
      .doubleClick(testTaskAutomationFocused)
      .perform();

    waitActions.waitForElementIsEnabled(deleteTaskMapbtnDisabled);
   // expect(deleteTaskMapbtnDisabled.isPresent()).toBe(false);
    return this;
  };

  this.editTaskmap = function() {
    this.isPageLoaded();
    waitActions.waitForElementIsEnabled(taskDescriptionSearch);
    //Test task must be present in environment
    inputBoxActions.type(taskDescriptionSearch, "test c 827");
    waitActions.waitForElementIsDisplayed(testc827);
    browser
      .actions()
      .doubleClick(testc827)
      .perform();
    waitActions.waitForElementIsEnabled(taskStatusDropDown);
    buttonActions.click(taskStatusDropDown);
    waitActions.waitForElementIsEnabled(taskStatusDropDownStarted);
    buttonActions.click(taskStatusDropDownStarted);
    waitActions.waitForElementIsEnabled(saveNewTaskMapbtn);
    buttonActions.click(saveNewTaskMapbtn);
    waitActions.waitForElementIsEnabled(editTaskmapSuccessMsg);
    buttonActions.click(successMsgCloseButton);
    this.isPageLoaded();
    waitActions.waitForElementIsEnabled(taskDescriptionSearch);
    inputBoxActions.type(taskDescriptionSearch, "test c 827");
    waitActions.waitForElementIsDisplayed(testc827);
    browser
      .actions()
      .doubleClick(testc827)
      .perform();

    waitActions.waitForElementIsEnabled(taskStatusDropDown);
    buttonActions.click(taskStatusDropDown);
    waitActions.waitForElementIsEnabled(taskStatusDropDownWaived);
    buttonActions.click(taskStatusDropDownWaived);
    waitActions.waitForElementIsEnabled(saveNewTaskMapbtn);
    buttonActions.click(saveNewTaskMapbtn);
    waitActions.waitForElementIsEnabled(editTaskmapSuccessMsg);
    buttonActions.click(successMsgCloseButton);

    return this;
  };
  this.editTaskMapUserRole = function() {
    this.isPageLoaded();
    waitActions.waitForElementIsEnabled(taskDescriptionSearch);
    inputBoxActions.type(taskDescriptionSearch, "test c 827");
    waitActions.waitForElementIsDisplayed(testc827);
    browser
      .actions()
      .doubleClick(testc827)
      .perform();
    waitActions.waitForElementIsDisplayed(saveNewTaskMapbtnDisabled);
    return this;
  };

  this.TestTaskPresence = async function() {
    await DBUtil.ConnectDBAsync(
      "Delete  FASTTaskMap where FASTtaskmapdesc like 'TestTaskAutomation%'"
    );
    
  };
};
