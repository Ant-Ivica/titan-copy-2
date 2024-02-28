"use strict";

angular.module('psDashboard').controller('psDashboardController', ['$scope', '$rootScope', '$http', '$interval', '$cookies', 'UserInfo',
function ($scope, $rootScope, $http, $interval, $cookies, UserInfo) {
    var DashBoardCtrl = this;

    DashBoardCtrl.getCurrentUser = function () {
        UserInfo.getUser().then(function (response) {
            $rootScope.$broadcast('getUser', response);
            $scope.activityright = response.ActivityRight;
            $scope.canmanageteq = response.CanManageTEQ;
            $scope.canmanagebeq = response.CanManageBEQ;
            $scope.canmanageaccessreq = response.CanAccessReq;
            DashBoardCtrl.LoadBEQExceptions();
            DashBoardCtrl.LoadTEQExceptions();


        }, function (error) {

        }); 
    };

    var hasAccess = false;
    var isUser = true;
    var hasBEQAccess = false;
    var hasTEQAccess = false;

    if ($scope.canmanagebeq) {
        hasBEQAccess = true;
    }

    if ($scope.canmanageteq) {
        hasTEQAccess = true;
    }

    if ($scope.activityright === 'Admin' || $scope.activityright === 'SuperAdmin') {
        hasAccess = true;
    }

    if ($scope.activityright !== 'Admin' && $scope.activityright !== 'SuperAdmin' && $scope.activityright !== 'User') {
        isUser = false;
    }

    $scope.hasAccess = hasAccess;
    $scope.hasBEQAccess = hasBEQAccess;
    $scope.hasTEQAccess = hasTEQAccess;

    DashBoardCtrl.LoadBEQExceptions = function () {
        $http.get('Dashboard/BEQException/')
           .success(function (data) {
               DashBoardCtrl.BEQSummaryList = data;
           });
    };

    DashBoardCtrl.LoadTEQExceptions = function () {
        $http.get('Dashboard/TEQException/')
           .success(function (data) {
               DashBoardCtrl.TEQSummaryList = data;
           });
    };

    //$interval(function () {
    //    DashBoardCtrl.LoadTEQExceptions();
    //}.bind(this), 900000);

    //$interval(function () {
    //    DashBoardCtrl.LoadBEQExceptions();
    //}.bind(this), 900000);

}]);


angular.module('psDashboard').controller("TEQLineCtrl", ['$rootScope', '$scope', '$http', '$timeout', '$interval', function ($rootScope, $scope, $http, $timeout, $interval) {
    var teqLnchartCtrl = this;
    teqLnchartCtrl.TEQlineChartData = "";

    $scope.$on('getUser', function (evt, response) {
        $scope.currentuser = response.UserName;
        $scope.activityright = response.ActivityRight;

        teqLnchartCtrl.LoadTEQException();

        $scope.canmanageteq = response.CanManageTEQ;
        $scope.canmanagebeq = response.CanManageBEQ;

    });

    //$interval(function () {
    //    teqLnchartCtrl.LoadTEQException();
    //}.bind(this), 900000);

    teqLnchartCtrl.GraphData = [];
    teqLnchartCtrl.labels = [], teqLnchartCtrl.data1 = [], teqLnchartCtrl.data2 = [];
    teqLnchartCtrl.data3 = []; teqLnchartCtrl.data4 = [];
    teqLnchartCtrl.data5 = [];

    teqLnchartCtrl.LoadTEQException = function () {
        $http.get('Dashboard/GraphicalTEQException/')
        .success(function (data) {
            teqLnchartCtrl.GraphData = [];
            teqLnchartCtrl.labels1 = [],
            teqLnchartCtrl.data1 = [], teqLnchartCtrl.data2 = [];
            teqLnchartCtrl.data3 = []; teqLnchartCtrl.data4 = [];
            teqLnchartCtrl.GraphData = data;

            for (var i = 0; i < teqLnchartCtrl.GraphData.length; i++) {
                teqLnchartCtrl.labels1.push(teqLnchartCtrl.GraphData[i].Hour);
                teqLnchartCtrl.data1.push(teqLnchartCtrl.GraphData[i].NewCount);
                teqLnchartCtrl.data2.push(teqLnchartCtrl.GraphData[i].ActiveCount);
                teqLnchartCtrl.data3.push(teqLnchartCtrl.GraphData[i].HoldCount);
                teqLnchartCtrl.data4.push(teqLnchartCtrl.GraphData[i].ArchiveCount);
                teqLnchartCtrl.data5.push(teqLnchartCtrl.GraphData[i].QueueCount);
            }

            teqLnchartCtrl.labels = teqLnchartCtrl.labels1;
            teqLnchartCtrl.data = [
                teqLnchartCtrl.data1,
                teqLnchartCtrl.data2,
                teqLnchartCtrl.data3,
                teqLnchartCtrl.data4
            ];
            teqLnchartCtrl.type = 'StackedBar';
            teqLnchartCtrl.optionsMixed = {
                legend: {
                    display: true,
                    position: 'bottom',
                    reverse: true
                },
                scales: {
                    xAxes: [{
                        barThickness: '20',
                        stacked: true,
                        time: {
                            unit: 'hour'
                        }
                    }],
                    yAxes: [{
                        stacked: true,
                        ticks: {
                            beginAtZero: true,
                            minStepSize: 1
                        }
                    }]
                }
            };

            teqLnchartCtrl.datasetOverride = [{
                label: "New",
                borderWidth: 1,
                type: 'bar',
                backgroundColor: 'rgba(244,210,209, 0.9)',
                borderColor: 'rgba(244,210,209, 0.9)',
            }, {
                label: "Active",
                borderWidth: 1,
                type: 'bar',
                backgroundColor: 'rgba(194,214,235, 0.9)',
                borderColor: 'rgba(194,214,235, 0.9)'
            }, {
                label: "Hold",
                borderWidth: 1,
                type: 'bar',
                backgroundColor: 'rgba(234,241,245, 0.9)',
                borderColor: 'rgba(234,241,245, 0.9)'

            }, {
                label: "Resolved",
                borderWidth: 2,
                fill: true,
                type: 'line',
                backgroundColor: 'rgba(213,230,218, 0.9)',
                pointBackgroundColor: 'rgba(213,230,218, 0.9)',
                pointHoverBackgroundColor: 'rgba(213,230,218, 0.9)',
                borderColor: 'rgba(213,230,218, 0.9)'                        
            }];

            teqLnchartCtrl.onClick = function (points, evt) {
                console.log(points, evt);
            };
        });
    }

}]);


angular.module('psDashboard').controller("BEQLineCtrl", ['$rootScope', '$scope', '$http', '$timeout', '$interval', function ($rootScope, $scope, $http, $timeout, $interval) {

    var LnCtrl = this;
    LnCtrl.lineChartData = "";

    $scope.$on('getUser', function (evt, response) {
        $scope.currentuser = response.UserName;
        $scope.activityright = response.ActivityRight;

        LnCtrl.LoadException();

        $scope.canmanageteq = response.CanManageTEQ;
        $scope.canmanagebeq = response.CanManageBEQ;

    });

    //$interval(function () {
    //    LnCtrl.LoadException();
    //}.bind(this), 900000);

    LnCtrl.GraphData = [];
    LnCtrl.labels1 = [], LnCtrl.data1 = [], LnCtrl.data2 = [], LnCtrl.data3 = [], LnCtrl.data4 = [], LnCtrl.data5 = [];

    $rootScope.$on('BEQExceptionGraph', function () { LnCtrl.LoadException(); });

    LnCtrl.LoadException = function () {
        $http.get('Dashboard/GraphicalBEQException/')
        .success(function (data) {
            LnCtrl.GraphData = [];
            LnCtrl.labels1 = [],
            LnCtrl.data1 = [], LnCtrl.data2 = [];
            LnCtrl.data3 = []; LnCtrl.data4 = [];
            LnCtrl.data5 = [];
            LnCtrl.GraphData = data;

            for (var i = 0; i < LnCtrl.GraphData.length; i++) {
                LnCtrl.labels1.push(LnCtrl.GraphData[i].Hour);
                LnCtrl.data1.push(LnCtrl.GraphData[i].NewCount);
                LnCtrl.data2.push(LnCtrl.GraphData[i].ActiveCount);
                LnCtrl.data3.push(LnCtrl.GraphData[i].HoldCount);
                LnCtrl.data4.push(LnCtrl.GraphData[i].ArchiveCount);
                LnCtrl.data5.push(LnCtrl.GraphData[i].QueueCount);
            }

            LnCtrl.labels = LnCtrl.labels1;

            LnCtrl.data = [
             LnCtrl.data1,
               LnCtrl.data2,
                LnCtrl.data3,
                 LnCtrl.data4
            ];
            LnCtrl.type = 'StackedBar';
            LnCtrl.optionsMixed = {
                legend: {
                    display: true,
                    position: 'bottom',
                    reverse: true
                },
                scales: {
                    xAxes: [{
                        barThickness: '20',
                        stacked: true,
                        time: {
                            unit: 'hour'
                        }
                    }],
                    yAxes: [{
                        stacked: true,
                        ticks: {
                            beginAtZero: true,
                            minStepSize: 1
                        }
                    }]
                }
            };

            LnCtrl.datasetOverride = [
                     {
                         label: "New",
                         borderWidth: 1,
                         type: 'bar',
                         backgroundColor: 'rgba(244,210,209, 0.9)',
                         borderColor: 'rgba(244,210,209, 0.9)',
                     },
                     {
                         label: "Active",
                         borderWidth: 1,
                         type: 'bar',
                         backgroundColor: 'rgba(194,214,235, 0.9)',
                         borderColor: 'rgba(194,214,235, 0.9)'
                     },

                     {
                         label: "Hold",
                         borderWidth: 1,
                         type: 'bar',
                         backgroundColor: 'rgba(234,241,245, 0.9)',
                         borderColor: 'rgba(234,241,245, 0.9)'
                     },
                     {
                         label: "Resolved",
                         borderWidth: 2,
                         fill: true,
                         type: 'line',
                         backgroundColor: 'rgba(213,230,218, 0.9)',
                         pointBackgroundColor: 'rgba(213,230,218, 0.9)',
                         pointHoverBackgroundColor: 'rgba(213,230,218, 0.9)',
                         borderColor: 'rgba(213,230,218, 0.9)'
                     }

            ];

            LnCtrl.onClick = function (points, evt) {
                console.log(points, evt);
            };
        });
    }

}]);
