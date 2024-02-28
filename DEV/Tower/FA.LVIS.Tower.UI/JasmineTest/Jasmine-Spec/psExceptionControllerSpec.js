'use strict'
describe("Technical Exception Module Controllers", function () {

    var $controllerConstructor;
    var scope;
    var techExeCtrl;
    var techExeRowCtrl;
    var $cookie = {};
    var uibModal;
    var filter;
    var $httpBackend;
    var timeout;
    var userInfo;
    var modalProvider;
    var exceptionRowEditor;
    var httpLocalBackend;
    var today;
    var filterSection15 = "15";
    var typecodestatus = false;
    var dayString = "07/31/2017";
    var userInfoData =
        {
            'AccessFailedCount': 0, 'ActiveDirectoryPassword': null, 'ActivityRight': 'User', 'CanManageTEQ': false, 'CanManageBEQ': false
         };
    var reporting = {
        'ApplicationId': null, 'CustomerId': 0, 'CustomerName': null, 'CustomerRefNum': null, 'ExternalRefNum': null, 'InternalRefId': null,
        'InternalRefNum': null, 'LenderId': null, 'ServiceRequestId': 0, 'Tenant': null, 'createddate': null, 'service': null
    };
    var status = {
        'ID': 201, 'Name': 'New', 'State' : 0
    };

    var TEQData =
        [{
            'Comments': null, 'CreatedBy': '', 'CreatedDate': "9/8/2017 5:14:52 PM", 'DocumentObjectid': 127659, 'ExceptionDesc': 'LendingQBEnrichmentHandler.ProcessMessage Exception: System.InvalidOperationException: There was an error generating the XML document. ---> .....',
            'ExceptionType': 'LendingQBEnrichmentHandler', 'ExceptionTypeid': 0, 'ExternalRefNum': 'UBC_263', 'InvolveResolved': false,
            'LastModifiedBy': '', 'LastModifiedDate': '9/8/2017 5:14:52 PM', 'MessageContent': null, 'MessageType': 'Service Request', 'MessageTypeid': 10020, 'Notes': null,
            'ParentExternalRefNum': '', 'Reporting': reporting, 'ServiceRequestId': 19193, 'ServiceType': 'EscrowTitle', 'Status': status, 'Tenant': 'Mortgage Solutions',
            'TenantId': 0, 'TransactionType': null, 'TypeCodeId': 201, 'children': null
        },
        {
            'Comments': null, 'CreatedBy': '', 'CreatedDate': "9/11/2017 5:14:52 PM", 'DocumentObjectid': 127737, 'ExceptionDesc': 'LendingQBEnrichmentHandler.ProcessMessage Exception: System.InvalidOperationException: There was an error generating the XML document. ---> .....',
            'ExceptionType': 'LendingQBEnrichmentHandler', 'ExceptionTypeid': 0, 'ExternalRefNum': 'UBC_264', 'InvolveResolved': false,
            'LastModifiedBy': '', 'LastModifiedDate': '9/11/2017 5:14:52 PM', 'MessageContent': null, 'MessageType': 'Service Request', 'MessageTypeid': 10020, 'Notes': null,
            'ParentExternalRefNum': '', 'Reporting': reporting, 'ServiceRequestId': 19203, 'ServiceType': 'EscrowTitle', 'Status': status, 'Tenant': 'Mortgage Solutions',
            'TenantId': 0, 'TransactionType': null, 'TypeCodeId': 201, 'children': null
        }];

    var FDetails = {
        'ReferenceNoType': '1',
        'ReferenceNo': 'UBC'
    }

    var msgContent = "<?xml version='1.0' encoding='utf-8'?>" +
                        "<Order>" +
                          "<IsFromExceptionQueue>false</IsFromExceptionQueue> <CustomerRefNum>1ALHU70240</CustomerRefNum> <ExternalRefNum>1ALHU1212-6364-48c6-aba3-3154ebfa8ecc</ExternalRefNum>" +
                          "<InternalRefId>0</InternalRefId> <InternalRefNum>2000749LV</InternalRefNum> <LocationId>4390</LocationId> <OriginalSource>LendingQB</OriginalSource> <RawRefNumFromSender>1ALHU1212-6364-48c6-aba3-3154ebfa8ecc</RawRefNumFromSender>" +
                          "<RecievedDateTime>2017-09-18T13:38:19.4212434+05:30</RecievedDateTime> <ServiceRequestId>19343</ServiceRequestId> <TenantId>2</TenantId> <OriginatingApplicationId>0</OriginatingApplicationId>" +
                          "<Liabilities>" +
                            "<Liability>" +
                              "<LiabilityDescription>3728 Clark Road, Live Oak, CA 95953</LiabilityDescription> <LiabilityHolderFullName>RUSHMORE LOAN MGMT SER</LiabilityHolderFullName> <LiabilityType>MortgageLoan</LiabilityType>" +
                              "<LiabilityTypeOtherDesc>3728 Clark Road, Live Oak, CA 95953</LiabilityTypeOtherDesc> <PayoffAccountNumber>1027600711241</PayoffAccountNumber> <PayoffActionType>TitleCompanyOrderPayoff</PayoffActionType>" +
                              "<PayoffAmount>298104.00</PayoffAmount> <PayoffExpDate>0001-01-01T00:00:00</PayoffExpDate> <PayoffApplicationSequenceType>EmptyEnumValue</PayoffApplicationSequenceType>" +
                              "<PerDiem>0</PerDiem>" +
                            "</Liability>" +
                            "<Liability>" +
                              "<LiabilityDescription>Installment Loan</LiabilityDescription> <LiabilityHolderFullName>SOFI LENDING CORP</LiabilityHolderFullName> <LiabilityType>Installment</LiabilityType>" +
                              "<LiabilityTypeOtherDesc>Installment Loan</LiabilityTypeOtherDesc> <PayoffAccountNumber>PL163621</PayoffAccountNumber> <PayoffActionType>TitleCompanyOrderPayoff</PayoffActionType>" +
                              "<PayoffAmount>32999.00</PayoffAmount> <PayoffExpDate>0001-01-01T00:00:00</PayoffExpDate> <PayoffApplicationSequenceType>EmptyEnumValue</PayoffApplicationSequenceType> <PerDiem>0</PerDiem>" +
                            "</Liability>" +
                          "</Liabilities>" +
                          "<Loans>" +
                            "<Loan>" +
                              "<LienPriorityType>EmptyEnumValue</LienPriorityType> <LoanAmount>400000.00</LoanAmount> <EstimatedClosingDate>0001-01-01T00:00:00</EstimatedClosingDate> <LoanPurposeType>Refinance</LoanPurposeType>" +
                              "<BusinessSegmentType>Residential</BusinessSegmentType> <LoanRefNum>1ALHU70240</LoanRefNum> <MortgageType>Conventional</MortgageType>" +
                            "</Loan>" +
                          "</Loans>" +
                          "<OrderDocuments />" +
                          "<Parties>" +
                            "<Party d3p1:type= 'LegalEntityParty' xmlns:d3p1='http://www.w3.org/2001/XMLSchema-instance'>" +
                              "<FullName>Michigan Mutual, Inc.</FullName> <PartyRoleType>Other</PartyRoleType>" +
                              "<PartyAddresses>" +
                                "<Address>" +
                                  "<AddressLine1>911 Military Street</AddressLine1> <AddressType>Other</AddressType> <CityName>Port Huron</CityName> <PostalCode>48060</PostalCode> <StateCode>MI</StateCode>" +
                                "</Address>" +
                              "</PartyAddresses>" +
                              "<BankruptcyIndicator>false</BankruptcyIndicator>" +
                              "<LocationId d3p1:nil='true' /> <LegalEntityType>BusinessEntity</LegalEntityType> <BusinessEntityType>None</BusinessEntityType>" +
                              "<Contacts>" +
                                "<Contact>" +
                                  "<FullName>1ALHUFN 1ALHULN</FullName> <PartyRoleType>Other</PartyRoleType> <PartyAddresses /> <BankruptcyIndicator>false</BankruptcyIndicator> <FirstName>1ALHUFN</FirstName>" +
                                  "<LastName>1ALHULN</LastName> <MaritalStatusType>NotApplicable</MaritalStatusType> <LanguageType>EmptyEnumValue</LanguageType> <ContactPoints> <ContactPoint> <ContactPointType>Work</ContactPointType>" +
                                      "<EmailAccount>dderisi@mimutual.com</EmailAccount> <PhoneNumber>8109829948</PhoneNumber> </ContactPoint> </ContactPoints>" +
                                "</Contact>" +
                              "</Contacts>" +
                            "</Party>" +
                          "</Parties>" +
                          "<Properties>" +
                            "<Property>" +
                              "<LegalDescription>See Exhibit A Attached Hereto and Made A Part Hereof</LegalDescription> <PropertyType>Other</PropertyType> <SalesContractAmount>575000.00</SalesContractAmount>" +
                              "<TitleType>Other</TitleType> <PropertyAddress> <AddressLine1>1ALHU Clark Road</AddressLine1> <AddressType>Other</AddressType> <CityName>Live Oak</CityName> <CountyFIPSCode>101</CountyFIPSCode>" +
                                "<CountyName>Sutter</CountyName> <PostalCode>95953</PostalCode> <StateCode>CA</StateCode> </PropertyAddress> <ConstructionMethodType>EmptyEnumValue</ConstructionMethodType>" +
                              "<PropertyCurrentOccupancyType>EmptyEnumValue</PropertyCurrentOccupancyType> </Property> </Properties>" +
                          "<Services>" +
                            "<ServiceInfo>" +
                              "<Products>" +
                                "<Product> <ProductName>ALTA Loan Policy - Extended</ProductName> <ProductId>20001</ProductId> </Product>" +
                              "</Products>" +
                              "<ProviderId>3046</ProviderId> <ServiceId>3</ServiceId> <ServiceServiceRequestId>19343</ServiceServiceRequestId> <ActionType>New</ActionType> </ServiceInfo>" +
                          "</Services>" +
                          "<Relationships />" +
                          "<InternalData>" +
                            "<TitleOfficeId>11057</TitleOfficeId> <EscrowOfficeId>11057</EscrowOfficeId> <EscrowStateCode>AZ</EscrowStateCode> <EscrowCountyName>Maricopa</EscrowCountyName>" +
                            "<ProviderRegionId>10926</ProviderRegionId> <BusinessSourceABEID>130950147</BusinessSourceABEID> <LenderABEID>130950147</LenderABEID> <LenderABEIDName>Southwest Direct Mortgage, LLC</LenderABEIDName>" +
                            "<PreferenceMapId>0</PreferenceMapId> <SearchType>0</SearchType> </InternalData>" +
                          "<ProcessEventMessages />" +
                          "<OrderComments />" +
                        "</Order>";

    var comments = [];

    var exceptionId = 4311;
    var documentObjectId = 128630;

    var modal = {
        close: function () { }, dismiss: function () { }
    };

    var uibModalInstance;

    var uibModal;

    beforeEach(module('ngCookies', 'app', 'psException'));

    beforeEach(inject(function ($controller, $rootScope, $http, $filter, UserInfo, $cookies, modalProvider, $timeout, $templateCache) {

        $controllerConstructor = $controller;
        scope = $rootScope.$new();
        $httpBackend = $http;
        userInfo = UserInfo;
        timeout = $timeout;
        filter = $filter;
        modalProvider = modalProvider;
        ////uibModal = $uibModal;

        techExeCtrl = $controllerConstructor("psExceptionController", { $scope: scope, $filter: filter, UserInfo: userInfo, modalProvider: modalProvider });

        ////techExeRowCtrl = $controllerConstructor("psExceptionRowEditCtrl", { $scope: scope, $uibModalInstance: modal, $uibModal: uibModal });


    }));

    beforeEach(inject(function ($httpBackend) {
        httpLocalBackend = $httpBackend;

        httpLocalBackend.whenGET('Security/GetCurrentUser/').respond(200, userInfoData);

        httpLocalBackend.whenGET('ExceptionController/GetTEQExceptionsbyFilter/' + filterSection15 + '/' + typecodestatus).respond(200, TEQData);

        httpLocalBackend.whenGET('ExceptionController/GetTEQExceptionByReferenceNum', FDetails).respond(200, TEQData);

        ////httpLocalBackend.whenGET('ExceptionController/GetMessageContent/' + documentObjectId).respond(200, msgContent);

        ////httpLocalBackend.whenGET('ExceptionController/GetExceptionNotes/' + exceptionId).respond(200, comments);
    }));

    describe("Date Validation for the search filter", function () {        

        it("Start date must be always less than the through date", function () {
            var prevdt = new Date(parseInt(dayString.substring(6), 10),        // Year
                              parseInt(dayString.substring(0, 2), 10) - 1, // Month (0-11)
                              parseInt(dayString.substring(3, 5), 10));    // Day
            today = new Date();
            techExeCtrl.Fromdate = filter('date')(prevdt, 'MM/dd/yyyy');
            techExeCtrl.ThroughDate = filter('date')(today, 'MM/dd/yyyy');
                        
            techExeCtrl.ValidateDate();
            expect(techExeCtrl.ValidateError).toBe(false);

        });

    });
    
    describe("Search Technical Exception", function () {        

        it("Search Current User info and return current user information", function () {
            userInfo.getUser();
            httpLocalBackend.flush();

            expect(scope.hasResubmitAccess).toBe(userInfoData.CanManageTEQ);

        });

        it("Search Technical Exception based on default Criteria and should return Technical Exception data", function () {
            techExeCtrl.search();
            httpLocalBackend.flush();

            expect(techExeCtrl.ExceptionType).toBe('');
            expect(techExeCtrl.serviceGrid.data[0].DocumentObjectid).toBe(TEQData[0].DocumentObjectid);
            expect(techExeCtrl.serviceGrid.data[1].ServiceRequestId).toBe(TEQData[1].ServiceRequestId);

        });

        it("Search Technical Exception based on external reference number and should return Technical Exception data with the searched reference number", function () {
            techExeCtrl.searchbyReferenceNo();
            httpLocalBackend.flush();

            expect(techExeCtrl.ReferenceNo).toBe('');
            expect(techExeCtrl.serviceGrid.data[0].ExternalRefNum).toBe(TEQData[0].ExternalRefNum);
            expect(techExeCtrl.serviceGrid.data[1].ExternalRefNum).toBe(TEQData[1].ExternalRefNum);

        });

        ////it("Fetch Message Content based on document Id", function () {
        ////    httpLocalBackend.flush();

        ////    expect(techExeRowCtrl.entity.MessageContent).not.toBe(null);
        ////    expect(techExeRowCtrl.entity.MessageContent).toEqual(msgContent);

        ////});

        ////it("Fetch Exception Notes based on exception Id", function () {
        ////    httpLocalBackend.flush();

        ////    expect(techExeRowCtrl.entity.Comments.length).toBe(0);

        ////});

    });   


});