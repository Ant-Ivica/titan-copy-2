angular.module('psMenu').factory("fastDataService", fastDataService);

fastDataService.$inject = ["$http", "$q"];

function fastDataService($http, $q) {
    var service = {
        getFastEnvironmentInfo: getFastEnvironmentInfo
    }
    return service;

    function getFastEnvironmentInfo() {
        return $http({ method: "GET", url: "UtilitiesController/GetFastEnvironmentInfo" })
            .then(function successcb(response, status, headers, config) {
                return response;
            })
            .catch(function errorcb(response, status, headers, config) {
                var val = $q.reject(response);
                return val;
            });
    }
}




angular.module('psMenu').component('fastinfo', {
    templateUrl: "ext-modules/psMenu/psFastEnvInfo.component.html",
    controller: ["fastDataService", fastinfoController],
    controllerAs: "vm",
});

fastinfoController.$inject = ["fastDataService"];
function fastinfoController(fastDataService) {
    var vm = this;
    vm.$onInit = function () {
        RetrieveData();        
    };
    vm.tooltipOpenIndicator = false;

    var RetrieveData = function () {
        fastDataService.getFastEnvironmentInfo().then(function (response) {
            vm.fastEnvironmentData = response.data;            
        })
    }

    vm.handletooltip = function () {
        vm.tooltipOpenIndicator = !vm.tooltipOpenIndicator;
        if (vm.tooltipOpenIndicator) {
            RetrieveData();        
        }
    }
}


