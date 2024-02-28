module.exports = function () {
    'use strict';
    // HOme Page
    var objRepo = require('../resources/FASTWebOrdersObjRepo.json');
    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var verifyAction = require('../commons/verifyActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var mouseActions = require('../commons/mouseActions.js');
    var dropdownActions = require('../commons/dropDownActions.js');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var verifyAction = new verifyAction();
    var inputBoxActions = new inputBoxActions();
    var mouseActions = new mouseActions();
    var dropdownActions = new dropdownActions();

    var fastWebOrdersTab = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.fastWebOrdersTab);
    var fastWebOrdersHeader = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.fastWebOrdersHeader);
    var orderSearchDropdown = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.orderSearchDropdown);
    var orderSearchValue = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.orderSearchValue);
    var orderSearch = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.orderSearch);
    var fastWebOrderColumnHeader = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.fastWebOrderColumnHeader);
    var customerRefColumnHeader = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.customerRefColumnHeader);
    var borrowerNameColumnHeader = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.borrowerNameColumnHeader);
    var propertyAddressColumnHeader = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.propertyAddressColumnHeader);
    var serviceColumnHeader = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.serviceColumnHeader);
    var portalOrderAlertColumnHeader = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.portalOrderAlertColumnHeader);
    var orderDateColumnHeader = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.orderDateColumnHeader);
    var orderDetailColumnHeader = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.orderDetailColumnHeader);
    var addOfficeButton = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.addOfficeButton);
    var addForwardToOffice = objLocator.findLocator(objRepo.fASTWebAddOfficeObjRepo.addForwardToOffice);
    var close = objLocator.findLocator(objRepo.fASTWebAddOfficeObjRepo.close);
    var submit = objLocator.findLocator(objRepo.fASTWebAddOfficeObjRepo.submit);
    var reload_Button = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.reload_Button);
    var userId = objLocator.findLocator(objRepo.fASTWebAddOfficeObjRepo.userId);
    var buid = objLocator.findLocator(objRepo.fASTWebAddOfficeObjRepo.buid);
    var firstName = objLocator.findLocator(objRepo.fASTWebAddOfficeObjRepo.firstName);
    var lastName = objLocator.findLocator(objRepo.fASTWebAddOfficeObjRepo.lastName);
    var fwResp = objLocator.findLocator(objRepo.fASTWebAddOfficeObjRepo.fwResp);
    var orderDetailButton = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.orderDetailButton);
    var fastWebOrdersPageElements = [fastWebOrdersHeader, orderSearchDropdown,
        orderSearchValue, fastWebOrderColumnHeader, customerRefColumnHeader,
        borrowerNameColumnHeader, propertyAddressColumnHeader, serviceColumnHeader,
        portalOrderAlertColumnHeader, orderDateColumnHeader, orderDetailColumnHeader, addOfficeButton];
    var fastwebOrderDetailTab = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.fastwebOrderDetailTab);
    var orderDetailsWindow = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.orderDetailsWindow);
    var fASTWeb = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.fASTWeb);
    var orderDate = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.orderDate);
    var customerRef = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.customerRef);
    var loanAmount = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.loanAmount);
    var salePrice = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.salePrice);
    var transactionType = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.transactionType);
    var propertyType = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.propertyType);
    var propertyUse = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.propertyUse);
    var customerOffice = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.customerOffice);
    var officeAddress = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.officeAddress);
    var contact = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.contact);
    var email = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.email);
    var phone = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.phone);
    var propertyAddress = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.propertyAddress);
    var APN = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.APN);
    var county = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.county);
    var legalDescription = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.legalDescription);
    var borrowerEntityType = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.borrowerEntityType);
    var maritalStatus = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.maritalStatus);
    var lastName = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.lastName);
    var firstName = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.firstName);
    var currentAddress = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.currentAddress);
    var spouseLastName = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.spouseLastName)
    var spouseFirstName = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.spouseFirstName);
    var serviceName = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.serviceName);
    var processor = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.processor);
    var address = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.address);
    var orderDeskType = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.orderDeskType);
    var contactName = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.contactName);
    var contactphone = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.contactphone);
    var contactemail = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.contactemail);
    var status = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.status);
    var fASTFile = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.fASTFile);
    var portalOrderAlert = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.portalOrderAlert);
    var comments = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.comments);
    var products = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.products);
    var fastWebOrderInputField = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.fastWebOrderInputField);
    var cancel = objLocator.findLocator(objRepo.fASTWebOrderDetailsObjRepo.cancel);
    var fastWebOrdersDetailsPageElements = [fastwebOrderDetailTab, orderDetailsWindow, fASTWeb, orderDate, customerRef,
        loanAmount, salePrice, transactionType, propertyType, propertyUse, customerOffice, officeAddress, contact, email, phone,
        propertyAddress, APN, county, legalDescription, borrowerEntityType, maritalStatus, lastName, firstName, currentAddress,
        spouseLastName, spouseFirstName, serviceName, processor, address, orderDeskType, contactName, contactphone, contactemail,
        status, fASTFile, portalOrderAlert, comments, products];
    var firstRow = objLocator.findLocator(objRepo.fastWebOrdersDataList.firstRow);
    var firstRowVerify = objLocator.findLocator(objRepo.fastWebOrdersDataList.firstRowVerify);
    var filterByTextServiceType = objLocator.findLocator(objRepo.fastWebOrdersDataList.filterByTextServiceType);
    var filterByTextFastWebOrderInput = objLocator.findLocator(objRepo.fastWebOrdersDataList.filterByTextFastWebOrderInput);
    var firstIndex = objLocator.findLocator(objRepo.fastWebOrdersDataList.firstIndex);
    var clearServiceType = objLocator.findLocator(objRepo.fastWebOrdersDataList.clearServiceType);
    var plusSymbol = objLocator.findLocator(objRepo.fastWebOrdersDataList.plusSymbol);
    var firstRowFirstColumn = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.firstRowFirstColumn);
    //var firstRowSecondColumn = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.firstRowSecondColumn);
    //var firstRowSecondColumn = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.firstRowSecondColumn);
    //var firstRowThirdColumn = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.firstRowThirdColumn);
    var firstRowFourthColumn = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.firstRowFourthColumn);
    //var firstRowFifthColumn = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.firstRowFifthColumn);
    //var firstRowSixthColumn = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.firstRowSixthColumn);
    var forwardToPage = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPage);
    //var forwardToPageFastWebNum=objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPageFastWebNum);
    // var forwardToPageOrderDate=objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPageOrderDate);
    // var forwardToPageCustomerRefOrLoanNum=objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPageCustomerRefOrLoanNum);
    // var forwardToPageBorrowerName=objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPageBorrowerName);
    // var forwardToPagePropertyAddressLine1=objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPagePropertyAddressLine1);
    // var forwardToPagePropertyAddressLine2=objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPagePropertyAddressLine2);
    // var forwardToPageServiceName=objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPageServiceName);
    // var forwardToPagePortOrderAlert=objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPagePortOrderAlert);
    var forwardToPageDropDown = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPageDropDown);
    var forwardToPageSubmitButton = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.forwardToPageSubmitButton);
    var successMessage = objLocator.findLocator(objRepo.fastWebOrdersRowDetails.successMessage);
    var orderSrch = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.orderSrch);
    var resultrow_Name = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.resultrow_Name);
    var search_Find = objLocator.findLocator(objRepo.fastWebOrdersObjRepo.xpath_Search);
    var screenshots = require('protractor-take-screenshots-on-demand');
    // After loading 'FastWeb Orders' page, check whether it properly loaded or not
    this.isPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(fastWebOrdersHeader);
        return this;
    };

    // Load 'FastWeb Orders' page
    this.clickOnFASTWebTab = function () {
        buttonActions.click(fastWebOrdersTab);
        return this;
    };

    // Check whether element is present or not 
    this.isElementPresent = function (item) {
        waitActions.waitForElementIsDisplayed(item);
        return this;
    };

    // // Get TransactionID from FASTWebOrders Page
    // this.getTransactionID = function () {
    //     firstRowFirstColumn.getText().then(function _onSuccess(text){
    //         console.log(text);
    //         return text;
    //     });
    // };

    //Verify Reload Button
    this.verifyReloadButton = function () {
        waitActions.waitForElementIsAvailable(firstRowFirstColumn);
        var transactionID = firstRowFirstColumn.getText().then(function _onSuccess(text){
            console.log(text);
        });
        browser.actions().click(firstRowFirstColumn).perform();
        browser.actions().doubleClick(firstRowFirstColumn).perform();
        waitActions.waitForElementIsAvailable(forwardToPage);
        console.log("Success Test for routing to forward to page");
        expect(forwardToPageSubmitButton.isEnabled()).toBe(false);
        console.log("Success checking Submit button is disabled without any Office selected");
        screenshots.takeScreenshot("Submit button is disabled without selecting any office");
        dropdownActions.selectDropdownbyNum(forwardToPageDropDown, 2);
        expect(forwardToPageSubmitButton.isEnabled()).toBe(true);
        console.log("Success checking Submit button is enabled post Forwarding Office selected");
        screenshots.takeScreenshot("Submit button is enabled post selecting any office");
        buttonActions.click(forwardToPageSubmitButton);
        console.log("Success Test for successfully forwarding the order");
        successMessage.getText().then(function (successMsg) {
            expect(successMsg).toContain("Successfully submitted");
        });
        screenshots.takeScreenshot("ForwardToOffice submitted Successfully");
        waitActions.waitForElementIsDisplayed(fastWebOrdersHeader);
        inputBoxActions.type(fastWebOrderInputField, transactionID);
        expect(firstRowFirstColumn.isPresent()).toBe(true);
        console.log("Success Test for searching before selecting Reload button");
        buttonActions.click(reload_Button);
        expect(firstRowFirstColumn.isPresent()).toBe(false);
        console.log("Successfully testing reload button");
        screenshots.takeScreenshot("Reload Button worked successfully");
    }

    //Verify status of Submit in Forward To Page
    this.verifyStatusOfSubmitButton_ForwardToOfficePage = function () {
        var transactionID = firstRowFirstColumn.getText().then(function _onSuccess(text){
            console.log(text);
        });
        browser.actions().click(firstRowFirstColumn).perform();
        browser.actions().doubleClick(firstRowFirstColumn).perform();
        waitActions.waitForElementIsAvailable(forwardToPage);
        console.log("Success Test for routing to forward to page");
        expect(forwardToPageSubmitButton.isEnabled()).toBe(false);
        console.log("Success checking Submit button is disabled without any Office selected");
        screenshots.takeScreenshot("Submit button is disabled without selecting any office");
        dropdownActions.selectDropdownbyNum(forwardToPageDropDown, 2);
        expect(forwardToPageSubmitButton.isEnabled()).toBe(true);
        console.log("Success checking Submit button is enabled post Forwarding Office selected");
        screenshots.takeScreenshot("Submit button is enabled post selecting any office");
        buttonActions.click(forwardToPageSubmitButton);
        console.log("Success Test for successfully forwarding the order");
        successMessage.getText().then(function (successMsg) {
            expect(successMsg).toContain("Successfully submitted");
        });
    }

    this.returnfastwebOrdersPageElements = function () {
        return fastWebOrdersPageElements;
    };

    this.clickOnOrderDetails = function () {
        buttonActions.click(orderDetailButton);
        return this;
    };

    this.clickAddOfficeButton = function () {
        buttonActions.click(addOfficeButton);
        return this;
    };
    this.isaddForwardToOffice = function () {
        waitActions.waitForElementIsDisplayed(addForwardToOffice);
        return this;
    };

    this.passInputvalues = function () {
        //inputBoxActions.type(userId, 423754);
        inputBoxActions.type(buid, 1031);
        inputBoxActions.type(firstName, 'Madison');
        inputBoxActions.type(lastName, 'Title and Escrow Order Desk')
    }

    this.isOrderDetailsLoaded = function () {
        waitActions.waitForElementIsDisplayed(fastwebOrderDetailTab);
        return this;
    };

    this.returnfastwebOrdersDetailsPageElements = function () {
        return fastWebOrdersDetailsPageElements;
    };

    this.filterByBorrowerName = function () {
        dropdownActions.selectDropdownbyNum(orderSrch, 1);
        inputBoxActions.type(orderSearchValue, 'Aaron 20Rodgers');
        browser.sleep(1000);
    }

    this.filterByFastWebOrderNum = function () {
        dropdownActions.selectDropdownbyNum(orderSrch, 2);
        inputBoxActions.type(orderSearchValue, 23397467);
        browser.sleep(1000);
    }

    this.filterByPropertyAddress = function () {
        dropdownActions.selectDropdownbyNum(orderSrch, 3);
        inputBoxActions.type(orderSearchValue, '7174 Packers Road');
        browser.sleep(1000);
    }

    this.validateSearchResult = function () {
        buttonActions.click(search_Find);
        browser.sleep(1000);
        waitActions.waitForElementIsDisplayed(resultrow_Name);
        console.log("Success Test for Search Filter");
        screenshots.takeScreenshot('Suceess for Search Filter');
    }

    // Verify all elements present in loaded page
    this.areElementsPresent = function () {
        fastWebOrdersPageElements.forEach(Element => {
            this.isElementPresent(Element);
            return this;
        });
    }

    this.areElementsPresentInOrderDetails = function () {
        fastWebOrdersDetailsPageElements.forEach(Element => {
            this.isElementPresent(Element);
            return this;
        });
    }
    // this.areElementsPresent = function (items) {
    //     items.forEach(Element => {
    //        this.isElementPresent(Element);
    //         return this;
    //     });
    //}
    this.clickOnOrderDetailsCancel = function () {
        buttonActions.click(cancel);
        return this;
    };

    this.clickOnOAddOfficeClose = function () {
        buttonActions.click(close);
        return this;
    };

    this.verifyFastWebOrdersPagePresent = function () {
        expect(fastWebOrdersTab.isPresent()).toBe(false);
        screenshots.takeScreenshot("Permission Denied other than SuperUsers to FASTWeb Orders");

    }

    this.clickOnOAddOfficeSubmit = function () {
        buttonActions.click(submit);
        browser.sleep(2000);
        waitActions.waitForElementIsDisplayed(fwResp);
        mouseActions.mouseMove(fwResp);
        fwResp.getText().then(function (successMsg) {
            console.log(successMsg);
            expect(successMsg).toContain("added successfully");
        });
        screenshots.takeScreenshot("Office details Added Sucessfully");
        return this;
    };

    // Verify default data loaded or not
    this.isFastwebOrdersDataLoaded = function () {
        inputBoxActions.typeAndEnter(filterByTextServiceType, "Title, Escrow");

        var indexValue;

        browser.wait(function () {

            return firstIndex.getText().then(function _onSuccess(text) {

                indexValue = text;

                //Clear Service type index
                buttonActions.click(clearServiceType);

                //Add 'Value' to 'FastWeb order#' index
                inputBoxActions.typeAndEnter(filterByTextFastWebOrderInput, indexValue);
                //Click on plus symbol
                buttonActions.click(plusSymbol);
                //Verify first row
                buttonActions.click(firstRow);
                waitActions.waitForElementIsDisplayed(firstRowVerify);

                return true;
            })
        }, 5000);

        return this;
    }



    // Go to Indivisual row details by double clicking on particular row in FastWeb page(Grid)
    this.doubleClickOnFirstRow = function () {

        var FWorderNum;
        var propertyAddress;
        var addressLine1;
        var addressLine2;
        inputBoxActions.typeAndEnter(filterByTextServiceType, "Title, Escrow");
        firstRowFourthColumn.getText().then(function (successMsg) {
            propertyAddress = successMsg;
        });
        browser.wait(function () {
            return firstIndex.getText().then(function _onSuccess(text) {
                //console.log("propertyAddress:"+propertyAddress);
                console.log(text);
                FWorderNum = text;
                // console.log(FWorderNum);
                browser
                    .actions()
                    .doubleClick(firstRowFirstColumn)
                    .perform();

                firstRowFirstColumn.isDisplayed().then(function (res) {
                    if (res) {
                        browser.actions().doubleClick(firstRowFirstColumn).perform();
                    }
                });
                waitActions.waitForElementIsAvailable(forwardToPage);


                // forwardToPagePropertyAddressLine1.getText().then(function (text) {
                //     addressLine1 = text;
                // });


                // forwardToPagePropertyAddressLine2.getText().then(function (text) {
                //     addressLine2 = text;
                // });



                // //Verify FW order number in 'ForwardTo' page
                // forwardToPageFastWebNum.getText().then(function (successMsg) {
                //     //console.log("FW order number:"+successMsg);
                //     expect(successMsg).toContain(FWorderNum);
                // });

                return true;
            })
        }, 5000);
    }

    //Select Drop down value and Click on 'Submit' button in FastWeb 'ForwardToList' screen
    this.selectDropAndSubmit = function () {
        browser.sleep(4000);
        //dropdownActions.selectDropdownbyValue(forwardToPageDropDown, "Santa Clara 1031 Exchange");
        dropdownActions.selectDropdownbyNum(forwardToPageDropDown, 2);
        buttonActions.click(forwardToPageSubmitButton);

        successMessage.getText().then(function (successMsg) {
            expect(successMsg).toContain("Successfully submitted");
        });
        screenshots.takeScreenshot("ForwardToOffice submitted Successfully");

    }

    // this.orderSearch = function FWOrderSearch(params) {
    //     dropdownActions.selectDropdownbyNum(orderSearchDropdown, params[0]);
    //     inputBoxActions.type(orderSearchValue, params[1]);
    //     buttonActions.click(orderSearch);
    // }
};