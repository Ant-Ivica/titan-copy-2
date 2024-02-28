"use strict";


angular.module('psContactProviderMappings').controller('psContactProviderMappingsController', psContactProviderMappingsController);
angular.module('psContactProviderMappings').service('psContactProviderMappingsRowEditor', psContactProviderMappingsRowEditor);
angular.module('psContactProviderMappings').service('psContactProviderMappingsApiUri', psContactProviderMappingsApiUri);

psContactProviderMappingsController.$inject = ['$scope', '$http', '$interval', '$uibModal', 'psContactProviderMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$route', '$routeParams', 'growl', 'UserInfo', '$rootScope', '$confirm', '$cookies', '$window'];
function psContactProviderMappingsController($scope, $http, $interval, $uibModal, psContactProviderMappingsRowEditor, uiGridGroupingConstants, $location, $route, $routeParams, growl, UserInfo, $rootScope, $confirm, $cookies, $window) {
    var vcontactprovidermap = this;
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
    vcontactprovidermap.editcontactprovider = psContactProviderMappingsRowEditor.editcontactprovider;
    vcontactprovidermap.addNewBranch = psContactProviderMappingsRowEditor.addNewBranch;     
    var splitparams = $routeParams.CustomerId.split(":");
    var customerName = splitparams[0];
    var customerId = splitparams[1];
    var tenantName = splitparams[2];
    var tenantId = splitparams[3];
    vcontactprovidermap.CustomerId = splitparams[1];
    vcontactprovidermap.CustomerName = splitparams[0];
    if ($routeParams.CustomerId.indexOf(":") > 1)
    {
        vcontactprovidermap.CustomerNameLink = splitparams[0];
    }       
    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deletecontactproviderRow(row)"></i></div>'
    vcontactprovidermap.serviceBranchGrid = {
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
        exporterCsvFilename: 'ContactProvider.csv',

        columnDefs: [
        { field: 'ProviderID', name: 'Provider Id', headerCellClass: 'grid-header', enableCellEdit: false },        
        { field: 'CustomerName', name: 'Customer Name',displayName: 'Customer Name', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'LocationName', name: 'Location Name', displayName: 'Location Name', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'LvisContactId', name: 'Contact Id', displayName: 'Contact ID', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'Tenant', name: 'Tenant', displayName: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false }        
        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vcontactprovidermap.editcontactprovider(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vcontactprovidermap.serviceBranchGrid.gridApi = gridApi;
        }
    };
    if ($rootScope.activityright) {
        $http.get('Security/GetContactProviderDetails/' + customerId).success(function (response) {
            vcontactprovidermap.serviceBranchGrid.data = response;
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
    $scope.toggleRow = function (rowNum) {
        $scope.gridApi.treeBase.toggleRowTreeState($scope.gridApi.grid.renderContainers.body.visibleRowCache[rowNum]);
    };

    $scope.changeGrouping = function () {
        $scope.gridApi.grouping.clearGrouping();
        $scope.gridApi.grouping.groupColumn('LocationName');
    };

    $scope.addNewBranch = function () {                                
        var newService = {
            "CustomerId":customerId,
            "CustomerName":customerName,            
            "ProviderID":"",
            "ContactId":"",
            "LocationId": "",
            "Tenant": tenantName,
            "TenantId":tenantId,
            "ContactProviderMapId":"0"
        };
        var rowTmp = {};
        rowTmp.entity = newService;        
        vcontactprovidermap.addNewBranch($scope.vcontactprovidermap.serviceBranchGrid, rowTmp);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vcontactprovidermap.serviceBranchGrid.selectItem(index, false);
        $scope.vcontactprovidermap.serviceBranchGrid.splice(index, 1);
    };

    $scope.deletecontactproviderRow = function (row) {
        $confirm({ text: 'Are you sure you want to delete ContactProvider "' + row.entity.LvisContactid + '" ?' }, { size: 'sm' })
         .then(function () {
             var index = $scope.vcontactprovidermap.serviceBranchGrid.data.indexOf(row.entity);

             $http.get('Contacts/Delete/' + row.entity.ContactId)
                .success(function (data) {
                    if (data == 0)
                        growl.error('Cannot Delete row (error in console)');
                    else {
                        $scope.vcontactprovidermap.serviceBranchGrid.data.splice(index, 1);
                        growl.success("The record was deleted successfully");
                    }
                });
         });
    };

}

psContactProviderMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psContactProviderMappingsRowEditor($http, $rootScope, $uibModal) {
    var service = {};
    service.editcontactprovider = editcontactprovider;
    service.addNewBranch = addNewBranch;
    function editcontactprovider(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psContactProviderMappings/contact-provider-mappings-edit.html',
            controller: 'psContactProviderMappingsRowEditCtrl',
            controllerAs: 'vcontactprovidermap',
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

    function addNewBranch(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psContactProviderMappings/contact-provider-mappings-add.html',
            controller: 'psContactProviderMappingsRowEditCtrl',
            controllerAs: 'vcontactprovidermap',
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

angular.module('psContactProviderMappings').controller('psContactProviderMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', 'psContactProviderMappingsApiUri',
function psContactProviderMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, psContactProviderMappingsApiUri) {
    var vcontactprovidermap = this;
    vcontactprovidermap.entity = angular.copy(row.entity);    
    vcontactprovidermap.save = save;
    vcontactprovidermap.LocationList = [];
    vcontactprovidermap.ContactIdList = [];
    vcontactprovidermap.LoadContactIdbylocationId = LoadContactIdbylocationId;

    if (vcontactprovidermap.entity.CustomerId != undefined && vcontactprovidermap.entity.CustomerId > 0) {
        $http.get('FASTOffices/GetLocationsListByCustId/' + vcontactprovidermap.entity.CustomerId)
            .success(function (data) {
                if (data != null && data != null) {
                        vcontactprovidermap.LocationList = data;                                  
                }                
                
            });
    }
    
    $http.get('Providers/GetProvidersForContactProvider/' + vcontactprovidermap.entity.TenantId).success(function (response) {
        vcontactprovidermap.ProviderList = response;
    });
    
    if (vcontactprovidermap.entity.ContactProviderMapId != '0')
        {
        $http.get('Security/GetContact/' +vcontactprovidermap.entity.LocationId)
                .success(function (data) {
                    vcontactprovidermap.ContactIdList = data;                   
                });
       }        


        function LoadContactIdbylocationId(locationId) {
        $http.get('Security/GetContact/' + locationId)
                .success(function (data) {
                    vcontactprovidermap.ContactIdList = data;                   
                });
    }
        function save() {
            if (row.entity.ContactId == "") {
                row.entity.ContactId = null;
            }
            row.entity = angular.extend(row.entity, vcontactprovidermap.entity);            
            if (vcontactprovidermap.entity.ContactProviderMapId == '0') {               
            //if (vcontactprovidermap.entity.ContactId == '0') {            
            $http.post(psContactProviderMappingsApiUri.AddContactProvider, row.entity)
                      .success(function (data) {                          
                          //real ID come back from response after the save in DB
                          vcontactprovidermap.entity.ContactProviderMapId = data.ContactProviderMapId;
                          if (vcontactprovidermap.entity.ContactProviderMapId == '0') {
                              growl.error("A similar records already exists for Customer ");                                                     
                          }
                          else {                              
                              row.entity = data;
                              grid.data.push(row.entity);
                              growl.success('A new record for Customer : "' + row.entity.CustomerName + '" was created successfully');
                          }
                      }).error(function (response) {                          
                          if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                              growl.error("Validation failed for one or more entities while Saving to DB");
                              return;
                          }
                          growl.error(response.ExceptionMessage);
                          return;
                      });
        }
        else
        {
            $http.post(psContactProviderMappingsApiUri.UpdateContactProvider, row.entity)
              .success(function (data) {
                  if (data.ContactProviderMapId == '0') {
                      growl.error('Cannot update record ' + row.entity.CustomerName + 'as a similar record already exists in the database ');
                  }
                  else {
                      row.entity = data;
                      grid.data = row.entity;
                      growl.success('The record was updated successfully');
                  }

              }).error(function (response) {                  
                  if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                      growl.error("Validation failed for one or more entities while Saving to DB");
                      return;
                  }
                  if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                      growl.error(row.entity.ProviderID + ' is a duplicate and cannot be added');
                      return;
                  }
                  growl.error(response.ExceptionMessage);
                  return;
              });            
        }
        $uibModalInstance.close(row.entity);
    }    
}]);

function psContactProviderMappingsApiUri() {
    var contactprovidermap = this;
    contactprovidermap.AddContactProvider = 'api/ContactProviderMappings/Post';

    contactprovidermap.UpdateContactProvider = 'api/ContactProviderMappings/UpdateContactProviderDetails';

}