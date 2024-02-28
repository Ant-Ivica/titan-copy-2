"use strict";

angular.module('app').controller('appController',
    ['$rootScope', '$scope', '$http', 'UserInfo', '$cookies', "deviceDetector", '$uibModal', '$window',
        function ($rootScope, $scope, $http, UserInfo, $cookies, deviceDetector, $uibModal, $window) {

            if (deviceDetector.browser != 'chrome' && deviceDetector.browser != 'ie') {
                if (!$cookies.get('browsercheck')) {

                    var instance = $uibModal.open({
                        template: '<div class="widget-header"><i class="fa fa-lg fa-exclamation-triangle"></i> <h3>Attention</h3></div><div class="modal-body">The browser you are using is not supported 100% by Tower. To get the best user experience, you should use "Chrome" or "Internet Explorer".</div>' +
                            '<div class="modal-footer"><a class="btn btn-default" ng-click="$close()">Close</a></div>', size: 'sm'
                    });

                    $cookies.put('browsercheck', 'true');
                }
            }

            $rootScope.errors = {};
            $scope.$on('getUser', function (evt, response) {
                $scope.currentuser = response.UserName;
                $scope.activityright = response.ActivityRight;
                $scope.canmanageteq = response.CanManageTEQ;
                $scope.canmanagebeq = response.CanManageBEQ;
                $scope.canmanageaccessreq= response.CanAccessReq;

                var activityright = $cookies.get('activityright');
                if (activityright) {
                    $cookies.remove('activityright');
                }

                $cookies.put('currentuser', response.UserName);
                $cookies.put('activityright', response.ActivityRight);

                var ManageAccessReq = $cookies.get('CanAccessReq');
                if (ManageAccessReq) {
                    $cookies.remove('CanAccessReq');
                }
                $cookies.put('CanAccessReq', response.CanAccessReq);


                UserInfo.getTenantName().then(function (response) {
                    $scope.tenantname = response;
                    $cookies.put('tenantname', $scope.tenantname);
                });

                UserInfo.addUserAuditLog().then(function (response) {

                });

                $scope.OpenPopupWindow = function () {
                    $window.open("./TowerUserGuide/help.html", "User Guide", "scrollbars=1,resizable=1,left=10,top=150");
                }
            });
        }
    ]);


angular.module('app').factory('UserInfo', ['$http', '$q', '$rootScope', function ($http, $q, $rootScope) {
    return {
        getUser: function () {
            var deferred = $q.defer();
            $http.get('Security/GetCurrentUser/')
              .then(function (response) {
                  var userinfo = [];
                  var currentuser = "";
                  userinfo = response.data;
                  $rootScope.activityright = userinfo.ActivityRight;
                  $rootScope.canmanageaccessreq = response.CanAccessReq;

                  deferred.resolve(userinfo);

              }, function (error) {
                  $rootScope.errors = { unauthorized: "You are not authorized to access this application." };
                  deferred.reject(error);
              });

            return deferred.promise;
        },
        addUserAuditLog: function () {
            var deferred = $q.defer();
            $http.post('Security/AddUserAuditLog/')
              .then(function (response) {
                  var userinfo = [];
                  var currentuser = "";
                  userinfo = response.data;

                  deferred.resolve(userinfo);

              }, function (error) {
                  deferred.reject(error);
              });

            return deferred.promise;
        },
        getTenantName: function () {
            var deferred = $q.defer();
            $http.get('Security/GetTenant/')
              .then(function (response) {
                  
                  var tenantName = "";
                  tenantName = response.data;
                  $rootScope.tenantname = response.data;
                  deferred.resolve(tenantName);

              }, function (error) {
                  deferred.reject(error);
              });

            return deferred.promise;
        },
        getCanManageTEQ: function () {
            var deferred = $q.defer();
            $http.get('Security/GetCanManageTEQ/')
              .then(function (response) {

                  var canmanageteq = "";
                  canmanageteq = response.data;

                  deferred.resolve(canmanageteq);

              }, function (error) {
                  deferred.reject(error);
              });

            return deferred.promise;
        }
    };
}]
);
angular.module('app').factory('OrderInfo', ['$http', '$q', function ($http, $q) {

    //Call Reporting Controlle
    //return {
        getReportOrderMessage= function () {
            var deferred = $q.defer();
            $http.get('ReportingController/GetMessageDetails/').then(function (response) {
                var orderinfo = [];                
                deferred.resolve(response.data);

            }, function (error) {                
                deferred.reject(error);
            });

            return deferred.promise;
        }

    //Call Excecption Controller
        getExcecptionOrderMessaage = function () {
            var deferred = $q.defer();
            $http.get('ReportingController/GetMessageDetails/').then(function (response) {
                var orderinfo = [];
                deferred.resolve(response.data);

            }, function (error) {
                deferred.reject(error);
            });

            return deferred.promise;
        }

        return {
            getReportOrderMessage: getReportOrderMessage,
            getExcecptionOrderMessaage: getExcecptionOrderMessaage
        };

    //};
}]
);
angular.module('app').factory('GetBusinessProgramService', ['$http', '$q', function ($http, $q) {
    return {
        getBusinessProgramType: function (regionId) {
            var deferred = $q.defer();
            $http.get('FilePreferences/GetBusinessFASTProgramTypeList/' + regionId)
              .then(function (response) {

                  deferred.resolve(response.data);

              }, function (error) {
                  deferred.reject(error);
              });

            return deferred.promise;
        }
    }
}]);

//Call Provider mapping name sharing with FastOffice and Product Provider module
angular.module('app').factory('ProviderMappingName', function () {
    return {};
});

//Call CUstomer mapping application id sharing with Location and Contact module
angular.module('app').factory('ServicePreferenceMapping', function () {
    return {};
});
