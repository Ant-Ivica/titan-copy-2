describe('Test Execution to Update User Role to Super Admin', function () {

    'use strict';


    var pages = {};
    var testData = require('../resources/testData.json');

    var DBpage = require('../utils/DBUtilsNew.js');
    var DBUtil = new DBpage();
    it('Modify Role to SuperAdmin', async () => {

        console.log("User" + testData.User.userName);
        console.log("User" + testData.User.Role);
        var roleid = 1;
        await DBUtil.ConnectDBAsync("update [Tower.userRoles] set  roleid =" + roleid + " where UserId in(select id from [Tower.users] where UserName like '%" + testData.User.userName + "%')")


        testData.User.Role = "SuperAdmin";
        console.log("User Role is SuperAdmin");


    }, 50000)
});  