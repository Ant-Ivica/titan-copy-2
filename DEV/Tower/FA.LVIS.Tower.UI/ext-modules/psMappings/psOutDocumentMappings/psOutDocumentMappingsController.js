"use strict";


angular.module('psOutDocumentMappings').controller('psOutDocumentMappingsController', psOutDocumentMappingsController);
angular.module('psOutDocumentMappings').service('psOutDocumentMappingsRowEditor', psOutDocumentMappingsRowEditor);
angular.module('psOutDocumentMappings').service('psOutDocumentMappingsApiUri', psOutDocumentMappingsApiUri);

psOutDocumentMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psOutDocumentMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$route', '$routeParams', 'UserInfo', '$q', '$cookies'];
function psOutDocumentMappingsController($scope, $rootScope, $http, $interval, $uibModal, psOutDocumentMappingsRowEditor, uiGridGroupingConstants, $location, $route, $routeParams, UserInfo, $q, $cookies) {

    var voutdocmap = this;

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
    var hasModifyAccess = false;
    var hasSuperAccess = false;

    if ($rootScope.activityright === 'Admin' || $rootScope.activityright === 'SuperAdmin') {
        hasAccess = true;
        hasModifyAccess = true;
    }
    if ($rootScope.activityright === 'SuperAdmin') {
        hasSuperAccess = true;
    }

    $scope.hasModifyAccess = hasModifyAccess;
    $scope.hasAccess = hasAccess;
    $rootScope.hasAccess = hasAccess;
    $scope.hasSuperAccess = hasSuperAccess;
    $rootScope.hasSuperAccess = hasSuperAccess;

    voutdocmap.search = "";
    voutdocmap.LenderName = ($routeParams.LenderName || "");

    var isGroups = ($routeParams.isGroups || "false");
    voutdocmap.ShowGroups = false;
    if (isGroups == "true")
        voutdocmap.ShowGroups = true;

    if ($routeParams.LenderName.indexOf(":") > 0)
        voutdocmap.search = voutdocmap.LenderName.split(":")[1];

    voutdocmap.editOutDocument = psOutDocumentMappingsRowEditor.editOutDocument;
    voutdocmap.addNewOutDocument = psOutDocumentMappingsRowEditor.addNewOutDocument;
    voutdocmap.removeOutDocRow = psOutDocumentMappingsRowEditor.removeOutDocRow;

    var detailButtonOut = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'

    voutdocmap.serviceOutDocumentGrid = {
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
        exporterCsvFilename: 'vdmapInfo.csv',
        columnDefs: [
         { field: 'InternalDocumentType.Name', name: 'LVIS Document Type', displayName: 'LVIS Document Type', headerCellClass: 'grid-header', enableCellEdit: false, sort: { priority: 0, direction: 'asc' } },
         { field: 'ExternalApplication.Name', name: 'Application', headerCellClass: 'grid-header', enableCellEdit: false, sort: { priority: 0, direction: 'asc' } },
        { field: 'Service.Name', name: 'Service', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'DocumentStatus.Name', name: 'Document Status', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'ExternalDocumentType.Name', name: 'External Document Type', headerCellClass: 'grid-header', enableCellEdit: false },
         { field: 'ExternalMessageTypeValue', name: 'External Message Type', headerCellClass: 'grid-header', enableCellEdit: false },
         { field: 'Tenant.Name', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, sort: { priority: 0, direction: 'asc' } }
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.voutdocmap.editOutDocument(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {

            voutdocmap.serviceOutDocumentGrid.gridApi = gridApi;
        }
    };
    voutdocmap.CategoryId = "";
    voutdocmap.Customerid = "";

    if (voutdocmap.ShowGroups) {
        voutdocmap.CategoryId = voutdocmap.search;
        voutdocmap.Applicationid = voutdocmap.LenderName.split(":")[2];

        if (!voutdocmap.Applicationid)
        { voutdocmap.Applicationid = 0; }

        $scope.Applicationid = voutdocmap.Applicationid;
        if ($rootScope.activityright) {
            $http.get('OutboundDocs/GetGroupDocs/' + voutdocmap.search + "/" + voutdocmap.Applicationid).success(function (response) {
                voutdocmap.serviceOutDocumentGrid.data = response;
            });
        }
    }
    else {

        voutdocmap.Customerid = voutdocmap.search;
        voutdocmap.Applicationid = voutdocmap.LenderName.split(":")[2];
        $scope.Applicationid = voutdocmap.Applicationid;
        $http.get('OutboundDocs/GetDocs/' + voutdocmap.search + "/" + voutdocmap.Applicationid).success(function (response) {
            voutdocmap.serviceOutDocumentGrid.data = response;
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
        $scope.gridApi.grouping.groupColumn('Name');
        //$scope.gridApi.grouping.aggregateColumn('state', uiGridGroupingConstants.aggregation.COUNT);
    };

    $http.get('Security/GetTenant')
    .then(function (response) {
        $scope.Tenant = response.data;
    });

    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.voutdocmap.serviceOutDocumentGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant.Name');
    if ($rootScope.tenantname == 'LVIS')
        $scope.voutdocmap.serviceOutDocumentGrid.columnDefs[pos].visible = true;
    else
        $scope.voutdocmap.serviceOutDocumentGrid.columnDefs[pos].visible = false;

    $scope.addNewOutDocument = function () {
        var newService = {
            "OutboundDocumentMapid": 0,
            "InternalDocumentType": "",
            "ExternalDocumentType": "",
            "ExternalMessageType": "",
            "Service": "",
            "DocumentStatus": "",
            "isGroups": voutdocmap.ShowGroups,
            "CategoryId": voutdocmap.CategoryId,
            "Customerid": voutdocmap.Customerid,
            "Tenant": "",
            "Appid": voutdocmap.Applicationid,
            "ExternalMessageTypeList": []

        };
        var rowTmp = {};
        rowTmp.entity = newService;
        voutdocmap.addNewOutDocument($scope.voutdocmap.serviceOutDocumentGrid, rowTmp);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.voutdocmap.serviceOutDocumentGrid.selectItem(index, false);
        $scope.voutdocmap.serviceOutDocumentGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        $http.post('OutboundDocs/DeleteDoc', row.entity)
        .success(function (data) {
            if (data === 0) {
                growl.error('Outbound Document record could not be updated');
                var index = $scope.voutdocmap.serviceOutDocumentGrid.data.indexOf(row.entity);
                $scope.voutdocmap.serviceOutDocumentGrid.data.splice(index, 1);
                return;
            }
            else {
                growl.success("Outbound Document record was updated successfully");
            }
        }).error(function (response) {
            growl.error(response.ExceptionMessage);
            return;
        });
    };
}

psOutDocumentMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psOutDocumentMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editOutDocument = editOutDocument;
    service.addNewOutDocument = addNewOutDocument;

    function editOutDocument(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psOutDocumentMappings/Out-Document-mappings-edit.html',
            controller: 'psOutDocumentMappingsRowEditCtrl',
            controllerAs: 'voutdocmap',
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

    function addNewOutDocument(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psOutDocumentMappings/Out-Document-mappings-add.html',
            controller: 'psOutDocumentMappingsRowEditCtrl',
            controllerAs: 'voutdocmap',
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


angular.module('psOutDocumentMappings').controller('psOutDocumentMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', '$confirm', 'growl', 'psOutDocumentMappingsApiUri',
function psOutDocumentMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, $confirm, growl, psOutDocumentMappingsApiUri) {

    var voutdocmap = this;
    voutdocmap.entity = angular.copy(row.entity);
    voutdocmap.save = save;
    voutdocmap.FormDirty = false;

    voutdocmap.ExternalApplicationList = [];

    voutdocmap.ExternalDoctypeList = [];
    voutdocmap.InternalDoctypeList = [];
    voutdocmap.ServiceList = [];
    voutdocmap.ExternalMessageTypeList = [];

    var getIndexIfObjWithOwnAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }

      
    voutdocmap.LoadExternalMessageTypes = function () {

        if (voutdocmap.entity.CategoryId != '') {           
            $http.get('OutboundDocs/GetOutboundMessageTypes/' + voutdocmap.entity.CategoryId + '/' + voutdocmap.entity.ExternalApplication.ID).success(function (data) {
                voutdocmap.ExternalMessageTypeList = data;
                //start-Below lines works untill AddSelectedMesssage is clicked for selecting MessageType List save button will not enabled              
                voutdocmap.FormDirty = false;
                $scope.EditDocForm.$setPristine();
                //End
            });
        }
        else {           
            $http.get('OutboundDocs/GetOutboundMessageTypesbyCustomer/' + voutdocmap.entity.Customerid + '/' + voutdocmap.entity.ExternalApplication.ID).success(function (data) {
                voutdocmap.ExternalMessageTypeList = data;
                //start-Below lines works untill AddSelectedMesssage is clicked for selecting MessageType List save button will not enabled
                voutdocmap.FormDirty = false;
                $scope.EditDocForm.$setPristine();
                //End
            });
        }
        
    }

    voutdocmap.LoadExternalDocTypes = function () {

        $http.get('InboundDocs/GetDocTypes/' + voutdocmap.entity.ExternalApplication.ID).success(function (data) {
            voutdocmap.ExternalDoctypeList = data;
        });

        voutdocmap.LoadExternalMessageTypes();
    }

    if (voutdocmap.entity.OutboundDocumentMapid == 0) {

        $http.get('Security/GetAllApplications').success(function (data) {
            voutdocmap.ExternalApplicationList = data;

            var index = getIndexIfObjWithOwnAttr(voutdocmap.ExternalApplicationList, "ID", parseInt(voutdocmap.entity.Appid));
            if (index > -1)
                voutdocmap.entity.ExternalApplication = voutdocmap.ExternalApplicationList[index];

            voutdocmap.LoadExternalDocTypes();
        });
    }
    else
        voutdocmap.LoadExternalDocTypes();

    $http.get('InboundDocs/GetDocTypes/1').success(function (data) {
        voutdocmap.InternalDoctypeList = data;
    });

    $http.get('InboundDocs/GetServices').success(function (data) {
        voutdocmap.ServiceList = data;
    });

    $http.get('OutboundDocs/GetStatus').success(function (data) {
        voutdocmap.DocumentStatusList = data;
        if (voutdocmap.entity.OutboundDocumentMapid == '0')
            voutdocmap.entity.DocumentStatus = voutdocmap.DocumentStatusList[0];
    });

    voutdocmap.editItem = function (obj, item) {
        item.Editable = true;
    }

    voutdocmap.doneEditing = function ($event, key, item) {
        voutdocmap.FormDirty = true;
        voutdocmap.entity.ExternalMessageTypeList[key].Sequence = item.Sequence;
        item.Editable = false;
    }


    //var IsMessageButtonActive = false;
    voutdocmap.AddMMessageType = function AddMMessageType() {
        var index = getIndexIfObjWithOwnAttr(voutdocmap.entity.ExternalMessageTypeList, "MessageTypeName", voutdocmap.entity.ExternalMessageType.MessageTypeName);
        if (index < 0) {
            voutdocmap.entity.ExternalMessageType.Sequence = voutdocmap.entity.ExternalMessageTypeList.length;
            voutdocmap.entity.ExternalMessageTypeList.push(voutdocmap.entity.ExternalMessageType);
        }
        voutdocmap.entity.ExternalMessageType = undefined;
        voutdocmap.FormDirty = true;
    }

    voutdocmap.Remove = function Remove(Condition) {
        voutdocmap.FormDirty = true;
        var index = voutdocmap.entity.ExternalMessageTypeList.indexOf(Condition);
        if (index >= 0) {
            voutdocmap.entity.ExternalMessageTypeList.splice(index, 1);
            for (var i = 0; i < voutdocmap.entity.ExternalMessageTypeList.length; i++) {
                voutdocmap.entity.ExternalMessageTypeList[i]["Sequence"] = i;
            }
        }
        if (voutdocmap.entity.ExternalMessageTypeList.length == 0) {
            voutdocmap.entity.ExternalMessageType = undefined;
        }
        ////start-Below lines works untill AddSelectedMesssage is clicked for selecting MessageType List save button will not enabled
        //voutdocmap.FormDirty = false;
        //$scope.EditDocForm.$setPristine();
        //End
    }

    function save() {
        if (voutdocmap.entity.OutboundDocumentMapid == 0) {

            row.entity = angular.extend(row.entity, voutdocmap.entity);

            $http.post(psOutDocumentMappingsApiUri.AddDocs, row.entity)
                  .success(function (data) {
                      
                      row.entity = data;
                      if (row.entity.docMapId <= 0 || data.length == 0) {                          
                          growl.error("There was an error adding Document Map Info record");
                          return;
                      }
                      else {
                          growl.success("Document Map Info record was added successfully");
                          grid.data.push(row.entity);
                      }
                  }).error(function (response) {
                      if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                          growl.error("Validation failed for one or more entities while Saving to DB");
                          return;
                      }
                      if (response.InnerException.InnerException.ExceptionMessage.indexOf("FOREIGN KEY") >= 0 
                          || response.InnerException.InnerException.ExceptionMessage.indexOf("duplicate") >= 0) {
                          growl.error('Document Map Info is a duplicate and cannot be added');
                          return;
                      }

                      growl.error(response.ExceptionMessage);
                      return;
                  });

        } else {
            row.entity = angular.extend(row.entity, voutdocmap.entity);
            if (row.entity.Service != null && row.entity.Service.ID == -1) {
                row.entity.Service = null;
            }
            $http.post(psOutDocumentMappingsApiUri.UpdateDoc, row.entity)
              .success(function (data) {
                  row.entity = data;

                  if (data == 0) {
                      growl.error('There was an error updating Document Map Info record');
                      return;
                  }
                  else {
                      growl.success("Document Map Info record was updated successfully");
                  }
              }).error(function (response) {
                  if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                      growl.error("Validation failed for one or more entities while Saving to DB");
                      return;
                  }
                  if (response.InnerException.InnerException.ExceptionMessage.indexOf("FOREIGN KEY") >= 0
                          || response.InnerException.InnerException.ExceptionMessage.indexOf("duplicate") >= 0) {
                      growl.error('Document Map Info record is a duplicate and cannot be updated');
                      return;
                  }

                  growl.error(response.ExceptionMessage);
                  return;
              });
        }

        $uibModalInstance.close(row.entity);
    }

    voutdocmap.remove = remove;
    function remove() {
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
        })
            .then(function () {

                if (row.entity.OutboundDocumentMapid != '0') {
                    row.entity = angular.extend(row.entity, voutdocmap.entity);
                    $http.post('OutboundDocs/DeleteDoc', row.entity.OutboundDocumentMapid)
        .success(function (data) {
            if (data > 0) {
                var index = grid.appScope.voutdocmap.serviceOutDocumentGrid.data.indexOf(row.entity);
                grid.appScope.voutdocmap.serviceOutDocumentGrid.data.splice(index, 1);
                growl.success("Outbound Document record was deleted successfully");
                return;
            }
            else {
                growl.error("Outbound Document record could not be deleted");
            }
        }).error(function (response) {

            $confirm({ text: 'Outbound Document info record cannot be deleted because it is being used by another part of the Tower application.' }, {
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

}
]);

function psOutDocumentMappingsApiUri()
{
    var outDoc = this;

    outDoc.AddDocs = 'api/OutDocumentMappings/AddDocs';

    outDoc.UpdateDoc = 'api/OutDocumentMappings/UpdateDoc';
}

