"use strict";

angular.module('psEndpointAccess').controller('psEndPointAccessController', psEndPointAccessController);

psEndPointAccessController.$inject = ['$route', '$routeParams', '$scope', '$rootScope', '$http', '$uibModal', '$location', '$cookies', 'growl',];
function psEndPointAccessController($route, $routeParams, $scope, $rootScope, $http, $uibModal, $location, $cookies, growl) {

    var vmUtilitiesEndPoint = this;
    $scope.showMessage = false;

    $scope.$on("getUser", function (evt, response) {
        $rootScope.activityright = response.ActivityRight;
        $rootScope.tenantname = response.tenantname;
    });

    if (!$rootScope.activityright) {
        $rootScope.activityright = $cookies.get('activityright');
    }

    if ($rootScope.activityright !== 'SuperAdmin' || $rootScope.tenantname !== 'LVIS') {
        var instance = $uibModal.open({
            template: '<div class="widget-header"><i class="fa fa-lg fa-exclamation-triangle"></i> <h3>Attention</h3></div><div class="modal-body">You are not authorized to view this page.</div>' +
                '<div class="modal-footer"><a class="btn btn-default" ng-click="$close()">Close</a></div>', size: 'sm'
        });

        instance.result.finally(function () {
            $location.path('/dashboard');
        });
    }

    $http.get('UtilitiesController/GetEndPointApplications')
        .then(function (response) {
            vmUtilitiesEndPoint.ApplicationList = response.data;
        });

    vmUtilitiesEndPoint.params = {};
    $scope.showPassword = false;
    vmUtilitiesEndPoint.toggleShowPassword = function () {
        $scope.showPassword = !$scope.showPassword;
    }
      vmUtilitiesEndPoint.save =
          function save() {
              vmUtilitiesEndPoint.Submitted = true;
            $http.post('UtilitiesController/AddCredentials/', vmUtilitiesEndPoint)
                .then(function (response) {
                    if (response.data == true) {
                        $scope.showMessage = true;
                        growl.success("Credentials for Application" + " was updated successfully ");
                        vmUtilitiesEndPoint.Submitted = false;
                    }
                },
                    function (error) {
                        $scope.showMessage = true;
                        growl.error("There was an error in Inserting the credentials.");
                        vmUtilitiesEndPoint.Submitted = false;
                    });
        }
}