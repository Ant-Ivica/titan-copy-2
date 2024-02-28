module.exports = function () {
    'use Strict';


    // Mapping Tables - FilePreference
    var objFilePrfernce = require('../resources/FilePreferencesObjRepo.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var taskMapObjRepo = require('../resources/TaskMapObjRepo.json');

    //common - objects
    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var gridActions = require('../commons/gridFilterActions.js');
    var mouseActions = require('../commons/mouseActions.js');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var dropDownActions = new dropDownActions();
    var inputBoxActions = new inputBoxActions();
    var gridFilterActions = new gridActions();
    var mouseActions = new mouseActions();

    var loanAmt;

    //Customers - Objects
    var lbl_FastFilePreference = objLocator.findLocator(objFilePrfernce.filePagePreference.lblFastFilePreference)
    var preference_grid = objLocator.findLocator(objFilePrfernce.filePagePreference.preferencegrid);
    var producttype = objLocator.findLocator(objFilePrfernce.filePagePreference.producttype);
    var productdropdown = objLocator.findLocator(objFilePrfernce.filePagePreference.producttype);

    //Add File Preference
    var butn_FilePreference = objLocator.findLocator(objFilePrfernce.filePagePreference.addFilePreference);
    var txt_FilePreference = objLocator.findLocator(objFilePrfernce.filePagePreference.addFilePrefenceText);
    //Fiters for Grid 
    //text box name
    var txt_name = objLocator.findLocator(objFilePrfernce.filePagePreference.name);
    var txt_programType = objLocator.findLocator(objFilePrfernce.filePagePreference.programType);
    var txt_searchType = objLocator.findLocator(objFilePrfernce.filePagePreference.searchType);
    var txt_loanPurpose = objLocator.findLocator(objFilePrfernce.filePagePreference.loanPurpose);
    var txt_customerLocation = objLocator.findLocator(objFilePrfernce.filePagePreference.customerLocation);
    var txt_customerName = objLocator.findLocator(objFilePrfernce.filePagePreference.txt_customerName);
    var txt_tenant = objLocator.findLocator(objFilePrfernce.filePagePreference.tenant);

    //Button Name
    var butn_name = objLocator.findLocator(objFilePrfernce.filePagePreference.btn_namecancel);
    var butn_programType = objLocator.findLocator(objFilePrfernce.filePagePreference.btn_programTypecancel);
    var butn_searchType = objLocator.findLocator(objFilePrfernce.filePagePreference.btn_searchTypeCancel);
    var butn_loanPurpose = objLocator.findLocator(objFilePrfernce.filePagePreference.btn_loanPurposeCancel);
    var butn_customerLocation = objLocator.findLocator(objFilePrfernce.filePagePreference.btn_customerLocationCancel);
    var butn_tenant = objLocator.findLocator(objFilePrfernce.filePagePreference.btn_tenantCancel);

    //result_row
    var resultrow_Name = objLocator.findLocator(objFilePrfernce.filePagePreference.xpathresultrow_Name);
    var resultrow_programtype = objLocator.findLocator(objFilePrfernce.filePagePreference.xpathresultrow_ProgramType);
    var resultrow_SearchType = objLocator.findLocator(objFilePrfernce.filePagePreference.xpathresultrow_SearchType);
    var resultrow_LoanPurpose = objLocator.findLocator(objFilePrfernce.filePagePreference.xpathresultrow_LoanPurpose);
    var resultrow_CustomerLocation = objLocator.findLocator(objFilePrfernce.filePagePreference.xpathresultrow_CustomerLocation);
    var resultrow_CustomerName = objLocator.findLocator(objFilePrfernce.filePagePreference.xpathresultrow_CustomerName);
    var resultrow_Tenant = objLocator.findLocator(objFilePrfernce.filePagePreference.xpathresultrow_Tenant);

    //Filters for 
    var addFilePrefernce = objLocator.findLocator(objFilePrfernce.filePagePreference.addFastFilePrefernce);
    var regionId = objLocator.findLocator(objFilePrfernce.filePagePreference.regionId);
    var state = objLocator.findLocator(objFilePrfernce.filePagePreference.state);
    var county = objLocator.findLocator(objFilePrfernce.filePagePreference.county);
    var loanAmount = objLocator.findLocator(objFilePrfernce.filePagePreference.loanAmount);
    var fast = objLocator.findLocator(taskMapObjRepo.TaskMapObjRepo.fastDropDown);
    var filePreference = objLocator.findLocator(objFilePrfernce.filePagePreference.filePreference)

    //Add File Preference
    var add_FileprefName = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addFileName);
    var add_FilepreLoanPurpose = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addLoanPurpose);
    var add_FilepreCustomer = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addCustomer);
    var add_FilepreCustomerLoc = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addCustomerLocation);
    //var add_FilepreProduct = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addProduct);
    var add_FilepreLoanAmount = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addLoanAmount);
    var add_FilepreRegionId = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addRegionID);
    var add_FilepreState = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addState);
    var add_FilepreCounty = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addCounty);
    var add_FilepreProgramType = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addProgramType);
    // var add_FilepreBusinessType = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addBusinessProgramType);
    // var add_FileProductType = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addProductType);
    var add_FileSearchType = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addSearchType);
    var add_FilepreSaveButton = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addSaveButton);
    var add_FilepreCancelButton = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addCancelButton);
    var add_successMessage = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_successAddFilePreference);
    var add_StateCounty = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addStateCounty);
    var add_RemoveStateCounty = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_addRemoveStateCounty);
    var xpath_NextPage = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_NextPage);
    var xpath_UpdateSuccess = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_UpdateSuccess);

    var FirstRow = objLocator.findLocator(objFilePrfernce.filePagePreference.FirstRow);
    var add_FilepreDeleteButton = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_DeleteButton);
    var add_FilepreCancelButton = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_CancelButton);
    var add_FilepreDeleteConfirmMessage = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_confirmDeleteMessage);
    var add_FilepreDeleteConfirmYes = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_confirmDeleteYes);
    var add_FilepreDeleteConfirmNo = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_confirmDeleteNo);
    var DeleteSuccessMessage = objLocator.findLocator(objFilePrfernce.filePagePreference.DeleteSuccessMessage);

    //Extra Search Itmes.
    var search_RegionId = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_RegionIdFilter);
    var search_State = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_StateFilter);
    var search_County = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_CountyFilter);
    var search_LoanAmount = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_LoanAmount);
    var search_Find = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_Search);
    var search_Reset = objLocator.findLocator(objFilePrfernce.filePagePreference.xpath_ResetButton);
    var columnGridMenu = objLocator.findLocator(objFilePrfernce.filePagePreference.columnGridMenu);
    var SelectCustName = objLocator.findLocator(objFilePrfernce.filePagePreference.SelectCustName);


    this.openSearchPage = function (path) {
        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.openFastFilePreference = function (path) {
        buttonActions.click(fast)
        buttonActions.click(filePreference);
        waitActions.waitForElementIsDisplayed(preference_grid);
    };

    this.UpdateFilePreference = function (path)
    {
        buttonActions.doubleClick(preference_grid);
        dropDownActions.selectDropdownbyValue(producttype, "COOP Lien Search");
        buttonActions.click(add_FilepreSaveButton);        
    }

    this.addCustomerNameColumn = function () {
        buttonActions.click(columnGridMenu);
        buttonActions.click(SelectCustName);
    }

    this.filterByRegion = function () {
        dropDownActions.selectDropdownbyNum(search_RegionId, 1);
    }

    this.filterByState = function () {
        dropDownActions.selectDropdownbyNum(search_State, 1);
    }

    this.filterByCounty = function () {
        browser.sleep(500);
        dropDownActions.selectDropdownbyNum(search_County, 0);
    }

    this.addLoanAmount = function () {
        inputBoxActions.type(search_LoanAmount, 6000);
    }

    this.validateSearchResult = function () {
        buttonActions.click(search_Find);
        waitActions.waitForElementIsDisplayed(search_Find);
        waitActions.waitForElementIsDisplayed(resultrow_Name);
        console.log("Success Test for Search Filter");
        screenshots.takeScreenshot('Suceess for Search Filter');
        browser.sleep(500);
    }

    this.validateResetButton = function () {
        buttonActions.click(search_Reset);
        //console.log("Sucess Reset filters");
        screenshots.takeScreenshot('Success for Reset Filters');
    }

    this.gridFilterByFieldName = function (FilterBy) {
        if (FilterBy == "Name") {
            gridFilterActions.filter(txt_name, "AutomationQA", resultrow_Name);
        }
        else if (FilterBy == "ProgramType") {
            gridFilterActions.filter(txt_programType, "Common", resultrow_programtype);
        }
        else if (FilterBy == "SearchType") {
            gridFilterActions.filter(txt_searchType, "Tax Search", resultrow_SearchType);
        }
        else if (FilterBy == "LoanPurpose") {
            gridFilterActions.filter(txt_loanPurpose, "Accommodation", resultrow_LoanPurpose);
        }
        else if (FilterBy == "CustomerLocation") {
            gridFilterActions.filter(txt_customerLocation, "Offerpad", resultrow_CustomerLocation);
        }
        else if (FilterBy == "CustomerName") {
            gridFilterActions.filter(txt_customerName, "Bank of America", resultrow_CustomerName);
        }
        else if (FilterBy == "Tenant") {
            gridFilterActions.filter(txt_tenant, "Agency", resultrow_Tenant);
        }
    }

    this.filterByInvalidData = function (FilterBy) {
        if (FilterBy == "CustomerID")
            inputBoxActions.type(filterByCustomerID, 100000)
        console.log("No records to display");
        return this;
    }

    this.clearFilter = function (Clear) {
        if (Clear == "Name")
            buttonActions.click(butn_name);
        else if (Clear == "ProgramType")
            buttonActions.click(butn_programType);
        else if (Clear == "SearchType")
            buttonActions.click(butn_searchType);
        else if (Clear == "LoanPurpose")
            buttonActions.click(butn_loanPurpose);
        else if (Clear == "CustomerLocation")
            buttonActions.click(butn_customerLocation);
        else if (Clear == "Tenant") {
            browser.actions().mouseMove(butn_tenant).perform();
            browser.actions().click().perform();
        }

    }

    this.openAddFilePrefrence = function () {
        buttonActions.click(butn_FilePreference);
        waitActions.waitForElementIsDisplayed(txt_FilePreference);
    }

    this.enterAddFilePreference = function () {
        inputBoxActions.type(add_FileprefName, "AutomationQA");
        dropDownActions.selectDropdownbyNum(add_FilepreLoanPurpose, 1);
        dropDownActions.selectDropdownbyNum(add_FilepreCustomer, 1);
        //dropDownActions.selectDropdownbyNum(add_FilepreCustomerLoc, 1);
        inputBoxActions.type(add_FilepreLoanAmount, 5000);
        dropDownActions.selectDropdownbyNum(add_FilepreRegionId, 1);
        dropDownActions.selectDropdownbyNum(add_FilepreState, 1);
        browser.sleep(1000);
        dropDownActions.selectDropdownbyNum(add_FilepreCounty, 1);
        buttonActions.click(add_StateCounty);
        waitActions.waitForElementIsDisplayed(add_RemoveStateCounty);
        // dropDownActions.selectDropdownbyNum(add_FilepreProgramType, 1);
        // console.log("Program Type");
        dropDownActions.selectDropdownbyValue(producttype, "COOP Lien Search");
        dropDownActions.selectDropdownbyNum(add_FileSearchType, 1);
        console.log("Search Type");
        screenshots.takeScreenshot('File Preference With values');
        buttonActions.click(add_FilepreSaveButton);
        waitActions.waitForElementIsDisplayed(add_successMessage);
        this.validateGirdData(add_successMessage, "FAST File Preference Info record was added successfully")
        screenshots.takeScreenshot("Successfully Added FilePreference")
    }

    this.verifyAddPrefernceButtonAvailable = function () {
        // expect(butn_FilePreference.isPresent()).toBe(false);
        screenshots.takeScreenshot("Permission Denied for User to ADD");
    }

    this.validateGirdData = function (resultRow, dataValidate) {
        resultRow.getText().then(function _onSuccess(text) {
            console.log(text);
            expect(text).toContain(dataValidate);
        }
        ).catch(function _onFailure(err) {
            console.log(err);
        })
        screenshots.takeScreenshot('Grid Search Result for_' + dataValidate);
    }

    this.resetSearchResult = function () {
        buttonActions.click(search_Reset);
        //This need to be correctted since not able to read the value from Loan Amount 
        loanAmt = search_LoanAmount.getText().then(function _onSuccess(text) {
            console.log(text);
            //expect(text).toContain('');
            //expect(text).toContain(chkLoanAmt);
            return text;
        }
        )
        browser.sleep(1000);
        console.log(loanAmt);
        screenshots.takeScreenshot('Success for Reset Filters');
    }

    this.updateFilePreference = function () {
        var i = 0;
        this.gridFilterByFieldName("Name");
        browser.sleep(1000);
        browser.actions().doubleClick(resultrow_Name).perform();
        resultrow_Name.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(resultrow_Name).perform();
            }
        });

        waitActions.waitForElementIsDisplayed(add_FilepreDeleteButton);
    }


    this.verifyFilePreferenceUpdateEnabled = function () {
        var i = 0;
        inputBoxActions.type(add_FileprefName, "AutomationQA Updated")
        // browser.sleep(1000);
        add_FilepreSaveButton.getAttribute('disbaled').then(function onDisbaled() {
            i++;
            console.log("Please update the Fast FilePreference")
            screenshots.takeScreenshot("Their is no modification to Save");
        });
        if (i === 0) {
            buttonActions.click(add_FilepreSaveButton);
            waitActions.waitForElementIsAvailable(xpath_UpdateSuccess);
            mouseActions.mouseMove(xpath_UpdateSuccess);
            xpath_UpdateSuccess.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Fast FilePreference Updated Successfully");
        }
    }

    this.verifyFilePreferenceUpdateDisabled = function () {
        add_FilepreSaveButton.getAttribute('disbaled').then(function onDisbaled() {
            screenshots.takeScreenshot("Permission Denied for User to UPDATE");
            buttonActions.click(add_FilepreCancelButton);
        });
    }

    this.deletFilePreference = function (res) {
        this.gridFilterByFieldName("Name");
        browser.sleep(1000);
        browser.actions().doubleClick(resultrow_Name).perform();
        resultrow_Name.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(resultrow_Name).perform();
            }
        });
        waitActions.waitForElementIsDisplayed(add_FilepreDeleteButton);
    }

    this.verifyFilePreferenceDeleteEnabled = function () {
        buttonActions.click(add_FilepreDeleteButton);
        browser.sleep(500);
        waitActions.waitForElementIsDisplayed(add_FilepreDeleteConfirmMessage);
        buttonActions.click(add_FilepreDeleteConfirmYes);
        waitActions.waitForElementIsDisplayed(DeleteSuccessMessage)
        mouseActions.mouseMove(DeleteSuccessMessage);
        browser.sleep(500);
        DeleteSuccessMessage.getText().then(function (text) {
            expect(text).toContain("deleted successfully");
        });
        screenshots.takeScreenshot('Delete Fast FilePreference');
        return this;
    }

    this.verifyFilePreferenceDeleteDisabled = function () {
        add_FilepreDeleteButton.getAttribute('disbaled').then(function () {
            console.log("This User Role doesn't have the Permission to Deleting File Preference");
            screenshots.takeScreenshot("Permission Denied for User to DELELTE")
            buttonActions.click(add_FilepreCancelButton);
        });
    }
};



