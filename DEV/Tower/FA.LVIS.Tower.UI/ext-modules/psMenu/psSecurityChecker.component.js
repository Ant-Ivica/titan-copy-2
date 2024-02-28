
angular.module('psMenu').component('appsecuritychecker', {
    templateUrl: "ext-modules/psMenu/psSecurityChecker.component.html",
    controller: ["$scope", "$rootScope", "UserInfo", "Idle", "$uibModal","$window", appsecuritycheckerController],
    controllerAs: "vm",
}).config(function (IdleProvider, KeepaliveProvider) {    
    KeepaliveProvider.interval(60); // in seconds
})
    .run(function (Idle) {       
        Idle.watch();
    });

appsecuritycheckerController.$inject = ["$scope", "$rootScope", "UserInfo", "Idle", "$uibModal", "$window"];
function appsecuritycheckerController($scope, $rootScope, UserInfo, Idle, $uibModal, $window) {
    var vm = this;    

    vm.animationsEnabled = true;

    vm.open = function (size, parentSelector) {
        var parentElem = parentSelector ?
            angular.element($document[0].querySelector('.modal-demo ' + parentSelector)) : undefined;
        var modalInstance = $uibModal.open({
            animation: vm.animationsEnabled,
            ariaLabelledBy: 'modal-title',
            ariaDescribedBy: 'modal-body',
            templateUrl: 'mySecurityModalContent.html',
            controller: 'SecurityModalInstanceCtrl',
            controllerAs: 'vm',
            size: size,
            appendTo: parentElem,
            backdrop: 'static',
            keyboard: false
        });

        modalInstance.result.then(function (selectedItem) {
            vm.selected = selectedItem;
        }, function () {            
        });
    }

    vm.$onInit = function () {
        //RetrieveData();            
    };

    $scope.idle = 300;
    $scope.timeout = 1500;

    $scope.$on('IdleStart', function () {
        //console.log('idlestart');
        UserInfo.getUser().then(function (response) {
            //console.log(response);
            $rootScope.$broadcast('getUser', response);
        }, function (error) {
        });
    });

    $scope.$on('IdleEnd', function () {
        //console.log('idleend');
        UserInfo.getUser().then(function (response) {
            //console.log(response);
            $rootScope.$broadcast('getUser', response);
        }, function (error) {
        });
    });

    $scope.$on('IdleWarn', function (e, countdown) {
        //console.log('idlewarn');
    });

    $scope.$on('IdleTimeout', function () {
        //console.log('idletimeout');
        //vm.open();          
        $window.location.href = "/tower/SessionExpired.html";
    });

    $scope.$on('Keepalive', function () {
        //console.log('keepalive');
    });

    $scope.$watch('idle', function (value) {
        if (value !== null) Idle.setIdle(value);
    });

    $scope.$watch('timeout', function (value) {
        if (value !== null) Idle.setTimeout(value);
    });

    var RetrieveData = function () {
       
    }
        
}

angular.module('psMenu').controller('SecurityModalInstanceCtrl', function ($uibModalInstance, $window) {
    var $ctrl = this;

    $ctrl.ok = function () {
        $uibModalInstance.close('closed');
        $window.location.href = "/tower";
    };   
});



