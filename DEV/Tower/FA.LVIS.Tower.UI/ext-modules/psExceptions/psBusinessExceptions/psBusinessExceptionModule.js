"use strict";

angular.module('psBusinessException', ['ngAnimate', 'ui.grid', 'ui.grid.grouping', 'ui.grid.moveColumns', 'ui.grid.selection', 'ui.bootstrap', 'ui.grid.edit', 'angular-confirm', 'angularjs-datetime-picker', 'angular-growl', 'ui.grid.expandable', 'MessageLogModule', 'ui.grid.cellNav', 'ui.grid.treeView', 'ngIdle']);

var appBusExe = angular.module('psBusinessException');

appBusExe.config(['KeepaliveProvider', 'IdleProvider', function (KeepaliveProvider, IdleProvider) {
    IdleProvider.idle(300);
    IdleProvider.timeout(10);
    KeepaliveProvider.interval(10);
}]);

