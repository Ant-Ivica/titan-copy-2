describe('Test Execution on - Auditing', function () {
    'use strict';

    var testDataSrc = require('../resources/testData.json');

    var pages = {};
    pages.Home = require('../pages/Home.js');
    pages.AuditingSummary = require('../pages/AuditingSummary');

    var homePage = new pages.Home();
    var auditing = new pages.AuditingSummary();


    it('Auditing: Validate the Filters', function () {
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin') {
            auditing.openSearchPage(testDataSrc.search.homeUrl);
            auditing.navigateToAuditingMenu();
            auditing.filterTests();
        }
    });

    it('Auditing: Validate Gird Filters', function () {
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin') {
            auditing.openSearchPage(testDataSrc.search.homeUrl);
            auditing.navigateToAuditingMenu();
            auditing.navigateToAuditingMenu();
            auditing.gridFilterByFieldName("User");
            auditing.clearFilter("User");
            console.log("User");
            auditing.gridFilterByFieldName("Date");
            auditing.clearFilter("Date");
            console.log("Date");
            auditing.gridFilterByFieldName("ActivityType");
            auditing.clearFilter("ActivityType");
            console.log("ActivityType");
            auditing.gridFilterByFieldName("Section");
            auditing.clearFilter("Section");
            console.log("Section");
            auditing.gridFilterByFieldName("Record");
            auditing.clearFilter("Record");
            console.log("Record");
            auditing.gridFilterByFieldName("Property");
            auditing.clearFilter("Property");
            console.log("Property");
            auditing.gridFilterByFieldName("OrignalValue");
            auditing.clearFilter("OrignalValue");
            console.log("OrignalValue");
            // auditing.gridFilterByFieldName("NewValue");
            // auditing.clearFilter("NewValue");
            // console.log("NewValue");
        }
    });

    it('Auditing: Validate Gird Data', function () {
        if (testDataSrc.User.Role === 'SuperAdmin' || testDataSrc.User.Role === 'Admin') {
            homePage.openSearchPage(testDataSrc.search.homeUrl);
            homePage.isPageLoaded();
            auditing.navigateToAuditingMenu();
            auditing.validatedata();
            console.log("User");
        }
    });
})