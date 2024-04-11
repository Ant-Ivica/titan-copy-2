using System.Web.Optimization;

namespace FA.LVIS.Tower.UI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));
            BundleTable.Bundles.ResetAll();
            var bundleCss = new StyleBundle("~/towercss");

            //bundleCss.Include("~/Content/bootstrap.min.css");
            //bundleCss.Include("~/Content/font-awesome.css");
            bundleCss.Include("~/Content/angular-gridster.min.css");
            bundleCss.Include("~/Content/angular-chart.css");
            bundleCss.Include("~/Content/angular-growl.min.css");
            bundleCss.Include("~/Content/timeline.min.css");
            bundleCss.Include("~/Content/angularjs-datetime-picker.css");
            bundleCss.Include("~/app/app.css");
            //bundleCss.Include("~/Content/ui-grid.css");
            bundleCss.Include("~/Content/isteven-multi-select.css");
            bundleCss.Include("~/Content/select.css");
            bundleCss.Include("~/ext-modules/psFramework/psFramework.min.css");
            bundleCss.Include("~/ext-modules/psMenu/psMenu.css");
            bundleCss.Include("~/ext-modules/psDashboard/psDashboard.css");
            bundleCss.Include("~/ext-modules/psMappings/psProviderMappings/main.css");

            bundles.Add(bundleCss);

            var jqbundle = new ScriptBundle("~/towerjqscripts");

            jqbundle.Include("~/Scripts/jquery-2.2.0.min.js");

            bundles.Add(jqbundle);

            var angularbundle = new ScriptBundle("~/towerangularscripts");
            angularbundle.Include("~/Scripts/angular.min.js");
            angularbundle.Include("~/Scripts/angular-animate.min.js");
            angularbundle.Include("~/Scripts/angular-route.min.js");
            angularbundle.Include("~/Scripts/angular-gridster.min.js");
            angularbundle.Include("~/Scripts/bootstrap.min.js");
            angularbundle.Include("~/Scripts/ngStorage.min.js");
            angularbundle.Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js");
            angularbundle.Include("~/Scripts/ui-grid.js");
            angularbundle.Include("~/Scripts/csv.js");
            angularbundle.Include("~/Scripts/pdfmake.js");
            angularbundle.Include("~/Scripts/vfs_fonts.js");
            angularbundle.Include("~/Scripts/angular-messages.min.js");
            angularbundle.Include("~/Scripts/angular-cookies.min.js");
            angularbundle.Include("~/Scripts/re-tree.js");
            angularbundle.Include("~/Scripts/ng-device-detector.js");
            angularbundle.Include("~/Scripts/charts/chart.min.js");
            angularbundle.Include("~/Scripts/Chart.js");
            angularbundle.Include("~/Scripts/charts/excanvas.min.js");
            angularbundle.Include("~/Scripts/charts/base.js");
            angularbundle.Include("~/Scripts/charts/line.js");
            angularbundle.Include("~/Scripts/charts/angular-chart.js");
            angularbundle.Include("~/Scripts/angular-confirm.min.js");
            angularbundle.Include("~/Scripts/angular-idle.min.js");

            angularbundle.Include("~/Scripts/checklist-model.js");
            angularbundle.Include("~/Scripts/moment.min.js");
            angularbundle.Include("~/Scripts/select-tpls.min.js");
            angularbundle.Include("~/Scripts/angularjs-datetime-picker.js");
            angularbundle.Include("~/Scripts/angular-growl.min.js");
            angularbundle.Include("~/Scripts/angular-autogrow.min.js");
            angularbundle.Include("~/Scripts/xlsx.full.min.js");

            bundles.Add(angularbundle);

            var bundle = new ScriptBundle("~/towerscripts");
            bundle.Include("~/app/appModule.js");
            bundle.Include("~/app/appConfig.js");
            bundle.Include("~/app/appRouteConfig.js");
            bundle.Include("~/app/directives/appService.js");
            bundle.Include("~/app/directives/antiForgeryTokenDirective.js");
            bundle.Include("~/app/directives/antiForgeryInterceptor.js");
            bundle.Include("~/app/appController.js");
            bundle.Include("~/app/directives/EnterClickDirective.js");
            

            bundle.Include("~/ext-modules/psMenu/psMenuModule.js");
            bundle.Include("~/ext-modules/psMenu/psMenuController.js");
            bundle.Include("~/ext-modules/psMenu/psMenuDirective.js");
            bundle.Include("~/ext-modules/psMenu/psMenuItemDirective.js");
            bundle.Include("~/ext-modules/psMenu/psMenuGroupDirective.js");
            bundle.Include("~/ext-modules/psMenu/psSubMenuController.js");
            bundle.Include("~/ext-modules/psMenu/psSubMenuDirective.js");
            bundle.Include("~/ext-modules/psMenu/psSubMenuItemDirective.js");
            bundle.Include("~/ext-modules/psMenu/ConnectorController.js");
            bundle.Include("~/ext-modules/psMenu/psFastEnvInfo.component.js");
            bundle.Include("~/ext-modules/psMenu/psAppVersionInfo.component.js");
            bundle.Include("~/ext-modules/psMenu/psSecurityChecker.component.js");

            bundle.Include("~/ext-modules/psFramework/psFrameworkModule.js");
            bundle.Include("~/ext-modules/psFramework/psFrameworkController.js");
            bundle.Include("~/ext-modules/psFramework/psFrameworkDirective.js");

            bundle.Include("~/ext-modules/psSecurity/psSecurityModule.js");
            bundle.Include("~/ext-modules/psSecurity/psSecurityController.js");
            bundle.Include("~/ext-modules/psSecurity/psSecurityDirective.js");
            bundle.Include("~/ext-modules/psSecurity/isteven-multi-select.js");

            bundle.Include("~/ext-modules/psDashboard/psDashboardModule.js");
            bundle.Include("~/ext-modules/psDashboard/psDashboardDirective.js");
            bundle.Include("~/ext-modules/psDashboard/psDashboardController.js");

            //bundle.Include("~/ext-modules/psUtilities/psUtilitiesModule.js");
            //bundle.Include("~/ext-modules/psUtilities/psUtilitiesDirective.js");

            bundle.Include("~/ext-modules/psMappings/psProviderMappings/ModalImportModule.js");

            bundle.Include("~/ext-modules/psMappings/psProviderMappings/psProviderMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psProviderMappings/psProviderMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psProviderMappings/psProviderMappingsController.js");

            bundle.Include("~/ext-modules/psMappings/psProductProviderMappings/psProductProviderMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psProductProviderMappings/psProductProviderMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psProductProviderMappings/psProductProviderMappingsController.js");

            bundle.Include("~/ext-modules/psMappings/psContactMappings/psContactMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psContactMappings/psContactMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psContactMappings/psContactMappingsController.js");

            bundle.Include("~/ext-modules/psMappings/psCustomerMappings/psCustomerMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psCustomerMappings/psCustomerMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psCustomerMappings/psCustomerMappingsController.js");

            bundle.Include("~/ext-modules/psMappings/psOutDocumentMappings/psOutDocumentMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psOutDocumentMappings/psOutDocumentMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psOutDocumentMappings/psOutDocumentMappingsController.js");

            bundle.Include("~/ext-modules/psMappings/psInDocumentMappings/psInDocumentMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psInDocumentMappings/psInDocumentMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psInDocumentMappings/psInDocumentMappingsController.js");

            bundle.Include("~/ext-modules/psMappings/psLocationMappings/psLocationMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psLocationMappings/psLocationMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psLocationMappings/psLocationMappingsController.js");

            bundle.Include("~/ext-modules/psMappings/psSubscription/psSubscriptionModule.js");
            bundle.Include("~/ext-modules/psMappings/psSubscription/psSubscriptionDirective.js");
            bundle.Include("~/ext-modules/psMappings/psSubscription/psSubscriptionController.js");

            bundle.Include("~/ext-modules/psMappings/psCategoryMappings/psCategoryMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psCategoryMappings/psCategoryMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psCategoryMappings/psCategoryMappingsController.js");

            bundle.Include("~/ext-modules/psExceptions/psFastWebOrders/psFastWebOrdersModule.js");
            bundle.Include("~/ext-modules/psExceptions/psFastWebOrders/psFastWebOrdersDirective.js");
            bundle.Include("~/ext-modules/psExceptions/psFastWebOrders/psFastWebOrdersController.js");

            bundle.Include("~/ext-modules/psMappings/psOutEventMappings/psOutEventMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psOutEventMappings/psOutEventMappingsController.js");
            bundle.Include("~/ext-modules/psMappings/psOutEventMappings/psOutEventMappingsDirective.js");

            bundle.Include("~/ext-modules/psAuditing/psAuditingModule.js");
            bundle.Include("~/ext-modules/psAuditing/psAuditingDirective.js");
            bundle.Include("~/ext-modules/psAuditing/psAuditingController.js");

            bundle.Include("~/ext-modules/psReporting/MessageLogModule.js");

            bundle.Include("~/ext-modules/psReporting/psReportingModule.js");
            bundle.Include("~/ext-modules/psReporting/psReportingDirective.js");
            bundle.Include("~/ext-modules/psReporting/psReportingController.js");
                        
            bundle.Include("~/ext-modules/psExceptions/psExceptionModule.js");
            bundle.Include("~/ext-modules/psExceptions/psExceptionDirective.js");
            bundle.Include("~/ext-modules/psExceptions/psExceptionController.js");
            bundle.Include("~/ext-modules/psUtilities/psManageServiceRequest/psManageServiceRequestModule.js");
            bundle.Include("~/ext-modules/psUtilities/psManageServiceRequest/psManageServiceRequestDirective.js");
            bundle.Include("~/ext-modules/psUtilities/psManageServiceRequest/psManageServiceRequestController.js");

            bundle.Include("~/ext-modules/psMappings/psContactProviderMappings/psContactProviderMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psContactProviderMappings/psContactProviderMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psContactProviderMappings/psContactProviderMappingsController.js");


            bundle.Include("~/ext-modules/psUtilities/psConfirmServiceRequest/psConfirmServiceRequestModule.js");
            bundle.Include("~/ext-modules/psUtilities/psConfirmServiceRequest/psConfirmServiceRequestDirective.js");
            bundle.Include("~/ext-modules/psUtilities/psConfirmServiceRequest/psConfirmServiceRequestController.js");

            bundle.Include("~/ext-modules/psUtilities/psEndpointAccess/psEndPointAccessModule.js");
            bundle.Include("~/ext-modules/psUtilities/psEndpointAccess/psEndPointAccessDirective.js");
            bundle.Include("~/ext-modules/psUtilities/psEndpointAccess/psEndPointAccessController.js");

            bundle.Include("~/ext-modules/psAccessRequest/gistfile1.js");
            bundle.Include("~/ext-modules/psAccessRequest/psAccessRequestModule.js");
            bundle.Include("~/ext-modules/psAccessRequest/psAccessRequestDirective.js");
            bundle.Include("~/ext-modules/psAccessRequest/psAccessRequestController.js");
       



            bundle.Include("~/ext-modules/psExceptions/psBusinessExceptions/psBusinessExceptionModule.js");
            bundle.Include("~/ext-modules/psExceptions/psBusinessExceptions/psBusinessExceptionController.js");
            bundle.Include("~/ext-modules/psExceptions/psBusinessExceptions/psBusinessExceptionDirective.js");

            // PDL
            bundle.Include("~/ext-modules/psMappings/psWebhooksMappings/psWebhooksMappingsModule.js");
            bundle.Include("~/ext-modules/psMappings/psWebhooksMappings/psWebhooksMappingsDirective.js");
            bundle.Include("~/ext-modules/psMappings/psWebhooksMappings/psWebhooksMappingsController.js");
            bundle.Include("~/ext-modules/psMappings/psWebhooksMappings/psWebhookDomainsMappingsController.js");


            bundles.Add(bundle);

            var submenubundle = new ScriptBundle("~/towersubmenuscripts");
            submenubundle.Include("~/ext-modules/psMappings/psFastOfficeMappings/psFastOfficeMappingsModule.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastOfficeMappings/psFastOfficeMappingsDirective.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastOfficeMappings/psFastOfficeMappingsController.js");
            
            submenubundle.Include("~/ext-modules/psMappings/psFastGabMappings/psFastGabmappingsModule.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastGabMappings/psFastGabmappingsDirective.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastGabMappings/psFastGabmappingsController.js");
            
            submenubundle.Include("~/ext-modules/psMappings/psFastFilePreferenceMappings/psFastFilePreferenceMappingsModule.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastFilePreferenceMappings/psFastFilePreferenceMappingsDirective.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastFilePreferenceMappings/psFastFilePreferenceMappingsController.js");
            
            submenubundle.Include("~/ext-modules/psMappings/psFastWorkFlowMappings/psFastWorkFlowMappingsModule.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastWorkFlowMappings/psFastWorkFlowMappingsDirective.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastWorkFlowMappings/psFastWorkFlowMappingsController.js");
            
            submenubundle.Include("~/ext-modules/psTerminalLogInformations/psTerminalLogInformationModule.js");
            submenubundle.Include("~/ext-modules/psTerminalLogInformations/psTerminalLogInformationDirective.js");
            submenubundle.Include("~/ext-modules/psTerminalLogInformations/psTerminalLogInformationController.js");
            
            submenubundle.Include("~/ext-modules/psMappings/psFastTaskMappings/psFastTaskMappingsModule.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastTaskMappings/psFastTaskMappingsDirective.js");
            submenubundle.Include("~/ext-modules/psMappings/psFastTaskMappings/psFastTaskMappingsController.js");
            
            submenubundle.Include("~/ext-modules/psMappings/psFasttoLvisDocumentMappings/psFasttoLvisDocumentMappingsModule.js");
            submenubundle.Include("~/ext-modules/psMappings/psFasttoLvisDocumentMappings/psFasttoLvisDocumentMappingsDirective.js");
            submenubundle.Include("~/ext-modules/psMappings/psFasttoLvisDocumentMappings/psFasttoLvisDocumentMappingsController.js");
            
            submenubundle.Include("~/ext-modules/psMappings/psLVISToFastDocumentMappings/psLVISToFastDocumentMappingsModule.js");
            submenubundle.Include("~/ext-modules/psMappings/psLVISToFastDocumentMappings/psLVISToFastDocumentMappingsDirective.js");
            submenubundle.Include("~/ext-modules/psMappings/psLVISToFastDocumentMappings/psLVISToFastDocumentMappingsController.js");
            
            submenubundle.Include("~/ext-modules/psHelp/psHelpModule.js");
            submenubundle.Include("~/ext-modules/psHelp/psHelpDirective.js");

            submenubundle.Include("~/ext-modules/psUtilities/psManageExternalRefNumber/psManageExternalRefNumberModule.js");
            submenubundle.Include("~/ext-modules/psUtilities/psManageExternalRefNumber/psManageExternalRefNumberDirective.js");
            submenubundle.Include("~/ext-modules/psUtilities/psManageExternalRefNumber/psManageExternalRefNumberController.js");

            
            bundles.Add(submenubundle);

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",  
            //          "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            BundleTable.EnableOptimizations = false;
            
        }
    }
}