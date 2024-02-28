"use strict";

angular.module('app').config(['$httpProvider', '$compileProvider', function ($httpProvider, $compileProvider) {
    //$httpProvider.decorator("$exceptionHandler", ["$delegate", function ($delegate) {
    //    return function (exception, cause) {
    //        $delegate(exception, cause);
    //        alert(exception.message);
    //    };
    //}]);



    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }

    //disable IE ajax request caching
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
    // extra
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

  //  $compileProvider.debugInfoEnabled(false);
}]);







