"use strict";

angular.module('psInDocumentMappings').controller('psInDocumentMappingsontroller', psInDocumentMappingsontroller);
angular.module('psInDocumentMappings').service('psInDocumentMappingsRowEditor', psInDocumentMappingsRowEditor);
angular.module('psInDocumentMappings').service('psInDocumentMappingsApiUri', psInDocumentMappingsApiUri);

psInDocumentMappingsontroller.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psInDocumentMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies', 'growl'];
function psInDocumentMappingsontroller($scope, $rootScope, $http, $interval, $uibModal, psInDocumentMappingsRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies, growl) {
    var vindocmap = this;

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

    vindocmap.editInDocument = psInDocumentMappingsRowEditor.editInDocument;
    vindocmap.addNewInDocument = psInDocumentMappingsRowEditor.addNewInDocument;
    vindocmap.removeInDocRow = psInDocumentMappingsRowEditor.removeInDocRow;

    var detailButtonIn = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'

    vindocmap.serviceInDocumentGrid = {
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
        exporterCsvFilename: 'document.csv',
        columnDefs: [
            { field: 'ExternalApplicationName', name: 'Application', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
            { field: 'ExternalDocumentTypeDesc', name: 'External Document Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ExternalDocumentDescription', name: 'External Document Description', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ServiceName', name: 'Service', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'InternalDocumentTypeDesc', name: 'LVIS Document Type', displayName: 'LVIS Document Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false }

         ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vindocmap.editInDocument(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {

            vindocmap.serviceInDocumentGrid.gridApi = gridApi;
        }
    };

    if ($rootScope.activityright) {
        $http.get('InboundDocs/GetInboundDocs/').success(function (response) {
            vindocmap.serviceInDocumentGrid.data = response;
        });
    }

    $http.get('Security/GetTenant')
    .then(function (response) {
        $scope.Tenant = response.data;
    });

    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.vindocmap.serviceInDocumentGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname == 'LVIS')
        $scope.vindocmap.serviceInDocumentGrid.columnDefs[pos].visible = true;
    else
        $scope.vindocmap.serviceInDocumentGrid.columnDefs[pos].visible = false;

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
        $scope.gridApi.grouping.groupColumn('LenderDocName');
        //$scope.gridApi.grouping.aggregateColumn('state', uiGridGroupingConstants.aggregation.COUNT);
    };

    $scope.addNewInDocument = function () {
        var newService = {
            "inboundDocumentMapid":0,
            "ExternalApplication": "",
            "ExternalDocumentType": "",
            "ExternalDocumentDescription": "",
            "Service": "",
            "InternalDocumentType": "",
            "TenantId": "",
            "Tenant": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vindocmap.addNewInDocument($scope.vindocmap.serviceInDocumentGrid, rowTmp);
    };
   

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vindocmap.serviceInDocumentGrid.selectItem(index, false);
        $scope.vindocmap.serviceInDocumentGrid.splice(index, 1);
    };
  

    $scope.deleteRow = function (row) {
        var index = $scope.vindocmap.serviceInDocumentGrid.data.indexOf(row.entity);
        $scope.vindocmap.serviceInDocumentGrid.data.splice(index, 1);
    };
   
}


psInDocumentMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psInDocumentMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editInDocument = editInDocument;
    service.addNewInDocument = addNewInDocument;


    function editInDocument(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psInDocumentMappings/In-Document-mappings-edit.html',
            controller:  'psInDocumentMappingsRowEditCtrl',
            controllerAs: 'vindocmap',
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

    function addNewInDocument(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psInDocumentMappings/In-Document-mappings-add.html',
            controller: 'psInDocumentMappingsRowEditCtrl',
            controllerAs: 'vindocmap',
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


angular.module('psInDocumentMappings').controller('psInDocumentMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', '$confirm', 'growl', 'psInDocumentMappingsApiUri',
function psInDocumentMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, $confirm, growl, psInDocumentMappingsApiUri) {

    var vindocmap = this;
    vindocmap.entity = angular.copy(row.entity);

    vindocmap.ExternalApplicationList = [];

    vindocmap.ExternalDoctypeList = [];
    vindocmap.InternalDoctypeList = [];
    vindocmap.ServiceList = [];

    vindocmap.LoadExternalDocTypes = function () {
        $http.get('InboundDocs/GetDocTypes/' + vindocmap.entity.ExternalApplication).success(function (data) {
            vindocmap.ExternalDoctypeList = data;
        });


    }
    if (vindocmap.entity.inboundDocumentMapid != '0') {
        $http.get('InboundDocs/GetDocTypes/' + vindocmap.entity.ExternalApplication).success(function (data) {
            vindocmap.ExternalDoctypeList = data;
        });
    }


    $http.get('Security/GetAllApplications').success(function (data) {
        vindocmap.ExternalApplicationList = data;
    });

    $http.get('InboundDocs/GetDocTypes/1').success(function (data) {
        vindocmap.InternalDoctypeList = data;
    });

    $http.get('InboundDocs/GetServices').success(function (data) {
        vindocmap.ServiceList = data;
    });

    if (vindocmap.entity.Tenant == '') {
        vindocmap.entity.Tenant = $scope.tenantname;
    }
    vindocmap.save = save;
    function save() {

        if (vindocmap.entity.inboundDocumentMapid == '0') {
            row.entity = angular.extend(row.entity, vindocmap.entity);
            $http.post(psInDocumentMappingsApiUri.AddDocs, row.entity)
        .success(function (data) {
            //real ID come back from response after the save in DB
            row.entity = data;
            grid.data.push(row.entity);

            if (data.length == 0) {
                growl.error('Inbound Document Map info record is a duplicate and cannot be added');
                return;
            }
            else {
                growl.success("Inbound Document info record was added successfully");
            }
        }).error(function (response) {

            if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                growl.error("Error updating Inbound Document info record");
                return;
            }
            if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                growl.error('Inbound Document Map info record is a duplicate and cannot be added');
                return;
            }
            growl.error(response.ExceptionMessage);
            return;
        });
         
        } else

        {
            vindocmap.entity = angular.extend(row.entity, vindocmap.entity);
            $http.post(psInDocumentMappingsApiUri.UpdateInboundDoc, row.entity)
         .success(function (data) {
             if (data == 0) {
                 growl.error('Inbound Document Map info is a duplicate and cannot be updated');
                 return;
             }
             else {
                 //Added line to bind updated coloumn to ui-grid
                 row.entity = data;
                 grid.data = row.entity;
                 growl.success("Inbound Document Map info record was updated successfully");
             }

         }).error(function (response) {
             if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                 growl.error("Error updating Inbound Document info record");
                 return;
             }
             if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                 growl.error('Inbound Document Map info record is a duplicate and cannot be updated');
                 return;
             }
             growl.error(response.ExceptionMessage);
             return;
         });

        }
        $uibModalInstance.close(row.entity);
    }

    vindocmap.remove = remove;
    function remove() {
        $confirm({ text: 'Proceed to delete this selection ?' }, {
            size: 'sm', defaultLabels: {
                title: ' Confirm Delete? ',
                ok: 'Yes',
                cancel: 'No'
            },
            template: '<div class="widget-header"><h2><i class="fa fa-question-circle" aria-hidden="true" style="padding-left:0px;"></i>{{data.title}}</h2></div>' +
                     '<div class="widget-content">{{data.text}}</div>' +
                     '<div class="modal-footer">' +
                     '<button class="btn btn-success" ng-click="ok()">{{data.ok}}</button>' +
                     '<button class="btn btn-danger" ng-click="cancel()">{{data.cancel}}</button>' +
                     '</div>'
            })
            .then(function () {
                if (row.entity.inboundDocumentMapid != '0') {
                    row.entity = angular.extend(row.entity, vindocmap.entity);
                    $http.post('InboundDocs/DeleteInboundDoc', row.entity.inboundDocumentMapid)
                    .success(function (data) {
                       
                            var index = grid.appScope.vindocmap.serviceInDocumentGrid.data.indexOf(row.entity);
                            grid.appScope.vindocmap.serviceInDocumentGrid.data.splice(index, 1);                            
                            growl.success("Inbound Document Map info record was deleted successfully");
                                        
                    }).error(function (response) {
                            growl.error("Inbound Document Map info record cannot  deleted successfully");                                          
                            growl.error(response.ExceptionMessage);
                        return;
                    });
                }

                $uibModalInstance.close(row.entity);
            });
    }


}]);

function psInDocumentMappingsApiUri()
{
    var inbound = this;

    inbound.UpdateInboundDoc = 'api/InDocumentMappings/UpdateInboundDoc';

    inbound.AddDocs = 'api/InDocumentMappings/AddDocs';

}



