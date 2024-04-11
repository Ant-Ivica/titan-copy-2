using FA.LVIS.Tower.DataContracts;
using LVIS.Common;
using LVIS.Infrastructure.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using FA.LVIS.Tower.Common;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;

namespace FA.LVIS.Tower.FastWebProcessing
{
    public class FastWebClientData
    {
        readonly Common.Logger sLogger = new Common.Logger();
        readonly Utils utils = new Utils();
        readonly private string userID = ConfigurationManager.AppSettings["FastWebUserID"];
        readonly private string superUserID = ConfigurationManager.AppSettings["FastWebSuperUserID"];
        readonly FastWebAdapter fastWebAdapter = new FastWebAdapter();
        readonly FastWebOrderDetails fastWebOrderDetails = new FastWebOrderDetails();
        public List<FastWebOrderDetailsCanonicalDTO> GetFastWebOrderDetails()
        {
            //Generating FW request
            var jsonFastNotifyRequest = GetFastWebNotifyRequest("10");
            //Sending FW request to FastWeb
            string response = fastWebAdapter.SendFastWebResponse(jsonFastNotifyRequest); 
            var fASTNotifyResponse = JsonConvert.DeserializeObject<FASTNotifyResponse>(response);
            List<FASTWEBOrder> fastweb_orders = fASTNotifyResponse?.FASTWEBORDERRESPONSE?.FASTNOTIFYRESPONSE?.FASTWEBORDERS?.FASTWEBOrder; //Get FastWeb order details
            return GetFastWebOrdersBySearchList(fastweb_orders);
        }

        public string GetFastWebNotifyRequest(string searchType, string searchText = null)
        {
            string userid = (searchType == "10") ? userID : superUserID;
            FASTNotifyRequest Orderrequest = new FASTNotifyRequest()
            {
                FASTWEB_HEADER = new FASTWEBHEADER()
                {
                    FWActionType = "FASTNotify"
                },
                FASTWEB_ORDER_REQUEST = new FASTWEB_ORDER_REQUEST()
                {
                    FASTNOTIFY_REQUEST = new FASTNOTIFY_REQUEST()
                    {
                        iProcessingRepID = userid,
                        SearchType = searchType,
                        SearchText = searchText,
                        ServiceTypes = new ServiceTypes()
                        {
                            ServiceType = new string[] { "1", "7" }
                        }
                    }
                }
            };

            return JsonConvert.SerializeObject(Orderrequest, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public FASTNotifyResponse pullFastWebOrderDetailsBySearchType(string SearchText, string SearchType)
        {
            string jsonrequest = GetFastWebNotifyRequest(SearchType, SearchText);
            string response = fastWebAdapter.SendFastWebResponse(jsonrequest);
            FASTNotifyResponse fastNotifyResp = JsonConvert.DeserializeObject<FASTNotifyResponse>(response);
            return fastNotifyResp;
        }

        public List<FastWebOrderDetailsCanonicalDTO> GetFastWebOrdersBySearchType(string SearchText, string SearchType)
        {
            FASTNotifyResponse fastwebResp = pullFastWebOrderDetailsBySearchType(SearchText, SearchType); //Get FastWeb order details

            return GetFastWebOrdersBySearchList(fastwebResp?.FASTWEBORDERRESPONSE?.FASTNOTIFYRESPONSE?.FASTWEBORDERS.FASTWEBOrder);
        }

        public List<FastWebOrderDetailsCanonicalDTO> GetFastWebOrdersBySearchList(List<FASTWEBOrder> fastweb_orders)
        {
            List<FastWebOrderDetailsCanonicalDTO> finalOrderDetails = new List<FastWebOrderDetailsCanonicalDTO>();
            
            if (fastweb_orders != null)
            {
                List<string> ServiceTypes = new List<string>();
                var fastWebOrderDetailsCanonicalDTOs = new List<FastWebOrderDetailsCanonicalDTO>();
                foreach (var fastweb_order in fastweb_orders)
                {
                    var fWOrderDetailsDTO = new FastWebOrderDetailsCanonicalDTO()
                    {
                        FASTWebOrderNumber = fastweb_order.FWOrderNumber.ToString(),
                        CustomerRefNumber = fastweb_order.CustomerReferenceNumber,
                        BorrowerName = fastweb_order.Borrowername,
                        PropertyAddressLine1 = fastweb_order.PropertyAddress,
                        PropertyAddressLine2 = fastweb_order.PropertyCity + ", " + fastweb_order.PropertyState + ", " + fastweb_order.PropertyZip,
                        PortalOrderAlert = fastweb_order.PortalOrderAlert,
                        OrderDate = (fastweb_order.OrderDate != null) ? Convert.ToDateTime(fastweb_order.OrderDate) : (DateTime?)null
                    };

                    if(!string.IsNullOrEmpty(fastweb_order.ServiceName))
                    {
                        if (fastweb_order.ServiceName.ToUpper().Contains("ESCROW"))
                            fWOrderDetailsDTO.ServiceName = "Escrow";
                        if (fastweb_order.ServiceName.ToUpper().Contains("TITLE"))
                            fWOrderDetailsDTO.ServiceName = "Title";
                    }

                    fastWebOrderDetailsCanonicalDTOs.Add(fWOrderDetailsDTO);
                }

                foreach (var fwOrder in fastWebOrderDetailsCanonicalDTOs.Select(x => x.FASTWebOrderNumber).Distinct())
                {
                    List<FastWebOrderDetailsCanonicalDTO> Details = fastWebOrderDetailsCanonicalDTOs.Where(se => se.FASTWebOrderNumber == fwOrder).OrderBy(s => s.OrderDate).ToList();
                    
                    FastWebOrderDetailsCanonicalDTO Parent = new FastWebOrderDetailsCanonicalDTO();
                    if (Details.Count == 1)
                    {
                        Parent = Details[0];
                        finalOrderDetails.Add(Parent);
                        continue;
                    }

                    Parent.children = new List<FastWebOrderDetailsCanonicalDTO>();
                    foreach (var child in Details.GroupBy(x=> new {x.ServiceName,x.FASTWebOrderNumber }).Select(x=>x.FirstOrDefault()).ToList())
                    {
                        
                        Parent.FASTWebOrderNumber = (string.IsNullOrEmpty(Parent.FASTWebOrderNumber) && !string.IsNullOrEmpty(child.FASTWebOrderNumber)) ? child.FASTWebOrderNumber : Parent.FASTWebOrderNumber;
                        Parent.CustomerRefNumber = (string.IsNullOrEmpty(Parent.CustomerRefNumber) && !string.IsNullOrEmpty(child.CustomerRefNumber)) ? child.CustomerRefNumber : Parent.CustomerRefNumber;
                        Parent.BorrowerName = (string.IsNullOrEmpty(Parent.BorrowerName) && !string.IsNullOrEmpty(child.BorrowerName)) ? child.BorrowerName : Parent.BorrowerName;
                        Parent.PropertyAddressLine1 = (string.IsNullOrEmpty(Parent.PropertyAddressLine1) && !string.IsNullOrEmpty(child.PropertyAddressLine1)) ? child.PropertyAddressLine1 : Parent.PropertyAddressLine1;
                        Parent.PropertyAddressLine2 = (string.IsNullOrEmpty(Parent.PropertyAddressLine2) && !string.IsNullOrEmpty(child.PropertyAddressLine2)) ? child.PropertyAddressLine2 : Parent.PropertyAddressLine2;
                        Parent.PortalOrderAlert = (string.IsNullOrEmpty(Parent.PortalOrderAlert) && !string.IsNullOrEmpty(child.PortalOrderAlert)) ? child.PortalOrderAlert : Parent.PortalOrderAlert;
                        Parent.OrderDate = (Parent.OrderDate == null && child.OrderDate != null) ? child.OrderDate : Parent.OrderDate;
                        ServiceTypes.Add(child.ServiceName);
                        Parent.children.Add(child);
                    }

                    Parent.ServiceName = string.Join(", ", ServiceTypes.Distinct().ToArray());
                    finalOrderDetails.Add(Parent);
                }
            }

            return finalOrderDetails;
        }

        public Office[] GetfastWebOffices(string fastWebNum)
        {
            try
            {
                List<Office> offices;
                string jsonRequest = fastWebOrderDetails.generateForwardToListRequestJSON(userID);
                sLogger.Debug($"FastWeb ForwardToList request: {jsonRequest}");
                string response = fastWebAdapter.SendFastWebResponse(jsonRequest);
                var forwardToListResponseDTO = JsonConvert.DeserializeObject<ForwardToListResponseDTO>(response);
                if (forwardToListResponseDTO?.Forwardtolistresponse?.Offices?.Office != null)
                {
                    offices = new List<Office>();
                    foreach (var ofc in forwardToListResponseDTO.Forwardtolistresponse.Offices.Office)
                        ofc.Fullname = ofc.Firstname + " " + ofc.Lastname;
                    offices = forwardToListResponseDTO.Forwardtolistresponse.Offices.Office.ToList();
                }
                else
                    throw new Exception("No FastWeb offices found in the response");

                return offices?.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SubmitForwardToOffice(string newprocessRepId, string fwOrderNum, string serviceTypes)
        {
            try
            {
                var fastWebOrderDetail = new FastWebOrderDetails();
                List<string> serviceTypeList = new List<string>();
                foreach (var type in serviceTypes?.Split(','))
                {
                    if (type.ToUpper().Contains("TITLE") && !serviceTypeList.Contains("1"))
                        serviceTypeList.Add("1");
                    if (type.ToUpper().Contains("ESCROW") && !serviceTypeList.Contains("7"))
                        serviceTypeList.Add("7");
                }

                string jsonRequest = fastWebOrderDetail.GenerateForwardToOfficeRequest(userID, newprocessRepId, fwOrderNum, serviceTypeList.ToArray());
                sLogger.Debug($"FastWeb ForwardToOffice request: {jsonRequest}");
                string response = fastWebAdapter.SendFastWebResponse(jsonRequest);
                if (response == null || response.ToUpper().Trim() == "NULL")
                    throw new Exception("ForwardToOffice response is NULL");
                var forwardToOfficeResponseDTO = JsonConvert.DeserializeObject<ForwardToOfficeResponseDTO>(response);
                if (forwardToOfficeResponseDTO != null && forwardToOfficeResponseDTO.FastweBackNack != null && forwardToOfficeResponseDTO.FastweBackNack.StatusDescription.Trim().ToUpper() == "SUCCESS")
                    return response;
                else
                    throw new Exception("NACK response received from FastWeb");
            }
            catch (Exception ex)
            {
                sLogger.Debug($"Exception:{ex.Message}");
                throw new Exception($"Failed to submit ForwardToOffice request. Exception:{ex.Message}");
            }
        }
    }
}
