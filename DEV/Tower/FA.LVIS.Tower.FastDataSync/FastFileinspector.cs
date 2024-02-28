using FA.LVIS.CommonHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FA.LVIS.Tower.FastDataSync
{

    public class CustomInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            string s_AccessUser = ConfigurationManager.AppSettings["FastAccessUser"];

            string s_Domain ="", s_AccessPwd = "";
            if (s_AccessUser != null)
            {
                string[] ss = s_AccessUser.Split(';');
                s_AccessUser = ss[0].Trim().Decrypt();
                if (ss.Length > 1)
                {
                    s_AccessUser = ss[0].Replace('\\', '/');
                    if (s_AccessUser.Contains('/'))
                    {
                        string[] s2 = s_AccessUser.Split('/');
                        s_AccessUser = s2[1].Trim();
                        s_Domain = s2[0].Trim();
                    }

                     s_AccessPwd = (ss[1].Trim()).Decrypt();

                    // Set                                
                }
            }
            string Nonce = ConfigurationManager.AppSettings["FastNonce"];


            var ws2004Prefix = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-";

            var u = (XNamespace)(ws2004Prefix + "wssecurity-utility-1.0.xsd");
            var o = (XNamespace)(ws2004Prefix + "wssecurity-secext-1.0.xsd");
            var soapenv = (XNamespace)("http://schemas.xmlsoap.org/soap/envelope/");

            var pwdTextType = (ws2004Prefix + "username-token-profile-1.0#PasswordText");
            var base64Type = (ws2004Prefix + "soap-message-security-1.0#Base64Binary");
            var xDoc = new XDocument(

                new XElement(o + "UsernameToken",
                        new XElement(o + "Username", s_Domain + "\\" + s_AccessUser),
                        new XElement(o + "Password", new XAttribute("Type", pwdTextType), s_AccessPwd),
                        new XElement(o + "Nonce", new XAttribute("EncodingType", base64Type), Nonce)));

            MessageHeader messageHeader = MessageHeader.CreateHeader("Security",
               o.ToString(), xDoc.Root, false);

            request.Headers.Add(messageHeader);
            return null;
        }
    }

    public class CustomBehavior : IEndpointBehavior
    {

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
         
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new CustomInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
