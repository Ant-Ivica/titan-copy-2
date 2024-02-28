"use strict";

angular.module('psAccessRequest').controller('psAccessRequestController', psAccessRequestController);
angular.module('psAccessRequest').service('psAccessRequestRowEditor', psAccessRequestRowEditor);

psAccessRequestController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psAccessRequestRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies', 'growl', '$route','$routeParams'];
function psAccessRequestController($scope, $rootScope, $http, $interval, $uibModal, psAccessRequestRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies, growl, $route, $routeParams) {
    var AccessReq = this;

    $scope.$on("getUser", function (evt, response) {
        $rootScope.activityright = response.ActivityRight;
        $rootScope.canmanageteq = response.CanManageTEQ;
        $rootScope.canmanagebeq = response.CanManageBEQ;
        $rootScope.canmanageaccessreq = response.CanAccessReq;

    });

    if (!$rootScope.activityright) {
        $rootScope.activityright = $cookies.get('activityright');
        $rootScope.canmanageaccessreq = $cookies.get('CanAccessReq');
    }

    if ($rootScope.activityright !== 'Admin' && $rootScope.activityright !== 'SuperAdmin' && $rootScope.activityright !== 'User') {
        UserInfo.getUser().then(function (response) {
            $rootScope.$broadcast('getUser', response);
            $rootScope.activityright = response.ActivityRight;
            $rootScope.canmanagebeq = response.CanManageBEQ;
            $rootScope.canmanageteq = response.CanManageTEQ;
            $rootScope.canmanageaccessreq = response.CanAccessReq;

        }, function (error) {

        });
    }

    var hasAccess = false;
    var hasSuperAccess = false;

    if ($rootScope.activityright === 'Admin') {
        hasAccess = true;
    }
    if ($rootScope.activityright === 'SuperAdmin') {
        hasSuperAccess = true;
        
    }
    $scope.hasAccess = hasAccess;
    $rootScope.hasAccess = hasAccess;
    $scope.hasSuperAccess = hasSuperAccess;
    $rootScope.hasSuperAccess = hasSuperAccess;

    AccessReq.EmailId = ($routeParams.emailid || "");

    AccessReq.editRowSummary = psAccessRequestRowEditor.editRowSummary;

    AccessReq.addRowSummary = psAccessRequestRowEditor.addRowSummary;

    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'
   
    AccessReq.serviceGrid = {
        enableColumnResize: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: true,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        enableRowHeaderSelection: false,
        multiSelect: false,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        enableSelectAll: true,
        paginationPageSizes: [15, 30, 45],
        paginationPageSize: 15,
        minRowsToShow: 16,
        groupingShowAggregationMenu: 0,
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterCsvFilename: 'Requests.csv',
        columnDefs: [
          { field: 'FirstName', name: 'First Name',  headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
          { field: 'LastName', name: 'Last Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'EmailId', name: 'Email', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'PhoneNo', name: 'Phone', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'CompanyName', name: 'Company Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'ProjectName', name: 'Project Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'ProjectName', name: 'Project Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'CustomerStatusName', name: 'Status', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },

        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.AccessReq.editRowSummary(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            AccessReq.serviceGrid.gridApi = gridApi;

        }
    };
    if (AccessReq.EmailId != "") {
        $http.get('CustomerRegistration/GetCustomerRegistration/' + AccessReq.EmailId+"/").success(function (response) {
            AccessReq.serviceGrid.data = response;

        });
    }
    else {
        $http.get('CustomerRegistration/GetCustomerRegistration/').success(function (response) {
            AccessReq.serviceGrid.data = response;

        });
    }
    $scope.expandAll = function () {
        $scope.gridApi.treeBase.expandAllRows();
    };

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };
    $scope.toggleRow = function (rowNum) {
        $scope.gridApi.treeBase.toggleRowTreeState($scope.gridApi.grid.renderContainers.body.visibleRowCache[rowNum]);
    };


    $scope.addNewRequest = function () {
        var newService = {
            "CustomerRegistrationId": 0,
            "FirstName": "",
            "LastName": "",
            "EmailId": "",
            "PhoneNo": "",
            "CompanyName": "",
            "ProjectName": "",
            "CustomerStatus":5001
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        AccessReq.addRowSummary($scope.AccessReq.serviceGrid, rowTmp);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.AccessReq.serviceGrid.selectItem(index, false);
        $scope.AccessReq.serviceGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        var index = $scope.AccessReq.serviceGrid.data.indexOf(row.entity);
        $scope.AccessReq.serviceGrid.data.splice(index, 1);
    };

}

psAccessRequestRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psAccessRequestRowEditor($http, $rootScope, $uibModal) {
    //var AccessReq = this;
    var service = {};
    service.addRowSummary = addRowSummary;
    service.editRowSummary = editRowSummary;

    function editRowSummary(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psAccessRequest/psAccessRequest-edit.html',
            controller: 'psAccessRequestRowEditCtrl',
            controllerAs: 'AccessReq',
            resolve: {
                grid: function () {
                    return grid;
                },
                row: function () {
                    return row;
                }
            }
        });
    }

    function addRowSummary(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psAccessRequest/psAccessRequest-add.html',
            controller: 'psAccessRequestRowEditCtrl',
            controllerAs: 'AccessReq',
            size: size,
            resolve: {
                grid: function () {
                    return grid;
                },
                row: function () {
                    return row;
                }
            }
        });
    }
    return service;
}

//angular.module('psSecurity').controller('psMappingsCustomerRowEditCtrl', ['$http', '$modalInstance', 'grid', 'row', 
angular.module('psAccessRequest').controller('psAccessRequestRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', 
function psAccessRequestRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm) {
    var AccessReq = this;
    AccessReq.entity = angular.copy(row.entity);
    AccessReq.save = save;
    AccessReq.Accept = Accept;
    AccessReq.Decline = Decline;
    AccessReq.Deactivate = Deactivate;
    AccessReq.Active = Active;
    AccessReq.ActiveAccess = true;
    AccessReq.AcceptAccess = true;
    AccessReq.DeactiveAccess = true;
    AccessReq.DeclineAccess = true;
    AccessReq.ChangeOther = ChangeOther;
    AccessReq.ignoreAccess = true;
    if ($scope.activityright === 'Admin' && (AccessReq.entity.CustomerStatus == 5001 || AccessReq.entity.CustomerStatus == 5002 || AccessReq.entity.CustomerStatus == 5004|| AccessReq.entity.CustomerStatus == 5003)) {
        AccessReq.ignoreAccess = false;
    }
    if ($scope.activityright === 'SuperAdmin') {
        AccessReq.ignoreAccess = false;

    }

    function ChangeOther() {
        AccessReq.entity.OtherRequirement = "";
    }

    if (AccessReq.entity.CustomerStatus == 5001) {
        AccessReq.AcceptAccess = false;
        AccessReq.ActiveAccess = false;
        AccessReq.DeclineAccess = false;

    }

    if (AccessReq.entity.CustomerStatus == 5002)
    {
        AccessReq.AcceptAccess = false;
        AccessReq.DeclineAccess = false;


    }

    if (AccessReq.entity.CustomerStatus == 5005) {
        AccessReq.DeactiveAccess = false;
    }

    if (AccessReq.entity.CustomerStatus == 5004) {
        AccessReq.ActiveAccess = false;
        AccessReq.AcceptAccess = false;
        AccessReq.DeclineAccess = false;

    }

    if (AccessReq.entity.CustomerStatus == 5003) {
        AccessReq.ActiveAccess = false;

    }

    var getIndexIfObjWithOwnAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }


    function save() {

        if (AccessReq.entity.PhoneNo != undefined)
        {
            if (AccessReq.entity.PhoneNo.length > 0  && AccessReq.entity.PhoneNo.length < 10) {
                growl.error('Invalid Phone number');
                return;
            }


        }

        if (AccessReq.entity.CustomerRegistrationId == 0) {

            row.entity = angular.extend(row.entity, AccessReq.entity);

            $http.post("api/CustomerRegistration/AddCustomer", row.entity).success(function (data)
            {
                   //real ID come back from response after the save in DB
                   row.entity = data;
                   grid.data.push(row.entity);

                   if (data.length == 0) {
                         growl.error('CustomerRegistration Info record is a duplicate and cannot be added');
                       return;
                   }
                   else {
                       growl.success("CustomerRegistration Info record was added successfully");
                   }
       });

        }
        else
        {
            AccessReq.entity = angular.extend(row.entity, AccessReq.entity);

            $http.post("api/CustomerRegistration/AddCustomer", row.entity).success(function (data)
            {
                //real ID come back from response after the save in DB
                row.entity = data;
                growl.success("CustomerRegistration Info record was updated successfully");

            });

        }

        $uibModalInstance.close(row.entity);
    }

    function Accept() {
        AccessReq.entity.CustomerStatus = 5005;
        AccessReq.save();
    }

    function Active() {
        AccessReq.entity.CustomerStatus = 5002;
        AccessReq.save();
    }

    function Decline() {

        AccessReq.entity.CustomerStatus = 5003;
        AccessReq.save();
    }

    function Deactivate() {

        AccessReq.entity.CustomerStatus = 5004;
        AccessReq.save();
    }


}]);
