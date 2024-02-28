"use strict";

angular.module('psFastTaskMappings').controller('psFastTaskMappingsController', psFastTaskMappingsController);
angular.module('psFastTaskMappings').service('psFastTaskMappingsRowEditor', psFastTaskMappingsRowEditor);


psFastTaskMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psFastTaskMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies', 'growl', '$route', '$routeParams'];
function psFastTaskMappingsController($scope, $rootScope, $http, $interval, $uibModal, psFastTaskMappingsRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies, growl, $route, $routeParams) {
    var vftask = this;

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

    //var ProviderOfficeMapRowCount = false;

    vftask.editNewFASTTaskMap = psFastTaskMappingsRowEditor.editNewFASTTaskMap;
    vftask.addNewFASTTaskMap = psFastTaskMappingsRowEditor.addNewFASTTaskMap;
    vftask.removegroupRow = psFastTaskMappingsRowEditor.removegroupRow;
    vftask.showgrpDocuments = psFastTaskMappingsRowEditor.showgrpDocuments;
    vftask.showgrpEvents = psFastTaskMappingsRowEditor.showgrpEvents;

    var IsInboundButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" >  {{ row.entity.IsInbound==false ? "NO" :"YES" }}</div>'
    //var Containsregioncode = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" > {{ row.entity.ContainsRegioncode==false ? "NO" :"YES" }}</div>'

    //var groupdocevent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)"  class="fa fa-file-text" tooltip-placement="bottom" uib-tooltip="Outbound Document" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showgrpDocuments(row)"></a> <a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-bolt" tooltip-placement="bottom" uib-tooltip="Outbound Event" style="padding:1px 7px;text-align:center;cursor:pointer" ng-click="grid.appScope.showgrpEvents(row)"></a>  </div>'
    //var IsInboundButtonld = '<div ng-if="row.entity.IsInbound == true"  class="ui-grid-cell-contents">Yes</div><div ng-if="row.entity.IsInbound == false" class="ui-grid-cell-contents">No</div>'
    //var Containsregioncodeold = '<div ng-if="row.entity.ContainsRegioncode == true"  class="ui-grid-cell-contents">Yes</div><div ng-if="row.entity.ContainsRegioncode == false" class="ui-grid-cell-contents">No</div>'

    vftask.serviceFastTaskGrid = {
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
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterCsvFilename: 'FastTaskMap.csv',
        columnDefs: [
            { field: 'FastTaskMapId', name: 'ID', visible: false, displayName: 'ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            //{ field: 'FastTaskMapName', name: 'Name ', displayName: 'Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'FastTaskMapDesc', name: 'FastTaskMapDesc', displayName: 'Task Description', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Typecode', name: 'Typecode', displayName: 'Task Status', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Region', name: 'Region', displayName: 'Region ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'MessageType', name: 'MessageType', displayName: 'Message Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'service', name: 'service', displayName: 'Service', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ApplicationName', name: 'External Application', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'CustomerName', name: 'CustomerName', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
            { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } }
        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vftask.editNewFASTTaskMap(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vftask.serviceFastTaskGrid.gridApi = gridApi;
        }
    };

    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.vftask.serviceFastTaskGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname === 'LVIS')
        $scope.vftask.serviceFastTaskGrid.columnDefs[pos].visible = true;
    else
        $scope.vftask.serviceFastTaskGrid.columnDefs[pos].visible = false;

    $http.get('Security/GetTenant')
    .then(function (response) {
        $scope.Tenant = response.data;
    });

    if ($rootScope.activityright) {
        $http.get('FastTask/GetFASTTaskMappingDetails/')
            .success(function (response) {
                vftask.serviceFastTaskGrid.data = response;
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

    $scope.changeGrouping = function () {
        $scope.gridApi.grouping.clearGrouping();
        $scope.gridApi.grouping.groupColumn('ID');
    };

    $scope.addNewFASTTaskMap = function () {
        var newService = {
            "ID": 0,
            "FastTaskMapName": "",
            "MessageTypeId": "",
            "TypecodeId": "",
            "RegionId": "",
            "serviceId": "",
            "FastTaskMapDesc": "",
            "TenantId": "",
            "Tenant": "",
            "ApplicationId": "",
            "CustomerId": ""

        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vftask.addNewFASTTaskMap($scope.vftask.serviceFastTaskGrid, rowTmp);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vftask.serviceFastTaskGrid.selectItem(index, false);
        $scope.vftask.serviceFastTaskGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        var index = $scope.vftask.serviceFastTaskGrid.data.indexOf(row.entity);
        $scope.vftask.serviceFastTaskGrid.data.splice(index, 1);
    };
}

psFastTaskMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psFastTaskMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editNewFASTTaskMap = editNewFASTTaskMap;
    service.addNewFASTTaskMap = addNewFASTTaskMap;

    function editNewFASTTaskMap(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastTaskMappings/fasttask-Mappings-edit.html',
            controller: 'psFastTaskMappingsRowEditCtrl',
            controllerAs: 'vftask',
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

    function addNewFASTTaskMap(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastTaskMappings/fasttask-Mappings-add.html',
            controller: 'psFastTaskMappingsRowEditCtrl',
            controllerAs: 'vftask',
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


angular.module('psFastTaskMappings').controller('psFastTaskMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', '$confirm', 'growl',
function psFastTaskMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, $confirm, growl) {

    var vftask = this;
    vftask.entity = angular.copy(row.entity);
    vftask.entity.IsInbound = row.entity.IsInbound;
    vftask.save = save;

    //bind Messagetype
    vftask.MessageTypeList = [];
    $http.get('FastTask/GetMessageType')
        .success(function (data) {
            vftask.MessageTypeList = data;
        });


    if (vftask.entity.Tenant === '') {
        vftask.entity.Tenant = $scope.tenantname;
    }

    //Bind TypecodeList
    vftask.TypecodeList = [];
    $http.get('FastTask/GetTypecode')
        .success(function (data) {
            vftask.TypecodeList = data;
        });

    //start-Bind-Region List
    vftask.RegionList = [];
    $http.get('Security/GetRegions/Fast')
        .success(function (data) {
            vftask.RegionList = data;
        });

    //start-Bind-Region List
    vftask.ServiceList = [];
    $http.get('InboundDocs/GetServices')
        .success(function (data) {
            vftask.ServiceList = data;
        });

    //Get Application List To Bind Dropdown
    vftask.ApplicationList = [];
    $http.get('Security/GetExternalApplications').success(function (data) {
        vftask.ApplicationList = data;
    });


    vftask.CustomerList = []
    $http.get('Customers/GetTenantBasedCustomer').success(function (response) {
        vftask.CustomerList = response;
        vftask.entity.CustomerId = vftask.entity.CustomerId;
    });

    //Send Data to Web APi for save/Delete
    function save() {
        if (vftask.entity.ID == '0') {
            row.entity = angular.extend(row.entity, vftask.entity);
            $http.post('FastTask/AddFastTaskMap', row.entity)
                .success(function (data) {
                    //real ID come back from response after the save in DB
                    row.entity = data;
                    grid.data.push(row.entity);
                    if (data.length === 0) {
                        growl.error('FAST Task Map info record is duplicate and cannot be added');
                        return;
                    }
                    else {
                        growl.success("FAST Task Map info record was added successfully");
                    }
                }).error(function (response) {

                    if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                        growl.error("Error adding FAST Task Map info record");
                        return;
                    }
                    if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0) {
                        growl.error('FAST Task Map info record is a duplicate and cannot be added');
                        return;
                    }
                    growl.error(response.ExceptionMessage);
                    return;
                });
        }
        else {
            row.entity = angular.extend(row.entity, vftask.entity);
            $http.post('FastTask/UpdateFastTaskMap', row.entity)
           .success(function (data) {
               if (data === 0) {
                   growl.error('FAST Task Map info record is duplicate and cannot be added');
                   return;
               }
               else {
                   row.entity = data;
                   grid.data = row.entity;
                   growl.success("FAST Task Map info was updated successfully");
               }
           }).error(function (response) {
               if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                   growl.error("Error updating FAST Task Map info record");
                   return;
               }
               if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0) {
                   growl.error('FAST Task Map info record is duplicate and cannot be added');
                   return;
               }

               growl.error(response.ExceptionMessage);
               return;
           });
        }
        $uibModalInstance.close(row.entity);
    }

    vftask.remove = remove;
    function remove() {
        if (vftask.entity.FastTaskMapId != '0') {
            row.entity = angular.extend(row.entity, vftask.entity);
            $http.post('FastTask/DeleteFastTask', row.entity.FastTaskMapId)
            .success(function (data) {
                if (data === 0) {
                    $confirm({ text: 'This record cannot be deleted because it is being used by another part of the Tower application.' }, {
                        size: 'sm', defaultLabels: {
                            title: ' Cannot Delete ',
                            ok: 'OK',
                            cancel: 'Cancel'
                        },
                        template: '<div class="widget-header"><h2><i class="fa fa-exclamation-triangle" aria-hidden="true" style="padding-left:0px;"></i>{{data.title}}</h2></div>' +
                              '<div class="widget-content">{{data.text}}</div>' +
                              '<div class="modal-footer">' +
                              '<button class="btn btn-default" ng-click="ok()">{{data.ok}}</button>' +
                              '</div>'
                    }).then(function () { });
                    // growl.error('Category record could not be Deleted');
                    return;
                }
                else {
                    $confirm({ text: 'Proceed to delete this selection ?' }, {
                        size: 'sm', defaultLabels: {
                            title: ' Confirm Delete ',
                            ok: 'Yes',
                            cancel: 'No'
                        },
                        template: '<div class="widget-header"><h2><i class="fa fa-question-circle" aria-hidden="true" style="padding-left:0px;"></i>{{data.title}}</h2></div>' +
                                   '<div class="widget-content">{{data.text}}</div>' +
                                   '<div class="modal-footer">' +
                                   '<button class="btn btn-success" ng-click="ok()">{{data.ok}}</button>' +
                                    '<button class="btn btn-danger" ng-click="cancel()">{{data.cancel}}</button>' +
                                   '</div>'
                    }).then(function () {
                        if (vftask.entity.FastTaskMapId != '0') {
                            row.entity = angular.extend(row.entity, vftask.entity);
                            $http.post('FastTask/ConfirmDeleteFastTask', row.entity.FastTaskMapId)
                           .success(function (data) {
                               if (data === 1) {
                                   var index = grid.appScope.vftask.serviceFastTaskGrid.data.indexOf(row.entity);
                                   grid.appScope.vftask.serviceFastTaskGrid.data.splice(index, 1);
                                   growl.success("FAST Task Map Info record was deleted successfully");
                               }
                               else {
                                   growl.error("There was an error deleting FAST Task Map Info record");
                               }
                           })
                        }
                    });
                    //growl.success("Category record was Deleted successfully");
                }
            });
            $uibModalInstance.close(row.entity);
        }
    }

}]);


