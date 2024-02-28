module.exports = function () {
    'use Strict';

    var objRepo = require('../resources/OfficeMapObjRepo.json');
    var taskMapObjRepo = require('../resources/TaskMapObjRepo.json');

    var utilspage = require('../utils/objectLocator.js');
    var buttonActions = require('../commons/buttonActions.js');
    var waitActions = require('../commons/waitActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var verifyActions = require('../commons/verifyActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var mouseActions = require('../commons/mouseActions.js');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var gridFilterActions = new gridFilterActions();
    var buttonActions = new buttonActions();
    var verifyActions = new verifyActions();
    var inputBoxActions = new inputBoxActions();
    var dropDownActions = new dropDownActions();
    var mouseActions = new mouseActions();

    var screenshots = require('protractor-take-screenshots-on-demand');
    var constantsFile = require('../resources/constantsTower.json');
    var testData = require('../resources/testData.json');


    //xpath
    var pagePath = objLocator.findLocator(objRepo.officeMapObjRepo.pageLabel);
    var fast = objLocator.findLocator(taskMapObjRepo.TaskMapObjRepo.fastDropDown);
    var officeMapLink = objLocator.findLocator(objRepo.officeMapObjRepo.officeMapLink);

    //Add Office Map
    addOfficeMap = objLocator.findLocator(objRepo.officeMapObjRepo.addOfficeMap);
    addOfficeMapLabel = objLocator.findLocator(objRepo.officeMapObjRepo.addOfficeMapLabel);
    addDescription = objLocator.findLocator(objRepo.officeMapObjRepo.addDescription);
    addProviderId = objLocator.findLocator(objRepo.officeMapObjRepo.addProviderId);
    addCustomerId = objLocator.findLocator(objRepo.officeMapObjRepo.addCustomerId);
    addLocationId = objLocator.findLocator(objRepo.officeMapObjRepo.addLocationId);
    addContactId = objLocator.findLocator(objRepo.officeMapObjRepo.addContactId);
    addState = objLocator.findLocator(objRepo.officeMapObjRepo.addState);
    addCounty = objLocator.findLocator(objRepo.officeMapObjRepo.addCounty);
    addStateCountyButton = objLocator.findLocator(objRepo.officeMapObjRepo.addStateCountyButton);
    addStateCountyLabel = objLocator.findLocator(objRepo.officeMapObjRepo.addStateCountyLabel);
    addTitleRegionId = objLocator.findLocator(objRepo.officeMapObjRepo.addTitleRegionId);
    addTitleOfficeId = objLocator.findLocator(objRepo.officeMapObjRepo.addTitleOfficeId);
    addTitleOfficer = objLocator.findLocator(objRepo.officeMapObjRepo.addTitleOfficer);
    addEscrowRegionId = objLocator.findLocator(objRepo.officeMapObjRepo.addEscrowRegionId);
    addEscrowOfficeId = objLocator.findLocator(objRepo.officeMapObjRepo.addEscrowOfficeId);
    addEscrowOfficer = objLocator.findLocator(objRepo.officeMapObjRepo.addEscrowOfficer);
    addEscrowAssistant = objLocator.findLocator(objRepo.officeMapObjRepo.addEscrowAssistant);
    addAutToken = objLocator.findLocator(objRepo.officeMapObjRepo.addAutToken);
    addSaveButton = objLocator.findLocator(objRepo.officeMapObjRepo.addSaveButton);
    addCancelButton = objLocator.findLocator(objRepo.officeMapObjRepo.addCancelButton);
    addOfficeMapSuccess = objLocator.findLocator(objRepo.officeMapObjRepo.addOfficeMapSuccess);

    updateOfficeMapeSuccess = objLocator.findLocator(objRepo.officeMapObjRepo.updateOfficeMapeSuccess);

    //Filter Office Map
    filterProviderId = objLocator.findLocator(objRepo.officeMapObjRepo.filterProviderId);
    filterExternalId = objLocator.findLocator(objRepo.officeMapObjRepo.filterExternalId);
    filterRegionId = objLocator.findLocator(objRepo.officeMapObjRepo.filterRegionId);
    filterTitleOfficeId = objLocator.findLocator(objRepo.officeMapObjRepo.filterTitleOfficeId);
    filterEscrowOfficeId = objLocator.findLocator(objRepo.officeMapObjRepo.filterEscrowOfficeId);
    filterTitleOfficer = objLocator.findLocator(objRepo.officeMapObjRepo.filterTitleOfficer);
    filterEscrowOfficer = objLocator.findLocator(objRepo.officeMapObjRepo.filterEscrowOfficer);
    filterLocationId = objLocator.findLocator(objRepo.officeMapObjRepo.filterLocationId);
    filterDescription = objLocator.findLocator(objRepo.officeMapObjRepo.filterDescription);
    filterTenant = objLocator.findLocator(objRepo.officeMapObjRepo.filterTenant);

    delProviderId = objLocator.findLocator(objRepo.officeMapObjRepo.delProviderId);
    delExternalId = objLocator.findLocator(objRepo.officeMapObjRepo.delExternalId);
    delRegionId = objLocator.findLocator(objRepo.officeMapObjRepo.delRegionId);
    delTitleOfficeId = objLocator.findLocator(objRepo.officeMapObjRepo.delTitleOfficeId);
    delEscrowOfficeId = objLocator.findLocator(objRepo.officeMapObjRepo.delEscrowOfficeId);
    delTitleOfficer = objLocator.findLocator(objRepo.officeMapObjRepo.delTitleOfficer);
    delEscrowOfficer = objLocator.findLocator(objRepo.officeMapObjRepo.delEscrowOfficer);
    delLocationId = objLocator.findLocator(objRepo.officeMapObjRepo.delLocationId);
    delDescription = objLocator.findLocator(objRepo.officeMapObjRepo.delDescription);
    delTenant = objLocator.findLocator(objRepo.officeMapObjRepo.delTenant);

    row_ProviderId = objLocator.findLocator(objRepo.officeMapObjRepo.row_ProviderId);
    row_ExternalId = objLocator.findLocator(objRepo.officeMapObjRepo.row_ExternalId);
    row_RegionId = objLocator.findLocator(objRepo.officeMapObjRepo.row_RegionId);
    row_TitleOfficeId = objLocator.findLocator(objRepo.officeMapObjRepo.row_TitleOfficeId);
    row_EscrowOfficeId = objLocator.findLocator(objRepo.officeMapObjRepo.row_EscrowOfficeId);
    row_TitleOfficer = objLocator.findLocator(objRepo.officeMapObjRepo.row_TitleOfficer);
    row_EscrowOfficer = objLocator.findLocator(objRepo.officeMapObjRepo.row_EscrowOfficer);
    row_LocationId = objLocator.findLocator(objRepo.officeMapObjRepo.row_LocationId);
    row_Description = objLocator.findLocator(objRepo.officeMapObjRepo.row_Description);
    row_Tenant = objLocator.findLocator(objRepo.officeMapObjRepo.row_Tenant);

    //Update Delete
    editPopUpLabel = objLocator.findLocator(objRepo.officeMapObjRepo.editPopUpLabel);
    deleteButton = objLocator.findLocator(objRepo.officeMapObjRepo.deleteButton);
    confirmMessage = objLocator.findLocator(objRepo.officeMapObjRepo.confirmMessage);
    confirmYes = objLocator.findLocator(objRepo.officeMapObjRepo.confirmYes);
    confirmNo = objLocator.findLocator(objRepo.officeMapObjRepo.confirmNo);

    //Sate & County Filters
    filterState = objLocator.findLocator(objRepo.officeMapObjRepo.filterState);
    filterCounty = objLocator.findLocator(objRepo.officeMapObjRepo.filterCounty);
    filterSearch = objLocator.findLocator(objRepo.officeMapObjRepo.filterSearch);
    filterReset = objLocator.findLocator(objRepo.officeMapObjRepo.filterReset);
    //Variables
    var random = getRandomNum(1, 100);

    this.navigateToOfficeMapPage = function () {
        buttonActions.click(fast);
        buttonActions.click(officeMapLink);
        waitActions.waitForElementIsDisplayed(pagePath);
    }

    this.isPageLoaded = function () {
        //waitActions.waitForElementIsDisplayed(pagePath);
        waitActions.waitForElementIsDisplayed(row_ProviderId);
        return this;
    };

    this.addNewOfficeMap = function () {
        var i = 0;
        buttonActions.click(addOfficeMap);
        screenshots.takeScreenshot("Add New Office Map Popup");
        waitActions.waitForElementIsDisplayed(addOfficeMapLabel);
        inputBoxActions.type(addDescription, "FOM" + random);
        dropDownActions.selectDropdownbyNum(addProviderId, 1);
      //  dropDownActions.selectDropdownbyValue(addCustomerId, "Bank of America(10023)");
        //dropDownActions.selectDropdownbyNum(addCustomerId, 8);
        //dropDownActions.selectDropdownbyValue(addLocationId, "Bank of America-13398754(2324)");
        //dropDownActions.selectDropdownbyNum(addLocationId, 2);
        //dropDownActions.selectDropdownbyNum(addContactId, 1);
        dropDownActions.selectDropdownbyValue(addState, "California");
        dropDownActions.selectDropdownbyValue(addCounty, "SANTA CRUZ");
        buttonActions.click(addStateCountyButton);
        waitActions.waitForElementIsDisplayed(addStateCountyLabel);
        browser.sleep(500);

        waitActions.waitForElementIsAvailable(addTitleRegionId);
        dropDownActions.selectDropdownbyNum(addTitleRegionId, 6);
        browser.sleep(500);
        //dropDownActions.selectDropdownbyValue(addTitleRegionId, "California(410)");
        dropDownActions.selectDropdownbyNum(addTitleOfficeId, 1);
        browser.sleep(500);
        //dropDownActions.selectDropdownbyNum(addEscrowRegionId, 6);
        dropDownActions.selectDropdownbyNum(addEscrowOfficeId, 1);
        browser.sleep(500);
        addSaveButton.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select Mandatory fields for the saving Office Map");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(addSaveButton);
            waitActions.waitForElementIsAvailable(addOfficeMapSuccess);
            mouseActions.mouseMove(addOfficeMapSuccess);
            addOfficeMapSuccess.getText().then(function (text) {
                expect(text).toContain("added successfully");
            });
            screenshots.takeScreenshot("Added Office Map Successfully");
        }
    };

    this.addOfficeMapavailable = function () {
        // expect(addSaveButton.isPresent()).toBe(false);
        screenshots.takeScreenshot("Permission Denied for User to ADD");
    }

    this.gridFilterByFieldName = function (FilterBy, Value) {
        if (FilterBy == "providerId") {
            gridFilterActions.filter(filterProviderId, Value, row_ProviderId);
            screenshots.takeScreenshot('Provider Id');
        }
        else if (FilterBy == "externalId") {
            gridFilterActions.filter(filterExternalId, Value, row_ExternalId);
            screenshots.takeScreenshot('External ID');
        }
        else if (FilterBy == "regionID") {
            gridFilterActions.filter(filterRegionId, Value, row_RegionId);
            screenshots.takeScreenshot('Region Id');
        }
        else if (FilterBy == "titleOfficeId") {
            gridFilterActions.filter(filterTitleOfficeId, Value, row_TitleOfficeId);
            screenshots.takeScreenshot('Title Office');
        }
        else if (FilterBy == "escrowOfficeId") {
            gridFilterActions.filter(filterEscrowOfficeId, Value, row_EscrowOfficeId);
            screenshots.takeScreenshot('Escrow Office');
        }
        else if (FilterBy == "titleOfficer") {
            gridFilterActions.filter(filterTitleOfficer, Value, row_TitleOfficer);
            screenshots.takeScreenshot('Title Officer');
        }
        else if (FilterBy == "escrowOfficer") {
            gridFilterActions.filter(filterEscrowOfficer, Value, row_EscrowOfficer);
            screenshots.takeScreenshot('Escrow Officer');
        }
        else if (FilterBy == "location") {
            gridFilterActions.filter(filterLocationId, Value, row_LocationId);
            screenshots.takeScreenshot('Location');
        }
        else if (FilterBy == "description") {
            gridFilterActions.filter(filterDescription, "FOM", row_Description);
            screenshots.takeScreenshot('Description');
        }
        else if (FilterBy == "tenant") {
            gridFilterActions.filter(filterTenant, Value, row_Tenant);
            screenshots.takeScreenshot('Tenant');
        }
    }

    this.clearFilter = function (Clear) {
        if (Clear == "providerId") {
            buttonActions.click(delProviderId);
        }
        else if (Clear == "externalId") {
            buttonActions.click(delExternalId);
        }
        else if (Clear == "regionID") {
            buttonActions.click(delRegionId);
        }
        else if (Clear == "titleOfficeId") {
            buttonActions.click(delTitleOfficeId);
        }
        else if (Clear == "escrowOfficeId") {
            buttonActions.click(delEscrowOfficeId);
        }
        else if (Clear == "titleOfficer") {
            buttonActions.click(delTitleOfficer);
        }
        else if (Clear == "escrowOfficer") {
            buttonActions.click(delEscrowOfficer);
        }
        else if (Clear == "location") {
            buttonActions.click(delLocationId);
        }
        else if (Clear == "description") {
            buttonActions.click(delDescription);
        }
        else if (Clear == "tenant") {
            browser.actions().mouseMove(delTenant).perform();
            browser.actions().click().perform();
        }
    }

    this.openExistingDocument = function () {
        this.gridFilterByFieldName("description", "FOM");//"Test"+random);
        browser.actions().doubleClick(row_ProviderId).perform();
        row_ProviderId.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_ProviderId).perform();
            }
        });
        waitActions.waitForElementIsAvailable(editPopUpLabel);
    }

    this.stateAndCountyFiltertest = function () {
        dropDownActions.selectDropdownbyValue(filterState, "California");
        dropDownActions.selectDropdownbyValue(filterCounty, "SANTA CRUZ");
        buttonActions.click(filterSearch);
        waitActions.waitForElementIsDisplayed(row_Description);
        screenshots.takeScreenshot("State and County Filter test");
        buttonActions.click(filterReset);
    }

    this.editOfficeMapEnabled = function () {
        var i = 0;
        //this.openExistingDocument();
        dropDownActions.selectDropdownbyValue(addState, "Georgia");
        dropDownActions.selectDropdownbyValue(addCounty, "BROOKS");
        buttonActions.click(addStateCountyButton);
        waitActions.waitForElementIsDisplayed(addStateCountyLabel);
        browser.sleep(500);
        waitActions.waitForElementIsAvailable(addTitleOfficer);
        dropDownActions.selectDropdownbyNum(addTitleOfficer, 1);
        waitActions.waitForElementIsAvailable(addEscrowOfficer);
        dropDownActions.selectDropdownbyNum(addEscrowOfficer, 1);
        addSaveButton.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select Mandatory fields for the saving Office Map");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(addSaveButton);
            waitActions.waitForElementIsDisplayed(updateOfficeMapeSuccess);
            mouseActions.mouseMove(updateOfficeMapeSuccess);
            updateOfficeMapeSuccess.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Office Map Updated Successfully");
        }
    }

    this.editOfficeMapDisabled = function () {
        addSaveButton.getAttribute('disbaled').then(function onDisbaled() {
            screenshots.takeScreenshot("Permission Denied for User to UPDATE");
            buttonActions.click(addCancelButton);
        });
    }

    this.deleteOfficeMapEnabled = function () {
        waitActions.waitForElementIsDisplayed(deleteButton);
        buttonActions.click(deleteButton);
        browser.sleep(1000);
        // waitActions.waitForElementIsDisplayed(confirmMessage);
        waitActions.waitForElementIsDisplayed(confirmYes);
        buttonActions.click(confirmYes);
        waitActions.waitForElementIsDisplayed(addOfficeMapSuccess)
        addOfficeMapSuccess.getText().then(function (successMsg) {
            expect(successMsg).toContain("deleted successfully");
        });
        screenshots.takeScreenshot("Office Map Deleted Successfully");
    }

    this.deleteOfficeMapDisabled = function () {
        deleteButton.getAttribute('disbaled').then(function () {
            console.log("This User Role doesn't have the Permission to Deleting File Preference");
            screenshots.takeScreenshot("Permission Denied for User to DELELTE")
            buttonActions.click(addCancelButton);
        });
    }


    // this.getString = function () {
    //     var test = "External Reference Number 13477226-e2vdg35 was bound successfully to 5850486";

    //     var result = test.substring(25, test.indexOf("was")).trim();
    //     console.log(result);
    // }

    this.verifyAddFeatureDisabled = function () {
        console.log("User Role part executed");
        expect(addNewDocLink.isPresent()).toBe(false);
    }
}
getRandomNum = function (min, max) {
    return parseInt(Math.random() * (max - min) + min);
};