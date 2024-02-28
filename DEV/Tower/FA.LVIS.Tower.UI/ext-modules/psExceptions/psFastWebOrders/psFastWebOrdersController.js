/// <reference path="FastWeb-exception-edit.html" />
"use strict";

angular.module('psFastWebOrders').controller('psFastWebOrdersController', psFastWebOrdersController);
angular.module('psFastWebOrders').service('psFastWebOrdersRowEditor', psFastWebOrdersRowEditor);

psFastWebOrdersController.$inject = ['$route', '$routeParams', '$scope', '$rootScope', '$http', '$interval', '$uibModal', 'uiGridGroupingConstants', '$window', '$filter', '$cookies', '$confirm', 'UserInfo', 'growl', 'psFastWebOrdersRowEditor', 'uiGridConstants', 'modalProvider', 'Idle', 'Keepalive'];
function psFastWebOrdersController($route, $routeParams, $scope, $rootScope, $http, $interval, $uibModal, uiGridGroupingConstants, $window, $filter, $cookies, $confirm, UserInfo, growl, psFastWebOrdersRowEditor, uiGridConstants, modalProvider, Idle, Keepalive) {
    var vmFastWebOrders = this;
    vmFastWebOrders.BusyReload = false;
    $scope.$on("getUser", function (evt, response) {
        $rootScope.activityright = response.ActivityRight;
    });

    if (!$rootScope.activityright) {
        $rootScope.activityright = $cookies.get('activityright');
    }

    if ($rootScope.activityright !== 'Admin' && $rootScope.activityright !== 'SuperAdmin' && $rootScope.activityright !== 'User') {
        UserInfo.getUser().then(function (response) {
            $rootScope.$broadcast('getUser', response);
            $rootScope.activityright = response.ActivityRight;

        }, function (error) {

        });
    }
    else {
        if (!$rootScope.canmanagebeq) {
            UserInfo.getUser().then(function (response) {
                $rootScope.$broadcast('getUser', response);

            }, function (error) {

            });
        }
    }

    vmFastWebOrders.editReportRow = psFastWebOrdersRowEditor.editReportRow;

    //Search Functionality by type
    vmFastWebOrders.SearchTypeSelection = [{
        'title': '---Select---',
        'value': '0'
    },
    {
        'title': 'Borrower Name',
        'value': '3'
    },
    {
        'title': 'FASTWeb Order #',
        'value': '1'
    },
    {
        'title': 'Property Address',
        'value': '4'
    }
    ];

    var MessageEvent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"  title="orderDetail"><button class="btn btn-info btn-xs" ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" style="text-align:center;cursor:pointer" ng-click="grid.appScope.sampledetails(grid, row)">Order Detail</button></div>'

    var showMenuloginfo = true;
    if ($rootScope.tenantname != 'LVIS')
        showMenuloginfo = false;
    else
        showMenuloginfo = true;

    vmFastWebOrders.showMenuloginfo = showMenuloginfo;
    var showMenuloginfofastweborders = false;
    if (($rootScope.tenantname == 'LVIS' || $rootScope.tenantname == 'Air Traffic Control') && $rootScope.activityright == 'SuperAdmin')
        showMenuloginfofastweborders = true;
    else
        showMenuloginfofastweborders = false;
    vmFastWebOrders.showMenuloginfofastweborders = showMenuloginfofastweborders;

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

    vmFastWebOrders.serviceGrid = {
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
            { field: 'FASTWebOrderNumber', name: 'FASTWebOrder#', displayName: 'FASTWeb Order #', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'CustomerRefNumber', name: 'CustomerRef#', displayName: 'Customer Ref #', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'BorrowerName', name: 'BorrowerName', displayName: 'Borrower Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'PropertyAddress', name: 'PropertyAddress', displayName: 'Property Address', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true, cellTemplate: '<span>{{row.entity.PropertyAddressLine1}} {{row.entity.PropertyAddressLine2}}</span>' },
            { field: 'ServiceName', name: 'Service', displayName: 'Service', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'PortalOrderAlert', name: 'PortalOrderAlert', displayName: 'Portal Order Alert', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'OrderDate', name: 'OrderDate', displayName: 'Order Date', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Order Detail', enableColumnMenu: false, headerCellClass: 'grid-header', enableFiltering: false, enableCellEdit: false, groupingShowAggregationMenu: false, cellTemplate: MessageEvent },
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vmFastWebOrders.editReportRow(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",

        onRegisterApi: function (gridApi) {
            vmFastWebOrders.serviceGrid.gridApi = gridApi;
        },
    };

    $scope.addNewGroup = function () {
        var newService = {
            "UserId": "",
            "BUID": "",
            "First Name": "",
            "Last Name": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vmFastWebOrders.addNewGroup($scope.vmFastWebOrders.serviceGrid, rowTmp);
    };

    vmFastWebOrders.Reload = function Reload() {
        vmFastWebOrders.BusyReload = true;
        $http.post('FastWebOrdersController/FastWebOrders/')
            .then(function (response) {
                var data = [];
                data = response.data;
                vmFastWebOrders.serviceGrid.data = [];
                writeoutNode(data, 0, vmFastWebOrders.serviceGrid.data);
                vmFastWebOrders.BusyReload = false;
            });
    }

    $scope.sampledetails = function (grid, row) {
        var modalInstance = $uibModal.open({
            templateUrl: 'ext-modules/psExceptions/psFastWebOrders/FastWeb-exception-edit.html',
            controller: 'vmFastWebOrdersEdit',
            controllerAs: 'vmFastWebOrders',
            resolve: {
                Orderdetail: function () {
                    return row;
                }
            }
        });
    };
    vmFastWebOrders.addNewGroup = function addNewGroup() {

        $uibModal.open({
            templateUrl: 'ext-modules/psExceptions/psFastWebOrders/add-office.html',
            controller: 'vmFastWebOrdersAdd',
            size: 'md',
            controllerAs: 'vmFastWebOrders'
        });
    }
    vmFastWebOrders.serviceGrid.data = [];

    $scope.expandAll = function () {
        vmFastWebOrders.serviceGrid.gridApi.treeBase.expandAllRows();
    };

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.closemodal = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.toggleRow = function (rowNum) {
        vmFastWebOrders.serviceGrid.gridApi.treeBase.toggleRowTreeState($scope.gridApi.grid.renderContainers.body.visibleRowCache[rowNum]);
    };

    $scope.changeGrouping = function () {
        $scope.gridApi.grouping.clearGrouping();
        //$scope.gridApi.grouping.groupColumn('Reporting.ApplicationId=4');
    };

    $http.post('FastWebOrdersController/FastWebOrders/')
        .then(function (response) {
            var data = [];
            data = response.data;
            writeoutNode(data, 0, vmFastWebOrders.serviceGrid.data);

        });

    //Search Type and Text selection
    vmFastWebOrders.SearchTypeSelection.value = '0';
    vmFastWebOrders.SearchText = '';
    vmFastWebOrders.BusyRef = false;
    vmFastWebOrders.searchbyType =
        function searchbyType() {
            if (vmFastWebOrders.SearchText != '') {
                vmFastWebOrders.BusyRef = true;
                vmFastWebOrders.SearchTextData = [];
                vmFastWebOrders.SearchTextData.push(vmFastWebOrders.SearchText);
                $http.post('FastWebOrdersController/GetFastWebOrdersBySearchType/' + vmFastWebOrders.SearchTypeSelection.value, vmFastWebOrders.SearchTextData)
                    .then(function (response) {
                        vmFastWebOrders.serviceGrid.data = [];
                        var fastwebdata = [];
                        fastwebdata = response.data;
                        writeoutNode(fastwebdata, 0, vmFastWebOrders.serviceGrid.data);
                        vmFastWebOrders.BusyRef = false;
                    });
                
            }
        }

    function resubmit() {
        if (!vmFastWebOrders.Messagechanged) {
            $confirm({ text: 'Are you sure you want to Resubmit?' }, { size: 'sm' })
                .then(function () {
                    vmFastWebOrders.Messageresubmit = false;
                    PostResubmit();
                });
        }
        else {
            PostResubmit();
        }
    }

}

//Pop-up new window to display Row details with 'Forward To' list
psFastWebOrdersRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psFastWebOrdersRowEditor($http, $rootScope, $uibModal) {

    var service = {};


    service.editReportRow = editReportRow;

    function editReportRow(grid, row) {
        var instance = $uibModal.open({
            templateUrl: 'ext-modules/psExceptions/psFastWebOrders/FastWebOrderDetails.html',
            controller: 'vmFastWebOrdersEdit',
            controllerAs: 'vmFastWebOrders',
            size: 'lg',
            resolve: {
                Orderdetail: function () {
                    return row;
                }

            }
        });

        instance.result.then(function () {
            //Get triggers when modal is closed
        }, function () {
            var index = grid.appScope.vmFastWebOrders.serviceGrid.data.indexOf(row.entity);
            if (row.entity.Exceptionid == 0) {
                row.entity.children.forEach(function (childNode) {
                    if (childNode != undefined) {
                        var childindex = grid.appScope.vmFastWebOrders.serviceGrid.data.indexOf(childNode);
                        grid.appScope.vmFastWebOrders.serviceGrid.data.splice(childindex, 1);
                        row.entity.children.InvolveResolved = true;
                    }
                });
                row.entity.InvolveResolved = true;
                grid.appScope.vmFastWebOrders.serviceGrid.data.splice(index, 1);
            }
            else {
                row.entity.InvolveResolved = true;
                grid.appScope.vmFastWebOrders.serviceGrid.data.splice(index, 1);
            }
        });
    }

    return service;
}

angular.module('psFastWebOrders').controller('vmFastWebOrdersEdit', ['$http', '$uibModalInstance', 'Orderdetail', 'growl', '$uibModalStack',
    function vmFastWebOrdersEdit($http, $uibModalInstance, Orderdetail, growl, $uibModalStack) {
        var vmFastWebOrders = this;
        vmFastWebOrders.RowDetail = Orderdetail;
        vmFastWebOrders.entity = [];
        vmFastWebOrders.OfficeTypeSelection = [];

        //Get selected row details in Grid
        $http.post('FastWebOrdersController/GetFastWeborderDetail/' + vmFastWebOrders.RowDetail.entity.ServiceName, vmFastWebOrders.RowDetail.entity.FASTWebOrderNumber)
            .then(function (response) {
                vmFastWebOrders.entity = response.data;
            });



        //Get FastWeb office list and display in Dropdown
        $http.post('FastWebOrdersController/GetFastWebOffices', vmFastWebOrders.RowDetail.entity.FASTWebOrderNumber)
            .then(function (response) {
                vmFastWebOrders.OfficeTypeSelection = response.data;
            });

        vmFastWebOrders.resubmit = function () {
            $http.post('FastWebOrdersController/SubmitFastWebOffice/' + vmFastWebOrders.RowDetail.entity.FASTWebOrderNumber + '/' + vmFastWebOrders.RowDetail.entity.ServiceName, vmFastWebOrders.OfficeValue)
                .then(function (response) {

                    growl.success("Successfully submitted");
                    $uibModalInstance.close();

                }, function (error) {
                    growl.error("Error: Something went wrong, please contact administrator");
                });
        }
    }]);

angular.module('psFastWebOrders').controller('vmFastWebOrdersAdd', ['$http', '$uibModalInstance', 'growl', '$confirm',
    function vmFastWebOrdersAdd($http, $uibModalInstance, growl, $confirm) {
        
        var vmFastWebOrders = this;
        var newService = {
            "UserId": "",
            "BUID": "",
            "First Name": "",
            "Last Name": ""
        };
        
        vmFastWebOrders.entity = newService;
        $http.get('FastWebOrdersController/GetFastWebUser')
            .then(function (response) {
                vmFastWebOrders.UserId = response.data;
            });

        vmFastWebOrders.Submit = function Submit() {
            $http.post('FastWebOrdersController/AddForwardToOffice', vmFastWebOrders.entity)
                .then(function (response) {
                    if (response) {
                        growl.success("New office added successfully for the UserId:-" + vmFastWebOrders.UserId);
                    }
                    $uibModalInstance.close();
                },
                    function (error) {
                    growl.error("There was error while submitting office details");                   
                });             
        };
        
    }]);

