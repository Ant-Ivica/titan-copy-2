describe("Test Execution on Tower Provider mapping screen", function() {
  "use strict";
  var pages = {};
  var testData = require("../resources/testData.json");
  pages.Provider = require("../pages/Providers.js");
  pages.Home = require("../pages/Home.js");

  var homePage = new pages.Home();

  var providerPage = new pages.Provider();
  it("Verify filter is working successfully", function() {
    providerPage.openProviderPage();
    providerPage.isPageLoaded();
    providerPage.verifyProviderIDTextBox();
    providerPage.verifyProviderNameTextBox();
    providerPage.verifyExternalIDTextBox();
    providerPage.verifyInternalApplicationTextBox();
    providerPage.verifyExternalApplicationTextBox();
    providerPage.verifyTenantTextBox();
  });

  it("Verify Add new Provider", function() {
    providerPage.openProviderPage();
    providerPage.isPageLoaded();

    if (testData.User.Role !== "User") {
      providerPage.addNewProvider();
    }
    if (testData.User.Role === "SuperAdmin") {
      providerPage.DeleteNewProvider();
    } else if (testData.User.Role === "User")
      providerPage.isAddNewProviderAvailable();
  });

  it("Verify navigation to FASTOfficeMap from Provider screen", function() {
    providerPage.openProviderPage(testData.search.providersmapping);
    providerPage.isPageLoaded();
    providerPage.fastOfficeMapNavigation();

    expect(browser.getCurrentUrl()).toContain("/fastofficemappings/");
  });

  it("Verify if ServiceProviderID is visible For only TitlePort ", function() {
    providerPage.openProviderPage();
    providerPage.isPageLoaded();
    providerPage.addServiceProviderID();
  });
});
