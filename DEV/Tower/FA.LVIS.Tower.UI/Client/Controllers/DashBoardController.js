"use strict";

angular.module('app').controller('DashBoardController', function ($http) {

    var DashBoardCtrl = this;

    $http.get('Dashboard/TechnicalException')
        .success(function (data) {
            DashBoardCtrl.ExceptionList = data;
        });

    DashBoardCtrl.greaterThan = function (prop, val) {
        return function(item) {                          
            if (item[prop] > val) {
                return true;
            } else {                
                return false;
            }
        }
    };
});
