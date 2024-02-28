"use strict";


angular.module('psContactMappings').controller('psContactMappingsController', psContactMappingsController);
angular.module('psContactMappings').service('psContactMappingsRowEditor', psContactMappingsRowEditor);
angular.module('psContactMappings').service('psContactMappingsApiUri', psContactMappingsApiUri);

psContactMappingsController.$inject = ['$scope', '$http', '$interval', '$uibModal', 'psContactMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$route', '$routeParams', 'growl', 'UserInfo', '$rootScope', '$confirm', '$cookies', '$window', 'ServicePreferenceMapping'];
function psContactMappingsController($scope, $http, $interval, $uibModal, psContactMappingsRowEditor, uiGridGroupingConstants, $location, $route, $routeParams, growl, UserInfo, $rootScope, $confirm, $cookies, $window, ServicePreferenceMapping) {
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

    vbranchmap.editbranch = psContactMappingsRowEditor.editbranch;
    vbranchmap.addNewBranch = psContactMappingsRowEditor.addNewBranch;

    var search = "";
    vbranchmap.LocationName = ($routeParams.LocationName || "");

    if ($routeParams.LocationName.indexOf(":") > 0)
        search = vbranchmap.LocationName.split(":")[1];

    if ($routeParams.LocationName.indexOf(":") > 1) {
        vbranchmap.LocationLink = vbranchmap.LocationName.split(":")[2] + ":" + vbranchmap.LocationName.split(":")[3];
    }

    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deletebranchRow(row)"></i></div>'

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
        exporterCsvFilename: 'ContactInfo.csv',
        
        columnDefs: [        
        { field: 'LocationId', name: 'Location ID', displayName: 'Location ID', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'LocationName', name: 'Location Name', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'LvisContactid', name: 'Contact Id', displayName: 'Contact ID', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'IsActive', name: 'Active', headerCellClass: 'grid-header', enableCellEdit: false },
        { field: 'ContactId', name: 'Unique Contact Id', visible: false, headerCellClass: 'grid-header', enableCellEdit: false }
        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vbranchmap.editbranch(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vbranchmap.serviceBranchGrid.gridApi = gridApi;
        }
    };

    if ($rootScope.activityright) {
        $http.get('Security/GetContact/' + search).then(function (response) {
            if (response != null && response.data != null) {
                var contactInfo = [];
                response.data.forEach(function (val) {
                    val.LocationId = search,
                    val.LocationName = vbranchmap.LocationName.split(":")[0],
                    val.IsActive = (val.IsActive == true) ? 'Yes' : 'No'
                    contactInfo.push(val);
                })
                vbranchmap.serviceBranchGrid.data = contactInfo;
            }
                
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
            "LocationId": search,
            "LocationName": vbranchmap.LocationName.split(":")[0],
            "LvisContactid": "",
            "IsActive": "",
            "ContactId": "0",
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vbranchmap.addNewBranch($scope.vbranchmap.serviceBranchGrid, rowTmp);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vbranchmap.serviceBranchGrid.selectItem(index, false);
        $scope.vbranchmap.serviceBranchGrid.splice(index, 1);
    };

    $scope.deletebranchRow = function (row) {
        $confirm({ text: 'Are you sure you want to delete Contact "' + row.entity.LvisContactid + '" ?' }, { size: 'sm' })
         .then(function () {
             var index = $scope.vbranchmap.serviceBranchGrid.data.indexOf(row.entity);

             $http.get('Contacts/Delete/' + row.entity.ContactId)
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

}

psContactMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psContactMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editbranch = editbranch;
    service.addNewBranch = addNewBranch;

    function editbranch(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psContactMappings/contact-mappings-edit.html',
            controller: 'psContactMappingsRowEditCtrl',
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
            templateUrl: 'ext-modules/psMappings/psContactMappings/contact-mappings-add.html',
            controller: 'psContactMappingsRowEditCtrl',
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

angular.module('psContactMappings').controller('psContactMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', 'psContactMappingsApiUri', 'ServicePreferenceMapping',
function psContactMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, psContactMappingsApiUri, ServicePreferenceMapping) {

    var vbranchmap = this;
    $scope.ServicePreferenceShow = false;
    vbranchmap.entity = angular.copy(row.entity);
    vbranchmap.entity.ServicePreference = vbranchmap.entity.CopyServicePrefernce;
    vbranchmap.save = save;
    vbranchmap.ServicePrefrences = [];
    vbranchmap.entity.ServicePreference = [];
    vbranchmap.ServicePreferenceMap = [];
    vbranchmap.ServicePreferencesLocation = [];
    $scope.InternalApplicationId = ServicePreferenceMapping.InternalApplicationId;

    var getIndexIfObjWithOwnAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }

    if (vbranchmap.entity.IsActive == 'Yes') {
        vbranchmap.entity.IsActive = true;
    } else if (vbranchmap.entity.IsActive == 'No') {
        vbranchmap.entity.IsActive = false;
    } else {
      vbranchmap.entity.IsActive = true;
    }

    if ($scope.InternalApplicationId === 9)
    { $scope.ServicePreferenceShow = true; }
    else { $scope.ServicePreferenceShow = false; }
    
    $http.get('InboundDocs/GetServices').success(function (data) {
        if (data.length > 0) {
            data.splice(1, 1) //remove Signing service 
        }
        vbranchmap.ServicePrefrences = data;

        if (vbranchmap.entity.ContactId === '0' && $scope.InternalApplicationId === 9) {
            $http.get('Locations/GetServicePreferenceLocation/' + vbranchmap.entity.LocationId).success(function (data) {
                vbranchmap.ServicePreferencesLocation = data;
                $scope.ServicePreferenceShow = true;
                if (vbranchmap.ServicePreferencesLocation != null) {
                    for (var i = 0; i < vbranchmap.ServicePreferencesLocation.length; i++) {
                        var tmp = getIndexIfObjWithOwnAttr(vbranchmap.ServicePrefrences, "ID", vbranchmap.ServicePreferencesLocation[i].ID);
                        if (tmp > -1)
                            vbranchmap.ServicePrefrences[tmp].Ticked = true;
                    }
                }
            });
        }

    });

    if (vbranchmap.entity.ContactId != '0' &&  $scope.InternalApplicationId === 9) {
        $http.get('Contacts/GetServicePreferenceContact/' + vbranchmap.entity.ContactId).success(function (data) {
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
        if (vbranchmap.entity.ContactId == '0') {
            row.entity = angular.extend(row.entity, vbranchmap.entity);
            $http.post(psContactMappingsApiUri.AddContact, row.entity)
                  .success(function (data) {
                      //real ID come back from response after the save in DB
                      row.entity = data;
                      row.entity.IsActive = (row.entity.IsActive == true) ? 'Yes' : 'No';
                      grid.data.push(row.entity);

                      if (data.length == 0) {                          
                          growl.error('Cannot add ' + row.entity.LocationId);
                          return;
                      }
                      else {
                          growl.success('A new record for Location ID: "' + row.entity.LocationId + '" was created successfully');
                      }

                  }).error(function (response) {
                      row.entity.IsActive = (row.entity.IsActive == true) ? 'Yes' : 'No';
                      if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                          growl.error("Validation failed for one or more entities while Saving to DB");
                          return;
                      }

                      growl.error(response.ExceptionMessage);
                      return;
                  });

        } else {

            row.entity = angular.extend(row.entity, vbranchmap.entity);
            $http.post(psContactMappingsApiUri.UpdateContact, row.entity)
          .success(function (data) {
              if (data == 0) {
                  row.entity.IsActive = (row.entity.IsActive == true) ? 'Yes' : 'No';
                  growl.error('Cannot update ' + row.entity.LocationId);
                  return;
              }
              else {
                  row.entity.IsActive = (row.entity.IsActive == true) ? 'Yes' : 'No';
                  growl.success('The record was updated successfully');
              }

          }).error(function (response) {
              row.entity.IsActive = (row.entity.IsActive == true) ? 'Yes' : 'No';
              if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                  growl.error("Validation failed for one or more entities while Saving to DB");
                  return;
              }
              if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                  growl.error(row.entity.ContactId + ' is a duplicate and cannot be added');
                  return;
              }

              growl.error(response.ExceptionMessage);
              return;
          });

        }
        $uibModalInstance.close(row.entity);
    }

    
    vbranchmap.remove = remove;
    function remove() {
        if (row.entity.ContactId != '0') {
            row.entity = angular.extend(row.entity, vbranchmap.entity);
            $http.post('Contacts/Delete', row.entity.ContactId)
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
                       if (row.entity.ContactId != '0') {
                           row.entity = angular.extend(row.entity, vbranchmap.entity);
                           $http.post('Contacts/ConfirmDeleteContact', row.entity.ContactId)
                          .success(function (data) {
                              if (data === 1) {
                                  var index = grid.appScope.vbranchmap.serviceBranchGrid.data.indexOf(row.entity);
                                  grid.appScope.vbranchmap.serviceBranchGrid.data.splice(index, 1);
                                  growl.success("Contact Info was Deleted successfully");
                                  return;
                              }
                              else {
                                  growl.error("There was an error deleting Contact Info record for: " + row.entity.LvisContactid);
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

function psContactMappingsApiUri() {
    var contactMap = this;

    contactMap.AddContact = 'api/ContactMappings/Post';

    contactMap.UpdateContact = 'api/ContactMappings/UpdateContactDetails';

}