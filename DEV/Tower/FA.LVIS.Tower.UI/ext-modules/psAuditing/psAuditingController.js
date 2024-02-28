"use strict";

angular.module('psAuditing').controller('psAuditingController', psAuditingController);
angular.module('psAuditing').controller('psAuditingsRowEditCtrl', psAuditingsRowEditCtrl);
angular.module('psAuditing').service('psAuditingsRowEditor', psAuditingsRowEditor);
angular.module('psAuditing').service('psAuditingApiUri', psAuditingApiUri);

psAuditingController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal',  'uiGridGroupingConstants', '$window', '$filter', '$confirm', 'UserInfo', '$location', '$cookies', 'growl','psAuditingsRowEditor','psAuditingApiUri'];
function psAuditingController($scope, $rootScope, $http, $interval, $uibModal, uiGridGroupingConstants, $window, $filter, $confirm, UserInfo, $location, $cookies, growl, psAuditingsRowEditor, psAuditingApiUri) {
    var vmAudit = this;
    
    $scope.$on("getUser", function (evt, response) {
        $rootScope.activityright = response.ActivityRight;
        $rootScope.canmanageteq = response.CanManageTEQ;
        $rootScope.canmanagebeq = response.CanManageBEQ;
    });

    if (!$rootScope.activityright) {
        $rootScope.activityright = $cookies.get('activityright');
    }

    if ($rootScope.activityright !== 'Admin' && $rootScope.activityright !== 'SuperAdmin' && $rootScope.activityright !== 'User') {
        UserInfo.getUser().then(function (response) {
            $rootScope.$broadcast('getUser', response);
            $rootScope.activityright = response.ActivityRight;
            $rootScope.canmanagebeq = response.CanManageBEQ;
            $rootScope.canmanageteq = response.CanManageTEQ;
        }, function (error) {

        });
    }

    var hasAccess = false;

    if ($rootScope.activityright !== 'Admin' && $rootScope.activityright !== 'SuperAdmin') {
        var instance = $uibModal.open({
            template: '<div class="widget-header"><i class="fa fa-lg fa-exclamation-triangle"></i> <h3>Attention</h3></div><div class="modal-body">You are not authorized to view this page.</div>' +
                '<div class="modal-footer"><a class="btn btn-default" ng-click="$close()">Close</a></div>', size: 'sm'
        });

        instance.result.finally(function () {
            $location.path('/dashboard');
        });
    }

    if ($rootScope.activityright === 'SuperAdmin' || $rootScope.activityright === 'Admin') {
        hasAccess = true;
    }

    $scope.hasAccess = hasAccess;

    var newDate = new Date();
    var date = new Date();
    vmAudit.Fromdate = $filter('date')(new Date(), 'MM/dd/yyyy');
    vmAudit.ThroughDate = $filter('date')(new Date(), 'MM/dd/yyyy');
    vmAudit.editAudit = psAuditingsRowEditor.editAudit;
    vmAudit.DateFilterSelection = [
    {
         'title': 'Custom',
         'value': '1'
    },
    {
        'title': 'Last 90 Days',
        'value': '90'
    },
    {
        'title': 'Last 60 Days',
        'value': '60'
    },
    {
       'title': 'Last 30 Days',
       'value': '30'
    },
    {
        'title': 'Last 15 Days',
        'value': '15'
    },
    {
        'title': 'Last 7 Days',
        'value': '7'
    },
    {
       'title': '24 hrs',
       'value': '24'
    },
    {
        'title': 'Today',
        'value': '0'
    }
    ];
    vmAudit.FilterSection = '7';
    vmAudit.Disabledate = true;

    vmAudit.Busy = false;
    
    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer"></i></div>'
     vmAudit.serviceGrid = {
        enableFiltering: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        enableSorting: true,
        enableGridMenu: true,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        groupingShowAggregationMenu: 0,
        paginationPageSizes: [15, 30, 45],
        paginationPageSize: 15,
        minRowsToShow: 16,
        enableColumnResizing: true,
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterCsvFilename: 'AuditingInfo.csv',
        columnDefs: [
            { field: 'UserName', name: 'User', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'EventDateutc', name: 'Date', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'EventType', name: 'Activity Type ', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
              { field: 'Section', name: 'Section', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
          //{ field: 'TableName', name: 'Table', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'RecordId', name: 'Record', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Property', name: 'Property', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'OriginalValue', name: 'Original Value', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'NewValue', name: 'New Value', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true }
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vmAudit.editAudit(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",

        onRegisterApi: function (gridApi) {
            vmAudit.serviceGrid.gridApi = gridApi;
           
        }
    };

    vmAudit.search = search;
    search();
    vmAudit.ValidateDate = ValidateDate;

    vmAudit.ValidateError = false;
    function ValidateDate() {

        var StartDate = new Date(vmAudit.Fromdate);
        var EndDate = new Date(vmAudit.ThroughDate);

        vmAudit.ValidateError = false;
        if (EndDate < StartDate)
            vmAudit.ValidateError = true;
    }
    vmAudit.changeSelect = changeSelect;

    function changeSelect(item) {

        if (item == 1)
            vmAudit.Disabledate = false;
        else
            vmAudit.Disabledate = true;
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

    $scope.changeGrouping = function () {
        $scope.gridApi.grouping.clearGrouping();
        $scope.gridApi.grouping.groupColumn('Name');
    };

    function search() {

        if (vmAudit.gmessage != undefined)
            vmAudit.gmessage.destroy();

        if (vmAudit.FilterSection == "1") {

            if (!vmAudit.Fromdate || !vmAudit.ThroughDate) {
                vmAudit.gmessage= growl.error("Please enter a valid Start/End date");
                return;
            }

            ValidateDate();

            if (vmAudit.ValidateError) {
                vmAudit.gmessage= growl.error("End date cannot be earlier than the Start date");
                return;
            }

            var Details = {
                search: vmAudit.txtSearch,
                Fromdate: vmAudit.Fromdate.toString(),
                ThroughDate: vmAudit.ThroughDate.toString()
            }
            vmAudit.Busy = true;
            $http.post(psAuditingApiUri.GetAuditDetails, Details)
           .then(function (response) {
               vmAudit.Busy = false;
               vmAudit.serviceGrid.data = response.data;
           }, function (data) {
               vmAudit.gmessage= growl.error(data.data);
           });

        }
        else
        {
            vmAudit.Busy = true;
            $http.get('AuditController/GetAuditDetailsFilter/'+vmAudit.FilterSection)
           .then(function (response) {
               vmAudit.Busy = false;
               vmAudit.serviceGrid.data = response.data;
           }, function (data) {
               vmAudit.gmessage= growl.error(data.data);
           });

        }
    }


}


psAuditingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psAuditingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editAudit = editAudit;

    function editAudit(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psAuditing/Auditing-edit.html',
            controller: ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', psAuditingsRowEditCtrl],
            controllerAs: 'vmAudit',
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


function psAuditingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope) {

    var vmAudit = this;
    vmAudit.entity = angular.copy(row.entity);


}

function psAuditingApiUri() {
    this.GetAuditDetails = 'api/audit/GetAuditDetails/';
}



