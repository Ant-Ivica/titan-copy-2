"use strict";

angular.module('psManageExternalRefNumber').controller('psManageExternalRefNumberController', psManageExternalRefNumberController);

psManageExternalRefNumberController.$inject = ['$route', '$routeParams', '$scope', '$rootScope', '$http', '$interval', '$uibModal', 'uiGridGroupingConstants', '$window', '$filter', '$confirm', 'UserInfo', '$location', '$cookies', 'growl', ];
function psManageExternalRefNumberController($route, $routeParams, $scope, $rootScope, $http, $interval, $uibModal, uiGridGroupingConstants, $window, $filter, $confirm, UserInfo, $location, $cookies, growl) {

    var vmUtilitiesExt = this;

    $scope.disableServiceReqId = false;
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

    vmUtilitiesExt.saveandaccept =
        function SaveandAccept() {
        $http.post('UtilitiesController/UpdateandAcceptExternalRefNum/' + vmUtilitiesExt.ServiceRequestId + '/' + vmUtilitiesExt.ExternalRefNum + '/' + vmUtilitiesExt.NewExternalRefNum)
            .then(function (response) {
                if (response.data == true) {
                    $scope.showMessage = true;
                    growl.success("Update External Reference Number " + vmUtilitiesExt.NewExternalRefNum + " was updated successfully ");
                }
            },
           function (error) {
               $scope.showMessage = true;
               growl.error("There was an error in Updating External Reference Number");
           });
    }

    vmUtilitiesExt.save =
    function save() {
        $http.post('UtilitiesController/UpdateExternalRefNum/' + vmUtilitiesExt.ServiceRequestId + "/" + vmUtilitiesExt.ExternalRefNum + "/" + vmUtilitiesExt.NewExternalRefNum)
        .then(function (response) {
            if (response.data == true) {
                $scope.showMessage = true;
                growl.success("Update External Reference Number " + vmUtilitiesExt.NewExternalRefNum + " was updated successfully ");
            }
        },
       function (error) {
           $scope.showMessage = true;
           growl.error("There was an error in Updating External Reference Number");
       });
}
    $scope.disabledResetButton = true;
    vmUtilitiesExt.ResetRefreshButton = ResetRefreshButton;
    function ResetRefreshButton() {
        $scope.disabledResetButton = false;
    }

    vmUtilitiesExt.RefreshSearch =
    function RefreshSearch() {
        vmUtilitiesExt.ServiceRequestId = '';
        vmUtilitiesExt.ExternalRefNum = '';
        vmUtilitiesExt.InternalRefNum = '';
        vmUtilitiesExt.NewExternalRefNum = '';
        $scope.disableServiceReqId = false;
        $scope.showMessage = false;
        $scope.UtilitiesForm.$setPristine();
        $scope.UtilitiesForm.$setUntouched();
    }

    vmUtilitiesExt.Search =
        function Search() {
            vmUtilitiesExt.Busy = true;
            $http.get('UtilitiesController/GetServiceReqInfo/' + vmUtilitiesExt.ServiceRequestId)
                .success(function (response) {
                    vmUtilitiesExt.entity = response;
                    if (vmUtilitiesExt.entity.ServiceRequestId > 0) {
                        vmUtilitiesExt.ExternalRefNum = vmUtilitiesExt.entity.ExternalRefNum;
                        vmUtilitiesExt.InternalRefNum = vmUtilitiesExt.entity.InternalRefNum;
                        $scope.disableServiceReqId = true;
                    }

                    if (vmUtilitiesExt.entity.ServiceRequestId === 0) {
                        $scope.showMessage = true;
                        growl.error("There is no matching record found for Service Request ID: " + vmUtilitiesExt.ServiceRequestId);
                    }

                    vmUtilitiesExt.Busy = false;
                });
        }

}















