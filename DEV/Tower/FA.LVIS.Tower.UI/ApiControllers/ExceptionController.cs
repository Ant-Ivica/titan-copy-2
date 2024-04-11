using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.FASTProcessing;
using FA.LVIS.Tower.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.UI.ApiControllers.Filters; 

namespace FA.LVIS.Tower.UI.ApiControllers
{
    [ValidateAntiForgeryTokenFilter]
    [RoutePrefix("ExceptionController")]
    [CustomAuthorize]
    public class ExceptionController : ApiController
    {
        #region "BEQ"

        [Route("BEQUpdateReject/{ExternalRefnum:maxlength(50)}/{InternalRefNum:maxlength(50)}/{InternalRefId:int:min(1)}", Name = "BEQUpdateReject")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public bool BEQUpdateReject(string ExternalRefnum, string InternalRefNum, int InternalRefId,[FromBody] ExceptionDTO MatchException)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions\\Update and Reject";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

            if (isCanManageBEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().BEQUpdateReject(MatchException, ExternalRefnum, InternalRefNum, InternalRefId, userId, Tenantid);
            }

            return false;
        }

        //GetExceptionComments
        [Route("GetExceptionComments", Name = "GetExceptionComments")]
        [HttpPost]
        public List<string> GetExceptionComments([FromBody] ExceptionDTO MatchException)
        {
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

            if (isCanManageBEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().GetExceptionComments(MatchException);
            }

            return new List<string>();
        }

        // This function takes in a string and decodes special characters and returns the decoded string
        public string DecodeString(string str)
        {
            // Special chars that did NOT work and is now handled: & . # % * + \ : < > / ?
            str = Regex.Replace(str, "@@26", "&");
            str = Regex.Replace(str, "@@27", ".");
            str = Regex.Replace(str, "@@28", "#");
            str = Regex.Replace(str, "@@29", "%");
            str = Regex.Replace(str, "@@30", "*");
            str = Regex.Replace(str, "@@31", "+");
            str = Regex.Replace(str, "@@32", "\\");
            str = Regex.Replace(str, "@@33", ":");
            str = Regex.Replace(str, "@@34", "<");
            str = Regex.Replace(str, "@@35", ">");
            str = Regex.Replace(str, "@@36", "/");
            str = Regex.Replace(str, "@@37", "?");
            return str;
        }


        [Route("BEQCreateOrder/{FileNotes:maxlength(1000)}", Name = "BEQCreateOrder")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public bool BEQCreateOrder(string FileNotes ,[FromBody] ExceptionDTO MatchException)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions\\Create Order";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

            if (isCanManageBEQ)
            {
                // Decoder for special characters
                FileNotes = DecodeString(FileNotes);

                return ServiceFactory.Resolve<IExceptionService>().BEQCreateOrder(MatchException,userId, Tenantid, FileNotes);
            }

            return false;
        }

        [Route("BEQUpdate/{ExternalRefnum:maxlength(50)}/{InternalRefNum:maxlength(50)}/{InternalRefId:int:min(1)}", Name = "BEQUpdate")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public bool BEQUpdate(string ExternalRefnum,string InternalRefNum, int InternalRefId,[FromBody] ExceptionDTO MatchException)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions\\Update";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

            if (isCanManageBEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().BEQUpdate(MatchException,ExternalRefnum, InternalRefNum, InternalRefId, userId, Tenantid);
            }

            return false;
        }

        [Route("BEQUnBindOrder", Name = "BEQUnBindOrder")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public bool BEQUnBindOrder([FromBody]PotentialMatchDTO BindMatch)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions\\Unbind";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            var Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

            if (isCanManageBEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().BEQUnBindOrder(BindMatch, userId,Tenantid);
            }

            return false;
        }

        internal SearchData RefineMatch(SearchDetailDTO model)
        {
            try
            {
                AuditLogHelper.sSection = "Exception\\Business Exceptions";
                // populate order data for matches search
                SearchData orderData = new SearchData();

                // File Number
                if (model.FileNumber != null && !model.FileNumber.Trim().Equals(""))
                {
                    orderData.FileNumber = model.FileNumber;
                    orderData.FileNumberFuzzyDifferences = 2;
                }
                else
                    orderData.FileNumberFuzzyDifferences = -1;

                // Address
                if (model.Address != null && !model.Address.Trim().Equals(""))
                {
                    orderData.Property.Address1 = model.Address;
                    orderData.Property.Address1Differences = 2;
                }
                else
                    orderData.Property.Address1Differences = -1;

                // City
                if (model.City != null && !model.City.Trim().Equals(""))
                {
                    orderData.Property.City = model.City;
                    orderData.Property.CityDifferences = 2;
                }
                else
                    orderData.Property.CityDifferences = -1;

                // County
                if (model.County != null && !model.County.Trim().Equals(""))
                {
                    orderData.Property.County = model.County;
                    orderData.Property.CountyDifferences = 2;
                }
                else
                    orderData.Property.CountyDifferences = -1;

                // Lender
                if (model.LenderName != null && !model.LenderName.Trim().Equals(""))
                {
                    orderData.LenderInformation.Name = model.LenderName;
                    orderData.LenderInformation.LenderNameMatchCount = 5;
                }
                else
                    orderData.LenderInformation.LenderNameMatchCount = 0;

                // Buyer
                if (model.Buyer != null && !model.Buyer.Trim().Equals(""))
                {
                    FASTProcessing.Entity buyer = new FASTProcessing.Entity();

                    string[] buyerNames = model.Buyer.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (buyerNames.Length > 1)
                    {
                        buyer.BSType = FASTProcessing.BuyerSellerType.Individual;
                        buyer.FirstName = buyerNames[0];
                        buyer.LastName = buyerNames[1];
                    }
                    else
                    {
                        buyer.BSType = FASTProcessing.BuyerSellerType.Business_Entity;
                        buyer.LastName = buyerNames[0];
                    }

                    orderData.Borrowers.Add(buyer);

                    orderData.BuyerFuzzyDifferences = 2;
                }
                else
                    orderData.BuyerFuzzyDifferences = -1;

                // Non-fuzzy match values
                if (model.State == null || model.State.Equals(""))
                    throw new Exception("State missing and is a required field. Search cannot be performed");

                orderData.Property.State = model.State;
                return orderData;

            }
            catch (Exception)
            {
            }

            return null;

        }

        [Route("SearchFastDetails", Name = "SearchFastDetails")]
        [HttpPost]
        public PotentialMatchDTO[] SearchFastDetails([FromBody]SearchDetailDTO Details)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions\\Search";
            PotentialMatchDTO[] Match = null;
            
            RunAccount impAccount = new RunAccount();
            impAccount.ImpDomain = ConfigurationManager.AppSettings["FastServiceDomain"];
            impAccount.ImpAccount = ConfigurationManager.AppSettings["FastServiceUser"];
            impAccount.ImpPassword = ConfigurationManager.AppSettings["FastServicePassword"];
            impAccount.Tenantid = Details.TenantId;
            if(impAccount.Tenantid == 0)
            {
                var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

                impAccount.Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;


            }
            FilesMatched matchEnum = 0;
            EQFASTSearch searchClient = new EQFASTSearch(impAccount);
            if (Details != null)
            {
                if (Details.FastFileID != null && (!string.IsNullOrEmpty(Details.FastFileID)))
                {
                    matchEnum = searchClient.getFastFilesByIDs(new List<int>() { int.Parse(Details.FastFileID) });
                }
                else
                {
                    SearchData Data = RefineMatch(Details);
                    if (Data != null)
                    {
                        if (!string.IsNullOrEmpty(Data.FileNumber))
                        {
                            matchEnum = searchClient.searchFastFiles(Data);
                        }
                        else
                        {
                            matchEnum = searchClient.searchFilesWithSolrSearch(Data);
                        }
                    }
                }

                if (matchEnum == FilesMatched.SingleMatch || matchEnum == FilesMatched.MultipleMatches)
                {
                    Match = new PotentialMatchDTO[searchClient.OrderMatches.Count];

                    for (int i = 0; i < searchClient.OrderMatches.Count; i++)
                    {
                        this.PopulateOrderToModel(ref Match[i], searchClient.OrderMatches[i], true);
                    }
                }
            }
            
        searchClient = null;
        return Match;
        }

        [Route("RejectOrder", Name = "RejectOrder")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public bool RejectOrder([FromBody]ExceptionDTO MatchException)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions\\Reject";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

            if (isCanManageBEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().BEQRejectOrder(MatchException, userId, MatchException.RejectNotes);
            }

            return false;
        }

        [Route("TEQRejectOrder/{FileNotes:maxlength(1000)}", Name = "TEQRejectOrder")]
        [HttpPost]
        public bool TEQRejectOrder(string FileNotes, [FromBody]ExceptionDTO MatchException)
        {
            AuditLogHelper.sSection = "Exception\\Technical Exceptions\\Reject";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            bool isCanManageTEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault().Value) : false;

            if (isCanManageTEQ)
            {
                FileNotes = DecodeString(FileNotes);
                return ServiceFactory.Resolve<IExceptionService>().TEQRejectOrder(MatchException, userId, FileNotes);
            }

            return false;
        }

        [Route("BEQParseParent", Name = "BEQParseParent")]
        [HttpPost]
        public BEQParseXMLDTO BEQParseParent(ExceptionDTO Parent)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            BEQParseXMLDTO OrderData = ServiceFactory.Resolve<IExceptionService>().BEQParseParent(Parent);

            return OrderData;
        }

        [Route("BEQBindOrder/{FileNotes:maxlength(1000)}", Name = "BEQBindOrder")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public bool BEQBindOrder(string FileNotes, [FromBody]PotentialMatchDTO BindMatch)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions\\Bind";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

            if (isCanManageBEQ)
            {
                // Decoder for special characters
                FileNotes = DecodeString(FileNotes);

                return ServiceFactory.Resolve<IExceptionService>().BindMatch(BindMatch, userId, FileNotes);
            }

            return false;
        }

        [Route("BEQBindAllOrder/{FileNumber:maxlength(50)}/{FileID:maxlength(10)}/{FileNotes:maxlength(1000)}", Name = "BEQBindAllOrder")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public bool BEQBindAllOrder(string FileNumber, string FileID, string FileNotes, [FromBody]ExceptionDTO BindMatch)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

            if (isCanManageBEQ)
            {
                // Decoder for special characters
                FileNotes = DecodeString(FileNotes);

                return ServiceFactory.Resolve<IExceptionService>().BEQBindAllOrder(FileNumber, FileID, BindMatch, userId, FileNotes);
            }

            return false;
        }

        [Route("BEQParentXml/{Exceptionid}", Name = "BEQParentXml")]
        [HttpGet]
        public string BEQParentXml(int Exceptionid)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            return ServiceFactory.Resolve<IExceptionService>().BEQParentXml(Exceptionid);

        }

        [Route("BEQParse/{DocumentObjectid}", Name = "BEQParse")]
        [HttpGet]
        public BEQParseXMLDTO BEQParse(long DocumentObjectid)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            BEQParseXMLDTO OrderData = ServiceFactory.Resolve<IExceptionService>().BEQParse(DocumentObjectid);

            return OrderData;
        }

        [Route("BEQPotentialSourceMatches/{tenantid:int:min(1)}", Name = "BEQPotentialSourceMatches")]
        [HttpPost]
        public IEnumerable<PotentialMatchDTO> BEQPotentialSourceMatches(int tenantid ,[FromBody]List<int> FastFileIDs)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            PotentialMatchDTO[] Match = null;

            if (FastFileIDs != null && FastFileIDs.Count() > 0)
            {
                PotentialMatchDTO match = new PotentialMatchDTO();
                RunAccount impAccount = new RunAccount();
                impAccount.ImpDomain = ConfigurationManager.AppSettings["FastServiceDomain"];
                impAccount.ImpAccount = ConfigurationManager.AppSettings["FastServiceUser"];
                impAccount.ImpPassword = ConfigurationManager.AppSettings["FastServicePassword"];
                impAccount.Tenantid = tenantid;
                if (impAccount.Tenantid == 0)
                {
                    var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

                    impAccount.Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;


                }
                EQFASTSearch searchClient = new EQFASTSearch(impAccount);
                FilesMatched matchEnum = searchClient.getFastFilesByIDs(FastFileIDs);

                if (matchEnum == FilesMatched.SingleMatch || matchEnum == FilesMatched.MultipleMatches)
                {
                    Match = new PotentialMatchDTO[searchClient.OrderMatches.Count];

                    for (int i = 0; i < searchClient.OrderMatches.Count; i++)
                    {
                        this.PopulateOrderToModel(ref Match[i], searchClient.OrderMatches[i], true);
                    }
                }

                searchClient = null;
            }

            return Match;
        }

        private void PopulateOrderToModel(ref PotentialMatchDTO match, FASTProcessing.Order order, bool SourceprovidedMatch)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            match = new PotentialMatchDTO();
            match.FileID = order.FileID;
            match.BUID = order.EscrowOfficeBUID;
            match.RegionID = order.RegionID;
            match.FileNumber = order.FileNumber;
            match.PropertyAddress = (!order.PropertyAddress.AddressLine2.Equals("") ? order.PropertyAddress.AddressLine1 + " " + order.PropertyAddress.AddressLine2 : order.PropertyAddress.AddressLine1);
            match.City = order.PropertyAddress.City;
            match.StateName = order.PropertyAddress.State;
            match.County = order.PropertyAddress.County;
            match.MortgageBroker = order.MortgageBroker;
            if (order.Buyers != null && order.Buyers.Count > 0)
            {
                foreach (FASTProcessing.Entity ent in order.Buyers)
                {
                    match.Buyer += ent.FullName + "; ";
                    if (ent.BSType == FASTProcessing.BuyerSellerType.Husband_Wife)
                    {
                        match.Buyer += ent.SpouseFullName + "; ";
                    }
                }
                match.Buyer = match.Buyer.Remove(match.Buyer.Length - 2, 2);
            }
            if (order.Sellers != null && order.Sellers.Count > 0)
            {
                foreach (FASTProcessing.Entity ent in order.Sellers)
                {
                    match.Seller += ent.FullName + "; ";
                    if (ent.BSType == FASTProcessing.BuyerSellerType.Husband_Wife)
                    {
                        match.Seller += ent.SpouseFullName + "; ";
                    }
                }
                match.Seller = match.Seller.Remove(match.Seller.Length - 2, 2);
            }
            match.TransactionType = (order.TransactionType == null) ? "" : order.TransactionType.Replace("_", " ");
            match.ServiceType = order.ServiceType;
            match.NewLoanLender = order.LoanLenderName;
            match.OfficerName = order.OfficerName;
            match.FirstNewLoanAmount = (order.FirstNewLoanAmount.HasValue ? order.FirstNewLoanAmount.Value : 0);
            match.LoanNumber = order.LoanNumber;
            match.SalePrice = (order.SaleAmount.HasValue ? order.SaleAmount.Value : 0);
            match.OpenDate = order.OpenDate.HasValue ? order.OpenDate.Value : new DateTime();
            ICustomerService RegionList = ServiceFactory.Resolve<ICustomerService>();
            IEnumerable<DC.Regions> regionsList = RegionList.GetFastRegions(Constants.APPLICATION_FAST);
            match.RegionName = regionsList.Where(se => se.Id == order.RegionID).Count() > 0 ? regionsList.Where(se => se.Id == order.RegionID).First().Name : order.RegionID.ToString();
            match.OfficeName = order.OfficeName;
            match.SourceProvidedMatch = SourceprovidedMatch;
        }

        [Route("SearchFastFile/{Regionid:int:min(1)}/{FastFile:maxlength(20)}/{unbind:bool}/{tenantid:int:min(0)}", Name = "SearchFastFile")]
        [HttpGet]
        public PotentialMatchDTO SearchFastFile(int Regionid, string FastFile, bool unbind, int tenantid)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions\\Search";
            PotentialMatchDTO match = new PotentialMatchDTO();
            RunAccount impAccount = new RunAccount();
            impAccount.ImpDomain = ConfigurationManager.AppSettings["FastServiceDomain"];
            impAccount.ImpAccount = ConfigurationManager.AppSettings["FastServiceUser"];
            impAccount.ImpPassword = ConfigurationManager.AppSettings["FastServicePassword"];
            impAccount.Tenantid = tenantid;
            if (impAccount.Tenantid == 0)
            {
                var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

                impAccount.Tenantid = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;


            }
            EQFASTSearch searchClient = new EQFASTSearch(impAccount);
            Order OrderData = searchClient.getFastFile(Regionid, FastFile);

            if (OrderData == null || OrderData.FileID == null)
            {
                return null;
            }

            if (unbind && OrderData.SecondSourceApplicationID != 38) // not an LVIS order
                throw new LVISCustom("The fast file " + FastFile + " in Region " + Regionid + " does not qualify for Unbind as this functionality can be performed on a fast file having second order source as LVIS only. Please check the file number and try again.");


            if (OrderData != null)
            {
                this.PopulateOrderToModel(ref match, OrderData, false);
                return match;
            }

            searchClient = null;
            return null;
        }

        [Route("GetBEQExceptions", Name = "GetBEQExceptions")]
        [HttpPost]
        public IEnumerable<ExceptionDTO> GetBEQExceptions(SearchDetail value)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IExceptionService>().GetBEQExceptions(value, tenantId);
        }

        [Route("GetBEQExceptionsbyType/{ExceptionType:maxlength(50)}", Name = "GetBEQExceptionsbyType")]
        [HttpPost]        
        public IEnumerable<ExceptionDTO> GetBEQExceptionsbyType(string ExceptionType ,[FromBody]SearchDetail value)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IExceptionService>().GetBEQExceptionsbyType(value, tenantId, ExceptionType);
        }

        [Route("GetBEQExceptionsbyFilter/{sFilter:maxlength(3)}/{Typecodestatus:bool}", Name = "GetBEQExceptionsbyFilter")]
        [HttpGet]
        public IEnumerable<ExceptionDTO> GetBEQExceptionsbyFilter(string sFilter, bool Typecodestatus)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IExceptionService>().GetBEQExceptions(sFilter, tenantId, Typecodestatus);
        }

        [Route("GetBEQExceptionsbyFilterType/{sFilter:maxlength(3)}/{Typecodestatus:bool}/{ExceptionType:maxlength(50)}", Name = "GetBEQExceptionsbyFilterType")]
        [HttpGet]
        public IEnumerable<ExceptionDTO> GetBEQExceptionsbyFilterType(string sFilter, bool Typecodestatus, string ExceptionType)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IExceptionService>().GetBEQExceptionsbyTypeName(sFilter, tenantId, Typecodestatus,  ExceptionType);
        }

        [Route("SaveBEQExceptionComments", Name = "SaveBEQExceptionComments")]
        [HttpPost]
        public ExceptionDTO SaveBEQExceptionComments(ExceptionDTO Exceptiondet)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            IExceptionService ExcepionList = ServiceFactory.Resolve<IExceptionService>();

            return ExcepionList.SaveBEQExceptionComments(Exceptiondet, userId);
        }

        [Route("GetBEQExceptionByReferenceNum", Name = "GetBEQExceptionByReferenceNum")]
        [HttpPost]
        public IEnumerable<ExceptionDTO> GetBEQExceptionByReferenceNum(SearchDetail value)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IExceptionService>().GetBEQExceptionByReferenceNum(value, tenantId);
        }

        [Route("DeleteBEQOrder/{FileNotes:maxlength(1000)}", Name = "DeleteBEQOrder")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public bool DeleteBEQOrder(string FileNotes, [FromBody]ExceptionDTO MatchException)
        {
            AuditLogHelper.sSection = "Exception\\Business Exceptions";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;
            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;
            if (isCanManageBEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().BEQDeleteOrderdetails(MatchException, userId, FileNotes);
            }

            return false;
        }

        [Route("ResubmitBEQException", Name = "ResubmitBEQException")]
        [HttpPost]
        [CustomAuthorize("SuperAdmin", "Admin")]
        public ExceptionDTO ResubmitBEQException(ExceptionDTO ExceptionDetails)
        {
            AuditLogHelper.sSection = "Exceptions\\Business Exception Queue";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            bool isCanManageBEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGEBEQ).FirstOrDefault().Value) : false;

            if (isCanManageBEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().BEQResubmitException(ExceptionDetails, userId);
            }
            else
            { return null; }
        }

        [Route("SentMail", Name = "SentMail")]
        [HttpPost]
        public bool SentMail(EmailDetail EmailDetail)
        {
            return ServiceFactory.Resolve<IExceptionService>().SendMail(EmailDetail.EmailSubject, EmailDetail.EmailId, EmailDetail.EmailBody);
        }

        #endregion "BEQ"

        #region "TEQ"

        [Route("GetTEQExceptions", Name = "GetTEQExceptions")]
        [HttpPost]
        public IEnumerable<ExceptionDTO> Get(SearchDetail value)
        {
            AuditLogHelper.sSection = "Exception\\GetTEQExceptions";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IExceptionService>().GetTEQExceptions(value, tenantId);
        }

        [Route("GetTEQExceptionsbyFilter/{sFilter:maxlength(3)}/{Typecodestatus:bool}", Name = "GetTEQExceptionsbyFilter")]
        [HttpGet]
        public IEnumerable<ExceptionDTO> GetTEQExceptionsbyFilter(string sFilter, bool Typecodestatus)
        {
            AuditLogHelper.sSection = "Exception\\GetTEQExceptionsbyFilter";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IExceptionService>().GetTEQExceptions(sFilter, tenantId, Typecodestatus);
        }

        [Route("GetMessageDetails/{iExceptionid}", Name = "GetExceptionMessageDetails")]
        [HttpGet]
        public IEnumerable<MessageLogDetailDTO> GetMessageDetails(int exceptionId)
        {
            AuditLogHelper.sSection = "Exception\\GetMessageDetails";
            return ServiceFactory.Resolve<IExceptionService>().GetMessageDetails(exceptionId);
        }

        [Route("GetExceptionStatus", Name = "GetExceptionStatus")]
        [HttpGet]
        public IEnumerable<ExceptionStatus> GetExceptionStatus()
        {
            AuditLogHelper.sSection = "Exception\\GetExceptionStatus";
            return ServiceFactory.Resolve<IExceptionService>().GetExceptionStatus();
        }
     
        [Route("ResubmitException", Name = "ResubmitException")]
        [HttpPost]
        public ExceptionDTO ResubmitException(ExceptionDTO ExceptionDetails)
        {
           
            AuditLogHelper.sSection = "Exceptions\\Technical Exception Queue";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            bool isCanManageTEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault().Value) : false;

            if (isCanManageTEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().ResubmitException(ExceptionDetails, userId);
            }
            else
            { return null; }
        }

        //Start-Bulk Resubmit
        [Route("BulkResubmitException", Name = "BulkResubmitException")]
        [HttpPost]
        public Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>>  BulkResubmitException([FromBody] List<DC.ExceptionDTO> value)
        {
            AuditLogHelper.sSection = "Exceptions\\Technical Exception Queue";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;
            bool isCanManageTEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault().Value) : false;

            if (isCanManageTEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().BulkResubmitException(value, userId);
            }
            else
            { return null; }

        }
        //End-Bulk Resubmit

        [Route("GetTEQExceptionByReferenceNum", Name = "GetTEQExceptionByReferenceNum")]
        [HttpPost]
        public IEnumerable<ExceptionDTO> GetTEQExceptionByReferenceNum(SearchDetail value)
        {
            AuditLogHelper.sSection = "Exceptions\\GetTEQExceptionByReferenceNum";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IExceptionService>().GetTEQExceptionByReferenceNum(value, tenantId);
        }

        [Route("SaveExceptionComments", Name = "SaveExceptionComments")]
        [HttpPost]
        public ExceptionDTO SaveExceptionComments(ExceptionDTO Exceptiondet)
        {
            AuditLogHelper.sSection = "Exceptions\\SaveExceptionComments";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;

            AuditLogHelper.sSection = "Exceptions\\Technical Exception Queue";
            IExceptionService ExcepionList = ServiceFactory.Resolve<IExceptionService>();

            return ExcepionList.SaveExceptionComments(Exceptiondet, userId);
        }

        [Route("GetMessageContent/{DocumentObjectid}", Name = "GetMessageContent")]
        [HttpGet]
        public String GetMessageContent(long DocumentObjectid)
        {
            AuditLogHelper.sSection = "Exceptions\\GetMessageContent";
            return ServiceFactory.Resolve<IExceptionService>().GetMessageContent(DocumentObjectid);
        }

        [Route("GetExceptionNotes/{iExceptionid}", Name = "GetExceptionNotes")]
        [HttpGet]
        public List<string> GetExceptionNotes(int iExceptionid)
        {
            AuditLogHelper.sSection = "Exception\\GetMessageDetails";
            return ServiceFactory.Resolve<IExceptionService>().GetExceptionNotes(iExceptionid);
        }


        [Route("GetTEQExceptionsbyType/{ExceptionType:maxlength(50)}/{sFilter:maxlength(3)}", Name = "GetTEQExceptionsbyType")]
        [HttpPost]
        public IEnumerable<ExceptionDTO> GetTEQExceptionsbyType(string ExceptionType, string sFilter, [FromBody]SearchDetail value)
        {
            AuditLogHelper.sSection = "Exception\\GetTEQExceptionsbyType";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();

            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;

            return ServiceFactory.Resolve<IExceptionService>().GetTEQExceptionsbyTypeName(tenantId, value, ExceptionType, sFilter,value.Typecodestatus);
        }
        [Route("BulkResolveException/{status:maxlength(10)}", Name = "BulkResolveException")]
        [HttpPost]
        public Tuple<ResubmitBulkExceptionDTO, List<ExceptionDTO>> BulkResolveException(string status,[FromBody] List<DC.ExceptionDTO> value)
        {
            AuditLogHelper.sSection = "Exceptions\\Technical Exception Queue";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var userId = (claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault() != null) ?
             Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.USER_ID).FirstOrDefault().Value) : 0;
            bool isCanManageTEQ = (claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault() != null) ?
             Convert.ToBoolean(claims.Where(c => c.Type == DC.Constants.CLAIM_MANAGETEQ).FirstOrDefault().Value) : false;

            if (isCanManageTEQ)
            {
                return ServiceFactory.Resolve<IExceptionService>().BulkResolveException(status,value, userId);
            }
            else
            {
                return null;
            }

        }
        [Route("GetTEQExceptionsbyCondition/{exceptionType:maxlength(50)}/{status:maxlength(20)}/{messagetype:maxlength(100)}", Name = "GetTEQExceptionsbyCondition")]
        [HttpPost]
        public IEnumerable<ExceptionDTO> GetTEQExceptionsbyCondition(string exceptionType,string status,string messagetype,[FromBody]SearchDetail value)
        {
            AuditLogHelper.sSection = "Exception\\GetTEQExceptionsbyCondition";
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
            Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            return ServiceFactory.Resolve<IExceptionService>().GetTEQExceptionsbyCondition(exceptionType,status,messagetype,tenantId,value);
        }

        [Route("GetExceptionList", Name = "GetExceptionList")]
        [HttpGet]
        public IEnumerable<DC.ExceptionType> GetExceptionList()
        {
            AuditLogHelper.sSection = "Exception\\GetExceptionTypeList";
            return ServiceFactory.Resolve<IExceptionService>().GetExceptionList();
        }
        [Route("GetStatusList", Name = "GetStatusList")]
        [HttpGet]
        public IEnumerable<ExceptionStatus> GetStatusList()
        {
            AuditLogHelper.sSection = "Exception\\GetStatusList";
            AuditLogHelper.sSection = "Exception\\GetStatusList";
            List<ExceptionStatus> exceptionStatus = new List<ExceptionStatus>() {
                new ExceptionStatus(){
                    ID = 201,
                    Name = "New"
                },
                new ExceptionStatus()
                {
                    ID = 202,
                    Name = "Active"
                },
                new ExceptionStatus()
                {
                    ID = 203,
                    Name = "Hold"
                },
                new ExceptionStatus()
                {
                    ID = 204,
                    Name = "Resolved"
                },
                new ExceptionStatus()
                {
                    ID = 205,
                    Name = "Resubmitted"
                }
            };
            return exceptionStatus;
        }
        [HttpGet]
        [Route("GetMessageTypeList", Name = "GetMessageTypeList")]
        public IEnumerable<DC.MessageType> GetMessageTypeList()
        {
            AuditLogHelper.sSection = "Mappings\\FASTTaskMap\\GetMessageTypeList";
            IFastTaskMappingService MessageTypemap = ServiceFactory.Resolve<IFastTaskMappingService>();
            List<DC.MessageType> newList = new List<DC.MessageType>();
            var claims = SecurityExtensions.GetOwinContext(Request).Authentication.User.Claims.ToList();
            var tenantId = (claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault() != null) ?
                Convert.ToInt32(claims.Where(c => c.Type == DC.Constants.TENANT_ID).FirstOrDefault().Value) : 0;
            newList = MessageTypemap.GetMessageType();
            return newList;
        }
        #endregion "TEQ"
    }
}
