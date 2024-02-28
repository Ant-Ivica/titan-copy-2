"use strict";

var app1 = angular.module('ModalImportModule', ['ngAnimate', 'ui.grid', 'ui.grid.grouping', 'ui.grid.moveColumns', 'ui.grid.selection', 'ui.bootstrap', 'ui.grid.edit', 'angular-confirm', 'angularjs-datetime-picker', 'angular-growl', 'ui.grid.expandable']);



app1.service('ModalImportProvider', ['$uibModal', function ($uibModal) {

    this.openPopupModal = function (grid) {
        var modalInstance = $uibModal.open({
            templateUrl: 'ext-modules/psMappings/psProviderMappings/import-provider-mappings.html',
            controller: 'ModalImportProviderCtrl',
            controllerAs: 'vmTestProviderControl',
            resolve: {
                grid: function () {
                    return grid;
                }
            }
        });
    }
}]);


angular.module('ModalImportModule')
 .directive("fileread", [function () {
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
                         
                         for (var i = 0; i < 8; i++)
                         {
                             $scope.Header = data[7][i];
                             //$scope.myString2 = $scope.myString1.replace(/ /g, "");
                             
                             $scope.SHeader = $scope.Header.replace(/ /g, "");
                             $scope.SHeader = $scope.SHeader.replace("?", "");
                             $scope.CustomHeader = $scope.SHeader.replace("*","");

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
                                 }

                                 if (data[i][columnIndex] == undefined)
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


angular.module('ModalImportModule').controller('ModalImportProviderCtrl', ModalImportProviderCtrl);

ModalImportProviderCtrl.$inject = ['$http', '$uibModalInstance', 'grid', 'growl', '$timeout'];
function ModalImportProviderCtrl($http, $uibModalInstance, grid, growl, $timeout) {

    var vmTestProviderControl = this;
    vmTestProviderControl.gridOptions = {};

    vmTestProviderControl.reset = reset;
    vmTestProviderControl.save = save;
    vmTestProviderControl.upload = upload;
 
    function reset() {
        vmTestProviderControl.gridOptions.data = [];
        vmTestProviderControl.gridOptions.columnDefs = [];
        vmTestProviderControl.gridOptions.Message = '';
        vmTestProviderControl.loading = false;
    }
    
    //upload();
    function save() {
        $http.post('Providers/AddImportData', vmTestProviderControl.gridOptions.data)
         .success(function (data) {

             grid.data = data;                    
             if (data.length == 0) {                 
                 $scope.opts.Message = 'Upload failed.Please validate the data';
                 return;
             }
             else {
                 growl.success('Records uploaded successfully');
             }
             $uibModalInstance.close();
         }).finally(function () { vmTestProviderControl.loading = false; })
        ;
     
    }

    // Reset the data source in a timeout so we can see the loading message
    function upload() {
        vmTestProviderControl.loading = true;
       
        $timeout(function () {
            save();
        }, 1000);
    }
};