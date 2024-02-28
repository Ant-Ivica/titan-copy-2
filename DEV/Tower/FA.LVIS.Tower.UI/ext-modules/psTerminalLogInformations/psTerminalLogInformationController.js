"use strict";

angular.module('psTerminalLogInformations').controller('psTerminalLogInformationsController', psTerminalLogInformationsController);
angular.module('psTerminalLogInformations').controller('psTerminalLogInformationsRowCtrl', psTerminalLogInformationsRowCtrl);
angular.module('psTerminalLogInformations').service('psTerminalLogInformationsRowEditor', psTerminalLogInformationsRowEditor);
angular.module('psTerminalLogInformations').service('psTerminalLogInformationsApiUri', psTerminalLogInformationsApiUri);


angular.module('psTerminalLogInformations').factory('UserInfo', ['$http', '$q', function ($http, $q) {
    return {
        getUser: function () {
            var deferred = $q.defer();
            $http.get('Security/GetCurrentUser/').then(function (response) {
                $timeout(function () {
                    deferred.resolve(response.data);
                }, 3000);
            }, function (error) {
                deferred.reject(error);
            });

            return deferred.promise;
        }
    };
}]);


psTerminalLogInformationsController.$inject = ['$route', '$routeParams', '$scope', '$rootScope', '$http', '$interval', '$uibModal', 'uiGridGroupingConstants', '$window', '$filter', '$confirm', 'UserInfo', '$location', '$cookies', 'growl', 'psTerminalLogInformationsRowEditor', 'uiGridConstants', 'modalProvider', 'psTerminalLogInformationsApiUri'];
function psTerminalLogInformationsController($route, $routeParams, $scope, $rootScope, $http, $interval, $uibModal, uiGridGroupingConstants, $window, $filter, $confirm, UserInfo, $location, $cookies, growl, psTerminalLogInformationsRowEditor, uiGridConstants, modalProvider, psTerminalLogInformationsApiUri) {
    var vmTILogs = this;
    vmTILogs.IncludeInfo = true;
    vmTILogs.IncludeError = true;

    vmTILogs.getCurrentUser = function () {
        UserInfo.getUser().then(function (response) {
            $rootScope.activityright = response.ActivityRight;
        }, function (error) {

        });
    };

    if (!$rootScope.activityright) {
        var activityright = $cookies.get('activityright');
        if (activityright)
        { $rootScope.activityright = activityright; }
    }

    if (!$rootScope.activityright) {
        UserInfo.getUser().then(function (response) {
            $rootScope.activityright = response.ActivityRight;
        }, function (error) {

        });
    }

    var showMenuloginfo = true;
    if ($rootScope.tenantname != 'LVIS')
        showMenuloginfo = false;
    else
        showMenuloginfo = true;

    vmTILogs.showMenuloginfo = showMenuloginfo;
    var showMenuloginfofastweborders = false;
    if (($rootScope.tenantname == 'LVIS' || $rootScope.tenantname == 'Air Traffic Control') && $rootScope.activityright == 'SuperAdmin')
        showMenuloginfofastweborders = true;
    else
        showMenuloginfofastweborders = false;
    vmTILogs.showMenuloginfofastweborders = showMenuloginfofastweborders;



    var hasAccess = false;
    var newDate = new Date();
    var date = new Date();
    vmTILogs.Fromdate = $filter('date')(new Date(), 'MM/dd/yyyy');
    //vmTILogs.StartdatetFilter = "00";
    //vmTILogs.EnddatetFilter = "24";
    var currenendtime = $filter('date')(new Date(), 'hh:mm a');

    var currEnd_time = currenendtime.split(" ");
    var SAMPM = currEnd_time[1];
    var time = currEnd_time[0];
    var finaltime = parseInt(time);

    if (SAMPM == 'AM') {
        if (finaltime == 1) {
            vmTILogs.StartdatetFilter = '01';
            vmTILogs.EnddatetFilter = '02';
        }
        if (finaltime == 2) {
            vmTILogs.StartdatetFilter = '02';
            vmTILogs.EnddatetFilter = '03';
        }
        if (finaltime == 3) {
            vmTILogs.StartdatetFilter = '03';
            vmTILogs.EnddatetFilter = '04';
        }
        if (finaltime == 4) {
            vmTILogs.StartdatetFilter = '04';
            vmTILogs.EnddatetFilter = '05';
        }
        if (finaltime == 5) {
            vmTILogs.StartdatetFilter = '05';
            vmTILogs.EnddatetFilter = '06';
        }
        if (finaltime == 6) {
            vmTILogs.StartdatetFilter = '06';
            vmTILogs.EnddatetFilter = '07';
        }
        if (finaltime == 7) {
            vmTILogs.StartdatetFilter = '07';
            vmTILogs.EnddatetFilter = '08';
        }
        if (finaltime == 8) {
            vmTILogs.StartdatetFilter = '08';
            vmTILogs.EnddatetFilter = '09';
        }
        if (finaltime == 9) {
            vmTILogs.StartdatetFilter = '09';
            vmTILogs.EnddatetFilter = '10';
        }
        if (finaltime == 10) {
            vmTILogs.StartdatetFilter = '10';
            vmTILogs.EnddatetFilter = '11';
        }
        if (finaltime == 11) {
            vmTILogs.StartdatetFilter = '11';
            vmTILogs.EnddatetFilter = '12';
        }
        if (finaltime == 12) {
            vmTILogs.StartdatetFilter = '00';
            vmTILogs.EnddatetFilter = '01';
        }
    }

    if (SAMPM == 'PM') {
        if (finaltime == 12) {
            vmTILogs.StartdatetFilter = '12';
            vmTILogs.EnddatetFilter = '13';
        }
        if (finaltime == 1) {
            vmTILogs.StartdatetFilter = '13';
            vmTILogs.EnddatetFilter = '14';
        }
        if (finaltime == 2) {
            vmTILogs.StartdatetFilter = '14';
            vmTILogs.EnddatetFilter = '15';
        }
        if (finaltime == 3) {
            vmTILogs.StartdatetFilter = '15';
            vmTILogs.EnddatetFilter = '16';
        }
        if (finaltime == 4) {
            vmTILogs.StartdatetFilter = '16';
            vmTILogs.EnddatetFilter = '17';
        }
        if (finaltime == 5) {
            vmTILogs.StartdatetFilter = '17';
            vmTILogs.EnddatetFilter = '18';
        }
        if (finaltime == 6) {
            vmTILogs.StartdatetFilter = '18';
            vmTILogs.EnddatetFilter = '19';
        }
        if (finaltime == 7) {
            vmTILogs.StartdatetFilter = '19';
            vmTILogs.EnddatetFilter = '20';
        }
        if (finaltime == 8) {
            vmTILogs.StartdatetFilter = '20';
            vmTILogs.EnddatetFilter = '21';
        }
        if (finaltime == 9) {
            vmTILogs.StartdatetFilter = '21';
            vmTILogs.EnddatetFilter = '22';
        }
        if (finaltime == 10) {
            vmTILogs.StartdatetFilter = '22';
            vmTILogs.EnddatetFilter = '23';
        }
        if (finaltime == 11) {
            vmTILogs.StartdatetFilter = '23';
            vmTILogs.EnddatetFilter = '00';
        }
    }

    $scope.totalCount = 0;
    $scope.totalPageCount = 0; 

    vmTILogs.StartDateFilterSelection = [
        
       
         {
             'title': '24',
             'value': '00:00:01 AM'
         }
    ,
    {
        'title': '01',
        'value': '01:00 AM'
    },
    {
        'title': '02',
        'value': '02:00 AM'
    },
     {
         'title': '03',
         'value': '03:00 AM'
     },
     {
         'title': '04',
         'value': '04:00 AM'
     },
     {
         'title': '05',
         'value': '05:00 AM'
     },
     {
         'title': '06',
         'value': '06:00 AM'
     },
     {
         'title': '07',
         'value': '07:00 AM'
     },
    {
        'title': '08',
        'value': '08:00 AM'
    },
    {
        'title': '09',
        'value': '09:00 AM'
    },
    {
        'title': '10',
        'value': '10:00 AM'
    },
    {
        'title': '11',
        'value': '11:00 AM'
    },
    {
        'title': '12',
        'value': '12:00 PM'
    },
    {
        'title': '13',
        'value': '01:00 PM'
    },
    {
        'title': '14',
        'value': '02:00 PM'
    },
    {
        'title': '15',
        'value': '03:00 PM'
    },
    {
        'title': '16',
        'value': '04:00 PM'
    },
    {
        'title': '17',
        'value': '05:00 PM'
    },
    {
        'title': '18',
        'value': '06:00 PM'
    },
    {
        'title': '19',
        'value': '07:00 PM'
    },
    {
        'title': '20',
        'value': '08:00 PM'
    },
    {
        'title': '21',
        'value': '09:00 PM'
    },
    {
        'title': '22',
        'value': '10:00 PM'
    },
    {
        'title': '23',
        'value': '11:00 PM'
    }   
    ];


    vmTILogs.EndDateFilterSelection = [
      
          {
              'title': '00',
              'value': '11:59 PM'
          },
           {
               'title': '24',
               'value': '00:00:01 AM'
           }
    ,
    {
        'title': '01',
        'value': '01:00 AM'
    },
    {
        'title': '02',
        'value': '02:00 AM'
    },
     {
         'title': '03',
         'value': '03:00 AM'
     },
     {
         'title': '04',
         'value': '04:00 AM'
     },
     {
         'title': '05',
         'value': '05:00 AM'
     },
     {
         'title': '06',
         'value': '06:00 AM'
     },
     {
         'title': '07',
         'value': '07:00 AM'
     },
    {
        'title': '08',
        'value': '08:00 AM'
    },
    {
        'title': '09',
        'value': '09:00 AM'
    },
    {
        'title': '10',
        'value': '10:00 AM'
    },
    {
        'title': '11',
        'value': '11:00 AM'
    },
    {
        'title': '12',
        'value': '12:00 PM'
    },
    {
        'title': '13',
        'value': '01:00 PM'
    },
    {
        'title': '14',
        'value': '02:00 PM'
    },
    {
        'title': '15',
        'value': '03:00 PM'
    },
    {
        'title': '16',
        'value': '04:00 PM'
    },
    {
        'title': '17',
        'value': '05:00 PM'
    },
    {
        'title': '18',
        'value': '06:00 PM'
    },
    {
        'title': '19',
        'value': '07:00 PM'
    },
    {
        'title': '20',
        'value': '08:00 PM'
    },
    {
        'title': '21',
        'value': '09:00 PM'
    },
    {
        'title': '22',
        'value': '10:00 PM'
    },
    {
        'title': '23',
        'value': '11:00 PM'
    }
    
    ];
    vmTILogs.Disabledate = true;
    vmTILogs.Busy = false;
    vmTILogs.Typecodestatus = false;
    vmTILogs.editReportRow = psTerminalLogInformationsRowEditor.editReportRow;
    vmTILogs.serviceGrid = {
        enableColumnResize: true,
        treeRowHeaderAlwaysVisible: true,
        enableRowSelection: true,
        enableRowHeaderSelection: false,
        multiSelect: false,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        enableSelectAll: false,
        paginationCurrentPage : 1,
        paginationPageSizes: [15, 30, 45],
        paginationPageSize: 15,
        minRowsToShow: 16,
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        groupingShowAggregationMenu: 0,
        columnDefs: [
            { field: 'Id', name: 'Log Id', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Date', name: 'Date', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Level', name: 'Level', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'logger', name: 'Logger', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'Message', name: 'Message', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            { field: 'HostName', name: 'Host Name', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
            //{ field: 'Excecption', name: 'Exception', headerCellClass: 'grid-header', enableCellEdit: false, groupingShowAggregationMenu: false, cellTooltip: true },
        ],
        rowTemplate: "<div ng-dblclick=\"grid.appScope.vmTILogs.editReportRow(grid, row)\" ng-repeat=\"(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name\" class=\"ui-grid-cell\" ng-class=\"{ 'ui-grid-row-header-cell': col.isRowHeader }\" ui-grid-cell></div>",

        onRegisterApi: function (gridApi) {
            vmTILogs.serviceGrid.gridApi = gridApi;           

        },

    };
    

    vmTILogs.search = search;
    //search();
    vmTILogs.ValidateError = false;
    var starttime = '';
    var endttime = '';
    function ValidateTime(starttime, endttime) {
        if (vmTILogs.gmessage != undefined)
            vmTILogs.gmessage.destroy();

        var StartTime = starttime;
        var EndTime = endttime;
        var seldate = vmTILogs.Fromdate.toString();


        if (StartTime != '' && EndTime != '' && seldate != '') {
            var Stime = '';
            var Etime = '';
            if (StartTime != '' && EndTime != '') {
                Stime = StartTime;
                Etime = EndTime;

                var start_time = Stime;
                var end_time = Etime;
                start_time = start_time.split(" ");
                var SAMPM = start_time[1];
                var time = start_time[0].split(":");
                var stime = time[0];
                if (stime == '12') {
                    stime = '00';
                }
                end_time = end_time.split(" ");
                var EAMPM = end_time[1];
                var time1 = end_time[0].split(":");
                var etime = time1[0];
                if (etime == '12') {
                    etime = '00';
                }

                var date = vmTILogs.Fromdate.toString();

                var date1 = date.replace(/[/]/g, '').trim();
                var sdatetime = date1 + stime;
                var edatetime = date1 + etime;

                stime = parseInt(sdatetime);
                etime = parseInt(edatetime);

                if (SAMPM == 'AM' && EAMPM == 'AM') {
                    if (stime >= etime) {
                        vmTILogs.Busy = false;
                        vmTILogs.ValidateError = true;
                        vmTILogs.gmessage = growl.error("End Time cannot be earlier than the Start Time");
                    }
                }

                if (SAMPM == 'PM' && EAMPM == 'PM') {
                    if (stime > etime) {
                        vmTILogs.Busy = false;
                        vmTILogs.ValidateError = true;
                        vmTILogs.gmessage = growl.error("End Time cannot be earlier than the Start Time");
                    }
                }

                if (SAMPM == 'PM' && EAMPM == 'AM') {

                    vmTILogs.Busy = false;
                    vmTILogs.ValidateError = true;
                    vmTILogs.gmessage = growl.error("End Time cannot be earlier than the Start Time");

                }
            }
        }

    }
    


    function search() {
      
        if (vmTILogs.gmessage != undefined)
            vmTILogs.gmessage.destroy();

        if (vmTILogs.Fromdate != "") {
            vmTILogs.ValidateError = false;
            vmTILogs.Busy = true;

            var StartTime1 = $filter('filter')(vmTILogs.StartDateFilterSelection, { title: vmTILogs.StartdatetFilter });
            var EndTime1 = $filter('filter')(vmTILogs.EndDateFilterSelection, { title: vmTILogs.EnddatetFilter });
            var StartTime = ''
            var EndTime = '';
            var Start_Time = ''
            var End_Time = '';

            //collect array object Properties
            angular.forEach(StartTime1, function (item) {
                Start_Time = item.value;

            });
            //collect array object Properties
            angular.forEach(EndTime1, function (item) {
                End_Time = item.value;
            });


            //validate Time
            ValidateTime(Start_Time, End_Time);
            if (vmTILogs.ValidateError) {
                vmTILogs.Busy = false;
                return;
            }
            //Include/Exclude Resolved Status Excecption
            if (vmTILogs.IncludeDebug) {
                vmTILogs.Typecodestatus = true;
            }
            else {
                vmTILogs.Typecodestatus = false;
            }
            
            //collect all parameters
            var Details = {
                Fromdate: vmTILogs.Fromdate.toString(),
                Typecodestatus: vmTILogs.Typecodestatus,
                StartTime: Start_Time,
                EndTime: End_Time,
                ErrorEnabled: vmTILogs.IncludeError,
                InfoEnabled: vmTILogs.IncludeInfo,
                MessageText: vmTILogs.MessageText,
                currPage: vmTILogs.serviceGrid.paginationCurrentPage,
                pageSize: vmTILogs.serviceGrid.paginationPageSize                  
            }

            $http.post(psTerminalLogInformationsApiUri.GetTerminalLogInformationdetails, Details)
                .then(function (response) {

                    vmTILogs.serviceGrid.data = [];
                    
                    vmTILogs.serviceGrid.data = response.data;
                    vmTILogs.Busy = false;
                    

                    
                    $http.post(psTerminalLogInformationsApiUri.GetLogDetailsCount, Details)
                        .then(function (response2) {
                            $scope.totalCount = response2.data;
                            $scope.totalPageCount = Math.ceil($scope.totalCount / vmTILogs.serviceGrid.paginationPageSize);
                        });
                });

        }
    }

    $scope.next = function () {
        vmTILogs.serviceGrid.paginationCurrentPage = Number(vmTILogs.serviceGrid.paginationCurrentPage) +1; 
        search()
    };
    $scope.last = function () {
        vmTILogs.serviceGrid.paginationCurrentPage = Math.ceil($scope.totalCount / vmTILogs.serviceGrid.paginationPageSize); 
        search()
    };
    $scope.previous = function () {
        vmTILogs.serviceGrid.paginationCurrentPage = Number(vmTILogs.serviceGrid.paginationCurrentPage) - 1; 
        search()
    };
    $scope.first = function () {
        vmTILogs.serviceGrid.paginationCurrentPage = 1; 
        search()
    };
    $scope.pageSizeChange = function () {
        
        vmTILogs.serviceGrid.paginationCurrentPage = 1; 
        search()
    }

    $scope.expandAll = function () {
        vmTILogs.serviceGrid.gridApi.treeBase.expandAllRows();
    };

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.closemodal = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };

    $scope.toggleRow = function (rowNum) {
        vmTILogs.serviceGrid.gridApi.treeBase.toggleRowTreeState($scope.gridApi.grid.renderContainers.body.visibleRowCache[rowNum]);
    };

    $scope.sampledetails = function (grid, row) {
        modalProvider.openPopupModal(row.entity.ServiceRequestId);
    };

    $scope.searchFromIcon = function () {
        vmTILogs.serviceGrid.paginationCurrentPage = 1; 
        vmTILogs.search();
    }

}

psTerminalLogInformationsRowEditor.$inject = ['$http', '$rootScope', '$uibModal'];
function psTerminalLogInformationsRowEditor($http, $rootScope, $uibModal) {

    var service = {};
    service.editReportRow = editReportRow;

    function editReportRow(grid, row) {
        $uibModal.open({
            templateUrl: 'ext-modules/psTerminalLogInformations/Terminallog-information-edit.html',
            controller: ['$http', '$uibModalInstance', 'grid', 'row', '$window', '$scope', '$filter', psTerminalLogInformationsRowCtrl],
            controllerAs: 'vmTILogs',
            resolve: {
                grid: function () {
                    return grid;
                },
                row: function () {
                    return row;
                }
            }
        });
    }

    return service;
}


function psTerminalLogInformationsRowCtrl($http, $uibModalInstance, grid, row, $window, $scope, $filter) {

    var vmTILogs = this;
    vmTILogs.entity = angular.copy(row.entity);

}

function psTerminalLogInformationsApiUri() {
    this.GetTerminalLogInformationdetails = 'api/terminalLogInformation/GetTerminalLogInformationdetails';
    this.GetLogDetails = 'api/terminalLogInformation/GetLogDetails';
    this.GetLogDetailsCount = 'api/terminalLogInformation/GetLogDetailsCount';
}






