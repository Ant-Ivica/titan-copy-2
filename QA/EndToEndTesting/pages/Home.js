module.exports = function () {
    'use strict';
    // HOme Page
    // var pages = {};
    var testData = require('../resources/testData.json');
    var objRepo = require('../resources/HomeObjRepo.json');
    var teqObjRepo = require('../resources/TechnicalExceptionQueuesObjRepo.json');
    var screenshots = require('protractor-take-screenshots-on-demand');
    var constantsFile = require('../resources/constantsTower.json');
    //pages.Home = require('../pages/Home.js');    
    var DBpage = require('../utils/DBUtils.js');
    var DBUtil = new DBpage();

    var DBpage = require('../utils/DBUtilsNew.js');
    var DBUtilNew = new DBpage();

    var utilspage = require('../utils/objectLocator.js');
    var waitActions = require('../commons/waitActions.js');
    var buttonActions = require('../commons/buttonActions.js');
    var verifyAction = require('../commons/verifyActions.js');
    var inputBoxActions = require('../commons/inputBoxActions.js');
    var mouseActions = require('../commons/mouseActions.js');
    var dropDownActions = require('../commons/dropDownActions.js');

    //var homePage = new pages.Home();
    var objLocator = new utilspage();
    var waitActions = new waitActions();
    var buttonActions = new buttonActions();
    var verifyAction = new verifyAction();
    var inputBoxActions = new inputBoxActions();
    var mouseActions = new mouseActions();
    var Dropdownactions = new dropDownActions();
    var faiLogo = objLocator.findLocator(objRepo.homeObjRepo.faiLogo);
    var beqSection = objLocator.findLocator(objRepo.homeObjRepo.beqSection);
    var teqSection = objLocator.findLocator(objRepo.homeObjRepo.teqSection);
    var reportingMenu = objLocator.findLocator(objRepo.homeObjRepo.reportingMenu);
    var bException = objLocator.findLocator(objRepo.homeObjRepo.bExceptions);
    var tException = objLocator.findLocator(objRepo.homeObjRepo.tExceptions);
    var mappingTablesMenu = objLocator.findLocator(objRepo.homeObjRepo.mappingTablesMenu);
    var statusMenu = objLocator.findLocator(objRepo.homeObjRepo.statusMenu);
    var helpMenu = objLocator.findLocator(objRepo.homeObjRepo.helpMenu);
    var accessRequestMenu = objLocator.findLocator(objRepo.homeObjRepo.accessRequestMenu);
    var securityMenu = objLocator.findLocator(objRepo.homeObjRepo.securityMenu);
    var utilitiesMenu = objLocator.findLocator(objRepo.homeObjRepo.utilitiesMenu);
    var auditingMenu = objLocator.findLocator(objRepo.homeObjRepo.auditingMenu);
    var exceptionsMenu = objLocator.findLocator(objRepo.homeObjRepo.exceptionsMenu);
    var auditingMenu_hidden = objLocator.findLocator(objRepo.homeObjRepo.auditingMenu_hidden);
    var securityMenu_hidden = objLocator.findLocator(objRepo.homeObjRepo.securityMenu_hidden);
    var utilitiesMenu_hidden = objLocator.findLocator(objRepo.homeObjRepo.utilitiesMenu_hidden);
    var Home = objLocator.findLocator(objRepo.homeObjRepo.Home);
    var beqTiles = '//div[@class="widget widget-nopad"][contains(.,\'Business\')]//span[@class=\'value ng-binding\'][1]';
    var teqTiles = '//div[@class="widget widget-nopad"][contains(.,\'Technical\')]//span[@class=\'value ng-binding\'][1]';
    var businessExceptionTiles = "//a[contains(@href,'businessexception')]";
    var technicalExceptionTiles = "//a[contains(@href,'Exceptions')]";
    var resolveCellXpath = "//div[@title='Resolved'][contains(text(),'Resolved')]";
    var userTitle = objLocator.findLocator(objRepo.homeObjRepo.userTitle);

    var servicesStatus = "//td[@class='ng-binding']/..";
    var activeServices = "//td[contains(text(),'Active')]";
    var inactiveServices = "//td[contains(text(),'Disabled')]/preceding-sibling::td";
    let ex = element(by.xpath("//td[@class='ng-binding'][contains(.,'SafeEscrow ')]"));
    var pageTitle = element(by.xpath("//li[@class='ng-binding']"));
    var teqExceptionHeader = element(by.xpath("//li[contains(text(),'FAST Receive Technical Exception Queues')]"))
    var resolveCheckBox = element(by.xpath("//input[@ng-model='vmException.IncludeResolve']"))
    var serachExceptions = element(by.xpath("//i[@ng-click='vmException.search()']"))
    var dropdownDateFilter = element(by.xpath("//select[@ng-model='vmException.FilterSection']"))
    var dropdownStatus = element(by.xpath("//select[@ng-attr-placeholder='{{colFilter.placeholder || aria.defaultFilterLabel}}']"))
    var resolveGridElements = element(by.xpath("//div[@title='Resolved'][contains(text(),'Resolved')]"));


    var lastTileUnderTEQ = objLocator.findLocator(objRepo.homeObjRepo.lastTileUnderTEQ);
    var userName, title;
    var Spinner = objLocator.findLocator(objRepo.homeObjRepo.Spinner);
    var tEQFirstRecord = objLocator.findLocator(teqObjRepo.technicalExceptionQueuesObjRepo.tEQFirstRecord);
    var splitValue;
    var expectedString;
    var role;
    var Details = []
    this.openSearchPage = function (path) {
        if (typeof path === 'undefined') {
            path = '';
        }
        browser.get(path);
        return this;
    };

    this.isPageLoaded = function () {
        waitActions.waitForElementIsDisplayed(technicalExceptionTiles);

        waitActions.waitForElementIsDisplayed(teqSection);
        return this;
    };


    
    this.verifyBEQSection = function () {
        waitActions.waitForElementIsDisplayed(beqSection);
        waitActions.waitForElementIsDisplayed(beqSection);

        return this;
    };

    this.getUserNameAndRole = async () => {
        browser.sleep(10000);
        waitActions.waitForElementIsDisplayed(userTitle);

        var text = await userTitle.getText();
        var textSplit = text.split('\\');
        var textSplitUserName = textSplit[1].split(" ");
        testData.User.userName = textSplitUserName[0];
        title = textSplitUserName[1].substring(1, textSplitUserName[1].length - 1);
        console.log(testData.User.userName, title);
        console.log("The role before query", testData.User.Role);
        var Results = await DBUtilNew.ConnectDBAsync("select roleid from [Tower.Users] as Us inner join [Tower.userRoles] as USR on Us.id = USR.UserId where Us.UserName like '%" + testData.User.userName + "'")
        var roleId = JSON.stringify(Results.recordset);
        if (roleId.includes('1')) {
            testData.User.Role = "SuperAdmin";
            console.log("User Role is SuperAdmin");
        }
        else if (roleId.includes('2')) {
            testData.User.Role = "Admin";
            console.log("User Role is Admin");

        }
        else if (roleId.includes('3')) {
            testData.User.Role = "User";
            console.log("User Role is User");
        }

    }



    this.verifyTEQSection = function () {
        waitActions.waitForElementIsDisplayed(teqSection);
        return this;
    };

    this.clickOnReportingTab = function () {
        buttonActions.click(reportingMenu);
        return this;
    };

    this.navigateToExceptionsPage = function () {
        waitActions.waitForElementIsDisplayed(exceptionsMenu);

        buttonActions.click(exceptionsMenu);
    };

    this.verifyMenuItemsOnHomeScreen = function () {
        waitActions.waitForElementIsDisplayed(faiLogo);
        waitActions.waitForElementIsDisplayed(userTitle);
        waitActions.waitForElementIsDisplayed(statusMenu);
        waitActions.waitForElementIsDisplayed(helpMenu);
        waitActions.waitForElementIsDisplayed(accessRequestMenu);
        waitActions.waitForElementIsDisplayed(utilitiesMenu);
        waitActions.waitForElementIsDisplayed(securityMenu);
        waitActions.waitForElementIsDisplayed(exceptionsMenu);
        waitActions.waitForElementIsDisplayed(auditingMenu);
        waitActions.waitForElementIsDisplayed(mappingTablesMenu);
        waitActions.waitForElementIsDisplayed(reportingMenu);
        return this;
    }


    this.verifyMenuItemsOnHomeScreenAsAdmin = function () {
        waitActions.waitForElementIsDisplayed(faiLogo);
        waitActions.waitForElementIsDisplayed(userTitle);
        waitActions.waitForElementIsDisplayed(statusMenu);
        waitActions.waitForElementIsDisplayed(helpMenu);
        utilitiesMenu_hidden.getAttribute("class").then(function (value) {
            expect(value).toContain('hide');
        })

        waitActions.waitForElementIsDisplayed(accessRequestMenu);
        waitActions.waitForElementIsDisplayed(securityMenu);
        waitActions.waitForElementIsDisplayed(exceptionsMenu);
        waitActions.waitForElementIsDisplayed(auditingMenu);
        waitActions.waitForElementIsDisplayed(mappingTablesMenu);
        waitActions.waitForElementIsDisplayed(reportingMenu);
        return this;
    }




    this.verifyMenuItemsOnHomeScreenAsUser = function () {
        waitActions.waitForElementIsDisplayed(faiLogo);
        waitActions.waitForElementIsDisplayed(userTitle);
        waitActions.waitForElementIsDisplayed(statusMenu);
        waitActions.waitForElementIsDisplayed(helpMenu);
        waitActions.waitForElementIsDisplayed(accessRequestMenu);
        //expect(utilitiesMenu.isPresent()).toBe(false);  
        utilitiesMenu_hidden.getAttribute("class").then(function (value) {
            expect(value).toContain('hide');
        })
        //  expect(securityMenu.isPresent()).toBe(false);  
        securityMenu_hidden.getAttribute("class").then(function (value) {
            expect(value).toContain('hide');
        })
        waitActions.waitForElementIsDisplayed(exceptionsMenu);
        // expect(auditingMenu.isPresent()).toBe(false);  
        auditingMenu_hidden.getAttribute("class").then(function (value) {
            expect(value).toContain('hide');
        })
        waitActions.waitForElementIsDisplayed(mappingTablesMenu);
        waitActions.waitForElementIsDisplayed(reportingMenu);
        return this;
    }


    this.verifyExceptionsSection = function () {
        waitActions.waitForElementIsDisplayed(bException);
        waitActions.waitForElementIsDisplayed(tException);
        return this;
    };

    this.verifyTilesCountBEQ = function () {
        let list = element.all(by.xpath(beqTiles));
        DBUtil.ConnectDB("select * from exceptiontype where exceptionGroupID=1")
            .then(function _onSuccess(_returned) {
                console.log("BEQ Exception counttiles:" + _returned.rowsAffected);
                console.log(list.count());
                expect(_returned.rowsAffected).toContain(list.count());
            }).catch(function _onFailure(err) {
                // console.log(err);
            })
        return this;
    };

    this.verifyTilesCountTEQ = function () {
        let list = element.all(by.xpath(teqTiles));
        DBUtil.ConnectDB("select * from exceptiontype where exceptionGroupID=2")
            .then(function _onSuccess(_returned) {
                console.log("TEQ Exception counttiles:" + _returned.rowsAffected);
                console.log(list.count());
                expect(_returned.rowsAffected).toContain(list.count());
            }).catch(function _onFailure(err) {
                // console.log(err);
            });
        return this;
    };

    this.verifyBEQExceptionsBasedCount = function () {
        element.all(by.xpath(businessExceptionTiles)).each(function (element) {
            element.getText().then(function (text) {
                var splitValue = text.split('\n');
                var dbValue = splitValue[0].replace(/ /g, '_').trim();
                DBUtil.ConnectDB("select * from exception ex inner join exceptiontype et on et.ExceptionTypeId = ex.ExceptionTypeId where et.ExceptionTypeName = '" + dbValue + "' and TypeCodeId != 204")
                    .then(function _onSuccess(_returned) {
                        console.log("BEQ Exception count:" + _returned.rowsAffected);
                        console.log(splitValue[1]);
                        expect(_returned.rowsAffected.toString()).toContain(splitValue[1].toString());
                    })
                    .catch(function _onFailure(err) {
                        //  console.log(err);
                    })
            });
        });
        return this;
    };

    this.verifyTEQExceptionsBasedCount = function () {
        element.all(by.xpath(technicalExceptionTiles)).each(function (element, index) {
            element.getText().then(function (text) {
                var splitValue = text.split('\n');
                DBUtil.ConnectDB("select * from exception ex inner join exceptiontype et on et.ExceptionTypeId = ex.ExceptionTypeId where et.ExceptionTypeName = '" + splitValue[0] + "' and TypeCodeId != 204")
                    .then(function _onSuccess(_returned) {
                        // console.log(_returned.rowsAffected);
                        console.log(dbValue);
                        console.log("TEQ Exception count:" + _returned.rowsAffected);
                        console.log(splitValue[1]);
                        expect(_returned.rowsAffected.toString()).toBe(splitValue[1].toString());
                    })
                    .catch(function _onFailure(err) {
                        console.log(err);
                    })
            });
        });
        return this;
    };

    this.navigateToMappingTablesPage = function () {
        waitActions.waitForElementIsDisplayed(mappingTablesMenu);
        buttonActions.click(mappingTablesMenu);
        return this;
    };

    this.navigateToSecurityPage = function () {

        waitActions.waitForElementIsDisplayed(securityMenu);
        buttonActions.click(securityMenu);
        return this;
    };

    this.navigateToAccessRequest = function () {
        buttonActions.click(accessRequestMenu);
        return this;
    };

    this.verifyTEQExceptionsTiles = function () {
        element.all(by.xpath(technicalExceptionTiles)).each(function (element, index) {
            element.getText().then(function (text) {
                var splitValue = text.split('\n');
                var dbValue = splitValue[0].replace(/ /g, '_').trim();
                DBUtil.ConnectDB("select * from exception ex inner join exceptiontype et on et.ExceptionTypeId = ex.ExceptionTypeId where et.ExceptionTypeName = '" + dbValue + "' and TypeCodeId != 204").then(
                    function success(Results) {
                        console.log(Results.rowsAffected);
                        expect(Results.rowsAffected).toContain(splitValue[1]);
                    },
                    function error(reason) {
                        //   console.log(reason);
                    });
            });
        });
        return this;
    };

    this.verifyExceptionDatesBEQ = function () {
        element.all(by.xpath(businessExceptionTiles)).each(function (element) {
            element.getText().then(function (text) {
                var splitValue = text.split('\n');
                var dbValue = splitValue[0].replace(/ /g, '_').trim();
                if (splitValue[1].toString() != 0) {
                    DBUtil.ConnectDB("select top 1 createdDate from exception ex inner join exceptiontype et on et.ExceptionTypeId = ex.ExceptionTypeId where et.ExceptionTypeName = '"
                        + dbValue + "' order by CreatedDate desc").then(
                            function success(Results) {
                                var date = new Date(splitValue[2]); // from ui
                                //console.log("BEQ"+dbValue , date);
                                // date.setHours(date.getHours() + 5);
                                // date.setMinutes(date.getMinutes() + 30);
                                //var isoDate = date.toISOString(); //from UI
                                var isoDate = date.toUTCString();
                                console.log("date UTC:", isoDate)
                                var actualDate = isoDate.substr(0, isoDate.indexOf('.'));
                                var myJSON = JSON.stringify(Results.recordset);
                                console.log(myJSON, actualDate);
                                expect(myJSON.toString()).toContain(actualDate);
                            },
                            function error(reason) {
                                // console.log(reason);
                            });
                }
            });
        });
        return this;
    };

    this.verifyExceptionDatesTEQ = function () {
        element.all(by.xpath(technicalExceptionTiles)).each(function (element) {
            element.getText().then(function (text) {
                var splitValue = text.split('\n');
                if (splitValue[1].toString() != 0) {
                    DBUtil.ConnectDB(" select top 1 createdDate from exception ex inner join exceptiontype et on et.ExceptionTypeId = ex.ExceptionTypeId where et.ExceptionTypeName = '" + splitValue[0] + "' order by CreatedDate desc").then(
                        function success(Results) {
                            var date = new Date(splitValue[2]);
                            // date.setHours(date.getHours() + 5);
                            // date.setMinutes(date.getMinutes() + 30);
                            // var isoDate = date.toISOString();
                            var isoDate = date.toUTCString();
                            console.log("date UTC:", isoDate)
                            var actualDate = isoDate.substr(0, isoDate.indexOf('.'));
                            var myJSON = JSON.stringify(Results.recordset);
                            console.log(myJSON, actualDate);
                            expect(myJSON.toString()).toContain(actualDate);
                        },
                        function error(reason) {
                            //console.log(reason);
                        });
                }
            });
        });
        return this;
    };

    this.verifyBEQExceptionTilesValue = function () {
        var i = 0, j = 0;
        element.all(by.xpath(businessExceptionTiles)).each(function (element) {
            element.getText().then(function (text) {
                var splitValue = text.split('\n');
                var dbValue = splitValue[0].trim();
                expect(dbValue).toContain(constantsFile.HomePageConstants.beqConstants[i]);
                i++;
            });
        });
        return this;
    };

    this.verifyTEQExceptionTilesValue = function () {
        var i = 0, j = 0;
        element.all(by.xpath(technicalExceptionTiles)).each(function (element) {
            element.getText().then(function (text) {
                var splitValue = text.split('\n');
                var dbValue = splitValue[0].trim();
                console.log(constantsFile.HomePageConstants.teqConstants[i]);
                expect(dbValue).toContain(constantsFile.HomePageConstants.teqConstants[i]);
                console.log(i)
                i++;
            });
        });
        return this;
    };

    this.verifyConvoylinkInHome = function () {
        buttonActions.click(lastTileUnderTEQ);
        waitActions.waitForElementIsDisplayed(pageTitle);
        pageTitle.getText().then(function _onSuccess(text) {
            console.log(text);
            expect(text).toContain("Convoy ");
            screenshots.takeScreenshot('text');

        });
        waitActions.waitForElementIsDisplayed(tEQFirstRecord);
        browser.navigate().back();
        waitActions.waitForElementIsDisplayed(lastTileUnderTEQ);
    }



    this.verifyNavigateToTEQSection = function () {

        element.all(by.xpath(technicalExceptionTiles)).each(function (element) {
            element.getText().then(function (text) {
                var splitValue = text.split('\n');
                var dbValue = { Name: splitValue[0].trim(), count: splitValue[1] };
                Details.push(dbValue);
            });
        }, 90000)

    };


    this.NavigateToLinks = function () {

        for (var i = 0; i < Details.length; i++) {

            (function (index) {

                console.log(Details[i].Name);
                browser.get(testData.search.technicalExceptions + Details[i].Name);

                if (Details[i].count > 0)
                    waitActions.waitForElementIsDisplayed(tEQFirstRecord);
                waitActions.waitForElementIsDisplayed(pageTitle);

            })(i);

        }

        return this;

    };


    // this.testExamp = function() {
    //     return statusMenu.getAttribute("class").then(function(value) {
    //         var item = value;
    //         console.log(item);
    //         return value !== '';
    //     });
    // };

    this.verifyStatusIsGreen = function () {
        let list = element.all(by.xpath(beqTiles));
        let listofServices;
        let listofActiveServices;
        let listofInactiveServices;
        var item;
        browser.wait(function () {
            return element(by.id('button-template-url')).getAttribute("class").then(function (value) {

                console.log(value);
                if (value.includes("success")) {
                    console.log("success msg");
                    buttonActions.click(statusMenu);
                    listofServices = element.all(by.xpath(servicesStatus));
                    listofActiveServices = element.all(by.xpath(activeServices));
                    screenshots.takeScreenshot('successButtonClick');
                    expect(listofServices.count()).toBe(listofActiveServices.count());
                    // console.log(listofServices.count().toString());
                    // screenshots.takeScreenshot();
                    mouseActions.mouseMove(ex);
                    // screenshots.takeScreenshot(listofActiveServices[listofActiveServices.count() - 1]);

                }
                else {
                    console.log("failure msg");

                    buttonActions.click(statusMenu);
                    listofInactiveServices = element.all(by.xpath(inactiveServices));
                    listofServices = element.all(by.xpath(servicesStatus));

                    expect(listofServices.count()).toBe(listofInactiveServices.count());
                    for (var i = 0; i < listofInactiveServices.count(); i++) {
                        mouseActions.mouseMove(listofInactiveServices[i]);
                        screenshots.takeScreenshot();
                        console.log(listofInactiveServices[i].getText());
                        var text = listofInactiveServices[i].textContent || listofInactiveServices[i].innerText;
                        console.log(text);

                    }
                }
                return value !== '';
            });
        });
        return this;
    };

    this.verifyTEQExceptionTilesValueResolved = function () {
        var i = 0, j = 0;
        var fastReceTile = element.all(by.xpath(technicalExceptionTiles)).filter(function (ele, index) {
            return ele.getText().then(function (text) {
                return text.includes("FAST Receive");
            })
        }).first();
        fastReceTile.click();
        waitActions.waitForElementIsDisplayed(teqExceptionHeader);
        waitActions.waitForElementIsDisplayed(serachExceptions)
        Dropdownactions.selectDropdownbyValue(dropdownDateFilter, "24 hrs")
        resolveCheckBox.click()
        serachExceptions.click();
        Dropdownactions.selectDropdownbyValue(dropdownStatus, "Resolved")
        waitActions.waitForElementIsDisplayed(serachExceptions)
        waitActions.waitForElementIsDisplayed(resolveGridElements);
        expect(resolveGridElements.getText()).toBe("Resolved");
        return this;

    };
}

    // this.verifyTEQLinksInHome = function() {
    //     element.all(by.xpath(technicalExceptionTiles)).each(function (element1) {
    //         element1.getText().then(function (text)        
    //         {
    //             console.log(text);
    //             var splitValue = text.split('\n');
    //             var expectedString = splitValue[0].trim();
    //             element1.click(lastTileUnderTEQ);
    //             pageTitle.getText().then(function(actualText)
    //             {
    //                 console.log(expectedString);
    //                 console.log(actualText);
    //                 expect(actualText).toContain(expectedString);

    //             },3000);

    //             //waitActions.waitForElementIsDisplayed(Spinner);
    //             // browser.wait(function () 
    //             // {
    //             //     return Spinner.isDisplayed().then(function (result) { return !result });
    //             // }, 60000);
    //             //waitActions.waitForElementIsDisplayed();
    //             browser.navigate().back();
    //             waitActions.waitForElementIsDisplayed(lastTileUnderTEQ);
    //           //  browser.navigate().back();
    //             //this.openSearchPage(testData.search.homeUrl);
    //            // waitActions.waitForElementIsDisplayed(lastTileUnderTEQ);


    //         })
    //     })
    //     return this;
    // };


// this.verifyConvoylinkInHome = function(xpath_tile,pageHeader) {
//     buttonActions.click(lastTileUnderTEQ);
//     waitActions.waitForElementIsDisplayed(pageTitle);
//     xpath_tile.getText().then(function _onSuccess(text)        
//         {
//             console.log(text);
//             expect(text).toContain(pageHeader);
//             screenshots.takeScreenshot('text');             

//         });
//         waitActions.waitForElementIsDisplayed(tEQFirstRecord);
//         browser.navigate().back();
//         waitActions.waitForElementIsDisplayed(lastTileUnderTEQ);
// }
 //})
//};
