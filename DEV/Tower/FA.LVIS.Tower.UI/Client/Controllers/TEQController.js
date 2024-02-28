/// <reference path="E:\Tower\FA.LVIS.Tower.UI\ext-modules/psExceptions/TestModal.html" />
/// <reference path="E:\Tower\FA.LVIS.Tower.UI\ext-modules/psExceptions/TestModal.html" />
/// <reference path="E:\Tower\FA.LVIS.Tower.UI\Scripts/angular-ui/ui-bootstrap-tpls.min.js" />
/// <reference path="E:\Tower\FA.LVIS.Tower.UI\Scripts/angular.min.js" />


"use strict";

angular.module('app').controller('TEQController', ['$http', '$uibModal', function ($http, $uibModal) {

    var TEQCntrl = this;
    TEQCntrl.DetailException = "";
    $http.get('Dashboard/TechnicalException')
        .success(function (data) {
            TEQCntrl.LVISExceptionList = data;
        });
   
    TEQCntrl.GO = function ($event) {
        TEQCntrl.Message = $event.target.id;

        $http.get('Dashboard/TechnicalException/' + TEQCntrl.Message)
       .success(function (data) {
           TEQCntrl.DetailException = data;
       });
    };

    TEQCntrl.animationsEnabled = true;
    TEQCntrl.Rowid = 0;
    TEQCntrl.ClickRow = function (DetailException) {
        alert(DetailException.Exceptionid);
    }

    TEQCntrl.open = function (DetailException) {

        var modalInstance = $uibModal.open({
            animation: TEQCntrl.animationsEnabled,
            templateUrl: 'myModalContent.html',
            controller: 'TEQInstanceController',
            size: 'lg',
            resolve: {
                items: function () {
                    return DetailException;
                }
            }
        });

        modalInstance.result.then(function (selectedItem) {
            TEQCntrl.selected = selectedItem;
        }, function () {
           
        });
    };

    TEQCntrl.toggleAnimation = function () {
        TEQCntrl.animationsEnabled = !TEQCntrl.animationsEnabled;
    };
}]);

// Please note that $uibModalInstance represents a modal window (instance) dependency.
// It is not the same as the $uibModal service used above.

angular.module('app').controller('TEQInstanceController', function ($uibModalInstance, items,$scope) {
    $scope.items = items;

    $scope.ok = function () {
        $uibModalInstance.close('test');
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
   
});
