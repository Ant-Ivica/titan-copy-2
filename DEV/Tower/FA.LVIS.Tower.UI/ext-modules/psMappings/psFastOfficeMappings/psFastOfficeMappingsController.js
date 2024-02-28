"use strict";

angular.module('psFastOfficeMappings').controller('psFastOfficeMappingsController', psFastOfficeMappingsController);
angular.module('psFastOfficeMappings').service('psFastOfficeMappingsRowEditor', psFastOfficeMappingsRowEditor);


psFastOfficeMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psFastOfficeMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies', 'growl', '$route', '$routeParams'];
function psFastOfficeMappingsController($scope, $rootScope, $http, $interval, $uibModal, psFastOfficeMappingsRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies, growl, $route, $routeParams) {
    var voffice = this;

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
    var ProviderOfficeMapRowCount = false;
    if ($rootScope.activityright === 'Admin' || $rootScope.activityright === 'SuperAdmin') {
        hasAccess = true;
    }

    $scope.hasAccess = hasAccess;
    $rootScope.hasAccess = hasAccess;

    var hasSuperAccess = false;

    if ($rootScope.activityright === 'SuperAdmin') {
        hasSuperAccess = true;
    }

    $scope.hasSuperAccess = hasSuperAccess;
    $rootScope.hasSuperAccess = hasSuperAccess;


    voffice.editNewFASTOfficeMap = psFastOfficeMappingsRowEditor.editNewFASTOfficeMap;
    voffice.addNewFASTOfficeMap = psFastOfficeMappingsRowEditor.addNewFASTOfficeMap;
    voffice.removegroupRow = psFastOfficeMappingsRowEditor.removegroupRow;

    voffice.showgrpDocuments = psFastOfficeMappingsRowEditor.showgrpDocuments;
    voffice.showgrpEvents = psFastOfficeMappingsRowEditor.showgrpEvents;
    voffice.Busy = false;


    var search = "";
    var Isvisible = false;
    if ($routeParams.ExternalID != ":") {
        voffice.ExternalId = ($routeParams.ExternalID || "");

        if ($routeParams.ExternalID.indexOf(":") > 0) {
            search = voffice.ExternalId.split(":")[1];
            voffice.providerExternalID = voffice.ExternalId.split(":")[0];
        }

        Isvisible = true;
    }

    else {

        Isvisible = false;
    }
    $scope.Isvisible = Isvisible;
    $rootScope.Isvisible = Isvisible;



    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" tooltip-placement="bottom" uib-tooltip="Outbound Document" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'
    //  var groupdocevent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)"  class="fa fa-file-text" tooltip-placement="bottom" uib-tooltip="Outbound Document" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showgrpDocuments(row)"></a> <a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-bolt" tooltip-placement="bottom" uib-tooltip="Outbound Event" style="padding:1px 7px;text-align:center;cursor:pointer" ng-click="grid.appScope.showgrpEvents(row)"></a>  </div>'

    voffice.serviceFastOfficeGrid = {
        enableColumnResize: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        enableSelectAll: true,
        paginationPageSizes: [15, 30, 45],
        paginationPageSize: 15,
        minRowsToShow: 16,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        groupingShowAggregationMenu: 0,
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterCsvFilename: 'FastOfficeMap.csv',
        columnDefs: [
            { field: 'ProviderId', name: 'Provider ID', displayName: 'Provider ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ExternalId', name: 'External ID', displayName: 'External ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Region', name: 'Region', displayName: 'Region ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'TitleOffice', name: 'Title Office', displayName: 'Title Office ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'EscrowOffice', name: 'Escrow Office', displayName: 'Escrow Office ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'TitleOfficerCode', name: 'Title Officer', displayName: 'Title Officer', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'EscrowOfficerCode', name: 'Escrow Officer', displayName: 'Escrow Officer', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Location', name: 'Location ID', displayName: 'Location ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'FASTOfficeMapDesc', name: 'Description', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'ApplicationName', name: 'Application Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
            { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
            
        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.voffice.editNewFASTOfficeMap(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            voffice.serviceFastOfficeGrid.gridApi = gridApi;
        }
    };

    $http.get('Security/GetTenant')
.then(function (response) {
    $scope.Tenant = response.data;
});
    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.voffice.serviceFastOfficeGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname === 'LVIS')
        $scope.voffice.serviceFastOfficeGrid.columnDefs[pos].visible = true;
    else
        $scope.voffice.serviceFastOfficeGrid.columnDefs[pos].visible = false;

    //var Responsedata;
    if (search != "") {
        if ($rootScope.activityright) {
            $http.get('FASTOffices/GetFASTOfficeMappingsprovider/' + search)
                .success(function (response) {
                    if (response.length > 0) {
                        response.forEach(function (data) {
                            if (data.ProviderName != null && data.ProviderName != '' && data.ProviderName != undefined) {
                                data.ProviderId = data.ProviderName + " (" + data.ExternalId + ")";
                            } else {
                                data.ProviderId = data.ProviderId + " (" + data.ExternalId + ")";
                            }
                        });

                    }
                    voffice.serviceFastOfficeGrid.data = response;
                });
        }
    }
    else {
        if ($rootScope.activityright) {
            $http.get('FASTOffices/GetFASTOfficeMapping/')
                .success(function (response) {
                    if (response.length > 0) {
                        response.forEach(function (data) {
                            if (data.ProviderName != null && data.ProviderName != '' && data.ProviderName != undefined) {
                                data.ProviderId = data.ProviderName + " (" + data.ExternalId + ")";
                            } else {
                                data.ProviderId = data.ProviderId + " (" + data.ExternalId + ")";
                            }
                        });

                    }
                    voffice.serviceFastOfficeGrid.data = response;
                });
        }
    }

    $scope.ProviderOfficeMapRowCount = ProviderOfficeMapRowCount;
    $rootScope.ProviderOfficeMapRowCount = ProviderOfficeMapRowCount;

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
        $scope.gridApi.grouping.groupColumn('EventId');
    };

    $scope.addNewFASTOfficeMap = function () {
        var newService = {
            "FASTOfficeMapId": 0,
            "ProviderId": "",
            "RegionId": "",
            "TitleOfficeId": "",
            "EscrowOfficeId": "",
            "FASTOfficeMapDesc": "",
            "LocationId": "",
            "TitleOfficerCode": "",
            "EscrowOfficerCode": "",
            "CustomerId": "",
            "Contactid": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        voffice.addNewFASTOfficeMap($scope.voffice.serviceFastOfficeGrid, rowTmp);
    };

    voffice.StateFipsList = [];
    if ($rootScope.activityright) {
        $http.get('FilePreferences/GetStateFipsList').success(function (data) {
            voffice.StateFipsList = data
        });
    }

    $scope.disabled = true;//To
    voffice.searchDetails = searchDetails;
    voffice.LoadCounty = LoadCounty;
    voffice.TitlePriority = false;

    var StateFips = voffice.StateFipsId;
    voffice.CountyFipsList = [];

    function LoadCounty(StateFips) {
        if (StateFips != null) {
            $http.get('FilePreferences/GetCountyFipsList/' + StateFips).success(function (data) {
                voffice.CountyFipsList = data;
                voffice.entity.CountyFipsId = '0'
                $scope.disabled = false;
            });
        }
    }

    voffice.ResetRefreshBotton = ResetRefreshBotton;
    function ResetRefreshBotton() {
        $scope.disabled = false;
    }

    voffice.RefreshSearch = RefreshSearch;
    function RefreshSearch() {
        voffice.entity.StateFipsId = undefined;
        voffice.entity.CountyFipsId = undefined;
        voffice.TitlePriority = false;
        $scope.disabled = true;

    }

    function searchDetails() {
        voffice.Busy = true;

        if ($scope.Isvisible) {
            $http.get('FASTOffices/GetFASTOfficeMappingsprovider/' + search)
                .success(function (response) {
                    if (response.length > 0) {
                        response.forEach(function (data) {
                            if (data.ProviderName != null && data.ProviderName != '' && data.ProviderName != undefined) {
                                data.ProviderId = data.ProviderName + " (" + data.ExternalId + ")";
                            } else {
                                data.ProviderId = data.ProviderId + " (" + data.ExternalId + ")";
                            }
                        });

                    }
                    voffice.serviceFastOfficeGrid.data = response;
                    voffice.Busy = false;
                });
        }
        else {
            $http.get('FASTOffices/GetOfficeDetails/' + voffice.entity.StateFipsId + '/' + voffice.entity.CountyFipsId + '/' + voffice.TitlePriority)
                .then(function (response) {
                    if (response.data.length > 0) {
                        response.data.forEach(function (data) {
                            if (data.ProviderName != null && data.ProviderName != '' && data.ProviderName != undefined) {
                                data.ProviderId = data.ProviderName + " (" + data.ExternalId + ")";
                            } else {
                                data.ProviderId = data.ProviderId + " (" + data.ExternalId + ")";
                            }
                        });

                    }
                    voffice.serviceFastOfficeGrid.data = response.data;
                    voffice.Busy = false;
                });
        }
    }

    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.voffice.serviceFastOfficeGrid.selectItem(index, false);
        $scope.voffice.serviceFastOfficeGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        var index = $scope.voffice.serviceFastOfficeGrid.data.indexOf(row.entity);
        $scope.voffice.serviceFastOfficeGrid.data.splice(index, 1);
    };
}

psFastOfficeMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psFastOfficeMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editNewFASTOfficeMap = editNewFASTOfficeMap;
    service.addNewFASTOfficeMap = addNewFASTOfficeMap;


    function editNewFASTOfficeMap(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastOfficeMappings/fastoffice-mappings-edit.html',
            controller: 'psFastOfficeMappingsRowEditCtrl',
            controllerAs: 'voffice',
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

    function addNewFASTOfficeMap(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastOfficeMappings/fastoffice-mappings-add.html',
            controller: 'psFastOfficeMappingsRowEditCtrl',
            controllerAs: 'voffice',
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


angular.module('psFastOfficeMappings').controller('psFastOfficeMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm',
    function psFastOfficeMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm) {

        var voffice = this;
        voffice.entity = angular.copy(row.entity);
        voffice.save = save;
        voffice.LoadExternalId = LoadExternalId;
        voffice.LoadTitleEscrowOffice = LoadTitleEscrowOffice;
        voffice.LoadEscrowOffice = LoadEscrowOffice;
        voffice.AddCondition = AddCondition;
        voffice.LoadEscrowOfficersList = LoadEscrowOfficersList;
        voffice.LoadTitleOfficersList = LoadTitleOfficersList;
        voffice.LoadContact = LoadContact;
        voffice.LoadLocationbyCustId = LoadLocationbyCustId;
        voffice.FormDirty = false;
        voffice.entity.LocationCondition = [];
        voffice.EscrowOfficerList = [];
        voffice.TitleOfficerList = [];
        voffice.TitleEscrowOfficeList = [];
        voffice.EscrowOfficeList = [];
        voffice.locationShallowCopy = [];
        var escrowFuncType = 79;
        var titleFuncType = 78;

        voffice.customerList = [];
        voffice.ContactList = [];

        voffice.localLang = {
            selectAll: "Select All",
            selectNone: "Select None",
            reset: "Undo Changes",
            search: "search...",
            nothingSelected: "Any"
        }
        // start-On change of Region clear respective search boxes
        $scope.clearSearch = function () {
            voffice.entity.TitleOffice = '';
            voffice.entity.EscrowOffice = '';
        }

        if (voffice.entity.FASTOfficeMapId != '0') {
            $http.get('FASTOffices/GetFASTOfficeDetailsByID/' + voffice.entity.FASTOfficeMapId).success(function (data) {
                voffice.entity = data;
                if (voffice.entity.ProviderName != null && voffice.entity.ProviderName != '' && voffice.entity.ProviderName != undefined) {
                    voffice.entity.ProviderName = voffice.entity.ProviderName + "(" + voffice.entity.ExternalId + ")";
                } else {
                    voffice.entity.ProviderName = voffice.entity.ProviderId + "(" + voffice.entity.ExternalId + ")";
                }
            });
        }

        if (voffice.entity.FASTOfficeMapId != '0') {
            LoadTitleEscrowOffice(voffice.entity.RegionId)
            LoadTitewOffice(voffice.entity.RegionId);
            LoadEscrowOffice(voffice.entity.EscrowRegionId);
            LoadEscrowOfficersList(voffice.entity.EscrowOfficeId);
            LoadTitleOfficersList(voffice.entity.TitleOfficeId);
        }

        if (voffice.entity.locationId != '0' && voffice.entity.locationId != undefined) {
            $http.get('Security/GetContact/' + voffice.entity.locationId).success(function (data) {
                if (data.length == 0) {
                    return;
                }
                voffice.ContactList = data;
            });
        }

        voffice.ProviderList = [];
        $http.get('FASTOffices/GetProviderList')
            .success(function (data) {
                voffice.ProviderList = data;
                if (voffice.ProviderList.length == 0 && voffice.entity.FASTOfficeMapId == '0') {
                    growl.warning("No unmapped Provider ID(s) found.");
                }
            });

        $http.get('Customers/GetTenantBasedCustomer').success(function (response) {
            voffice.customerList = response;
            voffice.entity.CustomerId = voffice.entity.CustomerId;
        });

        voffice.LocationList = [];
        $http.get('FASTOffices/GetLocationsList')
            .success(function (data) {
                voffice.LocationList = data;
                voffice.locationShallowCopy = voffice.LocationList.slice();
                if (voffice.LocationList.length == 0) {
                    growl.warning("No unmapped Location ID(s) found.");
                }
            });

        voffice.RegionList = [];
        $http.get('Security/GetRegionsWithoutClaim/Fast').then(function (data) {
            voffice.RegionList = data.data;
        });

        voffice.TokenList = [];
        $http.get('FASTOffices/GetAuthenticationToken').then(function (data) {
            voffice.TokenList = data.data;
        });

        function LoadContact(Locationid) {
            voffice.ContactList = [];
            voffice.entity.Contactid = 0;
            if (Locationid != null) {
                $http.get('Security/GetContact/' + Locationid).success(function (data) {
                    if (data.length == 0) {
                        return;
                    }
                    voffice.ContactList = data;
                });
            } else { return; }

        }

        function LoadLocationbyCustId(custId) {
            if (custId != undefined && custId > 0) {
                $http.get('FASTOffices/GetLocationsListByCustId/' + custId)
                    .success(function (data) {
                        voffice.LocationList = data;
                    });
            } else {
                voffice.LocationList = voffice.locationShallowCopy.slice();
            }

        }

        function LoadTitleEscrowOffice(RegionId) {
            $http.get('Security/GetFastOffices/' + RegionId).success(function (data) {
                if (data.length == 0) {
                    voffice.TitleEscrowOfficeList = [{
                        "Id": "0",
                        "Name": "--NA--",
                    }];
                    return;
                }
                voffice.TitleEscrowOfficeList = data;
                voffice.EscrowOfficeList = data;
            });
            voffice.entity.EscrowRegionId = RegionId;
        }

        function LoadTitewOffice(RegionId) {
            $http.get('Security/GetFastOffices/' + RegionId).success(function (data) {
                if (data.length == 0) {
                    voffice.TitleEscrowOfficeList = [{
                        "Id": "0",
                        "Name": "--NA--",
                    }];
                    return;
                }
                voffice.TitleEscrowOfficeList = data;
            });
        }

        function LoadEscrowOffice(RegionId) {
            $http.get('Security/GetFastOffices/' + RegionId).success(function (data) {
                if (data.length == 0) {
                    voffice.EscrowOfficeList = [{
                        "Id": "0",
                        "Name": "--NA--",
                    }];
                    return;
                }
                voffice.EscrowOfficeList = data;
            });
        }

        function LoadEscrowOfficersList(EscrowOfficeId) {
            $http.get('FASTOffices/GetTitleEscrowOfficers/' + EscrowOfficeId + '/' + escrowFuncType).success(function (data) {
                if (data.length == 0) {
                    voffice.EscrowOfficerList = [{
                        "ID": "0",
                        "Name": "--NA--",
                    }];
                    return;
                }
                voffice.EscrowOfficerList = data;
            });

            $http.get('FASTOffices/GetTitleEscrowOfficers/' + EscrowOfficeId + '/238').success(function (data) {
                if (data.length == 0) {
                    voffice.EscrowAssistantList = [{
                        "ID": "0",
                        "Name": "--NA--",
                    }];
                    return;
                }
                voffice.EscrowAssistantList = data;
            });
        }

        function LoadTitleOfficersList(TitleOfficeId) {
            $http.get('FASTOffices/GetTitleEscrowOfficers/' + TitleOfficeId + '/' + titleFuncType).success(function (data) {
                if (data.length == 0) {
                    voffice.TitleOfficerList = [{
                        "ID": "0",
                        "Name": "--NA--",
                    }];
                    return;
                }

                voffice.TitleOfficerList = data;
            });
        }

        voffice.ExternalId = "";
        function LoadExternalId() {
            $http.get('FASTOffices/GetExternalId/' + voffice.entity.ProviderId).success(function (data) {
                if (data.length == 0) {
                    voffice.ExternalId = "";
                    return;
                }
                voffice.ExternalId = data;
            });
        }

        voffice.StateFipsList = [];
        $http.get('FilePreferences/GetStateFipsList').success(function (data) {
            voffice.StateFipsList = data
        });

        voffice.LoadCounty = LoadCounty;
        voffice.CountyFipsList = [];
        function LoadCounty(StateFips) {
            if (StateFips != null) {
                $http.get('FilePreferences/GetCountyFipsList/' + StateFips).success(function (data) {
                    voffice.CountyFipsList = data;
                    voffice.county = voffice.CountyFipsList[0];
                });
            }
        }

        if (voffice.entity.Tenant == '') {
            voffice.entity.Tenant = $scope.tenantname;
        }

        function AddCondition() {
            var Condition = {
                PreferenceState: voffice.State, PreferenceCounty: voffice.county
            };

            for (var i = 0; i < voffice.entity.LocationCondition.length; i++) {
                if (voffice.entity.LocationCondition[i].PreferenceState.StateFIPS == voffice.State.StateFIPS &&
                    voffice.entity.LocationCondition[i].PreferenceCounty.countyFIPS == voffice.county.countyFIPS) {
                    return;
                }
            }
            voffice.entity.LocationCondition.push(Condition);
        }

        voffice.Remove = function Remove(Condition) {
            voffice.FormDirty = true;
            var index = voffice.entity.LocationCondition.indexOf(Condition);
            if (index >= 0)
                voffice.entity.LocationCondition.splice(index, 1);

            if (voffice.entity.LocationCondition.length == 0) {

                voffice.State = undefined;
                voffice.county = undefined;

            }
        }
        voffice.Change = function Change() {
            voffice.FormDirty = true;
        }

        //Send Data to Web APi for save/Delete
        function save() {
            if (voffice.entity.FASTOfficeMapId == '0') {
                row.entity = angular.extend(row.entity, voffice.entity);
                row.entity.ProviderId = voffice.entity.Provider.ProviderID;
                row.entity.TitleOfficeId = voffice.TitleOfficeId.id;
                row.entity.EscrowOfficeId = voffice.EscrowOfficeId.id;
                row.entity.FASTOfficeMapDesc = voffice.entity.FASTOfficeMapDesc;
                row.entity.ExternalID = voffice.entity.Provider.ExternalID;

                $http.post('FASTOffices/AddFastOffice', row.entity)
                    .success(function (data) {
                        //real ID come back from response after the save in DB
                        row.entity = data;
                        grid.data.push(row.entity);
                        if (data.length == 0) {
                            growl.error('FAST Office Map info record is duplicate and cannot be added');
                            return;
                        }
                        else {
                            growl.success("FAST Office Map info record was added successfully");
                        }
                    }).error(function (response) {

                        if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                            growl.error("Error adding FAST Office Map info record");
                            return;
                        }
                        if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0) {
                            growl.error('FAST Office Map info record is duplicate and cannot be added');
                            return;
                        }
                        if (response.InnerException.InnerException.ExceptionMessage.indexOf("Cannot insert duplicate key row in object 'dbo.FASTOfficeMap' with unique index 'FASTOfficeMap_idx_1'") >= 0) {
                            growl.error('FAST Office Map info record is duplicate and cannot be added');
                            return;
                        }
                        growl.error(response.ExceptionMessage);
                        return;
                    });
            }
            else {
                row.entity = angular.extend(row.entity, voffice.entity);
                row.entity.ProviderId = voffice.entity.ProviderId;
                if (voffice.entity.RegionId != undefined)
                    row.entity.RegionId = voffice.entity.RegionId
                if (voffice.entity.TitleOfficeId != undefined)
                    row.entity.TitleOfficeId = voffice.entity.TitleOfficeId
                if (voffice.entity.EscrowOfficeId != undefined)
                    row.entity.EscrowOfficeId = voffice.entity.EscrowOfficeId

                row.entity.locationId = voffice.entity.locationId
                row.entity.FASTOfficeMapDesc = voffice.entity.FASTOfficeMapDesc

                row.entity.TitleOfficerCode = voffice.entity.TitleOfficerCode;
                row.entity.EscrowOfficerCode = voffice.entity.EscrowOfficerCode;


                $http.post('FASTOffices/UpdateFastOffice', row.entity)
                    .success(function (data) {
                        if (data == 0) {
                            growl.error('FAST Office Map info is duplicate and cannot be added');
                            return;
                        }
                        else {
                            row.entity = data;
                            grid.data = row.entity;
                            growl.success("FAST Office Map info was updated successfully");
                        }
                    }).error(function (response) {
                        if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                            growl.error("Validation failed for one or more entities while Saving to DB");
                            return;
                        }
                        if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0) {
                            growl.error('FAST Office Map info is duplicate and cannot be updated');
                            return;
                        }
                        if (response.InnerException.InnerException.ExceptionMessage.indexOf("Cannot insert duplicate key row in object 'dbo.FASTOfficeMap' with unique index 'FASTOfficeMap_idx_1'") >= 0) {
                            growl.error('FAST Office Map info record is duplicate and cannot be updated');
                            return;
                        }
                        growl.error(response.ExceptionMessage);
                        return;
                    });
            }

            $uibModalInstance.close(row.entity);
        }

        voffice.remove = remove;
        function remove() {
            if (row.entity.FASTOfficeMapId != '0') {
                row.entity = angular.extend(row.entity, voffice.entity);
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
                    if (row.entity.FASTOfficeMapId != '0') {
                        row.entity = angular.extend(row.entity, voffice.entity);
                        $http.post('FASTOffices/ConfirmDeleteFASTOffice', row.entity.FASTOfficeMapId)
                            .success(function (data) {
                                if (data > 0) {
                                    var index = grid.appScope.voffice.serviceFastOfficeGrid.data.indexOf(row.entity);
                                    grid.appScope.voffice.serviceFastOfficeGrid.data.splice(index, 1);
                                    growl.success("FAST Office Map Info record was deleted successfully");
                                    return;
                                }
                                else {
                                    growl.error("There was an error deleting FAST Office Map Info record for: " + row.entity.FASTOfficeMapDesc);
                                }
                            }).error(function (response) {
                                growl.error('There was an error deleting FAST Office Map Info record. Exception: ' + response.ExceptionMessage);
                                return;
                            });
                    }
                });

                $uibModalInstance.close(row.entity);
            }
        }
    }]);


