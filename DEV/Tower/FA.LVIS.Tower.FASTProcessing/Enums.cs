using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.FASTProcessing
{
	public enum FilesMatched
	{
		NoMatch = 0,
		SingleMatch = 1,
		MultipleMatches = 2,
		TooManyFilesFound = 3
	}

	internal enum FileNotes_TypeCDID
	{
		ACCT = 694,		//Accounting
		ENGINEER = 960,	//Engineering
		EPIC = 695,		//EPIC
		EO = 22,		//Escrow
		OTHER = 735,	//Other
		TENNOTE = 1209,	//Search Results
		TO = 21,		//Title
		UO = 275		//UCC
	}

	public enum BuyerSellerType
	{
		Individual = 48,
		Husband_Wife = 49,
		Trust_Estate = 50,
		Business_Entity = 51
	}

	public enum EntityTypeID
	{
		Attorney = 86,
		AttorneyAgent = 1343,
		ConstructionCompanyBuilder = 87,
		Developer = 1344,
		ExchangeCompany = 1307,
		HomeWarranty = 96,
		HomeownerAssociation = 93,
		InspectionRepair = 84,
		InsuranceCompany = 85,
		InternalCustomer = 1830,
		Lease = 98,
		Lender = 84,
		LoanOfficer = 1840,
		Miscellaneous = 101,
		Mortgage_Broker = 670,
		Notary = 1841,
		OutsideEscrowCompany = 90,
		OutsideTitleCompany = 91,
		RealEstateAgent = 89,
		RealEstateBroker = 88,
		RealEstateInvestmentTrust = 261,
		SearchVendor = 1679,
		Survey = 97,
		TaxCollector = 99,
		TitleAgent = 253,
		Trustee = 92,
		UCCLender = 273,
		UtilityCompany = 95
	}

	public enum ServiceType
	{
		TO = 0,
		EO = 1,
		SEO = 2,
		UO = 3	//UCC
	}

	public enum ProductID
	{
		ABSTRACT = 884,
		AGENFLSCAN = 1254,
		AGEBPOTYPI = 1255,
		AGENPOSTCL = 1256,
		ALTACL1992 = 866,
		EAGLELENDR = 886,
		ALTAELLEND = 868,
		ALTAELOWNR = 869,
		ALTAEXLN06 = 1317,
		ALTAEXOW06 = 1318,
		ALTAHPAGT = 2189,
		EAGLEOWNER = 887,
		ALTAEXPPO = 1257,
		PLLANGOWNR = 877,
		SHFORMCOMM = 888,
		SHFEGLLNPY = 903,
		SHFRRESLP = 1711,
		ASFRESLPEX = 1828,
		ALTALHLOAN = 875,
		ALTALHOWNR = 876,
		ALTASTLN06 = 1320,
		ALTASMART = 1838,
		ALTASTOW06 = 1321,
		ALTAUP1991 = 881,
		AMFLPFC = 1674,
		AFPHBP = 1678,
		AMFPHP = 1676,
		AFPNHP = 1677,
		AMFSFLPF = 1675,
		ATTOASSIST = 1259,
		BACKPLNREQ = 1260,
		CLTLANDCT = 900,
		CLTATG1992 = 906,
		CLTAFSLGAV = 907,
		CLTAFINSLG = 908,
		CLTAINTBIN = 1261,
		CLTAJTAXLG = 909,
		CLTADATEDW = 1262,
		CLTALG1992 = 910,
		CLTALTBK12 = 911,
		CLTAMECHLG = 912,
		CLTAPARCEL = 1263,
		CLTAPGFM17 = 913,
		CLTAROGFM = 914,
		CLTARG1990 = 915,
		ALTASCLNPO = 882,
		ALTASTOWNR = 883,
		CLTASG1975 = 916,
		COMBOGTE = 1552,
		CTEAGLEOWN = 885,
		COMMIT = 1147,
		CMTENDRSE = 928,
		CONSTLNCON = 1264,
		CONTFORGTE = 917,
		COPIESONLY = 1265,
		ESRCH = 1008,
		ENDRSEMT = 929,
		FACT = 1148,
		FORGURCOM = 1647,
		FORECLREP = 891,
		FUTFINBIND = 918,
		HOATRSALGT = 1582,
		INTERNLNPO = 1266,
		INTERNOWPO = 1267,
		JUDFORECLS = 1550,
		LEGALVEST = 919,
		LTRREPORT = 893,
		LTDLBLYRPT = 920,
		LTDPRFORCL = 894,
		LIMTTRUSTT = 1268,
		LTGNGTE = 921,
		LOTBKGTE = 922,
		LOTBOOKREP = 1269,
		EAMAFLOP = 1654,
		EAMAFOWP = 1653,
		MEASR37REP = 1270,
		MISCREPORT = 895,
		MortGua = 1746,
		MortPriGua = 1697,
		MORTGPOLY = 901,
		NOPOLYISSD = 930,
		OWNPOLYLND = 902,
		OEREPORT = 896,
		PRLIM = 1149,
		PREJUDREP = 1271,
		PRIMELNPOL = 897,
		PRIVEXAM = 1272,
		PROFORMA = 898,
		PROPREPORT = 923,
		PROPSRCH = 1273,
		REOWNLCER = 1734,
		RECDOCGTE = 924,
		RPIR = 710,
		RPIRGI = 1150,
		SHDISPOL = 1733,
		SHFMCOMT = 1152,
		SIGNATURE = 1274,
		STANDOW92 = 1275,
		STRMLINTSG = 1577,
		SUBDVNGTE = 925,
		SE = 1136,
		SUEAGLE24 = 1151,
		TAXFORECLS = 1549,
		TXXHNTP = 1783,
		TELMPRFOR = 1735,
		TXMTGEBCL = 1304,
		TXMTGEPCY = 1302,
		TXNFCFE = 1305,
		TXOWNPCY = 1300,
		TXROWNPCY = 1301,
		TXSFMGEPCY = 1303,
		TITLEEXAM = 1276,
		TITLEGTY = 926,
		TITLEOPN = 927,
		TRACTSRCH = 1277,
		TRSTGUAR = 931,
		TXCPLNLET = 1799,
		UCCANNRPT = 1286,
		UCCARTORG = 1287,
		UCCBKRPSER = 1288,
		UCCORPCERT = 1289,
		UCCJUDGSER = 1290,
		UCCLITISER = 1291,
		UCCPNDCIVL = 1292,
		UCCTAXLIEN = 1293,
		UCCAFILLNG = 1294,
		UCCASRCH = 1295,
		UCCBUYPOL = 1296,
		UCCINSPOL = 1297,
		UCCIFILNG = 1298,
		UCCISRCH = 1299,
		UCCPOL = 932,
		WATERLNPOL = 1322,
		WATEROWPOL = 1323,
		WATERSEARC = 1324,
		WTRTCONVY = 1153,
		WYFORLP = 1651
	}

	public enum TransactionTypeID
	{
		EnergyImprovement = 1867,
		HELOC = 1869,
		LimitedCashRefi = 1866,
		Purchase = 1868,
		Reverse = 1870
	}

	public enum ExceptionTypes
	{
		No_Good_Match = 0,
		Mismatch_Office_ID = 1,
		Mismatch_Lender = 3,
		Duplicate_Order_Source = 4,
		Piggyback_Order = 5,
		Unhandled_Transaction_Type = 6
	}

}
