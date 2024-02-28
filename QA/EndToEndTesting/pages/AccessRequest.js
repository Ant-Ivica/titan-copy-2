module.exports = function () {
    'use Strict';

    var objRepo = require('../resources/AccessRequest.json');
    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var verifyActions = require('../commons/verifyActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var mouseActions = require('../commons/mouseActions.js');
    var checkBoxActions = require('../commons/checkBoxActions.js');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var verifyActions = new verifyActions();
    var inputBoxActions = new inputBoxActions();
    var dropDownActions = new dropDownActions();
    var gridFilterActions = new gridFilterActions();
    var mouseActions = new mouseActions();
    var checkBoxActions = new checkBoxActions();

    var screenshots = require('protractor-take-screenshots-on-demand');

    //Navigate to   Access Request
    var accessRequestLabel = objLocator.findLocator(objRepo.accessRequestObj.accessRequestLabel);
    var addAccessRequest = objLocator.findLocator(objRepo.accessRequestObj.addAccessRequest);

    //Add Access Request
    var addAccessRequestLabel = objLocator.findLocator(objRepo.accessRequestObj.addAccessRequestLabel);
    var addfirstName = objLocator.findLocator(objRepo.accessRequestObj.addfirstName);
    var addlastName = objLocator.findLocator(objRepo.accessRequestObj.addlastName);
    var addEmail = objLocator.findLocator(objRepo.accessRequestObj.addEmail);
    var addPhone = objLocator.findLocator(objRepo.accessRequestObj.addPhone);
    var addCompanyName = objLocator.findLocator(objRepo.accessRequestObj.addCompanyName);
    var addProjectName = objLocator.findLocator(objRepo.accessRequestObj.addProjectName);
    var addTitleSettlementchk = objLocator.findLocator(objRepo.accessRequestObj.addTitleSettlementchk);
    var addchkInteractiveOfficechk = objLocator.findLocator(objRepo.accessRequestObj.addchkInteractiveOfficechk);
    var addOtherchk = objLocator.findLocator(objRepo.accessRequestObj.addOtherchk);
    var addOther = objLocator.findLocator(objRepo.accessRequestObj.addOther);
    var addComments = objLocator.findLocator(objRepo.accessRequestObj.addComments);
    var addActiveButton = objLocator.findLocator(objRepo.accessRequestObj.addActiveButton);
    var addApproveButton = objLocator.findLocator(objRepo.accessRequestObj.addApproveButton);
    var addDeclineButton = objLocator.findLocator(objRepo.accessRequestObj.addDeclineButton);
    var addDeactivateButton = objLocator.findLocator(objRepo.accessRequestObj.addDeactivateButton);
    var addSaveButton = objLocator.findLocator(objRepo.accessRequestObj.addSaveButton);
    var addCancelButton = objLocator.findLocator(objRepo.accessRequestObj.addCancelButton);
    var addSuccessmessage = objLocator.findLocator(objRepo.accessRequestObj.addSuccessmessage);

    //Filters
    var firstName = objLocator.findLocator(objRepo.accessRequestObj.firstName);
    var lastName = objLocator.findLocator(objRepo.accessRequestObj.lastName);
    var emailId = objLocator.findLocator(objRepo.accessRequestObj.emailId);
    var phone = objLocator.findLocator(objRepo.accessRequestObj.phone);
    var companyName = objLocator.findLocator(objRepo.accessRequestObj.companyName);
    var projectName = objLocator.findLocator(objRepo.accessRequestObj.projectName);
    var status = objLocator.findLocator(objRepo.accessRequestObj.status);

    var delfirstName = objLocator.findLocator(objRepo.accessRequestObj.delfirstName);
    var dellastName = objLocator.findLocator(objRepo.accessRequestObj.dellastName);
    var delemailId = objLocator.findLocator(objRepo.accessRequestObj.delemailId);
    var delphone = objLocator.findLocator(objRepo.accessRequestObj.delphone);
    var delcompanyName = objLocator.findLocator(objRepo.accessRequestObj.delcompanyName);
    var delprojectName = objLocator.findLocator(objRepo.accessRequestObj.delprojectName);
    var delstatus = objLocator.findLocator(objRepo.accessRequestObj.delstatus);

    var row_firstName = objLocator.findLocator(objRepo.accessRequestObj.row_firstName);
    var row_lastName = objLocator.findLocator(objRepo.accessRequestObj.row_lastName);
    var row_Email = objLocator.findLocator(objRepo.accessRequestObj.row_Email);
    var row_Phone = objLocator.findLocator(objRepo.accessRequestObj.row_Phone);
    var row_CompanyName = objLocator.findLocator(objRepo.accessRequestObj.row_CompanyName);
    var row_ProjectName = objLocator.findLocator(objRepo.accessRequestObj.row_ProjectName);
    var row_Status = objLocator.findLocator(objRepo.accessRequestObj.row_Status);
    var accessRequestMenu = objLocator.findLocator(objRepo.accessRequestObj.accessRequestMenu);
    //Modify/Apporve/Decline/Deactivate
    var modifyAccessRequestlbl = objLocator.findLocator(objRepo.accessRequestObj.modifyAccessRequestlbl);
    //Var Declarations
    var random = getRandomNum(1, 100);

    this.openSearchPage = function (path) {
        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.navigateToAccessRequest = function (path) {
        browser.get(path);
        waitActions.waitForElementIsDisplayed(accessRequestMenu);
        buttonActions.click(accessRequestMenu);
    }

    this.isPageLoaded = function () {
        //waitActions.waitForElementIsDisplayed(accessRequestLabel);
        waitActions.waitForElementIsDisplayed(row_firstName);
        //random = getRandomNum(1, 100);
        console.log(random);
        return this;
    };

    this.gridFilterByFieldName = function (FilterBy, value) {
        console.log(random);
        if (FilterBy == "firstName") {
            gridFilterActions.filter(firstName, value, row_firstName);
        }
        else if (FilterBy == "lastName") {
            gridFilterActions.filter(lastName, value, row_lastName);
        }
        else if (FilterBy == "emailId") {
            gridFilterActions.filter(emailId, "Ellen" + random + ".Degeneres" + random + "@firstam.com", row_Email);
        }
        else if (FilterBy == "phone") {
            gridFilterActions.filter(phone, value, row_Phone);
        }
        else if (FilterBy == "companyName") {
            gridFilterActions.filter(companyName, value, row_CompanyName);
        }
        else if (FilterBy == "projectName") {
            gridFilterActions.filter(projectName, value, row_ProjectName);
        }
        else if (FilterBy == "status") {
            gridFilterActions.filter(status, value, row_Status);
        }
    };

    this.filterByInvalidData = function (FilterBy) {
        if (FilterBy == "CustomerID")
            inputBoxActions.type(filterByCustomerID, 100000)
        console.log("No records to display");
        return this;
    };

    this.clearFilter = function (Clear) {
        if (Clear == "firstName")
            buttonActions.click(delfirstName);
        else if (Clear == "lastName")
            buttonActions.click(dellastName);
        else if (Clear == "emailId")
            buttonActions.click(delemailId);
        else if (Clear == "phone")
            buttonActions.click(delphone);
        else if (Clear == "companyName")
            buttonActions.click(delcompanyName);
        else if (Clear == "projectName")
            buttonActions.click(delprojectName);
        else if (Clear == "status") {
            browser.actions().mouseMove(delstatus).perform();
            browser.actions().click().perform();
        }
    };

    this.addNewAccessRequest = function () {
        console.log(random);
        buttonActions.click(addAccessRequest);
        waitActions.waitForElementIsDisplayed(addAccessRequestLabel);
        screenshots.takeScreenshot("Add Access Request");
        inputBoxActions.type(addfirstName, "Ellen" + random);
        inputBoxActions.type(addlastName, "Degeneres" + random);
        inputBoxActions.type(addEmail, "Ellen" + random + ".Degeneres" + random + "@firstam.com");
        inputBoxActions.type(addPhone, "7234765234");
        inputBoxActions.type(addProjectName, "LVIS");
        inputBoxActions.type(addCompanyName, "FirstAmerican");
        checkBoxActions.click(addTitleSettlementchk);
        checkBoxActions.click(addchkInteractiveOfficechk);
        checkBoxActions.click(addOtherchk);
        inputBoxActions.type(addOther, "Other Comments");
        inputBoxActions.type(addComments, "Comments");
        console.log("Test" + random);
        buttonActions.click(addSaveButton);
        waitActions.waitForElementIsDisplayed(addSuccessmessage);
        browser.sleep(1000);
        addSuccessmessage.getText().then(function (successMsg) {
            expect(successMsg).toContain("added successfully");
        });
        screenshots.takeScreenshot("Added New Access Request Successfully");
    }

    this.addAccessRequestWithParam = function (firstName, lastName) {
        console.log(random);
        buttonActions.click(addAccessRequest);
        waitActions.waitForElementIsDisplayed(addAccessRequestLabel);
        screenshots.takeScreenshot("Add Access Request");
        inputBoxActions.type(addfirstName, firstName + random);
        inputBoxActions.type(addlastName, lastName + random);
        inputBoxActions.type(addEmail, firstName + random + "." + lastName + random + "@firstam.com");
        inputBoxActions.type(addPhone, "72894765234");
        inputBoxActions.type(addProjectName, "LVIS");
        inputBoxActions.type(addCompanyName, "First American");
        checkBoxActions.click(addTitleSettlementchk);
        checkBoxActions.click(addchkInteractiveOfficechk);
        checkBoxActions.click(addOtherchk);
        inputBoxActions.type(addOther, "Other Comments");
        inputBoxActions.type(addComments, "Comments");
        console.log("Test" + random);
        buttonActions.click(addSaveButton);
        waitActions.waitForElementIsDisplayed(addSuccessmessage);
        addSuccessmessage.getText().then(function (successMsg) {
            expect(successMsg).toContain("added successfully");
        });
        screenshots.takeScreenshot("Added New Access Request Successfully");
    }

    this.activeAccessRequest = function () {
        var i = 0;
        this.gridFilterByFieldName("firstName", "Ellen" + random);
        this.gridFilterByFieldName("lastName", "Degeneres" + random);
        this.gridFilterByFieldName("status", "New");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        inputBoxActions.type(addProjectName, "LVIS Test ");
        addActiveButton.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for Activating");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(addActiveButton);
            waitActions.waitForElementIsAvailable(addSuccessmessage);
            mouseActions.mouseMove(addSuccessmessage);
            addSuccessmessage.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Active the Access Request");
        }
    }


    this.approveAccessRequest = function () {
        var i = 0;
        this.gridFilterByFieldName("firstName", "Ellen" + random);
        this.gridFilterByFieldName("lastName", "Degeneres" + random);
        this.gridFilterByFieldName("status", "Active");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        inputBoxActions.type(addComments, "Approved the Customer");

        addApproveButton.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for Approving");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(addApproveButton);
            waitActions.waitForElementIsAvailable(addSuccessmessage);
            mouseActions.mouseMove(addSuccessmessage);
            addSuccessmessage.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Approved the Access Request");
        }
    }

    this.declineAccessRequest = function () {
        var i = 0;
        this.gridFilterByFieldName("firstName", "Tim" + random);
        this.gridFilterByFieldName("lastName", "Jefferey" + random);
        this.gridFilterByFieldName("status", "New");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        inputBoxActions.type(addComments, "Declined the Customer");

        addDeclineButton.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for Declining");
        });
        console.log(i);
        if (i === 0) {
            console.log("Got it Decline");
            buttonActions.click(addDeclineButton);
            waitActions.waitForElementIsAvailable(addSuccessmessage);
            mouseActions.mouseMove(addSuccessmessage);
            addSuccessmessage.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Decline Access Request");
        }
    }

    this.deactiveAccessRequest = function () {
        var i = 0;
        this.gridFilterByFieldName("firstName", "Ellen" + random);
        this.gridFilterByFieldName("lastName", "Degeneres" + random);
        this.gridFilterByFieldName("status", "Approve");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        inputBoxActions.type(addComments, "Deactivated the Customer");

        addDeactivateButton.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for Deactivate");
        });
        console.log(i);
        if (i === 0) {
            console.log("Got it Deactivate");
            buttonActions.click(addDeactivateButton);
            waitActions.waitForElementIsAvailable(addSuccessmessage);
            mouseActions.mouseMove(addSuccessmessage);
            addSuccessmessage.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Deactivate Access Request");
        }
    }

    this.addAccessRequestAvailable = function () {
        //expect(addAccessRequest.isPresent()).toBe(false);
        screenshots.takeScreenshot("USER Doesnt have the permission to ADD");

        //buttonActions.click(addCancelButton);
    }

    this.declineTestForAdmin = function () {
        var i = 0;
        this.gridFilterByFieldName("status", "New");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        inputBoxActions.type(addComments, "Declined the Customer");

        addDeclineButton.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for Declining");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(addDeclineButton);
            waitActions.waitForElementIsAvailable(addSuccessmessage);
            mouseActions.mouseMove(addSuccessmessage);
            addSuccessmessage.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Decline Access Request");
        }
    }

    this.declineTestForUser = function () {
        this.gridFilterByFieldName("status", "New");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        addDeclineButton.getAttribute('disbaled').then(function () {
            screenshots.takeScreenshot("This USER Does not have the permission to DECLINE");
            buttonActions.click(addCancelButton);
            console.log("Please select the correct order for Declining");
        });
    }

    this.activeTestForAdmin = function () {
        var i = 0;
        this.gridFilterByFieldName("status", "New");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        inputBoxActions.type(addComments, "Activate the Customer");
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        addActiveButton.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for Declining");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(addActiveButton);
            waitActions.waitForElementIsAvailable(addSuccessmessage);
            mouseActions.mouseMove(addSuccessmessage);
            addSuccessmessage.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Active Access Request for Admin");
        }
    }

    this.activeTestForUser = function () {
        this.gridFilterByFieldName("status", "New");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        addActiveButton.getAttribute('disbaled').then(function () {
            console.log("This User doesnt have permission to ACTIVE");
            screenshots.takeScreenshot("This USER Does not have the permission to ACTIVE");
            buttonActions.click(addCancelButton);
        });
    }

    this.approveTestForAdmin = function () {
        var i = 0;
        this.gridFilterByFieldName("status", "Active");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        inputBoxActions.type(addComments, "Approved the Customer");

        addApproveButton.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for Approving");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(addApproveButton);
            waitActions.waitForElementIsAvailable(addSuccessmessage);
            mouseActions.mouseMove(addSuccessmessage);
            addSuccessmessage.getText().then(function (text) {
                expect(text).toContain("updated successfully");
            });
            screenshots.takeScreenshot("Approved the Access Request for ADMIN");
        }
    }

    this.approveTestForUser = function () {
        this.gridFilterByFieldName("status", "Active");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        waitActions.waitForElementIsDisplayed(addApproveButton);
        addApproveButton.getAttribute('disbaled').then(function () {
            console.log("This User doesnt have permission to APPROVE");
            screenshots.takeScreenshot("This USER Does not have the permission to ACTIVE");
            buttonActions.click(addCancelButton);
        });
    }



    this.deactiveTestForAdminAndUser = function () {
        this.gridFilterByFieldName("status", "Active");
        browser.actions().doubleClick(row_firstName).perform();
        row_firstName.isDisplayed().then(function (res) {
            if (res) {
                browser.actions().doubleClick(row_firstName).perform();
            }
        });
        waitActions.waitForElementIsAvailable(modifyAccessRequestlbl);
        waitActions.waitForElementIsDisplayed(addDeactivateButton);
        addDeactivateButton.getAttribute('disbaled').then(function () {
            console.log("This User doesnt have permission to DEActive");
            screenshots.takeScreenshot("This USER Does not have the Permission to DEACTIVE")
            buttonActions.click(addCancelButton);
        })
    }


}

getRandomNum = function (min, max) {
    return parseInt(Math.random() * (max - min) + min);
};

