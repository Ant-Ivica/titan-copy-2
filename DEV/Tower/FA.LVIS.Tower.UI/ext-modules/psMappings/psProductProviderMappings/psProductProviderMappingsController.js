"use strict";


angular.module('psProductProviderMappings').controller('psProductProviderMappingsController', psProductProviderMappingsController);
angular.module('psProductProviderMappings').service('psProductProviderMappingsRowEditor', psProductProviderMappingsRowEditor);


psProductProviderMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psProductProviderMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', '$confirm', 'UserInfo', '$q', '$cookies', 'growl', '$routeParams', 'ProviderMappingName'];
function psProductProviderMappingsController($scope, $rootScope, $http, $interval, $uibModal, psProductProviderMappingsRowEditor, uiGridGroupingConstants, $location, $window, $confirm, UserInfo, $q, $cookies, growl, $routeParams, ProviderMappingName) {

    var vproductmap = this;

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

    var search = "";
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

    vproductmap.addNewBranch = psProductProviderMappingsRowEditor.addNewBranch;
    vproductmap.editProductBranch = psProductProviderMappingsRowEditor.editProductBranch;

    if ($routeParams.ProviderID != ":") {
        vproductmap.Product = ($routeParams.ProviderID || "");

        if (vproductmap.Product.indexOf(":") > 0) {
            search = vproductmap.Product.split(":")[0];
            vproductmap.providerExternalID = vproductmap.Product.split(":")[1];
        }
    }

    vproductmap.serviceOfficeGrid = {
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
        exporterCsvFilename: 'ProductProviderInfo.csv',
        columnDefs: [
            { field: 'ProviderName', headerCellClass: 'grid-header', name: 'Provider ID', displayName: 'Provider ID', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'CustomerName', headerCellClass: 'grid-header', name: 'Customer ID', displayName: 'Customer ID', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'LocationName', headerCellClass: 'grid-header', name: 'Location ID', displayName: 'Location ID', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ContactName', headerCellClass: 'grid-header', name: 'Contact ID', displayName: 'Contact ID', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ProductName', headerCellClass: 'grid-header', name: 'Product', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Service', headerCellClass: 'grid-header', name: 'Service', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Tenant', headerCellClass: 'grid-header', name: 'Tenant', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Application', headerCellClass: 'grid-header', name: 'Application', enableCellEdit: false, groupingShowAggregationMenu: false }

        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vproductmap.editProductBranch(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vproductmap.serviceOfficeGrid.gridApi = gridApi;
        }
    };

    if ($rootScope.activityright) {
        $http.get('ProductProvider/GetProductProviders/' + search).success(function (response) {
            if (response.length > 0) {
                response.forEach(function (data) {
                    if (data.ProviderName != null && data.ProviderName != '' && data.ProviderName != undefined) {
                        data.ProviderName = data.ProviderName + " (" + data.ExternalId + ")";
                    } else {
                        data.ProviderName = data.ProviderId + " (" + data.ExternalId + ")";
                    }
                });
            }
            vproductmap.serviceOfficeGrid.data = response;
        });
    }

    $scope.Tenant = '';
    $http.get('Security/GetTenant')
        .then(function (response) {
            $scope.Tenant = response.data;
        });

    $scope.addNewBranch = function () {
        var newService = {
            "ProductProviderMapId": "0",
            "ProviderId": search,
            "ProviderName": ProviderMappingName.ProviderNameExtension,
            "CustomerId": "",
            "ContactId": "",
            "LocationId": "",
            "ProductId": "",
            "ServiceId": "",
            "TenantId": "",
            "ApplicationId": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vproductmap.addNewBranch($scope.vproductmap.serviceOfficeGrid, rowTmp);
    };

}

psProductProviderMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psProductProviderMappingsRowEditor($http, $rootScope, $uibModal) {
    var service = {};
    service.addNewBranch = addNewBranch;
    service.editProductBranch = editProductBranch;

    function addNewBranch(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psProductProviderMappings/product-provider-mappings-add.html',
            controller: 'psProductProvideMappingsRowEditCtrl',
            controllerAs: 'vproductmap',
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

    function editProductBranch(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psProductProviderMappings/product-provider-mappings-edit.html',
            controller: 'psProductProvideMappingsRowEditCtrl',
            controllerAs: 'vproductmap',
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

angular.module('psProductProviderMappings').controller('psProductProvideMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm',
    function psProductProvideMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm) {
        var vproductmap = this;
        vproductmap.entity = angular.copy(row.entity);
        vproductmap.save = save;
        vproductmap.LoadLocationbyCustId = LoadLocationbyCustId;
        vproductmap.LoadContact = LoadContact;
        vproductmap.remove = remove;
        vproductmap.LoadProviderByExternalAppId = LoadProviderByExternalAppId;
        if (vproductmap.entity.Tenant == '' || vproductmap.entity.Tenant == undefined) {
            vproductmap.entity.Tenant = $scope.tenantname;
        }

        vproductmap.customerList = [];
        vproductmap.LocationList = [];
        vproductmap.ContactList = [];
        vproductmap.ProductList = [];
        vproductmap.ApplicationList = [];
        vproductmap.ServiceList = [];
        vproductmap.locationShallowCopy = [];
        $http.get('Customers/GetTenantBasedCustomer').success(function (response) {
            vproductmap.customerList = response;
            vproductmap.entity.CustomerId = vproductmap.entity.CustomerId;
        });

        $http.get('FASTOffices/GetLocationsList')
            .success(function (data) {
                vproductmap.LocationList = data;
                vproductmap.locationShallowCopy = vproductmap.LocationList.slice();
                if (vproductmap.LocationList.length == 0) {
                    growl.warning("No unmapped Location ID(s) found.");
                }
            });




        if (vproductmap.entity.LocationId != '0' && vproductmap.entity.LocationId != undefined && vproductmap.entity.LocationId != '') {
            $http.get('Security/GetContact/' + vproductmap.entity.LocationId).success(function (data) {
                if (data.length == 0) {
                    return;
                }
                vproductmap.ContactList = data;
            });
        }

        ////vproductmap.localLang = {
        ////    selectAll: "Select All",
        ////    selectNone: "Select None",
        ////    reset: "Undo Changes",
        ////    search: "search...",
        ////    nothingSelected: "--Any--"
        ////}

        if (vproductmap.entity.ApplicationId != '0' && vproductmap.entity.ApplicationId != undefined && vproductmap.entity.Application != '') {
            $http.get('FilePreferences/GetProductList/' + vproductmap.entity.ApplicationId).success(function (data) {
                if (data.length == 0) {
                    return;
                }
                vproductmap.ProductList = data;
            });
        }

        //Get Application List To Bind Dropdown    
        $http.get('Security/GetExternalApplications').success(function (data) {
            vproductmap.ApplicationList = data;
        });

     

        //start-Bind-Region List    
        $http.get('InboundDocs/GetServices')
            .success(function (data) {
                vproductmap.ServiceList = data;
            });

        function LoadLocationbyCustId(custId) {
            if (custId != undefined && custId > 0) {
                $http.get('FASTOffices/GetLocationsListByCustId/' + custId)
                    .success(function (data) {
                        vproductmap.LocationList = data;
                    });
            } else {
                vproductmap.LocationList = vproductmap.locationShallowCopy.slice();
            }

        }
        function LoadProviderByExternalAppId(externalAppId) {
            vproductmap.ProductList = [];
            vproductmap.entity.ProductId = 0;
            if (externalAppId != undefined && externalAppId > 0) {
                $http.get('FilePreferences/GetProductList/' + externalAppId)
                    .success(function (data) {          
                        vproductmap.ProductList = data;
                    });
            } else {
                return;
            }

        }

        function LoadContact(Locationid) {
            vproductmap.ContactList = [];
            vproductmap.entity.ContactId = 0;
            if (Locationid != null) {
                $http.get('Security/GetContact/' + Locationid).success(function (data) {
                    if (data.length == 0) {
                        return;
                    }
                    vproductmap.ContactList = data;
                });
            } else { return; }

        }

        function save() {
            if (vproductmap.entity.ProductProviderMapId == '0') {
                row.entity = angular.extend(row.entity, vproductmap.entity);
                $http.post('api/ProductProviderMappings/AddProductProvider', row.entity)
                    .success(function (data) {

                        //real ID come back from response after the save in DB
                        row.entity = data;

                        if (data.length == 0) {
                            growl.error('A record with the Product Provider ID: "' + row.entity.ProductProviderMapId + '" cannot be added');
                            return;
                        }
                        else {
                            if (data.ProviderName != null && data.ProviderName != '' && data.ProviderName != undefined) {
                                data.ProviderName = data.ProviderName + " (" + data.ExternalId + ")";
                            } else {
                                data.ProviderName = data.ProviderId + " (" + data.ExternalId + ")";
                            }
                            grid.data.push(row.entity);
                            growl.success('A new record for Product Provider ID: "' + row.entity.ProductProviderMapId + '" was created successfully');
                        }
                    }).error(function (response) {

                        growl.error('A record with the Product Provider ID: "' + row.entity.ProductProviderMapId + '" cannot be added');
                        return;
                    });
            } else {
                $http.post('api/ProductProviderMappings/UpdateProductProvider', vproductmap.entity)
                    .success(function (data) {
                        if (data.length == 0) {
                            growl.error('There was an error updating Product Provider ID: "' + vofficemap.entity.ProductProviderMapId);
                            return;
                        }
                        else {

                            row.entity = angular.extend(row.entity, vproductmap.entity);
                            if (data.ProviderName != null && data.ProviderName != '' && data.ProviderName != undefined) {
                                data.ProviderName = data.ProviderName + " (" + data.ExternalId + ")";
                            } else {
                                data.ProviderName = data.ProviderId + " (" + data.ExternalId + ")";
                            }
                            row.entity = data;
                            grid.data = row.entity;
                            growl.success('The record for Product Provider ID "' + row.entity.ProductProviderMapId + '" was updated successfully');
                        }
                    }).error(function (response) {
                        growl.error(response.InnerException.InnerException.ExceptionMessage);
                        return;
                    });
            }

            $uibModalInstance.close(row.entity);

        }

        function remove() {
            if (row.entity.ProductProviderMapId != 0) {
                row.entity = angular.extend(row.entity, vproductmap.entity);
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
                    if (row.entity.ProductProviderMapId != '0') {
                        row.entity = angular.extend(row.entity, vproductmap.entity);
                        $http.post('ProductProvider/DeleteProductProvider/' + row.entity.ProductProviderMapId)
                            .success(function (data) {
                                if (data === 1) {
                                    var index = grid.appScope.vproductmap.serviceOfficeGrid.data.indexOf(row.entity);
                                    grid.appScope.vproductmap.serviceOfficeGrid.data.splice(index, 1);
                                    growl.success("Product Provider info record was deleted successfully");
                                    $uibModalInstance.close(row.entity);
                                    return;
                                }
                                else {
                                    growl.error("There was an error deleting Product Provider Info record for: " + row.entity.ProductProviderMapId);
                                }
                            })
                    }
                });
            }
        }



    }]);