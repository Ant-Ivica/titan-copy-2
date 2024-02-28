describe('Test Execution on InboundDocumentMapping', function () {

    'use strict';
        
    var pages = {};
    var testData = require('../resources/testData.json');
    pages.InboundDocumentMapping = require('../pages/InboundDocumentMapping.js');
    pages.Customers = require('../pages/Customers.js');

    var inboundDocumentMappingPage = new pages.InboundDocumentMapping();
    var customerPage = new pages.Customers();
    
    
    it('Verify InboundDocumentMapping Screen Loaded properly', function () {
        inboundDocumentMappingPage.openSearchPage(testData.search.homeUrl);
        inboundDocumentMappingPage.isPageLoaded();     
    });

    it('Verify Add and delete inbound document mapping', function(){
        inboundDocumentMappingPage.addanddeleteNewInboundDocument();
    });

});