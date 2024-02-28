"use strict";

angular.module("psFramework", ["psMenu", "psSubMenu", "psDashboard", "psSecurity", "psProviderMappings", "psProductProviderMappings", "psContactMappings", "psContactProviderMappings", "psCustomerMappings", "psOutEventMappings", "psOutDocumentMappings", "psInDocumentMappings", "psLocationMappings", "psCategoryMappings", "psAuditing", "psReporting", "psfastgabmappings", "psFastOfficeMappings", "psException", "psBusinessException", "psFastFilePreferenceMappings", "psFastWorkFlowMappings", "psTerminalLogInformations", "psFastTaskMappings", "psFasttoLvisDocumentMappings", "psLvisToFastDocumentMappings", "psHelp", "psSubscription", "psManageExternalRefNumber", "psManageServiceRequest", "psConfirmServiceRequest", "psAccessRequest", "psFastWebOrders", "psWebhooksMappings", "psEndpointAccess"]);


angular.module('psFramework').config(['growlProvider', function (growlProvider) {
    growlProvider.globalDisableCountDown(true);
    growlProvider.globalInlineMessages(true);
}]);

