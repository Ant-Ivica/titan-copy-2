"use strict";

angular.module('app').config(['$routeProvider','$httpProvider',
    function ($routeProvider, $httpProvider) {
        var routes = [
            {
                url: '/dashboard',
                config: {
                    template: '<ps-dashboard></ps-dashboard>'
                },
                activetab: 'dashboard'
            },
            {
                url: '/reporting',
                config: {
                    template: '<ps-reporting></ps-reporting>'
                },
                activetab: 'reporting'
            },
            {
                url: '/auditing',
                config: {
                    template: '<ps-Auditing></ps-Auditing>'
                },
                activetab: 'auditing'
            },
            {
                url: '/security',
                config: {
                    template: '<ps-security></ps-security>'
                },
                activetab: 'security'
            },
            {
                url: '/manageexternalrefnumber',
                config: {
                    template: '<ps-Manage-External-Ref-Number></ps-Manage-External-Ref-Number>'
                },
                activetab: 'manageexternalrefnumber'
            },
             {
                 url: '/manageservicerequest',
                 config: {
                     template: '<ps-Manage-Service-Request></ps-Manage-Service-Request>'
                 },
                 activetab: 'manageservicerequest'
            },
            {
                url: '/endpointaccess',
                config: {
                    template: '<ps-EndPoint-Access></ps-EndPoint-Access>'
                },
                activetab: 'endpointaccess'
            },
              {
                  url: '/confirmservicerequest',
                  config: {
                      template: '<ps-Confirm-Service-Request></ps-Confirm-Service-Request>'
                  },
                  activetab: 'confirmservicerequest'
              },

            {
                url: '/providermappings',
                config: {
                    template: '<ps-provider-mappings></ps-provider-mappings>'
                },
                activetab: 'providermappings'
            },
            {
                url: '/providermappings/:ExternalID',
                config: {
                    template: '<ps-provider-mappings></ps-provider-mappings>'
                },
                activetab: 'providermappings'
            },
            {
                url: '/productprovidermappings/:ProviderID',
                config: {
                    template: '<ps-product-provider-mappings></ps-product-provider-mappings>'
                },
                activetab: 'productprovidermappings'
            },
            {
                url: '/fastofficemappings/:ExternalID',
                config: {
                    template: '<ps-fast-office-mappings></ps-fast-office-mappings>'
                },
                activetab: 'fastofficemappings'
            },
            {
                url: '/fastfilepreferencemappings',
                config: {
                    template: '<ps-fast-file-Preference-mappings></ps-fast-file-Preference-mappings>'
                },
                activetab: 'fastfilepreferencemappings'
            },
            {
                url: '/Customermappings/:CustomerName?',
                config: {
                    template: '<ps-Customer-mappings></ps-Customer-mappings>'
                },
                activetab: 'Customermappings'
            },
            {
                url: '/outdocmappings/:LenderName/:isGroups',
                config: {
                    template: '<ps-out-document-mappings></ps-out-document-mappings>'
                },
                activetab: 'outdocmappings'
            },
             {
                 url: '/indocmappings',
                 config: {
                     template: '<ps-in-document-mappings></ps-in-document-mappings>'
                 },
                 activetab: 'indocmappings'
             },
             {
                 url: '/Locationmappings/:CustomerName',
                 config: {
                     template: '<ps-Location-mappings></ps-Location-mappings>'
                 },
                 activetab: 'Locationmappings'
             },
             {
                 url: '/webhooksMappings/:CustomerName',
                 config: {
                     template: '<ps-Webhooks-Mappings></ps-Webhooks-Mappings>'
                 },
                 activetab: 'webhooksMappings'
             },
              {
                  url: '/Categorymappings',
                  config: {
                      template: '<ps-Category-mappings></ps-Category-mappings>'
                  },
                  activetab: 'Categorymappings'
              },
               {
                   url: '/fastgabmappings/:LocationName',
                   config: {
                       template: '<psfastgabmappings></psfastgabmappings>'
                   },
                   activetab: 'fastgabmappings'
               },
                {
                    url: '/outeventmappings/:LenderName',
                    config: {
                        template: '<ps-out-event-mappings></ps-out-event-mappings>'
                    },
                    activetab: 'outeventmappings'
                },
                {
                    url: '/Exceptions/:ExceptionType?',
                    config: {
                        template: '<ps-Exception></ps-Exception>'
                    },
                    activetab: 'Exceptions'
                },
                {
                    url: '/businessexception/:ExceptionType?',
                    config: {
                        template: '<ps-business-exception></ps-business-exception>'
                    },
                    activetab: 'BusinessExceptions'
                },
                {
                    url: '/fastweborders',
                    config: {
                        template: '<ps-fast-web-orders></ps-fast-web-orders>'
                    },
                    activetab: 'FastWebOrders'
                },
                {
                    url: '/businessexception',
                    config: {
                        template: '<ps-business-exception></ps-business-exception>'
                    },
                    activetab: 'BusinessExceptions'
                },
                {
                    url: '/terminalloginformation',
                    config: {
                        template: '<ps-Terminal-Log-Informations></ps-Terminal-Log-Informations>'
                    },
                    activetab: 'Terminalloginformation'
                },
                {
                    url: '/FastWorkFlowMappings',
                    config: {
                        template: '<ps-Fast-Work-Flow-Mappings></ps-Fast-Work-Flow-Mappings>',
                    },
                    activetab: 'FastWorkFlowMappings'
                },
                {
                    url: '/FastTaskMappings',
                    config: {
                        template: '<ps-Fast-Task-Mappings></ps-Fast-Task-Mappings>',
                    },
                    activetab: 'FastTaskMappings'
                },
                {
                    url: '/FastToLVISDocMappings',
                    config: {
                        template: '<ps-Fastto-Lvis-Document-Mappings></ps-Fastto-Lvis-Document-Mappings>',
                    },
                    activetab: 'FastToLVISDocMappings'
                },
                {
                    url: '/LVISToFastDocMappings',
                    config: {
                        template: '<ps-Lvis-To-Fast-Document-Mappings></<ps-Lvis-To-Fast-Document-Mappings>',
                    },
                    activetab: 'LVISToFastDocMappings'
                },
                {
                    url: '/subscription/:CustomerName/:isCategory',
                    config: {
                        template: '<ps-Subscription></ps-Subscription>'
                    },
                    activetab: 'Subscription'
                },
             {
                 url: '/Contactmappings/:LocationName',
                 config: {
                     template: '<ps-Contact-mappings></ps-Contact-mappings>'
                 },
                 activetab: 'Contactmappings'
             },
             {
                 url: '/ContactProvidermappings/:CustomerId',
                 config: {
                     template: '<ps-Contact-Provider-mappings></ps-Contact-Provider-mappings>'
                 },
                 activetab: 'ContactProvidermappings'
             }
             , {
                 url: '/AccessRequest',
                 config: {
                     template: '<ps-Access-Request></ps-Access-Request>'
                 },
                 activetab: 'AccessRequest'
             }
             , {
                 url: '/AccessRequest/:emailid',
                 config: {
                     template: '<ps-Access-Request></ps-Access-Request>'
                 },
                 activetab: 'AccessRequest'
             }

        ];

        routes.forEach(function (route) {
            $routeProvider.when(route.url, route.config, route.activetab);

        });

        $routeProvider.otherwise({ redirectTo: '/dashboard' });

        $httpProvider.interceptors.push('antiForgeryInterceptor');

    }]).constant('apiUrl', '');

function navCtrl($scope, $route) {
    //we set $route to  we have access to it in the HTML
    $scope.$route = $route;
}