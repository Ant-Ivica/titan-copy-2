describe('Test Execution to Update User Role to Admin', function () {

    'use strict';
  
  
    var pages = {};
    var testData = require('../resources/testData.json');
    
var DBpage = require('../utils/DBUtilsNew.js');
    var DBUtil = new DBpage();
    it('Modify Role to Admin', async () => {
  
      console.log("User"+testData.User.userName) ;
      console.log("User"+testData.User.Role) ;
         var roleid = 2;

         console.log("update [Tower.userRoles] set  roleid ="+roleid+" where UserId in(select id from [Tower.users] where UserName like '%"+testData.User.userName+"%')");
      await DBUtil.ConnectDBAsync("update [Tower.userRoles] set  roleid ="+roleid+" where UserId in(select id from [Tower.users] where UserName like '%"+testData.User.userName+"%')");
            
          testData.User.Role = "Admin";
          console.log("User Role is Admin");

        
    }, 500000)
});  