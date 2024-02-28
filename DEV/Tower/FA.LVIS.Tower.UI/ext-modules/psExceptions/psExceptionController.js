"use strict";

angular.module('psException').controller('psExceptionController', psExceptionController);
angular.module('psException').service('psExceptionRowEditor', psExceptionRowEditor);

psExceptionController.$inject = ['$route', '$routeParams', '$scope', '$rootScope', '$http', '$interval', '$uibModal', 'uiGridGroupingConstants', '$window', '$filter', '$confirm', 'UserInfo', '$location', '$cookies', 'growl', 'psExceptionRowEditor', 'uiGridConstants', 'modalProvider', '$timeout', '$templateCache', 'Idle', 'Keepalive',];
function psExceptionController($route, $routeParams, $scope, $rootScope, $http, $interval, $uibModal, uiGridGroupingConstants, $window, $filter, $confirm, UserInfo, $location, $cookies, growl, psExceptionRowEditor, uiGridConstants, modalProvider, $timeout, $templateCache, Idle, Keepalive) {
    var vmException = this;
    $scope.$on("getUser", function (evt, response) {
        $rootScope.activityright = response.ActivityRight;
        $rootScope.canmanageteq = response.CanManageTEQ;
        $scope.hasResubmitAccess = response.CanManageTEQ;
        $rootScope.hasResubmitAccess = response.CanManageTEQ;
        vmException.hasResubmitAccess = response.CanManageTEQ;
        vmException.tempResubmitAccess = response.CanManageTEQ;
    });

    if (!$rootScope.activityright) {
        $rootScope.activityright = $cookies.get('activityright');
    }

    if ($rootScope.activityright !== 'Admin' && $rootScope.activityright !== 'SuperAdmin' && $rootScope.activityright !== 'User') {
        UserInfo.getUser().then(function (response) {
            $rootScope.$broadcast('getUser', response);
            $rootScope.activityright = response.ActivityRight;
            $rootScope.canmanageteq = response.CanManageTEQ;
            $scope.hasResubmitAccess = response.CanManageTEQ;
            $rootScope.hasResubmitAccess = response.CanManageTEQ;
            vmException.hasResubmitAccess = response.CanManageTEQ;
            vmException.tempResubmitAccess = response.CanManageTEQ;
        }, function (error) {

        });
    } else {
        if (!$rootScope.canmanageteq) {
            UserInfo.getUser().then(function (response) {
                $rootScope.$broadcast('getUser', response);
                $rootScope.canmanageteq = response.CanManageTEQ;
                $scope.hasResubmitAccess = response.CanManageTEQ;
                $rootScope.hasResubmitAccess = response.CanManageTEQ;
                vmException.hasResubmitAccess = response.CanManageTEQ;
                vmException.tempResubmitAccess = response.CanManageTEQ;
            }, function (error) {

            });
        }
    }

    var hasResubmitAccess = $rootScope.canmanageteq;
    $scope.hasResubmitAccess = hasResubmitAccess;
    $rootScope.hasResubmitAccess = hasResubmitAccess;
    vmException.hasResubmitAccess = $rootScope.canmanageteq;
    var showMenuloginfo = true;
    if ($rootScope.tenantname != 'LVIS')
        showMenuloginfo = false;
    else
        showMenuloginfo = true;

    vmException.showMenuloginfo = showMenuloginfo;
    var showMenuloginfofastweborders = false;
    if (($rootScope.tenantname == 'LVIS' || $rootScope.tenantname == 'Air Traffic Control') && $rootScope.activityright == 'SuperAdmin')
        showMenuloginfofastweborders = true;
    else
        showMenuloginfofastweborders = false;
    vmException.showMenuloginfofastweborders = showMenuloginfofastweborders;
    vmException.Fromdate = $filter('date')(new Date(), 'MM/dd/yyyy');
    vmException.ThroughDate = $filter('date')(new Date(), 'MM/dd/yyyy');
    vmException.DateFilterSelection = [
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
    vmException.ReferencenoFilterSelection = [{
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

    $('.collapse').on('shown.bs.collapse', function () {
        $(this).parent().find(".glyphicon-plus").removeClass("glyphicon-plus").addClass("glyphicon-minus");
    }).on('hidden.bs.collapse', function () {
        $(this).parent().find(".glyphicon-minus").removeClass("glyphicon-minus").addClass("glyphicon-plus");
    });

    // On TEQ tab or bucket selection, this section will set the default "Date Filter" value
    vmException.ExceptionType = ($routeParams.ExceptionType || "");
    if (vmException.ExceptionType != "") {
        vmException.FilterSection = '24';
        //vmException.FilterSection = '90';  //Commented Because default Pull will be From Inception date
    }
    else { vmException.FilterSection = '24'; }

    vmException.Disabledate = true;
    vmException.Busy = false;

    //vmException.Typecodestatus = false;
    vmException.Typecodestatus = false;

    vmException.countRows = 0;

    vmException.editReportRow = psExceptionRowEditor.editReportRow;


    var CommentEvent = '<div ng-if="!col.grouping || col.grouping.groupPriority ===  || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"  title="{{row.entity.Comments}}"><a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-sticky-note" style="padding:5px 15px;text-align:center;"></a></div>'

    var MessageEvent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"  title="{{row.entity.Messages}}"><button class="btn btn-info btn-xs" ng-show="row.entity.MessageType && (row.treeNode.children && row.treeNode.children.length == 0)" style="text-align:center;cursor:pointer" ng-click="grid.appScope.sampledetails(grid, row)"> Message Log</button></div>'

    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer"></i></div>'
    vmException.serviceGrid = {
        enableColumnResize: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: false,
        enableRowHeaderSelection: true,
        multiSelect: true,
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
        columnDefs: [
            { field: 'Exceptionid', name: 'Exceptionid', visible: false, headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'ExceptionType', name: 'Exception Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'ExternalRefNum', name: 'ExternalRefNum', displayName: 'External Reference Number', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'MessageType', name: 'Message Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            {
                field: 'Status.Name', name: 'Status', filter: {
                    type: uiGridConstants.filter.SELECT,
                    selectOptions: [{ value: '', label: 'All' }, { value: 'New', label: 'New' }, { value: 'Active', label: 'Active' }, { value: 'Hold', label: 'Hold' }, { value: 'Resolved', label: 'Resolved' }, { value: 'Resubmitted', label: 'Resubmitted' }]
                }, headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true
            },
            { field: 'LastModifiedDate', name: 'LastModifiedDate', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'LastModifiedBy', name: 'LastModifiedBy', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Notes', name: 'Notes', headerCellClass: 'grid-header', visible: false, enableColumnMenu: false, enableFiltering: false, groupingShowAggregationMenu: false, cellTooltip: true, cellTemplate: CommentEvent },
            { field: 'Message Log', enableColumnMenu: false, headerCellClass: 'grid-header', enableFiltering: false, enableCellEdit: false, groupingShowAggregationMenu: false, cellTemplate: MessageEvent },
            { field: 'TypeCodeId', name: '', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: false, visible: false },
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vmException.editReportRow(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",

        onRegisterApi: function (gridApi) {
            vmException.serviceGrid.gridApi = gridApi;

            //Single row selection
            // To get count of selected tows for Enable/Disable Button Resubmit
            vmException.serviceGrid.gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                vmException.countRows = vmException.serviceGrid.gridApi.selection.getSelectedRows().length;

            });

            // Multiple row selections
            vmException.serviceGrid.gridApi.selection.on.rowSelectionChangedBatch($scope, function (row) {
                vmException.countRows = vmException.serviceGrid.gridApi.selection.getSelectedRows().length;

            });
        },
    };

    vmException.ExceptionStatus = 0;
    vmException.search = search;
    // pdl search();
    vmException.changeSelect = changeSelect;
    function changeSelect(item) {

        if (item == 1)
            vmException.Disabledate = false;
        else
            vmException.Disabledate = true;
    }
    vmException.ValidateDate = ValidateDate;
    vmException.ValidateError = false;
    vmException.Conditionalsearchdetails = [];

    function ValidateDate() {
        var StartDate = new Date(vmException.Fromdate);
        var EndDate = new Date(vmException.ThroughDate);
        vmException.ValidateError = false;
        if (EndDate < StartDate)
            vmException.ValidateError = true;
    }

    function search() {
        vmException.hasResubmitAccess = false;
        vmException.EnableSearchFilter = false;
        if (vmException.gmessage != undefined)
            vmException.gmessage.destroy();

        //Include/Exclude Resolved Status Excecption
        if (vmException.IncludeResolve) {
            vmException.Typecodestatus = true;
        }
        else {
            vmException.Typecodestatus = false;
        }

        if (!vmException.Fromdate || !vmException.ThroughDate) {
            vmException.gmessage = growl.error("Please enter a valid Start/End date");
            return;
        }

        ValidateDate();

        if (vmException.ValidateError) {
            vmException.gmessage = growl.error("End date cannot be earlier than the Start date");
            return;
        }

        var Details = {
            Fromdate: vmException.Fromdate.toString(),
            ThroughDate: vmException.ThroughDate.toString(),
            statusId: 0,
            Typecodestatus: vmException.Typecodestatus
        }

        vmException.Busy = true;

        if (vmException.ExceptionType != "") {
            $http.post('ExceptionController/GetTEQExceptionsbyType/' + vmException.ExceptionType + '/' + vmException.FilterSection, Details)
                .then(function (response) {

                    if (!vmException.showrefnum) {
                        vmException.serviceGrid.data = response.data;
                        vmException.EnableSearchFilter = true;                        
                    }
                    vmException.Busy = false;
                    Idle.watch();
                },

                    function (error) {
                        growl.error("There was an error Loading exceptions");
                        vmException.Busy = false;
                    });
        }
        else {
            if (vmException.FilterSection == "1") {
                $http.post('ExceptionController/GetTEQExceptions', Details)
                    .then(function (response) {
                        if (!vmException.showrefnum) {
                            vmException.serviceGrid.data = response.data;
                            vmException.EnableSearchFilter = true;                            
                        }
                        vmException.Busy = false;
                        Idle.watch();
                    },

                        function (error) {
                            growl.error("There was an error Loading exceptions");
                            vmException.Busy = false;
                        });
            }
            else {
                $http.get('ExceptionController/GetTEQExceptionsbyFilter/' + vmException.FilterSection + '/' + vmException.Typecodestatus)
                    .then(function (response) {
                        if (!vmException.showrefnum) {
                            vmException.serviceGrid.data = response.data;
                            vmException.EnableSearchFilter = true;                            
                        }
                        vmException.Busy = false;
                        Idle.watch();
                    },

                        function (error) {
                            growl.error("There was an error Loading exceptions");
                            vmException.Busy = false;
                        });
            }
        }
    }



    vmException.OpenPopUp = OpenPopUp;

    function OpenPopUp() {
        $uibModal.open({
            templateUrl: 'ext-modules/psExceptions/CleanupTEQPopUp.html',
            controller: 'psExceptionFilter',
            controllerAs: 'vmException',
            resolve: {
                grid: function () {
                    return vmException.serviceGrid;
                }
            }
        }).result.then(function (searchDetails) {
            vmException.Conditionalsearchdetails = searchDetails;
        });
    }

    //Filter By Reference Num
    vmException.FilterReferenceNoSection = '0';
    vmException.ReferenceNo = '';
    vmException.BusyRef = false;
    vmException.changerefSelect = changerefSelect;
    function changerefSelect(item) {
        if (item == 0)
            vmException.DisableReferenceNo = true;
        else
            vmException.DisableReferenceNo = false;
    }
    vmException.searchbyReferenceNo = searchbyReferenceNo;
    function searchbyReferenceNo() {
        if (vmException.ReferenceNo != '') {
            var FDetails = {
                ReferenceNoType: vmException.FilterReferenceNoSection,
                ReferenceNo: vmException.ReferenceNo
            }
            vmException.BusyRef = true;
            $http.post('ExceptionController/GetTEQExceptionByReferenceNum', FDetails)
                .then(function (response) {
                    vmException.serviceGrid.data = response.data;
                    vmException.BusyRef = false;
                }
                    ,

                    function (error) {
                        growl.error("There was an error Loading exceptions");
                        vmException.BusyRef = false;
                    });
        }
    }
    //Filter by Reference Num

    //start-Bulk Resumbit   
    vmException.BulkResubmit = BulkResubmit;
    function BulkResubmit() {
        $uibModal.open({
            templateUrl: 'ext-modules/psExceptions/TEQBulkException-Resubmit.html',
            controller: 'psExceptionBulkResubmitCtrl',
            size: 'sm',
            controllerAs: 'vmException',
            backdrop: 'static',
            keyboard: false,
            resolve: {
                ExceptionRows: function () {
                    vmException.rows = vmException.serviceGrid.gridApi.selection.getSelectedRows();
                    return vmException.rows;
                }
            }
        }).result.finally(function () {  
            //console.log('modal has closed'); 
            //Reload Updated Exception details.            
            vmException.serviceGrid.gridApi.selection.clearSelectedRows();
            search();
        });
    }


    vmException.BulkResolve = BulkResolve;
    function BulkResolve() {
        $http.get('ExceptionController/GetExceptionList')
            .then(function (response) {
                vmException.ExceptionTypeList = response.data;                
            });
        $http.get('ExceptionController/GetStatusList')
            .then(function (response) {
                vmException.StatusList = response.data;                
            });
        $http.get('ExceptionController/GetMessageTypeList')
            .then(function (response) {
                vmException.MessageTypeList = response.data;                
            });
        $uibModal.open({
            templateUrl: 'ext-modules/psExceptions/TEQBulkException-Resolve.html',
            controller: 'psExceptionBulkResolveCtrl',
            size: 'sm',
            controllerAs: 'vmException',
            backdrop: 'static',
            keyboard: false,
            resolve: {
                ExceptionRows: function () {
                    vmException.rows = vmException.serviceGrid.gridApi.selection.getSelectedRows();
                    return vmException.rows;
                }
            }
        }).result.finally(function () {
            vmException.serviceGrid.gridApi.selection.clearSelectedRows();
            vmException.ShowSpinner = true;
            if (vmException.Conditionalsearchdetails.length != 0) {
                SearchFilter();
            }
            else {
                search();
            }
            vmException.ShowSpinner = false;
        });
    }

    function SearchFilter() {
            var Details = {
                FromDate: vmException.Conditionalsearchdetails.StartDate,
                ThroughDate: vmException.Conditionalsearchdetails.EndDate,
                search: vmException.Conditionalsearchdetails.Description
            }        
        vmException.loading = true;
        $http.post('ExceptionController/GetTEQExceptionsbyCondition/' + vmException.Conditionalsearchdetails.Name +'/'+ vmException.Conditionalsearchdetails.Status +'/'+ vmException.Conditionalsearchdetails.MessageType, Details)
            .then(function (response) {
                {
                    vmException.serviceGrid.data = response.data;
                }
            });
        vmException.loading = false;
        vmException.Busy = false;
        $uibModalInstance.close();
    }

    $scope.expandAll = function () {
        vmException.serviceGrid.gridApi.treeBase.expandAllRows();
    };

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.closemodal = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.toggleRow = function (rowNum) {
        vmException.serviceGrid.gridApi.treeBase.toggleRowTreeState($scope.gridApi.grid.renderContainers.body.visibleRowCache[rowNum]);
    };

    $scope.sampledetails = function (grid, row) {
        modalProvider.openPopupModal(row.entity.ServiceRequestId);
    };


    vmException.showrefnum = false;
    vmException.showdates = true;
    vmException.isActive = false;
    $scope.mynumStyle = { color: '' };
    $scope.mydtStyle = { color: '#007acc' };
    $scope.ShowHide = function (item) {
        if (item == 'showdates') {
            vmException.showdates = true;
            vmException.showrefnum = false;
            vmException.isActive = !vmException.isActive;
            $scope.mynumStyle = { color: '' };
            $scope.mydtStyle = { color: '#007acc' };
        }
        else if (item == 'showrefnum') {
            vmException.showrefnum = true;
            vmException.showdates = false;
            vmException.isActive = !vmException.isActive;
            $scope.mydtStyle = { color: '' };
            $scope.mynumStyle = { color: '#007acc' };
        }
    }

    var pos = vmException.serviceGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname === 'LVIS')
        vmException.serviceGrid.columnDefs[pos].visible = true;
    else
        vmException.serviceGrid.columnDefs[pos].visible = false;

    Idle.watch();

    $scope.$on('IdleStart', function () {
        ////Implementation to notify user that in few mins the permitted Idle time will end
    });

    $scope.$on('IdleEnd', function () {
        ////Restarting the Idle timer
    });

    $scope.$on('IdleTimeout', function () {
        ////Timeout for the Idle User

        search();
    });
}

psExceptionRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psExceptionRowEditor($http, $rootScope, $uibModal) {
    var service = {};
    service.editReportRow = editReportRow;
    function editReportRow(grid, row) {

        var instance = $uibModal.open({
            templateUrl: 'ext-modules/psExceptions/technical-exception-edit.html',
            controller: 'psExceptionRowEditCtrl',
            controllerAs: 'vmException',
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
        }, function () {
            var index = grid.appScope.vmException.serviceGrid.data.indexOf(row.entity);
            grid.appScope.vmException.serviceGrid.data.splice(index, 1);
            rebindGridData(grid);
        });
    }

    function rebindGridData(grid) {
        var filterSection = 7;
        var typecodestatus = false;
        $http.get('ExceptionController/GetTEQExceptionsbyFilter/' + filterSection + '/' + typecodestatus)
            .then(function (response) {
                grid.appScope.vmException.serviceGrid.data = response.data;
            });
    }

    return service;
}

angular.module('psException').controller('psExceptionRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', '$rootScope', 'growl', '$confirm', '$uibModal', 'Idle', 'Keepalive',
    function psExceptionRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, $rootScope, growl, $confirm, $uibModal, Idle, Keepalive) {
        var vmException = this;
        vmException.entity = angular.copy(row.entity);
        vmException.save = save;
        vmException.resubmit = resubmit;
        vmException.Reject = Reject;
        vmException.enablesave = true;
        vmException.Resubmit = false;
        //For Bulk Resubmit TEQ Exception

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

        vmException.setFlag = function (item) {

            vmException.enablesave = item;
        }
        vmException.setFlag1 = function (item) {

            vmException.Resubmit = item;
        }

        $http.get('ExceptionController/GetMessageContent/' + vmException.entity.DocumentObjectid)
            .then(function (response) {
                vmException.entity.MessageContent = response.data;
            });

        $http.get('ExceptionController/GetExceptionNotes/' + vmException.entity.Exceptionid)
            .then(function (response) {
                vmException.entity.Comments = response.data;
            });

        function Reject() {
            $uibModal.open({
                templateUrl: 'ext-modules/psExceptions/TEQException-Reject.html',
                controller: 'psExceptionRejectCtrl',
                controllerAs: 'vmException',
                resolve: {
                    Exception: function () {
                        return vmException.entity;
                    }
                }
            });
        }

        $http.get('ExceptionController/GetExceptionStatus')
            .then(function (response) {
                vmException.StatusList = response.data;
            });
        var getIndexIfObjWithOwnAttr = function (array, attr, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                    return i;
                }
            }
            return -1;
        }
        vmException.Messagesaved = false;

        function save() {

            var index = getIndexIfObjWithOwnAttr(vmException.StatusList, "ID", vmException.entity.Status.ID);
            if (index > -1)
                vmException.entity.Status = vmException.StatusList[index];

            if (vmException.entity.Notes != "") {
                vmException.Messagesaved = true;
                $http.post('ExceptionController/SaveExceptionComments', vmException.entity)
                    .then(function (response) {
                        row.entity = response.data;
                        row.entity.Comments = response.data.Comments;
                        vmException.entity.Comments = response.data.Comments;
                        vmException.entity.Notes = "";
                        grid.data = response.data;

                        vmException.enablesave = false;
                        growl.success("Exception information was saved successfully");
                        vmException.Messagechanged = true;
                        $uibModalInstance.close(row.entity);
                    },
                        function (error) {
                            growl.error("Exception information comments could not be updated");
                            vmException.enablesave = true;
                            vmException.Messagesaved = true;
                        });
            }
        }

        var growlErrorMessage = null;

        function PostResubmit() {
            vmException.Messageresubmit = true;
            $http.post('ExceptionController/ResubmitException', vmException.entity)
                .then(function (response) {
                    row.entity = response.data;
                    row.entity.Comments = response.data.Comments;
                    vmException.entity.Comments = response.data.Comments;
                    vmException.entity = row.entity;
                    vmException.entity.Notes = "";
                    grid.data = response.data;
                    vmException.Resubmit = false;

                    console.log(response);
                    // pdl if status not set resolved then there was a issue 
                    if (response.data.Status.Name == 'Active') {
                        growlErrorMessage = growl.error("There was an error in resubmitting this exception - review Exception Information, Notes/History section for more information");
                    }
                    else {
                        if (growlErrorMessage != null)
                            growlErrorMessage.destroy();
                        growl.success("Exception was resubmitted successfully");
                        $uibModalInstance.close(row.entity);
                    }
                },
                    function (error) {
                        growl.error("There was an error in resubmitting this exception");
                        vmException.Resubmit = true;
                        vmException.Messageresubmit = false;
                    });
        }

        function resubmit() {
            if (!vmException.Messagechanged) {
                $confirm({ text: 'Are you sure you want to Resubmit?' }, { size: 'sm' })
                    .then(function () {
                        vmException.Messageresubmit = false;
                        PostResubmit();
                    });
            }
            else {
                PostResubmit();
            }

            //vmException.Messagechanged = false;
        }

    }]);

angular.module('psException').controller('psExceptionRejectCtrl', ['$http', '$uibModalInstance', 'Exception', 'growl', '$uibModalStack',
    function psExceptionRejectCtrl($http, $uibModalInstance, Exception, growl, $uibModalStack) {
        var vmException = this;
        vmException.entity = angular.copy(Exception);
        vmException.Isreject = false;
        vmException.RejectOrder = function () {
            vmException.Isreject = true;
            $http.post('ExceptionController/TEQRejectOrder/' + encodeString(vmException.RejectNotes), vmException.entity)
                .then(function (response) {
                    if (response.data == true) {
                        growl.success("External Reference Number " + vmException.entity.ExternalRefNum + " was Rejected successfully. ");
                        $uibModalStack.dismissAll("Exception was Rejected successfully");
                        vmException.Isreject = true;
                    }
                },
                    function (error) {
                        growl.error("There was an error in Rejecting this exception");
                        vmException.Isreject = false;
                    });
        };
    }]);

//Bulk REsubmit
angular.module('psException').controller('psExceptionBulkResubmitCtrl', ['$http', '$interval', '$scope', '$timeout', '$uibModalInstance', 'growl', '$uibModalStack', 'ExceptionRows', '$rootScope',
    function psExceptionBulkResubmitCtrl($http, $interval, $scope, $timeout, $uibModalInstance, growl, $uibModalStack, ExceptionRows, $rootScope) {
        var vmException = this;
        vmException.loading = false;
        vmException.Isubmitted = false;
        vmException.Isdispaly = false;
        vmException.entity = angular.copy(ExceptionRows);
        vmException.entity.TotalResubmitCount = 0;
        vmException.entity.SuccessResubmitCount = 0;
        vmException.entity.UnSuccessResubmitCount = 0;
        vmException.Submit = Submit;
        vmException.entity.TotalResubmitCount = vmException.entity.length;
        function Submit() {
            vmException.loading = true;
            vmException.Isubmitted = true;
            $scope.Currentvalue = vmException.entity.length;
            $http.post('ExceptionController/BulkResubmitException', vmException.entity)
                .then(function (response) {
                    vmException.loading = true;
                    vmException.Isdispaly = true;
                    if (response.data != null) {
                        vmException.entity.TotalResubmitCount = response.data.m_Item1.TotalResubmitCount;
                        vmException.entity.SuccessResubmitCount = response.data.m_Item1.SuccessResubmitCount;
                        vmException.entity.UnSuccessResubmitCount = response.data.m_Item1.UnSuccessResubmitCount;
                    }
                    vmException.loading = false;
                    vmException.Isubmitted = true;
                },
                    function (error) {
                        growl.error("There was an error in Submitting exception");
                        vmException.Isubmitted = true;
                        vmException.loading = false;
                    });
        }
    }]);

angular.module('psException').controller('psExceptionBulkResolveCtrl', ['$http', '$interval', '$scope', '$timeout', '$uibModalInstance', 'growl', '$uibModalStack', 'ExceptionRows', '$rootScope',
    function psExceptionBulkResolveCtrl($http,$interval, $scope, $timeout, $uibModalInstance, growl, $uibModalStack, ExceptionRows, $rootScope) {
        var vmException = this;
        vmException.loading = false;
        vmException.Isubmitted = false;
        vmException.Isdispaly = false;
        vmException.entity = angular.copy(ExceptionRows);
        vmException.entity.TotalResubmitCount = 0;
        vmException.entity.SuccessResubmitCount = 0;
        vmException.entity.UnSuccessResubmitCount = 0;
        vmException.SubmitResolve = SubmitResolve;
        vmException.entity.TotalResubmitCount = vmException.entity.length;
        $http.get('ExceptionController/GetStatusList')
            .then(function (response) {
                vmException.PopUpStatusList = response.data;
            });
        function SubmitResolve() {
            vmException.loading = true;
            vmException.Isubmitted = true;
            $scope.Currentvalue = vmException.entity.length;
            $http.post('ExceptionController/BulkResolveException/' + vmException.PopUpStatusID, vmException.entity)
                .then(function (response) {
                    vmException.loading = true;
                    vmException.Isdispaly = true;
                    if (response.data != null) {
                        vmException.entity.TotalResubmitCount = response.data.m_Item1.TotalResubmitCount;
                        vmException.entity.SuccessResubmitCount = response.data.m_Item1.SuccessResubmitCount;
                        vmException.entity.UnSuccessResubmitCount = response.data.m_Item1.UnSuccessResubmitCount;
                    }

                    vmException.loading = false;
                    vmException.Isubmitted = true;
                },
                function (error) {
                    growl.error("There was an error in updating exception");
                    vmException.Isdispaly = true;
                    vmException.Isubmitted = true;
                    vmException.loading = false;
                });
        }

    }]);



angular.module('psException').controller('psExceptionFilter', ['$http', '$uibModalInstance', 'grid','$window', '$scope', '$rootScope', 'growl',
    function psExceptionFilter($http, $uibModalInstance, grid,$window,$scope,$rootscope, growl) {
        var vmException = this;
        vmException.ValidateErrorDate = false;
        $http.get('ExceptionController/GetExceptionList')
            .then(function (response) {
                vmException.ExceptionTypeList = response.data;
               
            });
        $http.get('ExceptionController/GetStatusList')
            .then(function (response) {
                vmException.StatusList = response.data;
              
            });
        $http.get('ExceptionController/GetMessageTypeList')
            .then(function (response) {
                vmException.MessageTypeList = response.data;
               
            });
        function ValidateDate() {
            var StartDate = new Date(vmException.FromDate);
            var EndDate = new Date(vmException.ToDate);
            vmException.ValidateErrorDate = false;
            if (EndDate < StartDate)
                vmException.ValidateErrorDate = true;
        }
        vmException.SearchFilter = SearchFilter;
        function SearchFilter() {
            if (vmException.FromDate != undefined && vmException.ToDate != undefined) {
                ValidateDate();
            }
            if (vmException.ValidateErrorDate) {               
                return;
            }
            if (vmException.ExceptionDescription == undefined) {
                vmException.ExceptionDescription = "";
            }
            if (vmException.FromDate == undefined) {
                vmException.FromDate = null;
            }
            if (vmException.ToDate == undefined) {
                vmException.ToDate = null;
            }
            if (vmException.ExceptionType == undefined) {
                vmException.ExceptionType = "All";
            }
            if (vmException.Status == undefined) {
                vmException.Status = "0";
            }
            if (vmException.MessageType == undefined) {
                vmException.MessageType = "0";
            }
            var Details = {
                FromDate: vmException.FromDate,
                ThroughDate: vmException.ToDate,
                search: vmException.ExceptionDescription
            }
            vmException.loading = true;
            vmException.Busy = true;            
            $http.post('ExceptionController/GetTEQExceptionsbyCondition/' + vmException.ExceptionType + '/' + vmException.Status + '/' + vmException.MessageType, Details)
                .then(function (response) {
                    {
                        grid.data = response.data;                        
                        var SearchDetails = {
                            Name: vmException.ExceptionType,
                            StartDate: vmException.FromDate,
                            EndDate: vmException.ToDate,
                            Description: vmException.ExceptionDescription,
                            MessageType: vmException.MessageType,
                            Status: vmException.Status
                        }
                        vmException.loading = false;
                        vmException.Busy = false;
                        $uibModalInstance.close(SearchDetails);
                    }
                });
        }
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





