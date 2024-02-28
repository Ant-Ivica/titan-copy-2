"use strict";

angular.module('psSubscription').controller('psSubscriptionController', psSubscriptionController);
angular.module('psSubscription').service('psSubscriptionControllerRowEditor', psSubscriptionControllerRowEditor);
angular.module('psSubscription').service('psSubscriptionApiUri', psSubscriptionApiUri);


psSubscriptionController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psSubscriptionControllerRowEditor', 'uiGridGroupingConstants', '$location', '$route', '$routeParams', 'UserInfo', '$q', '$cookies'];
function psSubscriptionController($scope, $rootScope, $http, $interval, $uibModal, psSubscriptionControllerRowEditor, uiGridGroupingConstants, $location, $route, $routeParams, UserInfo, $q, $cookies) {

    var vsubscr = this;

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

    vsubscr.search = "";
    vsubscr.CustomerName = ($routeParams.CustomerName || "");

    var isCategory = ($routeParams.isCategory || "false");
    $rootScope.isCategory = isCategory;
    vsubscr.ShowGroups = false;
    if (isCategory == "true")
        vsubscr.ShowGroups = true;

    if ($routeParams.CustomerName.indexOf(":") > 0)
        vsubscr.search = vsubscr.CustomerName.split(":")[1];


    vsubscr.editRowSummary = psSubscriptionControllerRowEditor.editRowSummary;
    vsubscr.addRowSummary = psSubscriptionControllerRowEditor.addRowSummary;


    var detailButtonOut = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'

    vsubscr.SubscriptionGrid = {
        enableColumnResize: true,
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
        exporterCsvFilename: 'SubscriptionInfo.csv',
        columnDefs: [
            { field: 'MessageTypeId', visible: false, name: 'Message Type Id', displayName: 'Message Type Id', headerCellClass: 'grid-header', enableCellEdit: false, sort: { priority: 0, direction: 'asc' } },
            { field: 'MessageTypeName', name: 'Message Type Name', displayName: 'Message Type Name', headerCellClass: 'grid-header', enableCellEdit: false, sort: { priority: 0, direction: 'asc' } },
            { field: 'MessageTypeDesc', name: 'Message Type Description', displayName: 'Message Type Description', headerCellClass: 'grid-header', enableCellEdit: false, sort: { priority: 0, direction: 'asc' } },
            { field: 'TenantName', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, sort: { priority: 0, direction: 'asc' } }
            
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vsubscr.editRowSummary(grid, row)\"  ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vsubscr.SubscriptionGrid.gridApi = gridApi;
        }
    };
    vsubscr.CategoryId = "";
    vsubscr.Customerid = "";
    vsubscr.Tenantid = "";
    vsubscr.Tenant = "";

    if (vsubscr.ShowGroups) {
        vsubscr.CategoryId = vsubscr.search;
        vsubscr.Applicationid = vsubscr.CustomerName.split(":")[2];
        vsubscr.Tenantid = vsubscr.CustomerName.split(":")[3];
        vsubscr.Tenant = vsubscr.CustomerName.split(":")[4];

        if (!vsubscr.Applicationid)
        { vsubscr.Applicationid = 0; }

        $scope.Applicationid = vsubscr.Applicationid;
        if ($rootScope.activityright) {
            $http.get('Subscriptions/GetSubscriptionsByCategory/' + vsubscr.CategoryId + "/" + vsubscr.Applicationid).success(function (response) {
                vsubscr.SubscriptionGrid.data = response;
            });
        }
    }
    else {
        vsubscr.Customerid = vsubscr.search;
        vsubscr.Applicationid = vsubscr.CustomerName.split(":")[2];
        $scope.Applicationid = vsubscr.Applicationid;
        vsubscr.Tenantid = vsubscr.CustomerName.split(":")[3];
        vsubscr.Tenant = vsubscr.CustomerName.split(":")[4];

        $http.get('Subscriptions/GetSubscriptionsByCustomer/' + vsubscr.Customerid + "/" + vsubscr.Applicationid).success(function (response) {
            vsubscr.SubscriptionGrid.data = response;
        });
    }

    $http.get('Security/GetTenant')
        .then(function (response) {
            $scope.Tenant = response.data;
        });

    $scope.expandAll = function () {
        $scope.gridApi.treeBase.expandAllRows();
    };

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
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
    var pos = $scope.vsubscr.SubscriptionGrid.columnDefs.map(function (e) { return e.field; }).indexOf('TenantName');
    if ($rootScope.tenantname == 'LVIS')
        $scope.vsubscr.SubscriptionGrid.columnDefs[pos].visible = true;
    else
        $scope.vsubscr.SubscriptionGrid.columnDefs[pos].visible = false;

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vsubscr.SubscriptionGrid.selectItem(index, false);
        $scope.vsubscr.SubscriptionGrid.splice(index, 1);
    };
    $scope.addRowSummary = function () {
        var newService = {
            "SubscriptionId": 0,
            "MessageTypeId": "",
            "MessageTypeName": "",
            "MessageTypeDesc": "",
            "Applicationid": vsubscr.Applicationid,
            "CategoryId": vsubscr.CategoryId,
            "Customerid": vsubscr.Customerid,
            "TenantId": vsubscr.Tenantid,
            "Tenant": vsubscr.Tenant
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vsubscr.addRowSummary($scope.vsubscr.SubscriptionGrid, rowTmp);
    };
}

psSubscriptionControllerRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psSubscriptionControllerRowEditor($http, $rootScope, $uibModal) {
    //var vlmap = this;
    var service = {};
    service.editRowSummary = editRowSummary;
    service.addRowSummary = addRowSummary;

    function editRowSummary(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psSubscription/subscription-edit.html',
            controller: 'psSubscriptionRowEditCtrl',
            controllerAs: 'vsubscr',
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

    function addRowSummary(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psSubscription/subscription-add.html',
            controller: 'psSubscriptionRowEditCtrl',
            controllerAs: 'vsubscr',
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

angular.module('psSubscription').controller('psSubscriptionRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', 'psSubscriptionApiUri',
function psSubscriptionRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, psSubscriptionApiUri) {
    var vsubscr = this;
    vsubscr.entity = angular.copy(row.entity);
    vsubscr.LoadMEssageType = LoadMEssageType;
    vsubscr.LoadApplicationMEssageType = LoadApplicationMEssageType;
    vsubscr.save = save;
    vsubscr.isCategory = $scope.$parent.isCategory;
    
    var getIndexIfObjWithOwnAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }

    var ApplicationId = 0;
    if (vsubscr.entity.MessageTypeId != "") {
        ApplicationId = vsubscr.entity.ApplicationId;
        LoadApplicationMEssageType(ApplicationId);
        
    }
    else {
        ApplicationId = vsubscr.entity.Applicationid;
        LoadApplicationMEssageType(ApplicationId);
    }
      
    if ($scope.tenantname != '') {
        vsubscr.entity.Tenant = vsubscr.entity.Tenant;
       
    }

    //Commented bcz either Customer or Category already associated with an Tenant
    //else {
    //    vsubscr.TenantList = [];
    //    $http.get('FilePreferences/GetTenantList').success(function (data) {
    //        vsubscr.TenantList = data;
    //    });
    //    vsubscr.entity.Tenant = $scope.tenantname;      
    //}
      
    //start-Bind-Message List    
    vsubscr.LoadMesageTypeList = [];
    function LoadApplicationMEssageType(ApplicationId) {
        if (ApplicationId != "") {
            if (vsubscr.ExternalApplicationList != undefined) {
                var index = getIndexIfObjWithOwnAttr(vsubscr.ExternalApplicationList, "ApplicationId", parseInt(ApplicationId));
                if (index > -1) {
                    vsubscr.entity.Applicationid = vsubscr.ExternalApplicationList[index];
                    vsubscr.entity.ApplicationId = vsubscr.ExternalApplicationList[index];
                }
            }            
            $http.get('Subscriptions/GetApplicationMessageType/' + ApplicationId + '/' + vsubscr.entity.TenantId + '/' + '/' + vsubscr.entity.SubscriptionId).success(function (data) {
                vsubscr.LoadMesageTypeList = data;
            });
        }
    }
         //Get Message Type     
    function LoadMEssageType() {
       // var messageTypeId = vsubscr.entity.MessageTypeId;

            $http.get('Subscriptions/GetMessageTypeDetails/' + vsubscr.entity.MessageTypeId).success(function (data) {
                if (data.length == 0) {
                    //vsubscr.entity.MessageTypeName = "";
                    vsubscr.entity.MessageTypeDesc = "";
                    return;
                }                
                vsubscr.entity.MessageTypeName = data[0].MessageTypeName;
                vsubscr.entity.MessageTypeDesc = data[0].MessageTypeDescription;
            });
        }
   

        //Get Application List
        vsubscr.ExternalApplicationList = [];       
        $http.get('Subscriptions/GetApplicationByTenant/' + vsubscr.entity.TenantId).success(function (data) {
            vsubscr.ExternalApplicationList = data;
            var index = getIndexIfObjWithOwnAttr(vsubscr.ExternalApplicationList, "ApplicationId", parseInt(ApplicationId));
            if (index > -1) {
                vsubscr.entity.Applicationid = vsubscr.ExternalApplicationList[index];
                vsubscr.entity.ApplicationId = vsubscr.ExternalApplicationList[index];
            }                
        });



  //Save 
    function save() {
        if (vsubscr.entity.SubscriptionId == '0') {
            //row.entity = angular.extend(row.entity, vsubscr.entity);
            row.entity.Applicationid = vsubscr.entity.Applicationid.ApplicationId;
            row.entity.CategoryId = vsubscr.entity.CategoryId;
            row.entity.Customerid = vsubscr.entity.Customerid;
            row.entity.MessageTypeId = vsubscr.entity.MessageTypeId;
            row.entity.TenantId = vsubscr.entity.TenantId;


            $http.post(psSubscriptionApiUri.AddSubscription, row.entity)
        .success(function (data) {
            //real ID come back from response after the save in DB
            row.entity = data;
            grid.data.push(row.entity);
            if (data.length == 0) {
                growl.error('subscription Info record is a duplicate and cannot be added');
                return;
            }
            else {
                growl.success("Subscription Info record was added successfully");
            }
        }).error(function (response) {

            if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                growl.error("Subscription Info record could not be added");
                return;
            }
            if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0
                || response.InnerException.InnerException.ExceptionMessage.indexOf("Cannot insert duplicate key row in object") >= 0) {
                growl.error('Subscription Info record is a duplicate and cannot be added');
                return;
            }
            growl.error(response.InnerException.InnerException.ExceptionMessage);
            return;
        });
            //grid.data.push(vlmap.entity);

        } else {

            //vsubscr.entity = angular.extend(row.entity, vsubscr.entity);
            row.entity.SubscriptionID = vsubscr.entity.SubscriptionId;
            row.entity.Applicationid = vsubscr.entity.ApplicationId.ApplicationId;
            row.entity.CategoryId = vsubscr.entity.CategoryId;
            row.entity.Customerid = vsubscr.entity.CustomerId;
            row.entity.MessageTypeId = vsubscr.entity.MessageTypeId;
            row.entity.TenantId = vsubscr.entity.TenantId;
            $http.post(psSubscriptionApiUri.UpdateSubscription, row.entity)
         .success(function (data) {
             if (data == 0) {
                 growl.error('Subscription Info record is a duplicate and cannot be added');
                 return;
             }
             else {
                 //Added line to bind updated coloumn to ui-grid
                 row.entity = data;
                 grid.data = row.entity;
                 if (row.entity.Rcdcount == '0') {
                     growl.success("Subscription Info record was updated successfully");
                 }
                 else { growl.success("Subscription Info record already Exists."); }
             }

         }).error(function (response) {
             if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                 growl.error("Subscription Info record could not be added");
                 return;
             }
             if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0
                 || response.InnerException.InnerException.ExceptionMessage.indexOf("Cannot insert duplicate key row in object") >= 0) {
                 growl.error('Subscription Info record is a duplicate and cannot be added');
                 return;
             }
             growl.error(response.InnerException.InnerException.ExceptionMessage);
             return;
         });
        }
        $uibModalInstance.close(row.entity);
    }

    //Remove Subscription
    vsubscr.remove = remove;
    function remove() {
        if (row.entity.SubscriptionId != '0') {
            row.entity = angular.extend(row.entity, vsubscr.entity);
            $http.post('Subscriptions/Delete', row.entity.SubscriptionId)
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
                       if (row.entity.SubscriptionId != '0') {
                           row.entity = angular.extend(row.entity, vsubscr.entity);
                           $http.post('Subscriptions/ConfirmDeleteSubscription', row.entity.SubscriptionId)
                          .success(function (data) {
                              if (data === 1) {
                                  var index = grid.appScope.vsubscr.SubscriptionGrid.data.indexOf(row.entity);
                                  grid.appScope.vsubscr.SubscriptionGrid.data.splice(index, 1);
                                  growl.success("Subscription Info was Deleted successfully");
                                  return;
                              }
                              else {
                                  growl.error("There was an error deleting Subscription Info record for: " + row.entity.SubscriptionId);
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

function psSubscriptionApiUri() {
    this.AddSubscription = 'api/subscription/AddSubscription';

    this.UpdateSubscription = 'api/subscription/UpdateSubscription';
}

