using System;
using System.Text;
using System.Collections.Generic;

namespace FA.LVIS.Tower.FASTProcessing
{
	public class RunAccount
	{
		public string ImpDomain = "";
		public string ImpAccount = "";
		public string ImpPassword = "";
        public int Tenantid;
	}

    public class Workflowprocesstaskevent
    {
        public int Id;

        public String Description;
        
    }

    public struct OfficeData
    {
        public int OfficeID;
        public string OfficeName;
        public int RegionID;
        public string RegionName;
    }

    public struct OfficerData
    {
        public int OfficerID;
        public string OfficerCode;
        public string OfficerName;
        public int RegionID;
        public string RegionName;
        public string StatusCD;
        public string Email;
    }

    public class SearchData
	{
		//constructor
		public SearchData()
		{
		}

		private int? _region = null;
		public int? RegionID
		{
			get
			{
				return _region;
			}
			set
			{
				_region = value;
			}
		}

		private string _recFileNo = "";
		public string FileNumber
		{
			get
			{
				return _recFileNo;
			}
			set
			{
				_recFileNo = value.Trim().ToUpper();
			}
		}

		private int _fndiff = 0;
		public int FileNumberFuzzyDifferences
		{
			get
			{
				return _fndiff;
			}
			set
			{
				_fndiff = value;
			}
		}

		private int? _eOffcieID = null;
		public int? EscrowOfficeBUID
		{
			get
			{
				return _eOffcieID;
			}
			set
			{
				_eOffcieID = value;
			}
		}

		private TransactionTypeID _ttype = TransactionTypeID.Purchase;
		public TransactionTypeID TransactionType
		{
			get
			{
				return _ttype;
			}
			set
			{
				_ttype = value;
			}
		}

		private List<Entity> _seller = new List<Entity>();
		public List<Entity> Sellers
		{
			get
			{
				return _seller;
			}
		}

		private List<Entity> _borrower = new List<Entity>();
		public List<Entity> Borrowers
		{
			get
			{
				return _borrower;
			}
		}

		private LenderInfo _lender = new LenderInfo();
		public LenderInfo LenderInformation
		{
			get
			{
				return _lender;
			}
		}

		private PropertyInfo _property = new PropertyInfo();
		public PropertyInfo Property
		{
			get
			{
				return _property;
			}
		}

		private int _sfdiff = 2;
		public int SellerFuzzyDifferences
		{
			get
			{
				return _sfdiff;
			}
			set
			{
				_sfdiff = value;
			}
		}

		private int _bfdiff = 2;
		public int BuyerFuzzyDifferences
		{
			get
			{
				return _bfdiff;
			}
			set
			{
				_bfdiff = value;
			}
		}

		private List<string> _fileNos = new List<string>();
		public List<string> FileNumberList
		{
			get
			{
				return _fileNos;
			}
			set
			{
				_fileNos = value;
			}
		}
		
		private int _days = 30;
		public int DateRange_Days
		{
			get
			{
				return _days;
			}
			set
			{
				_days = value;
			}
		}

	}

	public class PropertyInfo
	{
		private string _pname = "";
		public string Name
		{
			get
			{
				return _pname;
			}
			set
			{
				_pname = value.Trim().ToUpper();
			}
		}

		private string _plot = "";
		public string Lot
		{
			get
			{
				return _plot;
			}
			set
			{
				_plot = value.Trim().ToUpper();
			}
		}

		private string _pparcel = "";
		public string Parcel
		{
			get
			{
				return _pparcel;
			}
			set
			{
				_pparcel = value.Trim().ToUpper();
			}
		}

		private string _ptract = "";
		public string Tract
		{
			get
			{
				return _ptract;
			}
			set
			{
				_ptract = value.Trim().ToUpper();
			}
		}

		private string _psubdiv = "";
		public string Subdivision
		{
			get
			{
				return _psubdiv;
			}
			set
			{
				_psubdiv = value.Trim().ToUpper();
			}
		}

		private string _papn = "";
		public string APN
		{
			get
			{
				return _papn;
			}
			set
			{
				_papn = value.Trim().ToUpper();
			}
		}

		private string _paddress1 = "";
		public string Address1
		{
			get
			{
				return _paddress1;
			}
			set
			{
				_paddress1 = value.Trim().ToUpper();
			}
		}

		private string _paddress2 = "";
		public string Address2
		{
			get
			{
				return _paddress2;
			}
			set
			{
				_paddress2 = value.Trim().ToUpper();
			}
		}

		private string _city = "";
		public string City
		{
			get
			{
				return _city;
			}
			set
			{
				_city = value.Trim().ToUpper();
			}
		}

		private string _state = "";
		public string State
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value.Trim().ToUpper();
			}
		}

		private string _county = "";
		public string County
		{
			get
			{
				return _county;
			}
			set
			{
				_county = value.Trim().ToUpper();
			}
		}

		private string _country = "";
		public string Country
		{
			get
			{
				return _country;
			}
			set
			{
				_country = value.Trim().ToUpper();
			}
		}

		private string _zip = "";
		public string Zip
		{
			get
			{
				return _zip;
			}
			set
			{
				_zip = value.Trim().ToUpper();
			}
		}
		
		private int _a1diff = 3;
		public int Address1Differences
		{
			get
			{
				return _a1diff;
			}
			set
			{
				_a1diff = value;
			}
		}

		private int _citydiff = 2;
		public int CityDifferences
		{
			get
			{
				return _citydiff;
			}
			set
			{
				_citydiff = value;
			}
		}

		private int _ctydiff = 2;
		public int CountyDifferences
		{
			get
			{
				return _ctydiff;
			}
			set
			{
				_ctydiff = value;
			}
		}
	
	}

	public class Address
	{
		private string _a1 = "";
		public string AddressLine1
		{
			get
			{
				return _a1;
			}
			set
			{
				_a1 = value;
			}
		}

		private string _a2 = "";
		public string AddressLine2
		{
			get
			{
				return _a2;
			}
			set
			{
				_a2 = value;
			}
		}

		private string _city = "";
		public string City
		{
			get
			{
				return _city;
			}
			set
			{
				_city = value;
			}
		}

		private string _st = "";
		public string State
		{
			get
			{
				return _st;
			}
			set
			{
				_st = value;
			}
		}

		private string _county = "";
		public string County
		{
			get
			{
				return _county;
			}
			set
			{
				_county = value;
			}
		}

	}

	public class LenderInfo
	{
		private bool _upLender = false;
		public bool UpdateLender
		{
			get
			{
				return _upLender;
			}
			set
			{
				_upLender = value;
			}
		}
		
		private string _lenderID = "";
		public string LenderOrderID
		{
			get
			{
				return _lenderID;
			}
			set
			{
				_lenderID = value.Trim().ToUpper();
			}
		}

		private string _pname = "";
		public string Name
		{
			get
			{
				return _pname;
			}
			set
			{
				_pname = value.Trim();
			}
		}

		private int? _addrBookEntryID = null;
		public int? AddrBookEntryID
		{
			get
			{
				return _addrBookEntryID;
			}
			set
			{
				_addrBookEntryID = value;
			}
		}

		private int? _EmployeeID = 3; //Wintrack
		public int? EmployeeID
		{
			get
			{
				return _EmployeeID;
			}
			set
			{
				_EmployeeID = value;
			}
		}

		private string _source = "WINTRACK";
		public string Source
		{
			get
			{
				return _source;
			}
			set
			{
				_source = value;
			}
		}

		private string _loanLender = "";
		public string LoanLender
		{
			get
			{
				return _loanLender;
			}
			set
			{
				_loanLender = value.Trim().ToUpper();
			}
		}

		private string _loanNo = "";
		public string LoanNumber
		{
			get
			{
				return _loanNo;
			}
			set
			{
				_loanNo = value.Trim().ToUpper();
			}
		}

		private decimal _loanAmount = 0;
		public decimal LoanAmount
		{
			get
			{
				return _loanAmount;
			}
			set
			{
				_loanAmount = value;
			}
		}
		
		private int _lnfdiff = 0;
		public int LenderNameMatchCount
		{
			get
			{
				return _lnfdiff;
			}
			set
			{
				_lnfdiff = value;
			}
		}

	}

	public class TermsInfo
	{
		private bool _upTerm = false;
		public bool UpdateTerms
		{
			get
			{
				return _upTerm;
			}
			set
			{
				_upTerm = value;
			}
		}
		
		private decimal _saleAmount = 0;
		public decimal SaleAmount
		{
			get
			{
				return _saleAmount;
			}
			set
			{
				_saleAmount = value;
			}
		}

		private int? _EmployeeID = 3; //Wintrack
		public int? EmployeeID
		{
			get
			{
				return _EmployeeID;
			}
			set
			{
				_EmployeeID = value;
			}
		}

		private string _source = "WINTRACK";
		public string Source
		{
			get
			{
				return _source;
			}
			set
			{
				_source = value;
			}
		}

	}

	public class ServiceInfo
	{
		private bool _aservice = true;
		public bool AddService
		{
			get
			{
				return _aservice;
			}
			set
			{
				_aservice = value;
			}
		}

		private string _source = "WINTRACK";
		public string Source
		{
			get
			{
				return _source;
			}
			set
			{
				_source = value;
			}
		}

		private ServiceType _stype = ServiceType.TO;
		public ServiceType ServiceType
		{
			get
			{
				return _stype;
			}
			set
			{
				_stype = value;
			}
		}

		private int? _tOfficeBUID = null;
		public int? TitleOfficeBUID
		{
			get
			{
				return _tOfficeBUID;
			}
			set
			{
				_tOfficeBUID = value;
			}
		}

		private int? _officeBUID = null;
		public int? EscrowOfficeBUID
		{
			get
			{
				return _officeBUID;
			}
			set
			{
				_officeBUID = value;
			}
		}

		private string _ExternalServiceNum = "";
		public string ExternalServiceNum
		{
			get
			{
				return _ExternalServiceNum;
			}
			set
			{
				_ExternalServiceNum = value;
			}
		}
	}

	/// <summary>
	/// This object hold data for a Buyer/Seller
	/// </summary>
	public class Entity
	{
		private string _fname = "";
		private string _lname = "";
		private string _pSSN = "";
		private string _sfname = "";
		private string _slname = "";
		private string _sSSN = "";

		///<summary>
		///This is also Name for a Business Entity or Trust. (not required)
		///</summary>
		public string FirstName
		{
			get
			{
				return _fname;
			}
			set
			{
				_fname = value.Trim().ToUpper();
			}
		}

		///<summary>
		///This is also ShortName for a Business Entity or Trust. (required)*
		///</summary>
		public string LastName
		{
			get
			{
				return _lname;
			}
			set
			{
				_lname = value.Trim().ToUpper();
			}
		}

		public string SSN
		{
			get
			{
				return _pSSN;
			}
			set
			{
				_pSSN = value.Trim();
			}
		}

		public string FullName
		{
			get
			{
				if (_ptype == BuyerSellerType.Individual || _ptype == BuyerSellerType.Husband_Wife)
					return (FirstName + " " + LastName).Trim();
				else
					return LastName;
			}
		}

		public string SpouseFullName
		{
			get
			{
				if (_ptype == BuyerSellerType.Husband_Wife)
					return (SpouseFirstName + " " + SpouseLastName).Trim();
				else
					return "";
			}
		}

		///<summary>
		///Only used for Husband Wife
		///</summary>
		public string SpouseFirstName
		{
			get
			{
				return _sfname;
			}
			set
			{
				_sfname = value.Trim().ToUpper();
			}
		}

		///<summary>
		///Only used for Husband Wife (required)*
		///</summary>
		public string SpouseLastName
		{
			get
			{
				return _slname;
			}
			set
			{
				_slname = value.Trim().ToUpper();
			}
		}

		///<summary>
		///Only used for Husband Wife (required)*
		///</summary>
		public string SpouseSSN
		{
			get
			{
				return _sSSN;
			}
			set
			{
				_sSSN = value.Trim();
			}
		}

		private BuyerSellerType _ptype = BuyerSellerType.Individual;
		public BuyerSellerType BSType
		{
			set
			{
				_ptype = value;
			}
			get
			{
				return _ptype;
			}
		}

		///<summary>
		///Only setup if BuyerSellerType.Business_Entity
		///</summary>
		private EntityTypeID? _etype = null;
		public EntityTypeID? EntityType
		{
			set
			{
				_etype = value;
			}
			get
			{
				return _etype;
			}
		}
	}

	public class ClientData
	{
		//constructor
		public ClientData()
		{
		}

		private int? _region = null;
		public int? RegionID
		{
			get
			{
				return _region;
			}
			set
			{
				_region = value;
			}
		}

		private string _recFileNo = "";
		public string FileNumber
		{
			get
			{
				return _recFileNo;
			}
			set
			{
				_recFileNo = value.Trim().ToUpper();
			}
		}

		private string _oNo = "";
		public string WTOrderNumber
		{
			get
			{
				return _oNo;
			}
			set
			{
				_oNo = value;
			}
		}

		private List<Entity> _borrower = new List<Entity>();
		public List<Entity> Borrowers
		{
			get
			{
				return _borrower;
			}
		}

		private LenderInfo _lender = new LenderInfo();
		public LenderInfo LenderInformation
		{
			get
			{
				return _lender;
			}
		}

		private TermsInfo _terms = new TermsInfo();
		public TermsInfo Terms
		{
			get
			{
				return _terms;
			}
		}

		private ServiceInfo _servInfo = new ServiceInfo();
		public ServiceInfo Services
		{
			get
			{
				return _servInfo;
			}
		}

		public string Source
		{
			get
			{
				return "WINTRACK";
			}
		}

		public int SecondSourceApplID
		{
			get
			{
				return 10; //Wintrack
			}
		}

		//private bool _uProgType = false;
		//public bool UpdateProgramType
		//{
		//	get
		//	{
		//		return _uProgType;
		//	}
		//	set
		//	{
		//		_uProgType = value;
		//	}
		//}

		//private string _ptype = String.Empty; //no program type
		//public string ProgramType
		//{
		//	get
		//	{
		//		return _ptype;
		//	}
		//	set
		//	{
		//		_ptype = value;
		//	}
		//}

		private bool _aProduct = false;
		public bool AddProduct
		{
			get
			{
				return _aProduct;
			}
			set
			{
				_aProduct = value;
			}
		}

		private ProductID _productId;
		public ProductID ProductID
		{
			get
			{
				return _productId;
			}
			set
			{
				_productId = value;
			}
		}

		private string _note = "";
		public string Notez
		{
			get
			{
				return _note;
			}
			set
			{
				_note = value.Trim();
			}
		}

		private string _exFileNo = "";
		public string ExternalFileNumber
		{
			get
			{
				return _exFileNo;
			}
			set
			{
				_exFileNo = value.Trim();
			}
		}

	}

	/// <summary>
	/// This object holds data from the FAST file object
	/// </summary>
	public class Order
	{
		private int? _region = null;
		public int? RegionID
		{
			get
			{
				return _region;
			}
			set
			{
				_region = value;
			}
		}

		private string _recFileNo = "";
		public string FileNumber
		{
			get
			{
				return _recFileNo;
			}
			set
			{
				_recFileNo = value;
			}
		}

		private string _extFileNum = "";
		public string ExternalFileNum
		{
			get
			{
				return _extFileNum;
			}
			set
			{
				_extFileNum = value;
			}
		}

		private int? _recFileID = null;
		public int? FileID
		{
			get
			{
				return _recFileID;
			}
			set
			{
				_recFileID = value;
			}
		}

		private int? _eOffcieID = null;
		public int? EscrowOfficeBUID
		{
			get
			{
				return _eOffcieID;
			}
			set
			{
				_eOffcieID = value;
			}
		}

		private int? _tOffcieID = null;
		public int? TitleOfficeBUID
		{
			get
			{
				return _tOffcieID;
			}
			set
			{
				_tOffcieID = value;
			}
		}

        private string _officeName = null;
        public string OfficeName
        {
            get
            {
                return _officeName;
            }
            set
            {
                _officeName = value;
            }
        }

		private DateTime? _date = null;
		public DateTime? OrderDate
		{
			get
			{
				return _date;
			}
			set
			{
				_date = value;
			}
		}

		private DateTime? _opendate = null;
		public DateTime? OpenDate
		{
			get
			{
				return _opendate;
			}
			set
			{
				_opendate = value;
			}
		}
		
		private decimal? _saleAmount = null;
		public decimal? SaleAmount
		{
			get
			{
				return _saleAmount;
			}
			set
			{
				_saleAmount = value;
			}
		}

		private decimal? _loanAmount = null;
		public decimal? FirstNewLoanAmount
		{
			get
			{
				return _loanAmount;
			}
			set
			{
				_loanAmount = value;
			}
		}

		private string _loanno = "";
		public string LoanNumber
		{
			get
			{
				return _loanno;
			}
			set
			{
				_loanno = value;
			}
		}

		private string _lendername = "";
		public string LoanLenderName
		{
			get
			{
				return _lendername;
			}
			set
			{
				_lendername = value;
			}
		}

		private string _ttype = "";
		public string TransactionType
		{
			get
			{
				return _ttype;
			}
			set
			{
				_ttype = value;
			}
		}

		private string _bseg = "";
		public string BusinessSegment
		{
			get
			{
				return _bseg;
			}
			set
			{
				_bseg = value.Trim().ToUpper();
			}
		}

		private string _status = "";
		public string Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}

		private List<Entity> _seller = new List<Entity>();
		public List<Entity> Sellers
		{
			get
			{
				return _seller;
			}
		}

		private List<Entity> _borrower = new List<Entity>();
		public List<Entity> Buyers
		{
			get
			{
				return _borrower;
			}
		}

		private Address _paddress = new Address();
		public Address PropertyAddress
		{
			get
			{
				return _paddress;
			}
			set
			{
				_paddress = value;
			}
		}

		private string _servtype = "";
		public string ServiceType
		{
			get
			{
				return _servtype;
			}
			set
			{
				_servtype = value;
			}
		}

		private string _officename = "";
		public string OfficerName
		{
			get
			{
				return _officename;
			}
			set
			{
				_officename = value;
			}
		}

		private ExceptionTypes? _extype = null;
		public ExceptionTypes? ExceptionType
		{
			get
			{
				return _extype;
			}
			set
			{
				_extype = value;
			}
		}

		private string _eComment = "";
		public string ExceptionComment
		{
			get
			{
				return _eComment;
			}
			set
			{
				_eComment = value.Trim().ToUpper();
			}
		}


        private int? _secondSourceApplicationID = null;
        public int? SecondSourceApplicationID
        {
            get
            {
                return _secondSourceApplicationID;
            }
            set
            {
                _secondSourceApplicationID = value;
            }
        }

        private string _mortgageBroker = "";
        public string MortgageBroker
        {
            get
            {
                return _mortgageBroker;
            }
            set
            {
                _mortgageBroker = value;
            }
        }
    }

}
