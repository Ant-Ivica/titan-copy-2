"use strict";


angular.module('psCategoryMappings').controller('psCategoryMappingsController', psCategoryMappingsController);
angular.module('psCategoryMappings').service('psCategoryMappingsRowEditor', psCategoryMappingsRowEditor);
angular.module('psCategoryMappings').service('psCategoryMappingsApiUri', psCategoryMappingsApiUri);

psCategoryMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psCategoryMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies', 'growl'];
function psCategoryMappingsController($scope, $rootScope, $http, $interval, $uibModal, psCategoryMappingsRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies, growl) {
    var vgmap = this;

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

    vgmap.editgroup = psCategoryMappingsRowEditor.editgroup;
    vgmap.addNewGroup = psCategoryMappingsRowEditor.addNewGroup;
    vgmap.removegroupRow = psCategoryMappingsRowEditor.removegroupRow;

    vgmap.showgrpDocuments = psCategoryMappingsRowEditor.showgrpDocuments;
    vgmap.showgrpEvents = psCategoryMappingsRowEditor.showgrpEvents;

    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" tooltip-placement="bottom" uib-tooltip="Outbound Document" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'
    var groupdocevent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents">' +
        '<a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)"  class="fa fa-file-text" tooltip-placement="bottom" uib-tooltip="Outbound Document" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showgrpDocuments(row)"></a> ' +
        '<a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)"  class="fa fa-newspaper-o" tooltip-placement="bottom" uib-tooltip="Subscription(s)" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showSubscriptions(row)"></a></div>'
    //var DisplayColumn= '<div ng-show="row.entity.Tenant == LVIS" visible=false>'

    vgmap.serviceGroupGrid = {
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
        exporterCsvFilename: 'Events.csv',
        columnDefs: [
            { field: 'CategoryName', name: 'Category Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
            { field: 'ApplicationName', name: 'Application', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
            { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
            { field: 'TenantId', name: 'TenantId', visible:false, headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
            { field: 'groupdocevent', name: '  ', headerCellClass: 'grid-header', enableColumnMenu: false, enableFiltering: false, groupingShowAggregationMenu: false, cellTemplate: groupdocevent }
        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vgmap.editgroup(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vgmap.serviceGroupGrid.gridApi = gridApi;
        }
    };

    if ($rootScope.activityright) {
        $http.get('Categories/GetCategories/').success(function (response) {
            vgmap.serviceGroupGrid.data = response;
        });
    }

    $http.get('Security/GetTenant')
   .then(function (response) {
       $scope.Tenant = response.data;
   });

    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.vgmap.serviceGroupGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname === 'LVIS')
        $scope.vgmap.serviceGroupGrid.columnDefs[pos].visible = true;
    else
        $scope.vgmap.serviceGroupGrid.columnDefs[pos].visible = false;

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
        $scope.gridApi.grouping.groupColumn('EventId');
        //$scope.gridApi.grouping.aggregateColumn('state', uiGridGroupingConstants.aggregation.COUNT);
    };

    $scope.addNewGroup = function () {
        var newService = {
            "CategoryId": "0",
            "CategoryName": "",
            "Tenant": "",
            "TenantId": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vgmap.addNewGroup($scope.vgmap.serviceGroupGrid, rowTmp);
    };
    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vgmap.serviceGroupGrid.selectItem(index, false);
        $scope.vgmap.serviceGroupGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        var index = $scope.vgmap.serviceGroupGrid.data.indexOf(row.entity);
        $scope.vgmap.serviceGroupGrid.data.splice(index, 1);
    };

    $scope.showgrpDocuments = function (row) {
        $window.location = "#/outdocmappings/" + row.entity.CategoryName + ":" + row.entity.CategoryId + ":" + row.entity.Applicationid + "/true";
    };

    $scope.showgrpEvents = function (row) {
        $window.location = "#/outeventmappings/" + row.entity.Name + ":";
    };

    $scope.showSubscriptions = function (row) {
        $window.location = "#/subscription/" + row.entity.CategoryName + ":" + row.entity.CategoryId + ":" + row.entity.Applicationid + ":" + row.entity.TenantId + ":" + row.entity.Tenant + "/true";
    };
}

psCategoryMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psCategoryMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editgroup = editgroup;
    service.addNewGroup = addNewGroup;

    function editgroup(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psCategoryMappings/Category-mappings-edit.html',
            controller: 'psCategoryMappingsRowEditCtrl',
            controllerAs: 'vgmap',
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

    function addNewGroup(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psCategoryMappings/Category-mappings-add.html',
            controller: 'psCategoryMappingsRowEditCtrl',
            controllerAs: 'vgmap',
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


angular.module('psCategoryMappings').controller('psCategoryMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', '$confirm', 'growl', 'psCategoryMappingsApiUri',
function psCategoryMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, $confirm, growl, psCategoryMappingsApiUri) {

    var vgmap = this;
    vgmap.entity = angular.copy(row.entity);
    vgmap.save = save;

    if (vgmap.entity.Tenant === '') {
        vgmap.entity.Tenant = $scope.tenantname;
    }

    function save() {
        if (vgmap.entity.CategoryId === '0') {
            row.entity = angular.extend(row.entity, vgmap.entity);

            $http.post(psCategoryMappingsApiUri.AddCategory, row.entity)
                .success(function (data) {

                    //real ID come back from response after the save in DB
                    row.entity = data;
                    grid.data.push(row.entity);

                    if (data.length === 0) {
                        growl.error('Category record is a duplicate and cannot be added');
                        return;
                    }
                    else {
                        growl.success("Category record was added successfully");
                    }
                }).error(function (response) {

                    if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                        growl.error("Category record could not be added");
                        return;
                    }
                    if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0
                        || response.InnerException.InnerException.ExceptionMessage.indexOf("Cannot insert duplicate key row in object") >= 0) {
                        growl.error('Category record is a duplicate and cannot be added');
                        return;
                    }

                    growl.error(response.InnerException.InnerException.ExceptionMessage);
                    return;
                });
        }
        else {

            vgmap.entity = angular.extend(row.entity, vgmap.entity);
            $http.post(psCategoryMappingsApiUri.UpdateCategory, row.entity)
          .success(function (data) {
              if (data === 0) {
                  growl.error('Category record could not be updated');
                  return;
              }
              else {
                  growl.success("Category record was updated successfully");
              }
          }).error(function (response) {
              if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                  growl.error("Category record could not be updated");
                  return;
              }
              if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0
                  || response.InnerException.InnerException.ExceptionMessage.indexOf("Cannot insert duplicate key row in object") >= 0) {
                  growl.error('Category record is a duplicate and cannot be added');
                  return;
              }
              growl.error(response.InnerException.InnerException.ExceptionMessage);
              return;
          });
        }
        $uibModalInstance.close(row.entity);
    }

    vgmap.remove = remove;
    function remove() {
        if (row.entity.id !== '0') {
            row.entity = angular.extend(row.entity, vgmap.entity);
            $http.post('Categories/DeleteCategory', row.entity.CategoryId)
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
                    $confirm({ text: 'Proceed to delete this selection?' }, {
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
                        if (row.entity.id !== '0') {
                            row.entity = angular.extend(row.entity, vgmap.entity);
                            $http.post('Categories/ConfirmDeleteCategory', row.entity.CategoryId)
                           .success(function (data) {
                               if (data === 1) {
                                   var index = grid.appScope.vgmap.serviceGroupGrid.data.indexOf(row.entity);
                                   grid.appScope.vgmap.serviceGroupGrid.data.splice(index, 1);
                                   growl.success("Category record was deleted successfully");
                               }
                               else {
                                   growl.error("There was an error deleting Category Info record for: " + row.entity.CategoryName);
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

function psCategoryMappingsApiUri()
{
    var cateMap = this;

    cateMap.AddCategory = 'api/CategoryMappings/AddCategory';

    cateMap.UpdateCategory = 'api/CategoryMappings/UpdateUserDetails';

}


