describe('Test Execution to Update User Role to Admin', function () {

    'use strict';
  
  
    var pages = {};
    var testData = require('../resources/testData.json');
    
var DBpage = require('../utils/DBUtilsNew.js');
    var DBUtil = new DBpage();
    it('Modify Role to Admin', async () => {
  
      console.log("User"+testData.User.userName) ;
      console.log("User"+testData.User.Role) ;
       var roleid = 1;
       if(testData.User.Role == "SuperAdmin")
         roleid = 2;
        if(testData.User.Role == "Admin")
          roleid = 3;
       if(testData.User.Role == "User")
            roleid = 1;

      await DBUtil.ConnectDBAsync("update [Tower.userRoles] set  roleid ="+roleid+" where UserId in(select id from [Tower.users] where UserName like '%"+testData.User.userName+"%')")
         
      if(roleId.includes('1'))
      {
          testData.User.Role = "SuperAdmin";
          console.log("User Role is SuperAdmin");
      }
      else if(roleId.includes('2'))
      {
          testData.User.Role = "Admin";
          console.log("User Role is Admin");

      }
      else if(roleId.includes('3'))
      {
          testData.User.Role = "User";
          console.log("User Role is User");
      }
        
    }, 5000)
});  