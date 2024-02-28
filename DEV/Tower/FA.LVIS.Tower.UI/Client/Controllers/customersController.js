/// <reference path="D:\Projects\LVIS\Tower\Tower\FA.LVIS.Tower.UI\Scripts/angular.js" />
/// <reference path="../app.js" />

app.controller('customersController', function ($http) {
    var custCtrl = this;

    $http.get('Api/Customers/')
        .then(function (response) {
            custCtrl.customers = response.data;
        });
});
