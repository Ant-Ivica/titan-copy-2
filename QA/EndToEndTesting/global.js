(function () {
    'use strict';

    // Helper functions
    global.wait = {
        until: {
            present: function (elementFinder, optionalTimeout) {
                browser.driver.wait(function () {
                    return elementFinder.isPresent().then(function (present) {
                        return present;
                    });
                }, optionalTimeout || 60000);
            }
        }
    };

    global.commons = {};
    global.commons.inputBoxActions = require('./commons/inputBoxActions.js');
    global.commons.buttonActions = require('./commons/buttonActions.js');
    global.commons.dropDownActions = require('./commons/dropDownActions.js');
    global.commons.mouseActions = require('./commons/mouseActions.js');
    global.commons.waitActions = require('./commons/waitActions.js');
    global.commons.verifyActions = require('./commons/verifyActions.js');
    global.commons.gridFilterActions = require('./commons/gridFilterActions.js');

    global.utils = {};
    global.utils.objectLocator = require('./utils/objectLocator.js');
    //global.utils.sqlConnection = require('./utils/SQLConnection.js');
    global.pages = {};
    global.pages.searchPage = require('./pages/searchPage.js');
    global.pages.reportingTower = require('./pages/reportingTower.js');
    global.pages.searchResultPage = require('./pages/searchResultPage.js');
    global.pages.Home = require('./pages/Home.js');
    global.pages.Categories = require('./pages/Categories.js');
    global.pages.Customers = require('./pages/Customers.js');
    global.pages.Locations = require('./pages/Locations.js');
    global.pages.FASTGABMap = require('./pages/FASTGABMap.js');
    global.pages.FilePreferences = require('./pages/FilePreferences.js');
    global.pages.ContactProviders = require('./pages/ContactProviders.js');
    global.pages.Contacts = require('./pages/Contacts.js');
    global.pages.TaskMap = require('./pages/TaskMap.js');
    global.pages.Webhook= require('./pages/Webhook.js');
    global.pages.Utilites = require('./pages/Utilites.js');
    global.pages.AuditingSummary = require('./pages/AuditingSummary.js');
    global.pages.FASTToLVISDocuments = require('./pages/FASTToLVISDocuments.js');
    // global.pages.SecurityProfiles = require('./pages/SecurityProfiles');
    global.pages.Help = require('./pages/Help');
}());
