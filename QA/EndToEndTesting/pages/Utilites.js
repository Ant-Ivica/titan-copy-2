module.exports = function () {
    'use Strict';

    // Utilities
    var homeObj = require('../resources/HomeObjRepo.json');
    var objUtililty = require('../resources/UtilitesObjRepo.json');
    var taskMapObjRepo = require('../resources/TaskMapObjRepo.json');
    var objRepo = require('../resources/towerReportingPageRepository.json');
    var tesObjRepo = require('../resources/testData.json');
    var constantsFile = require('../resources/constantsTower.json');
    var screenshots = require('protractor-take-screenshots-on-demand');


    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var gridActions = require('../commons/gridFilterActions.js');
    var checkBoxActions = require('../commons/checkBoxActions.js');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var dropDownActions = new dropDownActions();
    var inputBoxActions = new inputBoxActions();
    var gridActions = new gridActions();
    var checkBoxActions = new checkBoxActions();

    var reqNo;
    var confirmreqNo;
    var updateReqNo;
    var random;
    var extNum;
    var updatedStatus;
    var tenant2;
    var chktext;
    var updatedUniqueID;
    var extRefNumber;


    //Get the Xpaths 
    var menu_utilites = objLocator.findLocator(homeObj.homeObjRepo.utilitiesMenu);
    var utilitiesMenu_hidden = objLocator.findLocator(homeObj.homeObjRepo.utilitiesMenu_hidden);

    var reportingtab = objLocator.findLocator(objRepo.searchPage.Reportingtab);
    //xpath_Utilites - Manage Service Request screen
    var header = objLocator.findLocator(objUtililty.utilitesObj.xpath_header);
    var serviceReqno = objLocator.findLocator(objUtililty.utilitesObj.xpath_servicerequestID);
    var searchServiceReq = objLocator.findLocator(objUtililty.utilitesObj.xpath_Search);
    var externalrefNum = objLocator.findLocator(objUtililty.utilitesObj.xpath_externalRefNum);
    var servicereq_status = objLocator.findLocator(objUtililty.utilitesObj.xpath_serviceStatus);
    var update = objLocator.findLocator(objUtililty.utilitesObj.xpath_updte);
    var successmessage = objLocator.findLocator(objUtililty.utilitesObj.xpath_successmessage);
    var reset = objLocator.findLocator(objUtililty.utilitesObj.xpath_reset);
    var checkboxFASTUpdate = objLocator.findLocator(objUtililty.utilitesObj.xpath_checkBoxUniqueID);
    var checkboxExtRefNum = objLocator.findLocator(objUtililty.utilitesObj.xpath_extRefNum);

    //Grid Filter For Home Reporting Tower
    var ServiceReq = objLocator.findLocator(objRepo.searchPage.txtServiceReq);
    var Status = objLocator.findLocator(objRepo.searchPage.txtStatus);
    var externalApplication = objLocator.findLocator(objRepo.searchPage.externalApplication);
    var Servicerequest_popup = objLocator.findLocator(objRepo.searchPage.Servicerequest);
    var txtServiceType = objLocator.findLocator(objRepo.searchPage.txtServiceType);
    //Grid Result values
    var result_serviceReq = objLocator.findLocator(objRepo.searchPage.resultrow_ServiceReqId);
    var result_status = objLocator.findLocator(objRepo.searchPage.resultrow_Status);
    var result_ExternalRef = objLocator.findLocator(objRepo.searchPage.resultrow_ExternalRef);
    var resultrow_ExternalApp = objLocator.findLocator(objRepo.searchPage.resultrow_ExternalApp);
    var resultrow_ServiceType = objLocator.findLocator(objRepo.searchPage.resultrow_ServiceType);

    var FirstRow = objLocator.findLocator(objRepo.searchPage.FirstEntry);
    var closeButton = objLocator.findLocator(objRepo.searchPage.closeButton);
    let appNameList = "//option[contains(@value,'string')]";


    //Confirm Service Request
    var confirmServiceRequest = objLocator.findLocator(objUtililty.utilitesObj.xpath_confirmSRMenu);
    var confirmServiceReqHeader = objLocator.findLocator(objUtililty.utilitesObj.xpath_confirmServiceReqHeader);
    var conServiceRequestId = objLocator.findLocator(objUtililty.utilitesObj.xpath_conServiceRequestId);
    var confirmSearch = objLocator.findLocator(objUtililty.utilitesObj.xpath_conSerarch);
    var confirmExternalRef = objLocator.findLocator(objUtililty.utilitesObj.xpath_confExternalRefNum);
    var confirmSRClick = objLocator.findLocator(objUtililty.utilitesObj.xpath_confirmSRClick);
    var confirmSuccessMessage = objLocator.findLocator(objUtililty.utilitesObj.xpath_confirmSRSuccessMessage);
    var confirmReset = objLocator.findLocator(objUtililty.utilitesObj.xpath_confirmReset);


    //EndPoint Access
    var endpointAccess = objLocator.findLocator(objUtililty.utilitesObj.xpath_EndpointaccessMenu);
    var endpointAccessReqHeader = objLocator.findLocator(objUtililty.utilitesObj.xpath_EndPointAccessHeader);
    var applications = objLocator.findLocator(objUtililty.utilitesObj.xpath_applicationList);
    var userName = objLocator.findLocator(objUtililty.utilitesObj.xpath_userName);
    var password = objLocator.findLocator(objUtililty.utilitesObj.xpath_password);
    var submit = objLocator.findLocator(objUtililty.utilitesObj.xpath_CredentialSaveButton);
    var endPointSuccessMessage = objLocator.findLocator(objUtililty.utilitesObj.xpath_EndpointGrowlMessage);

    var updateInFast = objLocator.findLocator(objUtililty.utilitesObj.xpath_updateInFast);


    this.openSearchPage = function (path) {
        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.navigateToUtilitesManageSR = function (path) {
        waitActions.waitForElementIsDisplayed(menu_utilites);
        buttonActions.click(menu_utilites);
        waitActions.waitForElementIsDisplayed(header);
    };

    this.navigateToReportingOrderSummary = function () {
        waitActions.waitForElementIsDisplayed(reportingtab);
        buttonActions.click(reportingtab);
        waitActions.waitForElementIsDisplayed(result_serviceReq);
    };

    this.navigateToUtilitesConfirmSR = function () {
        waitActions.waitForElementIsDisplayed(menu_utilites);
        buttonActions.click(menu_utilites);
        screenshots.takeScreenshot('Manage Service Request -confirmServiceRequest');
        waitActions.waitForElementIsDisplayed(header);
        buttonActions.click(confirmServiceRequest);
        waitActions.waitForElementIsDisplayed(confirmServiceReqHeader);
        //  screenshots.takeScreenshot('Manage Service Request -confirmServiceReqHeade');
    };

    this.getOrderDetails = function (order_status, utiliy_option) {
        inputBoxActions.type(Status, '');
        //console.log(Status);
        gridActions.filter(Status, order_status, result_status);
        screenshots.takeScreenshot('Success Filter for_' + order_status);
        if (utiliy_option === "Update") {
            reqNo = result_serviceReq.getText().then(function _onSuccess(text) {
                return text;
            });
            extNum = result_ExternalRef.getText().then(function _onSuccess(text) {
                return text + "-4";
            });
            browser.sleep(1000);
            console.log(reqNo);
            this.navigateToUtilitesManageSR();
            this.validateUpdateServiceRequest(reqNo, extNum);
        }
        else if (utiliy_option === "Confirm") {
            confirmreqNo = result_serviceReq.getText().then(function _onSuccess(text) {
                return text;
            });
            browser.sleep(1000);
            this.navigateToUtilitesConfirmSR();
            this.validateConfirmServiceRequest(confirmreqNo);
        }
        else if (utiliy_option === "NotToUpdateInLVIS") {
            gridActions.filter(externalApplication, "RealEC", resultrow_ExternalApp);
            gridActions.filter(txtServiceType, "Title", resultrow_ServiceType);
            reqNo = result_serviceReq.getText().then(function _onSuccess(text) {
                return text;
            });
            extNum = result_ExternalRef.getText().then(function _onSuccess(text) {
                return text;
            });
            browser.sleep(1000);
            console.log(reqNo);
            this.navigateToUtilitesManageSR();
            this.validateUpdateUniqueIDandExternalRefNum_NotInLVIS(reqNo, extNum);
        }
        else if (utiliy_option === "UniqueIDInFAST") {
            gridActions.filter(externalApplication, "RealEC", resultrow_ExternalApp);
            gridActions.filter(txtServiceType, "Title", resultrow_ServiceType);
            reqNo = result_serviceReq.getText().then(function _onSuccess(text) {
                return text;
            });
            extNum = result_ExternalRef.getText().then(function _onSuccess(text) {
                return text;
            });
            updatedUniqueID = result_ExternalRef.getText().then(function _onSuccess(text) {
                return text + "-4";
            });
            browser.sleep(1000);
            console.log(reqNo);
            this.navigateToUtilitesManageSR();
            this.validateUniqueIdInFAST(reqNo, extNum, updatedUniqueID);
        }
        else if (utiliy_option === "ExternalRefNumInFAST") {
            gridActions.filter(externalApplication, "RealEC", resultrow_ExternalApp);
            gridActions.filter(txtServiceType, "Title", resultrow_ServiceType);
            reqNo = result_serviceReq.getText().then(function _onSuccess(text) {
                return text;
            });
            extNum = result_ExternalRef.getText().then(function _onSuccess(text) {
                return text;
            });
            browser.sleep(1000);
            console.log(reqNo);
            this.navigateToUtilitesManageSR();
            this.validateExternalRefNumInFAST(reqNo, extNum);
        }
    }

    this.validateUpdateServiceRequest = function (reqNo, extNum) {
        var i = 0;
        console.log(reqNo);
        inputBoxActions.type(serviceReqno, reqNo);
        buttonActions.click(searchServiceReq);
        waitActions.waitForElementIsDisplayed(externalrefNum);
        browser.sleep(500);
        inputBoxActions.type(externalrefNum, "");
        inputBoxActions.type(externalrefNum, extNum);

        //dropDownActions.selectDropdownbyValue(servicereq_status, "Open");
        dropDownActions.selectDropdownbyNum(servicereq_status, 1);
        //var chk = update.hasAttribute('disabled');
        browser.sleep(2000);
        update.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for updation");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(update);
            waitActions.waitForElementIsDisplayed(successmessage);
            successmessage.getText().then(function (successMsg) {
                console.log(successMsg);
                expect(successMsg).toContain("updated successfully");
            });
            screenshots.takeScreenshot('Manage Service Request - Update Service Request');
            console.log("Updated Successfully");
        }
        this.navigateToReportingOrderSummary();
        if (i === 0) {
            console.log(i);
            gridActions.filter(ServiceReq, reqNo, result_serviceReq);
            updatedStatus = result_status.getText().then(function _onSuccess(text) {
                expect(text).toContain("Open");
                return text;
            });
            updatedUniqueID = result_ExternalRef.getText().then(function _onSuccess(text) {
                expect(text).toContain(extNum);
                return text;
            });

        }
        screenshots.takeScreenshot('Validated the Manage Service Request - Update Service Request');
    }


    this.validateUpdateUniqueIDandExternalRefNum_NotInLVIS = function (reqNo, extNum) {
        var i = 0;
        console.log(reqNo);
        inputBoxActions.type(serviceReqno, reqNo);
        buttonActions.click(searchServiceReq);
        browser.sleep(500);
        waitActions.waitForElementIsDisplayed(externalrefNum);
        checkBoxActions.click(checkboxFASTUpdate);
        checkBoxActions.click(checkboxExtRefNum);
        var extRefNumber = (getRandomNum(1, 100)) + "-4";
        console.log(extRefNumber);
        inputBoxActions.type(externalrefNum, "");
        inputBoxActions.type(externalrefNum, extRefNumber);
        //        dropDownActions.selectDropdownbyValue(servicereq_status, "Open");
        //dropDownActions.selectDropdownbyNum(servicereq_status, 1);
        //var chk = update.hasAttribute('disabled');
        update.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for updation");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(update);
            waitActions.waitForElementIsDisplayed(successmessage);
            successmessage.getText().then(function (successMsg) {
                console.log(successMsg);
                expect(successMsg).toContain("updated successfully");
            });
            screenshots.takeScreenshot('Manage Service Request - Update Service Request In FAST is success');
            console.log("Updated Successfully");
        }
        this.navigateToReportingOrderSummary();
        if (i === 0) {
            console.log(i);
            gridActions.filter(ServiceReq, reqNo, result_serviceReq);
            // updatedStatus = result_status.getText().then(function _onSuccess(text) {
            //     expect(text).toContain("New");
            //     return text;
            // });
            updatedUniqueID = result_ExternalRef.getText().then(function _onSuccess(text) {
                expect(text).toContain(extNum);
                return text;
            });
        }
        screenshots.takeScreenshot('Validated the Manage Service Request - Update Service Request In FAST');
    }


    this.validateResetServiceRequest = function () {
        inputBoxActions.type(serviceReqno, reqNo);
        buttonActions.click(searchServiceReq);
        waitActions.waitForElementIsDisplayed(externalrefNum);
        browser.sleep(500);
        buttonActions.click(reset);
        waitActions.waitForElementIsDisplayed(externalrefNum);
        screenshots.takeScreenshot('Manage Service Request - Reset Service Request');
    }

    this.validateConfirmServiceRequest = function (confirmreqNo) {
        inputBoxActions.type(conServiceRequestId, confirmreqNo);
        buttonActions.click(confirmSearch);
        waitActions.waitForElementIsDisplayed(confirmExternalRef);
        browser.sleep(500);
        buttonActions.click(confirmSRClick);
        waitActions.waitForElementIsDisplayed(confirmSuccessMessage);
        this.navigateToReportingOrderSummary();
        gridActions.filter(ServiceReq, confirmreqNo, result_serviceReq);
        updatedStatus = result_status.getText().then(function _onSuccess(text) {
            expect(text).toContain("In TEQ");
            return text;
        });
        // buttonActions.click(closeButton);
        screenshots.takeScreenshot('Validated Manage SR - Confirm Service Request');
        this.validateMessageLogs();
    }

    this.validateMessageLogs = function () {
        browser.actions().doubleClick(FirstRow).perform();
        waitActions.waitForElementIsDisplayed(Servicerequest_popup);
        //browser.sleep(1200);
        //waitActions.waitForElementIsDisplayed(Servicerequest);
        var list = objLocator.findLocator(objRepo.searchPage.DivMessage);
        var detailes = objLocator.findLocator(objRepo.searchPage.DivMessageFastfile);
        list.getText().then(function (text) {
            chktext = text.toString().search(": Service Created");
            console.log(chktext);
        }
        );
        // chktext.getText().then(function _onSuccess(text) {
        //     expect(text).toContain("Service Created");
        //     return text;
        // });
        screenshots.takeScreenshot('Confirm Service Created: Service Created');
        buttonActions.click(closeButton);
        return this;
    }


    this.validateResetServiceRequestForCSR = function () {
        inputBoxActions.type(conServiceRequestId, confirmreqNo);
        buttonActions.click(confirmSearch);
        waitActions.waitForElementIsDisplayed(confirmExternalRef);
        browser.sleep(500);
        buttonActions.click(confirmReset);
        waitActions.waitForElementIsDisplayed(confirmExternalRef);
        screenshots.takeScreenshot('Confirm Service Request - Reset Service Request');
    }

    this.navigateToUtilitesEndPointAccess = function () {
        waitActions.waitForElementIsDisplayed(menu_utilites);
        buttonActions.click(menu_utilites);
        //   screenshots.takeScreenshot('EndPont Access');
        buttonActions.click(endpointAccess);
        waitActions.waitForElementIsDisplayed(endpointAccessReqHeader);
        //console.log(endpointAccess);
        waitActions.waitForElementIsDisplayed(applications);
        //  screenshots.takeScreenshot('Manage Service Request -confirmServiceReqHeade');
    };

    this.validateAvailableApplications = function () {
        buttonActions.click(applications);
        var i = 0, j = 0;
        element.all(by.xpath(appNameList)).each(function (element) {
            element.getText().then(function (text) {
                console.log(text);
                expect(text).toContain(constantsFile.filePref.appList[i]);
                console.log(i)
                i++;
            });
        });
        return this;
    };

    this.validateEndpointRequest = function () {
        dropDownActions.selectDropdownbyValue(applications, "CALCULATOR");
        inputBoxActions.type(userName, "TestAppUser2");
        inputBoxActions.type(password, "TestPassword2");
        buttonActions.click(submit);
        endPointSuccessMessage.getText().then(function (text) {
            expect(text).toContain("Credentials for Application was updated successfully");
        });
    };

    this.getTestData = function () {
        console.log(tesObjRepo.User.Tenant);
    }

    this.isUtilityPresent = function () {
        utilitiesMenu_hidden.getAttribute("class").then(function (value) {
            expect(value).toContain('hide');
        })
    }

    this.validateUniqueIdInFAST = function (reqNo, extNum, updatedUniqueID) {
        var i = 0;
        //No Need to validate in the Reporting tower since it wont reflect in the LVIS
        inputBoxActions.type(serviceReqno, reqNo);
        buttonActions.click(searchServiceReq);
        waitActions.waitForElementIsDisplayed(externalrefNum);
        browser.sleep(2000);
        checkBoxActions.click(checkboxFASTUpdate);
        inputBoxActions.type(externalrefNum, updatedUniqueID);
        browser.sleep(2000);
        update.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for updation");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(update);
            waitActions.waitForElementIsDisplayed(successmessage);
            screenshots.takeScreenshot('Manage Service Request - Update Service Request Unique ID In FAST');
            console.log("Updated Successfully");
        }
        this.navigateToReportingOrderSummary();
        if (i === 0) {
            console.log(i);
            gridActions.filter(ServiceReq, reqNo, result_serviceReq);
            result_ExternalRef.getText().then(function _onSuccess(text) {
                expect(text).toContain(extNum);
                return text;
            });
        }
    }

    this.validateExternalRefNumInFAST = function (reqNo, extNum) {
        var i = 0;
        //No Need to validate in the Reporting tower since it wont reflect in the LVIS
        inputBoxActions.type(serviceReqno, reqNo);
        buttonActions.click(searchServiceReq);
        waitActions.waitForElementIsDisplayed(externalrefNum);
        browser.sleep(2000);
        checkBoxActions.click(checkboxExtRefNum);
        var extRefNumber = (getRandomNum(1, 100)) + "-4";
        inputBoxActions.type(externalrefNum, extRefNumber);
        browser.sleep(2000);
        update.getAttribute('disbaled').then(function () {
            i++;
            console.log("Please select the correct order for updation");
        });
        console.log(i);
        if (i === 0) {
            buttonActions.click(update);
            waitActions.waitForElementIsDisplayed(successmessage);
            screenshots.takeScreenshot('Manage Service Request - Update Service Request External Ref Num In FAST');
            console.log("Updated Successfully");
        }
        this.navigateToReportingOrderSummary();
        if (i === 0) {
            console.log(i);
            gridActions.filter(ServiceReq, reqNo, result_serviceReq);
            result_ExternalRef.getText().then(function _onSuccess(text) {
                expect(text).toContain(extNum);
                return text;
            });
        }
    }
};

getRandomNum = function (min, max) {
    return parseInt(Math.random() * (max - min) + min);
};