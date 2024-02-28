using System;
using System.Xml;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Configuration;
using FA.LVIS.Tower.DataContracts;
using System.Net.Http;
using Constants = FA.LVIS.Tower.DataContracts.Constants;
using Newtonsoft.Json;
using FA.LVIS.CommonHelper;

namespace FA.LVIS.Tower.FASTProcessing
{
    public class EQFASTSearch
    {
        #region Members

        private RunAccount ImpAccount = null;

        private string FastAdminURL = string.Empty;
        private string FastUtilityURL = string.Empty;

        private List<Order> allMatches = new List<Order>();

        /// <summary>
        /// Returns the list of Orders (detail FAST order info) that match with the 'search criteria'
        /// </summary>
        public List<Order> OrderMatches
        {
            get
            {
                return allMatches;
            }
        }

        #endregion

        public EQFASTSearch(RunAccount account = null)
        {
            ImpAccount = account;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Tls12;

        }

        #region PUBLIC METHODS

        private readonly string s_AccessUserDomain = ConfigurationManager.AppSettings["FastAccessDomain"].Decrypt();
        private readonly string s_AccessUser = ConfigurationManager.AppSettings["FastAccessUser"].Decrypt();
       
        private FastFile.FastFileServiceClient GetFASTFileService()
        {
            string s_AccessPwd = ConfigurationManager.AppSettings["FastAccessPwd"].Decrypt();

          FastFile.FastFileServiceClient ffService = null;

            if (ConfigurationManager.AppSettings["RLAFastFileServiceEndpoint"] != null)
            {
                string[] ss = ConfigurationManager.AppSettings["RLAFastFileServiceEndpoint"].Split(',');
                if (ss.Length == 2)
                {

                    if (ImpAccount.Tenantid == int.Parse(ss[0].Trim()))
                    {
                        ffService = new FastFile.FastFileServiceClient(ss[1]);

                    }
                }
            }

            if (ffService == null)
            {
                if (ConfigurationManager.AppSettings["AltFastFileServiceEndpoint"] != null)
                {
                    string[] ss = ConfigurationManager.AppSettings["AltFastFileServiceEndpoint"].Split(',');
                    if (ss.Length == 2)
                    {

                        if (ImpAccount.Tenantid == int.Parse(ss[0].Trim()))
                        {
                            ffService = new FastFile.FastFileServiceClient(ss[1]);

                        }
                        else
                            ffService = new FastFile.FastFileServiceClient("basicHttpWSSecurityFS");


                    }
                    else
                        ffService = new FastFile.FastFileServiceClient("basicHttpWSSecurityFS");
                }
                else

                    ffService = new FastFile.FastFileServiceClient("basicHttpWSSecurityFS");
            }

            ffService.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(s_AccessUser, s_AccessPwd, s_AccessUserDomain);
            ffService.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
            if (ConfigurationManager.AppSettings["IsBasic"] == "1")
                ffService.Endpoint.EndpointBehaviors.Add(new CustomBehavior());

            return ffService;
        }


        private FastAdmin.FastAdminServiceClient GetFASTAdminService()
        {
            string s_AccessPwd = ConfigurationManager.AppSettings["FastAccessPwd"].Decrypt();

            FastAdmin.FastAdminServiceClient fAdmService = null;
            if (ConfigurationManager.AppSettings["RLAAdminServiceAccess"] != null)
            {
                string[] ss = ConfigurationManager.AppSettings["RLAAdminServiceAccess"].Split(',');
                if (ss.Length == 2)
                {
                    if (ImpAccount.Tenantid == int.Parse(ss[0].Trim()))
                    {
                        fAdmService = new FastAdmin.FastAdminServiceClient(ss[1]);

                    }
                }
            }
            if (fAdmService == null)
            {
                if (ConfigurationManager.AppSettings["AltFastAdminServiceEndpoint"] != null)
                {
                    string[] ss = ConfigurationManager.AppSettings["AltFastAdminServiceEndpoint"].Split(',');
                    if (ss.Length == 2)
                    {
                        if (ImpAccount.Tenantid == int.Parse(ss[0].Trim()))
                        {
                            fAdmService = new FastAdmin.FastAdminServiceClient(ss[1]);

                        }
                        else
                            fAdmService = new FastAdmin.FastAdminServiceClient("basicHttpWSSecurityAS");
                    }
                    else
                        fAdmService = new FastAdmin.FastAdminServiceClient("basicHttpWSSecurityAS");
                }
                else
                    fAdmService = new FastAdmin.FastAdminServiceClient("basicHttpWSSecurityAS");

            }

            fAdmService.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(s_AccessUser, s_AccessPwd, s_AccessUserDomain);
            fAdmService.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
            if (ConfigurationManager.AppSettings["IsBasic"] == "1")
                fAdmService.Endpoint.EndpointBehaviors.Add(new CustomBehavior());

            return fAdmService;
        }



        private FastEscrow.FastEscrowServiceClient GetFastEscrowService()
        {
            string s_AccessPwd = ConfigurationManager.AppSettings["FastAccessPwd"].Decrypt();

            FastEscrow.FastEscrowServiceClient fEscrowService = null;
            fEscrowService = new FastEscrow.FastEscrowServiceClient("basicHttpWSSecurityES");


            fEscrowService.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(s_AccessUser, s_AccessPwd, s_AccessUserDomain);
            fEscrowService.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Identification;
            if (ConfigurationManager.AppSettings["IsBasic"] == "1")
                fEscrowService.Endpoint.EndpointBehaviors.Add(new CustomBehavior());

            return fEscrowService;
        }

        internal Order mapFastFileDetails(int? regionID, FastFile.OrderDetailsResponse oDetail, int? escrowOfficeID = null)
        {
            Order Order = new Order();
            Order.RegionID = regionID;
            Order.FileNumber = oDetail.FileNumber;
            Order.FileID = oDetail.FileID;
            Order.OpenDate = oDetail.OpenDate;
            Order.OrderDate = oDetail.OrderDate;
            Order.FirstNewLoanAmount = oDetail.FirstNewLoanAmount;
            Order.ExternalFileNum = oDetail.ExternalFileNumber;
            Order.SaleAmount = oDetail.SalesPrice;
            Order.MortgageBroker = oDetail.FileBusinessParties?.FirstOrDefault(x =>  x.RoleTypeObjectCD == "MortgBrkr" )?.Name??string.Empty;
            Order.SecondSourceApplicationID = oDetail.SecondSourceApplicationID;
            if (oDetail.Services != null && oDetail.Services.Count >= 1)
            {
                foreach (FastFile.Service orderService in oDetail.Services)
                {
                    if (orderService.ServiceTypeObjectCD == "EO")
                    {
                        Order.EscrowOfficeBUID = orderService.OfficeInfo.BUID.Value;
                    }
                    if (orderService.ServiceTypeObjectCD == "TO")
                    {
                        Order.TitleOfficeBUID = orderService.OfficeInfo.BUID.Value;
                    }
                    Order.OfficeName = orderService.OfficeInfo.Name;
                }
            }
            if (escrowOfficeID.HasValue && Order.EscrowOfficeBUID.HasValue && Order.EscrowOfficeBUID.Value != escrowOfficeID.Value)
            {
                Order.ExceptionType = ExceptionTypes.Mismatch_Office_ID;
            }
            if (oDetail.NewLoan != null && oDetail.NewLoan.Count >= 1)
            {
                Order.LoanNumber = oDetail.NewLoan[0].LoanNumber;

                if (oDetail.NewLoan[0].FileBusinessParty != null)
                    Order.LoanLenderName = oDetail.NewLoan[0].FileBusinessParty.Name;
            }
            if (oDetail.Services != null && oDetail.Services.Count >= 1)
            {
                foreach (FastFile.Service s in oDetail.Services)
                {
                    if (Order.ServiceType == "")
                        Order.ServiceType = s.ServiceType;
                    else
                        Order.ServiceType = Order.ServiceType + " | " + s.ServiceType;
                    if (Order.OfficerName == "")
                        Order.OfficerName = s.OfficerName;
                    else
                        Order.OfficerName = Order.OfficerName + (s.OfficerName != "" ? " | " + s.OfficerName : "");
                }
            }
            if (oDetail.Sellers != null && oDetail.Sellers.Count >= 1)
            {
                foreach (FastFile.BuyerSeller bs in oDetail.Sellers)
                {
                    Entity e = new Entity();
                    e.FirstName = bs.FirstName != null ? bs.FirstName : "";
                    e.LastName = bs.LastName != null ? bs.LastName : "";
                    e.BSType = (BuyerSellerType)bs.BuyerSellerTypeID.Value;
                    e.SpouseFirstName = bs.SpouseFirstName != null ? bs.SpouseFirstName : "";
                    e.SpouseLastName = bs.SpouseLastName != null ? bs.SpouseLastName : "";

                    Order.Sellers.Add(e);
                }
            }
            if (oDetail.Buyers != null && oDetail.Buyers.Count >= 1)
            {
                foreach (FastFile.BuyerSeller bs in oDetail.Buyers)
                {
                    Entity e = new Entity();
                    e.FirstName = bs.FirstName != null ? bs.FirstName : "";
                    e.LastName = bs.LastName != null ? bs.LastName : "";
                    e.BSType = (BuyerSellerType)bs.BuyerSellerTypeID.Value;
                    e.SpouseFirstName = bs.SpouseFirstName != null ? bs.SpouseFirstName : "";
                    e.SpouseLastName = bs.SpouseLastName != null ? bs.SpouseLastName : "";

                    Order.Buyers.Add(e);
                }
            }
            if (oDetail.RealProperties != null && oDetail.RealProperties.Count >= 1)
            {
                if (oDetail.RealProperties[0].PropertyAddress != null && oDetail.RealProperties[0].PropertyAddress.Count >= 1)
                    Order.PropertyAddress = getAddress(oDetail.RealProperties[0].PropertyAddress[0]);
            }
            if (oDetail.TransactionTypeCdID.HasValue)
                Order.TransactionType = getTransactionType(oDetail.TransactionTypeCdID.Value);

            if (oDetail.BusinessSegmentCdID.HasValue)
                Order.BusinessSegment = getBusinessSegment(oDetail.BusinessSegmentCdID.Value);

            return Order;
        }

        public Order getFastFile(int? regionID, string fileNo)
        {
            if (!regionID.HasValue && String.IsNullOrWhiteSpace(fileNo))
                throw new Exception("Both RegionID and File Number must have a value.");
            FastFile.FastFileServiceClient ffService = GetFASTFileService();


            Order o = new Order();

            try
            {
                FastFile.FileSearchRequest fsreq = new FastFile.FileSearchRequest();
                fsreq.eFileStatus = FastFile.FileStatus.Open;

                fsreq.RegionID = regionID;
                fsreq.FileNumber = fileNo;
                fsreq.eFileType = FastFile.FileType.FileNo;

                FastFile.FileSearchResponse fsresp = ffService.GetFilesBySearch(new FastFile.GetFilesBySearchRequest(fsreq)).GetFilesBySearchResult;

                if (fsresp.FileSearchResults != null)
                {
                    if (fsresp.FileSearchResults.Count > 1)
                        throw new Exception("Weird... Multiple Files found from the given File Number " + fileNo);
                    else if (fsresp.FileSearchResults.Count == 0)
                        throw new Exception("No File found for the given File Number " + fileNo);
                    else
                    {
                        FastFile.FileSearchResult f = fsresp.FileSearchResults[0];

                        FastFile.OrderDetailsResponse oDetail = ffService.GetOrderDetails(new FastFile.GetOrderDetailsRequest(f.FileID.Value)).GetOrderDetailsResult;

                        o = mapFastFileDetails(regionID, oDetail);
                    }
                }

                ffService.Close();
            }
            catch (FaultException faultEx)
            {
                // Abort
                ffService.Abort();
                ffService.Close();

                // Rethrow.
                throw (faultEx.InnerException != null ? faultEx.InnerException : new Exception("FastFile Service Communication. " + faultEx.Message));
                //switch (faultEx.Code.Name)
                //{
                //	case "ConnectionFault":
                //		throw new Exception("FastFile Service Connection problem");
                //	case "DataReaderFault":
                //		throw new Exception("FastFile Service Data problem");
                //	default:
                //		throw new Exception("FastFile Service Communication unknown problem");
                //}
            }
            catch (Exception ex)
            {
                // Abort in the face of any other exception.
                ffService.Abort();
                ffService.Close();
                // Rethrow.
                throw ex;
            }
            //finally
            //{
            //    if (ImpAccount != null)
            //        Impersonator.EndImpersonate();
            //}

            return o;
        }

        /// <summary>
        /// Uses the Data entered to search, for a matching File in FAST, across all regions.
        /// </summary>
        /// <param name="cData">SearchData class hold required data to execute the Search in FAST</param>
        /// <returns>Enumeration: One, None or Multiple</returns>
        public FilesMatched searchFastFiles(SearchData cData)
        {
            bool searchByBorrower = false;
            FastFile.FastFileServiceClient ffService = GetFASTFileService();
            try
            {
                //cleanup in case same object is used to search again
                allMatches.Clear();

                FastFile.FileSearchRequest fsreq = new FastFile.FileSearchRequest();
                fsreq.eFileStatus = FastFile.FileStatus.Open;
                fsreq.PropertyState = cData.Property.State.ToUpper();

                if (cData.RegionID.HasValue)
                    fsreq.RegionID = cData.RegionID;

                fsreq.DateTo = DateTime.Now;
                fsreq.DateFrom = DateTime.Now.AddDays(cData.DateRange_Days * -1);

                //1st search - Property State + (File# or Seller Name)
                Dictionary<int, FastFile.FileSearchResult> gotFiles = new Dictionary<int, FastFile.FileSearchResult>();

                if (!String.IsNullOrWhiteSpace(cData.FileNumber))
                {
                    fsreq.FileNumber = cData.FileNumber;
                    fsreq.eFileType = FastFile.FileType.FileNo;

                    //Fast Search
                    FastFile.FileSearchResponse fsresp = ffService.GetFilesBySearch(new FastFile.GetFilesBySearchRequest(fsreq)).GetFilesBySearchResult;

                    if (fsresp.FileSearchResults != null && fsresp.FileSearchResults.Count > 0)
                    {
                        if (fsresp.FileSearchResults.Count >= 101)
                        {
                            ffService.Close();
                            return FilesMatched.TooManyFilesFound;
                        }

                        foreach (FastFile.FileSearchResult f in fsresp.FileSearchResults)
                        {
                            if (f.FileID.HasValue && !gotFiles.ContainsKey(f.FileID.Value))
                                gotFiles.Add(f.FileID.Value, f);
                        }
                    }
                }
                else if (cData.Sellers.Count > 0 || cData.Borrowers.Count > 0)
                {
                    fsreq.eFileType = FastFile.FileType.Any;

                    if (cData.Borrowers.Count > 0)
                    {
                        fsreq.ePrincipalSource = FastFile.PrincipalSource.Buyer;

                        //Multiple Search using all Borrowers' last name
                        foreach (Entity s in cData.Borrowers)
                        {
                            fsreq.PrincipalName = s.LastName;

                            //Fast Search
                            FastFile.FileSearchResponse fsresp = ffService.GetFilesBySearch(new FastFile.GetFilesBySearchRequest(fsreq)).GetFilesBySearchResult;

                            if (fsresp.FileSearchResults != null && fsresp.FileSearchResults.Count > 0)
                            {
                                if (fsresp.FileSearchResults.Count >= 101)
                                {
                                    ffService.Close();
                                    return FilesMatched.TooManyFilesFound;
                                }

                                foreach (FastFile.FileSearchResult f in fsresp.FileSearchResults)
                                {
                                    if (cData.FileNumberList.Count > 0)
                                    {
                                        //filter by given File IDs
                                        foreach (string fNo in cData.FileNumberList)
                                        {
                                            if ((cData.FileNumberFuzzyDifferences == 0 && fNo == f.FileNum) ||  //exact
                                                    (cData.FileNumberFuzzyDifferences > 0 && dif(fNo, f.FileNum) <= cData.FileNumberFuzzyDifferences))  //fuzzy
                                            {
                                                if (f.FileID.HasValue && !gotFiles.ContainsKey(f.FileID.Value))
                                                {
                                                    gotFiles.Add(f.FileID.Value, f);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (f.FileID.HasValue && !gotFiles.ContainsKey(f.FileID.Value))
                                        {
                                            gotFiles.Add(f.FileID.Value, f);
                                            searchByBorrower = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (gotFiles.Count == 0 && cData.Sellers.Count > 0)
                    {
                        fsreq.ePrincipalSource = FastFile.PrincipalSource.Seller;

                        //Multiple Search using all sellers' last name
                        foreach (Entity s in cData.Sellers)
                        {
                            fsreq.PrincipalName = s.LastName;

                            //Fast Search
                            FastFile.FileSearchResponse fsresp = ffService.GetFilesBySearch(new FastFile.GetFilesBySearchRequest(fsreq)).GetFilesBySearchResult;

                            if (fsresp.FileSearchResults != null && fsresp.FileSearchResults.Count > 0)
                            {
                                if (fsresp.FileSearchResults.Count >= 101)
                                {
                                    ffService.Close();
                                    return FilesMatched.TooManyFilesFound;
                                }

                                foreach (FastFile.FileSearchResult f in fsresp.FileSearchResults)
                                {
                                    if (cData.FileNumberList.Count > 0)
                                    {
                                        //filter by given File IDs
                                        foreach (string fNo in cData.FileNumberList)
                                        {
                                            if ((cData.FileNumberFuzzyDifferences == 0 && fNo == f.FileNum) ||  //exact
                                                    (cData.FileNumberFuzzyDifferences > 0 && dif(fNo, f.FileNum) <= cData.FileNumberFuzzyDifferences))  //fuzzy
                                            {
                                                if (f.FileID.HasValue && !gotFiles.ContainsKey(f.FileID.Value))
                                                {
                                                    gotFiles.Add(f.FileID.Value, f);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (f.FileID.HasValue && !gotFiles.ContainsKey(f.FileID.Value))
                                            gotFiles.Add(f.FileID.Value, f);
                                    }
                                }
                            }
                        }
                    }
                }

                //Begin internal filtering
                if (gotFiles.Count > 0)
                {
                    Dictionary<int, List<int>> tmpRegionFiles = new Dictionary<int, List<int>>();

                    //first filters
                    foreach (int k in gotFiles.Keys)
                    {
                        FastFile.FileSearchResult f = gotFiles[k];

                        if (f.OwningRegionID.HasValue && f.OwningRegionID != 12198) //First American Exchange region
                        {
                            List<int> rFileIDs = new List<int>();
                            if (tmpRegionFiles.ContainsKey(f.OwningRegionID.Value))
                                rFileIDs = tmpRegionFiles[f.OwningRegionID.Value];
                            else
                                tmpRegionFiles.Add(f.OwningRegionID.Value, rFileIDs);

                            rFileIDs.Add(f.FileID.Value);
                        }
                    }

                    #region second filters - Borrowers, Sellers, Property & Lender matching logic
                    foreach (int regionId in tmpRegionFiles.Keys)
                    {
                        foreach (int fileId in tmpRegionFiles[regionId])
                        {
                            FastFile.OrderDetailsResponse oDetail = ffService.GetOrderDetails(new FastFile.GetOrderDetailsRequest(fileId)).GetOrderDetailsResult;

                            bool candidate = true;

                            //Borrower/Buyer matching logic
                            if (candidate && oDetail.Buyers.Count > 0 && cData.Borrowers.Count > 0 && cData.BuyerFuzzyDifferences != -1) //empty borrower is a match
                            {
                                if (searchByBorrower)
                                    candidate = LastNameMatch(oDetail.Buyers, cData.Borrowers, cData.BuyerFuzzyDifferences); //strict
                                else
                                    candidate = nameMatch(oDetail.Buyers, cData.Borrowers, cData.BuyerFuzzyDifferences); //loose
                            }

                            //Seller Match Logic
                            if (oDetail.Sellers.Count > 0 && cData.Sellers.Count > 0 && cData.SellerFuzzyDifferences != -1)
                            {
                                if (cData.Borrowers.Count == 0)
                                    candidate = LastNameMatch(oDetail.Sellers, cData.Sellers, cData.SellerFuzzyDifferences); //strict
                                else
                                    candidate = nameMatch(oDetail.Sellers, cData.Sellers, cData.SellerFuzzyDifferences); //loose
                            }

                            //Property match logic
                            if (candidate &&
                                ((cData.Property.Address1.Length > 0 && cData.Property.Address1Differences != -1) ||
                                 (cData.Property.City.Length > 0 && cData.Property.CityDifferences != -1) ||
                                 (cData.Property.County.Length > 0 && cData.Property.CountyDifferences != -1)))
                            {
                                candidate = propertyMatch(cData.Property, oDetail.RealProperties);
                            }

                            //loan lender match logic
                            if (candidate &&
                                cData.LenderInformation != null &&
                                cData.LenderInformation.Name.Length > 0 &&
                                cData.LenderInformation.LenderNameMatchCount > 0)
                            {
                                candidate = lenderMatch(cData.LenderInformation, oDetail.NewLoan);
                            }

                            if (candidate)
                                allMatches.Add(mapFastFileDetails(regionId, oDetail));
                        }
                    }
                    #endregion
                }

                ffService.Close();
            }
            catch (FaultException faultEx)
            {
                // Abort
                ffService.Abort();
                ffService.Close();

                // Rethrow.
                throw faultEx;
                //switch (faultEx.Code.Name)
                //{
                //	case "ConnectionFault":
                //		throw new Exception("FastFile Service Connection problem");
                //	case "DataReaderFault":
                //		throw new Exception("FastFile Service Data problem");
                //	default:
                //		if (faultEx.InnerException != null)
                //			throw faultEx.InnerException;
                //		else
                //			throw new Exception("FastFile Service Communication unknown problem: " + faultEx.StackTrace);
                //}
            }
            catch (Exception ex)
            {
                // Abort in the face of any other exception.
                ffService.Abort();
                ffService.Close();
                // Rethrow.
                throw ex;
            }
            //finally
            //{
            //    if (ImpAccount != null)
            //        Impersonator.EndImpersonate();
            //}

            if (allMatches.Count == 1)
                return FilesMatched.SingleMatch;
            else if (allMatches.Count > 1)
                return FilesMatched.MultipleMatches;
            else
                return FilesMatched.NoMatch;
        }

        public FilesMatched searchFilesWithSolrSearch(SearchData cData)
        {
            FastFile.FastFileServiceClient ffService = GetFASTFileService();

            try
            {
                if (cData.Sellers.Count > 0 || cData.Borrowers.Count > 0)
                {
                    string FastEnv = ConfigurationManager.AppSettings["FastEnvironment"];
                    if (cData.Borrowers.Count > 0)
                    {
                        Tuple<int, string, string> borrower = new Tuple<int, string, string>(Convert.ToInt32(cData.Borrowers[0].BSType), cData.Borrowers[0].FirstName, cData.Borrowers[0].LastName);
                        var advanceSearch = CreateAdvanceSearch(borrower, cData.Property.Address1, cData.Property.State, cData.RegionID, FastEnv);
                        List<SolrFileDto> solrData = ExecuteAdvanceSearch(advanceSearch);
                        foreach (SolrFileDto files in solrData)
                        {
                            FastFile.OrderDetailsResponse oDetail = ffService.GetOrderDetails(new FastFile.GetOrderDetailsRequest(files.FileId)).GetOrderDetailsResult;
                            allMatches.Add(mapFastFileDetails(files.RegionId, oDetail));
                        }
                    }

                    if (allMatches.Count == 0 && cData.Sellers.Count > 0)
                    {
                        Tuple<int, string, string> borrower = new Tuple<int, string, string>(Convert.ToInt32(cData.Sellers[0].BSType), cData.Sellers[0].FirstName, cData.Borrowers[0].LastName);
                        var advanceSearch = CreateAdvanceSearch(borrower, cData.Property.Address1, cData.Property.State, cData.RegionID, FastEnv);
                        List<SolrFileDto> solrData = ExecuteAdvanceSearch(advanceSearch);
                        foreach (SolrFileDto files in solrData)
                        {
                            FastFile.OrderDetailsResponse oDetail = ffService.GetOrderDetails(new FastFile.GetOrderDetailsRequest(files.FileId)).GetOrderDetailsResult;
                            allMatches.Add(mapFastFileDetails(files.RegionId, oDetail));
                        }
                    }

                }
            }
            catch (FaultException faultEx)
            {
                ffService.Abort();
                ffService.Close();
                throw faultEx;
            }
            catch (Exception ex)
            {

                ffService.Abort();
                ffService.Close();
                throw ex;
            }

            if (allMatches.Count == 1)
                return FilesMatched.SingleMatch;
            else if (allMatches.Count > 1)
                return FilesMatched.MultipleMatches;
            else
                return FilesMatched.NoMatch;
        }
        /// <summary>
        /// Uses the Data entered to search, for a matching File in FAST, across all regions.
        /// </summary>
        /// <param name="cData">SearchData class hold required data to execute the Search in FAST</param>
        /// <returns>Enumeration: One, None or Multiple</returns>
        public FilesMatched getFastFilesByIDs(List<int> fileIDs)
        {
            //create object but dummy initialization
            FastFile.FastFileServiceClient ffService = GetFASTFileService();

            try
            {
                //cleanup in case same object is used to search again
                allMatches.Clear();

                foreach (int fileId in fileIDs)
                {
                    FastFile.OrderDetailsResponse oDetail = ffService.GetOrderDetails(new FastFile.GetOrderDetailsRequest(fileId)).GetOrderDetailsResult;

                    // get region from service
                    int? titleRegionID = 0;
                    int? closingRegionID = 0;
                    if (oDetail.Services != null)
                    {
                        foreach (FastFile.Service s in oDetail.Services)
                        {
                            if (s.ServiceTypeObjectCD == ServiceType.TO.ToString())
                                titleRegionID = s.OfficeInfo.RegionID;
                            else if (s.ServiceTypeObjectCD == ServiceType.EO.ToString())
                                closingRegionID = s.OfficeInfo.RegionID;
                        }
                    }

                    // - use closing region id if exist, otherwise title region id
                    allMatches.Add(mapFastFileDetails((closingRegionID != null && closingRegionID != 0 ? closingRegionID : titleRegionID), oDetail));
                }

                ffService.Close();
            }
            catch (FaultException faultEx)
            {
                // Abort
                ffService.Abort();
                ffService.Close();
                // Rethrow.
                throw faultEx;
            }
            catch (Exception ex)
            {
                // Abort in the face of any other exception.
                ffService.Abort();
                ffService.Close();
                // Rethrow.
                throw ex;
            }
            //finally
            //{
            //    if (ImpAccount != null)
            //        Impersonator.EndImpersonate();
            //}

            if (allMatches.Count == 1)
                return FilesMatched.SingleMatch;
            else if (allMatches.Count > 1)
                return FilesMatched.MultipleMatches;
            else
                return FilesMatched.NoMatch;
        }

        #endregion

        #region Seller - Buyer - Helper Methods to match names

        internal bool nameMatch(List<FastFile.BuyerSeller> fastBS, List<Entity> BuyersSellers, int differences)
        {
            foreach (FastFile.BuyerSeller fBS in fastBS)
            {
                foreach (Entity s in BuyersSellers) //RealEC will only send individuals
                {
                    //if (s.BSType == BuyerSellerType.Individual)
                    //{
                    if (//(!String.IsNullOrWhiteSpace(s.FirstName) &&		//both first/last name on both exist
                        //!String.IsNullOrWhiteSpace(s.LastName) &&
                        //!String.IsNullOrWhiteSpace(fBS.FirstName) &&
                        //!String.IsNullOrWhiteSpace(fBS.LastName) &&
                        //existMatch(s.FirstName, fBS.FirstName, differences) &&
                        //existMatch(s.LastName, fBS.LastName, differences))
                        //||
                       (//(String.IsNullOrWhiteSpace(s.FirstName) || String.IsNullOrWhiteSpace(fBS.FirstName)) &&		//any first is empty - match only last							
                        !String.IsNullOrWhiteSpace(s.LastName) &&
                        !String.IsNullOrWhiteSpace(fBS.LastName) &&
                        existMatch(s.LastName, fBS.LastName, differences)))
                    {
                        return true;
                    }
                    //}
                    if (fBS.EntityTypeID == (int)BuyerSellerType.Husband_Wife)
                    {
                        if (//(!String.IsNullOrWhiteSpace(s.FirstName) &&		//both first/last name on both
                            //!String.IsNullOrWhiteSpace(s.LastName) &&
                            //!String.IsNullOrWhiteSpace(fBS.SpouseFirstName) &&
                            //!String.IsNullOrWhiteSpace(fBS.SpouseLastName) &&
                            //existMatch(s.FirstName, fBS.SpouseFirstName, differences) &&
                            //existMatch(s.LastName, fBS.SpouseLastName, differences))
                            //||
                           (//(String.IsNullOrWhiteSpace(s.FirstName) || String.IsNullOrWhiteSpace(fBS.SpouseFirstName)) &&	//any first is empty - match only last	
                            !String.IsNullOrWhiteSpace(s.LastName) &&
                            !String.IsNullOrWhiteSpace(fBS.SpouseLastName) &&
                            existMatch(s.LastName, fBS.SpouseLastName, differences)))
                        {
                            return true;
                        }
                    }
                    if ((s.BSType == BuyerSellerType.Business_Entity || s.BSType == BuyerSellerType.Trust_Estate) &&
                        (fBS.EntityTypeID == (int)BuyerSellerType.Business_Entity || fBS.EntityTypeID == (int)BuyerSellerType.Trust_Estate) &&
                         existMatch(fBS.Name.ToUpper().Replace("  ", " ").Trim(), s.FullName, differences))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal bool existMatch(string itemA, string itemB, int differences)
        {
            if (differences == 0)
                return exactMatch(itemA, itemB);
            else if (dif(itemA, itemB) <= differences)
                return true;

            return false;
        }

        internal bool exactMatch(string a, string b)
        {
            if (a.ToUpper().Replace("  ", " ").Trim() == b.ToUpper().Replace("  ", " ").Trim())
                return true;
            else
                return false;
        }

        internal bool LastNameMatch(List<FastFile.BuyerSeller> fastBS, List<Entity> BuyersSellers, int differences)
        {
            //Strict Match - Last names only will be compared, uses a configurable diff #

            int partyCount = 0;
            foreach (FastFile.BuyerSeller fBS in fastBS)
            {
                foreach (Entity s in BuyersSellers)
                {
                    if (s.BSType == BuyerSellerType.Individual)
                    {
                        if (!String.IsNullOrWhiteSpace(s.LastName) && !String.IsNullOrWhiteSpace(fBS.LastName) && existMatch(s.LastName, fBS.LastName, differences))
                        {
                            partyCount++;
                        }
                    }
                    if (fBS.EntityTypeID == (int)BuyerSellerType.Husband_Wife)
                    {
                        if (!String.IsNullOrWhiteSpace(s.LastName) && !String.IsNullOrWhiteSpace(fBS.SpouseLastName) && existMatch(s.LastName, fBS.SpouseLastName, differences))
                        {
                            partyCount++;
                        }
                    }
                    if ((s.BSType == BuyerSellerType.Business_Entity || s.BSType == BuyerSellerType.Trust_Estate) &&
                        (fBS.BuyerSellerTypeID == (int)BuyerSellerType.Business_Entity || fBS.BuyerSellerTypeID == (int)BuyerSellerType.Trust_Estate) &&
                         existMatch(fBS.Name.ToUpper(), s.FullName, differences))
                    {
                        partyCount++;
                    }
                }
            }

            if (partyCount == BuyersSellers.Count) //every party on the RealEC order must be found on the Fast file.
                return true;

            return false;
        }
        private AdvanceSearch CreateAdvanceSearch(Tuple<int, string, string> buyer, string addressLine1, string propertyState, int? regionId, string solrEnv)
        {
            var advanceSearchFields = new List<AdvanceSeachField>();

            //buyer: <buyerType, firstname, lastname>
            //advanceSearchFields.Add(CreateAdvanceSearchField(buyer.Item1, buyer.Item2));
            advanceSearchFields.Add(CreateAdvanceSearchField(buyer.Item1, buyer.Item3));

            advanceSearchFields.RemoveAll(f => f == null);

            advanceSearchFields.Add(new AdvanceSeachField
            {
                SelectedItem = 1,
                TypeOfSearch = SearchFieldType.PropertyInformation
            });

            advanceSearchFields.Add(new AdvanceSeachField
            {
                TypeOfSearch = SearchFieldType.MutipleStatus,
                SearchQuery = "Open",
                SelectedItem = 1
            });

            var dateRange = $"[{DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd")}T00:00:00Z" +
                            $" TO {DateTime.Now.ToString("yyyy-MM-dd")}T00:00:00Z]";

            var propertyInfo = new DataContracts.PropertyInformation
            {
                Country = "USA",
                State = propertyState,
                PropertyAddressLine = KeyPart(addressLine1)
            };

            var advanceSearch = new AdvanceSearch()
            {
                NumberOfRows = 20,
                TestRegionFlag = 0,
                ExchangeFlag = 2,
                AdvanceSeachField = advanceSearchFields,
                PropertyInformation = propertyInfo,
                Dates = dateRange,
                dateType = DateType.Open_Date,
                Environment = solrEnv
            };

            if (regionId != null)
                advanceSearch.RegionID = regionId.Value;

            return advanceSearch;

        }
        private AdvanceSeachField CreateAdvanceSearchField(int buyerType, string buyerName)
                            => string.IsNullOrWhiteSpace(buyerName) ? null :
                                    new AdvanceSeachField
                                    {
                                        TypeOfSearch = SearchFieldType.Principle,
                                        SearchQuery = buyerName,
                                        //SelectedItem = MapToSolrBuyerType(buyerType)
                                    };
        private int MapToSolrBuyerType(int fastBuyerType)
        {
            IDictionary<int, int> FastSolrBuyerTypeMap = new Dictionary<int, int>
            {
                { 48, 7},  // individual buyer
                { 49, 10}, // HusbandWife_Buyer
                { 50, 9},  //TrustEstate_Buyer
                { 51, 8},  // BusinessEntity_Buyer
            };

            if (!FastSolrBuyerTypeMap.ContainsKey(fastBuyerType))
                throw new ArgumentOutOfRangeException($"invalid fast buyer type:{fastBuyerType}");

            return FastSolrBuyerTypeMap[fastBuyerType];
        }
        public string KeyPart(string addressLine1) => addressLine1.Any(x => Char.IsWhiteSpace(x))
                                    ? $"{addressLine1.Split(' ')[0]} {addressLine1.Split(' ')[1][0]}" : addressLine1;

        #endregion

        #region SolrSearch
        Tuple<DateTime, GenericTokenModel> TokenDetails;
        public List<SolrFileDto> ExecuteAdvanceSearch(AdvanceSearch advanceSearch)
        {
            var result = new List<SolrFileDto>();

            try
            {
                string solrSearchUrl = ConfigurationManager.AppSettings["SolrSearchUrl"];
                TokenDetails = GetAccessToken();
                Dictionary<string, dynamic> output = CallSolrInternal(advanceSearch, solrSearchUrl, TokenDetails.Item2.AccessToken).Result;
                SolrFileSearchResponse fileSearchResult = JsonConvert.DeserializeObject<SolrFileSearchResponse>(output["Output"]);
                result.AddRange(fileSearchResult?.Files);
            }
            catch (Exception e)
            {
                //Log exception
            }

            result.RemoveAll(f => f == null);

            return result;
        }

        private string isTruthyValue(object obj)
        {
            string returnVal = string.Empty;
            if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
            {
                string text = Convert.ToString(obj);
                returnVal = removeInvalidXmlChars(text);
                returnVal = returnVal.Trim();
            }
            return returnVal;
        }
        private string removeInvalidXmlChars(string inputXml)
        {
            char[] validXmlChars = inputXml.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        private async Task<Dictionary<string, dynamic>> CallSolrInternal(AdvanceSearch message, string URL, string auth)
        {
            HttpClient client = new HttpClient();
            Task<HttpResponseMessage> response = null;
            try
            {
                client.BaseAddress = new Uri(URL);
                client.Timeout = TimeSpan.FromMinutes(5);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + auth);
                response = client.PostAsync(URL, new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
                Dictionary<string, dynamic> output = new Dictionary<string, dynamic>();
                if (!response.Result.IsSuccessStatusCode)
                    throw new System.Exception("Error occurred while calling SolrSearch");

                output.Add("Status", response.Result.IsSuccessStatusCode);
                output.Add("Message", isTruthyValue(response.Result));
                output.Add("StatusCode", Convert.ToInt32(response.Result.StatusCode));
                if (response?.Result?.IsSuccessStatusCode == true)
                {
                    output["Output"] = response.Result.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    output["Message"] = isTruthyValue(response?.Result); //response.Result.Content.ReadAsStringAsync().Result;

                }
                await response;
                return output;

            }
            catch (Exception ex)
            {
                //log exception 
                throw ex;
            }
            finally
            {
                client.Dispose();
            }


        }
        public Tuple<DateTime, GenericTokenModel> GetAccessToken()
        {
            if (TokenDetails == null || (DateTime.Now - this.TokenDetails.Item1) > TimeSpan.FromSeconds(this.TokenDetails.Item2.ExpiresIn))
            {
                string clientId = ConfigurationManager.AppSettings["ida:ClientId"].Decrypt();
                string clientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"].Decrypt();
                string solrSearchToken = ConfigurationManager.AppSettings["IdaTokenUrl"];
                //string result = string.Empty;

                using (var client = new HttpClient())
                {
                    var body = new Dictionary<string, string>()
                    {
                        { "grant_type", "client_credentials" },
                        { "client_id", clientId },
                        { "client_secret", clientSecret }
                    };
                    HttpClient tokenClient = new HttpClient();
                    HttpResponseMessage httpResponse = tokenClient.PostAsync(solrSearchToken, new FormUrlEncodedContent(body)).Result;
                    string responseString = httpResponse.Content.ReadAsStringAsync().Result;
                    TokenDetails = new Tuple<DateTime, GenericTokenModel>(DateTime.Now, JsonConvert.DeserializeObject<GenericTokenModel>(responseString));
                }

            }

            return TokenDetails;
        }

        public class GenericTokenModel
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }
            [JsonProperty("expires_in")]
            public float ExpiresIn { get; set; }
            [JsonProperty("token_type")]
            public string TokenType { get; set; }

        }
        #endregion

        #region Property Match

        internal bool propertyMatch(PropertyInfo p, List<FastFile.Property> allProperties)
        {
            foreach (FastFile.Property py in allProperties)
            {
                //if (!String.IsNullOrWhiteSpace(py.Name.ToUpper()) && !String.IsNullOrWhiteSpace(p.Name) && dif(p.Name, py.Name) <= 3)
                //	return true;
                //if ((!String.IsNullOrWhiteSpace(py.Lot) && !String.IsNullOrWhiteSpace(p.Lot) && dif(p.Lot, py.Lot) <= 0) && 
                //	(!String.IsNullOrWhiteSpace(py.Parcel) && !String.IsNullOrWhiteSpace(p.Parcel) && dif(p.Parcel, py.Parcel) <= 0))
                //	return true;
                if (py.PropertyAddress != null && addressMatch(py.PropertyAddress[0], p))
                    return true;
            }

            return false;
        }

        internal bool addressMatch(FastFile.PhysicalAddress a, PropertyInfo p)
        {
            //if property search data address is empty, then no match
            if (String.IsNullOrWhiteSpace(p.Address1) && String.IsNullOrWhiteSpace(p.City))
                return false;

            bool CheckAddress = !String.IsNullOrWhiteSpace(p.Address1) && !String.IsNullOrWhiteSpace(a.AddrLine1) && p.Address1Differences != -1;
            bool CheckCity = !String.IsNullOrWhiteSpace(p.City) && !String.IsNullOrWhiteSpace(a.City) && p.CityDifferences != -1;
            bool CheckCounty = !String.IsNullOrWhiteSpace(p.County) && !String.IsNullOrWhiteSpace(a.County) && p.CountyDifferences != -1;

            //if all have values
            if (CheckAddress && CheckCity && CheckCounty)
            {
                bool AddressMatch = dif(a.AddrLine1, p.Address1) <= p.Address1Differences;
                bool CityMatch = dif(a.City, p.City) <= p.CityDifferences;
                bool CountyMatch = dif(a.County, p.County) <= p.CountyDifferences;

                if (AddressMatch && CityMatch && CountyMatch)
                    return true;
            }
            if (CheckAddress && CheckCity && !CheckCounty)
            {
                bool AddressMatch = dif(a.AddrLine1, p.Address1) <= p.Address1Differences;
                bool CityMatch = dif(a.City, p.City) <= p.CityDifferences;

                if (AddressMatch && CityMatch)
                    return true;
            }
            if (CheckAddress && !CheckCity && CheckCounty)
            {
                bool AddressMatch = dif(a.AddrLine1, p.Address1) <= p.Address1Differences;
                bool CountyMatch = dif(a.County, p.County) <= p.CountyDifferences;

                if (AddressMatch && CountyMatch)
                    return true;
            }
            if (CheckAddress && !CheckCity && !CheckCounty)
            {
                if (dif(a.AddrLine1, p.Address1) <= p.Address1Differences)
                    return true;
            }
            if (!CheckAddress && CheckCity && !CheckCounty)
            {
                if (dif(a.City, p.City) <= p.CityDifferences)
                    return true;
            }

            return false;
        }

        internal Address getAddress(FastFile.PhysicalAddress a)
        {
            Address pAddr = new Address();
            pAddr.AddressLine1 = a.AddrLine1;
            pAddr.AddressLine2 = a.AddrLine2;
            pAddr.City = a.City;
            pAddr.County = a.County;
            pAddr.State = a.State;

            return pAddr;
        }

        #endregion

        #region Lender Match

        internal bool lenderMatch(LenderInfo li, List<FastFile.NewLoan> Loans)
        {
            //if no loans in fast then any lender matches
            if (Loans == null || Loans.Count == 0)
                return true;

            li.Name = li.Name.ToUpper().Replace("  ", " ").Trim();

            //if no given leder name to search for, then any lender match
            if (li.Name == "")
                return true;

            foreach (FastFile.NewLoan nl in Loans)
            {
                //empty lender = match
                if (nl.FileBusinessParty.Name.Trim() == "")
                    return true;

                if (lenderStartsWith(nl.FileBusinessParty.Name, li.Name, li.LenderNameMatchCount))
                    return true;
            }

            return false;
        }

        internal bool lenderStartsWith(string FastLenderName, string clientLenderName, int count)
        {
            FastLenderName = FixStringforCompare(FastLenderName);
            clientLenderName = FixStringforCompare(clientLenderName);

            int chrFastLenderName = FastLenderName.Length < count ? FastLenderName.Length : count;
            int chrClientLenderName = clientLenderName.Length < count ? clientLenderName.Length : count;

            if (FastLenderName.Substring(0, chrFastLenderName) == clientLenderName.Substring(0, chrClientLenderName))
                return true;

            return false;
        }

        #endregion

        #region Business Helper Methods

        internal string getTransactionType(int ttype)
        {
            switch (ttype)
            {
                case 1:
                    return "Sale w/Mortgage";
                case 2:
                    return "Sale/Cash";
                case 3:
                    return "Refinance";
                case 4:
                    return "Equity Loan";
                case 5:
                    return "Bulk Sale";
                case 6:
                    return "Mobile Home";
                case 7:
                    return "Search Package";
                case 8:
                    return "Construction Loan";
                case 9:
                    return "Limited Escrow";
                case 10:
                    return "Sale/Exchange";
                case 251:
                    return "Foreclosure";
                case 836:
                    return "REO";
                case 837:
                    return "Accommodation";
                case 1145:
                    return "DFS";
                case 1244:
                    return "Settlement Statement Only";
                case 1245:
                    return "Construction Disbursement";
                case 1246:
                    return "Second Mortgage";
                case 1646:
                    return "Short Sale";
                case 1655:
                    return "Mortgage Modification";
                case 1656:
                    return "REO Sale w/Mortgage";
                case 1657:
                    return "REO Sale/Cash";
                case 1842:
                    return "Short Sale w/Mortgage";
                case 1843:
                    return "Short Sale/Cash";
            }

            return "";
        }

        internal string getBusinessSegment(int segId)
        {
            switch (segId)
            {
                case 838:
                    return "Commercial";
                case 839:
                    return "Residential";
                case 841:
                    return "New Home/Subdivision";
                case 842:
                    return "Time Share";
                case 1201:
                    return "Controlled-Residential";
                case 1202:
                    return "Controlled-Commercial";
                case 1850:
                    return "Default-Residential";
                case 1851:
                    return "Default-Commercial";
                case 1834:
                    return "New Home";
                case 1835:
                    return "Subdivision";
            }
            return "";
        }

        #endregion

        #region Helpers

        public List<Workflowprocesstaskevent> GetFastWorkFlowProcessTaskEvent(int TypeId)
        {


            FastAdmin.FastAdminServiceClient fAdmService = GetFASTAdminService();

            //create object but dummy initialization
            try
            {
                List<Workflowprocesstaskevent> list = new List<Workflowprocesstaskevent>();

                //cleanup in case same object is used to search again

                FastAdmin.GetTypeCodesRequest req = new FastAdmin.GetTypeCodesRequest() { classTypeID = TypeId };
                FastAdmin.GetTypeCodesResponse Respon = fAdmService.GetTypeCodes(req);
                List<Workflowprocesstaskevent> List = new List<Workflowprocesstaskevent>();

                if (Respon != null && Respon.GetTypeCodesResult != null)
                {
                    var det = Respon.GetTypeCodesResult.TypeCodes.Where(se => se.Description.StartsWith("LVIS"));
                    foreach (FastAdmin.TypeCodeInfo typedet in det)
                    {
                        list.Add(new Workflowprocesstaskevent() { Id = typedet.ID.Value, Description = typedet.Description + " (" + typedet.ID.Value + ")" });
                    }
                    fAdmService.Close();
                    return list.Distinct().ToList();
                }

            }
            catch (FaultException faultEx)
            {
                fAdmService.Close();

                // Rethrow.
                throw faultEx;
            }
            catch (Exception ex)
            {
                fAdmService.Close();
                // Rethrow.
                throw ex;
            }

            return null;
        }

        private Int32 dif(String a, String b)
        {
            a = FixStringforCompare(a);
            b = FixStringforCompare(b);

            if (string.IsNullOrEmpty(a))
            {
                if (!string.IsNullOrEmpty(b))
                {
                    return b.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(b))
            {
                if (!string.IsNullOrEmpty(a))
                {
                    return a.Length;
                }
                return 0;
            }

            Int32 cost;
            Int32[,] d = new int[a.Length + 1, b.Length + 1];
            Int32 min1;
            Int32 min2;
            Int32 min3;

            for (Int32 i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (Int32 i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (Int32 i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (Int32 j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = Convert.ToInt32(!(a[i - 1] == b[j - 1]));

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];

        }

        private string FixStringforCompare(string rawstring)
        {
            rawstring = rawstring.ToUpper().Replace("  ", " ").Trim();
            rawstring = Regex.Replace(rawstring, @"[^\w]", "", RegexOptions.None);
            return rawstring;
        }

        internal string getCorrectChars(string s, int tot, string wcard)
        {
            string onlyChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-";
            string tmp = "";
            for (int i = 0; i < tot; i++)
            {
                if (s.Length >= tot && onlyChars.Contains(s[i]))
                    tmp += s[i];
                else
                    break;
            }

            return tmp + wcard;
        }

        public List<OfficerData> GetEmployeesByFunctionTypes(int officeId, int funcType)
        {
            FastAdmin.FastAdminServiceClient fAdmService = GetFASTAdminService();

            List<OfficerData> list = new List<OfficerData>();

            try
            {
                FastAdmin.GetEmployeesByFunctionTypesRequest1 req = new FastAdmin.GetEmployeesByFunctionTypesRequest1();

                req.getEmployeesByFunctionTypesRequest = new FastAdmin.GetEmployeesByFunctionTypesRequest();
                req.getEmployeesByFunctionTypesRequest.OfficeID = officeId;
                req.getEmployeesByFunctionTypesRequest.StatusCD = 1;
                req.getEmployeesByFunctionTypesRequest.FunctionTypes = new List<int>();
                req.getEmployeesByFunctionTypesRequest.FunctionTypes.Add(funcType);

                FastAdmin.GetEmployeesByFunctionTypesResponse result = fAdmService.GetEmployeesByFunctionTypes(req).GetEmployeesByFunctionTypesResult;

                if (result != null)
                {
                    foreach (var emp in result.Employees)
                    {
                        OfficerData data = new OfficerData()
                        {
                            OfficerCode = emp.ObjectCd,
                            OfficerID = emp.EmployeeID.GetValueOrDefault(0),
                            OfficerName = emp.FirstName + " " + emp.LastName,
                            Email = emp.ElectronicAddress != null ? emp.ElectronicAddress.Where(se => !string.IsNullOrEmpty(se.EmailAddress)).SingleOrDefault().EmailAddress : string.Empty

                        };

                        list.Add(data);
                    }

                    fAdmService.Close();
                    return list;
                }
            }
            catch (FaultException faultEx)
            {
                fAdmService.Close();

                throw faultEx;
            }
            catch (Exception ex)
            {
                fAdmService.Close();
                throw ex;
            }

            return list;
        }

        public List<OfficerData> SearchEmplyeeByType(int officeId, int funcType, int regionid)
        {
            FastAdmin.FastAdminServiceClient fAdmService = GetFASTAdminService();

            List<OfficerData> list = new List<OfficerData>();

            try
            {
                FastAdmin.SearchEmployeeByTypeRequest req = new FastAdmin.SearchEmployeeByTypeRequest();

                req.getEmployeeRequest = new FastAdmin.EmployeeSearchRequest();
                req.getEmployeeRequest.FunctionTypeCdID = funcType;
                req.getEmployeeRequest.OfficeID = officeId;
                req.getEmployeeRequest.RegionID = regionid;
                FastAdmin.SearchEmployeeByTypeResponse result = fAdmService.SearchEmployeeByType(req);

                if (result != null)
                {
                    foreach (var emp in result.SearchEmployeeByTypeResult.SearchEmployees)
                    {
                        OfficerData data = new OfficerData()
                        {
                            OfficerCode = emp.IDCode,
                            OfficerID = emp.EmployeeID.GetValueOrDefault(0),
                            OfficerName = emp.FirstName + " " + emp.LastName,
                            Email = emp.EmployeeElectronicAddress != null ? emp.EmployeeElectronicAddress.EmailAddress : string.Empty

                        };

                        list.Add(data);
                    }

                    fAdmService.Close();
                    return list;
                }
            }
            catch (FaultException faultEx)
            {
                fAdmService.Close();

                throw faultEx;
            }
            catch (Exception ex)
            {
                fAdmService.Close();
                throw ex;
            }

            return list;
        }

        public FastEscrow.UpdateExternalServiceNumRequest GetUpdateExternaServiceNumRequest(int fileId, string serviceType, string externalServiceRefNum)
        {
            return new FastEscrow.UpdateExternalServiceNumRequest()
            {

                FileID = fileId,
                ExternalServiceNumber = externalServiceRefNum,
                Source = Constants.APPLICATION_LVIS,
                ServiceTypeObjectCD = serviceType,
                LoginName = Constants.APPLICATION_LVIS
            };
        }


        public FastEscrow.UpdateExternalServiceNumberRequest UpdateExternaServiceNumber(FastEscrow.UpdateExternalServiceNumRequest updtExternalServiceNumRequest)
        {
            return new FastEscrow.UpdateExternalServiceNumberRequest()
            {
                oExternalServiceNum = updtExternalServiceNumRequest,
            };
        }

        public int UpdateExternalServiceNumber(FastEscrow.UpdateExternalServiceNumRequest req)
        {

            int status = 0;
            FastEscrow.FastEscrowServiceClient fEscrowService = GetFastEscrowService();
            FastEscrow.UpdateExternalServiceNumberResponse resp = null;
            FastEscrow.UpdateExternalServiceNumberRequest requpdateReq = new FastEscrow.UpdateExternalServiceNumberRequest(req);
            fEscrowService = new FastEscrow.FastEscrowServiceClient("basicHttpWSSecurityES");
            // string[] ss = ConfigurationManager.AppSettings["AltFastEscrowServiceEndpoint"].Split(',');
            try
            {
                // fsServiceClient = new FastEscrow.FastEscrowServiceClient(ss[1]);
                resp = fEscrowService.UpdateExternalServiceNumber(requpdateReq);
                status = (int)resp.UpdateExternalServiceNumberResult.Status;
            }
            catch (Exception ex)
            {
                string err = "For FileId = " + req.FileID.ToString() + "Error From FAST" + ex.ToString();
                throw new Exception(err, ex);
            }
            return status;
        }

        public FastEscrow.UpdateExternalFileNumberRequest GetExternalServiveNumRequest(int fileId, string externalFileNum, string secondExternalFileNum)
        {
            return new FastEscrow.UpdateExternalFileNumberRequest()
            {
                ExternalFileNum = externalFileNum,
                FileID = fileId,
                Source = Constants.APPLICATION_LVIS,
                LoginName = Constants.APPLICATION_LVIS,
            };
        }

        public int updateExternalFileNumber(FastEscrow.UpdateExternalFileNumberRequest req)
        {

            int status = 0;
            FastEscrow.FastEscrowServiceClient fEscrowService = GetFastEscrowService();
            FastEscrow.UpdateExternalFileNumberResponse1 resp = null;
            FastEscrow.UpdateExternalFileNumberRequest1 reqExternalFileNum = new FastEscrow.UpdateExternalFileNumberRequest1(req);
            fEscrowService = new FastEscrow.FastEscrowServiceClient("basicHttpWSSecurityES");
            try
            {
                resp = fEscrowService.UpdateExternalFileNumber(reqExternalFileNum);
                status = (int)resp.UpdateExternalFileNumberResult.Status;
            }
            catch (Exception ex)
            {
                string err = "For FileId = " + req.FileID.ToString() + "Error From FAST" + ex.ToString();
                throw new Exception(err, ex);
            }
            return status;
        }

        #endregion
    }
}
