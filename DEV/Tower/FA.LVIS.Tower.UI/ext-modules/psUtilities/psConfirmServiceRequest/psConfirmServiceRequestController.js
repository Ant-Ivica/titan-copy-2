"use strict";


angular.module('psConfirmServiceRequest').controller('psConfirmServiceRequestController', psConfirmServiceRequestController);

psConfirmServiceRequestController.$inject = ['$route', '$routeParams', '$scope', '$rootScope', '$http', '$interval', '$uibModal', 'uiGridGroupingConstants', '$window', '$filter', '$confirm', 'UserInfo', '$location', '$cookies', 'growl', ];
function psConfirmServiceRequestController($route, $routeParams, $scope, $rootScope, $http, $interval, $uibModal, uiGridGroupingConstants, $window, $filter, $confirm, UserInfo, $location, $cookies, growl) {
    var vmUtilitiesConfirm = this;

    $scope.disableServiceReqId = false;
    $scope.showMessage = false;
    $scope.tenantname = $rootScope.tenantname;     
    $scope.$on("getUser", function (evt, response) {
        $rootScope.activityright = response.ActivityRight;
        $rootScope.tenantname = response.tenantname;
    });

    if (!$rootScope.activityright) {
        $rootScope.activityright = $cookies.get('activityright');
    }

    if ($rootScope.activityright !== 'SuperAdmin') {
        var instance = $uibModal.open({
            template: '<div class="widget-header"><i class="fa fa-lg fa-exclamation-triangle"></i> <h3>Attention</h3></div><div class="modal-body">You are not authorized to view this page.</div>' +
                '<div class="modal-footer"><a class="btn btn-default" ng-click="$close()">Close</a></div>', size: 'sm'
        });

        instance.result.finally(function () {
            $location.path('/dashboard');
        });
    }
    vmUtilitiesConfirm.Confirm =
        function Confirm() {         
      vmUtilitiesConfirm.ConfirmServicecheck = true;
      $http.get('UtilitiesController/ConfirmService/' + vmUtilitiesConfirm.ServiceRequestId)
           .success(function (response) {
               if (response == true) {
                   vmUtilitiesConfirm.gmsg =growl.success(" Confirm  Service " + vmUtilitiesConfirm.ServiceRequestId + " successful");
               }
               else {
                   vmUtilitiesConfirm.gmsg =growl.error("Confirm Service " + vmUtilitiesConfirm.ServiceRequestId + " not successful");
               }
               vmUtilitiesConfirm.ConfirmServicecheck = false;
           });
  }

    vmUtilitiesConfirm.ConfirmServicecheck = false;
    vmUtilitiesConfirm.RefreshSearch =
    function RefreshSearch() {
        vmUtilitiesConfirm.ServiceRequestId = '';
        vmUtilitiesConfirm.ExternalRefNum = '';
        vmUtilitiesConfirm.InternalRefNum = '';
        vmUtilitiesConfirm.InternalRefId = '';
        vmUtilitiesConfirm.CustomerRefNum = '';
        vmUtilitiesConfirm.ConfirmServicecheck = false
        vmUtilitiesConfirm.Status = [];
        $scope.disableServiceReqId = false;
        $scope.showMessage = false;
        $scope.UtilitiesConfirmForm.$setPristine();
        $scope.UtilitiesConfirmForm.$setUntouched();
        if (vmUtilitiesConfirm.gmsg != undefined)
            vmUtilitiesConfirm.gmsg.destroy();
    }
    var getIndexIfObjWithOwnAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }


    $http.get('UtilitiesController/GetStatus')
   .then(function (response) {
       vmUtilitiesConfirm.StatusList = response.data;
   });

    vmUtilitiesConfirm.Search =
        function Search() {
        vmUtilitiesConfirm.Busy = true;
        $http.get('UtilitiesController/GetServiceReqInfoWithTenant/' + vmUtilitiesConfirm.ServiceRequestId)
            .success(function (response) {
                vmUtilitiesConfirm.entity = response;
                if (vmUtilitiesConfirm.entity.ServiceRequestId > 0)
                {
                    vmUtilitiesConfirm.ExternalRefNum = vmUtilitiesConfirm.entity.ExternalRefNum;
                    vmUtilitiesConfirm.InternalRefNum = vmUtilitiesConfirm.entity.InternalRefNum;
                    vmUtilitiesConfirm.InternalRefId = vmUtilitiesConfirm.entity.InternalRefId;
                    vmUtilitiesConfirm.CustomerRefNum = vmUtilitiesConfirm.entity.CustomerRefNum;
                    var index = getIndexIfObjWithOwnAttr(vmUtilitiesConfirm.StatusList, "ID", vmUtilitiesConfirm.entity.Status.ID);
                    if (index > -1)
                        vmUtilitiesConfirm.Status = vmUtilitiesConfirm.StatusList[index];
                    $scope.disableServiceReqId = true;
                }
                
                if (vmUtilitiesConfirm.entity.ServiceRequestId === 0)
                {
                    $scope.showMessage = true;
                    vmUtilitiesConfirm.gmsg =growl.error("There is no matching record found for Service Request ID: " + vmUtilitiesConfirm.ServiceRequestId);
                }

                vmUtilitiesConfirm.Busy = false;
        });
    }
   
}















