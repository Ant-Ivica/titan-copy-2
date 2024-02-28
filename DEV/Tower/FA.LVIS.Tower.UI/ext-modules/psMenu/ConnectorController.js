"use strict";

angular.module('psMenu').controller('ConnectorController',
    ['$scope', '$http', '$interval', 'growl', 'GetApplicationStatus',
        function ($scope, $http, $interval, growl, GetApplicationStatus) {
            var Connector = this;
            //$http.get('ApplicationController/GetApplicationStatus/')
            // .then(function (response) {
            //     Connector.BiztalkPortList = response.data;
            // }, function (data) {
            //     growl.error("Unable to retrieve application information at this time.");
            // });

            //Connector.LoadApplicationStatus = function () {
            //    var datapromise = GetApplicationStatus.getApplicationStatus();
            //    datapromise.then(function (data) {
            //        if (data.length != 0) {
            //            Connector.BiztalkPortList = data;
            //            var Count = 0;
            //            //collect array object Properties
            //            angular.forEach(Connector.BiztalkPortList, function (item) {
            //                if (!item.Active) {
            //                    Count = Count + 1;
            //                }
            //                $rootScope.ApplicationStatusCount = Count;
            //            });

            //            return;
            //        }
            //        else {
            //            growl.error("Unable to retrieve application information at this time.");
            //        }
            //    });
            //};

            //Connector.LoadApplicationStatus();

            //$interval(function () {
            //    Connector.LoadApplicationStatus();
            //}.bind(this), 30000);
        }
    ]);