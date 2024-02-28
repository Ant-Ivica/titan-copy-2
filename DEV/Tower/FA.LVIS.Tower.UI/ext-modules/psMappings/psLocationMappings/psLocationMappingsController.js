"use strict";


angular.module('psLocationMappings').controller('psLocationMappingsController', psLocationMappingsController);
angular.module('psLocationMappings').service('psLocationMappingsRowEditor', psLocationMappingsRowEditor);
angular.module('psLocationMappings').controller('psLocationMappingsBulkImportCtrl', psLocationMappingsBulkImportCtrl);
angular.module('psLocationMappings').service('psLocationMappingsBulkImport', psLocationMappingsBulkImport);

psLocationMappingsController.$inject = ['$scope', '$http', '$interval', '$uibModal', 'psLocationMappingsRowEditor', 'psLocationMappingsBulkImport', 'uiGridGroupingConstants', '$location', '$route', '$routeParams', 'growl', 'UserInfo', '$rootScope', '$confirm', '$cookies', '$window', 'ServicePreferenceMapping'];
function psLocationMappingsController($scope, $http, $interval, $uibModal, psLocationMappingsRowEditor, psLocationMappingsBulkImport, uiGridGroupingConstants, $location, $route, $routeParams, growl, UserInfo, $rootScope, $confirm, $cookies, $window, ServicePreferenceMapping) {
    var vbranchmap = this;

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

    vbranchmap.editbranch = psLocationMappingsRowEditor.editbranch;
    vbranchmap.addNewBranch = psLocationMappingsRowEditor.addNewBranch;
    vbranchmap.removebranchRow = psLocationMappingsRowEditor.removebranchRow;
    vbranchmap.bulkImportLocations = psLocationMappingsBulkImport.bulkImportLocations;

    var search = "";
    vbranchmap.CustomerName = ($routeParams.CustomerName || "");

    if ($routeParams.CustomerName.indexOf(":") > 0)
    {
        search = vbranchmap.CustomerName.split(":")[1];
        vbranchmap.CustomerNameLink = vbranchmap.CustomerName.split(":")[0];
    }

    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deletebranchRow(row)"></i></div>'
    var Locationdocevent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents">' +
                            '<a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-map" tooltip-placement="bottom" uib-tooltip="FAST GAB Map" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.ShowGabDetails(row)"></a>' +
                            '<a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-user" tooltip-placement="bottom" uib-tooltip="Contacts" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.ShowBranches(row)"></a></div>'
    vbranchmap.serviceBranchGrid = {
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
        exporterCsvFilename: 'Locations.csv',         
        ////importerDataAddCallback: function (grid, newObjects) {
            
        ////    //$scope.data = $scope.data.concat(newObjects);
        ////    var object = newObjects;            
        ////    //vbranchmap.serviceBranchGrid.data = vbranchmap.serviceBranchGrid.data.concat(newObjects);          
        ////    $http.post('Locations/BulkImport', object)
        ////           .success(function (data) {                       
        ////               row.entity = $scope.data;
        ////               grid.data.push(row.entity);                      
        ////           });
        ////},
        columnDefs: [
        { field: 'CustomerName', name: 'Customer Name', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'ExternalId', name: 'External ID', displayName: 'External ID', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'LocationName', name: 'Location Name', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'CustomerId', name: 'CustomerId', visible: false, headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'LocationID', name: 'LocationID', visible: false, headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'RegionID', name: 'RegionID', visible: false, headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'BusinessSourceABEID', visible: false, name: 'BusinessSourceABEID', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'NewLenderABEID', visible: false, name: 'NewLenderABEID', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'Description', visible: false, name: 'Description', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
        { field: 'FastGab', name: '  ', headerCellClass: 'grid-header', enableColumnMenu: false, enableFiltering: false, groupingShowAggregationMenu: false, cellTemplate: Locationdocevent }
        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vbranchmap.editbranch(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vbranchmap.serviceBranchGrid.gridApi = gridApi;
        }
    };

    if ($rootScope.activityright) {
        $http.get('Locations/GetLocations/' + search).success(function (response) {
            vbranchmap.serviceBranchGrid.data = response;
        });
    }

    $scope.Tenant ='';
    $http.get('Security/GetTenant')
      .then(function (response) {
          $scope.Tenant = response.data;
      });

    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.vbranchmap.serviceBranchGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname == 'LVIS' || $scope.Tenant == 'LVIS')
        $scope.vbranchmap.serviceBranchGrid.columnDefs[pos].visible = true;
    else
        $scope.vbranchmap.serviceBranchGrid.columnDefs[pos].visible = false;

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
        $scope.gridApi.grouping.groupColumn('ExternalId');
        //$scope.gridApi.grouping.aggregateColumn('state', uiGridGroupingConstants.aggregation.COUNT);
    };
    
    $scope.addNewBranch = function () {
        var newService = {
            "LocationId": "0",
            "ExternalId": "",
            "LocationName": "",
            "CustomerId": search,
            "CustomerName": vbranchmap.CustomerName.split(":")[0],
            "TenantId": "",
            "Tenant": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vbranchmap.addNewBranch($scope.vbranchmap.serviceBranchGrid, rowTmp);
    };

    $scope.bulkImportLocations = function () {
        var newService = {
            "LocationId": "0",
            "ExternalId": "",
            "LocationName": "",
            "CustomerId": search,
            "CustomerName": vbranchmap.CustomerName.split(":")[0],

        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vbranchmap.bulkImportLocations($scope.vbranchmap.serviceBranchGrid, rowTmp);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vbranchmap.serviceBranchGrid.selectItem(index, false);
        $scope.vbranchmap.serviceBranchGrid.splice(index, 1);
    };

    $scope.deletebranchRow = function (row) {
        $confirm({ text: 'Are you sure you want to delete Location "' + row.entity.LocationName + '" ?' }, { size: 'sm' })
         .then(function () {
             var index = $scope.vbranchmap.serviceBranchGrid.data.indexOf(row.entity);

             $http.get('Locations/Delete/' + row.entity.LocationId)
                .success(function (data) {
                    if (data == 0)
                        growl.error('Cannot Delete row (error in console)');
                    else {
                        $scope.vbranchmap.serviceBranchGrid.data.splice(index, 1);
                        growl.success("The record was deleted successfully");
                    }
                });
         });
    };

    $scope.ShowGabDetails = function (row) {

        $window.location = "#/fastgabmappings/" + row.entity.LocationName + ":" + row.entity.LocationId + ":" + vbranchmap.CustomerName;
    };

    $scope.ShowBranches = function (row) {      
        ServicePreferenceMapping.CustomerId = row.entity.CustomerId;
        $window.location = "#/Contactmappings/" + row.entity.LocationName + ":" + row.entity.LocationId + ":" + vbranchmap.CustomerName;
    };    
}

psLocationMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psLocationMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editbranch = editbranch;
    service.addNewBranch = addNewBranch;

    function editbranch(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psLocationMappings/location-mappings-edit.html',
            controller:  'psLocationMappingsRowEditCtrl',
            controllerAs: 'vbranchmap',
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
            templateUrl: 'ext-modules/psMappings/psLocationMappings/location-mappings-add.html',
            controller: 'psLocationMappingsRowEditCtrl',
            controllerAs: 'vbranchmap',
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


angular.module('psLocationMappings').controller('psLocationMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', 'ServicePreferenceMapping',
function psLocationMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, ServicePreferenceMapping) {
    var vbranchmap = this;
    $scope.ServicePreferenceShow = false;
    vbranchmap.entity = angular.copy(row.entity);
    vbranchmap.entity.ServicePreference = vbranchmap.entity.CopyServicePrefernce;
    vbranchmap.save = save;
    vbranchmap.ServicePrefrences = [];
    vbranchmap.entity.ServicePreference = [];
    vbranchmap.ServicePreferenceMap = [];
    vbranchmap.ServicePreferenceMapCustomer = [];
    $scope.InternalApplicationId = ServicePreferenceMapping.InternalApplicationId;

    var getIndexIfObjWithOwnAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }

    if (vbranchmap.entity.Tenant == '') {
        vbranchmap.entity.Tenant = $scope.tenantname;
    }

    if ($scope.InternalApplicationId === 9)
    { $scope.ServicePreferenceShow = true; }
    else { $scope.ServicePreferenceShow = false; }

    $http.get('InboundDocs/GetServices').success(function (data) {
        if (data.length > 0) {
            data.splice(1, 1) //remove Signing service 
        }
        vbranchmap.ServicePrefrences = data;
    });

    if (vbranchmap.entity.LocationId != '0' && $scope.InternalApplicationId === 9) {
         $http.get('Locations/GetServicePreferenceLocation/' + vbranchmap.entity.LocationId).success(function (data) {
             vbranchmap.ServicePreferenceMap = data;
             $scope.ServicePreferenceShow = true;
            if (vbranchmap.ServicePreferenceMap != null) {
                for (var i = 0; i < vbranchmap.ServicePreferenceMap.length; i++) {
                    var tmp = getIndexIfObjWithOwnAttr(vbranchmap.ServicePrefrences, "ID", vbranchmap.ServicePreferenceMap[i].ID);
                    if (tmp > -1)
                        vbranchmap.ServicePrefrences[tmp].Ticked = true;
                }
            }
         });
    }

    function save() {
        if (vbranchmap.entity.LocationId == '0') {
            row.entity = angular.extend(row.entity, vbranchmap.entity);
            $http.post('Locations', row.entity)
                  .success(function (data) {
                      //real ID come back from response after the save in DB
                      row.entity = data;
                      grid.data.push(row.entity);

                      if (data.length == 0) {
                          growl.error('Cannot add ' + row.entity.ExternalId);
                          return;
                      }
                      else {
                          //grid.data = data;
                          growl.success('A new record for External Location ID: "' + row.entity.ExternalId + '" was created successfully');
                      }

                  }).error(function (response) {

                      if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                          growl.error("Validation failed for one or more entities while Saving to DB");
                          return;
                      }
                      if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                          growl.error(row.entity.ExternalId + ' is a duplicate and cannot be added');
                          return;
                      }

                      growl.error(response.ExceptionMessage);
                      return;
                  });

        } else {

            row.entity = angular.extend(row.entity, vbranchmap.entity);
            $http.post('Locations/Update', row.entity)
          .success(function (data) {
              if (data == 0) {
                  growl.error('Cannot update ' + row.entity.ExternalId);
                  return;
              }
              else {
                  growl.success('The record for "' + row.entity.ExternalId + '" was updated successfully');
              }

          }).error(function (response) {

              if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                  growl.error("Validation failed for one or more entities while Saving to DB");
                  return;
              }
              if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                  growl.error(row.entity.ExternalId + ' is a duplicate and cannot be added');
                  return;
              }

              growl.error(response.ExceptionMessage);
              return;
          });

        }
        $uibModalInstance.close(row.entity);
    }

    if (vbranchmap.entity.LocationId === '0' && $scope.InternalApplicationId === 9) {
        $http.get('Customers/GetServicePreferenceCustomer/' + vbranchmap.entity.CustomerId).success(function (data) {
            vbranchmap.ServicePreferenceMapCustomer = data;
            if (vbranchmap.ServicePreferenceMapCustomer != null) {
                for (var i = 0; i < vbranchmap.ServicePreferenceMapCustomer.length; i++) {
                    var tmp = getIndexIfObjWithOwnAttr(vbranchmap.ServicePrefrences, "ID", vbranchmap.ServicePreferenceMapCustomer[i].ID);
                    if (tmp > -1)
                        vbranchmap.ServicePrefrences[tmp].Ticked = true;
                }
            }
        });
    }

    vbranchmap.remove = remove;
    function remove()
    {
        if (row.entity.LocationId != '0') {
             row.entity = angular.extend(row.entity, vbranchmap.entity);
             $http.post('Locations/Delete', row.entity.LocationId)
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
                        if (row.entity.LocationId != '0') {
                            row.entity = angular.extend(row.entity, vbranchmap.entity);
                            $http.post('Locations/ConfirmDeleteLocation', row.entity.LocationId)
                           .success(function (data) {
                               if (data === 1) {
                                   var index = grid.appScope.vbranchmap.serviceBranchGrid.data.indexOf(row.entity);
                                   grid.appScope.vbranchmap.serviceBranchGrid.data.splice(index, 1);
                                   growl.success("Location Info was Deleted successfully");
                                   return;
                               }
                               else {
                                   growl.error("There was an error deleting Location Info record for: " + row.entity.LocationName);
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


psLocationMappingsBulkImport.$inject = ['$http', '$rootScope', '$uibModal'];
function psLocationMappingsBulkImport($http, $rootScope, $uibModal) {

    var service = {};
    service.bulkImportLocations = bulkImportLocations;

    function bulkImportLocations(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psLocationMappings/location-mappings-bulk-import.html',
            controller: ['$http', '$uibModalInstance', 'grid', 'row', '$q', '$timeout', '$window', 'growl', '$scope', '$confirm', psLocationMappingsBulkImportCtrl],
            controllerAs: 'vbranchmappopup',
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

angular.module('psLocationMappings')
 .directive("fileexcelread", [function () {
     return {
         scope: {
             opts: '='
         },
         link: function ($scope, $elm, $attrs) {
             $elm.on('change', function (changeEvent) {
                 var reader = new FileReader();

                 reader.onload = function (evt) {
                     $scope.$apply(function () {
                         $scope.opts.Datacount = 0;
                         var binary = "";
                         var bytes = new Uint8Array(evt.target.result);
                         var length = bytes.byteLength;

                         for (var i = 0; i < length; i++) {
                             binary += String.fromCharCode(bytes[i]);
                         }

                         var workbook = XLSX.read(binary, { type: 'binary' });

                         var data = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]], { header: 1 });

                         $scope.opts.columnDefs = [];

                         for (var i = 0; i < 8; i++) {
                             $scope.Header = data[7][i];
                             //$scope.myString2 = $scope.myString1.replace(/ /g, "");

                             $scope.SHeader = $scope.Header.replace(/ /g, "");
                             $scope.SHeader = $scope.SHeader.replace("?", "");
                             $scope.CustomHeader = $scope.SHeader.replace("*", "");

                             $scope.opts.columnDefs.push({ field: $scope.CustomHeader });
                         }

                         $scope.opts.data = [];
                         $scope.opts.Message = 'There are few Records Missing with Mandatory Filed.';

                         for (var i = 8; i < data.length; i++) {
                             var newRow = {};
                             var validate = true;

                             for (var columnIndex = 0; columnIndex < 8; columnIndex++) {

                                 if ($scope.opts.columnDefs[columnIndex].field == "Description" && data[i][columnIndex] == undefined) {
                                     newRow[$scope.opts.columnDefs[columnIndex].field] = '';
                                     continue;
                                 } else if ($scope.opts.columnDefs[columnIndex].field == "NewLenderABEID" && data[i][columnIndex] == undefined) {
                                     newRow[$scope.opts.columnDefs[columnIndex].field] = '';
                                     continue;
                                 } else if ($scope.opts.columnDefs[columnIndex].field == "LoanType" && data[i][columnIndex] == undefined) {
                                     newRow[$scope.opts.columnDefs[columnIndex].field] = '';
                                     continue;
                                 } else if ($scope.opts.columnDefs[columnIndex].field == "DefaultFASTFileNotes" && data[i][columnIndex] == undefined) {
                                     newRow[$scope.opts.columnDefs[columnIndex].field] = '';
                                     continue;
                                 } else if (data[i][columnIndex] == undefined)
                                     validate = false;

                                 newRow[$scope.opts.columnDefs[columnIndex].field] = data[i][columnIndex];
                             }

                             if (validate)
                                 $scope.opts.data.push(newRow);
                             else
                                 //$scope.opts.Message += i + ","
                                 $scope.opts.Message;
                         }
                         $elm.val(null);
                     });
                 };

                 reader.readAsArrayBuffer(changeEvent.target.files[0]);
             });
         }
     }
 }]);


function psLocationMappingsBulkImportCtrl($http, $uibModalInstance, grid, row, $q, $timeout, $window, growl, $scope, $confirm) {
    var vbranchmappopup = this;
    ////vbranchmap.entity = angular.copy(row.entity);
    ////vbranchmap.save = save;
    vbranchmappopup.gridOptions = {};
    vbranchmappopup.loading = false;

    vbranchmappopup.reset = reset;
    vbranchmappopup.save = save;
    vbranchmappopup.upload = upload;

    function reset() {
        vbranchmappopup.gridOptions.data = [];
        vbranchmappopup.gridOptions.columnDefs = [];
        vbranchmappopup.gridOptions.Message = '';
        vbranchmappopup.loading = false;
    }

    //upload();
    function save()
    {
        vbranchmappopup.gridOptions.data.forEach(function (item) {
            item.Name = item.LocationName;
            item.CustomerId = row.entity.CustomerId;
            item.Notes = item.DefaultFASTFileNotes;
        });

        $http.post('Locations/BulkImport', vbranchmappopup.gridOptions.data)
         .success(function (data) {

             ////grid.data = data;
             if (data.length == 3 || data == null ) {
                 ////$scope.opts.Message = 'Upload failed.Please validate the data';
                 ////return;
                 var uniqueCount, updatedCount, failedCount;

                 uniqueCount = data[0].split(":");
                 updatedCount = data[1].split(":");
                 failedCount = data[2].split(":");

                 if(uniqueCount[1] > 0)
                     growl.success(uniqueCount[1] + " records inserted successfully");

                 if(updatedCount[1] > 0)
                     growl.success(updatedCount[1] + " records updated successfully");

                 growl.error("Due to Invalid data " + failedCount[1] + " records failed to upload");


                 $http.get('Locations/GetLocations/' + row.entity.CustomerId).success(function (response) {
                     grid.data = response;
                 });
             }
             else if (data.length == 2) {

                 ////growl.success('Records uploaded successfully');
                 var uniqueCount, updatedCount;
                 
                 uniqueCount = data[0].split(":");
                 updatedCount = data[1].split(":");

                 if(uniqueCount[1] > 0)
                    growl.success(uniqueCount[1] + " records inserted successfully");

                 if(updatedCount[1] > 0)
                    growl.success(updatedCount[1] + " records updated successfully");

                 $http.get('Locations/GetLocations/' + row.entity.CustomerId).success(function (response) {
                     grid.data = response;
                 });
             }
             
         }).finally(function () {
             vbranchmappopup.loading = false;
             $uibModalInstance.close();
         })
        ;

    }

    // Reset the data source in a timeout so we can see the loading message
    function upload() {
        vbranchmappopup.loading = true;

        $timeout(function () {
            save();
        }, 1000);
    }
}


