"use strict";

angular.module('psSecurity').controller('psSecurityController', psSecurityController);
angular.module('psSecurity').service('psSecurityRowEditor', psSecurityRowEditor);
angular.module('psSecurity').service('psSecurityApiUri', psSecurityApiUri);

psSecurityController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psSecurityRowEditor', 'uiGridGroupingConstants', '$window', '$filter', '$confirm', 'UserInfo', '$location', '$cookies', 'growl', 'psSecurityApiUri'];
function psSecurityController($scope, $rootScope, $http, $interval, $uibModal, psSecurityRowEditor, uiGridGroupingConstants, $window, $filter, $confirm, UserInfo, $location, $cookies, growl, psSecurityApiUri) {
    var vm = this;

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

    if ($rootScope.activityright !== 'Admin' && $rootScope.activityright !== 'SuperAdmin') {
        var instance = $uibModal.open({
            template: '<div class="widget-header"><i class="fa fa-lg fa-exclamation-triangle"></i> <h3>Attention</h3></div><div class="modal-body">You are not authorized to view this page.</div>' +
                '<div class="modal-footer"><a class="btn btn-default" ng-click="$close()">Close</a></div>', size: 'sm'
        });

        instance.result.finally(function () {
            $location.path('/dashboard');
        });
    }

    if ($rootScope.activityright === 'Admin' || $rootScope.activityright === 'SuperAdmin') {
        hasAccess = true;
    }

    if ($rootScope.activityright === 'SuperAdmin') {
        hasModifyAccess = true;
    }

    $scope.hasAccess = hasAccess;
    $scope.hasModifyAccess = hasModifyAccess;
   

    vm.Busy = false;
    vm.editRow = psSecurityRowEditor.editRow;
    vm.addRow = psSecurityRowEditor.addRow;
    vm.removeRow = psSecurityRowEditor.removeRow;

    var activedetail = '<div ng-if="row.entity.IsActive == true"  class="ui-grid-cell-contents">Yes</div><div ng-if="row.entity.IsActive == false" class="ui-grid-cell-contents">No</div>'
    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'

    vm.serviceGrid = {
        enableFiltering: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        enableSorting: true,
        enableGridMenu: true,
        enableSelectAll: true,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        groupingShowAggregationMenu: 0,
        paginationPageSizes: [15, 30, 45],
        paginationPageSize: 15,
        minRowsToShow: 16,
        columnDefs: [
            { field: 'Name', name: 'Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Role', name: 'Activity Rights', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'IsActive', name: 'Active', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTemplate: activedetail },
            { field: 'sTenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false }
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vm.editRow(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vm.serviceGrid.gridApi = gridApi;
        }
    };

    if ($rootScope.activityright) {
        $http.get('Security/GetUsers')
            .then(function (response) {
                vm.serviceGrid.data = response.data;
            });
    }

    $http.get('Security/GetTenant')
    .then(function (response) {
        $scope.Tenant = response.data;        
        var pos = $scope.vm.serviceGrid.columnDefs.map(function (e) { return e.field; }).indexOf('sTenant');
        if ($rootScope.tenantname == 'LVIS')
            $scope.vm.serviceGrid.columnDefs[pos].visible = true;
        else
            $scope.vm.serviceGrid.columnDefs[pos].visible = false;
    });

    $http.get('Security/GetShowTenants')
     .then(function (response) {
          var pos = $scope.vm.serviceGrid.columnDefs.map(function (e) { return e.field; }).indexOf('sTenant');
          $scope.vm.serviceGrid.columnDefs[pos].visible = response.data;
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
        $scope.gridApi.grouping.groupColumn('Name');
        //$scope.gridApi.grouping.aggregateColumn('state', uiGridGroupingConstants.aggregation.COUNT);
    };

    $scope.addRow = function () {
        var newService = {
            "ID": "",
            "UserId": "",
            "Role": "",
            "IsActive": "false",
            "EmailId": "",
            "Employeeid": "",
            "sTenant": $scope.Tenant,
            "ManageBEQ": false,
            "ManageTEQ":false

        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vm.addRow($scope.vm.serviceGrid, rowTmp);
    };

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vm.serviceGrid.selectItem(index, false);
        $scope.vm.serviceGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {

        $confirm({ text: 'Are you sure you want to delete "' + row.entity.Name + '" user profile?' }, { size: 'sm' })
       .then(function () {
           var index = $scope.vm.serviceGrid.data.indexOf(row.entity);

           $http.post(psSecurityApiUri.deleteUser, row.entity)
              .success(function (data) {
                  if (data == 0)
                      alert('Cannot Delete row (error in console)');
                  else {
                      $scope.vm.serviceGrid.data.splice(index, 1);
                      growl.success ( "The record was deleted successfully");
                  }
              });
       });
    };
}

psSecurityRowEditor.$inject = ['$http', '$rootScope', '$uibModal', '$filter', '$cookies'];
function psSecurityRowEditor($http, $rootScope, $uibModal, $filter, $cookies) {

    var service = {};
    service.editRow = editRow;
    service.addRow = addRow;

    function editRow(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psSecurity/service-edit.html',
            controller: 'psSecurityRowEditCtrl',
            controllerAs: 'vm',
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

    function addRow(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psSecurity/service-add.html',
            controller: 'psSecurityRowEditCtrl',
            controllerAs: 'vm',
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

angular.module('psSecurity').controller('psSecurityRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$filter', '$scope', 'growl', 'psSecurityApiUri',
function psSecurityRowEditCtrl($http, $uibModalInstance, grid, row, $window, $filter, $scope, growl, psSecurityApiUri) {


    //Avoid Duplicates
    var i = 0;

    //Initialize controller
    var vm = this;
    vm.entity = angular.copy(row.entity, vm.entity);
    vm.MangeTenantAccess = false;
    vm.hasExceptionManageAccess = false;
    if ($scope.$parent.activityright === "SuperAdmin") {
        vm.hasExceptionManageAccess = true;
        vm.MangeTenantAccess = true;
    }
    else {
        vm.entity.ManageTEQ = false;
        vm.entity.ManageBEQ = false;
    }

    if ($scope.$parent.activityright === "SuperAdmin" && ($scope.tenantname == "LVIS" || $scope.tenantname == "Air Traffic Control")) {
        vm.hasAccessrightAccess = true;
    }


    vm.entity.Tenant = $scope.tenantname;

    vm.entity.IsActive = row.entity.IsActive;
    vm.save = save;
    vm.Busy = false;
    vm.ApplicationRoles =[];
    vm.Instances =[];
    vm.Tenants =[];
    vm.ShowTenants = false;



    $http.get('FilePreferences/GetTenantList').success(function (data) {
        vm.Tenants = data;
        if (vm.entity.Tenant == 'LVIS')
            vm.ShowTenants = true;
        else {
            $http.get('Security/GetShowTenants')
                .then(function (response) {
                    if (response.data == true) {
                        vm.ShowTenants = true;

                        if ($scope.$parent.activityright === 'Admin' || $scope.$parent.activityright === 'SuperAdmin') {
                            vm.MangeTenantAccess = true;
                        }
                       
                        $http.get('Security/GetEnv').then(function (response) {
                            if (response.data != 'FAF.PROD') {
                                for (var i = 0; i < vm.Tenants.length; i++) {
                                    if (vm.Tenants[i]['TenantName'] === 'LVIS') {
                                        vm.Tenants.splice(i, 1);
                                    }
                                }
                            }
                            else {
                                vm.Tenants = vm.Tenants.filter(function (item) {
                                    return item['TenantName'] == vm.entity.Tenant
                                });
                            }
                        });
                        

                    }

                });
        }

        if (row.entity.ID == "") {
            for (var i = 0; i < vm.Tenants.length; i++) {
                if (vm.Tenants[i]['TenantName'] === vm.entity.Tenant) {
                    vm.entity.TenantId = vm.Tenants[i]['TenantId'];
                }
            }
        }

    });

    $http.get('Security/GetRoles')
        .then(function (response) {
            vm.ApplicationRoles = response.data;
        });


    var getIndexIfObjWithOwnAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                return i;
        }
    }
        return -1;
    }

    var getIndexIfObjWithOwnAttrmultiple = function (array, attr1, value1, attr2, value2) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr1) && array[i][attr1] === value1)
                if (array[i].hasOwnProperty(attr2) && array[i][attr2] === value2) {
                    return i;
        }
    }
        return -1;
    }

        function save() {


            if (row.entity.ID == '0') {

                row.entity = angular.extend(row.entity, vm.entity);

                $http.post(psSecurityApiUri.addUser, row.entity)
                    .success(function (data) {

                    if (data.length == 0) {
                        growl.error('A record with the name "' + row.entity.UserName + '" already exists');
                        return;
                    }
                    else {
                        grid.data = data;
                        growl.success('A new record for "' + row.entity.UserName + '" was created successfully');
                    }
                }).error(function (response) {
                    growl.error('There was an error adding a record for "' + row.entity.UserName + '"');
                        return;
                });

            }
            else {
            vm.entity.Group = row.entity.Group;
            row.entity = angular.extend(row.entity, vm.entity);
                $http.post(psSecurityApiUri.updateUser, row.entity)
                  .then(function (response) {

                      if (response.data.length == 0) {
                          growl.error('There was an error updating this record for "' + row.entity.UserName + '"');
                          return;
                      }
                      else {
                          row.entity = response.data;
                          growl.success('The record for "' + row.entity.UserName + '" was updated successfully');
                  }
            }, function (data) { 

        });

        }

            $uibModalInstance.close(row.entity);

    }



    vm.remove = remove;
        function remove() {
        console.dir(row)
        if (row.entity.ID != '0') {

            row.entity = angular.extend(row.entity, vm.entity);
            var index = grid.appScope.vm.serviceGrid.data.indexOf(row.entity);

            $http.get(psSecurityApiUri.deleteUser + row.entity.ID)
              .then(function (response) {
                  if (response.data == 0) {
                      growl.error('Cannot Delete row (error in console)');
                      return;
                  }
                  else
                      grid.appScope.vm.serviceGrid.data.splice(index, 1);
            });

        }
        $uibModalInstance.close(row.entity);
    }

    vm.search = search;
    vm.GetValue = GetValue;
    vm.txtUser = "";
    vm.currentItem = "";
    vm.Identity = "CORP";
    vm.Busy = false;

        function search() {
        vm.Busy = true;
        vm.searchdata = "";

        $http.get('Security/FindUser/' + vm.Identity + "/" +vm.txtUser)
            .then(function (response) {
                vm.Busy = false;
                if (!response.data.length) {
                    growl.error('No match found for "' + vm.Identity + "/" +vm.txtUser + '"');
                    return;
                }
                else
                    vm.searchdata = response.data;

        });

    }

        function GetValue() {
                console.dir(row)
                row.entity = angular.extend(row.entity, vm.entity);
                row.entity.ID = 0;
                vm.entity.UserId = vm.currentItem[0].UserId;
                vm.entity.UserName = vm.currentItem[0].UserName;
                vm.entity.Name = vm.currentItem[0].Name;
                vm.entity.EmailId = vm.currentItem[0].Emailid;
                vm.entity.Employeeid = vm.currentItem[0].Employeeid;
                vm.entity.Role = 'User'
                vm.entity.Group = "";
                vm.entity.IsActive = false;
    }


}]);


angular.module('psSecurity').factory('UserInfo', ['$http', '$q', function ($http, $q) {
    return {
        getUser: function () {
            var deferred = $q.defer();
            $http.get('Security/GetCurrentUser/').then(function (response) {
                var userinfo = [];
                var currentuser = "";
                var IsAuthorized = true;
                userinfo = response.data;
                deferred.resolve(userinfo);

            }, function (error) {
                IsAuthorized = false;
                deferred.reject(error);
            });

            return deferred.promise;
        }
    };
}]
);

function psSecurityApiUri() {
    var security = this;
    security.addUser = 'api/Security/Post';
    security.updateUser = 'api/Security/UpdateUserDetails';
    security.deleteUser = 'api/Security/Delete/';
}


