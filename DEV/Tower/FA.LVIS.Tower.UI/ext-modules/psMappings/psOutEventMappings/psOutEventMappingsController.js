"use strict";


angular.module('psOutEventMappings').controller('psOutEventMappingsController', psOutEventMappingsController);
angular.module('psOutEventMappings').controller('psOutEventMappingsRowEditCtrl', psOutEventMappingsRowEditCtrl);
angular.module('psOutEventMappings').service('psOutEventMappingsRowEditor', psOutEventMappingsRowEditor);

psOutEventMappingsController.$inject = ['$scope', '$http', '$interval', '$uibModal', 'psOutEventMappingsRowEditor', 'uiGridGroupingConstants', '$location', '$routeParams'];
function psOutEventMappingsController($scope, $http, $interval, $uibModal, psOutEventMappingsRowEditor, uiGridGroupingConstants, $location, $routeParams) {

    var vemap = this;

    vemap.LenderName = ($routeParams.LenderName || "");

    vemap.editevent = psOutEventMappingsRowEditor.editevent;
    vemap.addNewEvent = psOutEventMappingsRowEditor.addNewEvent;
    vemap.removeeventRow = psOutEventMappingsRowEditor.removeRow;
   
    var detailButton = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents"> <i ng-show="(row.treeNode.children && row.treeNode.children.length == 0)" class="fa fa-times-circle" style="color:red;padding:5px 25px;text-align:center;cursor:pointer" ng-click="grid.appScope.deleteRow(row)"></i></div>' 

    vemap.serviceEventGrid = {
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
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterCsvFilename: 'Events.csv',
        columnDefs: [
           { field: 'ExternalEvent', name: 'External Event', headerCellClass: 'grid-header', enableCellEdit: false },
           { field: 'Name', name: 'Name', headerCellClass: 'grid-header',  enableCellEdit: false, sort: { priority: 0, direction: 'asc' } },
           { field: 'Description', name: 'Description', headerCellClass: 'grid-header', enableCellEdit: false },           
           { field: 'Services', name: 'Service(s)', headerCellClass: 'grid-header', enableCellEdit: false },
           { field: 'remove', name: '', headerCellClass: 'grid-header', enableColumnMenu: false, enableFiltering: false, width: '6%', cellTemplate: detailButton }   
        ],
      
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vemap.editevent(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",
        onRegisterApi: function (gridApi) {
            vemap.serviceEventGrid.gridApi = gridApi;
        }
    };

    $http.get('OutboundEvents/GetEvents/' +vemap.LenderName.split(":")[1]).success(function (response) {
        vemap.serviceEventGrid.data = response;
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
        $scope.gridApi.grouping.groupColumn('EventId');
        //$scope.gridApi.grouping.aggregateColumn('state', uiGridGroupingConstants.aggregation.COUNT);
    };

    vemap.serviceEventGrid.getAggregates = function () {
        var aggregatesTree = [];
        var gender

        var recursiveExtract = function (treeChildren) {
            alert(treeChildren);
            return treeChildren.map(function (node) {
                var newNode = {};
                angular.forEach(node.row.entity, function (attributeCol) {
                    if (typeof (attributeCol.groupVal) !== 'undefined') {
                        newNode.groupVal = attributeCol.groupVal;
                        newNode.aggVal = attributeCol.value;
                    }
                });
                newNode.otherAggregations = node.aggregations.map(function (aggregation) {
                    return { colName: aggregation.col.name, value: aggregation.value, type: aggregation.type };
                });
                if (node.children) {
                    newNode.children = recursiveExtract(node.children);
                }
                return newNode;
            });
        }
        aggregatesTree = recursiveExtract($scope.gridApi.grid.treeBase.tree);
        //console.log(aggregatesTree);
    };

    $scope.addNewEvent = function () {
        var newService = {
            "EID": "0",
            "ExternalEvent":"",
            "Name": "",
            "Description": "",
            "Services": ""
          
        };
        var rowTmp = {};
        rowTmp.entity = newService;
        vemap.addNewEvent($scope.vemap.serviceEventGrid, rowTmp);
    };
    $scope.removeRowIndex = function (row) {
        var index = this.row.rowIndex;
        $scope.vemap.serviceEventGrid.selectItem(index, false);
        $scope.vemap.serviceEventGrid.splice(index, 1);
    };

    $scope.deleteRow = function (row) {
        var index = $scope.vemap.serviceEventGrid.data.indexOf(row.entity);
        $scope.vemap.serviceEventGrid.data.splice(index, 1);
    };

   
}
psOutEventMappingsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psOutEventMappingsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editevent = editevent;
    service.addNewEvent = addNewEvent;


    function editevent(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psOutEventMappings/service-out-event-mappings-edit.html',
            controller: ['$http', '$uibModalInstance', 'grid', 'row', psOutEventMappingsRowEditCtrl],
            controllerAs: 'vemap',
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

    function addNewEvent(grid, row, size) {
        $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psOutEventMappings/service-out-event-mappings-add.html',
            controller: ['$http', '$uibModalInstance', 'grid', 'row', '$window', psOutEventMappingsRowEditCtrl],
            controllerAs: 'vemap',
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



//angular.module('psSecurity').controller('psMappingsLenderRowEditCtrl', ['$http', '$modalInstance', 'grid', 'row', 
function psOutEventMappingsRowEditCtrl($http, $uibModalInstance, grid, row, $window) {
    //alert(1);
    var vemap = this;
    vemap.entity = angular.copy(row.entity);
    vemap.save = save;
    function save() {
     
        if (vemap.entity.EID == '0') {

        /*
         * $http.post('http://localhost:8080/service/save', row.entity).success(function(response) { $modalInstance.close(row.entity); }).error(function(response) { alert('Cannot edit row (error in console)'); console.dir(response); });
         */
        row.entity = angular.extend(row.entity, vemap.entity);
        //real ID come back from response after the save in DB
        row.entity.id = Math.floor(100 + Math.random() * 1000);

        grid.data.push(row.entity);

        } else {
            
            vemap.entity = angular.extend(row.entity, vemap.entity);
            /*
             * $http.post('http://localhost:8080/service/save', row.entity).success(function(response) { $modalInstance.close(row.entity); }).error(function(response) { alert('Cannot edit row (error in console)'); console.dir(response); });
             */
        }
        $uibModalInstance.close(row.entity);
    }

    vemap.remove = remove;
    function remove() {
        console.dir(row)
        if (row.entity.EID != '0') {

            row.entity = angular.extend(row.entity, vemap.entity);
            var index = grid.appScope.vemap.serviceEventGrid.data.indexOf(row.entity);
            grid.appScope.vemap.serviceEventGrid.data.splice(index, 1);
            /*
             * $http.delete('http://localhost:8080/service/delete/'+row.entity.id).success(function(response) { $modalInstance.close(row.entity); }).error(function(response) { alert('Cannot delete row (error in console)'); console.dir(response); });
             */

        }
        $uibModalInstance.close(row.entity);
    }
}




