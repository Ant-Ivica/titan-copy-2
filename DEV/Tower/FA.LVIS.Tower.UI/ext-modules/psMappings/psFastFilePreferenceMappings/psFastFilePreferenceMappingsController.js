"use strict";

angular.module('psFastFilePreferenceMappings').controller('psFastFilePreferenceMappingsController', psFastFilePreferenceMappingsController);
angular.module('psFastFilePreferenceMappings').service('psFastFilePreferenceMappingsRowEditor', psFastFilePreferenceMappingsRowEditor);

psFastFilePreferenceMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psFastFilePreferenceMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies', 'growl', '$route', '$filter'];
function psFastFilePreferenceMappingsController($scope, $rootScope, $http, $interval, $uibModal, psFastFilePreferenceMappingsRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies, growl, $route, $filter) {
    var vffmap = this;

    $scope.$on("getUser", function (evt, response) {
        $rootScope.activityright = response.ActivityRight;
        $rootScope.canmanageteq = response.CanManageTEQ;
        $rootScope.canmanagebeq = response.CanManageBEQ;
    });

    if (!$rootScope.activityright) {
        $rootScope.activityright = $cookies.get('activityright');
    }

    if (!$rootScope.activityright) {
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
    var hasValidatorAccess = false;

    if ($rootScope.activityright === 'Admin' || $rootScope.activityright === 'SuperAdmin') {
        hasAccess = true;
    }
    if ($rootScope.activityright === 'SuperAdmin') {
        hasSuperAccess = true;
        hasValidatorAccess = true;
    }

    $scope.hasAccess = hasAccess;
    $rootScope.hasAccess = hasAccess;
    $scope.hasSuperAccess = hasSuperAccess;
    $rootScope.hasSuperAccess = hasSuperAccess;

    $scope.hasValidatorAccess = hasValidatorAccess;
    $rootScope.hasValidatorAccess = hasValidatorAccess;

    $scope.isActive = function (viewLocation) {
        return viewLocation === $location.path();
    };

    $scope.hasSuperAccess = hasSuperAccess;
    $rootScope.hasSuperAccess = hasSuperAccess;

    vffmap.Busy = false;

    vffmap.editRowSummary = psFastFilePreferenceMappingsRowEditor.editRowSummary;
    vffmap.addRowSummary = psFastFilePreferenceMappingsRowEditor.addRowSummary;
    vffmap.removeRow = psFastFilePreferenceMappingsRowEditor.removeRow;

    vffmap.showDocuments = psFastFilePreferenceMappingsRowEditor.showDocuments;
    vffmap.showEvents = psFastFilePreferenceMappingsRowEditor.showEvents;

    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'
    var branchdocevent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"><a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-map-marker" tooltip-placement="bottom" uib-tooltip="Locations" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.ShowBranches(row)"></a> <a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)"  class="fa fa-file-text" tooltip-placement="bottom" uib-tooltip="Outbound Document" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showDocuments(row)"></a>  </div>'

    vffmap.serviceFastFileGrid = {
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
        exporterPdfTableHeaderStyle: {
            fontSize: 10, bold: true, italics: true, color: 'red'
        },
        exporterCsvFilename: 'FastFile.csv',
        columnDefs: [
          { field: 'FASTPreferenceMapID', name: 'FASTPreferenceMapID', displayName: 'FASTPreferenceMapID', headerCellClass: 'grid-header', visible: false, enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
          { field: 'FASTPreferenceMapName', name: 'Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'Region', name: 'Region ID', displayName: 'Region ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'RegionId', name: 'Region ID', displayName: 'Region ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, visible: false },
          { field: 'ProgramTypeName', name: 'Program Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'FASTProgramTypeId', visible: false, name: 'FASTProgramTypeId', displayName: 'FASTProgramTypeId', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'FASTSearchTypeId', visible: false, name: 'FASTSearchTypeId', displayName: 'FASTSearchTypeId', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'SearchType', name: 'Search Type', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'LoanPurpose', name: 'Loan Purpose', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'Location', name: 'Customer Location', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'CustomerName', visible: false, name: 'Customer Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vffmap.editRowSummary(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vffmap.serviceFastFileGrid.gridApi = gridApi;
        }
    };

    if ($rootScope.activityright) {
        $http.get('FilePreferences/GetFastFilePreferenceDetails/').success(function (response) {
            vffmap.serviceFastFileGrid.data = response;
        });
    }

    //Get ProgramList To Bind Dropdown
    vffmap.StateFipsList = [];
    if ($rootScope.activityright) {
        $http.get('FilePreferences/GetStateFipsList').success(function (data) {
            vffmap.StateFipsList = data
        });
    }

    vffmap.RegionList = [];
    if ($rootScope.activityright) {
        $http.get('Security/GetRegionsWithoutClaim/Fast').then(function (data) {
            vffmap.RegionList = data.data;
        });
    }


    $scope.disabled = true;//To
    vffmap.search = search;
    vffmap.LoanAmount = 0;
    vffmap.LoadCounty = LoadCounty;

    var StateFips = vffmap.StateFipsId;
    vffmap.CountyFipsList = [];

    function LoadCounty(StateFips) {
        if (StateFips != null) {
            $http.get('FilePreferences/GetCountyFipsList/' + StateFips).success(function (data) {
                vffmap.CountyFipsList = data;
                vffmap.entity.CountyFipsId = '0'
                $scope.disabled = false;
            });
        }
    }


    vffmap.ResetRefreshBotton = ResetRefreshBotton;
    function ResetRefreshBotton() {

        $scope.disabled = false;
    }

    vffmap.LoanSelection = [{
        'title': '---Select---',
        'value': '0'
    },
    {
        'title': '<= 1.5M',
        'value': '1'
    },
    {
        'title': '> 1.5M',
        'value': '2'
    }];

    // Reset Search
    vffmap.RefreshSearch = RefreshSearch;
    function RefreshSearch() {
        vffmap.entity.LoanAmount = 0;
        vffmap.entity.StateFipsId = undefined;
        vffmap.entity.CountyFipsId = undefined;
        vffmap.entity.RegionId = undefined;
        vffmap.CountyFipsList = [];
        $scope.disabled = true;
    }

    function search() {
        vffmap.Busy = true;
        vffmap.entity.LoanAmount = $filter('currency')(vffmap.entity.LoanAmount, '', 0);
        $http.get('FilePreferences/GetFilePreferences/' + vffmap.entity.StateFipsId + '/' + vffmap.entity.CountyFipsId + '/' + vffmap.entity.LoanAmount + "/" + vffmap.entity.RegionId)
        .then(function (response) {
            vffmap.serviceFastFileGrid.data = response.data;
            vffmap.Busy = false;
        });
    }

    $http.get('Security/GetTenant')
    .then(function (response) {
        $scope.Tenant = response.data;
    });


    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.vffmap.serviceFastFileGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname === 'LVIS')
        $scope.vffmap.serviceFastFileGrid.columnDefs[pos].visible = true;
    else
        $scope.vffmap.serviceFastFileGrid.columnDefs[pos].visible = false;


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
        $scope.gridApi.grouping.groupColumn('CustomerName');
        //$scope.gridApi.grouping.aggregateColumn('state', uiGridGroupingConstants.aggregation.COUNT);
    };

    $scope.addRowSummary = function () {
        var newService = {
            "FASTPreferenceMapID": 0,
            "FASTPreferenceMapName": "",
            "FASTProgramTypeId": "",
            "FASTProductTypeId": "",
            "FASTSearchTypeId": "",
            "ServiceId": "",
            "LocationId": "",
            "LoanPurposeTypeCodeId": "",
            "TenantId": "",
            "Tenant": ""
        };
        var rowTmp = {
        };
        rowTmp.entity = newService;
        vffmap.addRowSummary($scope.vffmap.serviceFastFileGrid, rowTmp);
    };
    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vffmap.serviceFastFileGrid.selectItem(index, false);
        $scope.vffmap.serviceFastFileGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        var index = $scope.vffmap.serviceFastFileGrid.data.indexOf(row.entity);
        $scope.vffmap.serviceFastFileGrid.data.splice(index, 1);
    };

    //Validator Operation
    vffmap.Validator = function () {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastFilePreferenceMappings/FastFile-Preference-validator.html',
            controller: 'psFastFilePreferenceValidatorCtrl',
            size: 'md',
            controllerAs: 'vffmapValidator',
            resolve: {
            }
        });
    }
}

psFastFilePreferenceMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psFastFilePreferenceMappingsRowEditor($http, $rootScope, $uibModal) {
    //var vffmap = this;
    var service = {
    };
    service.editRowSummary = editRowSummary;
    service.addRowSummary = addRowSummary;


    function editRowSummary(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psFastFilePreferenceMappings/FastFile-Preference-mappings-Edit.html',
            controller: 'psFastFilePreferenceMappingsRowEditCtrl',
            controllerAs: 'vffmap',
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
            templateUrl: 'ext-modules/psMappings/psFastFilePreferenceMappings/FastFile-Preference-mappings-add.html',
            controller: 'psFastFilePreferenceMappingsRowEditCtrl',
            controllerAs: 'vffmap',
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


//angular.module('psSecurity').controller('psMappingsCustomerRowEditCtrl', ['$http', '$modalInstance', 'grid', 'row', 
angular.module('psFastFilePreferenceMappings').controller('psFastFilePreferenceMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', '$filter', 'GetBusinessProgramService',
function psFastFilePreferenceMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, $filter, GetBusinessProgramService) {
    var vffmap = this;
    vffmap.entity = angular.copy(row.entity);
    vffmap.save = save;
    vffmap.LoadProgramType = LoadProgramType;
    vffmap.AddCondition = AddCondition;
    vffmap.LoadLocationbyCustId = LoadLocationbyCustId;
    vffmap.FormDirty = false;
    vffmap.CompareState =  function(a,b) {
        
            
        const bandA = a.PreferenceState.toUpperCase();
        const bandB = b.PreferenceState.toUpperCase();

            let comparison = 0;
            if (bandA > bandB) {
                comparison = 1;
            } else if (bandA < bandB) {
                comparison = -1;
            }
            return comparison;
         
    }
    vffmap.CompareCounty =  function(a, b) {

        // Use toUpperCase() to ignore character casing
        const bandA = a.county.toUpperCase();
        const bandB = b.county.toUpperCase();

        let comparison = 0;
        if (bandA == "ALL") {
            comparison = -1
        }else if (bandB == "ALL") {
            comparison = +1
        }
        else if (bandA > bandB) {
            comparison = 1;
        } else if (bandA < bandB) {
            comparison = -1;
        }
        return comparison;

    }



    vffmap.entity.Conditions = [];

    if (vffmap.entity.Tenant === '') {
        vffmap.entity.Tenant = $scope.tenantname;
    }

    vffmap.localLang = {
        selectAll: "Select All",
        selectNone: "Select None",
        reset: "Undo Changes",
        search: "search...",
        nothingSelected: "--Any--"
    }

    //Get ProgramList To Bind Dropdown
    vffmap.ProgramList = [];
    vffmap.ProductList = [];
    vffmap.SearchList = [];
    vffmap.LoanPurposeList = [];
    vffmap.customerList = [];
    vffmap.LocationList = [];
    vffmap.BusinessFASTProgramTypeList = [];
    vffmap.entity.BusinessProgramTypes = [];
    vffmap.FastProductList = [];

    var getIndexIfObjWithOwnAttr = function (array, attr, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                return i;
            }
        }
        return -1;
    }

    var containsObject = function (obj, list) {
        var i;
        for (i = 0; i < list.length; i++) {
            if (angular.equals(list[i], obj)) {
                return true;
            }
        }

        return false;
    };

    function AddProgramTypeforNullRegion() {
        vffmap.ProgramList = [{
            "ProgramTypeId": "0",
            "ProgramTypeName": "--NA--",
        }];
        vffmap.entity.FASTProgramTypeId = vffmap.ProgramList[0].ProgramTypeId;
        vffmap.entity.ProgramTypeName = vffmap.ProgramList[0].ProgramTypeName;
        return;
    }

    if ((vffmap.entity.FASTPreferenceMapID == '0' && vffmap.entity.RegionId == undefined)) {
        AddProgramTypeforNullRegion();
    } else {
        LoadProgramType(vffmap.entity.RegionId);
    }

    if (vffmap.entity.FASTPreferenceMapID != '0') {
        $http.get('FilePreferences/GetFastFilePreferenceDetailsByID/' + vffmap.entity.FASTPreferenceMapID).success(function (data) {

            vffmap.entity = data;
            if (data.CustomerId > 0 && data.CustomerId != undefined) {
                $http.get('FASTOffices/GetLocationsListByCustId/' + data.CustomerId)
                .success(function (data) {
                    vffmap.LocationList = data;
            });

                if (vffmap.entity.LocationDet != null &&
                    vffmap.entity.LocationDet != undefined) {
                    vffmap.entity.LocationDet.ExternalId = null;
                    vffmap.entity.LocationDet.CustomerId = null;
                    vffmap.entity.LocationDet.CustomerName = null;
                }
            }

            if (vffmap.entity.RegionId == '0') {
                AddProgramTypeforNullRegion();
            }

            vffmap.entity.LoanAmount = $filter('currency') (vffmap.entity.LoanAmount, '', 0);

            if (vffmap.entity.BusinessProgramTypes != null) {
                for (var i = 0; i < vffmap.entity.BusinessProgramTypes.length; i++) {

                    var tmp = getIndexIfObjWithOwnAttr(vffmap.BusinessFASTProgramTypeList, "Id", vffmap.entity.BusinessProgramTypes[i].Id);
                    if (tmp > -1)
                        vffmap.BusinessFASTProgramTypeList[tmp].Ticked = true;
                }
            }

            if (vffmap.entity.ProductTypes != null) {

                for (var i = 0; i < vffmap.entity.ProductTypes.length; i++) {

                    var tmp = getIndexIfObjWithOwnAttr(vffmap.ProductList, "ProductTypeId", vffmap.entity.ProductTypes[i].ProductTypeId);
                    if (tmp > -1)
                        vffmap.ProductList[tmp].Ticked = true;
                }
            }

            if (vffmap.entity.FastProductTypes != null) {

                for (var i = 0; i < vffmap.entity.FastProductTypes.length; i++) {

                    var tmp = getIndexIfObjWithOwnAttr(vffmap.FastProductList, "ProductTypeId", vffmap.entity.FastProductTypes[i].ProductTypeId);
                    if (tmp > -1)
                        vffmap.FastProductList[tmp].Ticked = true;
                }
            }

            vffmap.StateFipsList = [];
            
            $http.get('FilePreferences/GetStateFipsList').success(function (stateList) {

                for (var i = 0; i < vffmap.entity.Conditions.length; i++) {
                    for (var j = 0; j < stateList.length; j++) {
                        if (stateList[j].StateFIPS == vffmap.entity.Conditions[i].PreferenceState.StateFIPS && vffmap.entity.Conditions[i].PreferenceCounty.county === "ALL") {
                            stateList.splice(stateList.indexOf(stateList[j]), 1);
                        }

                    }
                    
                }
                vffmap.StateFipsList = stateList
            });

        });
    }

        ////$http.get('Locations/GetLocationsbyTenant').success(function (response) {
        ////    vffmap.LocationList = response;
        ////});

        //Get ProductList To Bind Dropdown
    $http.get('FilePreferences/GetLoanPurposeList').success(function (data) {
        vffmap.LoanPurposeList = data;
    });

        //Get ProductList To Bind Dropdown Later need to change to LVIS DropDown
    $http.get('FilePreferences/GetProductList/1').success(function (data) {
        vffmap.ProductList = data;
    });


    $http.get('FilePreferences/GetProductList/5').success(function (data) {
        vffmap.FastProductList = data;
    });

        //Get SearchList To Bind Dropdown
    $http.get('FilePreferences/GetSearchList').success(function (data) {
        vffmap.SearchList = data;
    });

    if (vffmap.entity.FASTPreferenceMapID == '0') {


        //Get ProgramList To Bind Dropdown
        vffmap.StateFipsList = [];

        $http.get('FilePreferences/GetStateFipsList').success(function (data) {
            vffmap.StateFipsList = data
        });
    }

    vffmap.RegionList = [];

        //function LoadRegion() {
    $http.get('Security/GetRegionsWithoutClaim/Fast').then(function (data) {
        vffmap.RegionList = data.data;
    });

    $http.get('Customers/GetTenantBasedCustomer').success(function (response) {
        vffmap.customerList = response;
        vffmap.entity.CustomerId = vffmap.entity.CustomerId;
    });

    


        function LoadLocationbyCustId(custId) {
        if (custId != undefined && custId > 0) {
            $http.get('FASTOffices/GetLocationsListByCustId/' + custId)
                .success(function (data) {
                    vffmap.LocationList = data;
                    vffmap.entity.LocationDet = '--Any--';
        });
        }
    }

        function LoadProgramType(RegionId) {
        if (RegionId != null) {
            $http.get('FilePreferences/GetProgramTypeList/' + RegionId).success(function (data) {
                if (data.length == 0) {
                    vffmap.ProgramList = [{
                        "ProgramTypeId": "0",
                        "ProgramTypeName": "--NA--",
                }];
                    return;
                }
                vffmap.ProgramList = data;
            });
        } else {
            AddProgramTypeforNullRegion();
        }


        var datapromise = GetBusinessProgramService.getBusinessProgramType(RegionId);
        datapromise.then(function (data) {
            if (data.length == 0) {
                vffmap.BusinessFASTProgramTypeList = [{
                    "Id": "0",
                    "Name": "--NA--",
            }];
                return;
        }

            vffmap.BusinessFASTProgramTypeList = data;
        });
    }

    vffmap.LoadCounty = LoadCounty;
    vffmap.CountyFipsList = [];

        function LoadCounty(StateFips) {
        if (StateFips != null) {
            $http.get('FilePreferences/GetCountyFipsList/' + StateFips).success(function (data) {
                if (data.length > 1) {
                    for (var i = 0; i < vffmap.entity.Conditions.length; i++) {
                        var cond = vffmap.entity.Conditions[i];
                        if (cond.PreferenceState.StateFIPS == StateFips) {
                            for (var j = 0; j < data.length; j++) {
                                var countyLocal = data[j];
                                if (countyLocal.countyFIPS == cond.PreferenceCounty.countyFIPS) {
                                    data.splice(data.indexOf(countyLocal), 1);
                                    if (data.length > 0 && data[0].county == "ALL")
                                        data.splice(0, 1);
                                }
                            }
                    }
                    }
                }

                vffmap.CountyFipsList = data;
                vffmap.county = vffmap.CountyFipsList[0];
        });
        }
    }

    if (vffmap.entity.Tenant == '') {
        vffmap.entity.Tenant = $scope.tenantname;
    }

        function AddCondition() {

        var Condition = {
                PreferenceState: vffmap.State, PreferenceCounty: vffmap.county
        };

        for (var i = 0; i < vffmap.entity.Conditions.length; i++) {
            if (vffmap.entity.Conditions[i].PreferenceState.StateFIPS == vffmap.State.StateFIPS &&
                vffmap.entity.Conditions[i].PreferenceCounty.countyFIPS == vffmap.county.countyFIPS) {
                return;
        }
            }
            if (vffmap.county.county == "ALL") {
                vffmap.StateFipsList.splice(vffmap.StateFipsList.indexOf(vffmap.State), 1)
                vffmap.CountyFipsList = [];
            }
            else {
                if (vffmap.CountyFipsList.length > 0 && vffmap.CountyFipsList[0].county == "ALL") {
                    vffmap.CountyFipsList.splice(0, 1);
                }
                vffmap.CountyFipsList.splice(vffmap.CountyFipsList.indexOf(vffmap.county),1);


            }



        vffmap.entity.Conditions.push(Condition);
    }

    vffmap.Remove = function Remove(Condition) {
        vffmap.FormDirty = true;
        var index = vffmap.entity.Conditions.indexOf(Condition);
        if (index >= 0)
            vffmap.entity.Conditions.splice(index, 1);

        if (Condition.PreferenceCounty.county == "ALL") {
            vffmap.StateFipsList.push(Condition.PreferenceState)
            vffmap.StateFipsList.sort(vffmap.CompareState)
            vffmap.CountyFipsList = [];
        }
        else if (vffmap.State.StateFIPS == Condition.PreferenceState.StateFIPS) {
            vffmap.CountyFipsList.push(Condition.PreferenceCounty);


            var addALL = true;
            for (var i = 0; i < vffmap.entity.Conditions.length; i++) {
                if (Condition.PreferenceState.StateFIPS == vffmap.entity.Conditions[i].PreferenceState.StateFIPS) {
                    addALL = false;
                    break;
                }
            }
            if (addALL) {
                vffmap.CountyFipsList.push({
                    "countyFIPS": "0",
                    "county": "ALL",
                    "State": 0
                })
            }
            vffmap.CountyFipsList.sort(vffmap.CompareCounty)
            


        }

       

        if (vffmap.entity.Conditions.length == 0) {

            vffmap.State = undefined;
            vffmap.county = undefined;


    }
    }
    vffmap.Change = function Change() {

        vffmap.FormDirty = true;
    }

        function save() {
            //vffmap.entity.LoanAmount = $filter('currency')(vffmap.entity.LoanAmount, '', 0);

        if (vffmap.entity.FASTPreferenceMapID == '0') {

            row.entity = angular.extend(row.entity, vffmap.entity);

            $http.post('FilePreferences/AddFastFile', row.entity)
            .success(function (data) {
                //real ID come back from response after the save in DB
                row.entity = data;
                grid.data.push(row.entity);

                if (data.length == 0) {
                    growl.error('FAST File Preference Info record is a duplicate and cannot be added');
                    return;
                }
                else {
                    growl.success("FAST File Preference Info record was added successfully");
            }
            }).error(function (response) {
                if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                    growl.error("There was an error adding FAST File Preference Info record");
                    return;
            }
                if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint") >= 0) {
                    growl.error('FAST File Preference Info record is a duplicate and cannot be added');
                    return;
            }
                growl.error(response.ExceptionMessage);
                return;
        });
        }
        else {
            vffmap.entity = angular.extend(row.entity, vffmap.entity);
            $http.post('FilePreferences/UpdateFastFile', row.entity)
             .success(function (data) {
                 if (data == 0) {
                     growl.error('FAST File Preference Info record is a duplicate and cannot be added');
                     return;
                 }
                 else {
                     //Added line to bind updated coloumn to ui-grid
                     row.entity = data;
                     grid.data = row.entity;
                     growl.success("FAST File Preference Info record was updated successfully");
             }

            }).error(function (response) {
                 if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                     growl.error("Error updating FAST File Preference Info record");
                     return;
            }
                 if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0) {
                     growl.error('FAST File Preference Info record is a duplicate and cannot be added');
                     return;
            }
                 growl.error(response.ExceptionMessage);
                 return;
        });
        }

        $uibModalInstance.close(row.entity);
    }

    vffmap.remove = remove;
        function remove() {
            //if (vffmap.entity.FASTPreferenceMapID != '0') {
            //row.entity = angular.extend(row.entity, vffmap.entity);
        $confirm({
                text: 'Proceed to delete this selection ?'
        }, {
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
                if (vffmap.entity.FASTPreferenceMapID != '0') {
                    row.entity = angular.extend(row.entity, vffmap.entity);
                    $http.post('FilePreferences/ConfirmFASTFilePreferenceDelete', row.entity.FASTPreferenceMapID)
                    .success(function (data) {
                        if (data > 0) {
                            var index = grid.appScope.vffmap.serviceFastFileGrid.data.indexOf(row.entity);
                            grid.appScope.vffmap.serviceFastFileGrid.data.splice(index, 1);
                            growl.success("FAST File Preference Info record was deleted successfully");
                            //return;
                        }
                        else {
                            growl.error('There was an error deleting FAST File Preference Info record');
                            //return;
                    }
                    }).error(function (response) {
                        growl.error('There was an error deleting FAST File Preference Info record. Exception: ' + response.ExceptionMessage);
                        return;
                });
            }
        });
        $uibModalInstance.close(row.entity);
    }
}]);

angular.module('psFastFilePreferenceMappings').controller('psFastFilePreferenceValidatorCtrl', ['$http', '$scope', '$uibModalInstance', 'growl', '$filter', '$uibModalStack',
function psFastFilePreferenceValidatorCtrl($http, $scope, $uibModalInstance, growl, $filter, $uibModalStack) {
    var vffmapValidator = this;
    var Tenatnt = $scope.tenantname;

    vffmapValidator.StateFipsList = [];
    $http.get('FilePreferences/GetStateFipsList').success(function (data) {
        vffmapValidator.StateFipsList = data
    });


    vffmapValidator.LoanAmount = 0;
    vffmapValidator.LoadCounty = LoadCounty;
    vffmapValidator.CountyFipsList = [];
    vffmapValidator.ProductList = [];

    vffmapValidator.localLang = {
        selectAll: "Select All",
        selectNone: "Select None",
        reset: "Undo Changes",
        search: "search...",
        nothingSelected: "--Any--"
    }


    function LoadCounty(StateFips) {
        if (StateFips != null) {
            $http.get('FilePreferences/GetCountyFipsList/' + StateFips).success(function (data) {
                vffmapValidator.CountyFipsList = data;
                vffmapValidator.county = vffmapValidator.CountyFipsList[0];
            });
        }
    }


    vffmapValidator.ServiceList = [];
    $http.get('InboundDocs/GetServices').success(function (data) {
        vffmapValidator.ServiceList = data;
    });

    vffmapValidator.LocationList = [];
    $http.get('Locations/GetLocationsbyTenant').success(function (response) {
        vffmapValidator.LocationList = response;
    });

    vffmapValidator.RegionList = [];
    $http.get('Security/GetRegionsWithoutClaim/Fast').then(function (data) {
        vffmapValidator.RegionList = data.data;
    });

    vffmapValidator.LoanPurposeList = [];
    //Get ProductList To Bind Dropdown
    $http.get('FilePreferences/GetLoanPurposeList').success(function (data) {
        vffmapValidator.LoanPurposeList = data;
    });


    vffmapValidator.TenantList = [];
    var IsVisibleTenantList = false;
    var selTenantId = 0;
    //Get TenantList To Bind Dropdown
    $http.get('FilePreferences/GetTenantList').success(function (data) {
        vffmapValidator.TenantList = data;
        if (Tenatnt != 'LVIS' && vffmapValidator.TenantList.length > 0) {
            angular.forEach(vffmapValidator.TenantList, function (item) {
                if (Tenatnt === item.TenantName) {
                    IsVisibleTenantList = true;
                    $scope.IsVisibleTenantList = IsVisibleTenantList;
                    selTenantId = item.TenantId;
                    //$scope.selTenantId = selTenantId;                   
                    vffmapValidator.TenantList = $filter('filter')(vffmapValidator.TenantList, { TenantId: selTenantId });
                    vffmapValidator.entity.TenantId = selTenantId;
                }
            });
        }
    });


    //Get ProductList To Bind Dropdown Later need to change to LVIS DropDown
    $http.get('FilePreferences/GetProductList/1').success(function (data) {
        vffmapValidator.ProductList = data;
    });

    vffmapValidator.FastFilePreferenceData = [];
    vffmapValidator.hasAccess = false;
    vffmapValidator.Validate = function () {
        if (vffmapValidator.entity.ServiceId == '0') {
            vffmapValidator.entity.ServiceId = 0;
        }
        if (vffmapValidator.entity.LoanPurposeDet == '0') {
            vffmapValidator.entity.LoanPurposeDet = 0;
        }

        var serviceId = 0;
        if (vffmapValidator.entity.ServiceId != '') {
            serviceId = vffmapValidator.entity.ServiceId.ID;
        }
        else {
            serviceId = 0;
        }
        var LoanPurposeDet = 0;
        if (vffmapValidator.entity.LoanPurposeDet != '') {
            LoanPurposeDet = vffmapValidator.entity.LoanPurposeDet.ID;
        }
        else {
            LoanPurposeDet = 0;
        }

        var ProductId = 0;
        if (vffmapValidator.entity.ProductId == null) {
            ProductId = 0;
        }
        else {
            ProductId = vffmapValidator.entity.ProductId;
        }

        $http.post('FilePreferences/GetValidatorFilePreferences/' + vffmapValidator.State.StateFIPS + '/' + vffmapValidator.county.county + '/' + vffmapValidator.entity.LoanAmount + "/" + serviceId + "/" + vffmapValidator.entity.LocationId + "/" + vffmapValidator.entity.RegionId + "/" + LoanPurposeDet + "/" + vffmapValidator.entity.TenantId + "/" + ProductId)
        .then(function (response) {
            if (response.data.length != 0) {
                vffmapValidator.FastFilePreferenceData = response.data;
            }
            else {

                growl.error('No File Preferences found based on your look up criteria.');
                return;
            }
        },
        function (error) {
            vffmapValidator.FastFilePreferenceData = 'Mappings not available.';
            //growl.error("There was an error in deleting this exception");
            //vffmapValidator.MessageDeletechanged = false;
        });
    };
}]);










