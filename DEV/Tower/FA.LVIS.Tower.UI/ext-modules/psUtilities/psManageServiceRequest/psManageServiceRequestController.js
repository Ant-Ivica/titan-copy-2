"use strict";

angular.module('psManageServiceRequest').controller('psManageServiceRequestController', psManageServiceRequestController);

psManageServiceRequestController.$inject = ['$route', '$routeParams', '$scope', '$rootScope', '$http', '$interval', '$uibModal', 'uiGridGroupingConstants', '$window', '$filter', '$confirm', 'UserInfo', '$location', '$cookies', 'growl',];
function psManageServiceRequestController($route, $routeParams, $scope, $rootScope, $http, $interval, $uibModal, uiGridGroupingConstants, $window, $filter, $confirm, UserInfo, $location, $cookies, growl) {
    var vmUtilities = this;

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

    vmUtilities.save =
        function save() {
            $http.post('UtilitiesController/UpdateServiceRequestInfo/' + vmUtilities.ServiceRequestId + "/" + vmUtilities.ExternalRefNum + "/" + vmUtilities.InternalRefNum + "/" + vmUtilities.InternalRefId + "/" + vmUtilities.CustomerRefNum + "/" + vmUtilities.Status.ID + "/" + vmUtilities.chkUniqueID + "/" + vmUtilities.chkExternalRefNum)
                .then(function (response) {
                    if (response.data == true) {
                        $scope.showMessage = true;
                        growl.success("Service Request information with id " + vmUtilities.ServiceRequestId + " was updated successfully ");
                    }
                },
                    function (error) {
                        $scope.showMessage = true;
                        growl.error("There was an error in Updating Service Request information.");
                    });
        }

    $http.get('UtilitiesController/GetStatus')
        .then(function (response) {
            vmUtilities.StatusList = response.data;
        });


    vmUtilities.RefreshSearch =
        function RefreshSearch() {
            vmUtilities.ServiceRequestId = '';
            vmUtilities.ExternalRefNum = '';
            vmUtilities.InternalRefNum = '';
            vmUtilities.InternalRefId = '';
            vmUtilities.CustomerRefNum = '';
            vmUtilities.chkUniqueID = false;
            vmUtilities.chkExternalRefNum = false;
            vmUtilities.Status = [];
            $scope.disableServiceReqId = false;
            $scope.showMessage = false;
            $scope.UtilitiesForm.$setPristine();
            $scope.UtilitiesForm.$setUntouched();
        }
    var getIndexIfObjWithOwnAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }
    vmUtilities.Search =
        function Search() {
            vmUtilities.Busy = true;
            $http.get('UtilitiesController/GetServiceReqInfo/' + vmUtilities.ServiceRequestId)
                .success(function (response) {
                    vmUtilities.entity = response;
                    if (vmUtilities.entity.ServiceRequestId > 0) {
                        vmUtilities.ExternalRefNum = vmUtilities.entity.ExternalRefNum;
                        vmUtilities.InternalRefNum = vmUtilities.entity.InternalRefNum;
                        vmUtilities.InternalRefId = vmUtilities.entity.InternalRefId;
                        vmUtilities.CustomerRefNum = vmUtilities.entity.CustomerRefNum;
                        vmUtilities.chkExternalRefNum = false;
                        vmUtilities.chkUniqueID = false;
                        var index = getIndexIfObjWithOwnAttr(vmUtilities.StatusList, "ID", vmUtilities.entity.Status.ID);
                        if (index > -1)
                            vmUtilities.Status = vmUtilities.StatusList[index];
                        $scope.disableServiceReqId = true;
                    }

                    if (vmUtilities.entity.ServiceRequestId === 0) {
                        $scope.showMessage = true;
                        growl.error("There is no matching record found for Service Request ID: " + vmUtilities.ServiceRequestId);
                    }

                    vmUtilities.Busy = false;
                });
        }

}















