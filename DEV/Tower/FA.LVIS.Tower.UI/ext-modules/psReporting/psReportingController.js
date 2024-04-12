"use strict";

angular.module('psReporting').controller('psReportingController', psReportingController);
//angular.module('psReporting').controller('psReportingRowEditCtrl', psReportingRowEditCtrl);
angular.module('psReporting').service('psReportingRowEditor', psReportingRowEditor);

psReportingController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'uiGridGroupingConstants', '$templateCache', '$window', '$filter', '$confirm', 'UserInfo', '$location', '$cookies', 'growl', 'psReportingRowEditor','modalProvider'];
function psReportingController($scope, $rootScope, $http, $interval, $uibModal, uiGridGroupingConstants, $templateCache, $window, $filter, $confirm, UserInfo, $location, $cookies, growl, psReportingRowEditor, modalProvider) {
    var vmReport = this;
    $scope.orderToInvalidate = [];
    $scope.inValidBtnEnable = true; //Invalidate Button to be disabled.
    //$scope.pageLoader = false;
    $scope.loggedTenant = $rootScope.tenantname;
    $scope.togglingTenant = $rootScope.tenantname;

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
            //$rootScope.activityright = response.ActivityRight;
            //$rootScope.canmanagebeq = response.CanManageBEQ;
            //$rootScope.canmanageteq = response.CanManageTEQ;
            search();

        }, function (error) {

        });
    }

    var hasAccess = false;
    var hasSuperAccess = false;

    if ($rootScope.activityright === 'Admin' || $rootScope.activityright === 'SuperAdmin') {
        hasAccess = true;
    }
    if ($rootScope.activityright === 'SuperAdmin') {
        hasSuperAccess = true;
    }

    $scope.hasAccess = hasAccess;
    $rootScope.hasAccess = hasAccess;
    $scope.hasSuperAccess = hasSuperAccess;
    $rootScope.hasSuperAccess = hasSuperAccess;

    var newDate = new Date();
    var date = new Date();
    vmReport.Fromdate = $filter('date')(new Date(), 'MM/dd/yyyy');
    vmReport.ThroughDate = $filter('date')(new Date(), 'MM/dd/yyyy');

    vmReport.Busy = false;
    vmReport.editReportRow = psReportingRowEditor.editReportRow;      
    vmReport.DateFilterSelection = [
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
    
    //Search FUnctionality by Reference No
    vmReport.ReferencenoFilterSelection = [{
        'title': '---Select---',
        'value': '0'
    },
    {
    'title': 'External Reference Number',
    'value': '1'
    },
    {
    'title': 'Internal Reference Number',
    'value': '2'
    },
    {
    'title': 'Customer Reference Number',
    'value': '3'
    },
    {
    'title': 'Internal Reference Id',
    'value': '4'
    }
    ];
    //Search FUnctionality by Reference No

    vmReport.FilterSection = '7';
    vmReport.Disabledate = true;
    
   //// $templateCache.put('ui-grid/selectionRowHeaderButtons',
   ////"<div class=\"ui-grid-selection-row-header-buttons \" ng-class=\"{'ui-grid-row-selected': row.isSelected}\" ><input style=\"margin: 0; vertical-align: middle\" type=\"checkbox\" ng-model=\"row.isSelected\" ng-click=\"row.isSelected=!row.isSelected;selectButtonClick(row, $event)\">&nbsp;</div>"
   //// );


    ////$templateCache.put('ui-grid/selectionSelectAllButtons',
    ////  "<div class=\"ui-grid-selection-row-header-buttons \" ng-class=\"{'ui-grid-all-selected': grid.selection.selectAll}\" ng-if=\"grid.options.enableSelectAll\"><input style=\"margin: 0; vertical-align: middle\" type=\"checkbox\" ng-model=\"grid.selection.selectAll\" ng-click=\"grid.selection.selectAll=!grid.selection.selectAll;headerButtonClick($event)\"></div>"
    ////);
    
    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer"></i></div>'
    // var TestSettings = '<div style="word-wrap: normal" title="{{row.getProperty(col.field)}}">{{row.getProperty(col.field)}}</div>';
    vmReport.serviceGrid = {
        enableColumnResize: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,        
        multiSelect: true,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        enableSelectAll: false,
        paginationPageSizes: [15, 30, 45],
        paginationPageSize: 15,
        minRowsToShow: 16,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        groupingShowAggregationMenu: 0,
        expandableRowTemplate: 'ext-modules/psReporting/reporting-row-detail.html',
        columnDefs: [            
            { field: 'ServiceRequestId', name: 'Service Request ID', displayName: 'Service Request ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'CustomerName', name: 'Customer Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'ApplicationId', name: 'External Application', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'ExternalRefNum', name: 'External Reference Number', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'createddate', name: 'Order Date', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'InternalRefNum', name: 'Internal Reference Number', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'service', name: 'Service Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'OrderStatus', name: 'Status', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'LVISActionType', name: 'Request Action Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vmReport.editReportRow(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",

        onRegisterApi: function (gridApi) {
            vmReport.serviceGrid.gridApi = gridApi;
            gridApi.cellNav.on.navigate($scope, function (newRowCol, oldRowCol) {                
                console.log('navigation event');
            });
            gridApi.selection.on.rowSelectionChanged($scope, function (row) {

                console.log("Row " + row.entity + " selected: " + row.isSelected);
                
                if (row.isSelected == true && row.entity.InternalRefNum == null) {
                    $scope.orderToInvalidate.push(row.entity);
                    $scope.inValidBtnEnable = false;            //Invalidate Button to be enabled.
                } else if (row.isSelected == false) {
                    $scope.orderToInvalidate = $scope.orderToInvalidate.filter(function (el) { return el.ServiceRequestId != row.entity.ServiceRequestId; });
                    if ($scope.orderToInvalidate.length == 0) {
                        $scope.inValidBtnEnable = true;        //Invalidate Button to be disabled.
                    }
                }

            });//end single row
            
            ////// Multiple row selections
            ////gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
            ////    console.log("multiple items checked");
            ////});

        },
    };

    vmReport.inValidateConfirm = inValidateConfirm;
    vmReport.inValidateProcess = inValidateProcess;

    $scope.modalInstance;
    function inValidateConfirm() {
        $confirm({ text: 'Are you sure you want to Invalidate selected order(s)?' }, { size: 'sm' })
       .then(function () {
           inValidateProcess();
       });
    }
    
    function inValidateProcess() {
        console.log("entered invalidate process method.");
        //$scope.modalInstance.close();
        //$scope.pageLoader = true;
            $http.post('ReportingController/InvalidateOrderData', $scope.orderToInvalidate)
               .success(function (data) {
                   $scope.orderToInvalidate = [];

                   if (data.length > 0) {
                       growl.error('Failed to Invalidate following Service Request ID:' + data.join(','));
                       //$scope.pageLoader = false;
                       $scope.inValidBtnEnable = true;
                       return;
                   }
                   else {                       
                       if (vmReport.FilterSection == "1") {
                           var Details = {
                               Fromdate: vmReport.Fromdate.toString(),
                               ThroughDate: vmReport.ThroughDate.toString()
                           }
                           $http.post('ReportingController/GetReportDetails/' + $scope.togglingTenant, Details)
                           .then(function (response) {
                               vmReport.serviceGrid.data = response.data;
                               $scope.inValidBtnEnable = true;
                               growl.success('Record(s) Invalidated Successfully');
                           });
                       } else {
                           $http.get('ReportingController/GetReportDetailsFilter/' + vmReport.FilterSection + '/' + $scope.togglingTenant)
                             .then(function (response) {
                                 vmReport.serviceGrid.data = response.data;
                                 $scope.inValidBtnEnable = true;
                                 growl.success('Record(s) Invalidated Successfully');
                             });
                       }
                   }
                   //$scope.modalInstance.close();
               });   
        
        
    };

    vmReport.search = search;
    search();

    vmReport.changeSelect = changeSelect;

    function changeSelect(item) {
        if (item == 1)
            vmReport.Disabledate = false;
        else
            vmReport.Disabledate = true;
    }

    vmReport.ValidateDate = ValidateDate;
    vmReport.ValidateError = false;

    function ValidateDate() {
        var StartDate = new Date(vmReport.Fromdate);
        var EndDate = new Date(vmReport.ThroughDate);

        vmReport.ValidateError = false;
        if (EndDate < StartDate)
            vmReport.ValidateError = true;
    }

    function search() {        
        $scope.title = 'Orders Summary';

        if (vmReport.gmessage != undefined)
            vmReport.gmessage.destroy();

        if (vmReport.FilterSection == "1") {
            if (!vmReport.Fromdate || !vmReport.ThroughDate) {
                vmReport.gmessage= growl.error("Please enter a valid Start/End date");
                return;
            }
            ValidateDate();
            if (vmReport.ValidateError) {
                vmReport.gmessage = growl.error("End date cannot be earlier than the Start date");
                return;
            }
            var Details = {
                Fromdate: vmReport.Fromdate.toString(),
                ThroughDate: vmReport.ThroughDate.toString()
            }
            vmReport.Busy = true;

            $http.post('ReportingController/GetReportDetails/' + $scope.togglingTenant, Details)
            .then(function (response) {
                    if (!vmReport.showrefnum) {
                        vmReport.serviceGrid.data = response.data;
                    }
                    vmReport.Busy = false;                
            });
        }
        else
        {
            vmReport.Busy = true; 
            $http.get('ReportingController/GetReportDetailsFilter/' + vmReport.FilterSection + '/' + $scope.togglingTenant)
              .then(function (response) {
                      if (!vmReport.showrefnum) {
                          vmReport.serviceGrid.data = response.data;
                      }
                      vmReport.Busy = false;                  
              });
        }
    }

    vmReport.FilterReferenceNoSection = '0';
    vmReport.ReferenceNo = '';
    vmReport.BusyRef = false;
    vmReport.changerefSelect = changerefSelect;
    function changerefSelect(item) {
        if (item == 0)
            vmReport.DisableReferenceNo = true;
        else
            vmReport.DisableReferenceNo = false;
    }    
    
    vmReport.searchbyReferenceNo = searchbyReferenceNo;
    function searchbyReferenceNo()
    {
        if (vmReport.ReferenceNo != '')
        {
            var FDetails = {
                ReferenceNoType: vmReport.FilterReferenceNoSection,
                ReferenceNo: vmReport.ReferenceNo
            }
            vmReport.BusyRef = true;
            
            $http.post('ReportingController/GetReportDetailsbyReferenceFilter/' + $scope.togglingTenant, FDetails)
              .then(function (response) {
                    vmReport.serviceGrid.data = response.data;
                    vmReport.BusyRef = false;
             });           
        }
    }  
   
    vmReport.loadRFOrder = loadRFOrder;
    function loadRFOrder() {        
        $scope.title = 'RF Orders Summary';             

        if (vmReport.gmessage != undefined)
            vmReport.gmessage.destroy();

        if (vmReport.FilterSection == "1") {
            if (!vmReport.Fromdate || !vmReport.ThroughDate) {
                vmReport.gmessage = growl.error("Please enter a valid Start/End date");
                return;
            }
            ValidateDate();
            if (vmReport.ValidateError) {
                vmReport.gmessage = growl.error("End date cannot be earlier than the Start date");
                return;
            }
            var Details = {
                Fromdate: vmReport.Fromdate.toString(),
                ThroughDate: vmReport.ThroughDate.toString()
            }
            vmReport.Busy = true;

            $http.post('ReportingController/GetReportDetails/' + $scope.togglingTenant, Details)
            .then(function (response) {
                if (!vmReport.showrefnum) {
                    vmReport.serviceGrid.data = response.data;
                }
                columnToggle();
                vmReport.Busy = false;
            });
        }
        else {
            vmReport.Busy = true;
            $http.get('ReportingController/GetReportDetailsFilter/' + vmReport.FilterSection + '/' + $scope.togglingTenant)
              .then(function (response) {
                  if (!vmReport.showrefnum) {
                      vmReport.serviceGrid.data = response.data;
                  }
                  columnToggle();
                  vmReport.Busy = false;
              });
        }

    }

    vmReport.switchGridInfo = switchGridInfo;
    function switchGridInfo(val)
    {
        if (val == 'RF') {
            $scope.togglingTenant = 'RF';
            loadRFOrder();
        } else if (val == '') {
            $scope.togglingTenant = $scope.Tenant;
            search();
            columnToggle();
        }
    }

    $http.get('Security/GetTenant')
      .then(function (response) {

          $scope.Tenant = response.data;
          $scope.togglingTenant = $scope.Tenant;
          columnToggle();
      });

    function columnToggle()
    {
        if ($scope.togglingTenant != '') {
            //Code Hide/Display Tenant GridColumn based on Tenant Conditions
            var pos = $scope.vmReport.serviceGrid.columnDefs.map(function (e) { return e.name; }).indexOf('Tenant');
            var posAT = $scope.vmReport.serviceGrid.columnDefs.map(function (e) { return e.name; }).indexOf('Request Action Type');
            var posIRN = $scope.vmReport.serviceGrid.columnDefs.map(function (e) { return e.name; }).indexOf('Internal Reference Number');

            if ($scope.togglingTenant != 'RF') {
                $scope.vmReport.serviceGrid.columnDefs[pos].visible = true;
                $scope.vmReport.serviceGrid.columnDefs[posAT].visible = false;
                $scope.vmReport.serviceGrid.columnDefs[posIRN].visible = true;
            }
            else {
                $scope.vmReport.serviceGrid.columnDefs[pos].visible = false;
                $scope.vmReport.serviceGrid.columnDefs[posAT].visible = true;
                $scope.vmReport.serviceGrid.columnDefs[posIRN].visible = false;
            }
        }
    }

    $scope.expandAll = function () {
        vmReport.serviceGrid.gridApi.treeBase.expandAllRows();
    };

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.closemodal = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.toggleRow = function (rowNum) {
        vmReport.serviceGrid.gridApi.treeBase.toggleRowTreeState($scope.gridApi.grid.renderContainers.body.visibleRowCache[rowNum]);
    };

    vmReport.showrefnum = false;
    vmReport.showdates = true;
    $scope.mynumStyle = { color: '' };
    $scope.mydtStyle = { color: '#007acc' };
    $scope.ShowHide = function (item) {
        if (item == 'showdates') {
            vmReport.showdates = true;
            vmReport.showrefnum = false;
            $scope.mynumStyle = { color: '' };
            $scope.mydtStyle = { color: '#007acc' };
        }
        else if (item == 'showrefnum') {
            vmReport.showrefnum = true;
            vmReport.showdates = false;
            $scope.mydtStyle = { color: '' };
            $scope.mynumStyle = { color: '#007acc' };
        }
    }
}

psReportingRowEditor.$inject = ['$http', '$rootScope', 'modalProvider'];
function psReportingRowEditor($http, $rootScope, modalProvider) {
    
    var service = {};
    service.editReportRow = editReportRow;

    function editReportRow(grid, row) {
        modalProvider.openPopupModal(row.entity.ServiceRequestId);
    }

    return service;
}




