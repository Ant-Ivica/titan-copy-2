"use strict";

var app = angular.module('MessageLogModule', ['ngAnimate', 'ui.grid', 'ui.grid.grouping', 'ui.grid.moveColumns', 'ui.grid.selection', 'ui.bootstrap', 'ui.grid.edit', 'angular-confirm', 'angularjs-datetime-picker', 'angular-growl', 'ui.grid.expandable']);

app.service('modalProvider', ['$uibModal', function ($uibModal) {

    this.openPopupModal = function (Serviceid) {
        var modalInstance = $uibModal.open({
            templateUrl: 'ext-modules/psReporting/MessageLogDetails.html',
            controller: 'MessageLogModuleCtrl',
            controllerAs: 'vmTest',
            resolve: {
                Requestid: function () {
                    return Serviceid;
                }
            }
        });
    }
}]);


angular.module('MessageLogModule').controller('MessageLogModuleCtrl', ['$http', '$rootScope', '$scope', '$window', '$location', '$anchorScroll', '$uibModalInstance', 'Requestid', '$uibModal',
function MessageLogModuleCtrl($http, $rootScope, $scope, $window, $location, $anchorScroll, $uibModalInstance, Requestid, $uibModal) {

    var vmTest = this;
    vmTest.ServiceRequestId = Requestid;
    vmTest.loading = true;
    vmTest.Refresh = function () {
        vmTest.loading = true;
        $http.get('ReportingController/GetMessageDetails/' + Requestid).then(function (data) {
            vmTest.MessageDetails = data.data;
            vmTest.loading = false;
        });
    }

    $http.get('ReportingController/GetMessageDetails/' + Requestid).then(function (data) {
        vmTest.MessageDetails = data.data;
        vmTest.loading = false;
    });

    vmTest.searchdet = function (MessageLogs) {       
             return (MessageLogs.ParentMessageLogId == 0 || MessageLogs.ExceptionDescription != '');       
    }
         
   
    var isScrolled = false;
    //Start-Scroll To Top of the screen
    vmTest.scrollTo = function (eID) {
        var est = document.getElementById(eID);
        var docPos = f_scrollTop();
        est.scrollIntoView();
        window.scrollTo(0, docPos);

    };  
    function f_scrollTop() {
        return f_filterResults (
            window.pageYOffset ? window.pageYOffset : 0,
            document.documentElement ? document.documentElement.scrollTop : 0,
            document.body ? document.body.scrollTop : 0
        );
    }
    function f_filterResults(n_win, n_docel, n_body) {
        var n_result = n_win ? n_win : 0;
        if (n_docel && (!n_result || (n_result > n_docel)))
            n_result = n_docel;
        return n_body && (!n_result || (n_result > n_body)) ? n_body : n_result;
    }
    //End-Scroll To Top of the screen
    vmTest.setContent = function (Documentobjectid, HeaderValue) {

        $uibModal.open({
            templateUrl: 'ext-modules/psReporting/MessagLogMessageView.html',
            controller: 'MessageLogModuleMessagecntrl',
            controllerAs: 'vmTest',
            resolve: {
                Documentobjectid: function () {
                    return Documentobjectid;
                },
                HeaderValue: function () {
                    return HeaderValue;
                }
            }
        });

    };


    vmTest.setExceptionContent = function (Content) {       
        $uibModal.open({
            templateUrl: 'ext-modules/psReporting/MessageExceptionView.html',
            controller: 'MessageLogModuleExceptionViewcntrl',
            controllerAs: 'vmTest',
            resolve: {
                Content: function () {
                    return Content;
                }
            }
        });

    };


}]);


angular.module('MessageLogModule').controller('MessageLogModuleMessagecntrl', ['$http', '$uibModalInstance', 'Documentobjectid', 'HeaderValue',
function MessageLogModuleMessagecntrl($http, $uibModalInstance, Documentobjectid, HeaderValue) {

    var vmTest = this;
    vmTest.Content = '';
    vmTest.HeaderValue = HeaderValue;

    $http.get('ExceptionController/GetMessageContent/' + Documentobjectid)
      .then(function (response) {
          vmTest.Content = response.data;
      });

}]);


angular.module('MessageLogModule').controller('MessageLogModuleExceptionViewcntrl', ['$http', '$uibModalInstance', 'Content',
function MessageLogModuleExceptionViewcntrl($http, $uibModalInstance, Content) {
    var vmTest = this;
    vmTest.ExceptionDescription = Content;
   


}]);