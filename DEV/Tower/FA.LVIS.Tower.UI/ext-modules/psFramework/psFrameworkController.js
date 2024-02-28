"use strict";

angular.module("psFramework").controller("psFrameworkController",
    ['$scope', '$window', '$timeout', '$rootScope', '$location', '$http', '$cookies',
        function ($scope, $window, $timeout, $rootScope, $location, $http, $cookies) {

            $scope.isMenuVisible = true;
            $scope.isMenuButtonVisible = false;

            if (!$rootScope.currentuser) {
                $rootScope.currentuser = $cookies.get('currentuser');
            }

            if (!$rootScope.tenantname) {
                $rootScope.tenantname = $cookies.get('tenantname');
            }

            $scope.$on('ps-menu-item-selected-event', function (evt, data) {
                $scope.routeString = data.route;
                $location.path(data.route);
                checkWidth();
                broadcastMenuState();
            });

            $http.get('Security/GetAssemblyVersion/')
            .success(function (response) {
                $scope.assemblyversion = response;
            });

            $http.get('Security/GetServerName/')
            .success(function (response) {
                $scope.servername = response;
            });
            
            $($window).on('resize.psFramework', function () {
                $scope.$apply(function () {
                    checkWidth();
                    broadcastMenuState();
                });
            });
            $scope.$on("$destroy", function () {
                $($window).off("resize.psFramework"); // remove the handler added earlier
            });

            $scope.$on('ps-submenu-item-selected-event', function (evt, data) {
                $scope.routeString = data.route;
                $location.path(data.route);
                checkWidth();
                broadcastSubMenuState();
            });


            var checkWidth = function () {
                var width = Math.max($($window).width(), $window.innerWidth);
                $scope.isMenuVisible = (width >= 768);
                $scope.isMenuButtonVisible = !$scope.isMenuVisible;
            };

            $scope.menuButtonClicked = function () {
                $scope.isMenuVisible = !$scope.isMenuVisible;
                broadcastMenuState();
                //$scope.$apply();
            };

            $scope.subMenuButtonClicked = function () {
                $scope.isMenuVisible = !$scope.isMenuVisible;
                broadcastSubMenuState();
                //$scope.$apply();
            };

            var broadcastMenuState = function () {
                $rootScope.$broadcast('ps-menu-show',
                    {
                        show: $scope.isMenuVisible,
                    });
            };

            var broadcastSubMenuState = function () {
                $rootScope.$broadcast('ps-submenu-show',
                    {
                        show: $scope.isMenuVisible,
                    });
            };

            $timeout(function () {
                checkWidth();
            }, 0);

        }
    ]);