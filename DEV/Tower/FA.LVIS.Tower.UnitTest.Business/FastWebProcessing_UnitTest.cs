using System;
using System.Collections.Generic;
using FA.LVIS.CommonHelper;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.FastWebProcessing;
using LVIS.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FA.LVIS.Tower.UnitTests.Business
{
    [TestClass]
    public class FastWebProcessing_UnitTest
    {
        readonly private string fastwebtester_UserId = "409826"; //test_fastwebtesterindia
        readonly private string bangaloreTest_UserId = "423754"; //BangaloreTestDirect
        readonly private string superUserID = "30449"; //Cory
        readonly private Utils utils;
        readonly private FastWebAdapter fastWebAdapter;
        readonly private FastWebClientData fastWebClientData;
        readonly private FastWebOrderDetails fastWebOrderDetails;
        
        public FastWebProcessing_UnitTest()
        {
            utils = new Utils();
            fastWebAdapter = new FastWebAdapter();
            fastWebClientData = new FastWebClientData();
            fastWebOrderDetails = new FastWebOrderDetails();
        }


        [TestMethod]
        public void FastWebClientData_UnitTest()
        {
            var results = fastWebClientData.GetFastWebOrderDetails();
            if (results != null)
            {
                Assert.AreEqual(true, results.Count > 0);

            }
        }

        [TestMethod]
        public void FastWeb_ForwardToList_UnitTest()
        {
            //Utils utils = new Utils();
            //string pwd = "FWNotify:fastweb123";
            //string credentials = utils.Encrypt(pwd);

            string reqJSON = fastWebOrderDetails.generateForwardToListRequestJSON(bangaloreTest_UserId);
            var response = fastWebAdapter.SendFastWebResponse(reqJSON);
            Assert.AreEqual(true, response != null);
        }

        [TestMethod]
        public void FastWeb_ForwardToOffice_UnitTest()
        {
            string fastwebnum = "23397578";
            List<string> serviceTypes = new List<string>();
            serviceTypes.Add("1"); serviceTypes.Add("7");
            string reqJSON_ForwardToList = fastWebOrderDetails.generateForwardToListRequestJSON(bangaloreTest_UserId);
            var response_ForwardToList = fastWebAdapter.SendFastWebResponse(reqJSON_ForwardToList);
            var forwardToListResponseDTO = JsonConvert.DeserializeObject<ForwardToListResponseDTO>(response_ForwardToList);
            string reqJSON = fastWebOrderDetails.GenerateForwardToOfficeRequest(bangaloreTest_UserId, forwardToListResponseDTO.Forwardtolistresponse.Offices.Office[0].Userid.ToString(), fastwebnum, serviceTypes.ToArray());
            var response = fastWebAdapter.SendFastWebResponse(reqJSON);
            Assert.AreEqual(true, response != null);
            if (response != null)
            {
                var forwardToOfficeResponseDTO = JsonConvert.DeserializeObject<ForwardToOfficeResponseDTO>(response);
                Assert.AreEqual(true, forwardToOfficeResponseDTO != null && forwardToOfficeResponseDTO.FastweBackNack != null && forwardToOfficeResponseDTO.FastweBackNack.StatusDescription.Trim().ToUpper() == "SUCCESS");
            }
        }

        [TestMethod]
        public void FastWeb_SuperUserCheck_UnitTest()
        {
            string searchType = "1";
            string searchText = "10";
            try
            {
                string jsonFastNotifyRequest = fastWebClientData.GetFastWebNotifyRequest(searchText);
                var fwResponse = fastWebAdapter.SendFastWebResponse(jsonFastNotifyRequest);
                var fASTNotifyResponse = JsonConvert.DeserializeObject<FASTNotifyResponse>(fwResponse);
                searchText = fASTNotifyResponse?.FASTWEBORDERRESPONSE?.FASTNOTIFYRESPONSE?.FASTWEBORDERS?.FASTWEBOrder[0]?.FWOrderNumber.ToString() ?? null;
                string jsonRequest = fastWebClientData.GetFastWebNotifyRequest(searchType, searchText);
                string response = fastWebAdapter.SendFastWebResponse(jsonRequest);
                var fastwebResp = JsonConvert.DeserializeObject<FASTNotifyResponse>(response);
                Assert.AreEqual(searchText, fastwebResp?.FASTWEBORDERRESPONSE?.FASTNOTIFYRESPONSE?.FASTWEBORDERS.FASTWEBOrder[0].FWOrderNumber.ToString());
            }
            catch(Exception ex)
            {
                Assert.Fail($"Something went wrong!! Exception:{ex.Message}");
            }
        }

        readonly string credentials = "LVISTestCustomer:Spring2014!";
        readonly string encryptedCredentials = "s+d00kVR1DDJnvZqMlfX7ptmrT2iCe3O58owYHakwvRXuEgDqHWp+xq0PirilVyI";
        [TestMethod]
        public void UnitTest_Decrypt()
        {
            //string data = Util.Encrypt("RldOb3RpZnk6ZmFzdHdlYjEyMw==");
            string encryptString = encryptedCredentials.Decrypt();
            string str = utils.DecodedBase64(encryptString);
            Assert.AreEqual(credentials, str);
        }

        [TestMethod]
        public void UnitTest_Encrypt()
        {
            string credentialsBase64 = utils.Base64Encode(credentials);
            string encryptedString = utils.Encrypt(credentialsBase64);
            Assert.AreEqual(encryptedCredentials, encryptedString);
        }
    }
}
