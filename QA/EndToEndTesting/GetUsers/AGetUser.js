// describe('Test Execution on Access Request', function () {

//     'use strict';
//     var pages = {};
//     pages.Home = require('../pages/Home.js');
//     pages.AccessRequest = require('../pages/AccessRequest.js');

//     var testDataSrc = require('../resources/testData.json');
//     var homePage = new pages.Home();
//     var AccessRequest = new pages.AccessRequest();

//     it('GetUserdetails', function () {    
//         browser.get(testDataSrc.search.homeUrl).then(function()
//         {
//             homePage.getUserNameAndRole();   

//         })
//     });

// });


// //import { ConnectionPool } from 'mssql/msnodesqlv8';
var testDataSrc = require('../resources/testData.json');
var pages = {};
pages.Home = require('../pages/Home.js');
var testDataSrc = require('../resources/testData.json');
var homePage = new pages.Home();
var DBpage = require('../utils/DBUtilsNew.js');
var DBUtil = new DBpage();
describe('Get User Name and Role', function () {

  it('should prepare db', async () => {
    await browser.get(testDataSrc.search.homeUrl);

    homePage.isPageLoaded();
    browser.sleep(10000);
    await homePage.getUserNameAndRole();
    // console.log(await DBUtil.ConnectDB("select * from application"));

  }, 500000)

})
