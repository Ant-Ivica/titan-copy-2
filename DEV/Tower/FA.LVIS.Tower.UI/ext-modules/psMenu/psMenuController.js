/// <reference path="C:\Users\avidyarthi\Desktop\Tower\Tower\FA.LVIS.Tower.UI\Scripts/angular.js" />
//this module is used for loading menu bar items

"use strict";

angular.module('psMenu').controller('psMenuController',
    ['$scope', '$http', '$rootScope', '$interval', 'GetApplicationStatus',
        function ($scope, $http, $rootScope, $interval, GetApplicationStatus) {
            $scope.isVertical = false;
            $scope.openMenuScope = null;
            $scope.showMenu = true;
            $scope.allowHorizontalToggle = false;
            $scope.ApplicationStatusDisabled = false;

            this.getActiveElement = function () {
                return $scope.activeElement;
            };

            this.setActiveElement = function (el) {
                $scope.activeElement = el;
            };

            this.isVertical = function () {
                return $scope.isVertical;
            }

            this.setRoute = function (route) {
                $rootScope.$broadcast('ps-menu-item-selected-event',
                    { route: route });
            };

            this.setOpenMenuScope = function (scope) {
                $scope.openMenuScope = scope;
            };

            //$scope.toggleMenuOrientation = function () {

            //    // close any open menu
            //    if ($scope.openMenuScope)
            //        $scope.openMenuScope.closeMenu();

            //    $scope.isVertical = !$scope.isVertical;

            //    $rootScope.$broadcast('ps-menu-orientation-changed-event',
            //        { isMenuVertical: $scope.isVertical });
            //};

            angular.element(document).bind('click', function (e) {
                if ($scope.openMenuScope && !$scope.isVertical) {
                    if ($(e.target).parent().hasClass('ps-selectable-item'))
                        return;
                    $scope.$apply(function () {
                        $scope.openMenuScope.closeMenu();
                    });
                    e.preventDefault();
                    e.stopPropagation();
                }
            });

            $scope.$on('ps-menu-show', function(evt, data) {
                $scope.showMenu = data.show;
                $scope.isVertical = data.isVertical;
                $scope.allowHorizontalToggle = data.allowHorizontalToggle;
            });


            //start-To Highlight Application status Button for the Connector status.
            //var Connector = this;
            //$http.get('ApplicationController/GetApplicationStatus/')
            // .then(function (response) {
            //     Connector.BiztalkPortList = response.data;
            //     var Count = 0;
            //     //collect array object Properties
            //     angular.forEach(Connector.BiztalkPortList, function (item) {
            //         if (!item.Active) {
            //             Count = Count + 1;
            //         }
            //         $scope.Count = Count;
            //     });
            // }, function (data) {
            //     growl.error("Unable to retrieve application information at this time.");
            // });
            //End

            var Connector = this;
            Connector.LoadApplicationStatus = function () {
                var datapromise = GetApplicationStatus.getApplicationStatus();
                datapromise.then(function (data) {
                    if (data.length != 0) {
                        Connector.BiztalkPortList = data;
                        var Count = 0;
                        //collect array object Properties
                        angular.forEach(Connector.BiztalkPortList, function (item) {
                            if (!item.Active) {
                                Count = Count + 1;
                            }
                            if (Count > 0)
                            { $scope.ApplicationStatusDisabled = true; }
                            else
                            { $scope.ApplicationStatusDisabled = false; }
                        });

                        return;
                    }
                    else {
                        growl.error("Unable to retrieve application information at this time.");
                    }
                });
            };

            Connector.LoadApplicationStatus();

            $interval(function () {
                Connector.LoadApplicationStatus();
            }.bind(this), 300000);
        }
    ]);

angular.module('psMenu').factory('GetApplicationStatus', ['$http', '$q', function ($http, $q) {
    return {
        getApplicationStatus: function () {
            var deferred = $q.defer();
            $http.get('ApplicationController/GetApplicationStatus/')
              .then(function (response) {

                  deferred.resolve(response.data);

              }, function (error) {
                  deferred.reject(error);
              });

            return deferred.promise;
        }
    }
}]);