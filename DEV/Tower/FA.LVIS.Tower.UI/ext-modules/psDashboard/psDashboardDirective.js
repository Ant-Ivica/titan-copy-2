/// <reference path="C:\Users\avidyarthi\Desktop\Tower\Tower\FA.LVIS.Tower.UI\Scripts/charts/angular-chart.js" />
/// <reference path="C:\Users\avidyarthi\Desktop\Tower\Tower\FA.LVIS.Tower.UI\Scripts/charts/chart.min.js" />
/// <reference path="C:\Users\avidyarthi\Desktop\Tower\Tower\FA.LVIS.Tower.UI\Scripts/angular.js" />
"use strict";

angular.module('psDashboard', ["chart.js"]).directive('psDashboard', function () {
    return {
     //   controller: 'psDashboardController',
        templateUrl: 'ext-modules/psDashboard/psDashboardTemplate.html',
        link: function (scope, element, attrs) {
            
        }
    };
});
