"use strict";


angular.module('psWebhooksMappings').controller('psWebhooksMappingsController', psWebhooksMappingsController);
angular.module('psWebhooksMappings').service('psWebhooksMappingsControllerRowEditor', psWebhooksMappingsControllerRowEditor);

angular.module('psWebhooksMappings').service('psWebhooksMappingsApiUri', psWebhooksMappingsApiUri);

psWebhooksMappingsController.$inject = ['$scope', '$http', '$interval', '$uibModal', 'psWebhooksMappingsControllerRowEditor',  'uiGridGroupingConstants', '$route', '$routeParams', 'growl', 'UserInfo', '$rootScope', '$confirm', '$cookies', '$window'];
function psWebhooksMappingsController($scope, $http, $interval, $uibModal, psWebhooksMappingsControllerRowEditor, uiGridGroupingConstants, $route, $routeParams, growl, UserInfo, $rootScope, $confirm, $cookies, $window) {
    var webhook = this;

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
    ;
    var customerDetail = $routeParams.CustomerName.split(":");
    webhook.CustomerName = customerDetail[0];
    webhook.CustomerID = customerDetail[1];

    
    webhook.editRowSummary = psWebhooksMappingsControllerRowEditor.editRowSummary;
    webhook.addRowSummary = psWebhooksMappingsControllerRowEditor.addRowSummary;
    webhook.editWebhookDomainList = psWebhooksMappingsControllerRowEditor.editWebhookDomainList;

    webhook.serviceBranchGrid = {
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
        exporterCsvFilename: 'webhook.csv',
        columnDefs: [
            { field: 'UserID', name: 'Id', headerCellClass: 'grid-header', enableCellEdit: false, visible: false },
            { field: 'URL', name: 'URL', displayName: 'URL', headerCellClass: 'grid-header', enableCellEdit: false },
            { field: 'Secret', name: 'Secret', headerCellClass: 'grid-header', enableCellEdit: false },
            { field: 'ActionType', name: 'ActionType', headerCellClass: 'grid-header', enableCellEdit: false },
            { field: 'Active', name: 'Active', headerCellClass: 'grid-header', enableCellEdit: false },
            { field: 'MaxAttempts', name: 'Max Attempts', headerCellClass: 'grid-header', enableCellEdit: false },
            { field: 'X_API_ID', name: 'X API Key', headerCellClass: 'grid-header', enableCellEdit: false }

            ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.webhook.editRowSummary(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            webhook.serviceBranchGrid.gridApi = gridApi;
        }
    };
   
    if ($rootScope.activityright && webhook.CustomerName != '') {
        $http.get('Customers/GetWebhookUser/' + webhook.CustomerID).success(function (res) {
            $http.get('Customers/GetWebhooks/' + res).success(function (response) {
                webhook.serviceBranchGrid.data = response;
            });
        });
       
    }


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
        //$scope.gridApi.grouping.groupColumn('ExternalId');
        //$scope.gridApi.grouping.aggregateColumn('state', uiGridGroupingConstants.aggregation.COUNT);
    };
    
    $scope.addRowSummary = function () {
        var newService = {
            "CustomerID": "",
            "URL": "",
            "UserID": "",
            "Action": "",
            "MaxAttempts": "1",
            "Secret": "",
            "Active":"No",
            "X_API_ID": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        webhook.addRowSummary($scope.webhook.serviceBranchGrid, rowTmp);        
    };

    $scope.editWebhookDomainList = function () {
        var newService = {
            "CustomerID": "",
            "URL": "",
            "UserID": "",
            "Action": "",
            "MaxAttempts": "1",
            "Secret": "",
            "Active": "No",
            "X_API_ID": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        webhook.editWebhookDomainList($scope.webhook.serviceBranchGrid, rowTmp);
    };

}

psWebhooksMappingsControllerRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psWebhooksMappingsControllerRowEditor($http, $rootScope, $uibModal) {
    
    var service = {};
    service.editRowSummary = editRowSummary;
    service.addRowSummary = addRowSummary;
    service.editWebhookDomainList = editWebhookDomainList;

    function editRowSummary(grid, row) {
        if ($rootScope.activityright === 'User') {
            return;
        }
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psWebhooksMappings/webhook-edit.html',
            controller: 'psWebhooksMappingsRowEditCtrl',
            controllerAs: 'webhook',
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
            templateUrl: 'ext-modules/psMappings/psWebhooksMappings/webhook-add.html',
            controller: 'psWebhooksMappingsRowEditCtrl',
            controllerAs: 'webhook',
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

    function editWebhookDomainList(grid, row, size) {
        console.log('psWebhooksMappingController.js - editWebhookDomainList')
        $uibModal.open({
            //windowClass: 'webhookDomain',
            size: 'webhookDomainSize',
            templateUrl: 'ext-modules/psMappings/psWebhooksMappings/webhookDomainList.html',
            controller: 'psWebhookDomainsMappingsController',
            controllerAs: 'domainList',
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



angular.module('psWebhooksMappings').controller('psWebhooksMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', 'psWebhooksMappingsApiUri', '$routeParams',
    function psWebhooksMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, psWebhooksMappingsApiUri, $routeParams) {
        
        var webhook = this;
        webhook.entity = angular.copy(row.entity);
        webhook.entity.MaxAttempts = String(webhook.entity.MaxAttempts)
        webhook.save = save;
        var customerDetail = $routeParams.CustomerName.split(":");
        webhook.CustomerName = customerDetail[0];
        webhook.CustomerID = customerDetail[1];
        webhook.IsActive = webhook.entity.Active === "Yes";


        var getIndexIfObjWithOwnAttr = function (array, attr, value) {
            for (var i = 0; i < array.length; i++) {
                if (array[i].hasOwnProperty(attr) && array[i][attr] === value) {
                    return i;
                }
            }
            return -1;
        }
        function create_UUID() {
            var dt = new Date().getTime();
            var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = (dt + Math.random() * 16) % 16 | 0;
                dt = Math.floor(dt / 16);
                return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
            });
            return uuid;
        }
     
        //Save 
        function save(operation) {

            //row.entity = angular.extend(row.entity, webhook.entity
            var url = "";
       

            if (operation == 'add') {
                url = psWebhooksMappingsApiUri.AddWebhook;
                webhook.entity.UserID = create_UUID()
            }
            else
                url = psWebhooksMappingsApiUri.UpdateWebhook;

            // first get customers externalID
            $http.get('Customers/GetWebhookUser/' + webhook.CustomerID).success(function (res) {
                var externalId = res;
                // next get customer webhookDomains
                $http.get('Customers/GetWebhookDomains/' + webhook.CustomerID).success(function (data) {
                    var urlDomain = extractHostname(webhook.entity.URL);
                    var isValidDomain = false;
                    //data.forEach(obj => { if (obj.Domain.toLowerCase() == urlDomain.toLowerCase()) isValidDomain = true });
                    data.forEach(function (obj, i) { if (obj.Domain.toLowerCase() == urlDomain.toLowerCase()) isValidDomain = true });
                    if (!isValidDomain) {
                        growl.error('The URL domain - ' + urlDomain + " - is not a valid customer domain");
                        return;
                    }

                    $http.post(url, { "Webhook": webhook.entity, "User": externalId, "OriginalUserID": row.entity.UserID, "OriginalActionType": row.entity.ActionType })
                        .success(function (data) {

                            if (data) {
                                if (operation == 'add') {
                                    growl.success("Webhook record was added successfully");

                                    grid.data.push(webhook.entity)
                                }
                                else if (operation == 'edit') {
                                    growl.success("Webhook record was edited successfully");
                                    row.entity.UserID = webhook.entity.UserID;
                                    row.entity.Secret = webhook.entity.Secret;
                                    row.entity.ActionType = webhook.entity.ActionType;
                                    row.entity.MaxAttempts = webhook.entity.MaxAttempts;
                                    row.entity.URL = webhook.entity.URL;
                                    row.entity.Active = webhook.entity.Active;
                                    row.entity.X_API_ID = webhook.entity.X_API_ID;
                                }
                                $uibModalInstance.close(row.entity);
                                return;
                            }
                            else {
                                growl.success("Unable to perform " + operation);
                            }
                        }).error(function (response) {

                            if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                                growl.error("Webhook record could not be added");
                                return;
                            }
                            if (response.InnerException.InnerException.ExceptionMessage.indexOf("Violation of UNIQUE KEY constraint 'UK_TypeCode'") >= 0
                                || response.InnerException.InnerException.ExceptionMessage.indexOf("Cannot insert duplicate key row in object") >= 0) {
                                growl.error('Webhook record is a duplicate and cannot be added');
                                return;
                            }
                            growl.error("Unable to perform operation!");
                            return;
                        });
                });
            });

        }
        
        $http.get('Customers/GetWebhookUser/' + webhook.CustomerID).success(function (res) {
            $http.get('Customers/GetAvailbleActionType/' + res).success(function (response) {
                webhook.LoadActionTypeList = response;
                if (row.entity.ActionType) {
                    webhook.LoadActionTypeList.push(row.entity.ActionType)
                }
            });
        });
        webhook.setActiveField = setActiveField;
        function setActiveField() {
            
            if (webhook.IsActive) {
                webhook.entity.Active = "Yes";
            }
            else {
                webhook.entity.Active = "No";
            }

        }

        webhook.remove = remove;
        function remove() {
            
            $http.post(psWebhooksMappingsApiUri.RemoveWebhook, { "Webhook": webhook.entity, "User": webhook.CustomerName, "OriginalUserID": row.entity.UserID, "OriginalActionType": row.entity.ActionType })
                .success(function (data) {
                    
                    if (data) {
                        growl.success("Webhook record was deleted successfully");

                        grid.options.data.splice(grid.options.data.indexOf(row.entity), 1)
                    }
                    else {
                        growl.success("Unable to delete webhook");
                    }
                }).error(function (response) {
                    growl.error("Unable to delete webhook");
                    return;
                });
            $uibModalInstance.close(row.entity);
        }

        function extractHostname(url) {
            var hostname;
            //find & remove protocol (http, ftp, etc.) and get hostname

            if (url.indexOf("//") > -1) {
                hostname = url.split('/')[2];
            }
            else {
                hostname = url.split('/')[0];
            }

            //find & remove port number
            hostname = hostname.split(':')[0];
            //find & remove "?"
            hostname = hostname.split('?')[0];

            return hostname;
        }
    }]);



        function psWebhooksMappingsApiUri() {
            this.AddWebhook = 'Customers/AddWebhook';
            this.RemoveWebhook = 'Customers/DeleteWebhook'
            this.UpdateWebhook = 'Customers/UpdateWebhook';
        }



    
    