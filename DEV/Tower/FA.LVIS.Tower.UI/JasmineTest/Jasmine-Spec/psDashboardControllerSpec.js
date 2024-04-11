'use strict';
describe("Dashboard module controllers", function () {
    var $controllerConstructor;
    var scope;
    var ctrl;
    var graphTEQCtrl;
    var graphBEQCtrl;
    var $httpBackend;
    var $cookie = {};
    var timeout;
    var userInfo;
    var BEQData =
        [{ 'DateTime': '11/28/2016 3:00:04 PM', 'ExceptionName': 'No Good Match', 'NoofExceptions': 4, 'ThresholdCount': 125 },
         { 'DateTime': '7/21/2017 1:34:54 PM', 'ExceptionName': 'Mismatch Office ID', 'NoofExceptions': 0, 'ThresholdCount': 125 }];
    var TEQData =
        [{ 'DateTime': '12/28/2016 5:47:24 PM', 'ExceptionName': 'EndPoint Recieve', 'NoofExceptions': 2, 'ThresholdCount': 1 },
         { 'DateTime': '6/28/2017 4:10:34 PM', 'ExceptionName': 'Fast Recieve', 'NoofExceptions': 9, 'ThresholdCount': 5 }];
    var userInfoData =
        {
            'AccessFailedCount': 0, 'ActiveDirectoryPassword': null, 'ActivityRight': 'User', 'CanManageTEQ': false, 'CanManageBEQ': false        
        };
    var graphBEQData =
        [{ 'ActiveCount': 0, 'ArchiveCount': 0, 'Datetime': '7/21/2017', 'HoldCount': 0, 'Hour': '02:00 AM', 'NewCount': 0, 'QueueCount': 0 },
         { 'ActiveCount': 0, 'ArchiveCount': 0, 'Datetime': '7/21/2017', 'HoldCount': 0, 'Hour': '03:00 AM', 'NewCount': 0, 'QueueCount': 0 },
         { 'ActiveCount': 0, 'ArchiveCount': 0, 'Datetime': '7/21/2017', 'HoldCount': 0, 'Hour': '03:00 AM', 'NewCount': 0, 'QueueCount': 0 }];
    var graphTEQData =
        [{ 'ActiveCount': 0, 'ArchiveCount': 0, 'Datetime': '7/21/2017', 'HoldCount': 0, 'Hour': '12:00 AM', 'NewCount': 0, 'QueueCount': 0 },
         { 'ActiveCount': 0, 'ArchiveCount': 0, 'Datetime': '7/21/2017', 'HoldCount': 0, 'Hour': '01:00 AM', 'NewCount': 0, 'QueueCount': 0 },
         { 'ActiveCount': 0, 'ArchiveCount': 0, 'Datetime': '7/21/2017', 'HoldCount': 0, 'Hour': '02:00 AM', 'NewCount': 0, 'QueueCount': 0 }];
    var httpLocalBackend;

    beforeEach(module('ngCookies', 'app', 'psDashboard'));

    beforeEach(inject(function ($controller, $rootScope, $http, $cookies, $timeout, UserInfo) {
        $controllerConstructor = $controller;
        scope = $rootScope.$new();
        $httpBackend = $http;
        userInfo = UserInfo;
        timeout = $timeout;

        ctrl = $controllerConstructor('psDashboardController', { $scope: scope, $cookies: $cookie, UserInfo: userInfo });

        graphTEQCtrl = $controllerConstructor('TEQLineCtrl', { $scope: scope });
        
        graphBEQCtrl = $controllerConstructor('BEQLineCtrl', { $scope: scope });
    }));

    beforeEach(inject(function ($httpBackend) {
        httpLocalBackend = $httpBackend;
    }));

    describe("DashboardController Get Current User Information Data", function () {

        it("Should return current user information", function () {

            httpLocalBackend.expectGET('Security/GetCurrentUser/').respond(200, userInfoData);
            userInfo.getUser();
            httpLocalBackend.flush();


            //Assert
            expect(scope.activityright).toEqual(userInfoData.ActivityRight);
            expect(scope.hasBEQAccess).toEqual(userInfoData.CanManageBEQ);
            expect(scope.hasTEQAccess).toEqual(userInfoData.CanManageTEQ);
        });
    });

    
    describe("Dashboard Controller Get Business Exception Queue Summary Data", function () {

        it("should return Business Exception Queue Summary data", function () {                              

            httpLocalBackend.expectGET('Dashboard/BEQException/').respond(200, BEQData);
            ctrl.LoadBEQExceptions();
            httpLocalBackend.flush();

            expect(ctrl.BEQSummaryList.length).toBeGreaterThan(1);
            expect(ctrl.BEQSummaryList).toEqual(BEQData);
        });
    });
    

    describe("Dashboard Controller Get Technical Exception Queue Summary Data", function () {

        it("should return Technical Exception Queue Summary data", function () {           

            httpLocalBackend.expectGET('Dashboard/TEQException/').respond(200, TEQData);
            ctrl.LoadTEQExceptions();
            httpLocalBackend.flush();
            
            expect(ctrl.TEQSummaryList.length).toBeGreaterThan(1);
            expect(ctrl.TEQSummaryList).toEqual(TEQData);
        });
    });

    describe("TEQLine Controller Get Graphical Technical Exception Queue Summary Data", function () {

        it("should return Graphical Technical Exception Queue Summary data", function () {           

            httpLocalBackend.expectGET('Dashboard/GraphicalTEQException/').respond(200, graphTEQData);
            graphTEQCtrl.LoadTEQException();
            httpLocalBackend.flush();
            
            expect(graphTEQCtrl.GraphData.length).toBeGreaterThan(2);
            expect(graphTEQCtrl.GraphData).toEqual(graphTEQData);
        });
    });

    describe("BEQLine Controller Get Graphical Business Exception Queue Summary Data", function () {

        it("should return Graphical Business Exception Queue Summary data", function () {

            httpLocalBackend.expectGET('Dashboard/GraphicalBEQException/').respond(200, graphBEQData);
            graphBEQCtrl.LoadException();
            httpLocalBackend.flush();
            
            expect(graphBEQCtrl.GraphData.length).toBeGreaterThan(2);
            expect(graphBEQCtrl.GraphData).toEqual(graphBEQData);
        });
    });

});

