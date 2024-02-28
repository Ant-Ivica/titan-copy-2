"use strict";

angular.module('psCustomerMappings').controller('psCustomerMappingsController', psCustomerMappingsController);
angular.module('psCustomerMappings').service('psCustomerMappingsRowEditor', psCustomerMappingsRowEditor);
angular.module('psCustomerMappings').service('psCustomerMappingsApiUri', psCustomerMappingsApiUri);

psCustomerMappingsController.$inject = ['$scope', '$rootScope', '$http', '$interval', '$uibModal', 'psCustomerMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$window', 'UserInfo', '$q', '$cookies', 'growl', '$route', 'ServicePreferenceMapping'];
function psCustomerMappingsController($scope, $rootScope, $http, $interval, $uibModal, psCustomerMappingsRowEditor, uiGridGroupingConstants, $location, $window, UserInfo, $q, $cookies, growl, $route, ServicePreferenceMapping) {
    var vlmap = this;

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

    $scope.isActive = function (viewLocation) {
        return viewLocation === $location.path();
    };

    vlmap.editRowSummary = psCustomerMappingsRowEditor.editRowSummary;
    vlmap.addRowSummary = psCustomerMappingsRowEditor.addRowSummary;
    vlmap.removeRow = psCustomerMappingsRowEditor.removeRow;

    vlmap.showDocuments = psCustomerMappingsRowEditor.showDocuments;
    vlmap.showEvents = psCustomerMappingsRowEditor.showEvents;

    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>'
    var branchdocevent = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents">' +
        '<a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-map-marker" tooltip-placement="bottom" uib-tooltip="Locations" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.ShowBranches(row)"></a>' +
        '<a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-list-alt" tooltip-placement="bottom" uib-tooltip="Contact Provider" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.ShowContactProviderDetails(row)"></a>' +
        '<a ng-show="(row.treeNode.children && row.treeNode.children.length == 0)"  class="fa fa-file-text" tooltip-placement="bottom" uib-tooltip="Outbound Document" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showDocuments(row)"></a>' +
        '<a ng-show="(row.treeNode.children && row.treeNode.children.length == 0 && row.entity.Applicationid != 34)"  class="fa fa-newspaper-o" tooltip-placement="bottom" uib-tooltip="Subscription(s)" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showSubscriptions(row)"></a>' +
        '<a ng-show="(row.entity.TenantId == 8 && row.entity.Applicationid == 34)" tooltip-placement="bottom" uib-tooltip="Webhook(s)" style="padding:5px 15px;text-align:center;cursor:pointer" ng-click="grid.appScope.showWebHooks(row)"><img src="images/webhook.png" height="15px" width="15px" style="margin-top:-5px"></img></a></div>'

    vlmap.serviceCustomerGrid = {
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
        exporterCsvFilename: 'CustomersInfo.csv',
        columnDefs: [
          { field: 'LVISCustomerID', name: 'Customer ID', displayName: 'Customer ID', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, sort: { priority: 0, direction: 'asc' } },
          { field: 'CustomerName', name: 'Customer Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'Category', name: 'Category', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'ApplicationName', name: 'Application', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'Tenant', name: 'Tenant', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false },
          { field: 'Locationdocevent', name: '  ', headerCellClass: 'grid-header', enableColumnMenu: false, enableFiltering: false, groupingShowAggregationMenu: false, cellTemplate: branchdocevent }

        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.vlmap.editRowSummary(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vlmap.serviceCustomerGrid.gridApi = gridApi;

        }
    };

    if ($rootScope.activityright) {
        $http.get('Customers/GetCustomers/').success(function (response) {

            //Set filter - customer name when re-route link from location mappings
            var url = $location.path().split('/');
            var CustName = url[2];

            if (CustName != null) {
                var indexpos = $scope.vlmap.serviceCustomerGrid.columnDefs.map(function (e) { return e.field; }).indexOf('CustomerName');

                indexpos = (indexpos != null) ? parseInt(indexpos) + 1 : null;

                if (indexpos != null) {
                    vlmap.serviceCustomerGrid.gridApi.grid.columns[indexpos].filters[0] = {
                        term: CustName
                    };
                }
            }

            vlmap.serviceCustomerGrid.data = response;

        });
    }

    $http.get('Security/GetTenant')
    .then(function (response) {
        $scope.Tenant = response.data;
    });


    //COde Hide/Display Tenant GridColumn based on Conditions
    var pos = $scope.vlmap.serviceCustomerGrid.columnDefs.map(function (e) { return e.field; }).indexOf('Tenant');
    if ($rootScope.tenantname == 'LVIS')
        $scope.vlmap.serviceCustomerGrid.columnDefs[pos].visible = true;
    else
        $scope.vlmap.serviceCustomerGrid.columnDefs[pos].visible = false;

    $scope.expandAll = function () {
        $scope.gridApi.treeBase.expandAllRows();
    };

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };
    $scope.toggleRow = function (rowNum) {
        $scope.gridApi.treeBase.toggleRowTreeState($scope.gridApi.grid.renderContainers.body.visibleRowCache[rowNum]);
    };


    $scope.addRowSummary = function () {
        var newService = {
            "LVISCustomerID": 0,
            "CustomerName": "",
            "CategoryId": "",
            "Category": "",
            "Applicationid": "",
            "TenantId": "",
            "Tenant": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vlmap.addRowSummary($scope.vlmap.serviceCustomerGrid, rowTmp);
    };
    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vlmap.serviceCustomerGrid.selectItem(index, false);
        $scope.vlmap.serviceCustomerGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        var index = $scope.vlmap.serviceCustomerGrid.data.indexOf(row.entity);
        $scope.vlmap.serviceCustomerGrid.data.splice(index, 1);
    };

    $scope.showDocuments = function (row) {
        $window.location = "#/outdocmappings/" + row.entity.CustomerName + ":" + row.entity.LVISCustomerID + ":" + row.entity.Applicationid + "/false";
    };

    $scope.showEvents = function (row) {
        $window.location = "#/outeventmappings/" + row.entity.CustomerName + ":" + row.entity.LVISCustomerID;
    };

    $scope.ShowBranches = function (row) {
        ServicePreferenceMapping.InternalApplicationId = row.entity.Applicationid;
        $window.location = "#/Locationmappings/" + row.entity.CustomerName + ":" + row.entity.LVISCustomerID;
    };
    $scope.ShowContactProviderDetails = function (row) {
        $window.location = "#/ContactProvidermappings/" + row.entity.CustomerName + ":" + row.entity.LVISCustomerID + ":" + row.entity.Tenant + ":" + row.entity.TenantId;
    };
    $scope.showSubscriptions = function (row) {
        $window.location = "#/subscription/" + row.entity.CustomerName + ":" + row.entity.LVISCustomerID + ":" + row.entity.Applicationid + ":" + row.entity.TenantId + ":" + row.entity.Tenant + "/false";
    };

    $scope.showWebHooks = function (row) {
        $window.location = "#/webhooksMappings/" + row.entity.CustomerName + ":" + row.entity.LVISCustomerID;
    };
}

psCustomerMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psCustomerMappingsRowEditor($http, $rootScope, $uibModal) {
    //var vlmap = this;
    var service = {};
    service.editRowSummary = editRowSummary;
    service.addRowSummary = addRowSummary;

    function editRowSummary(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psCustomerMappings/customer-mappings-edit.html',
            controller: 'psCustomerMappingsRowEditCtrl',
            controllerAs: 'vlmap',
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
            templateUrl: 'ext-modules/psMappings/psCustomerMappings/customer-mappings-add.html',
            controller: 'psCustomerMappingsRowEditCtrl',
            controllerAs: 'vlmap',
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
angular.module('psCustomerMappings').controller('psCustomerMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', 'psCustomerMappingsApiUri',
    function psCustomerMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, psCustomerMappingsApiUri) {
        var vlmap = this;
        vlmap.entity = angular.copy(row.entity);
        vlmap.save = save;
        vlmap.entity.CopyCustomerPrefernce = vlmap.entity.CustomerPreference;
        vlmap.entity.CopyServicePrefernce = vlmap.entity.ServicePreference;
        vlmap.CAPIShow = false;
        vlmap.slaSubtypeShow = false;
        vlmap.dtSubtypeShow = false;
        vlmap.dtSubTypeCredential = false;
        $scope.ServicePreferenceShow = false;
        vlmap.CustomerSLAPrefernceSubTypeList = [];
        vlmap.CustomerDTPreferenceSubTypeList = [];
        vlmap.ServiceList = [];
        vlmap.ServicePrefrences = [];
        vlmap.entity.ServicePrefrence = [];
        vlmap.entity.GenerateCredential = false;
        vlmap.IsCustomerCredentialEditAllowed = false;
        vlmap.cusotmerCredentialAutherization = function () {
            var applicationId = vlmap.entity.Applicationid ? vlmap.entity.Applicationid : 0;
            $http.get('Customers/IsCustomerCredentialEditAllowed/' + applicationId).success(function (data) {
                vlmap.IsCustomerCredentialEditAllowed = data;
                if (vlmap.IsCustomerCredentialEditAllowed) {
                    $http.get('Customers/GetUserName/' + vlmap.entity.LVISCustomerID +"/" + applicationId).success(function (data) {
                        vlmap.entity.CustomerUserId = data;
                    });
                }
            });
        }
        vlmap.cusotmerCredentialAutherization();


        var getIndexIfObjWithOwnAttr = function (array, attr, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                    return i;
                }
            }
            return -1;
        }

        if (vlmap.entity.CustomerPreference != undefined && vlmap.entity.CustomerPreference.length > 0) {
            vlmap.entity.CustomerPreference.some(function (val) {
                if (val.TypeCodeDesc == 'Deliver Fee Sheet' && val.TypeCodeId == 801) {
                    vlmap.CAPIShow = true;
                } else if (val.TypeCodeDesc == 'SLA Preference Type' && val.TypeCodeId == 820) {
                    vlmap.slaSubtypeShow = true;
                } else if (val.TypeCodeDesc == 'Delivery Type' && val.TypeCodeId == 825) {
                    vlmap.dtSubtypeShow = true;
                }
            })
        }

        vlmap.LoadServicePreference = function () {
            if (vlmap.entity.Applicationid === 9)
            { $scope.ServicePreferenceShow = true; }
            else { $scope.ServicePreferenceShow = false; }
            vlmap.cusotmerCredentialAutherization();
            vlmap.entity.GenerateCredential = false;
        }

        vlmap.checkAll = function (checked) {
            vlmap.CAPIShow = true;
            vlmap.slaSubtypeShow = true;
            vlmap.dtSubtypeShow = true;
            if (vlmap.selectedAll) {
                vlmap.selectedAll = true;
                vlmap.unselectedAll = false;

                angular.forEach(vlmap.CustomerPrefernceList, function (item) {
                    item.Ticked = vlmap.selectedAll;
                });
                vlmap.entity.CustomerPreference = angular.copy(vlmap.CustomerPrefernceList);

                angular.forEach(vlmap.CustomerSLAPrefernceSubTypeList, function (item) {
                    item.Ticked = vlmap.selectedAll;
                });

                vlmap.entity.CustomerPreference.concat(vlmap.CustomerSLAPrefernceSubTypeList);

            }
        };

        vlmap.uncheckAll = function (checked) {
            vlmap.CAPIShow = false;
            vlmap.slaSubtypeShow = false;
            vlmap.dtSubtypeShow = false;
            if (vlmap.unselectedAll) {
                vlmap.selectedAll = false;
                vlmap.entity.CustomerPreference = [];
                angular.forEach(vlmap.CustomerPrefernceList, function (item) {
                    item.Ticked = false;
                });
                angular.forEach(vlmap.CustomerSLAPrefernceSubTypeList, function (item) {
                    item.Ticked = false;
                });
                angular.forEach(vlmap.CustomerDTPreferenceSubTypeList, function (item) {
                    item.Ticked = false;
                });
                vlmap.entity.CustomerPreference = [];
                vlmap.entity.CAPIClientID = "";
                vlmap.entity.ServiceSelect = '--Any--';
                vlmap.entity.DTDeliveryUrl = "";
                vlmap.entity.DTCredentials = "";
            }
        };

        vlmap.insertCodeType = function (role, inputVal) {
            if (inputVal != "" && inputVal != undefined) {
                role.Ticked = true;
                vlmap.Test(role);
            } else {
                role.Ticked = false;
                vlmap.Test(role);
            }
        }

        vlmap.insertSelectedType = function (selectedValue) {
            if (selectedValue != null) {
                vlmap.CustomerDTPreferenceSubTypeList.forEach(function (val) {
                    if (val.TypeCodeDesc == selectedValue.TypeCodeDesc) {
                        selectedValue.Ticked = true;
                        vlmap.Test(selectedValue);
                    }
                });
            } else {
                vlmap.entity.CustomerPreference.forEach(function (val) {
                    if (val.TypeCodeId == 826 || val.TypeCodeId == 827 || val.TypeCodeId == 828) {
                        val.Ticked = false;
                        vlmap.Test(val);
                    }
                })
            }
        }
        vlmap.GeratePassword = function () {


            if (vlmap.entity.CustomerUserId != vlmap.entity.CustomerUserId.match(/[a-zA-Z0-9\_]{4,15}/)) {
                $confirm({ text: 'Proceed to delete this selection?' }, {
                    size: 'sm', defaultLabels: {
                        title: ' Confirm Delete ',
                        ok: 'Yes',
                        cancel: 'No'
                    },
                    template: '<div class="widget-header"><h2>Invalid user Id</h2></div>' +
                        '<div class="widget-content">To Generate Password <b>Please Enter Valid UserId</b></div>' +
                        '<div class="modal-footer">' +
                        '<button class="btn btn-danger" ng-click="cancel()">Close</button>' +
                        '</div>'
                }).then(function () {
                    vlmap.entity.GenerateCredential = true;
                });
                return;
            }

            if (!vlmap.entity.GenerateCredential) {

                $http.get('Customers/IsUniqueUserName/' + vlmap.entity.CustomerUserId + "/" + vlmap.entity.LVISCustomerID).success(function (data) {
                    if (data) {
                        $confirm({ text: 'Proceed to generate credentials?' }, {
                            size: 'sm', defaultLabels: {
                                title: 'Confirm Generate Credentials ',
                                ok: 'Yes',
                                cancel: 'No'
                            },
                            template: '<div class="widget-header"><h2><i class="fa fa-question-circle" aria-hidden="true" style="padding-left:0px;"></i>Confirmation</h2></div>' +
                                '<div class="widget-content">Proceed to <b>generate password</b></div>' +
                                '<div class="modal-footer">' +
                                '<button class="btn btn-success" ng-click="ok()">Ok</button>' +
                                '<button class="btn btn-danger" ng-click="cancel()">Close</button>' +
                                '</div>'
                        }).then(function () {
                            vlmap.entity.GenerateCredential = true;
                        });
                    }
                    else {
                        $confirm({ text: 'Proceed to delete this selection?' }, {
                            size: 'sm', defaultLabels: {
                                title: ' Confirm Delete ',
                                ok: 'Yes',
                                cancel: 'No'
                            },
                            template: '<div class="widget-header"><h2>Duplicate user Id</h2></div>' +
                                '<div class="widget-content">To Generate Password <b>Please Enter Unique UserId</b></div>' +
                                '<div class="modal-footer">' +
                                '<button class="btn btn-danger" ng-click="cancel()">Close</button>' +
                                '</div>'
                        }).then(function () {
                            vlmap.entity.GenerateCredential = true;
                        });
                    }
                });

            }
        }
        vlmap.Test = function (role) {
            vlmap.selectedAll = false;
            vlmap.unselectedAll = false;

            var tmp = getIndexIfObjWithOwnAttr(vlmap.entity.CustomerPreference, "TypeCodeId", role.TypeCodeId);

            if (role.Ticked && tmp < 0) {

                vlmap.entity.CustomerPreference.push(role);
            }
            else if (!role.Ticked && tmp > -1) {

                vlmap.entity.CustomerPreference.splice(tmp, 1);
            }

            if (vlmap.entity.CustomerPreference.length > 0) {
                vlmap.CAPIShow = false;
                vlmap.slaSubtypeShow = false;
                vlmap.dtSubtypeShow = false;
                vlmap.entity.CustomerPreference.some(function (val) {
                    if (val.TypeCodeDesc == 'Deliver Fee Sheet' && val.TypeCodeId == 801) {
                        vlmap.CAPIShow = true;
                    } else if (val.TypeCodeDesc == 'SLA Preference Type' && val.TypeCodeId == 820) {
                        vlmap.slaSubtypeShow = true;
                    } else if (val.TypeCodeDesc == 'Delivery Type' && val.TypeCodeId == 825) {
                        vlmap.dtSubtypeShow = true;
                    }
                })

                if (role.TypeCodeId == 809) {
                    vlmap.entity.GenerateCredential = false;
                }
                if (vlmap.slaSubtypeShow == false && role.TypeCodeId == 820) {

                    if (vlmap.entity.CustomerPreference != null) {

                        var copyCustomerPref = vlmap.entity.CustomerPreference.slice();

                        for (var i = 0; i < copyCustomerPref.length; i++) {

                            for (var j = 0; j < vlmap.CustomerSLAPrefernceSubTypeList.length; j++) {

                                if (copyCustomerPref[i].TypeCodeId == vlmap.CustomerSLAPrefernceSubTypeList[j].TypeCodeId) {
                                    vlmap.CustomerSLAPrefernceSubTypeList[j].Ticked = false;
                                    vlmap.entity.CustomerPreference.some(function (e, k) {
                                        if (e.TypeCodeId == copyCustomerPref[i].TypeCodeId) {
                                            vlmap.entity.CustomerPreference.splice(k, 1);
                                            return;
                                        }
                                    });
                                }
                            }

                        }
                    }
                }

                if (vlmap.CAPIShow == false && role.TypeCodeId == 801) {
                    vlmap.entity.CAPIClientID = "";
                }

                if (vlmap.dtSubtypeShow == false && role.TypeCodeId == 825) {
                    vlmap.entity.ServiceSelect = '--Any--';
                    vlmap.entity.DTDeliveryUrl = "";
                    vlmap.entity.DTCredentials = "";

                    if (vlmap.entity.CustomerPreference != null) {

                        var copyCustomerPref = vlmap.entity.CustomerPreference.slice();

                        for (var i = 0; i < copyCustomerPref.length; i++) {

                            for (var j = 0; j < vlmap.CustomerDTPreferenceSubTypeList.length; j++) {

                                if (copyCustomerPref[i].TypeCodeId == vlmap.CustomerDTPreferenceSubTypeList[j].TypeCodeId) {
                                    vlmap.CustomerDTPreferenceSubTypeList[j].Ticked = false;
                                    vlmap.entity.CustomerPreference.some(function (e, k) {
                                        if (e.TypeCodeId == copyCustomerPref[i].TypeCodeId) {
                                            vlmap.entity.CustomerPreference.splice(k, 1);
                                            return;
                                        }
                                    });
                                }
                            }

                        }
                    }

                }

            } else {
                vlmap.CAPIShow = false;
                vlmap.slaSubtypeShow = false;
                vlmap.dtSubtypeShow = false;
                vlmap.entity.CAPIClientID = "";
                vlmap.entity.ServiceSelect = '--Any--';
                vlmap.entity.DTDeliveryUrl = "";
                vlmap.entity.DTCredentials = "";
            }
        };

        //Get CategoryList To Bind Dropdown
        vlmap.CategoryList = [];
        $http.get('Customers/GetCategoryList').success(function (data) {
            vlmap.CategoryList = data
        });

        //Get Application List To Bind Dropdown
        vlmap.ApplicationList = [];
        $http.get('Security/GetExternalApplications').success(function (data) {
            vlmap.ApplicationList = data;
        });

        $http.get('InboundDocs/GetServices').success(function (data) {
            if (data.length > 0) {
                data.splice(1, 1) //remove Signing service 
            }
            vlmap.ServicePrefrences = data;

            if (vlmap.entity.LVISCustomerID != '0' && vlmap.entity.Applicationid === 9) {
                vlmap.entity.ServicePreference = vlmap.entity.CopyServicePrefernce;
                $scope.ServicePreferenceShow = true;
                if (vlmap.entity.ServicePreference != null) {
                    for (var i = 0; i < vlmap.entity.ServicePreference.length; i++) {

                        var tmp = getIndexIfObjWithOwnAttr(vlmap.ServicePrefrences, "ID", vlmap.entity.ServicePreference[i].ID);
                        if (tmp > -1)
                            vlmap.ServicePrefrences[tmp].Ticked = true;
                    }
                }
            }
            if ((vlmap.ServicePrefrences != null && vlmap.entity.LVISCustomerID === 0) &&
                (vlmap.entity.Tenant === 'IBSS' || vlmap.entity.Tenant === 'Agency' || vlmap.entity.Tenant === 'MortgageSolutions' ||
                                                                  vlmap.entity.Tenant === 'RLA')) {
                for (var i = 0; i < vlmap.ServicePrefrences.length; i++) {
                    vlmap.ServicePrefrences[i].Ticked = true;
                }
            }

        });

        //Get Customer Preference To Bind Checkbox
        vlmap.CustomerPrefernceList = [];
        $http.get('Customers/GetCustomerPreferenceTypes').success(function (data) {
            vlmap.CustomerPrefernceList = data;
            subTypeLoad();

            if (vlmap.entity.LVISCustomerID != '0') {

                vlmap.entity.CustomerPreference = vlmap.entity.CopyCustomerPrefernce;

                if (vlmap.entity.CustomerPreference != null) {

                    for (var i = 0; i < vlmap.entity.CustomerPreference.length; i++) {

                        var tmp = getIndexIfObjWithOwnAttr(vlmap.CustomerPrefernceList, "TypeCodeId", vlmap.entity.CustomerPreference[i].TypeCodeId);
                        if (tmp > -1)
                            vlmap.CustomerPrefernceList[tmp].Ticked = true;
                    }
                }
            }

        });

        //Get Customer Preference Subtype Bind Checkbox

        function subTypeLoad() {
            vlmap.CustomerPrefernceList.forEach(function (data) {
                switch (data.TypeCodeId) {
                    case 820:
                        getSLAPreferenceSubType(data.TypeCodeId)
                        break;
                    case 825:
                        getSLAPreferenceSubType(data.TypeCodeId)
                        break;
                    case 809:
                        getSLAPreferenceSubType(data.TypeCodeId)
                        break;
                }

            });
        }


        function getSLAPreferenceSubType(typeCodeID) {
            $http.get('Customers/GetCustomerPreferenceSubTypes/' + typeCodeID).success(function (data) {
                if (data.length > 0 && typeCodeID == 820) {
                    data.forEach(function (val) {
                        if (val.TypeCodeDesc.indexOf('Description') > -1) {
                            var arrayItem = val.TypeCodeDesc.split(',');
                            arrayItem.forEach(function (item) {
                                if (item.indexOf('Description') > -1) {
                                    var spec = item.split(':');
                                    spec.forEach(function (actual) {
                                        if (actual.indexOf('Description') == -1) {
                                            val.TypeCodeDesc = actual;
                                            vlmap.CustomerSLAPrefernceSubTypeList.push(val);
                                        }
                                    })
                                }
                            })
                        } else {
                            vlmap.CustomerSLAPrefernceSubTypeList.push(val);
                        }

                    })
                } else if (data.length > 0 && typeCodeID == 825) {
                    vlmap.CustomerDTPreferenceSubTypeList = data;
                    vlmap.CustomerDTPreferenceSubTypeList.forEach(function (data) {
                        if (data.TypeCodeId == 826 || data.TypeCodeId == 827 || data.TypeCodeId == 828) {
                            vlmap.ServiceList.push(data);
                        }
                        if (vlmap.entity.ServiceSelect != undefined && vlmap.entity.ServiceSelect == data.TypeCodeDesc) {
                            vlmap.entity.ServiceSelect = data;
                        }
                    });
                }

                ////vlmap.CustomerSLAPrefernceSubTypeList = data;
                if (vlmap.entity.LVISCustomerID != '0') {

                    vlmap.entity.CustomerPreference = vlmap.entity.CopyCustomerPrefernce;

                    if (vlmap.entity.CustomerPreference != null) {

                        for (var i = 0; i < vlmap.entity.CustomerPreference.length; i++) {

                            var tmp = getIndexIfObjWithOwnAttr(vlmap.CustomerSLAPrefernceSubTypeList, "TypeCodeId", vlmap.entity.CustomerPreference[i].TypeCodeId);
                            if (tmp > -1)
                                vlmap.CustomerSLAPrefernceSubTypeList[tmp].Ticked = true;
                        }
                    }
                }
            })
        }

        if (vlmap.entity.Tenant == '') {
            vlmap.entity.Tenant = $scope.tenantname;
        }

        var getIndexIfObjWithOwnAttr = function (array, attr, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                    return i;
                }
            }
            return -1;
        }

        function save() {
            if (vlmap.entity.LVISCustomerID == '0') {
                row.entity = angular.extend(row.entity, vlmap.entity);
                row.entity.ServiceSelect = (row.entity.ServiceSelect != null && row.entity.ServiceSelect != '--Any--') ? row.entity.ServiceSelect.TypeCodeDesc : "";
                $http.post(psCustomerMappingsApiUri.AddCustomer, row.entity)
            .success(function (data) {
                //real ID come back from response after the save in DB
                row.entity = data;
                grid.data.push(row.entity);

                if (data.length == 0) {
                    growl.error('Customer Info record is a duplicate and cannot be added');
                    return;
                }
                else {
                    growl.success("Customer Info record was added successfully");
                }
            }).error(function (response) {

                if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                    growl.error("Customer Info record could not be added");
                    return;
                }
                if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0
                    || response.InnerException.InnerException.ExceptionMessage.indexOf("Cannot insert duplicate key row in object") >= 0) {
                    growl.error('Customer Info record is a duplicate and cannot be added');
                    return;
                }
                growl.error(response.InnerException.InnerException.ExceptionMessage);
                return;
            });

                //grid.data.push(vlmap.entity);

            } else {

                vlmap.entity = angular.extend(row.entity, vlmap.entity);
                row.entity.ServiceSelect = (row.entity.ServiceSelect != null && row.entity.ServiceSelect != '--Any--') ? row.entity.ServiceSelect.TypeCodeDesc : "";
                $http.post(psCustomerMappingsApiUri.UpdateCustomer, row.entity)
             .success(function (data) {
                 if (data == 0) {
                     growl.error('Customer Info record is a duplicate and cannot be added');
                     return;
                 }
                 else {
                     //Added line to bind updated coloumn to ui-grid
                     row.entity = data;
                     grid.data = row.entity;
                     growl.success("Customer Info record was updated successfully");
                 }

             }).error(function (response) {
                 if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                     growl.error("Customer Info record could not be added");
                     return;
                 }
                 if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0
                     || response.InnerException.InnerException.ExceptionMessage.indexOf("Cannot insert duplicate key row in object") >= 0) {
                     growl.error('Customer Info record is a duplicate and cannot be added');
                     return;
                 }
                 growl.error(response.InnerException.InnerException.ExceptionMessage);
                 return;
             });
            }
            $uibModalInstance.close(row.entity);
        }

        vlmap.remove = remove;
        function remove() {
            if (row.entity.LVISCustomerID != '0') {
                row.entity = angular.extend(row.entity, vlmap.entity);
                $http.post('Customers/DeleteCustomer', row.entity.LVISCustomerID)
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
                       $confirm({ text: 'Proceed to delete this selection?' }, {
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
                           if (row.entity.LVISCustomerID != '0') {
                               row.entity = angular.extend(row.entity, vlmap.entity);
                               $http.post('Customers/ConfirmDeleteCustomer', row.entity.LVISCustomerID)
                              .success(function (data) {
                                  if (data === 1) {
                                      var index = grid.appScope.vlmap.serviceCustomerGrid.data.indexOf(row.entity);
                                      grid.appScope.vlmap.serviceCustomerGrid.data.splice(index, 1);
                                      growl.success("Customer Info record was deleted successfully");
                                      return;
                                  }
                                  else {
                                      growl.error("There was an error deleting Customer Info record for Customer: " + row.entity.CustomerName);
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

function psCustomerMappingsApiUri() {
    var custoMap = this;

    custoMap.AddCustomer = 'api/CustomerMappings/AddCustomer';

    custoMap.UpdateCustomer = 'api/CustomerMappings/UpdateCustomer';

}
