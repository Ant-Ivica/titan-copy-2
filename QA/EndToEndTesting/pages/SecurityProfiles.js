module.exports = function () {
    'use strict';
    var pages={};
    var objRepo = require('../resources/SecurityProfilesObjRepo.json');
    var utilspage = require('../utils/objectLocator.js');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var constantsFile = require('../resources/constantsTower.json');
    var buttonActions = require('../commons/buttonActions.js');
    var waitActions = require('../commons/waitActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var verifyActions = require('../commons/verifyActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var gridFilterActions = new gridFilterActions();
    var buttonActions = new buttonActions();
    var verifyActions = new verifyActions();
    var inputBoxActions = new inputBoxActions();
    var dropDownActions = require('../commons/dropDownActions.js');
    var dropdownactions = new dropDownActions();

    //xpaths
    var pageLabel = objLocator.findLocator(objRepo.SecurityObjRepo.pageLabel);
    var name_Txt = objLocator.findLocator(objRepo.SecurityObjRepo.name_Txt);
    var activity_Txt = objLocator.findLocator(objRepo.SecurityObjRepo.activity_Txt);
    var active_txt = objLocator.findLocator(objRepo.SecurityObjRepo.active_txt);
    var tenant_Txt = objLocator.findLocator(objRepo.SecurityObjRepo.tenant_Txt);
    var name_results = objLocator.findLocator(objRepo.SecurityObjRepo.name_results);
    var activity_results = objLocator.findLocator(objRepo.SecurityObjRepo.activity_results);
    var active_results = objLocator.findLocator(objRepo.SecurityObjRepo.active_results);
    var tenant_results = objLocator.findLocator(objRepo.SecurityObjRepo.tenant_results);
    var name_Close = objLocator.findLocator(objRepo.SecurityObjRepo.name_Close);
    var activity_Close = objLocator.findLocator(objRepo.SecurityObjRepo.activity_Close);
    var active_Close = objLocator.findLocator(objRepo.SecurityObjRepo.active_Close);
    var tenant_Close = objLocator.findLocator(objRepo.SecurityObjRepo.tenant_Close);
    var result_File = objLocator.findLocator(objRepo.SecurityObjRepo.result_File);
    var editProfilePopUp = objLocator.findLocator(objRepo.SecurityObjRepo.editProfilePopUp);
    var userProfile_lbl = objLocator.findLocator(objRepo.SecurityObjRepo.userProfile_lbl);
    var userRights_lbl = objLocator.findLocator(objRepo.SecurityObjRepo.userRights_lbl);
    //let tenantList= objLocator.findLocator(objRepo.SecurityObjRepo.tenantList);
    var activityRigths = objLocator.findLocator(objRepo.SecurityObjRepo.activityRigths);
    var active = objLocator.findLocator(objRepo.SecurityObjRepo.active);
    var manageTEQ = objLocator.findLocator(objRepo.SecurityObjRepo.manageTEQ);
    var manageBEQ = objLocator.findLocator(objRepo.SecurityObjRepo.manageBEQ);
    var manageRequest = objLocator.findLocator(objRepo.SecurityObjRepo.manageRequest);
    var superAdmin_lbl = objLocator.findLocator(objRepo.SecurityObjRepo.superAdmin_lbl);
    var superAdmin_btn = objLocator.findLocator(objRepo.SecurityObjRepo.superAdmin_btn);
    var save_btn = objLocator.findLocator(objRepo.SecurityObjRepo.save_btn);
    var admin_lbl = objLocator.findLocator(objRepo.SecurityObjRepo.admin_lbl);
    var admin_btn = objLocator.findLocator(objRepo.SecurityObjRepo.admin_btn);
    var user_lbl = objLocator.findLocator(objRepo.SecurityObjRepo.user_lbl);
    var user_btn = objLocator.findLocator(objRepo.SecurityObjRepo.user_btn);
    var updateSuccessMsg = objLocator.findLocator(objRepo.SecurityObjRepo.updateSuccessMsg);
    var addNewUser_btn = objLocator.findLocator(objRepo.SecurityObjRepo.addNewUser_btn);
    var addNewUserActive_btn = objLocator.findLocator(objRepo.SecurityObjRepo.addNewUserActive_btn);
    var identity_List = objLocator.findLocator(objRepo.SecurityObjRepo.identity_List);
    var identitySearch_txt = objLocator.findLocator(objRepo.SecurityObjRepo.identitySearch_txt);
    var searchButton = objLocator.findLocator(objRepo.SecurityObjRepo.searchButton);
    var searchResult = objLocator.findLocator(objRepo.SecurityObjRepo.searchResult);
    var errorMessage = objLocator.findLocator(objRepo.SecurityObjRepo.errorMessage);
    var addUserPopupClose = objLocator.findLocator(objRepo.SecurityObjRepo.addUserPopupClose);
    var userID = "//div[@class='form-group'][contains(.,'User Id')]//descendant::input";
    var name = "//div[@class='form-group'][contains(.,'Name')]//descendant::input";
    var newUsername = "//div[@class='form-group has-error'][contains(.,'Name')]//descendant::input";
    let tenantList = element.all(by.xpath("//select[contains(@ng-model,'vm.entity.TenantId')]"));
    var home = objLocator.findLocator(objRepo.SecurityObjRepo.home);
     var userTitle = objLocator.findLocator(objRepo.SecurityObjRepo.userTitle); 

    this.isPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(pageLabel);
        return this;
    };

    this.gridFilterByFieldName = function (FilterBy, Value) {
        if (FilterBy == "Name") {
            gridFilterActions.filter(name_Txt, Value, name_results);
            screenshots.takeScreenshot('Name');
        }
        else if (FilterBy == "Activity Rights") {
            gridFilterActions.filter(activity_Txt, Value, activity_results);
            screenshots.takeScreenshot('Activity Rights');

        }
        else if (FilterBy == "Active") {
            gridFilterActions.filter(active_txt, Value, active_results);
            screenshots.takeScreenshot('Active');
        }
        else if (FilterBy == "Tenant") {
            gridFilterActions.filter(tenant_Txt, Value, tenant_results);
            screenshots.takeScreenshot('Tenant');
        }

    }

    this.clearFilter = function (Clear) {
        if (Clear == "Name") {
            buttonActions.click(name_Close);

        }
        else if (Clear == "Activity Rights") {
            buttonActions.click(activity_Close);
        }
        else if (Clear == "Active") {
            buttonActions.click(active_Close);

        }
        else if (Clear == "Tenant") {
            browser.actions().mouseMove(tenant_Close).perform();
            browser.actions().click().perform();

        }

    }

    this.clickOnRecordToEdit = function () {

        waitActions.waitForElementIsAvailable(result_File);
        browser
            .actions()
            .doubleClick(result_File)
            .perform();

        result_File.isDisplayed().then(function (res) {
            if (res) {

                browser.actions().doubleClick(result_File).perform();
            }
        });
        waitActions.waitForElementIsAvailable(editProfilePopUp);

        screenshots.takeScreenshot("Edit User to Super Admin");

        return this;
    }

    this.verifyEditUserProfilePopUp = function () {
        var i = 0;
        waitActions.waitForElementIsAvailable(userProfile_lbl);
        waitActions.waitForElementIsAvailable(userRights_lbl);
        expect(element(by.xpath(userID)).getAttribute('disabled')).toBe('true');
        expect(element(by.xpath(name)).getAttribute('disabled')).toBe('true');
        expect(tenantList.count()).toBe(7);
        tenantList.each(function (element) {
            element.getText().then(function (text) {

                expect(text).toContain(constantsFile.HomePageConstants.securityEditUser[i]);
                i++;
            });
        });
        waitActions.waitForElementIsAvailable(tenantList);
        waitActions.waitForElementIsAvailable(activityRigths);
        waitActions.waitForElementIsAvailable(active);
        waitActions.waitForElementIsAvailable(manageTEQ);
        waitActions.waitForElementIsAvailable(manageBEQ);
        waitActions.waitForElementIsAvailable(manageRequest);
    }

    this.selectSuperAdminRole = function () {
        buttonActions.click(superAdmin_lbl);
        superAdmin_btn.getAttribute('class').then(function (val) {
            if (val.includes('ng-valid-parse')) {
                buttonActions.click(save_btn);
                gridFilterActions.validateGirdData(activity_results, "SuperAdmin");
                console.log("SUPER Admin_Should click on save button");
            }
            else
                console.log("SUPER Admin already selected");
        });
    }

    this.verifySuperAdminOptionNotAvailable = function(){
        expect(superAdmin_btn.isPresent()).toBe(false);  
    }
    this.verifyAdminOptionNotAvailable = function(){
        expect(admin_btn.isPresent()).toBe(false);  
    }

    this.selectAdminRole = function () {
        buttonActions.click(admin_lbl);
        admin_btn.getAttribute('class').then(function (val) {
            if (val.includes('ng-valid-parse')) {
                buttonActions.click(save_btn);
                gridFilterActions.validateGirdData(activity_results, "Admin");
                console.log("Admin_Should click on save button");
            }
            else
                console.log("Admin_already selected");
        });
    }

    this.selectUserRole = function () {
        buttonActions.click(user_lbl);
        user_btn.getAttribute('class').then(function (val) {
            if (val.includes('ng-valid-parse')) {
                buttonActions.click(save_btn);
                gridFilterActions.validateGirdData(activity_results, "User");
                console.log("User_Should click on save button");
            }
            else
                console.log("User_already selected");
        });
    }

    this.verifyActiveButtonAction = function () {
        active.getAttribute('class').then(function (result) {
            if (result.includes('ng-not-empty')) {
                buttonActions.click(active);
                buttonActions.click(save_btn);
                console.log("to be chnaged to no");
                updateSuccessMsg.getText().then(function (successMsg) {
                    expect(successMsg).toContain("updated successfully");
                });
               gridFilterActions.validateGirdData(active_results, "No");
            }
            else {
                buttonActions.click(active);
                buttonActions.click(save_btn);
                console.log("to be chnaged to yes");
                updateSuccessMsg.getText().then(function (successMsg) {
                    expect(successMsg).toContain("updated successfully");
                });
               gridFilterActions.validateGirdData(active_results, "Yes");
            }
        })
    }

    this.verifyManageUserRights = function () {
       
        manageTEQMethod();
        manageBEQMethod();
        manageRequestMethod();
        buttonActions.click(save_btn);
                updateSuccessMsg.getText().then(function (successMsg) {
                    expect(successMsg).toContain("updated successfully");
                });

    }

    this.verifyManageUserRightNotAvailable = function(){
        expect(manageTEQ.isPresent()).toBe(false);
        expect(manageBEQ.isPresent()).toBe(false);
        expect(manageRequest.isPresent()).toBe(false);
    }

    function manageTEQMethod(){
    manageTEQ.getAttribute('class').then(function (result) {
            if (result.includes('ng-not-empty')) {
                buttonActions.click(manageTEQ);
                screenshots.takeScreenshot('Manage TEQ');
            }
            else {
                buttonActions.click(manageTEQ);
                screenshots.takeScreenshot('Manage TEQ');
               
            }
        })
    }

  function manageBEQMethod(){  
  manageBEQ.getAttribute('class').then(function (result) {
            if (result.includes('ng-not-empty')) {
                buttonActions.click(manageBEQ);
                screenshots.takeScreenshot('Manage BEQ');
            }
            else {
                buttonActions.click(manageBEQ);
                screenshots.takeScreenshot('Manage BEQ');
            }
        })
    }

  function manageRequestMethod(){
  manageRequest.getAttribute('class').then(function (result) {
            if (result.includes('ng-not-empty')) {
                buttonActions.click(manageRequest);
                screenshots.takeScreenshot('Manage Access Request');
            }
            else {
                buttonActions.click(manageRequest);
                screenshots.takeScreenshot('Manage Access Request');
            }
        })
    }

    this.clickOnAddNewUserButton = function(){
    buttonActions.click(addNewUser_btn);
    }

    this.verifyAddNewUserButtonIsnotDisplayed = function(){
       expect(addNewUser_btn.isPresent()).toBe(false);
        }
    
    

    this.verifyNewUserProfilePopUp = function () {
        var i = 0;
        waitActions.waitForElementIsAvailable(userProfile_lbl);
        waitActions.waitForElementIsAvailable(userRights_lbl);
        expect(element(by.xpath(userID)).getAttribute('disabled')).toBe('true');
        expect(element(by.xpath(newUsername)).getAttribute('disabled')).toBe('true');
        expect(tenantList.count()).toBe(7);
        tenantList.each(function (element) {
            element.getText().then(function (text) {

                expect(text).toContain(constantsFile.HomePageConstants.securityEditUser[i]);
                i++;
            });
        });
        waitActions.waitForElementIsAvailable(tenantList);
        waitActions.waitForElementIsAvailable(activityRigths);
        waitActions.waitForElementIsAvailable(addNewUserActive_btn);
        waitActions.waitForElementIsAvailable(manageTEQ);
        waitActions.waitForElementIsAvailable(manageBEQ);
        waitActions.waitForElementIsAvailable(manageRequest);
        screenshots.takeScreenshot('verifyNewUserProfilePopUp');
    }

    this.verifyValidINTLAccountDisplayResults = function(searchValue){
        buttonActions.selectDropdownbyNum(identity_List,1);
        inputBoxActions.typeAndEnter(identitySearch_txt,searchValue);
        buttonActions.click(searchButton);
        waitActions.waitForElementIsAvailable(searchResult);
        screenshots.takeScreenshot('verifyValidINTLAccountDisplayResults');

    }

    this.verifyInvalidIntlAccountDisplayErrMsg = function(searchValue){
    
      // waitActions.waitForElementIsAvailable(errorMessage);
       errorMessage.isDisplayed().then(function (res) {
           if (res) {
               console.log("true it returned");
            buttonActions.click(addUserPopupClose);
           }
       });
        buttonActions.selectDropdownbyNum(identity_List,1);
        inputBoxActions.typeAndEnter(identitySearch_txt,searchValue);
        buttonActions.click(searchButton);
        waitActions.waitForElementIsAvailable(errorMessage);
        errorMessage.getText().then(function (text) {
            var expectedString = "No match found for \"INTL/"+searchValue+"\"";
            console.log(expectedString);
            expect(text).toBe(expectedString);
            screenshots.takeScreenshot('verifyInvalidIntlAccountDisplayErrMsg');

       
        })
    }

    this.verifyInvalidCorpAccountDisplayErrMsg = function(searchValue){
       
                buttonActions.selectDropdownbyNum(identity_List,0);
                inputBoxActions.typeAndEnter(identitySearch_txt,searchValue);
                buttonActions.click(searchButton);
                waitActions.waitForElementIsAvailable(errorMessage);
                errorMessage.getText().then(function (text) {
                    var expectedString = "No match found for \"CORP/"+searchValue+"\"";
                    console.log(expectedString);
                    expect(text).toContain(expectedString);
                    screenshots.takeScreenshot('verifyInvalidCorpAccountDisplayErrMsg');
                });
    }

        this.updateTenant = function(){
                    dropdownactions.selectDropdownbyNum(tenantList,1);
                    waitActions.waitForElementIsEnabled(save_btn);
                    buttonActions.click(save_btn);
                    waitActions.waitForElementIsDisplayed(updateSuccessMsg);
                    updateSuccessMsg.getText().then(function (successMsg) {
                        expect(successMsg).toContain("updated successfully");
                        console.log('Success')
        
                    })
            }
        
            this.navigateToHomePage = function() {
                buttonActions.click(home);
            }

     
       this.getUserAndTenant= async () => {
                    browser.sleep(10000);
                    waitActions.waitForElementIsDisplayed(userTitle);
                    var text = await userTitle.getText();
                    var textSplit = text.split('\\');
                    var textSplitUserName = textSplit[1].split("(");
                    testData.User.tenant = textSplitUserName[1];
                    console.log(textSplitUserName);
                    console.log(tenant);                      
         } 
            
        this.VerifyAllTenantsExceptLVIS = function() {
            buttonActions.selectDropdownbyNum(tenantList,2);
            console.log(tenantList);  
            expect(tenantList).not.toContain("LVIS");
            console.log('LVIS not available')
                
        } 
}

