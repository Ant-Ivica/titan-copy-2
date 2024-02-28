describe('Test Execution on Tower Home Page', function () {

  'use strict';


  var pages = {};
  var testData = require('../resources/testData.json');
  pages.Home=require('../pages/Home.js');

  var homePage = new pages.Home();


  it('Verify DashBoard Page is Displayed Successfully', function () {
    homePage.openSearchPage(testData.search.homeUrl);
    homePage.isPageLoaded();
    expect(browser.getCurrentUrl()).toContain(testData.search.homeUrl);
    homePage.verifyBEQSection();
    homePage.verifyTEQSection();
    homePage.verifyExceptionsSection();
    if(testData.User.Role ==='SuperAdmin')
    homePage.verifyMenuItemsOnHomeScreen();
    else if(testData.User.Role==='Admin')
    homePage.verifyMenuItemsOnHomeScreenAsAdmin();
    else if (testData.User.Role==='User')
    homePage.verifyMenuItemsOnHomeScreenAsUser();
   // homePage.getUserNameAndTitle();

  });

  it('Verify pop-up displays to remind user to clear cache', function(){
    homePage.openSearchPage(testData.search.homeUrl);
    homePage.isPageLoaded();
    homePage.isClearCachePopupDisplayed();
    
  }, 100000);

  let versionInfo = "";
describe("On initial Load after deployment", function () {
  it("should have a release notification Modal dialogue", function () {
    homePage.openSearchPage(testData.search.homeUrl);
    homePage.isPageLoaded();
    browser.sleep(5000);

    var value = browser
      .executeScript("return window.localStorage.getItem('TowerAppVersion');")
      .then((x) => (versionInfo = x));

    expect(element(by.id("modal-body")).getText()).toContain(
      "An Update has been made to LVIS Tower Application since you last Logged on"
    );
  });
});


// describe("On any subsequent Load after deployment", function () {
//     it("should not have a release notification Modal dialogue", function () {
//     console.log("Open Browser");
//       browser.get("http://snavnapplvis017/tower/index.html#/reporting");
//       browser.sleep(5000);
//       browser.executeScript(
//         "window.localStorage.setItem('TowerAppVersion',`{versionInfo}` );"
//       );
//       expect(element(by.id("modal-body")).isPresent()).toBeFalsy();
//     });
//   }); 

   it('Verify Exception Values of Tiles BEQ', function () {
  
    homePage.verifyBEQExceptionTilesValue();
   homePage.verifyExceptionDatesBEQ();
 
 },100000);

   it('Verify Exception Values of Tiles TEQ', function () {
 
   homePage.verifyTEQExceptionTilesValue();
   homePage.verifyExceptionDatesTEQ();

   },100000); 

 it('Verify Results in BEQ Exceptions', function () {
   homePage.verifyTilesCountBEQ(); 
   homePage.verifyBEQExceptionsBasedCount(); 

 },100000);

 it('Verify Results in TEQ Exceptions', function () {
   homePage.verifyTilesCountTEQ();
  homePage.verifyTEQExceptionsBasedCount();
 },100000);

it('Verify the status',function(){
  homePage.verifyStatusIsGreen();  
  },600000);
  
it('Verify Exception Values of Tiles TEQ Resolved', function () { 
    homePage.openSearchPage(testData.search.homeUrl);
    homePage.verifyTEQExceptionTilesValueResolved();    
},100000); 

});