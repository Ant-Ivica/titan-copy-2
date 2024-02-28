module.exports = function() {
  "use strict";
  var objRepo = require("../resources/ProcessTriggersObjRepo.json");
  var screenshots = require("protractor-take-screenshots-on-demand");
  var utilspage = require("../utils/objectLocator.js");
  var waitActions = require("../commons/waitActions.js");
  var dropDownActions = require("../commons/dropDownActions.js");
  var inputBoxActions = require("../commons/inputBoxActions.js");
  var buttonActions = require("../commons/buttonActions.js");

  var inputBoxActions = new inputBoxActions();
  var objLocator = new utilspage();
  var waitActions = new waitActions();
  var buttonActions = new buttonActions();
  var Dropdownactions = new dropDownActions();

  var testData = require("../resources/testData.json");
  var objRepo1 = require("../resources/HomeObjRepo.json");
  var DBpage = require("../utils/DBUtilsNew.js");
  var DBUtil = new DBpage();

  var fasTDropDown = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.fastDropDown
  );
  var processTriggersDropDown = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.processTriggersDropDown
  );

  var addNewFASTProcessTriggerBtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.addNewFASTProcessTriggerBtn
  );
  var descriptionSearch = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.descriptionSearch
  );
  var descriptionSearchResult = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.descriptionSearchResult
  );
  var descriptionSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.descriptionSearchResultCancelBtn
  );

  var processEventIDSearch = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.processEventIDSearch
  );
  var processEventIDSearchResult = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.processEventIDSearchResult
  );
  var processEventIDSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.processEventIDSearchResultCancelBtn
  );

  var messageTypeSearch = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.messageTypeSearch
  );
  var messageTypeSearchResult = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.messageTypeSearchResult
  );
  var messageTypeSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.messageTypeSearchResultCancelBtn
  );
  var serviceSearch = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.serviceSearch
  );
  var serviceSearchResult = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.serviceSearchResult
  );
  var serviceSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.serviceSearchResultCancelBtn
  );

  var customerSearch = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.customerSearch
  );
  var customerSearchResult = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.customerSearchResult
  );
  var customerSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.customerSearchResultCancelBtn
  );
  var tenantSearch = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.tenantSearch
  );
  var tenantSearchResult = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.tenantSearchResult
  );

  var tenantSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.tenantSearchResultCancelBtn
  );
  taskEventIDSearch;

  var taskEventIDSearch = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.taskEventIDSearch
  );
  var taskEventIDSearchResult = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.taskEventIDSearchResult
  );

  var taskEventIDSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.taskEventIDSearchResultCancelBtn
  );
  var taskEventIdDropDown = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.taskEventIdDropDown
  );
  var taskEventIdLVIS1stMissingDoc = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.taskEventIdLVIS1stMissingDoc
  );

  var processEventIdDropDown = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.processEventIdDropDown
  );
  var processEventIdLVIS1stMissingDoc = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.processEventIdLVIS1stMissingDoc
  );
  var taskStatusDropDownWaived = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.taskStatusDropDownWaived
  );
  var messageTypeDropDown = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.messageTypeDropDown
  );
  var messageTypeDropDownAdditionalInfo = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.messageTypeDropDownAdditionalInfo
  );
  var serviceDropDown = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.serviceDropDown
  );
  var serviceDropDownEscrow = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.serviceDropDownEscrow
  );
  var saveNewProcessTriggerbtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.saveNewProcessTriggerbtn
  );
  var mappingTableTab = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.mappingTableTab
  );

  var saveNewProcessTriggerbtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.saveNewProcessTriggerbtn
  );
  var successMsgCloseButton = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.successMsgCloseButton
  );
  var deleteNewProcessTriggerbtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.deleteNewProcessTriggerbtn
  );
  var testProcessTriggerAutomation = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.testProcessTriggerAutomation
  );
  var testProcessTriggerAutomationFocused = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.testProcessTriggerAutomationFocused
  );

  var deleteConfirmationBtn = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.deleteConfirmationBtn
  );
  var descriptionInputText = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.descriptionInputText
  );
  var deleteProcessTriggerSuccessMsg = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.deleteProcessTriggerSuccessMsg
  );
  var addProcessTriggerSuccessMsg = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.addProcessTriggerSuccessMsg
  );
  var TestProcessTriggerAutomationGrid = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.TestProcessTriggerAutomationGrid
  );

  var TestProcessTriggerAutomationGridFocused = objLocator.findLocator(
    objRepo.ProcessTriggersObjRepo.TestProcessTriggerAutomationGridFocused
  );
  var faiLogo = objLocator.findLocator(objRepo1.homeObjRepo.faiLogo);
  var extnum;
  var internalrefnum;
  var custrefnum;

  this.openProcessTriggersPage = function(path) {
    if (typeof path === "undefined") {
      path = "";
    }
    browser.get(path);
    return this;
  };

  this.isPageLoaded = function() {
    waitActions.waitForElementIsEnabled(faiLogo);
    waitActions.waitForElementIsEnabled(mappingTableTab);
    browser
      .actions()
      .click(mappingTableTab)
      .perform();
    waitActions.waitForElementIsEnabled(fasTDropDown);
    browser
      .actions()
      .click(fasTDropDown)
      .perform();
    waitActions.waitForElementIsEnabled(processTriggersDropDown);
    browser
      .actions()
      .mouseMove(processTriggersDropDown)
      .perform();
    browser
      .actions()
      .click(processTriggersDropDown)
      .perform();
    waitActions.waitForElementIsEnabled(processEventIDSearch);
    return this;
  };

  this.verifyTaskDescriptioonTextBox = function() {
    waitActions.waitForElementIsEnabled(processEventIDSearch);
    inputBoxActions.type(processEventIDSearch, "cur ");
    waitActions.waitForElementIsEnabled(processEventIDSearchResult);
    buttonActions.click(processEventIDSearchCancelBtn);

    return this;
  };
  this.verifyTaskStatusTextBox = function() {
    waitActions.waitForElementIsEnabled(taskStatusSearch);
    inputBoxActions.type(taskStatusSearch, "Star");
    waitActions.waitForElementIsEnabled(taskStatusSearchResult);
    buttonActions.click(taskStatusSearchResultCancelBtn);

    return this;
  };
  this.verifyDescriptionTextBox = function() {
    waitActions.waitForElementIsEnabled(descriptionSearch);
    inputBoxActions.type(descriptionSearch, "hud");
    waitActions.waitForElementIsEnabled(descriptionSearchResult);
    buttonActions.click(descriptionSearchResultCancelBtn);

    return this;
  };
  this.verifyMessageTypeTextBox = function() {
    waitActions.waitForElementIsEnabled(messageTypeSearch);
    inputBoxActions.type(messageTypeSearch, "comment");
    waitActions.waitForElementIsEnabled(messageTypeSearchResult);
    buttonActions.click(messageTypeSearchResultCancelBtn);

    return this;
  };
  this.verifyServiceTextBox = function() {
    waitActions.waitForElementIsEnabled(serviceSearch);
    inputBoxActions.type(serviceSearch, "Escrow");
    waitActions.waitForElementIsEnabled(serviceSearchResult);
    buttonActions.click(serviceSearchResultCancelBtn);

    return this;
  };
  this.verifyProcessEventIDTextBox = function() {
    waitActions.waitForElementIsEnabled(processEventIDSearch);
    inputBoxActions.type(processEventIDSearch, "loan con");
    waitActions.waitForElementIsEnabled(processEventIDSearchResult);
    waitActions.waitForElementIsEnabled(processEventIDSearchResultCancelBtn);
    buttonActions.click(processEventIDSearchResultCancelBtn);

    return this;
  };
  this.verifyCustomerTextBox = function() {
    waitActions.waitForElementIsEnabled(customerSearch);
    inputBoxActions.type(customerSearch, "Any");
    waitActions.waitForElementIsEnabled(customerSearchResult);
    buttonActions.click(customerSearchResultCancelBtn);

    return this;
  };
  this.verifyTenantTextBox = function() {
    waitActions.waitForElementIsEnabled(tenantSearch);
    inputBoxActions.type(tenantSearch, "air");
    waitActions.waitForElementIsEnabled(tenantSearchResult);
    //buttonActions.click(tenantSearchResultCancelBtn);

    return this;
  };
  this.verifyTaskEventIDTextBox = function() {
    waitActions.waitForElementIsEnabled(taskEventIDSearch);
    inputBoxActions.type(taskEventIDSearch, "LVIS - Docs Recorded ");
    waitActions.waitForElementIsEnabled(taskEventIDSearchResult);
    buttonActions.click(taskEventIDSearchResultCancelBtn);

    return this;
  };

  this.addNewProcessTrigger = function() {
    this.isPageLoaded();
    waitActions.waitForElementIsEnabled(addNewFASTProcessTriggerBtn);
    buttonActions.click(addNewFASTProcessTriggerBtn);
    waitActions.waitForElementIsEnabled(messageTypeDropDown);
    buttonActions.click(messageTypeDropDown);
    waitActions.waitForElementIsEnabled(messageTypeDropDownAdditionalInfo);
    buttonActions.click(messageTypeDropDownAdditionalInfo);
    waitActions.waitForElementIsEnabled(taskEventIdDropDown);
    buttonActions.click(taskEventIdLVIS1stMissingDoc);
    waitActions.waitForElementIsEnabled(taskEventIdLVIS1stMissingDoc);
    buttonActions.click(taskEventIdLVIS1stMissingDoc);
    waitActions.waitForElementIsEnabled(processEventIdDropDown);
    buttonActions.click(processEventIdDropDown);
    waitActions.waitForElementIsEnabled(processEventIdLVIS1stMissingDoc);
    buttonActions.click(processEventIdLVIS1stMissingDoc);
    waitActions.waitForElementIsEnabled(processEventIdDropDown);
    buttonActions.click(processEventIdDropDown);
    waitActions.waitForElementIsEnabled(processEventIdLVIS1stMissingDoc);
    buttonActions.click(processEventIdLVIS1stMissingDoc);
    waitActions.waitForElementIsEnabled(descriptionInputText);
    inputBoxActions.type(descriptionInputText, "TestProcessTriggerAutomation");
    waitActions.waitForElementIsEnabled(saveNewProcessTriggerbtn);
    buttonActions.click(saveNewProcessTriggerbtn);
    waitActions.waitForElementIsEnabled(addProcessTriggerSuccessMsg);
    buttonActions.click(successMsgCloseButton);
    ////////////////////////////
    return this;
  };

  this.DeleteNewProcessTrigger = function() {
    this.isPageLoaded();
    waitActions.waitForElementIsEnabled(descriptionSearch);
    inputBoxActions.type(descriptionSearch, "TestProcessTriggerAutomation");
    //TestProcessTriggerAutomation
    waitActions.waitForElementIsEnabled(TestProcessTriggerAutomationGrid);
    buttonActions.click(TestProcessTriggerAutomationGrid);
    browser
      .actions()
      .doubleClick(TestProcessTriggerAutomationGridFocused)
      .perform();

    waitActions.waitForElementIsEnabled(deleteNewProcessTriggerbtn);
    buttonActions.click(deleteNewProcessTriggerbtn);
    waitActions.waitForElementIsEnabled(deleteConfirmationBtn);
    buttonActions.click(deleteConfirmationBtn);
    waitActions.waitForElementIsEnabled(deleteProcessTriggerSuccessMsg);
    buttonActions.click(successMsgCloseButton);
    return this;
  };
  this.isAddNewProcessTriggerAvailable = function() {
    expect(addNewFASTProcessTriggerBtn.isPresent()).toBe(false);
  };
  this.TestProcessTriggerPresence = function() {
    this.isPageLoaded();
    waitActions.waitForElementIsEnabled(descriptionSearch);
    inputBoxActions.type(descriptionSearch, "TestProcessTriggerAutomation");
    //TestProcessTriggerAutomation
    waitActions.waitForElementIsEnabled(TestProcessTriggerAutomationGrid);
    if (TestProcessTriggerAutomationGrid.isPresent) {
    }
    buttonActions.click(TestProcessTriggerAutomationGrid);
    browser
      .actions()
      .doubleClick(TestProcessTriggerAutomationGridFocused)
      .perform();

    waitActions.waitForElementIsEnabled(deleteNewProcessTriggerbtn);
    buttonActions.click(deleteNewProcessTriggerbtn);
    waitActions.waitForElementIsEnabled(deleteConfirmationBtn);
    buttonActions.click(deleteConfirmationBtn);
    waitActions.waitForElementIsEnabled(deleteProcessTriggerSuccessMsg);
    buttonActions.click(successMsgCloseButton);
    return this;
  };
  this.ProcessTriggerDeletion = async function() {
    await DBUtil.ConnectDBAsync(
      "Delete  FASTWorkflowMap where FASTWorkflowMapDesc like 'TestProcessTriggerAutomation%'"
    );
  };
};
