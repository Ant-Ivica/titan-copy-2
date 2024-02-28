describe('Test Execution on Security Page',function()
{
    'use strict';
    var pages={};
    var testData = require('../resources/testData.json');
    pages.SecurityProfiles = require('../pages/SecurityProfiles.js');
    pages.Home = require('../pages/Home.js');
    var homePage = new pages.Home();
    var securityProfiles = new pages.SecurityProfiles();

  
  
    it('Verify Security Profile Page is loaded Successfully', function () {
      console.log(testData.User.Role);
      
      if(testData.User.Role ==='SuperAdmin' ||testData.User.Role ==='Admin')
      {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToSecurityPage();
        securityProfiles.isPageLoaded();
      }
      else if(testData.User.Role === 'User')
      {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.verifyMenuItemsOnHomeScreenAsUser();
      }
    });

    it('Verify Filter Options on Security Page', function()
    {
      if(testData.User.Role ==='SuperAdmin' ||testData.User.Role ==='Admin')
      {
      homePage.openSearchPage(testData.search.homeUrl);
      homePage.isPageLoaded();
      homePage.navigateToSecurityPage();
      securityProfiles.isPageLoaded();
      securityProfiles.gridFilterByFieldName("Name","LVIS_Automation");
        securityProfiles.clearFilter("Name");
        securityProfiles.gridFilterByFieldName("Activity Rights", "SuperAdmin");
       securityProfiles.clearFilter("Activity Rights");
        //securityProfiles.gridFilterByFieldName("Active","Yes");
        //securityProfiles.clearFilter("Active");
        securityProfiles.gridFilterByFieldName("Tenant","LVIS");
        securityProfiles.clearFilter("Tenant");
      }
      })

      it('Verify Update Controls of SuperAdminUser', function(){
        if(testData.User.Role ==='SuperAdmin' ||testData.User.Role ==='Admin')
       {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToSecurityPage();
        securityProfiles.isPageLoaded();
        securityProfiles.gridFilterByFieldName("Name","N, KUBRA");
        securityProfiles.clickOnRecordToEdit();
        if(testData.User.Role ==='SuperAdmin')
        {
           securityProfiles.verifyEditUserProfilePopUp();
           securityProfiles.selectSuperAdminRole();
        }
        else if(testData.User.Role ==='Admin')
        {
          securityProfiles.verifySuperAdminOptionNotAvailable();
        }
      }
        })

        it('Verify Update Controls of AdminUser', function(){
          if(testData.User.Role ==='SuperAdmin' ||testData.User.Role ==='Admin')
          {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToSecurityPage();
        securityProfiles.isPageLoaded();
        securityProfiles.gridFilterByFieldName("Name","N, KUBRA");
        securityProfiles.clickOnRecordToEdit();
        if(testData.User.Role ==='SuperAdmin')
          {
       securityProfiles.verifyEditUserProfilePopUp();
        securityProfiles.selectAdminRole();
          }
          else if(testData.User.Role ==='Admin')
          {
             securityProfiles.selectAdminRole();
          }
        }
        })

        it('Verify Update Controls of User', function(){
          if(testData.User.Role ==='SuperAdmin' ||testData.User.Role ==='Admin')
          {
        homePage.openSearchPage(testData.search.homeUrl);
        homePage.isPageLoaded();
        homePage.navigateToSecurityPage();
        securityProfiles.isPageLoaded();
        securityProfiles.gridFilterByFieldName("Name","N, KUBRA");
        securityProfiles.clickOnRecordToEdit();
        if(testData.User.Role ==='SuperAdmin')
        {
       securityProfiles.verifyEditUserProfilePopUp();
        securityProfiles.selectUserRole();
        }
        else if(testData.User.Role === 'Admin')
        {
          securityProfiles.selectUserRole();
        }
      }
        })

        it('Verify Activate and Inactivate Functionality', function(){
          if(testData.User.Role ==='SuperAdmin' ||testData.User.Role ==='Admin')
          {
          homePage.openSearchPage(testData.search.homeUrl);
          homePage.isPageLoaded();
          homePage.navigateToSecurityPage();
          securityProfiles.isPageLoaded();
          securityProfiles.gridFilterByFieldName("Name","N, KUBRA");
          securityProfiles.clickOnRecordToEdit();
          if(testData.User.Role ==='SuperAdmin') 
          {
         securityProfiles.verifyEditUserProfilePopUp();
          securityProfiles.verifyActiveButtonAction();
          }
          else if(testData.User.Role ==='Admin')
          {
            securityProfiles.verifyActiveButtonAction();
          }
        }
          })

          it('Verify User Right Functionality', function()
          {
            if(testData.User.Role ==='SuperAdmin' ||testData.User.Role ==='Admin')
            {
            homePage.openSearchPage(testData.search.homeUrl);
            homePage.isPageLoaded();
            homePage.navigateToSecurityPage();
            securityProfiles.isPageLoaded();
            securityProfiles.gridFilterByFieldName("Name","N, KUBRA");
            securityProfiles.clickOnRecordToEdit();
          
           if(testData.User.Role ==='SuperAdmin')
            {
              securityProfiles.verifyEditUserProfilePopUp();
           securityProfiles.verifyManageUserRights();
            }
            else(testData.User.Role === 'Admin')
            {
              securityProfiles.verifyManageUserRightNotAvailable();
            }
          }
          })

          it('Verify Add New User Functionality_Super Admin Only',function()
          {
            if(testData.User.Role ==='SuperAdmin')
            {
            homePage.openSearchPage(testData.search.homeUrl);
            homePage.isPageLoaded();
            homePage.navigateToSecurityPage();
            securityProfiles.isPageLoaded();
            securityProfiles.clickOnAddNewUserButton();
            securityProfiles.verifyNewUserProfilePopUp();
            securityProfiles.verifyInvalidCorpAccountDisplayErrMsg("random");
           securityProfiles.verifyValidINTLAccountDisplayResults("jahnavi");
            securityProfiles.verifyInvalidIntlAccountDisplayErrMsg("random");
            }
            else if(testData.User.Role === 'Admin')
            {
              homePage.openSearchPage(testData.search.homeUrl);
            homePage.isPageLoaded();
            homePage.navigateToSecurityPage();
            securityProfiles.isPageLoaded();
              securityProfiles.verifyAddNewUserButtonIsnotDisplayed();
            }
          
          })

          it('Verify User can switch between Tenancy', function()
          {
            if(testData.User.Role ==='SuperAdmin' ||testData.User.Role ==='Admin')
            {
            homePage.openSearchPage(testData.search.homeUrl);
            homePage.isPageLoaded();
            homePage.navigateToSecurityPage();
            securityProfiles.isPageLoaded();
            securityProfiles.gridFilterByFieldName("Name","Karthik, Sandhya");
            securityProfiles.clickOnRecordToEdit();
            securityProfiles.updateTenant();
            securityProfiles.getUserAndTenant();
            securityProfiles.navigateToHomePage();
            homePage.navigateToSecurityPage();
            securityProfiles.isPageLoaded();
            securityProfiles.gridFilterByFieldName("Name","Karthik, Sandhya");
            securityProfiles.clickOnRecordToEdit();
            securityProfiles.VerifyAllTenantsExceptLVIS();
            } 
          }) 

})
