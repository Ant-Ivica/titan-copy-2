"use strict";


angular.module('psProviderMappings').controller('psProviderMappingsController', psProviderMappingsController);
//angular.module('psProviderMappings').controller('psProviderMappingsRowEditCtrl', psProviderMappingsRowEditCtrl);
angular.module('psProviderMappings').service('psProviderMappingsRowEditor', psProviderMappingsRowEditor);
angular.module('psProviderMappings').service('psProviderMappingsApiUri', psProviderMappingsApiUri);

psProviderMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psProviderMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', '$confirm', 'UserInfo', '$q', '$cookies', 'growl', 'ModalImportProvider', 'ProviderMappingName'];
function psProviderMappingsController($scope, $rootScope, $http, $interval, $uibModal, psProviderMappingsRowEditor, uiGridGroupingConstants, $location, $window, $confirm, UserInfo, $q, $cookies, growl, ModalImportProvider, ProviderMappingName) {

    var vofficemap = this;

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

    vofficemap.editOfficeRow = psProviderMappingsRowEditor.editOfficeRow;
    vofficemap.addOfficeRow = psProviderMappingsRowEditor.addOfficeRow;
    vofficemap.removeRow = psProviderMappingsRowEditor.removeRow;
    
    //var detailOfficeButton = '<div ng-if="row.entity.IsUseRuleEngine == true"  class="ui-grid-cell-contents">Yes</div><div ng-if="row.entity.IsUseRuleEngine == false" class="ui-grid-cell-contents">No</div>'
    var branchdocevent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <a ng-show="row.entity.IsUseRuleEngine && (row.treeNode.children && row.treeNode.children.length == 0)"   class="fa fa-cog" tooltip-placement="bottom" uib-tooltip="Rules Engine" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showRuleENgine(row)">' + 
                         '<a ng-show="!row.entity.IsUseRuleEngine && (row.treeNode.children && row.treeNode.children.length == 0)"  class="fa fa-map" tooltip-placement="bottom" uib-tooltip="FAST Office Map" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showFastOffice(row)">' + 
                         '<a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)"  class="fa fa-list-alt" tooltip-placement="bottom" uib-tooltip="Product Provider" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showProductProvider(row)"></a></div>'

    vofficemap.serviceOfficeGrid = {
        enableFiltering: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: true,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        enableRowHeaderSelection: false,
        multiSelect: false,
        enableSorting: true,
        enableGridMenu: true,
        enableSelectAll: true,
        paginationPageSizes: [15, 30, 45],
        paginationPageSize: 15,
        minRowsToShow: 16,
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterCsvFilename: 'OfficesInfo.csv',
        columnDefs: [
            { field: 'ProviderID', headerCellClass: 'grid-header', name: 'Provider ID', displayName: 'Provider ID', enableCellEdit: false, groupingShowAggregationMenu: false },
               { field: 'ProviderName', headerCellClass: 'grid-header', name: 'Provider Name', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ExternalID', headerCellClass: 'grid-header', name: 'External ID', displayName: 'External ID', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'InternalApplication', headerCellClass: 'grid-header', name: 'Internal Application', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ExternalApplication', headerCellClass: 'grid-header', name: 'External Application', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'UseRuleEngine',headerCellClass: 'grid-header', name: 'Use Rule Engine', displayName: 'Use Rule Engine', visible: false, enableCellEdit: false, groupingShowAggregationMenu: false },
            { field:  'Tenant', headerCellClass: 'grid-header', name: 'Tenant', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'remove', name: '', headerCellClass: 'grid-header', enableColumnMenu: false, enableFiltering: false, groupingShowAggregationMenu: false, cellTemplate: branchdocevent }
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vofficemap.editOfficeRow(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vofficemap.serviceOfficeGrid.gridApi = gridApi;
        }
    };
  
    if ($rootScope.activityright) {
        $http.get('Providers/GetProviders/').success(function (response) {

            //Set Filter to ExternalID when re-navigatelink from Fast Office Mappings by clicking on externalId.
            var url = $location.path().split('/');
            var ExternalID = url[2];

            if (ExternalID != null) {
                var indexpos = $scope.vofficemap.serviceOfficeGrid.columnDefs.map(function (e) { return e.field; }).indexOf('ExternalID');

                indexpos = (indexpos != null) ? parseInt(indexpos) + 1 : null;

                if (indexpos != null) {
                    vofficemap.serviceOfficeGrid.gridApi.grid.columns[indexpos].filters[0] = {
                        term: ExternalID
                    };
                }
            }

            vofficemap.serviceOfficeGrid.data = response;
        });
    }

    $http.get('Security/GetTenant')
   .then(function (response) {
       $scope.Tenant = response.data;
   });

    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.vofficemap.serviceOfficeGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname == 'LVIS')
        $scope.vofficemap.serviceOfficeGrid.columnDefs[pos].visible = true;
    else
        $scope.vofficemap.serviceOfficeGrid.columnDefs[pos].visible = false;

  
    $scope.expandAll = function () {
        $scope.gridApi.treeBase.expandAllRows();
    };

    $scope.toggleRow = function (rowNum) {
        $scope.gridApi.treeBase.toggleRowTreeState($scope.gridApi.grid.renderContainers.body.visibleRowCache[rowNum]);
    };

    $scope.changeGrouping = function () {
        $scope.gridApi.grouping.clearGrouping();
        $scope.gridApi.grouping.groupColumn('ExternalId');
    };

    $scope.addOfficeRow = function () {
        var newService = {
            "ProviderID": 0,
            "ProviderName":"",
            "ServiceProviderId":"",
            "ExternalID": "",
            "ExternalApplication": "",
            "InternalApplication": "",
            "IsUseRuleEngine": "false",
            "TenantId": "",
            "UseRuleEngine":""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vofficemap.addOfficeRow($scope.vofficemap.serviceOfficeGrid, rowTmp);


    };

    $scope.ImportProvider = function () {
        ModalImportProvider.openPopupModal($scope.vofficemap.serviceOfficeGrid);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vofficemap.serviceOfficeGrid.selectItem(index, false);
        $scope.vofficemap.serviceOfficeGrid.splice(index, 1);
    };

    $scope.showFastOffice = function (row) {

        $window.location = "#/fastofficemappings/" + row.entity.ExternalID + ":" + row.entity.ProviderID;
    };

    $scope.showProductProvider = function (row) {
        if (row.entity.ProviderName != null && row.entity.ProviderName != '' && row.entity.ProviderName != undefined) {
            $scope.ProviderNameExtension = row.entity.ProviderName + " (" + row.entity.ExternalID + ")";
        } else { $scope.ProviderNameExtension = row.entity.ProviderID + " (" + row.entity.ExternalID + ")"; }

        ProviderMappingName.ProviderNameExtension = $scope.ProviderNameExtension;

        $window.location = "#/productprovidermappings/" + row.entity.ProviderID + ":" + row.entity.ExternalID;
    };

    $scope.deleteRow = function (row) {
        $confirm({ text: 'Are you sure you want to delete External Provider "' + row.entity.ProviderID + '" ?' }, { size: 'sm' })
         .then(function () {
             var index = $scope.vofficemap.serviceOfficeGrid.data.indexOf(row.entity);

             $http.get('Providers/Delete/' + row.entity.ID)
                .success(function (data) {
                    if (data == 0)
                        growl.error('Cannot delete row (error in console)');
                    else {
                        $scope.vofficemap.serviceOfficeGrid.data.splice(index, 1);
                        growl.success("Provider info record was deleted successfully");
                    }
                });
         });
    }
}

psProviderMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psProviderMappingsRowEditor($http, $rootScope, $uibModal) {
    var service = {};
    service.editOfficeRow = editOfficeRow;
    service.addOfficeRow = addOfficeRow;

    function editOfficeRow(grid, row) {

        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psProviderMappings/provider-mappings-edit.html',
            controller: 'psProviderMappingsRowEditCtrl',
            controllerAs: 'vofficemap',
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

    function addOfficeRow(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psProviderMappings/provider-mappings-add.html',
            controller: 'psProviderMappingsRowEditCtrl',
            controllerAs: 'vofficemap',
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


angular.module('psProviderMappings').controller('psProviderMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl','$confirm', 'psProviderMappingsApiUri',
function psProviderMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, psProviderMappingsApiUri) {

    var vofficemap = this;
    vofficemap.entity = angular.copy(row.entity);
    vofficemap.entity.IsUseRuleEngine = row.entity.IsUseRuleEngine;
    vofficemap.save = save;
    vofficemap.AddCondition = AddCondition;
    if (vofficemap.entity.LocationCondition == undefined)
        vofficemap.entity.LocationCondition = [];

    if (vofficemap.entity.Tenant == '') {
        vofficemap.entity.Tenant = $scope.tenantname;
    }
    
   

    vofficemap.ExternalApplicationList = [];
    vofficemap.InternalApplicationList = [];
    $http.get('Security/GetExternalApplications').success(function (data) {
        vofficemap.ExternalApplicationList = data;
    });
    $http.get('Security/GetInternalApplications').success(function (data) {
        vofficemap.InternalApplicationList = data;
    });

    vofficemap.StateFipsList = [];   
    $http.get('FilePreferences/GetStateFipsList').success(function (data) {
        vofficemap.StateFipsList = data
    });    

    vofficemap.LoadCounty = LoadCounty;
    vofficemap.CountyFipsList = [];

    if (vofficemap.entity.ProviderID != '0') {
        $http.get('Providers/GetProviderDetailsByID/' + vofficemap.entity.ProviderID).success(function (data) {
            vofficemap.entity = data;
        });
 

          
    }


    vofficemap.TenantList = [];
    $http.get('FilePreferences/GetTenantList').then(function (response) {
        vofficemap.TenantList = response.data;
        ////vofficemap.TenantList = vofficemap.TenantList.filter((item) => item.TenantName !== 'LVIS');
        vofficemap.TenantList.forEach(function (data, index) {
            if (data.TenantName == 'LVIS') {
                vofficemap.TenantList.splice(index, 1);
            }
        })
   });

    function LoadCounty(StateFips) {
        if (StateFips != null) {
            $http.get('FilePreferences/GetCountyFipsList/' + StateFips).success(function (data) {
                vofficemap.CountyFipsList = data;
                vofficemap.county = vofficemap.CountyFipsList[0];
            });
        }
    }

    function AddCondition() {
        var Condition = {
            PreferenceState: vofficemap.State, PreferenceCounty: vofficemap.county
        };

        for (var i = 0; i < vofficemap.entity.LocationCondition.length; i++) {
            if (vofficemap.entity.LocationCondition[i].PreferenceState.StateFIPS == vofficemap.State.StateFIPS &&
                vofficemap.entity.LocationCondition[i].PreferenceCounty.countyFIPS == vofficemap.county.countyFIPS) {
                return;
            }
        }
        vofficemap.entity.LocationCondition.push(Condition);
    }

    vofficemap.Remove = function Remove(Condition) {
        vofficemap.FormDirty = true;
        var index = vofficemap.entity.LocationCondition.indexOf(Condition);                                                
        if (index >= 0)
            vofficemap.entity.LocationCondition.splice(index, 1);

        if (vofficemap.entity.LocationCondition.length == 0) {

            vofficemap.State = undefined;
            vofficemap.county = undefined;

        }
    }

    function save() {
        if (row.entity.ProviderID == '0') {
            row.entity = angular.extend(row.entity, vofficemap.entity);
            $http.post(psProviderMappingsApiUri.addProvider, row.entity)
                  .success(function (data) {

                      //real ID come back from response after the save in DB
                      row.entity = data;                      

                      if (data.length == 0) {
                          growl.error('A record with the External Provider ID: "' + row.entity.ExternalID + '" cannot be added');
                          return;
                      }
                      else {
                          grid.data.push(row.entity);
                          growl.success('A new record for External Provider ID: "' + row.entity.ExternalID + '" was created successfully');
                      }
                  }).error(function (response) {


                      if (response.ExceptionMessage.indexOf("Provider Name already exists") >= 0) {
                          growl.error(row.entity.ProviderID + "Cannot be saved as Provider Name already exists");
                          return;
                      }

                      if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                          growl.error("Validation failed for one or more entities while Saving to DB");
                          return;
                      }
                      if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                          growl.error(row.entity.ProviderID + ' is a duplicate and cannot be added');
                          return;
                      }

                      growl.error('A record with the External Provider ID: "' + row.entity.ProviderID + '" cannot be added');
                      return;
                  });
        }
        else {

            $http.post(psProviderMappingsApiUri.updateProvider, vofficemap.entity)
          .success(function (data) {
              if (data.length == 0) {
                  growl.error('There was an error updating External Provider ID: "' +  vofficemap.entity.ExternalID);
                  return;
              }
              else {

                  row.entity = angular.extend(row.entity, vofficemap.entity);
                  row.entity = data;
                  grid.data = row.entity;
                  growl.success('The record for "' + row.entity.ExternalID + '" was updated successfully');
              }

          }).error(function (response) {

              if (response.ExceptionMessage.indexOf("Provider Name already exists") >= 0) {
                  growl.error(row.entity.ProviderID + " cannot saved as Provider Name already exists ");
                  return;
              }

              if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                  growl.error("Validation failed for one or more entities while Saving to DB");
                  return;
              }
              if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                  growl.error(row.entity.ProviderID + ' is a duplicate and cannot be added');
                  return;
              }

              growl.error(response.InnerException.InnerException.ExceptionMessage);
              return;
          });

        }

        $uibModalInstance.close(row.entity);
    }

    vofficemap.remove = remove;

    function remove()
    {
        if (row.entity.ProviderID != '0') {
            row.entity = angular.extend(row.entity, vofficemap.entity);
            $http.post('Providers/DeleteProvider', row.entity.ProviderID)
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
                        if (row.entity.ProviderID != '0') {
                         row.entity = angular.extend(row.entity, vofficemap.entity);
                            $http.post('Providers/ConfirmDeleteProvider', row.entity.ProviderID)
                           .success(function (data) {
                               if (data === 1) {
                                   var index = grid.appScope.vofficemap.serviceOfficeGrid.data.indexOf(row.entity);
                                   grid.appScope.vofficemap.serviceOfficeGrid.data.splice(index, 1);
                                   growl.success("Provider info record was deleted successfully");
                                   return;
                               }
                               else {
                                   growl.error("There was an error deleting Provider Info record for: " + row.entity.ProviderID);
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

function psProviderMappingsApiUri()
{
    var providerMap = this;

    providerMap.addProvider = 'api/ProviderMappings/Post';

    providerMap.updateProvider = 'api/ProviderMappings/UpdateProviderDetails';
}

