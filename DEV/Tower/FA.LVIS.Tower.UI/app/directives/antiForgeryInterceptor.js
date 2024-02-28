
(function() {
    'use strict';

    function antiForgeryInterceptor() {
        return {
            request: function($config) {
                var antiForgeryTokenField = document.getElementById('__antiForgeryToken');
                $config.headers['XSRF-TOKEN'] = "12123test";
                if (antiForgeryTokenField) {
                    var xsrfToken = antiForgeryTokenField.value;
                    $config.headers['XSRF-TOKEN'] = xsrfToken;
                }

                return $config;
            }
        };
    }

    angular.module('app').service('antiForgeryInterceptor', antiForgeryInterceptor);
})();