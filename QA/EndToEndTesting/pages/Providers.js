module.exports = function() {
  "use strict";
  // Providers Page
  var pages = {};
  var objRepo = require("../resources/ProvidersObjRepo.json");
  var utilspage = require("../utils/objectLocator.js");
  var waitActions = require("../commons/waitActions.js");
  var buttonActions = require("../commons/buttonActions.js");
  var verifyAction = require("../commons/verifyActions.js");
  var inputBoxActions = require("../commons/inputBoxActions.js");
  var mouseActions = require("../commons/mouseActions.js");
  var testData = require("../resources/testData.json");

  var objLocator = new utilspage();
  var waitActions = new waitActions();
  var buttonActions = new buttonActions();
  var verifyAction = new verifyAction();
  var inputBoxActions = new inputBoxActions();
  var mouseActions = new mouseActions();
  var externalId = objLocator.findLocator(objRepo.ProvidersObjRepo.externalId);
  var serviceProviderID= objLocator.findLocator(objRepo.ProvidersObjRepo.ServiceProviderID)
  var internalIdDropDown = objLocator.findLocator(
    objRepo.ProvidersObjRepo.internalIdDropDown
  );
  var internalIdDropDownLVIS = objLocator.findLocator(
    objRepo.ProvidersObjRepo.internalIdDropDownLVIS
  );

  var externalAppDropDown = objLocator.findLocator(
    objRepo.ProvidersObjRepo.externalAppDropDown
  );
  var externalAppDropDownLenderSimulator = objLocator.findLocator(
    objRepo.ProvidersObjRepo.externalAppDropDownLenderSimulator
  );
  var externalAppDropDownTitlePort = objLocator.findLocator(
    objRepo.ProvidersObjRepo.externalAppDropDownTitlePort
  );
  var tenantDropDown = objLocator.findLocator(
    objRepo.ProvidersObjRepo.tenantDropDown
  );
  var tenantDropDownAgency = objLocator.findLocator(
    objRepo.ProvidersObjRepo.tenantDropDownAgency
  );
  var saveButton = objLocator.findLocator(objRepo.ProvidersObjRepo.saveButton);
  var deleteButton = objLocator.findLocator(
    objRepo.ProvidersObjRepo.deleteButton
  );
  var deleteConfirmationButton = objLocator.findLocator(
    objRepo.ProvidersObjRepo.deleteConfirmationButton
  );
  var addNewFASTOfficeMapBtn = objLocator.findLocator(
    objRepo.ProvidersObjRepo.addNewFASTOfficeMapBtn
  );
  var testIDResult = objLocator.findLocator(
    objRepo.ProvidersObjRepo.testIDResult
  );
  var testIDResultSelected = objLocator.findLocator(
    objRepo.ProvidersObjRepo.testIDResultSelected
  );
  var providerIDSearch = objLocator.findLocator(
    objRepo.ProvidersObjRepo.providerIDSearch
  );
  var providerIDSearchResult = objLocator.findLocator(
    objRepo.ProvidersObjRepo.providerIDSearchResult
  );
  var providerIDSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProvidersObjRepo.providerIDSearchResultCancelBtn
  );
  var providerNameSearch = objLocator.findLocator(
    objRepo.ProvidersObjRepo.providerNameSearch
  );
  var providerNameSearchResult = objLocator.findLocator(
    objRepo.ProvidersObjRepo.providerNameSearchResult
  );
  var providerNameSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProvidersObjRepo.providerNameSearchResultCancelBtn
  );
  var externalIDSearch = objLocator.findLocator(
    objRepo.ProvidersObjRepo.externalIDSearch
  );
  var externalIDSearchResult = objLocator.findLocator(
    objRepo.ProvidersObjRepo.externalIDSearchResult
  );
  var externalIDSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProvidersObjRepo.externalIDSearchResultCancelBtn
  );
  var internalAppSearch = objLocator.findLocator(
    objRepo.ProvidersObjRepo.internalAppSearch
  );
  var internalAppSearchResult = objLocator.findLocator(
    objRepo.ProvidersObjRepo.internalAppSearchResult
  );
  var internalAppSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProvidersObjRepo.internalAppSearchResultCancelBtn
  );
  var externalAppSearch = objLocator.findLocator(
    objRepo.ProvidersObjRepo.externalAppSearch
  );
  var externalAppSearchResult = objLocator.findLocator(
    objRepo.ProvidersObjRepo.externalAppSearchResult
  );
  var externalAppSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProvidersObjRepo.externalAppSearchResultCancelBtn
  );
  var tenantSearch = objLocator.findLocator(
    objRepo.ProvidersObjRepo.tenantSearch
  );
  var tenantSearchResult = objLocator.findLocator(
    objRepo.ProvidersObjRepo.tenantSearchResult
  );
  var tenantSearchResultCancelBtn = objLocator.findLocator(
    objRepo.ProvidersObjRepo.tenantSearchResultCancelBtn
  );
  var FASTofficeMapLink = objLocator.findLocator(
    objRepo.ProvidersObjRepo.FASTofficeMapLink
  );
  var addNewProviderBtn = objLocator.findLocator(
    objRepo.ProvidersObjRepo.addNewProviderBtn
  );

  var mappingTableTab = objLocator.findLocator(
    objRepo.ProvidersObjRepo.mappingTableTab
  );

  var providerPage = objLocator.findLocator(
    objRepo.ProvidersObjRepo.providerPage
  );
  var addNewFASTOfficemapBtn = objLocator.findLocator(
    objRepo.ProvidersObjRepo.addNewFASTOfficemapBtn
  );

  var addNewProviderBtnHidden = objLocator.findLocator(
    objRepo.ProvidersObjRepo.addNewProviderBtnHidden
  );

  this.openProviderPage = function() {
    browser.get(testData.search.homeUrl);
    waitActions.waitForElementIsDisplayed(mappingTableTab);
    buttonActions.click(mappingTableTab);
    waitActions.waitForElementIsDisplayed(providerPage);
    buttonActions.click(providerPage);

    return this;
  };
  this.openProviderPageURL = function(path) {
    if (typeof path === "undefined") {
      path = "";
    }
    browser.get(path);

    return this;
  };
  this.isPageLoaded = function() {
    waitActions.waitForElementIsDisplayed(providerIDSearch);
    return this;
  };

  this.verifyProviderIDTextBox = function() {
    waitActions.waitForElementIsEnabled(providerIDSearch);
    inputBoxActions.type(providerIDSearch, "4333");
    waitActions.waitForElementIsDisplayed(providerIDSearchResult);
    buttonActions.click(providerIDSearchResultCancelBtn);

    return this;
  };

  this.verifyProviderNameTextBox = function() {
    waitActions.waitForElementIsEnabled(providerNameSearch);
    inputBoxActions.type(providerNameSearch, "test");
    waitActions.waitForElementIsDisplayed(providerNameSearchResult);
    buttonActions.click(providerNameSearchResultCancelBtn);

    return this;
  };

  this.verifyExternalIDTextBox = function() {
    waitActions.waitForElementIsEnabled(externalIDSearch);
    inputBoxActions.type(externalIDSearch, "test");
    waitActions.waitForElementIsDisplayed(externalIDSearchResult);
    buttonActions.click(externalIDSearchResultCancelBtn);

    return this;
  };

  this.verifyInternalApplicationTextBox = function() {
    waitActions.waitForElementIsEnabled(internalAppSearch);
    inputBoxActions.type(internalAppSearch, "FAST");
    waitActions.waitForElementIsDisplayed(internalAppSearchResult);
    buttonActions.click(internalAppSearchResultCancelBtn);

    return this;
  };
  this.verifyExternalApplicationTextBox = function() {
    waitActions.waitForElementIsEnabled(externalAppSearch);
    inputBoxActions.type(externalAppSearch, "FAST");
    waitActions.waitForElementIsDisplayed(externalAppSearchResult);
    buttonActions.click(externalAppSearchResultCancelBtn);

    return this;
  };
  this.verifyTenantTextBox = function() {
    waitActions.waitForElementIsEnabled(tenantSearch);
    inputBoxActions.type(tenantSearch, "air");
    waitActions.waitForElementIsDisplayed(tenantSearchResult);
    buttonActions.click(tenantSearchResultCancelBtn);

    return this;
  };

  this.fastOfficeMapNavigation = function() {
    waitActions.waitForElementIsEnabled(tenantSearch);
    inputBoxActions.type(tenantSearch, "rf");
    waitActions.waitForElementIsEnabled(FASTofficeMapLink);
    buttonActions.click(FASTofficeMapLink);
    waitActions.waitForElementIsEnabled(addNewFASTOfficemapBtn);

    return this;
  };
  this.isAddNewProviderAvailable = function() {
    expect(addNewProviderBtnHidden.isPresent()).toBe(true);
  };

  this.addNewProvider = function() {
    waitActions.waitForElementIsEnabled(addNewProviderBtn);
    buttonActions.click(addNewProviderBtn);
    waitActions.waitForElementIsEnabled(externalId);
    inputBoxActions.type(externalId, "TestIDProvider");
    waitActions.waitForElementIsEnabled(internalIdDropDown);
    buttonActions.click(internalIdDropDown);
    waitActions.waitForElementIsEnabled(internalIdDropDownLVIS);
    buttonActions.click(internalIdDropDownLVIS);
    waitActions.waitForElementIsEnabled(externalAppDropDown);
    buttonActions.click(externalAppDropDown);
    waitActions.waitForElementIsEnabled(externalAppDropDownLenderSimulator);
    buttonActions.click(externalAppDropDownLenderSimulator);
    waitActions.waitForElementIsEnabled(tenantDropDown);
    buttonActions.click(tenantDropDown);
    waitActions.waitForElementIsEnabled(tenantDropDownAgency);
    buttonActions.click(tenantDropDownAgency);
    waitActions.waitForElementIsEnabled(saveButton);
    buttonActions.click(saveButton);
  };

  this.DeleteNewProvider = function() {
    /*Deleting the provider*/
    browser.refresh();

    waitActions.waitForElementIsEnabled(externalIDSearch);
    inputBoxActions.type(externalIDSearch, "TestIDProvider");
    waitActions.waitForElementIsDisplayed(testIDResult);
    buttonActions.click(testIDResult);
    browser
      .actions()
      .doubleClick(testIDResultSelected)
      .perform();
    waitActions.waitForElementIsEnabled(deleteButton);
    buttonActions.click(deleteButton);
    waitActions.waitForElementIsEnabled(deleteConfirmationButton);
    buttonActions.click(deleteConfirmationButton);

    return this;
  };
  this.addServiceProviderID = function() {
    var providerID= Math.floor(Math.random() * (+60000 - +10)) + +10
    waitActions.waitForElementIsEnabled(addNewProviderBtn);
    buttonActions.click(addNewProviderBtn);
    waitActions.waitForElementIsEnabled(externalId);
    inputBoxActions.type(externalId, "IDProvider"+providerID);
    waitActions.waitForElementIsEnabled(internalIdDropDown);
    buttonActions.click(internalIdDropDown);
    waitActions.waitForElementIsEnabled(internalIdDropDownLVIS);
    buttonActions.click(internalIdDropDownLVIS);
    waitActions.waitForElementIsEnabled(externalAppDropDown);
    buttonActions.click(externalAppDropDown);
    waitActions.waitForElementIsEnabled(externalAppDropDownTitlePort);
    buttonActions.click(externalAppDropDownTitlePort);
    waitActions.waitForElementIsEnabled(serviceProviderID);
    inputBoxActions.type(serviceProviderID, providerID);
    waitActions.waitForElementIsEnabled(tenantDropDown);
    buttonActions.click(tenantDropDown);
    waitActions.waitForElementIsEnabled(tenantDropDownAgency);
    buttonActions.click(tenantDropDownAgency);
    waitActions.waitForElementIsEnabled(saveButton);
    buttonActions.click(saveButton);
    /*Deleting the provider*/
    browser.refresh();

    waitActions.waitForElementIsEnabled(externalIDSearch);
    inputBoxActions.type(externalIDSearch, "IDProvider"+providerID);
 
    return this;
  };





  this.editProvider = function() {};
};
