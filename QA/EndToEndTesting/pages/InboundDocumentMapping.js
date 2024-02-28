module.exports = function() {

    'use strict';
    //var objRepo = require('../resources/SecurityProfilesObjRepo.json');
    var utilspage = require('../utils/objectLocator.js');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var constantsFile = require('../resources/constantsTower.json');
    var buttonActions = require('../commons/buttonActions.js');
    var waitActions = require('../commons/waitActions.js');
    var verifyActions = require('../commons/verifyActions.js');
    var gridFilterActions = require('../commons/gridFilterActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var screenshots = require('protractor-take-screenshots-on-demand');

    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var gridFilterActions = new gridFilterActions();
    var buttonActions = new buttonActions();
    var verifyActions = new verifyActions();
    var dropDownActions =new dropDownActions();
    var inputBoxActions =new inputBoxActions();
    var inboundDocumentMappingRepo = require('../resources/InboundDocumentMappingObjRepo.json');

    //  var objLocator = new utils.objectLocator();
    //var inputBoxActions = new commons.inputBoxActions();
    //  var buttonActions = new commons.buttonActions();
    //  var waitActions = new commons.waitActions();
    //var dropDownActions = new commons.dropDownActions();

     var mappingsheader=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.Mappings);
     var iBDM=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.iBDM);
     var iBGrid=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.iBGrid);
     var customers_Header=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.customers_Header);
     var addNewInboundDocument=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.addNewInboundDocument);
     var cancelbtn=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.cancelbtn);
     var Application=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.Application);
     var edoctype=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.edoctype);
     var lvisdoctype=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.lvisdoctype);
     var savebtn=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.savebtn);
     var confMsg=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.confMsg);
     var service=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.service);
     var appTxtBox=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.appTxtBox);
     var result=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.result);
     var rowSelected=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.rowSelected);
     var deletebtn=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.deletebtn);
     var yesbtn=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.yesbtn);
     var confMsg1=objLocator.findLocator(inboundDocumentMappingRepo.InboundDocumentMappingObjRepo.confMsg1);



    this.openSearchPage = function (path) {
    if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.isPageLoaded = function (){
        waitActions.waitForElementIsDisplayed(mappingsheader);
        buttonActions.click(mappingsheader);
        //waitActions.waitForElementIsDisplayed(customers_Header);
        waitActions.waitForElementIsDisplayed(iBDM);
        buttonActions.click(iBDM);
        waitActions.waitForElementIsDisplayed(iBGrid);
        return this;
    };

    this.addanddeleteNewInboundDocument = function () {
    waitActions.waitForElementIsDisplayed(iBGrid);
    buttonActions.click(addNewInboundDocument);
    waitActions.waitForElementIsDisplayed(cancelbtn);
    dropDownActions.selectDropdownbyValue (Application, 'Elite');
    dropDownActions.selectDropdownbyValue (edoctype, 'OTHER');
    dropDownActions.selectDropdownbyValue (service, 'Escrow');
    dropDownActions.selectDropdownbyValue (lvisdoctype, 'Clear to Close');
    buttonActions.click(savebtn);
    waitActions.waitForElementIsDisplayed(confMsg);
    inputBoxActions.type(appTxtBox, 'Elite');
    waitActions.waitForElementIsDisplayed(result);
    browser.actions().doubleClick(rowSelected).perform(); 
    rowSelected.isDisplayed().then(function (res) {
                    if (res) {
                        browser.actions().doubleClick(rowSelected).perform();
                    }
                });        
    browser.actions().doubleClick(rowSelected).perform();
    browser.actions().doubleClick(rowSelected).perform();
    waitActions.waitForElementIsDisplayed(deletebtn);
    buttonActions.click(deletebtn);
    waitActions.waitForElementIsDisplayed(yesbtn);
    buttonActions.click(yesbtn);
    waitActions.waitForElementIsDisplayed(confMsg1);
    return this;
    };
    
};