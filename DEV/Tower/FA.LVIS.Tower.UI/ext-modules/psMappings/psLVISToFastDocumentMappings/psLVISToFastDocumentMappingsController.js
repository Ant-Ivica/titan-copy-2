"use strict";

angular.module('psLvisToFastDocumentMappings').controller('psLVISToFastDocumentMappingsController', psLVISToFastDocumentMappingsController);
angular.module('psLvisToFastDocumentMappings').service('psLVISToFastDocumentMappingsRowEditor', psLVISToFastDocumentMappingsRowEditor);
angular.module('psLvisToFastDocumentMappings').service('psLvisToFastDocumentMappingsApiUri', psLvisToFastDocumentMappingsApiUri);

psLVISToFastDocumentMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psLVISToFastDocumentMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies', 'growl'];
function psLVISToFastDocumentMappingsController($scope, $rootScope, $http, $interval, $uibModal, psLVISToFastDocumentMappingsRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies, growl) {
    var vLVISdocmap = this;

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

    vLVISdocmap.editInDocument = psLVISToFastDocumentMappingsRowEditor.editInDocument;
    vLVISdocmap.addNewInDocument = psLVISToFastDocumentMappingsRowEditor.addNewInDocument;
    vLVISdocmap.removeInDocRow = psLVISToFastDocumentMappingsRowEditor.removeInDocRow;

    var detailButtonIn = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'

    vLVISdocmap.serviceInDocumentGrid = {
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
            { field: 'InternalDocumentTypeDesc', name: 'LVIS Document Type', displayName: 'LVIS Document Type', headerCellClass: 'grid-header', enableCellEdit: false },
            { field: 'ExternalDocumentDescription', name: 'LVIS Document Description', displayName: 'LVIS Document Description', headerCellClass: 'grid-header', enableCellEdit: false },
            { field: 'ServiceName', name: 'Service', headerCellClass: 'grid-header', enableCellEdit: false },
            { field: 'ExternalDocumentTypeDesc', name: 'FAST Document Type', displayName: 'FAST Document Type', headerCellClass: 'grid-header', enableCellEdit: false },
            { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false }
        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vLVISdocmap.editInDocument(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {

            vLVISdocmap.serviceInDocumentGrid.gridApi = gridApi;
        }
    };

    if ($rootScope.activityright) {
        $http.get('FastDocs/LVISToFastDocs/').success(function (response) {
            vLVISdocmap.serviceInDocumentGrid.data = response;
        });
    }

    $http.get('Security/GetTenant')
    .then(function (response) {
        $scope.Tenant = response.data;
    });

    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.vLVISdocmap.serviceInDocumentGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname == 'LVIS')
        $scope.vLVISdocmap.serviceInDocumentGrid.columnDefs[pos].visible = true;
    else
        $scope.vLVISdocmap.serviceInDocumentGrid.columnDefs[pos].visible = false;

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
            "inboundDocumentMapid": 0,
            "ExternalApplication": 5,
            "ExternalDocumentType": "",
            "ExternalDocumentDescription": "",
            "Service": "",
            "InternalDocumentType": "",
            "TenantId": "",
            "Tenant": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vLVISdocmap.addNewInDocument($scope.vLVISdocmap.serviceInDocumentGrid, rowTmp);
    };


    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vLVISdocmap.serviceInDocumentGrid.selectItem(index, false);
        $scope.vLVISdocmap.serviceInDocumentGrid.splice(index, 1);
    };


    $scope.deleteRow = function (row) {
        var index = $scope.vLVISdocmap.serviceInDocumentGrid.data.indexOf(row.entity);
        $scope.vLVISdocmap.serviceInDocumentGrid.data.splice(index, 1);
    };
}

psLVISToFastDocumentMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psLVISToFastDocumentMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editInDocument = editInDocument;
    service.addNewInDocument = addNewInDocument;

    function editInDocument(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psLVISToFastDocumentMappings/LVIS-Out-Document-mappings-edit.html',
            controller: 'psLVISToFastDocumentMappingsRowEditCtrl',
            controllerAs: 'vLVISdocmap',
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
            templateUrl: 'ext-modules/psMappings/psLVISToFastDocumentMappings/LVIS-Out-Document-mappings-add.html',
            controller: 'psLVISToFastDocumentMappingsRowEditCtrl',
            controllerAs: 'vLVISdocmap',
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


angular.module('psLvisToFastDocumentMappings').controller('psLVISToFastDocumentMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', '$confirm', 'growl', 'psLvisToFastDocumentMappingsApiUri',
function psLVISToFastDocumentMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, $confirm, growl, psLvisToFastDocumentMappingsApiUri) {

    var vLVISdocmap = this;
    vLVISdocmap.entity = angular.copy(row.entity);

    vLVISdocmap.ExternalApplicationList = [];

    vLVISdocmap.ExternalDoctypeList = [];
    vLVISdocmap.InternalDoctypeList = [];
    vLVISdocmap.ServiceList = [];

    $http.get('InboundDocs/GetDocTypes/5').success(function (data) {
        vLVISdocmap.ExternalDoctypeList = data;
    });


    $http.get('InboundDocs/GetDocTypes/1').success(function (data) {
        vLVISdocmap.InternalDoctypeList = data;
    });

    $http.get('InboundDocs/GetServices').success(function (data) {
        vLVISdocmap.ServiceList = data;
    });

    if (vLVISdocmap.entity.Tenant == '') {
        vLVISdocmap.entity.Tenant = $scope.tenantname;
    }


    vLVISdocmap.save = save;
    function save() {

        if (vLVISdocmap.entity.inboundDocumentMapid == '0') {
            row.entity = angular.extend(row.entity, vLVISdocmap.entity);

            $http.post(psLvisToFastDocumentMappingsApiUri.AddDoc, row.entity)
        .success(function (data) {
            //real ID come back from response after the save in DB
            row.entity = data;
            grid.data.push(row.entity);

            if (data.length == 0) {
                growl.error('Document Map info record is a duplicate and cannot be added');
                return;
            }
            else {
                growl.success("Document Map info record was added successfully");
            }
        }).error(function (response) {

            if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                growl.error("Error in adding Document Map info record");
                return;
            }
            if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                growl.error('Document Map info record is a duplicate and cannot be added');
                return;
            }
            growl.error(response.ExceptionMessage);
            return;
        });

        } else {
            vLVISdocmap.entity = angular.extend(row.entity, vLVISdocmap.entity);
            $http.post(psLvisToFastDocumentMappingsApiUri.UpdatedDoc, row.entity)
         .success(function (data) {
             if (data == 0) {
                 growl.error('Document Map info record is a duplicate and cannot be updated');
                 return;
             }
             else {
                 //Added line to bind updated coloumn to ui-grid
                 row.entity = data;
                 grid.data = row.entity;
                 growl.success("Document Map info record was updated successfully");
             }

         }).error(function (response) {
             if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                 growl.error("Error in updating Document Map info record");
                 return;
             }
             if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                 growl.error('Document Map info record is a duplicate and cannot be added');
                 return;
             }
             growl.error(response.ExceptionMessage);
             return;
         });

        }
        $uibModalInstance.close(row.entity);
    }


    vLVISdocmap.remove = remove;
    function remove() {
        $confirm({ text: 'Proceed to delete this selection ?' }, {
            size: 'sm', defaultLabels: {
                title: ' Confirm Delete ',
                ok: 'Yes',
                cancel: 'No'
            },
            template: '<div class="widget-header"><h2><i class="fa fa-exclamation-triangle" aria-hidden="true" style="padding-left:0px;"></i>{{data.title}}</h2></div>' +
                       '<div class="widget-content">{{data.text}}</div>' +
                       '<div class="modal-footer">' +
                       '<button class="btn btn-success" ng-click="ok()">{{data.ok}}</button>' +
                       '<button class="btn btn-danger" ng-click="cancel()">{{data.cancel}}</button>' +
                       '</div>'
        })
            .then(function () {

                if (row.entity.inboundDocumentMapid != '0') {
                    row.entity = angular.extend(row.entity, vLVISdocmap.entity);
                    $http.post('OutboundDocs/DeleteDoc', row.entity.inboundDocumentMapid)
        .success(function (data) {
            if (data === 1) {
                var index = grid.appScope.vLVISdocmap.serviceInDocumentGrid.data.indexOf(row.entity);
                grid.appScope.vLVISdocmap.serviceInDocumentGrid.data.splice(index, 1);
                growl.success("LVIS to FAST info record was deleted successfully");
                return;
            }
            else {
                growl.error("There was an error deleting LVIS to FAST Map info record");
            }

        }).error(function (response) {

            $confirm({ text: 'This cannot be deleted because it is being used by another part of the Tower application.' }, {
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
        });
                }

                $uibModalInstance.close(row.entity);
            });
    }

}]);

function psLvisToFastDocumentMappingsApiUri() {

    var fastDoc = this;
      
    fastDoc.AddDoc = 'api/FastDocumentMappings/AddDoc';

    fastDoc.UpdatedDoc = 'api/FastDocumentMappings/UpdateDoc';

}



