"use strict";

angular.module('psFastWorkFlowMappings').controller('psFastWorkFlowMappingsController', psFastWorkFlowMappingsController);
angular.module('psFastWorkFlowMappings').service('psFastWorkFlowMappingsRowEditor', psFastWorkFlowMappingsRowEditor);

psFastWorkFlowMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psFastWorkFlowMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies', 'growl', '$route', '$routeParams'];
function psFastWorkFlowMappingsController($scope, $rootScope, $http, $interval, $uibModal, psFastWorkFlowMappingsRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies, growl, $route, $routeParams) {
    var vfwflow = this;

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

    vfwflow.editNewFASTWorkFlowMap = psFastWorkFlowMappingsRowEditor.editNewFASTWorkFlowMap;
    vfwflow.addNewFASTWorkFlowMap = psFastWorkFlowMappingsRowEditor.addNewFASTWorkFlowMap;
    vfwflow.removegroupRow = psFastWorkFlowMappingsRowEditor.removegroupRow;

    vfwflow.showgrpDocuments = psFastWorkFlowMappingsRowEditor.showgrpDocuments;
    vfwflow.showgrpEvents = psFastWorkFlowMappingsRowEditor.showgrpEvents;

    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" tooltip-placement="bottom" uib-tooltip="Outbound Document" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'
    //  var groupdocevent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)"  class="fa fa-file-text" tooltip-placement="bottom" uib-tooltip="Outbound Document" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showgrpDocuments(row)"></a> <a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-bolt" tooltip-placement="bottom" uib-tooltip="Outbound Event" style="padding:1px 7px;text-align:center;cursor:pointer" ng-click="grid.appScope.showgrpEvents(row)"></a>  </div>'

    vfwflow.serviceFastWorkFlowGrid = {
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
        exporterCsvFilename: 'FastWorkFlowMap.csv',
        columnDefs: [
            { field: 'FASTWorkFlowMapId', name: 'ID', visible: false, displayName: 'ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'MessageType', name: 'Message ', displayName: 'Message Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ProcessEventId', name: 'ProcessEventId', visible:false, headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'TaskeventId', name: 'TaskeventId', visible:false, headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
             { field: 'ProcessEvent', name: 'ProcessEvent', displayName: 'Process Event ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Taskevent', name: 'Taskevent', displayName: 'Task Event ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'serviceName', name: 'Service', displayName: 'Service', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'FASTOWorkFlowMapDesc', name: 'Description', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Customer', name: 'Customer', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },

        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vfwflow.editNewFASTWorkFlowMap(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vfwflow.serviceFastWorkFlowGrid.gridApi = gridApi;
        }
    };

    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.vfwflow.serviceFastWorkFlowGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname === 'LVIS')
        $scope.vfwflow.serviceFastWorkFlowGrid.columnDefs[pos].visible = true;
    else
        $scope.vfwflow.serviceFastWorkFlowGrid.columnDefs[pos].visible = false;


    $http.get('Security/GetTenant')
        .then(function (response) {
            $scope.Tenant = response.data;
        });

    if ($rootScope.activityright) {
        $http.get('FASTWorkFlow/GetFASTWorkFlowMapping/')
                .success(function (response) {
                    vfwflow.serviceFastWorkFlowGrid.data = response;
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

    $scope.addNewFASTWorkFlowMap = function () {
        var newService = {
            "FASTWorkFlowMapId": 0,
            "MessageTypeId": "",
            "ProcessEventId": "",
            "TaskeventId": "",
            "FASTOWorkFlowMapDesc": "",
            "ProcessEvent": "",
            "Taskevent": "",
            "TenantId": "",
            "Tenant": "",
            "serviceId": "",
            "Customerid":""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vfwflow.addNewFASTWorkFlowMap($scope.vfwflow.serviceFastWorkFlowGrid, rowTmp);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vfwflow.serviceFastWorkFlowGrid.selectItem(index, false);
        $scope.vfwflow.serviceFastWorkFlowGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        var index = $scope.vfwflow.serviceFastWorkFlowGrid.data.indexOf(row.entity);
        $scope.vfwflow.serviceFastWorkFlowGrid.data.splice(index, 1);
    };
}

psFastWorkFlowMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psFastWorkFlowMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editNewFASTWorkFlowMap = editNewFASTWorkFlowMap;
    service.addNewFASTWorkFlowMap = addNewFASTWorkFlowMap;


    function editNewFASTWorkFlowMap(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastWorkFlowMappings/fastworkflow-Mapping-edit.html',
            controller: 'psFastWorkFlowMappingsRowEditCtrl',
            controllerAs: 'vfwflow',
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

    function addNewFASTWorkFlowMap(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastWorkFlowMappings/fastworkflow-Mapping-add.html',
            controller: 'psFastWorkFlowMappingsRowEditCtrl',
            controllerAs: 'vfwflow',
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


angular.module('psFastWorkFlowMappings').controller('psFastWorkFlowMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl','$confirm',
function psFastWorkFlowMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm) {

    var vfwflow = this;
    vfwflow.entity = angular.copy(row.entity);
    vfwflow.save = save;


    vfwflow.CustomerList = [];

    $http.get('Customers/GetTenantBasedCustomer')
    .success(function (data) {
        vfwflow.CustomerList = data;
    });

    if (vfwflow.entity.Tenant === '') {
        vfwflow.entity.Tenant = $scope.tenantname;
        vfwflow.entity.Tenatid = 0;
    }

    //start-Bind-Message List
    vfwflow.MessageTypeList = [];
    $http.get('FastTask/GetMessageType')
        .success(function (data) {
            vfwflow.MessageTypeList = data;            
        });


    //start-Bind-Message List
    vfwflow.ServiceTypeList = [];
    $http.get('InboundDocs/GetServices')
        .success(function (data) {
            vfwflow.ServiceTypeList = data;
        });



    //start-ProcessEvent List
    vfwflow.ProcesseventIdList = [];
    $http.get('FASTWorkFlow/BindProcessTaskEvent/' + vfwflow.entity.Tenatid)
        .success(function (data) {
            vfwflow.ProcesseventIdList = data;
        });

    //start-TaskEvent List
    vfwflow.TaskeventIdList = [];
    $http.get('FASTWorkFlow/BindTaskEvent/' + vfwflow.entity.Tenatid)
        .success(function (data) {
            vfwflow.TaskeventIdList = data;
        });
   
    //Send Data to Web APi for save/Delete
    function save() {
        if (vfwflow.entity.FASTWorkFlowMapId == '0') {
            row.entity = angular.extend(row.entity, vfwflow.entity);
           
            $http.post('FASTWorkFlow/AddFastWorkflowmap', row.entity)
                .success(function (data) {
                    //real ID come back from response after the save in DB
                    row.entity = data;
                    grid.data.push(row.entity);
                    if (data.length == 0) {
                        growl.error('FAST Process Trigger info record is duplicate and cannot be added');
                        return;
                    }
                    else {
                        growl.success("FAST Process Trigger info was added successfully");
                    }
                }).error(function (response) {

                    if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                        growl.error("Validation failed for one or more entities while Saving to DB");
                        return;
                    }
                    if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0) {
                        growl.error('FAST Process Trigger info record is a duplicate and cannot be added');
                        return;
                    }
                    growl.error(response.ExceptionMessage);
                    return;
                });
        }
        else {
            row.entity = angular.extend(row.entity, vfwflow.entity);

            $http.post('FASTWorkFlow/UpdateFastworkflowmap', row.entity)
           .success(function (data) {
               if (data == 0) {
                   growl.error('FAST Process Trigger info record is a duplicate and cannot be added');
                   return;
               }
               else {
                   row.entity = data;
                   grid.data = row.entity;                                                 
                   growl.success("FAST Process Trigger info record was updated successfully");
               }
               
           }).error(function (response) {
               if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                   growl.error("Validation failed for one or more entities while Saving to DB");
                   return;
               }
               if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0) {
                   growl.error('FAST Process Trigger info record is duplicate and cannot be added');
                   return;
               }

               growl.error(response.ExceptionMessage);
               return;
           });
        }
        $uibModalInstance.close(row.entity);
    }

    vfwflow.remove = remove;
    function remove()
    {
        if (row.entity.FASTWorkFlowMapId != '0') {
            row.entity = angular.extend(row.entity, vfwflow.entity);
            $http.post('FASTWorkFlow/Delete', row.entity.FASTWorkFlowMapId)
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
                        if (row.entity.FASTWorkFlowMapId != '0') {
                            row.entity = angular.extend(row.entity, vfwflow.entity);
                            $http.post('FASTWorkFlow/ConfirmDelete', row.entity.FASTWorkFlowMapId)
                           .success(function (data) {
                               if (data === 1) {
                                   var index = grid.appScope.vfwflow.serviceFastWorkFlowGrid.data.indexOf(row.entity);
                                   grid.appScope.vfwflow.serviceFastWorkFlowGrid.data.splice(index, 1);
                                   growl.success("FAST Process Trigger info record was deleted successfully");
                                   return;
                               }
                               else {
                                   growl.error("There was an error deleting FAST Process Trigger info record for: " + row.entity.FASTOWorkFlowMapDesc);
                               }
                           })
                        }
                    });
                }
            });
            $uibModalInstance.close(row.entity);
        }
    }
    
}]);


