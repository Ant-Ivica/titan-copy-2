/// <reference path="D:\Projects\LVIS\Tower\Tower - Copy\FA.LVIS.Tower.UI\Scripts/angular.js" />
/// <reference path="../app.js" />

app.controller('lendersController', function ($http) {

    var lendersCtrl = this;

    /*
    lendersCtrl.lenders = [
        { "LvisABEID": "10001", "LenderName" : "Bank of America" },
        { "LvisABEID": "10002", "LenderName": "Wells Fargo" },
        { "LvisABEID": "10003", "LenderName": "Chevron" },
        { "LvisABEID": "10004", "LenderName": "BMO Harris" }
    ];
    */

    $http.get('Api/Lenders/')
        .then(function (response) {
            lendersCtrl.lenders = response.data;
        });
});
