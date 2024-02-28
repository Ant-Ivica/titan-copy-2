"use strict";

angular.module('psSubMenu').directive('psSubMenu', ['$timeout', function ($timeout) {
    return {
        scope: {
            label: '@',
        },
        transclude: true,
        templateUrl: 'ext-modules/psMenu/psSubMenuTemplate.html',
        controller: 'psSubMenuController',
        link: function (scope, el, attr) {
            var item = el.find('.ps-selectable-item:first');
            $timeout(function () {
                item.trigger('click');
            });
        }
    };
}]);


