module.exports = function () {
    'use strict';
    var utilspage = require('../utils/objectLocator.js'); 
    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var objRepo = require('../resources/HelpObjRepo.json');
    var Helpicon = objLocator.findLocator(objRepo.helpObjRepo.Help);
   var  Basic= objLocator.findLocator(objRepo.helpObjRepo.Basic);
    this.isHelpPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(Helpicon);
        browser.actions().click(Helpicon) .perform();
        browser.sleep(6000);
        browser.getAllWindowHandles().then(function(handles){           
              browser.switchTo().window(handles[1]).then(function(){
            //    // waitActions.waitForElementIsDisplayed(Basic);
            //     // browser.driver.close();
             })
            console.log(handles);
        },10000);
      
        return this;
    };
   
};