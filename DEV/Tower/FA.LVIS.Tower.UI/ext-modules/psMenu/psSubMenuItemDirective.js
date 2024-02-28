/// <reference path="C:\Development\AT_Tower\DEV\Iteration0\Tower\FA.LVIS.Tower.UI\Scripts/angular.js" />
/// <reference path="C:\Development\AT_Tower\DEV\Iteration0\Tower\FA.LVIS.Tower.UI\Scripts/jquery-3.1.1.js" />
/// <reference path="psSubMenuController.js" />
"use strict";

angular.module('psSubMenu').directive('psSubMenuItem', function () {
    return {
        require: '^psSubMenu',
        scope: {
            label: '@',
            route: '@',
            activetab: '@'
        },
        templateUrl: 'ext-modules/psMenu/psSubMenuItemTemplate.html',        
        link: function (scope, el, attr, ctrl) {
            
            scope.isSubMenuActive = function () {                
                return scope.label === scope.activetab;
            };

            scope.isVertical = function () {
                return el.parents('.ps-subitem-section').length > 0;
            }

            el.on('click', function (evt) {
                evt.stopPropagation();
                evt.preventDefault();
                scope.$apply(function () {
                    ctrl.setActiveElement(el);
                    ctrl.setRoute(scope.route);
                });
            });
        }
    };
});

