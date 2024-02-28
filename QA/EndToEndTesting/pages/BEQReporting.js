module.exports = function() {
  "use strict";
  var objRepo = require("../resources/BEQReportingObjRepo.json");
  var screenshots = require("protractor-take-screenshots-on-demand");
  var utilspage = require("../utils/objectLocator.js");
  var waitActions = require("../commons/waitActions.js");
  var dropDownActions = require("../commons/dropDownActions.js");
  var buttonActions = require("../commons/buttonActions.js");
 // var inputBoxActions = new inputBoxActions();
  var objLocator = new utilspage();
  var waitActions = new waitActions();
  var buttonActions = new buttonActions();
  var Dropdownactions = new dropDownActions();

  var testData = require("../resources/testData.json");
  var objRepo1 = require("../resources/HomeObjRepo.json");

  var faiLogo = objLocator.findLocator(objRepo1.homeObjRepo.faiLogo);

  var dateFilterDropDown = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.dateFilterDropDown
  );
  var dateFilterLast30Days = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.dateFilterLast30Days
  );
  var exceptionsTab = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.exceptionsTab
  );
  var includeResolveCheckBox = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.includeResolveCheckBox
  );
  var searchButton = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.searchButton
  );
  var resultGridHeader = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.resultGridHeader
  );
  var messageLogBtnVisible = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.messageLogBtnVisible
  );
  var orderSummaryPopUp = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.orderSummaryPopUp
  );
  var orderSummaryCloseBtn = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.orderSummaryCloseBtn
  );
  var incomingMsgLogLink = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.incomingMsgLogLink
  );

  var gridRowCollapsed = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.gridRowCollapsed
  );
  var gridRowExpanded = objLocator.findLocator(
    objRepo.BEQReportingObjRepo.gridRowExpanded
  );

  this.openBEQReportingPage = function(path) {
    if (typeof path === "undefined") {
      path = "";
    }
    browser.get(path);
    return this;
  };

  this.isPageLoaded = function() {
    waitActions.waitForElementIsDisplayed(faiLogo);
    waitActions.waitForElementIsDisplayed(exceptionsTab);
    browser
      .actions()
      .click(exceptionsTab)
      .perform();
    waitActions.waitForElementIsEnabled(dateFilterDropDown);
    waitActions.waitForElementIsEnabled(includeResolveCheckBox);

    return this;
  };

  this.verifyMessageLogButtonDisplayed = function() {
    waitActions.waitForElementIsDisplayed(faiLogo);
    waitActions.waitForElementIsEnabled(dateFilterDropDown);
    buttonActions.click(dateFilterDropDown);
    waitActions.waitForElementIsEnabled(dateFilterLast30Days);
    buttonActions.click(dateFilterLast30Days);
    waitActions.waitForElementIsEnabled(includeResolveCheckBox);
    buttonActions.click(includeResolveCheckBox);
    waitActions.waitForElementIsEnabled(searchButton);
    buttonActions.click(searchButton);
    waitActions.waitForElementIsEnabled(searchButton);
    waitActions.waitForElementIsEnabled(resultGridHeader);
    waitActions.waitForElementIsEnabled(messageLogBtnVisible);
    buttonActions.click(messageLogBtnVisible);
    waitActions.waitForElementIsEnabled(incomingMsgLogLink);
    buttonActions.click(orderSummaryCloseBtn);
  };

  this.verifyGroupedMessageLogDisplayed = function() {
    waitActions.waitForElementIsDisplayed(faiLogo);
    waitActions.waitForElementIsEnabled(dateFilterDropDown);
    buttonActions.click(dateFilterDropDown);
    waitActions.waitForElementIsEnabled(dateFilterLast30Days);
    buttonActions.click(dateFilterLast30Days);
    waitActions.waitForElementIsEnabled(includeResolveCheckBox);
    buttonActions.click(includeResolveCheckBox);
    waitActions.waitForElementIsEnabled(searchButton);
    buttonActions.click(searchButton);
    waitActions.waitForElementIsEnabled(searchButton);
    waitActions.waitForElementIsEnabled(resultGridHeader);
    waitActions.waitForElementIsEnabled(gridRowCollapsed);
    buttonActions.click(gridRowCollapsed);
    waitActions.waitForElementIsEnabled(messageLogBtnVisible);
    buttonActions.click(messageLogBtnVisible);
    waitActions.waitForElementIsEnabled(orderSummaryPopUp);
    waitActions.waitForElementIsEnabled(incomingMsgLogLink);
    buttonActions.click(orderSummaryCloseBtn);
  };
};
