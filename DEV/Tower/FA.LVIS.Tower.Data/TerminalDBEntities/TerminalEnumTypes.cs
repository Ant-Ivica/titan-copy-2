using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Data.TerminalDBEntities
{
    public enum ApplicationEnum
    {// it is critical that the Enum names match Constants (e.g. Constants.APPLICATION_REALEC)
        LVIS = 1,
        TesterApp = 2,
        LenderSimulator = 3,
        RealEC = 4,
        FAST = 5,
        ELS = 6,
        WinTrack = 7,
        CAPI = 8,
        SettlementServices = 9,
        Elite = 10,
        FastWeb = 11,
        Simplifile = 14,
        Keystone = 21,
        LendingQB = 22,
        ValuTrust = 23,
        Encompass = 15,
        LTX = 25,
        TitleVision = 26,
        TitleProcess = 27,
        TitlePort = 28,
        SigningProcess = 29,
        PropertyTaxProcess = 31,
        SafeEscrow = 33,
        OpenAPI = 34
    }

    public enum TenantIdEnum
    {
        Agency = 1,
        MortgageServices = 2,
        LVIS = 3,
        QA = 4,
        RF = 5,
        RLA = 7,
        AirTrafficControl = 8
    }

    public enum ExceptionTypeEnum
    {
        None = 0,
        EndPointServiceHandler = 3,
        FASTEnrichmentHandler = 4,
        FASTQueueHandler = 5,
        RealECEnrichmentHandler = 6,
        RealECQueueHandler = 7,
        ELSEnrichmentHandler = 8,
        ELSQueueHandler = 9,
        WinscapeEnrichmentHandler = 10,
        WinscapeQueueHandler = 11,
        ConvoyQueueHandler = 12,
        BizTalkQueueHandler = 13,
        EliteQueueHandler = 14,
        EliteEnrichmentHandler = 16,
        SettlementServiceHandler = 18,
        SettlementQueueHandler = 19,
        CalculatorQueueHandler = 21,
        SimplifileEnrichmentHandler = 22,
        SimplifileQueueHandler = 23,
        Unhandled = 24,
        KeystoneQueueHandler = 25,
        KeystoneEnrichmentHandler = 26,
        LendingQBEnrichmentHandler = 27,
        LendingQBQueueHandler = 28,
        ValuTrustQueueHandler = 29,
        ValuTrustEnrichmentHandler = 30,
        NoGoodMatch = 52,
        MismatchOfficeID = 53,
        MismatchLender = 54,
        DuplicateOrderSource = 55,
        PiggybackOrder = 56,
        UnhandledTransactionType = 57,
        UnboundOrder = 58,
        DuplicateServiceRequested = 59,
        MultipleMatchFound = 60,
        PotentialMatchFound = 61,
        NewServiceReceived = 62,
        UnhandledServiceType = 63,
        FastWebEnrichmentHandler = 17,
        FastWebQueueHandler = 15,
        UnhandledExceptionOccured = 64,
        FASTGABMapNotfound = 66,
        UpdatedUnboundOrder = 67,
        InvalidOrderData = 68,
        TitleAccessIssue = 69,
        EscrowAccessIssue = 70,
        LTXQueueHandler = 31,
        LTXEnrichmentHandler = 32,
        TEQUnhandledServiceType = 71,
        TitleProcessQueueHandler = 72,
        TitleProcessEnrichmentHandler = 73,
        TitleVisionEnrichmentHandler = 74,
        TitleVisionQueueHandler = 75,
        TitlePortEnrichmentHandler = 76,
        TitlePortqueueHandler = 77,
        SorterQueueHandler = 78,
        SigningProcessQueuehandler = 79,
        SigningProcessEnrichmentHandler = 80,
        PropertyTaxProcessQueuehandler = 81,
        PropertyTaxProcessEnrichmentHandler = 82,
        EventsQueueHandler = 83,
        SafeEscrowQueueHandler = 84,
        SafeEscrowEnrichementHandler = 85,
        OpenAPIEnrichementHandler = 86,
        RetryQueue = 87,
        OpenAPIQueueHandler = 88,
        LoanExceeds50000Threshold = 89,
        CheckIfOrderCreatedOrNot = 91,
        DomainNotConfigured = 110
    }

    public enum TypecodeEnum
    {
        MessageStatus=100,

    }
    public enum EndpointTypeCode
    {
         calculator = 	2,
         realec	=3,
         fast	=4,
         els =	5,
         settlementservices=	6,
         everbank=	7,
         fastweb=	8,
         simplifile=	9,
         keystone=	10,
         LendingQB	=11,
         ValuTrust=	12,
         LTX	=13,
         TitleVision=	14,
         TitleProcess=	15,
         TitlePort=	16,
         signingprocess	=17,
         propertytaxapiprocess=	18,
         clearsearch	=19,
         openapi=	20,
         safeescrow	=22,
         NA= 0
    }
}
