/// <reference path="business-exception-edit.html" />
"use strict";

angular.module('psBusinessException').controller('psBusinessExceptionController', psBusinessExceptionController);
angular.module('psBusinessException').service('psBusinessExceptionRowEditor', psBusinessExceptionRowEditor);


psBusinessExceptionController.$inject = ['$route', '$routeParams', '$scope', '$rootScope', '$http', '$interval', '$uibModal', 'uiGridGroupingConstants', '$window', '$filter', '$cookies', '$confirm', 'UserInfo', 'growl', 'psBusinessExceptionRowEditor', 'uiGridConstants', 'modalProvider', 'Idle', 'Keepalive'];
function psBusinessExceptionController($route, $routeParams, $scope, $rootScope, $http, $interval, $uibModal, uiGridGroupingConstants, $window, $filter, $cookies, $confirm, UserInfo, growl, psBusinessExceptionRowEditor, uiGridConstants, modalProvider, Idle, Keepalive) {
    var vmBEQException = this;
    $scope.$on("getUser", function (evt, response) {
        $rootScope.activityright = response.ActivityRight;
        $rootScope.canmanagebeq = response.CanManageBEQ;
        $scope.hasResubmitAccess = response.CanManageBEQ;
        $rootScope.hasResubmitAccess = response.CanManageBEQ;

    });

    if (!$rootScope.activityright) {
        $rootScope.activityright = $cookies.get('activityright');
    }

    if ($rootScope.activityright !== 'Admin' && $rootScope.activityright !== 'SuperAdmin' && $rootScope.activityright !== 'User') {
        UserInfo.getUser().then(function (response) {
            $rootScope.$broadcast('getUser', response);
            $rootScope.activityright = response.ActivityRight;
            $rootScope.canmanagebeq = response.CanManageBEQ;
            $scope.hasResubmitAccess = response.CanManageBEQ;
            $rootScope.hasResubmitAccess = response.CanManageBEQ;

        }, function (error) {

        });
    }
    else {
        if (!$rootScope.canmanagebeq) {
            UserInfo.getUser().then(function (response) {
                $rootScope.$broadcast('getUser', response);
                $rootScope.canmanagebeq = response.CanManageBEQ;
                $scope.hasResubmitAccess = response.CanManageBEQ;
                $rootScope.hasResubmitAccess = response.CanManageBEQ;

            }, function (error) {

            });
        }
    }
    var hasResubmitAccess = $rootScope.canmanagebeq;
    $scope.hasResubmitAccess = hasResubmitAccess;
    $rootScope.hasResubmitAccess = hasResubmitAccess;
    var showMenuloginfo = true;
    if ($rootScope.tenantname != 'LVIS')
        showMenuloginfo = false;
    else
        showMenuloginfo = true;
   
    vmBEQException.showMenuloginfo = showMenuloginfo;
    var showMenuloginfofastweborders = false;
    if (($rootScope.tenantname == 'LVIS' || $rootScope.tenantname == 'Air Traffic Control') && $rootScope.activityright == 'SuperAdmin')
        showMenuloginfofastweborders = true;
    else
        showMenuloginfofastweborders = false;
    vmBEQException.showMenuloginfofastweborders = showMenuloginfofastweborders;
    vmBEQException.sendEmailToEscrowOfficer = psBusinessExceptionRowEditor.sendEmailToEscrowOfficer;
    var newDate = new Date();
    var date = new Date();
    vmBEQException.Fromdate = $filter('date')(new Date(), 'MM/dd/yyyy');
    vmBEQException.ThroughDate = $filter('date')(new Date(), 'MM/dd/yyyy');
    vmBEQException.DateFilterSelection = [
        {
            'title': 'Custom',
            'value': '1'
        },
        {
            'title': 'All',
            'value': '19'
        },
        {
            'title': 'Last 365 Days',
            'value': '2'
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
    vmBEQException.ReferencenoFilterSelection = [{
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

    // On BEQ tab or bucket selection, this section will set the default "Date Filter" value
    vmBEQException.ExceptionType = ($routeParams.ExceptionType || "");
    if (vmBEQException.ExceptionType != '') {
        vmBEQException.FilterSection = '15';
        // vmBEQException.FilterSection = '90';  //COmmented as per US REquirement to Pull all from Inception Date
    }
    else { vmBEQException.FilterSection = '15'; }

    vmBEQException.Disabledate = true;
    vmBEQException.Busy = false;
    vmBEQException.Typecodestatus = false;

    vmBEQException.Unbind = psBusinessExceptionRowEditor.Unbind;

    vmBEQException.editReportRow = psBusinessExceptionRowEditor.editReportRow;

    var CommentEvent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"  title="{{row.entity.Comments}}"><a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-sticky-note" style="padding:5px 15px;text-align:center;cursor:pointer"></a></div>'

    var MessageEvent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"  title="{{row.entity.Messages}}"><button class="btn btn-info btn-xs" ng-show="row.entity.MessageType && (row.treeNode.children && row.treeNode.children.length == 0)" style="text-align:center;cursor:pointer" ng-click="grid.appScope.sampledetails(grid, row)"> Message Log</button></div>'

    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer"></i></div>'

    vmBEQException.serviceGrid = {
        enableColumnResize: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        enableSelectAll: true,
        paginationPageSizes: [15, 30, 45],
        paginationPageSize: 15,
        minRowsToShow: 16,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        groupingShowAggregationMenu: 0,
        showTreeExpandNoChildren: false,
        columnDefs: [
            { field: 'ExternalRefNum', name: 'ExternalRefNum', displayName: 'External Reference Number', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'ServiceType', name: 'Service', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'TransactionType', name: 'Transaction Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Exceptionid', name: 'Exceptionid', visible: false, headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'ExceptionType', name: 'Exception Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Reporting.ApplicationId', name: 'External Application', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Buyer', name: 'Buyer', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Reporting.CustomerName', name: 'Customer Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            {
                field: 'Status.Name', name: 'Status', filter: {
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ value: '', label: 'All' }, { value: 'New', label: 'New' }, { value: 'Active', label: 'Active' }, { value: 'Hold', label: 'Hold' }, { value: 'Resolved', label: 'Resolved' }]
                }, headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true
            },
            { field: 'Reporting.createddate', name: 'Order Date', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'LastModifiedDate', name: 'LastModifiedDate', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'LastModifiedBy', name: 'LastModifiedBy', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Message Log', enableColumnMenu: false, headerCellClass: 'grid-header', enableFiltering: false, enableCellEdit: false, groupingShowAggregationMenu: false, cellTemplate: MessageEvent },
            { field: 'TypeCodeId', name: '', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: false, visible: false },
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vmBEQException.editReportRow(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",

        onRegisterApi: function (gridApi) {
            vmBEQException.serviceGrid.gridApi = gridApi;
        },
    };
    vmBEQException.ExceptionStatus = 0;
    vmBEQException.search = search;
    search();
    vmBEQException.changeSelect = changeSelect;
    function changeSelect(item) {

        if (item == 1)
            vmBEQException.Disabledate = false;
        else
            vmBEQException.Disabledate = true;
    }

    vmBEQException.ValidateDate = ValidateDate;
    vmBEQException.ValidateError = false;
    var writeoutNode = function (childArray, currentLevel, dataArray) {
        childArray.forEach(function (childNode) {
            if (childNode != undefined) {
                childNode.$$treeLevel = currentLevel;

                dataArray.push(childNode);

                if (childNode.children != undefined)
                    writeoutNode(childNode.children, currentLevel + 1, dataArray);
            }
        });
    };


    function ValidateDate() {
        var StartDate = new Date(vmBEQException.Fromdate);
        var EndDate = new Date(vmBEQException.ThroughDate);
        vmBEQException.ValidateError = false;
        if (EndDate < StartDate)
            vmBEQException.ValidateError = true;
    }

    function search() {
        vmBEQException.serviceGrid.data = [];
        if (vmBEQException.FilterSection == "1") {

            //Include/Exclude Resolved Status Excecption
            if (vmBEQException.IncludeResolve) {
                vmBEQException.Typecodestatus = true;
            }
            else {
                vmBEQException.Typecodestatus = false;
            }

            if (!vmBEQException.Fromdate || !vmBEQException.ThroughDate) {
                growl.error("Please enter a valid Start/End date");
                return;
            }

            ValidateDate();
            if (vmBEQException.ValidateError) {
                growl.error("End date cannot be earlier than the Start date");
                return;
            }
            var Details = {
                Fromdate: vmBEQException.Fromdate.toString(),
                ThroughDate: vmBEQException.ThroughDate.toString(),
                statusId: 0,
                Typecodestatus: vmBEQException.Typecodestatus
            }
            vmBEQException.Busy = true;

            if (vmBEQException.ExceptionType != "") {
                $http.post('ExceptionController/GetBEQExceptionsbyType/' + vmBEQException.ExceptionType, Details)
                    .then(function (response) {
                        if (!vmBEQException.showrefnum) {
                            var data = [];
                            data = response.data;
                            writeoutNode(data, 0, vmBEQException.serviceGrid.data);
                        }
                        vmBEQException.Busy = false;
                        Idle.watch();
                    },

                     function (error) {
                            growl.error("There was an error Loading exceptions");
                         vmBEQException.Busy = false;
                        });
            }
            else {
                $http.post('ExceptionController/GetBEQExceptions', Details)
                    .then(function (response) {
                        if (!vmBEQException.showrefnum) {
                            var data = [];
                            data = response.data;
                            writeoutNode(data, 0, vmBEQException.serviceGrid.data);
                        }
                        vmBEQException.Busy = false;


                        Idle.watch();

                    },

                        function (error) {
                            growl.error("There was an error Loading exceptions");
                            vmBEQException.Busy = false;
                        });
            }
        }
        else {

            vmBEQException.Busy = true;
            //Include/Exclude Resolved Status Excecption
            if (vmBEQException.IncludeResolve) {
                vmBEQException.Typecodestatus = true;
            }
            else {
                vmBEQException.Typecodestatus = false;
            }

            if (vmBEQException.ExceptionType != "") {
                $http.get('ExceptionController/GetBEQExceptionsbyFilterType/' + vmBEQException.FilterSection + '/' + vmBEQException.Typecodestatus + '/' + vmBEQException.ExceptionType)
                    .then(function (response) {
                        if (!vmBEQException.showrefnum) {
                            var data = [];
                            data = response.data;
                            writeoutNode(data, 0, vmBEQException.serviceGrid.data);
                        }
                        vmBEQException.Busy = false;
                        Idle.watch();
                    },

                        function (error) {
                            growl.error("There was an error Loading exceptions");
                            vmBEQException.Busy = false;
                        });
            }
            else {
                $http.get('ExceptionController/GetBEQExceptionsbyFilter/' + vmBEQException.FilterSection + '/' + vmBEQException.Typecodestatus)
                    .then(function (response) {
                        if (!vmBEQException.showrefnum) {
                            var data = [];
                            data = response.data;
                            writeoutNode(data, 0, vmBEQException.serviceGrid.data);
                        }
                        vmBEQException.Busy = false;




                        Idle.watch();
                    },

                        function (error) {
                            growl.error("There was an error Loading exceptions");
                            vmBEQException.Busy = false;
                        });
            }
        }
    }

    //Filter By Reference Num
    vmBEQException.FilterReferenceNoSection = '0';
    vmBEQException.ReferenceNo = '';
    vmBEQException.BusyRef = false;
    vmBEQException.changerefSelect = changerefSelect;
    function changerefSelect(item) {
        if (item == 0)
            vmBEQException.DisableReferenceNo = true;
        else
            vmBEQException.DisableReferenceNo = false;
    }

    vmBEQException.searchbyReferenceNo = searchbyReferenceNo;
    function searchbyReferenceNo() {
        if (vmBEQException.ReferenceNo != '') {
            var FDetails = {
                ReferenceNoType: vmBEQException.FilterReferenceNoSection,
                ReferenceNo: vmBEQException.ReferenceNo
            }
            vmBEQException.BusyRef = true;
            $http.post('ExceptionController/GetBEQExceptionByReferenceNum', FDetails)
                .then(function (response) {
                    vmBEQException.serviceGrid.data = response.data;
                    vmBEQException.BusyRef = false;
                },

                    function (error) {
                        growl.error("There was an error Loading exceptions");
                        vmBEQException.BusyRef = false;
                    });
        }
    }

    $scope.expandAll = function () {
        vmBEQException.serviceGrid.gridApi.treeBase.expandAllRows();
    };

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.closemodal = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.toggleRow = function (rowNum) {
        vmBEQException.serviceGrid.gridApi.treeBase.toggleRowTreeState($scope.gridApi.grid.renderContainers.body.visibleRowCache[rowNum]);
    };

    $scope.sampledetails = function (grid, row) {
        modalProvider.openPopupModal(row.entity.ServiceRequestId);
    };

    $scope.changeGrouping = function () {
        $scope.gridApi.grouping.clearGrouping();
        //$scope.gridApi.grouping.groupColumn('Reporting.ApplicationId=4');
    };

    vmBEQException.showrefnum = false;
    vmBEQException.showdates = true;
    $scope.mynumStyle = { color: '' };
    $scope.mydtStyle = { color: '#007acc' };

    $scope.ShowHide = function (item) {
        if (item == 'showdates') {
            vmBEQException.showdates = true;
            vmBEQException.showrefnum = false;
            $scope.mynumStyle = { color: '' };
            $scope.mydtStyle = { color: '#007acc' };
        }
        else if (item == 'showrefnum') {
            vmBEQException.showrefnum = true;
            vmBEQException.showdates = false;
            $scope.mydtStyle = { color: '' };
            $scope.mynumStyle = { color: '#007acc' };
        }
    }

    var pos = $scope.vmBEQException.serviceGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname === 'LVIS')
        $scope.vmBEQException.serviceGrid.columnDefs[pos].visible = true;
    else
        $scope.vmBEQException.serviceGrid.columnDefs[pos].visible = false;

    Idle.watch();

    $scope.$on('IdleStart', function () {
        ////Implementation to notify user that in few mins the permitted Idle time will end
    });

    $scope.$on('IdleEnd', function () {
        ////Restarting the Idle timer
    });

    $scope.$on('IdleTimeout', function () {
        ////Timeout for the Idle User
        if (!vmBEQException.showrefnum)
            search();
        else
            searchbyReferenceNo();
    });
}

psBusinessExceptionRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psBusinessExceptionRowEditor($http, $rootScope, $uibModal) {

    var service = {};

    service.editReportRow = editReportRow;
    service.Unbind = Unbind;
    function Unbind() {
        $uibModal.open({
            templateUrl: 'ext-modules/psExceptions/psBusinessExceptions/BEQException-UNBind.html',
            controller: 'psBusinessExceptionUnbindCtrl',
            size: 'lg',
            controllerAs: 'vmBEQException'
        });
    }

    function editReportRow(grid, row) {
        var instance = $uibModal.open({
            templateUrl: 'ext-modules/psExceptions/psBusinessExceptions/business-exception-edit.html',
            controller: 'psBusinessExceptionRowEditCtrl',
            controllerAs: 'vmBEQException',
            size: 'lg',
            resolve: {
                grid: function () {
                    return grid;
                },
                row: function () {
                    return row;
                }

            }
        });

        instance.result.then(function () {
            //Get triggers when modal is closed
        }, function () {
            var index = grid.appScope.vmBEQException.serviceGrid.data.indexOf(row.entity);
            if (row.entity.Exceptionid == 0) {
                row.entity.children.forEach(function (childNode) {
                    if (childNode != undefined) {
                        var childindex = grid.appScope.vmBEQException.serviceGrid.data.indexOf(childNode);
                        grid.appScope.vmBEQException.serviceGrid.data.splice(childindex, 1);
                        row.entity.children.InvolveResolved = true;
                    }
                });
                row.entity.InvolveResolved = true;
                grid.appScope.vmBEQException.serviceGrid.data.splice(index, 1);
            }
            else {
                row.entity.InvolveResolved = true;
                grid.appScope.vmBEQException.serviceGrid.data.splice(index, 1);
            }
        });
    }

    return service;
}

angular.module('psBusinessException').controller('psBusinessExceptionRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', '$rootScope', 'growl', '$confirm', '$uibModal', '$uibModalStack', 'Idle', 'Keepalive',
    function psBusinessExceptionRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, $rootScope, growl, $confirm, $uibModal, $uibModalStack, Idle, Keepalive) {
        var vmBEQException = this;
        vmBEQException.showffidSearch = false;
        vmBEQException.entity = angular.copy(row.entity);
        vmBEQException.entity.FastFile = "";
        vmBEQException.entity.FastFileID = "";
        vmBEQException.save = save;
        vmBEQException.Messagechanged = false;
        vmBEQException.SearchFile = SearchFile;
        vmBEQException.FilterDet = FilterDet;
        vmBEQException.enablesave = true;
        vmBEQException.resubmit = resubmit;
        vmBEQException.Resubmit = false;
        $scope.$on("getUser", function (evt, response) {
            $rootScope.activityright = response.ActivityRight;
        });
        if (!$rootScope.activityright) {
            $rootScope.activityright = $cookies.get('activityright');
        }

        
        vmBEQException.showffidSearch = $rootScope.tenantname == 'LVIS' && $rootScope.activityright == 'SuperAdmin'; 
        //End-Bind-Provider List
        vmBEQException.RegionList = [];
        vmBEQException.PotentialMatches = [];

        Idle.watch();

        $scope.$on('IdleStart', function () {
            ////Implementation to notify user that in few mins the permitted Idle time will end
        });

        $scope.$on('IdleEnd', function () {
            ////Restarting the Idle timer
        });

        $scope.$on('IdleTimeout', function () {
            ////Timeout for the Idle User

            $uibModalInstance.dismiss();
        });

        $http.get('Security/GetRegionsWithoutClaim/Fast').then(function (data) {
            vmBEQException.RegionList = data.data;
        });

        $http.get('ExceptionController/GetExceptionStatus')
            .then(function (response) {
                vmBEQException.StatusList = response.data;
            });

        vmBEQException.loading = true;

        vmBEQException.FastMatches = [];
        vmBEQException.FastFilterMatches = [];
        vmBEQException.SearchCriteria = [];

        if (vmBEQException.entity.Exceptionid == 0) {
            $http.post('ExceptionController/BEQParseParent', vmBEQException.entity)
                .then(function (response) {
                    vmBEQException.BEQException = response.data;
                    vmBEQException.SearchCriteria.FileNumber = vmBEQException.BEQException.FastFile;
                    vmBEQException.SearchCriteria.City = vmBEQException.BEQException.City;
                    vmBEQException.SearchCriteria.County = vmBEQException.BEQException.County;
                    vmBEQException.SearchCriteria.State = vmBEQException.BEQException.State;
                    vmBEQException.SearchCriteria.Organization = vmBEQException.BEQException.Organization;
                    vmBEQException.SearchCriteria.LenderName = vmBEQException.BEQException.LenderName;
                    vmBEQException.SearchCriteria.Address = vmBEQException.BEQException.Address;
                    vmBEQException.SearchCriteria.Buyer = vmBEQException.BEQException.Buyer;

                    $http.post('ExceptionController/BEQPotentialSourceMatches/' + vmBEQException.entity.TenantId, vmBEQException.BEQException.FastFileIDs).then(function (data) {
                        vmBEQException.PotentialMatches = [];
                        if (data.data != null)
                            vmBEQException.PotentialMatches = vmBEQException.PotentialMatches.concat(data.data);
                        vmBEQException.loading = false;
                    },
                        function (error) {
                            growl.error("There was an error in loading Potential Source matches of this exception");
                            vmBEQException.loading = false;
                        });
                },
                    function (error) {
                        growl.error("There was an error in loading details of this exception");
                        vmBEQException.loading = false;
                    });
        }
        else {
            $http.get('ExceptionController/BEQParse/' + vmBEQException.entity.DocumentObjectid)
                .then(function (response) {
                    vmBEQException.BEQException = response.data;
                    vmBEQException.SearchCriteria.City = vmBEQException.BEQException.City;
                    vmBEQException.SearchCriteria.FileNumber = vmBEQException.BEQException.FastFile;
                    vmBEQException.SearchCriteria.County = vmBEQException.BEQException.County;
                    vmBEQException.SearchCriteria.State = vmBEQException.BEQException.State;
                    vmBEQException.SearchCriteria.Organization = vmBEQException.BEQException.Organization;
                    vmBEQException.SearchCriteria.LenderName = vmBEQException.BEQException.LenderName;
                    vmBEQException.SearchCriteria.Address = vmBEQException.BEQException.Address;
                    vmBEQException.SearchCriteria.Buyer = vmBEQException.BEQException.Buyer;
                    $http.post('ExceptionController/BEQPotentialSourceMatches/' + vmBEQException.entity.TenantId, vmBEQException.BEQException.FastFileIDs).then(function (data) {
                        vmBEQException.PotentialMatches = [];
                        if (data.data != null)
                            vmBEQException.PotentialMatches = vmBEQException.PotentialMatches.concat(data.data);
                        vmBEQException.loading = false;
                    },
                        function (error) {
                            growl.error("There was an error Loading Potential Source matches of this exception");
                            vmBEQException.loading = false;
                        });
                },
                    function (error) {
                        growl.error("There was an error in Loading Details of this exception");
                        vmBEQException.loading = false;
                    });
        }

        $http.post('ExceptionController/GetExceptionComments', vmBEQException.entity)
            .then(function (response) {
                vmBEQException.entity.Comments = response.data;
            });

        vmBEQException.setContent = function (servicerequestid) {

            $http.get('ExceptionController/BEQParentXml/' + servicerequestid)
                .then(function (response) {
                    var XmlContent = response.data;


                    $uibModal.open({
                        templateUrl: 'ext-modules/psExceptions/psBusinessExceptions/MessageExceptionView.html',
                        controller: 'BEQMessageLogModuleMessagecntrl',
                        controllerAs: 'vmBEQException',
                        resolve: {
                            XmlContent: function () {
                                return XmlContent;
                            }
                        }
                    });
                });
        };

        var getIndexIfObjWithOwnAttr = function (array, attr, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                    return i;
                }
            }
            return -1;
        }

        $scope.disabledResetButton = true;
        vmBEQException.ResetRefreshBotton = ResetRefreshBotton;
        function ResetRefreshBotton() {
            $scope.disabledResetButton = false;
        }

        vmBEQException.RefreshSearch = RefreshSearch;
        function RefreshSearch() {
            vmBEQException.entity.FastFile = '';
            vmBEQException.entity.Region = '';
            vmBEQException.entity.FastFileID = '';
            vmBEQException.SearchCriteria.City = vmBEQException.BEQException.City;
            vmBEQException.SearchCriteria.FileNumber = vmBEQException.BEQException.FastFile;
            vmBEQException.SearchCriteria.County = vmBEQException.BEQException.County;
            vmBEQException.SearchCriteria.State = vmBEQException.BEQException.State;
            vmBEQException.SearchCriteria.Organization = vmBEQException.BEQException.Organization;
            vmBEQException.SearchCriteria.LenderName = vmBEQException.BEQException.LenderName;
            vmBEQException.SearchCriteria.Address = vmBEQException.BEQException.Address;
            vmBEQException.SearchCriteria.Buyer = vmBEQException.BEQException.Buyer;
            

            $scope.disabledResetButton = true;
        }

        function save() {

            var index = getIndexIfObjWithOwnAttr(vmBEQException.StatusList, "ID", vmBEQException.entity.Status.ID);
            if (index > -1)
                vmBEQException.entity.Status = vmBEQException.StatusList[index];

            if (vmBEQException.entity.Notes != "") {
                $http.post('ExceptionController/SaveBEQExceptionComments', vmBEQException.entity)
                    .then(function (response) {
                        row.entity = response.data;
                        row.entity.Comments = response.data.Comments;
                        vmBEQException.entity.Comments = response.data.Comments;
                        vmBEQException.entity.Notes = "";
                        grid.data = response.data;

                        growl.success("Exception information was saved successfully");
                        $uibModalInstance.close(row.entity);
                    },
                    function (error) {
                        growl.error("There was error in saving this order");
                    }); 
                
            }
        }

        function SearchFile() {

            var match = false;
            if (vmBEQException.FastMatches != null && vmBEQException.FastMatches != undefined) {

                vmBEQException.FastMatches.forEach(function (arrayItem) {

                    if (arrayItem != null) {
                        if (arrayItem.FileNumber == vmBEQException.entity.FastFile && arrayItem.SourceProvidedMatch == false && vmBEQException.entity.Region.Id == arrayItem.RegionID)
                            match = true;
                    }
                });
            }

            if (match == false) {
                vmBEQException.Matchloading = true;

                if (vmBEQException.entity.FastFile != null && vmBEQException.entity.FastFile != undefined && vmBEQException.entity.FastFile == "") {

                    var SearchDetails = {
                        FileNumber: vmBEQException.SearchCriteria.FileNumber,
                        City: vmBEQException.SearchCriteria.City,
                        County: vmBEQException.SearchCriteria.County,
                        State: vmBEQException.SearchCriteria.State,
                        Address: vmBEQException.SearchCriteria.Address,
                        Buyer: vmBEQException.SearchCriteria.Buyer,
                        Organization: vmBEQException.SearchCriteria.Organization,
                        LenderName: vmBEQException.SearchCriteria.LenderName,
                        TenantId: vmBEQException.entity.TenantId,
                        FastFileID: vmBEQException.entity.FastFileID
                    }


                    vmBEQException.Matchloading = true;
                    $http.post('ExceptionController/SearchFastDetails', SearchDetails)
                        .then(function (response) {
                            if (response.data != null) {
                                vmBEQException.FastMatches = vmBEQException.FastMatches.concat(response.data);
                            };
                            vmBEQException.FastFilterMatches = vmBEQException.FastMatches;
                            vmBEQException.Matchloading = false;
                        },
                            function (error) {
                                growl.error("There was an error in searching this order");
                            });
                }

                else {
                    $http.get('ExceptionController/SearchFastFile/' + vmBEQException.entity.Region.Id + "/" + vmBEQException.entity.FastFile + "/false/" + vmBEQException.entity.TenantId)
                        .then(function (response) {
                            if (response.data != null) {
                                vmBEQException.FastMatches = vmBEQException.FastMatches.concat(response.data);
                            };
                            vmBEQException.FastFilterMatches = vmBEQException.FastMatches;
                            vmBEQException.Matchloading = false;
                        },
                            function (error) {
                                growl.error("There was an error in searching this order");
                            });
                }
            }
            else {
                vmBEQException.FastFilterMatches = vmBEQException.FilterDet(vmBEQException.FastMatches);
            }
        }


        function FilterDet(ExceptionLogs) {

            if (ExceptionLogs != null && vmBEQException.entity.FastFile != '' && vmBEQException.entity.FastFile != undefined) {

                var Matches = [];

                ExceptionLogs.forEach(function (arrayItem) {

                    if (arrayItem != null) {
                        if (arrayItem.FileNumber == vmBEQException.entity.FastFile)
                            Matches.push(arrayItem);
                    }
                });

                return Matches;
            }
            return (ExceptionLogs);
        }

        vmBEQException.NewOrder = function () {

            $uibModal.open({
                templateUrl: 'ext-modules/psExceptions/psBusinessExceptions/BEQException-CreateOrder.html',
                controller: 'psBusinessExceptionNewOrderCtrl',
                controllerAs: 'vmBEQException',
                resolve: {
                    Exception: function () {
                        return vmBEQException.entity;
                    }
                }
            });
        }

        //Delte Operation
        vmBEQException.Delete = function () {
            $uibModal.open({
                templateUrl: 'ext-modules/psExceptions/psBusinessExceptions/BEQException-Delete.html',
                controller: 'psBusinessExceptionDeleteCtrl',
                size: 'sm',
                controllerAs: 'vmBEQException',
                resolve: {
                    Exception: function () {
                        return vmBEQException.entity;
                    }
                }
            });
        }

        vmBEQException.RejectOrder = function () {
            $uibModal.open({
                templateUrl: 'ext-modules/psExceptions/psBusinessExceptions/BEQException-Reject.html',
                controller: 'psBusinessExceptionRejectCtrl',
                controllerAs: 'vmBEQException',
                resolve: {
                    Exception: function () {
                        return vmBEQException.entity;
                    }
                }
            });
        }

        vmBEQException.FastUpdate = function () {
            $uibModal.open({
                templateUrl: 'ext-modules/psExceptions/psBusinessExceptions/BEQException-UpdateFast.html',
                controller: 'psBusinessExceptionUpdateFastCtrl',
                controllerAs: 'vmBEQException',
                resolve: {
                    PotentialMatches: function () {
                        return vmBEQException.PotentialMatches;
                    },
                    Exception: function () {
                        return vmBEQException.entity;
                    }
                }
            });
        }

        vmBEQException.sendEmail = function () {

            $uibModal.open({
                templateUrl: 'ext-modules/psExceptions/psBusinessExceptions/BEQException-Email.html',
                controller: 'psBusinessExceptionEmailCtrl',
                controllerAs: 'vmBEQException',
                resolve: {
                    Exception: function () {
                        return vmBEQException;
                    }
                }
            });
        }

        vmBEQException.Bind = function (Match) {
            $uibModal.open({
                templateUrl: 'ext-modules/psExceptions/psBusinessExceptions/BEQException-Bind.html',
                controller: 'psBusinessExceptionBindCtrl',
                controllerAs: 'vmBEQException',
                resolve: {
                    Match: function () {
                        return Match;
                    },
                    Exception: function () {
                        return vmBEQException.entity;
                    }
                }
            });
        }

        function PostResubmit() {
            vmBEQException.Messageresubmit = true;
            $http.post('ExceptionController/ResubmitBEQException', vmBEQException.entity)
                .then(function (response) {
                    row.entity = response.data;
                    row.entity.Comments = response.data.Comments;
                    vmBEQException.entity.Comments = response.data.Comments;
                    vmBEQException.entity = row.entity;
                    vmBEQException.entity.Notes = "";
                    grid.data = response.data;
                    vmBEQException.Resubmit = false;

                    growl.success("Exception was resubmitted successfully");
                    $uibModalStack.dismissAll("Exception was resubmitted successfully");
                    $uibModalInstance.close(row.entity);
                },
                    function (error) {
                        growl.error("There was an error in resubmitting this exception");
                        vmBEQException.Resubmit = true;
                        vmBEQException.Messageresubmit = false;
                    });
        }

        function resubmit() {
            if (!vmBEQException.Messagechanged) {
                $confirm({ text: 'Are you sure you want to Resubmit?' }, { size: 'sm' })
                    .then(function () {
                        vmBEQException.Messageresubmit = false;
                        PostResubmit();
                    });
            }
            else {
                PostResubmit();
            }
        }

        $http.get('ExceptionController/GetMessageContent/' + vmBEQException.entity.DocumentObjectid)
            .then(function (response) {
                vmBEQException.entity.MessageContent = response.data;
            });
    }]);

angular.module('psBusinessException').controller('psBusinessExceptionEmailCtrl', ['$http', '$uibModalInstance', 'Exception', 'growl', '$uibModalStack',
    function psBusinessExceptionEmailCtrl($http, $uibModalInstance, Exception, growl, $uibModalStack) {
        var vmBEQException = this;
        vmBEQException.entity = angular.copy(Exception.entity);
        vmBEQException.Exception = angular.copy(Exception.BEQException);
        vmBEQException.emailBodyDesc = "";
        vmBEQException.EMailNotes = "";
        switch (vmBEQException.entity.ExceptionType.toUpperCase().trim()) {
            case "DUPLICATE_ORDER_SOURCE":
                vmBEQException.emailBodyDesc = "Duplicate Order Source";
                break;
            case "MISMATCH_LENDER":
                vmBEQException.emailBodyDesc = "This message is being sent to you because the lender integrated order request was routed to the Business Exception Queue (BEQ). The error is due to the mismatch of the current lender in the New Loan Screen in FAST and the lender on the integrated order request. Please confirm the lender with either the buyer, broker and/or contract addendums then update your FAST file accordingly. All FAST data must match the order details below in order for us to connect the orders.  If unable to confirm, please advise if we should reject the order back to the lender, due to a pending contract update.\n\n" +
                    "We will hold the order in the BEQ for 3 business days before we reject automatically back to the lender.\n";
                break;
            case "MISMATCH_OFFICE_ID":
                vmBEQException.emailBodyDesc = "Mismatch Office ID";
                break;
            case "NEW_SERVICE_RECEIVED":
                vmBEQException.emailBodyDesc = "New Service Received";
                break;
            case "NO_GOOD_MATCH":
                vmBEQException.emailBodyDesc = "This message is being sent to you because the lender integrated order request was routed to the Business Exception Queue (BEQ). The error is due to no existing FAST file to connect to.\n\n" +
                    "For more details, please visit: https://falive.net/docs/DOC-74166 \n\n" +
                    "Also, all FAST data must match the order details below in order for us to connect the orders. We will hold the order in the BEQ for 3 business days before we reject automatically back to the lender.\n\n" +
                    "Please use the contacts we provide to you below to obtain the contract, if necessary.\n";
                break;
            case "POTENTIAL_MATCH_FOUND":
                vmBEQException.emailBodyDesc = "Potential Match Found";
                break;
            case "UNBOUND_ORDER":
                vmBEQException.emailBodyDesc = "Unbound Order";
                break;
            case "UNHANDLED":
                vmBEQException.emailBodyDesc = "Unhandled Excpetion Occured";
                break;
            case "UNHANDLED_SERVICE_TYPE":
                vmBEQException.emailBodyDesc = "Unhandled Service Type";
                break;
            case "Piggyback_Order":
                vmBEQException.emailBodyDesc = "Piggyback Order";
                break;
            case "Duplicate_Service_Requested":
                vmBEQException.emailBodyDesc = "Duplicate Service Requested";
                break;
            case "Multiple_Match_Found":
                vmBEQException.emailBodyDesc = "Multiple Match Found";
                break;
            case "Unhandled_Transaction_Type":
                vmBEQException.emailBodyDesc = "Unhandled Transaction Type";
                break;
            case "Updated_Unbound_Order":
                vmBEQException.emailBodyDesc = "Updated Unbound Order";
                break;
            case "Invalid_Order_Data":
                vmBEQException.emailBodyDesc = "Invalid Order Data";
                break;
            default:
                vmBEQException.emailBodyDesc = "";
                break;
        }

        vmBEQException.EMailNotes = vmBEQException.emailBodyDesc +
            "\n\nFAST region: " + ((vmBEQException.Exception.RegionName) ? vmBEQException.Exception.RegionName : "") + "\n" +
            "Transaction Type: " + ((vmBEQException.Exception.Transaction) ? vmBEQException.Exception.Transaction : "") + "\n" +
            "Service Type: " + ((vmBEQException.entity.ServiceType) ? vmBEQException.entity.ServiceType : "") + "\n" +
            "Property Address: " +
            ((vmBEQException.Exception.Address) ? (vmBEQException.Exception.Address + ", ") : "") +
            ((vmBEQException.Exception.City) ? (vmBEQException.Exception.City + ", ") : "") +
            ((vmBEQException.Exception.State) ? (vmBEQException.Exception.State + ", ") : "") +
            ((vmBEQException.Exception.County) ? (vmBEQException.Exception.County + "\n") : "\n") +
            "Buyer: " + ((vmBEQException.Exception.Buyer) ? vmBEQException.Exception.Buyer : "") + "\n" +
            "Escrow Officer: " + ((vmBEQException.Exception.escrowOfficer) ? vmBEQException.Exception.escrowOfficer : "") + "\n" +
            "Escrow Assistant: " + ((vmBEQException.Exception.escrowAssistant) ? vmBEQException.Exception.escrowAssistant : "") + "\n" +
            "Loan Officer: " + ((vmBEQException.Exception.LoanOfficer) ? vmBEQException.Exception.LoanOfficer : "") + "\n\n" +
            //"Created Date: " + ((vmBEQException.entity.CreatedDate) ? vmBEQException.entity.CreatedDate : "") + "\n\n" +
            "Regards,\n" + "Direct Integration Team\n";

        vmBEQException.email = function () {
            var escrowOfficerEmailValue = document.getElementById('escrowOfficerEmailId').value;
            var escrowAssistantEmailValue = document.getElementById('escrowAssistantEmailId').value;
            var emailBody = document.getElementById('emailbody-textarea').value;
            if ((escrowOfficerEmailValue != null && escrowOfficerEmailValue != "") || (escrowAssistantEmailValue != null && escrowAssistantEmailValue != "")) {

                var EmailDetail = {
                    EmailId: escrowOfficerEmailValue + ((escrowAssistantEmailValue != null || escrowAssistantEmailValue != "") ? (";" + escrowAssistantEmailValue) : ""),
                    EmailSubject: "BEQ Exception - " + vmBEQException.entity.ParentExternalRefNum,
                    EmailBody: emailBody,
                }

                $http.post('ExceptionController/SentMail', EmailDetail)
                    .then(function (response) {
                        var addOfficerInfo = "";
                        var addAssistantInfo = "";
                        if (escrowOfficerEmailValue != null && escrowOfficerEmailValue != "")
                            addOfficerInfo = "Escrow Officer"
                        if (escrowAssistantEmailValue != null && escrowAssistantEmailValue != "")
                            addAssistantInfo = "Escrow Assistant"
                        if (response) {
                            $uibModalInstance.close()
                            growl.success("Email sent to " + addOfficerInfo + ((addOfficerInfo != "" && addAssistantInfo != "") ? (" and " + addAssistantInfo) : addAssistantInfo));
                        }
                        else
                            growl.error("Something went wrong");

                    }, function (error) {
                        $rootScope.errors = { unauthorized: "You are not authorized to access this application." };
                        deferred.reject(error);
                    });
            }
            else
                growl.error("Escrow Officer Email Id or Escrow Assistant Email Id required");
        }

    }]);


angular.module('psBusinessException').controller('psBusinessExceptionUpdateFastCtrl', ['$http', '$uibModalInstance', 'PotentialMatches', 'Exception', 'growl', '$uibModalStack',
    function psBusinessExceptionUpdateFastCtrl($http, $uibModalInstance, PotentialMatches, Exception, growl, $uibModalStack) {
        var vmBEQException = this;
        vmBEQException.entity = angular.copy(Exception);
        vmBEQException.PotentialMatches = angular.copy(PotentialMatches);
        vmBEQException.entity.update = vmBEQException.PotentialMatches[0];
        vmBEQException.ExternalRefNum = vmBEQException.entity.ExternalRefNum;
        vmBEQException.Messagerejectchanged = false;
        vmBEQException.RegionList = [];
        $http.get('Security/GetRegionsWithoutClaim/Fast').then(function (data) {
            vmBEQException.RegionList = data.data;
        });
        vmBEQException.FileNumber = null; 
        vmBEQException.Region = null;
        vmBEQException.FileID = 0;
        vmBEQException.isPotentialMathcFound = vmBEQException.entity.update ? true : false;
        

        vmBEQException.Update = function () {
            var url = "";
            var successMessage = ""
            if (vmBEQException.isPotentialMathcFound) {
                url = 'ExceptionController/BEQUpdate/' + vmBEQException.ExternalRefNum + "/" + vmBEQException.entity.update.FileNumber + "/" + vmBEQException.entity.update.FileID;
                successMessage = "External Reference Number " + vmBEQException.ExternalRefNum + " was updated successfully to " + vmBEQException.entity.update.FileNumber;
            } else {
                url = 'ExceptionController/BEQUpdate/' + vmBEQException.ExternalRefNum + "/" + vmBEQException.FileNumber + "/" + vmBEQException.FileID;
                successMessage = "External Reference Number " + vmBEQException.ExternalRefNum + " was updated successfully to " + vmBEQException.FileNumber;
            }

            $http.post(url, vmBEQException.entity)
                .then(function (response) {
                    if (response.data == true) {
                        growl.success(successMessage);
                        $uibModalStack.dismissAll("Exception was bound successfully");
                    }
                },
                    function (error) {
                        growl.error("There was an error in Binding/Updating this exception");
                    });
        }


        vmBEQException.UpdateReject = function () {
            var url = "";
            var successMessage = ""
            if (vmBEQException.isPotentialMathcFound) {
                url = 'ExceptionController/BEQUpdateReject/' + vmBEQException.ExternalRefNum + "/" + vmBEQException.entity.update.FileNumber + "/" + vmBEQException.entity.update.FileID;
                successMessage = "External Reference Number " + vmBEQException.ExternalRefNum + " was updated successfully to " + vmBEQException.entity.update.FileNumber;
            } else {
                url = 'ExceptionController/BEQUpdateReject/' + vmBEQException.ExternalRefNum + "/" + vmBEQException.FileNumber + "/" + vmBEQException.FileID;
                successMessage = "External Reference Number " + vmBEQException.ExternalRefNum + " was updated successfully to " + vmBEQException.FileNumber;

            }
            $http.post(url, vmBEQException.entity)
                .then(function (response) {
                    if (response.data == true) {
                        growl.success(successMessage);
                        $uibModalStack.dismissAll("Exception was Updated and Rejected successfully");
                    }
                },
                    function (error) {
                        growl.error("There was an error in BEQ Update and Reject this exception");
                    });
        }

        vmBEQException.fileFound = false;
        vmBEQException.SearchFile = function SearchFile() {
            $http.get('ExceptionController/SearchFastFile/' + vmBEQException.Region.Id + "/" + vmBEQException.FileNumber + "/false/0")
                .then(function (response) {
                    vmBEQException.fileFound = response.data ? true : false
                    if (vmBEQException.fileFound) {
                        vmBEQException.FileID = response.data.FileID
                        growl.success("Excact Match found");
                    } else {
                        growl.error("Excact Match not found");
                    }
                },
                    function (error) {
                        growl.error("Error: Something went wrong, please contact administrator");
                    });
        }
    }]);


angular.module('psBusinessException').controller('psBusinessExceptionBindCtrl', ['$http', '$uibModalInstance', 'Match', 'Exception', 'growl', '$uibModalStack',
    function psBusinessExceptionBindCtrl($http, $uibModalInstance, Match, Exception, growl, $uibModalStack) {
        var vmBEQException = this;
        vmBEQException.entity = angular.copy(Exception);
        vmBEQException.Match = angular.copy(Match);

        vmBEQException.Match.ExceptionID = Exception.Exceptionid;
        vmBEQException.Messagebindchanged = false;
        vmBEQException.BindOrder = function () {

            if (Exception.Exceptionid == 0) {
                vmBEQException.Messagebindchanged = true;
                $http.post('ExceptionController/BEQBindAllOrder/' + vmBEQException.Match.FileNumber + "/" + vmBEQException.Match.FileID + "/" + encodeString(vmBEQException.FileNotes), vmBEQException.entity)
                    .then(function (response) {
                        if (response.data == true) {
                            growl.success("External Reference Number " + vmBEQException.entity.ExternalRefNum + " was bound successfully to " + vmBEQException.Match.FileNumber);
                            $uibModalStack.dismissAll("Exception was bound successfully");
                            vmBEQException.Messagebindchanged = true;
                        }
                    },
                        function (error) {
                            growl.error("There was an error in binding this exception");
                            vmBEQException.Messagebindchanged = false;
                        });
            }
            else {
                vmBEQException.Messagebindchanged = true;
                $http.post('ExceptionController/BEQBindOrder/' + encodeString(vmBEQException.FileNotes), vmBEQException.Match)
                    .then(function (response) {
                        if (response.data == true) {
                            growl.success("External Reference Number " + vmBEQException.entity.ExternalRefNum + " was bound successfully to " + vmBEQException.Match.FileNumber);
                            $uibModalStack.dismissAll("Exception was bound successfully");
                            vmBEQException.Messagebindchanged = true;
                        }
                    },
                        function (error) {
                            growl.error("There was an error in binding this exception");
                            vmBEQException.Messagebindchanged = false;
                        });
            }
        };

    }]);


angular.module('psBusinessException').controller('psBusinessExceptionUnbindCtrl', ['$http', '$uibModalInstance', 'growl', '$confirm',
    function psBusinessExceptionUnbindCtrl($http, $uibModalInstance, growl, $confirm) {

        var vmBEQException = this;
        vmBEQException.PotentialMatches = [];
        vmBEQException.RegionList = [];
        vmBEQException.SearchFile = SearchFile;
        vmBEQException.show = false;
        vmBEQException.loading = false;

        $http.get('Security/GetRegionsWithoutClaim/Fast').then(function (data) {
            vmBEQException.RegionList = data.data;
        });

        function SearchFile() {
            vmBEQException.PotentialMatches = [];
            vmBEQException.loading = true;
            $http.get('ExceptionController/SearchFastFile/' + vmBEQException.entity.Region.Id + "/" + vmBEQException.entity.FastFile + "/true/0")
                .then(function (response) {
                    vmBEQException.show = true;
                    vmBEQException.loading = false;
                    if (response.data != null) {
                        vmBEQException.PotentialMatches.push(response.data);
                        vmBEQException.Messageunbindchanged = true;
                    }
                    else {
                        vmBEQException.show = true;
                        vmBEQException.Messageunbindchanged = true;
                    }
                },
                    function (error) {
                        vmBEQException.show = false;
                        vmBEQException.loading = false;
                        vmBEQException.Messageunbindchanged = false;

                    growl.error("Error: Something went wrong, please contact administrator");
                    });
        }
        vmBEQException.Messageunbindchanged = false;
        vmBEQException.UNBind = function (Match) {
            $confirm({ text: 'Are you sure you want to UNBIND File Number ' + Match.FileNumber + ' ?' }, { size: 'sm' })
                .then(function () {
                    vmBEQException.Messageunbindchanged = true;
                    $http.post('ExceptionController/BEQUnBindOrder', Match)
                        .then(function (response) {
                            vmBEQException.show = false;
                            vmBEQException.PotentialMatches = [];
                            if (response.data == true) {
                                growl.success(" UNBIND File Number " + Match.FileNumber + " successful");
                            }
                            else {
                                growl.error(" UNBIND File Number " + Match.FileNumber + " not successful");
                            }
                        },
                            function (error) {
                                growl.error("Error: Something went wrong, please contact administrator");
                                vmBEQException.show = false;
                                mBEQException.Messageunbindchanged = false;
                            });

                });
        }

    }]);


angular.module('psBusinessException').controller('psBusinessExceptionRejectCtrl', ['$http', '$uibModalInstance', 'Exception', 'growl', '$uibModalStack',
    function psBusinessExceptionRejectCtrl($http, $uibModalInstance, Exception, growl, $uibModalStack) {
        var vmBEQException = this;
        vmBEQException.entity = angular.copy(Exception);
        vmBEQException.MessageRejectchanged = false;
        vmBEQException.RejectOrder = function () {
            vmBEQException.MessageRejectchanged = true;
            $http.post('ExceptionController/RejectOrder', vmBEQException.entity)
                .then(function (response) {
                    if (response.data == true) {


                        growl.success("External Reference Number " + vmBEQException.entity.ExternalRefNum + " was Rejected successfully ");
                        $uibModalStack.dismissAll("Exception was Rejected successfully");

                    }
                },
                    function (error) {
                        growl.error("There was an error in Rejecting this exception");
                        vmBEQException.MessageRejectchanged = false;
                    });

        };
    }]);


angular.module('psBusinessException').controller('psBusinessExceptionDeleteCtrl', ['$http', '$uibModalInstance', 'Exception', 'growl', '$uibModalStack',
    function psBusinessExceptionDeleteCtrl($http, $uibModalInstance, Exception, growl, $uibModalStack) {
        var vmBEQException = this;
        vmBEQException.entity = angular.copy(Exception);
        vmBEQException.MessageDeletechanged = false;
        vmBEQException.DeleteOrder = function () {
            vmBEQException.MessageDeletechanged = true;
            $http.post('ExceptionController/DeleteBEQOrder/' + vmBEQException.deleteNotes, vmBEQException.entity)
                .then(function (response) {
                    if (response.data == true) {
                        growl.success("External Reference Number " + vmBEQException.entity.ExternalRefNum + " was Deleted successfully ");
                        $uibModalStack.dismissAll("Exception was Deleted successfully");
                        vmBEQException.MessageDeletechanged = true;
                    }
                },
                    function (error) {
                        growl.error("There was an error in deleting this exception");
                        vmBEQException.MessageDeletechanged = false;
                    });

        };
    }]);


angular.module('psBusinessException').controller('psBusinessExceptionNewOrderCtrl', ['$http', '$uibModalInstance', 'Exception', 'growl', '$uibModalStack',
    function psBusinessExceptionNewOrderCtrl($http, $uibModalInstance, Exception, growl, $uibModalStack) {
        var vmBEQException = this;
        vmBEQException.entity = angular.copy(Exception);
        vmBEQException.CreateOrderChanged = false;
        vmBEQException.CreateOrder = function () {
            vmBEQException.CreateOrderChanged = true;
            $http.post('ExceptionController/BEQCreateOrder/' + encodeString(vmBEQException.NewOrderNotes), vmBEQException.entity)
                .then(function (response) {
                    if (response.data == true) {
                        growl.success("CreateOrder " + vmBEQException.entity.ExternalRefNum + " was created successfully");
                        $uibModalStack.dismissAll("Created order successfully");
                        vmBEQException.CreateOrderChanged = true;
                    }
                },
                    function (error) {
                        growl.error("There was an error in create order of this this exception");
                        vmBEQException.CreateOrderChanged = false;
                    });

        };
    }]);



angular.module('psBusinessException').controller('BEQMessageLogModuleMessagecntrl', ['$http', '$uibModalInstance', 'XmlContent',
    function BEQMessageLogModuleMessagecntrl($http, $uibModalInstance, XmlContent) {
        var vmBEQException = this;
        vmBEQException.XmlContent = XmlContent;

    }]);



// This function takes in a string and encodes special characters and returns the encoded string
function encodeString(str) {
    // Special chars that did NOT work and is now handled: & . # % * + \ : < > / ?
    if (str != undefined) {
        str = str.replace(/&/gi, "@@26");
        str = str.replace(/\./gi, "@@27");
        str = str.replace(/#/gi, "@@28");
        str = str.replace(/%/gi, "@@29");
        str = str.replace(/\*/gi, "@@30");
        str = str.replace(/\+/gi, "@@31");
        str = str.replace(/\\/gi, "@@32");
        str = str.replace(/:/gi, "@@33");
        str = str.replace(/</gi, "@@34");
        str = str.replace(/>/gi, "@@35");
        str = str.replace(/\//gi, "@@36");
        str = str.replace(/\?/gi, "@@37");
    }
   
    return str;
};