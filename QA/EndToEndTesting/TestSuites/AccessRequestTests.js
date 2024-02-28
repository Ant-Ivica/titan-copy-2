describe('Test Execution on Access Request', function () {

    'use strict';
    var pages = {};
    pages.Home = require('../pages/Home.js');
    pages.AccessRequest = require('../pages/AccessRequest.js');

    var testDataSrc = require('../resources/testData.json');
    var homePage = new pages.Home();
    var AccessRequest = new pages.AccessRequest();



    it('AccessRequest Add Access Request Test', function () {
        AccessRequest.navigateToAccessRequest(testDataSrc.search.homeUrl);
        AccessRequest.isPageLoaded();
        if (testDataSrc.User.Role === 'SuperAdmin') {
            AccessRequest.addNewAccessRequest();
        }
        else if (testDataSrc.User.Role === 'Admin' || testDataSrc.User.Role === 'User') {
            AccessRequest.addAccessRequestAvailable()
        }
    });

    it('AccessRequest Access Request Filter Test', function () {
        AccessRequest.navigateToAccessRequest(testDataSrc.search.homeUrl);
        AccessRequest.isPageLoaded();
        AccessRequest.gridFilterByFieldName("firstName", "Ellen");
        AccessRequest.clearFilter("firstName");
        AccessRequest.gridFilterByFieldName("lastName", "Degeneres");
        AccessRequest.clearFilter("lastName");
        // AccessRequest.gridFilterByFieldName("emailId");
        // AccessRequest.clearFilter("emailId");
        AccessRequest.gridFilterByFieldName("phone", "7234765234");
        AccessRequest.clearFilter("phone");
        AccessRequest.gridFilterByFieldName("companyName", "First");
        AccessRequest.clearFilter("companyName");
        AccessRequest.gridFilterByFieldName("projectName", "LVIS");
        AccessRequest.clearFilter("projectName");
        AccessRequest.gridFilterByFieldName("status", "New");
        AccessRequest.clearFilter("status");

    });

    it('AccessRequest Access Request Activate Test ', function () {
        AccessRequest.navigateToAccessRequest(testDataSrc.search.homeUrl);
        AccessRequest.isPageLoaded();
        if (testDataSrc.User.Role === 'SuperAdmin')
            AccessRequest.activeAccessRequest();
        else if (testDataSrc.User.Role === 'Admin') {
            AccessRequest.activeTestForAdmin();
        }
        else if (testDataSrc.User.Role === 'User') {
            AccessRequest.activeTestForUser();
        }
    });

    it('AccessRequest Access Request Approve Test', function () {
        AccessRequest.navigateToAccessRequest(testDataSrc.search.homeUrl);
        AccessRequest.isPageLoaded();
        if (testDataSrc.User.Role === 'SuperAdmin') {
            AccessRequest.approveAccessRequest();
        }
        else if (testDataSrc.User.Role === 'Admin') {
            AccessRequest.approveTestForAdmin();
        }
        else if (testDataSrc.User.Role === 'User') {
            AccessRequest.approveTestForUser();
        }
    });

    it('AccessRequest Access Request Decline Test', function () {
        AccessRequest.navigateToAccessRequest(testDataSrc.search.homeUrl);
        AccessRequest.isPageLoaded();
        if (testDataSrc.User.Role === 'SuperAdmin') {
            AccessRequest.addAccessRequestWithParam("Tim", "Jefferey");
            AccessRequest.declineAccessRequest();
        }
        else if (testDataSrc.User.Role === 'Admin') {
            AccessRequest.declineTestForAdmin();
        }
        else if (testDataSrc.User.Role === 'User') {
            AccessRequest.declineTestForUser();
        }

    });

    it('AccessRequest Access Request Deactivate Test', function () {
        AccessRequest.navigateToAccessRequest(testDataSrc.search.homeUrl);
        AccessRequest.isPageLoaded();
        if (testDataSrc.User.Role === 'SuperAdmin')
            AccessRequest.deactiveAccessRequest();
        else if (testDataSrc.User.Role === 'Admin' || testDataSrc.User.Role === 'User') {
            AccessRequest.deactiveTestForAdminAndUser();
        }
    });
});
