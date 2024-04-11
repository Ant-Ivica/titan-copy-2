using FA.LVIS.Tower.DataContracts;
using LVIS.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;

namespace FA.LVIS.Tower.FastWebProcessing
{
    public class FastWebOrderDetails
    {
        readonly FastWebAdapter fastWebAdapter = new FastWebAdapter();
        readonly private string userID = ConfigurationManager.AppSettings["FastWebUserID"];
        readonly private string superUserID = ConfigurationManager.AppSettings["FastWebSuperUserID"];

        public string GenerateForwardToOfficeRequest(string userID, string newprocessRepId, string fwOrderNum, string[] serviceType)
        {
            var forwardToOfficeRequestDTO = new ForwardToOfficeRequestDTO()
            {
                FastwebHeader = new FastWebHeader()
                {
                    FWActionType = "ForwardToOffice"
                },
                ForwardToRequest = new FORWARD_TO_REQUEST()
                {
                    FORWARD_TO_OFFICE_REQUEST = new ForwardToOfficeRequest()
                    {
                        FwOrderNumber = fwOrderNum,
                        NewprocessRepId = newprocessRepId,
                        UserId = userID,
                        ServiceTypes = new ServiceTypes()
                        {
                            ServiceType = serviceType
                        }
                    }
                }
            };

            return JsonConvert.SerializeObject(forwardToOfficeRequestDTO, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public string generateForwardToListRequestJSON(string userId)
        {
            var forwardToListRequestDTO = new ForwardToListRequestDTO()
            {
                FASTWEB_HEADER = new FASTWEBHEADER()
                {
                    FWActionType = "ForwardToList"
                },
                FORWARD_TO_REQUEST = new FORWARD_TO_REQUEST()
                {
                    FORWARD_TO_LIST_REQUEST = new FORWARD_TO_LIST_REQUEST()
                    {
                        UserId = userId
                    }
                }
            };

            return JsonConvert.SerializeObject(forwardToListRequestDTO, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public FastWebOrderDetailsDTO GetFastWebOrderDetails(int fastwebnum, string servicename)
        {
            try
            { 
                FastWebOrderDetailsDTO finalOrderDetails = new FastWebOrderDetailsDTO();
            FastWebOrderDetailResponse OrderDetailsResponse = pullFastWebOrderDetailsResponse(fastwebnum);

            var Orderdetails = OrderDetailsResponse?.FASTWEBORDERRESPONSE?.ORDERDETAILRESPONSE?.OrderDetails;
            var Orderdetailsresp = OrderDetailsResponse?.FASTWEBORDERRESPONSE?.ORDERDETAILRESPONSE?.PropertyData;
            var OrderdetailsPartyData = OrderDetailsResponse?.FASTWEBORDERRESPONSE?.ORDERDETAILRESPONSE?.PartyData?.Party?.FirstOrDefault();
            ServiceInformation OrderdetailsServiceData = new ServiceInformation();
            if (servicename.ToUpper().Contains("TITLE"))
                OrderdetailsServiceData = OrderDetailsResponse?.FASTWEBORDERRESPONSE?.ORDERDETAILRESPONSE?.ServiceInformation.Where(x => x.ServiceName.ToUpper().Contains("TITLE")).FirstOrDefault();
            else
                OrderdetailsServiceData = OrderDetailsResponse?.FASTWEBORDERRESPONSE?.ORDERDETAILRESPONSE?.ServiceInformation.Where(x => x.ServiceName.ToUpper().Contains("ESCROW")).FirstOrDefault();
            finalOrderDetails.FASTWebOrderNumber = Orderdetails?.Fwordernumber;
            finalOrderDetails.OrderDate = Orderdetails?.Dateopened;
            finalOrderDetails.CustomerRefNumber = Orderdetails?.Customerrefnumber;
            finalOrderDetails.Loanamount = Orderdetails?.Loanamount;
            finalOrderDetails.SalePrice = Orderdetails?.Saleprice;
            finalOrderDetails.Transactiontype = Orderdetails?.Transactiontype;
            finalOrderDetails.Propertytype = Orderdetails?.Propertytype;
            finalOrderDetails.Propertyuse = Orderdetails?.Propertyuse;
            finalOrderDetails.Customeroffice = Orderdetails?.Customeroffice;
            finalOrderDetails.Officeaddress = Orderdetails?.Officeaddress;
            finalOrderDetails.Contact = Orderdetails?.CustomerContact;
            finalOrderDetails.Email = Orderdetails?.Email;
            finalOrderDetails.Orderphone = Orderdetails?.Phone;
            finalOrderDetails.Propertyaddress = Orderdetailsresp?.Propertyaddress;
            finalOrderDetails.APN = Orderdetailsresp?.APN;
            finalOrderDetails.County = Orderdetailsresp?.County;
            finalOrderDetails.Legaldescription = Orderdetailsresp?.Legaldescription;
            finalOrderDetails.Borrowerentitytype = OrderdetailsPartyData?.Entitytype;
            finalOrderDetails.Maritalstatus = OrderdetailsPartyData?.Maritalstatus;
            finalOrderDetails.Lastname = OrderdetailsPartyData?.Lastname;
            finalOrderDetails.Firstname = OrderdetailsPartyData?.Firstname;
            finalOrderDetails.Currentaddress = OrderdetailsPartyData?.Address;
            finalOrderDetails.Spouselastname = OrderdetailsPartyData?.Spouselastname;
            finalOrderDetails.Spousefirstname = OrderdetailsPartyData?.Spousefirstname;
            if (servicename.ToUpper().Contains("TITLE"))
                finalOrderDetails.Servicenname = "Title Insurance";
            else if (servicename.ToUpper().Contains("ESCROW"))
                finalOrderDetails.Servicenname = "Escrow/Closing";
            finalOrderDetails.Processor = OrderdetailsServiceData?.Processorname;
            finalOrderDetails.Address = OrderdetailsServiceData?.Processoraddress;
            finalOrderDetails.Orderdesktype = OrderdetailsServiceData?.Orderdesktype;
            finalOrderDetails.Contactname = OrderdetailsServiceData?.Contactname;
            finalOrderDetails.Servicephone = OrderdetailsServiceData?.Contactnumber;
            finalOrderDetails.Serviceemail = OrderdetailsServiceData?.Contactemail;
            finalOrderDetails.Status = OrderdetailsServiceData?.Orderstatus;
            finalOrderDetails.FASTfilenumber = OrderdetailsServiceData?.Fastfilenumber;
            finalOrderDetails.Portalordealert = OrderdetailsServiceData?.Portalorderalert;
            finalOrderDetails.Comments = OrderdetailsServiceData?.Comments;
                finalOrderDetails.Product = OrderdetailsServiceData?.Productsordered?.Product?.FirstOrDefault()?.ProductName;

            return finalOrderDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    private string GetFastWebOrderRequest(int FastWeborder)
    {
        var fastWebOrderDetailRequest = new FastWebOrderDetailRequest()
        {
            FastWebHeader = new FastWebHeader()
            {
                FWActionType = "OrderDetail"
            },
            FASTWEB_ORDER_REQUEST = new FASTWEB_ORDER_REQUEST()
            {
                FASTNOTIFY_REQUEST = new FASTNOTIFY_REQUEST()
                {
                    iProcessingRepID = superUserID
                },
                ORDER_DETAIL_REQUEST = new ORDER_DETAIL_REQUEST()
                {
                    FWOrderNumber = FastWeborder
                }
            }
        };
            return JsonConvert.SerializeObject(fastWebOrderDetailRequest, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
    }
    public FastWebOrderDetailResponse pullFastWebOrderDetailsResponse(int FastWeborder)
    {
            string jsonRequest = GetFastWebOrderRequest(FastWeborder);
            string response = fastWebAdapter.SendFastWebResponse(jsonRequest);
            FastWebOrderDetailResponse fastOrdeDetailsResp = JsonConvert.DeserializeObject<FastWebOrderDetailResponse>(response);
            return fastOrdeDetailsResp;
    }
        public string AddForwardToOfc(ADD_FORWARD_TO_OFFICE_REQEUST AddForwardOfc)
        {
            try
            {                
                if (AddForwardOfc.BUID == null || AddForwardOfc.FirstName == null || AddForwardOfc.LastName == null)
                    throw new Exception("Office details are empty");
                string jsonRequest = createAddForwardToOfficeRequest(AddForwardOfc);
                string response = fastWebAdapter.SendFastWebResponse(jsonRequest);
                var FastwebAckNack = JsonConvert.DeserializeObject<ForwardToListResponseDTO>(response);
                if (FastwebAckNack?.FastwebAckNack?.StatusDescription != "Failure")
                    response = "Office details submission successful for the UserId :" + AddForwardOfc.UserId + " ";
                else
                    throw new Exception("There was error while submitting office details, Response from FASTWeb is:-" + FastwebAckNack.FastwebAckNack.StatusDescription + "");

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string createAddForwardToOfficeRequest(ADD_FORWARD_TO_OFFICE_REQEUST AddForwardOfc)
        {
            var AddForwardToOfc = new AddForwardToOffice()
            {
                FastWebHeader = new FastWebHeader()
                {
                    FWActionType = "AddForwardToOffice"
                },
                ForwardTorequest = new FORWARD_TO_REQUEST()
                {
                    AddForwardToOfficeRequest = new ADD_FORWARD_TO_OFFICE_REQEUST()
                    {
                        UserId = userID.ToInt(),
                        BUID = AddForwardOfc.BUID,
                        FirstName = AddForwardOfc.FirstName,
                        LastName = AddForwardOfc.LastName
                    }
                }
            };

            return JsonConvert.SerializeObject(AddForwardToOfc, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
        
    }
}
