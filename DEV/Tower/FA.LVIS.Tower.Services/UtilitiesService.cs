using FA.LVIS.Tower.Data;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;

namespace FA.LVIS.Tower.Services
{
    public class UtilitiesService : Core.ServiceBase, IUtilitiesService
    {
        public bool ConfirmService(int servicerequestid, int userid)
        {
            IUtilitiesDataProvider UtilitiesDataProvider = DataProviderFactory.Resolve<IUtilitiesDataProvider>();
            return UtilitiesDataProvider.ConfirmService(servicerequestid, userid);
        }

        public ServiceRequestDTO GetServiceReqInfo(int servicerequestid)
        {
            IUtilitiesDataProvider UtilitiesDataProvider = DataProviderFactory.Resolve<IUtilitiesDataProvider>();
            return UtilitiesDataProvider.GetServiceReqInfo(servicerequestid);
        }

        public IEnumerable<ExceptionStatus> GetStatus()
        {
            IUtilitiesDataProvider UtilitiesDataProvider = DataProviderFactory.Resolve<IUtilitiesDataProvider>();
            return UtilitiesDataProvider.GetStatus();
        }

        public IEnumerable<ApplicationMappingDTO> GetApplications()
        {
            IUtilitiesDataProvider UtilitiesDataProvider = DataProviderFactory.Resolve<IUtilitiesDataProvider>();
            return UtilitiesDataProvider.GetApplications();
        }

        public bool AddCredentials(AddCredentialDTO addCredential, int userId)
        {
            IUtilitiesDataProvider UtilitiesDataProvider = DataProviderFactory.Resolve<IUtilitiesDataProvider>();
            return UtilitiesDataProvider.AddCredentials(addCredential.Application, addCredential.UserName, addCredential.Password, userId);
        }

        public int UpdateandAcceptExternalRefNum(int servicerequestid, string externalRefnum, string newexternalRefnum, int userId)
        {
            IUtilitiesDataProvider UtilitiesDataProvider = DataProviderFactory.Resolve<IUtilitiesDataProvider>();
            return UtilitiesDataProvider.UpdateandAcceptExternalRefNum(servicerequestid, externalRefnum, newexternalRefnum, userId);
        }

        public int UpdateExternalRefNum(int servicerequestid, string externalRefnum, string newexternalRefnum, int userId)
        {
            IUtilitiesDataProvider UtilitiesDataProvider = DataProviderFactory.Resolve<IUtilitiesDataProvider>();
            return UtilitiesDataProvider.UpdateExternalRefNum(servicerequestid, externalRefnum, newexternalRefnum, userId);
        }

        public int UpdateServiceRequestInfo(int servicerequestid, string externalRefnum, string internalRefnum, int internalRefid, string customerRefnum, int userId, int status, bool chkUniqueID, bool chkExternalRefNum)
        {
            IUtilitiesDataProvider UtilitiesDataProvider = DataProviderFactory.Resolve<IUtilitiesDataProvider>();
            return UtilitiesDataProvider.UpdateServiceRequestInfo(servicerequestid, externalRefnum, internalRefnum, internalRefid, customerRefnum, userId, status, chkUniqueID, chkExternalRefNum);
        }

        public FastAppDetailsDTO PopulateFastAppEnvironmentDetails()
        {
            var ObjFastAppDetails = new FastAppDetailsDTO();
                       
            //check for the server environment if the below two environment values dont match the functionality wouldn't work 
            var EnabledSettings = new List<string> { "FAF.QA", "FAF.STG" };
            var envIndicator = ConfigurationManager.AppSettings["tibco_env"];
            if (EnabledSettings.Contains(envIndicator))
            {
                try
                {
                    //this reads the tower config for fast environment details.
                    var client = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
                    var endpointAddressList = client.Endpoints.Cast<ChannelEndpointElement>().Select(endpoint => endpoint.Address).ToList();
                    var solrSearch = ConfigurationManager.AppSettings["FastEnvironment"];
                    var solrSearchEndpoint = ConfigurationManager.AppSettings["SolrSearchUrl"];

                    ObjFastAppDetails.EnableFastInfo = true;

                    if (!string.IsNullOrWhiteSpace(solrSearch))
                    {
                        ObjFastAppDetails.FastEnvDetails.Add(new FastAppInfoDTO("SolrSearchEnv", solrSearch, "Tower"));
                    }
                    if (!string.IsNullOrWhiteSpace(solrSearchEndpoint))
                    {
                        ObjFastAppDetails.FastEnvDetails.Add(new FastAppInfoDTO("SolrSearchEndpoint", solrSearchEndpoint, "Tower"));
                    }
                    if (endpointAddressList.Any())
                    {
                        ObjFastAppDetails.FastEnvDetails.AddRange(
                            endpointAddressList.Distinct().
                            Select(x => new FastAppInfoDTO("Fast Endpoints", x.AbsoluteUri, "Tower")));
                    }
              
                    //this reads the terminal windows service configs from their respective folders for FAST environment details
               
                    var WinSvcLst = new List<string> { "LVIS.Events", "LVIS.FAST", "LVIS.Enrichment.FAST" };
              
                    foreach (var winSvc in WinSvcLst)
                    {
                        var filePath = $@"c:\LVISWindowsServices\{winSvc}\LVIS.WindowsServices.exe.config";
                        var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
                        var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                        var clientDtls = configuration.GetSection("system.serviceModel/client") as ClientSection;
                        var AddressList = clientDtls.Endpoints.Cast<ChannelEndpointElement>().Select(endpoint => endpoint.Address).ToList();
                        if (AddressList.Any())
                        {
                            ObjFastAppDetails.FastEnvDetails.AddRange(
                                AddressList.Distinct()
                                .Select(x => new FastAppInfoDTO("Fast Endpoints", x.AbsoluteUri, winSvc)));
                        }

                        var trmlsolrSearch = "";
                        if ((configuration.GetSection("appSettings") as AppSettingsSection).Settings["FastEnvironment"] != null)
                        {
                            trmlsolrSearch = (configuration.GetSection("appSettings") as AppSettingsSection).Settings["FastEnvironment"].Value;
                        }
                        var trmlsolrSearchEndpoint = "";
                        if ((configuration.GetSection("appSettings") as AppSettingsSection).Settings["SolrSearchUrl"] != null)
                        {
                            trmlsolrSearchEndpoint=(configuration.GetSection("appSettings") as AppSettingsSection).Settings["SolrSearchUrl"].Value;
                        }
                        ObjFastAppDetails.EnableFastInfo = true;

                        if (!string.IsNullOrWhiteSpace(trmlsolrSearch))
                        {
                            ObjFastAppDetails.FastEnvDetails.Add(new FastAppInfoDTO("SolrSearchEnv", trmlsolrSearch, $"{winSvc}"));
                        }
                        if (!string.IsNullOrWhiteSpace(trmlsolrSearchEndpoint))
                        {
                            ObjFastAppDetails.FastEnvDetails.Add(new FastAppInfoDTO("SolrSearchEndpoint", trmlsolrSearchEndpoint, $"{winSvc}"));
                        }
                    }
                }
                catch(Exception ex)
                {
                    Logger.Error("Exception occured while retrieving EnvironmentInfo for display: ", ex);
                }
            }
            return ObjFastAppDetails;
        }
    }
}
