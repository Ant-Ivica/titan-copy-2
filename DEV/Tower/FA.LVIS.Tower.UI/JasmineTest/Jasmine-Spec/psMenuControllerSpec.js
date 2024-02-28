'use strict';
describe("Menu Module Controller", function () {
    var $controllerConstructor;
    var scope;
    var ctrl;
    var $httpBackend;
    var applicationStatusInfo;
    var httpLocalBackend;
    var applicationStatusInfoData = [{ Incoming: false, QueueName: "RealEC", Active: false },
                                     { Incoming: false, QueueName: "FAST", Active: false },
                                     { Incoming: false, QueueName: "CALCULATOR", Active: false }];

    beforeEach(module('ngCookies', 'app', 'psMenu'));

    beforeEach(inject(function ($controller, $rootScope, $http, GetApplicationStatus) {
        $controllerConstructor = $controller;
        scope = $rootScope.$new();
        $httpBackend = $http;
        applicationStatusInfo = GetApplicationStatus;

        ctrl = $controllerConstructor('psMenuController', { $scope: scope, GetApplicationStatus: applicationStatusInfo });
    }));

    beforeEach(inject(function ($httpBackend) {
        httpLocalBackend = $httpBackend;
    }));

    describe("Menu Get Application Status Information", function () {

        it("Should return application status information", function () {
            ////ctrl.LoadApplicationStatus();
            httpLocalBackend.whenGET('ApplicationController/GetApplicationStatus/').respond(200, applicationStatusInfo);
            applicationStatusInfo.getApplicationStatus();
            httpLocalBackend.flush();


            //Assert
            expect(scope.ApplicationStatusDisabled).toEqual(true);
        });
    });

})