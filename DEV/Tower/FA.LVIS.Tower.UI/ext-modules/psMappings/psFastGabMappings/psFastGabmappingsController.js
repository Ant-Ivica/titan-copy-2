"use strict";

angular.module('psfastgabmappings').controller('psfastgabmappingsController', psfastgabmappingsController);
angular.module('psfastgabmappings').service('psfastgabmappingsRowEditor', psfastgabmappingsRowEditor);
angular.module('psfastgabmappings').service('psfastgabmappingsApiUri', psfastgabmappingsApiUri);

psfastgabmappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psfastgabmappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies' ,'$route', '$routeParams'];
function psfastgabmappingsController($scope, $rootScope, $http, $interval, $uibModal, psfastgabmappingsRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies,  $route, $routeParams) {

    var vGab = this;

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

    var search = "";
    vGab.LocationName = ($routeParams.LocationName || "");

    if ($routeParams.LocationName.indexOf(":") > 0) {
        search = vGab.LocationName.split(":")[1];
    }

    if ($routeParams.LocationName.indexOf(":") > 1) {
        vGab.LocationLink = vGab.LocationName.split(":")[2] + ":" + vGab.LocationName.split(":")[3];
    }

    vGab.editFastGab = psfastgabmappingsRowEditor.editFastGab;
    vGab.addFastGabRow = psfastgabmappingsRowEditor.addFastGabRow;
   
    vGab.serviceFastGabGrid = {
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
        groupingShowAggregationMenu: 0,
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterCsvFilename: 'FastGabDetails.csv',
        columnDefs: [
            { field: 'LocationID', headerCellClass: 'grid-header', name: 'LocationId', displayName: 'Location ID', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'LocationName', headerCellClass: 'grid-header', name: 'Location Name', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Region', headerCellClass: 'grid-header', name: 'RegionId', displayName: 'Region ID', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'BusinessSourceABEID', headerCellClass: 'grid-header', name: 'BusinessSource Abeid', displayName: 'Business Source ABEID', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'NewLenderABEID', headerCellClass: 'grid-header', name: 'New Lender Abeid', displayName: 'New Lender ABEID', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'LoanType', headerCellClass: 'grid-header', name: 'Loan Type', displayName: 'Loan Type', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'FASTGABMapDesc', headerCellClass: 'grid-header', name: 'Description', enableCellEdit: false, groupingShowAggregationMenu: false },
           ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vGab.editFastGab(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vGab.serviceFastGabGrid.gridApi = gridApi;
        }
    };

    if ($rootScope.activityright) {
        $http.get('Customers/GetCustomerGabDetails/' + search).success(function (response) {
            vGab.serviceFastGabGrid.data = response;
        });
    }

    //Get ProgramList To Bind Dropdown
    vGab.StateFipsList = [];
    if ($rootScope.activityright) {
        $http.get('FilePreferences/GetStateFipsList').success(function (data) {
            vGab.StateFipsList = data
        });
    }


    $http.get('Security/GetTenant')
        .then(function (response) {
            $scope.Tenant = response.data;
        });
    $scope.disabled = true;//To
     
    vGab.LoadCounty = LoadCounty;

    var StateFips = vGab.StateFipsId;
    vGab.CountyFipsList = [];

    function LoadCounty(StateFips) {
        if (StateFips != null) {
            $http.get('FilePreferences/GetCountyFipsList/' + StateFips).success(function (data) {
                vGab.CountyFipsList = data;
                vGab.entity.CountyFipsId = '0'
                $scope.disabled = false;
            });
        }
    }

    vGab.ResetRefreshBotton = ResetRefreshBotton;
    function ResetRefreshBotton() {
        $scope.disabled = false;
    }

    // Reset Search
    vGab.RefreshSearch = RefreshSearch;
    function RefreshSearch() {
        vGab.entity.LoanAmount = 0;
        vGab.entity.StateFipsId = undefined;
        vGab.entity.CountyFipsId = undefined;
        vGab.entity.RegionId = undefined;
        vGab.CountyFipsList = [];
        $scope.disabled = true;
    }

    //Search FastGabMAp
    vGab.Search = Search;
    function Search() {
        vGab.Busy = true;        
        $http.get('Customers/GetFastGabDetatils/' + search + '/' +vGab.entity.StateFipsId + '/' +vGab.entity.CountyFipsId)
        .then(function (response) {
            vGab.serviceFastGabGrid.data = response.data;
            vGab.Busy = false;
        });
    }

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

    $scope.addFastGabRow = function () {
        var newService = {
            "ID": "0",
            "LocationID": search,
            "LocationName": vGab.LocationName.split(":")[0],
            "RegionID": "",
            "BusinessSourceABEID": "",
            "NewLenderABEID": "",
            "LoanTypeCodeId": "",
            "FASTGABMapDesc": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vGab.addFastGabRow($scope.vGab.serviceFastGabGrid, rowTmp);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vGab.serviceFastGabGrid.selectItem(index, false);
        $scope.vGab.serviceFastGabGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        $confirm({ text: 'Are you sure you want to delete External Provider "' + row.entity.ProviderID + '" ?' }, { size: 'sm' })
         .then(function () {
             var index = $scope.vGab.serviceFastGabGrid.data.indexOf(row.entity);

         });
    }
}


psfastgabmappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psfastgabmappingsRowEditor($http, $rootScope, $uibModal) {
    var service = {};
    service.editFastGab = editFastGab;
    service.addFastGabRow = addFastGabRow;

    function editFastGab(grid, row) {

        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastGabMappings/FastGab-mappings-edit.html',
            controller: 'psfastgabmappingsRowEditCtrl',
            controllerAs: 'vGab',
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

    function addFastGabRow(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastGabMappings/FastGab-mappings-add.html',
            controller: 'psfastgabmappingsRowEditCtrl',
            controllerAs: 'vGab',
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


angular.module('psfastgabmappings').controller('psfastgabmappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', 'psfastgabmappingsApiUri',
function psfastgabmappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, psfastgabmappingsApiUri) {

    var vGab = this;
    vGab.entity = angular.copy(row.entity);
    vGab.save = save;
    vGab.AddCondition = AddCondition;
    vGab.FormDirty = false;
    vGab.entity.Conditions = [];
    
    $http.get('Security/GetRegionsWithoutClaim/Fast').then(function (data) {
        vGab.RegionList = data.data;
    });

    //vGab.LoanTypeList = [];  
    $http.get('Customers/GetLoanTypeDetatils').then(function (data) {
        vGab.LoanTypeList = data.data;
    });

    //Get StateFips List
    vGab.StateFipsList = [];
    $http.get('FilePreferences/GetStateFipsList').success(function (data) {
        vGab.StateFipsList = data
    });
    vGab.LoadCounty = LoadCounty;
    vGab.CountyFipsList = [];

    function LoadCounty(StateFips) {
        if (StateFips != null) {
            $http.get('FilePreferences/GetCountyFipsList/' + StateFips).success(function (data) {
                vGab.CountyFipsList = data;
                vGab.county = vGab.CountyFipsList[0];
            });
        }
    }
    
    if (vGab.entity.FASTGABMapId != '0') {
        $http.get('Customers/GetCustomerGabMap/' + vGab.entity.FASTGABMapId).success(function (response) {
            vGab.entity = response;
        });
    }

    //Add state&County 
    function AddCondition() {
        var Condition = {
            PreferenceState: vGab.State, PreferenceCounty: vGab.county
        };
        for (var i = 0; i < vGab.entity.Conditions.length; i++) {
            if (vGab.entity.Conditions[i].PreferenceState.StateFIPS == vGab.State.StateFIPS &&
                vGab.entity.Conditions[i].PreferenceCounty.countyFIPS == vGab.county.countyFIPS) {
                return;
            }
        }
        vGab.entity.Conditions.push(Condition);
    }

    //Remove State & County
    vGab.Remove = function Remove(Condition) {
        vGab.FormDirty = true;
        var index = vGab.entity.Conditions.indexOf(Condition);
        if (index >= 0)
            vGab.entity.Conditions.splice(index, 1);

        if (vGab.entity.Conditions.length == 0) {

            vGab.State = undefined;
            vGab.county = undefined;
        }
    }
    //On change Enable/Disable Link
    vGab.Change = function Change() {
        vGab.FormDirty = true;
    }

    function save() {

        if (row.entity.ID === '0') {

            row.entity = angular.extend(row.entity, vGab.entity);

            $http.post(psfastgabmappingsApiUri.Add, row.entity)
            .success(function (data) {

                //real ID come back from response after the save in DB
                row.entity = data;
                grid.data.push(row.entity);


                if (data.ID === 0) {
                    growl.error('Cannot add FAST GAB detail record for ' + row.entity.LocationID);
                    return;
                }
                else {                    
                    growl.success("FAST GAB detail record for " + row.entity.LocationID + " was added successfully");
                }

            }).error(function (response) {
                if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                    growl.error("Error adding FAST GAB detail record");
                    return;
                }
                if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                    growl.error("FAST GAB detail record for " + row.entity.LocationID + " is a duplicate and cannot be added");
                    return;
                }

                growl.error(response.ExceptionMessage);
                return;
            });

        }
        else {

            row.entity = angular.extend(row.entity, vGab.entity);
            $http.post(psfastgabmappingsApiUri.Update, row.entity)
          .success(function (data) {
              row.entity = data;
              grid.data = row.entity;
              if (data === 0) {
                  growl.error('Cannot update FAST GAB detail record for ' + row.entity.LocationID);
                  return;
              }
              else {
                  growl.success('FAST GAB detail record for "' + row.entity.LocationID + '" was updated successfully');
              }

          }).error(function (response) {

              if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                  growl.error("Error updating FAST GAB detail record");
                  return;
              }
              if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                  growl.error(row.entity.LocationID + ' entries are duplicate and cannot be added');
                  return;
              }

              growl.error(response.ExceptionMessage);
              return;
          });
        }

        $uibModalInstance.close(row.entity);
    }


    vGab.remove = remove;
    function remove()
    {

        if (row.entity.ID !== '0') {
            row.entity = angular.extend(row.entity, vGab.entity);
            $http.post('Customers/DeleteFastGab', row.entity.FASTGABMapId)
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
                        if (row.entity.ID !== '0') {
                            row.entity = angular.extend(row.entity, vGab.entity);
                            $http.post('Customers/ConfirmDeleteFastGab', row.entity.FASTGABMapId)
                           .success(function (data) {
                               if (data === 1) {
                                   var index = grid.appScope.vGab.serviceFastGabGrid.data.indexOf(row.entity);
                                   grid.appScope.vGab.serviceFastGabGrid.data.splice(index, 1);
                                   growl.success("FAST GAB Info record was deleted successfully");
                                   return;
                               }
                               else {
                                   growl.error("There was an error deleting FAST GAB Info record");
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


function psfastgabmappingsApiUri() {
    var fastGapMap = this;
    fastGapMap.Add = 'api/CustomerMappings/AddFastGabDetails';
    fastGapMap.Update = 'api/CustomerMappings/updateFastGab';    
}



