angular.module('psMenu').factory("appVersionDataService", appVersionDataService);

appVersionDataService.$inject = ["$http", "$q"];

function appVersionDataService($http, $q) {
    var service = {
        getVersionInfo: getVersionInfo
    }
    return service;

    function getVersionInfo() {
        return $http({ method: "GET", url: "UtilitiesController/GetAppversionInfo" })
            .then(function successcb(response, status, headers, config) {
                return response;
            })
            .catch(function errorcb(response, status, headers, config) {
                var val = $q.reject(response);
                return val;
            });
    }
}




angular.module('psMenu').component('appversioninfo', {
    templateUrl: "ext-modules/psMenu/psAppVersionInfo.component.html",
    controller: ["appVersionDataService", "$uibModal", versioninfoController],
    controllerAs: "vm",
});

versioninfoController.$inject = ["appVersionDataService", "$uibModal"];
function versioninfoController(appVersionDataService, $uibModal) {
    var vm = this;    

    vm.animationsEnabled = true;

    vm.open = function (size, parentSelector) {
        var parentElem = parentSelector ?
            angular.element($document[0].querySelector('.modal-demo ' + parentSelector)) : undefined;
        var modalInstance = $uibModal.open({
            animation: vm.animationsEnabled,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            controllerAs: 'vm',
            size: size,
            appendTo: parentElem
        });

        modalInstance.result.then(function (selectedItem) {
            vm.selected = selectedItem;
        }, function () {            
        });
    }

    vm.$onInit = function () {
        RetrieveData();       
    };

    var RetrieveData = function () {
        appVersionDataService.getVersionInfo().then(function (response) {
            vm.appVersionData = response.data;
            if (vm.appVersionData !== undefined && vm.appVersionData !== null && vm.appVersionData.Version !== "") {
                if (localStorage.TowerAppVersion != undefined) {
                    if (localStorage.TowerAppVersion !== vm.appVersionData.Version) {
                        vm.open();
                        localStorage.TowerAppVersion = vm.appVersionData.Version;
                    }
                }
                else {
                    //this if for the first time deployment after this change should be removed post that
                    vm.open();
                    localStorage.TowerAppVersion = vm.appVersionData.Version;
                }
            }
        })
    }

    vm.handletooltip = function () {

    }
}

angular.module('psMenu').controller('ModalInstanceCtrl', function ($uibModalInstance) {
    var $ctrl = this;

    $ctrl.ok = function () {
        $uibModalInstance.close('closed');
    };

    $ctrl.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});



