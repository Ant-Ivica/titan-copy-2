"use strict";

angular.module('psException', ['ngAnimate', 'ui.grid', 'ui.grid.grouping', 'ui.grid.moveColumns', 'ui.grid.selection', 'ui.bootstrap', 'ui.grid.edit', 'angular-confirm', 'angularjs-datetime-picker', 'angular-growl', 'ui.grid.expandable', 'ui.grid.cellNav', 'MessageLogModule', 'ngIdle']);

var appExe = angular.module('psException');

appExe.config(['KeepaliveProvider', 'IdleProvider', function (KeepaliveProvider, IdleProvider) {
    IdleProvider.idle(300);
    IdleProvider.timeout(10);
    KeepaliveProvider.interval(10);
}]);



