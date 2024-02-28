"use strict";

angular.module('psWebhooksMappings').controller('psWebhookDomainsMappingsController', psWebhookDomainsMappingsController);
angular.module('psWebhooksMappings').service('psWebhookDomainsMappingsApiUri', psWebhookDomainsMappingsApiUri);

psWebhookDomainsMappingsController.$inject = ['$scope', '$http', '$interval', '$uibModal', 'uiGridGroupingConstants', '$route', '$routeParams', 'growl', 'UserInfo', '$rootScope', '$confirm', '$cookies', '$window'];
function psWebhookDomainsMappingsController($scope, $http, $interval, $uibModal, uiGridGroupingConstants, $route, $routeParams, growl, UserInfo, $rootScope, $confirm, $cookies, $window) {

    var domainList = this;
    console.log('psWebhooksDomainMappingsController.js - editWebhookDomainList');

    var customerDetail = $routeParams.CustomerName.split(":");
    domainList.CustomerName = customerDetail[0];
    domainList.CustomerID = customerDetail[1];
    domainList.serviceBranchGrid = {
        enableColumnResize: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: true,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        enableRowHeaderSelection: false,
        multiSelect: false,
        enableSorting: true,
        //enableFiltering: true,
        enableGridMenu: true,
        enableSelectAll: true,
        paginationPageSizes: [5],   //paginationPageSizes: [5, 10, 15],
        paginationPageSize: 5,
        minRowsToShow: 5,
        rowHeight: 40,
        groupingShowAggregationMenu: 0,
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterCsvFilename: 'domainList.csv',
        columnDefs: [
            { field: 'WebhookDomainId', name: 'WebhookDomainId', headerCellClass: 'grid-header', enableCellEdit: false, visible: false },
            { field: 'Domain', name: 'Domain', displayName: 'Domain', headerCellClass: 'grid-header', enableCellEdit: false }//,
            //{ name: ' ', cellTemplate: "<div\"><button ng-click=\"grid.appScope.domainList.remove(row.entity)\" class=\"btn btn-danger\">Delete</button></div>" }
        ],

        rowTemplate: "<div ng-dblclick=\"grid.appScope.domainList.editRowDomain(grid, row)\", ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            domainList.serviceBranchGrid.gridApi = gridApi;
        }
    }

    // PDL      if ($rootScope.activityright && domainList.CustomerName != '') {
    if (domainList.CustomerName != '') {
        $http.get('Customers/GetWebhookDomains/' + domainList.CustomerID).success(function (response) {
            domainList.serviceBranchGrid.data = response;
        });
    }

    $scope.addRowDomain = function () {
        var newService = {
            "Domain": ""
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        addRowDomain($scope.domainList.serviceBranchGrid, rowTmp);
    };


    //domainList.addRowDomain = addRowDomain;
    function addRowDomain(grid, row, size) {
        $uibModal.open({
            size: 'webhookDomainSize',
            templateUrl: 'ext-modules/psMappings/psWebhooksMappings/webhookDomain-add.html',
            controller: 'psWebhookDomainsMappingsRowEditCtrl',
            controllerAs: 'webhookDomain',
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

    domainList.editRowDomain = function (grid, row, size) {
        $uibModal.open({
            size: 'webhookDomainSize',
            templateUrl: 'ext-modules/psMappings/psWebhooksMappings/webhookDomain-edit.html',
            controller: 'psWebhookDomainsMappingsRowEditCtrl',
            controllerAs: 'webhookDomain',
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

};


angular.module('psWebhooksMappings').controller('psWebhookDomainsMappingsRowEditCtrl', ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', 'growl', '$confirm', 'psWebhookDomainsMappingsApiUri', '$routeParams',
    function psWebhookDomainsMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window, $scope, growl, $confirm, psWebhookDomainsMappingsApiUri, $routeParams) {
        var webhookDomain = this;
        webhookDomain.entity = angular.copy(row.entity);
        webhookDomain.save = save;
        webhookDomain.remove = remove;
        var customerDetail = $routeParams.CustomerName.split(":");
        webhookDomain.CustomerName = customerDetail[0];
        webhookDomain.CustomerID = customerDetail[1];


        //Save
        function save(operation) {
            var url = "";

            if (operation == 'add')
                url = psWebhookDomainsMappingsApiUri.AddWebhookDomain;
            else
                url = psWebhookDomainsMappingsApiUri.UpdateWebhookDomain;

            //$http.post(url, { "WebhookDomain": webhookDomain.entity.Domain })
            //$http.post(url, { "WebhookDomain": { "CustomerId": 10300, "Domain": "xxx.yyy.zzz", "CreatedDate": "", "CreatedBy": 0, "LastUpdatedDate": "", "LastUpdatedBy": 0 } })
            $http.post(url, { "WebhookDomainId": webhookDomain.entity.WebhookDomainId, "CustomerId": webhookDomain.CustomerID, "Domain": webhookDomain.entity.Domain, "CreatedDate": "", "CreatedBy": 0, "LastUpdatedDate": "", "LastUpdatedBy": 0 })
                .success(function (data) {
                    if (data) {
                        if (operation == 'add') {
                            growl.success("Webhook domain record was added successfully");
                            webhookDomain.entity.WebhookDomainId = data.WebhookDomainId;
                            grid.data.push(webhookDomain.entity)
                        }
                        else if (operation == 'edit') {
                            growl.success("Webhook domain record was edited successfully");
                            row.entity.Domain = webhookDomain.entity.Domain;
                        }
                        $uibModalInstance.close(row.entity);
                        return;
                    }
                    else {
                        growl.success("Unable to perform " + operation);
                    }
                }).error(function (response) {
                    if (response.ExceptionMessage.indexOf("Validation failed for one or more entities") >= 0) {
                        growl.error("Webhook domain record could not be added");
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


            $http.get('Customers/GetWebhookDomains/' + webhookDomain.CustomerID).success(function (response) {
                webhookDomain.serviceBranchGrid.data = response;
            });
        }

        function remove() {
            $http.get('Customers/GetWebhooks/' + webhookDomain.CustomerName)
                .success(function (response) {
                    var urlDomain = webhookDomain.entity.Domain;
                    var isDomainFound = false;

                    //response.forEach(obj => { if (extractHostname(obj.URL).toLowerCase() == urlDomain.toLowerCase()) isDomainFound = true });
                    response.forEach(function (obj, i) { if (extractHostname(obj.URL).toLowerCase() == urlDomain.toLowerCase()) isDomainFound = true });
                   if (isDomainFound) {
                        growl.error('The URL domain - ' + urlDomain + " - is currently being used");
                        return;
                    }

                    $http.post(psWebhookDomainsMappingsApiUri.RemoveWebhookDomain, webhookDomain.entity)  //, "User": domainList.CustomerName, "OriginalUserID": row.entity.UserID, "OriginalActionType": row.entity.ActionType })
                        .success(function (data) {
                            if (data) {
                                growl.success("Webhook domain record was deleted successfully");
                                grid.options.data.splice(grid.options.data.indexOf(row.entity), 1)
                                $uibModalInstance.close(row.entity);
                            }
                            else {
                                growl.success("Unable to delete webhook domain (1)");
                            }
                        })  // end success
                        .error(function (response) {
                            growl.error("Unable to delete webhook domain (2)");
                            return;
                        });  // end error
                })  // end success
                .error(function (response) {
                    growl.error("Unable to delete webhook domain (3)");
                    return;
                }); // end error
        } // end remove

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



function psWebhookDomainsMappingsApiUri() {
    this.AddWebhookDomain = 'Customers/AddWebhookDomain';
    this.RemoveWebhookDomain = 'Customers/DeleteWebhookDomain';
    this.UpdateWebhookDomain = 'Customers/UpdateWebhookDomain';
}
