namespace TibcoMessaging
{
    using System;
    using System.Net;
    using TIBCO.EMS.UFO;
    using System.Configuration;
    using FAF.Messaging;
    using System.Threading;
    using System.Text;
    using System.Security.Cryptography;
    using EMSException = TIBCO.EMS.EMSException;
    using IExceptionListener = TIBCO.EMS.IExceptionListener;
    using LVIS.Common;
    using FA.LVIS.CommonHelper;


    /// <summary>
    /// Provides operations to publish messages to a TIBCO topic.
    /// </summary>
    public class UFOPublisher : IDisposable, IPublisher , IExceptionListener
    {
        
        private byte[] s_AccessSecret = null;
        private int pwdlength = 0;
        
        public UFOPublisher()
        {
            GetConfig();
        }

       
        private void GetConfig()
        {
            TibcoUserName = ConfigurationManager.AppSettings["tibco_username"].Decrypt();
            TibcoURL = ConfigurationManager.AppSettings["tibco_url"];
            // this mean we should have other values
            var pwd = ConfigurationManager.AppSettings["tibco_password"].Decrypt();
            pwdlength = pwd.Length;
            s_AccessSecret = Return16BytesChar(pwd);
            ProtectedMemory.Protect(s_AccessSecret, MemoryProtectionScope.SameLogon);
        }


        private string getPwd()
        {
            if (pwdlength == 0)
                return null;
            ProtectedMemory.Unprotect(s_AccessSecret, MemoryProtectionScope.SameLogon);
            return UnicodeEncoding.ASCII.GetString(s_AccessSecret).Substring(0, pwdlength);
        }

        private string TibcoUserName { get; set; }
        private string TibcoURL { get; set; }

        private byte[] Return16BytesChar(string str)
        {
            int n = 16 - str.Length % 16;
            if (n > 0)
                str += "                ".Substring(0, n);
            return UnicodeEncoding.ASCII.GetBytes(str);
        }
        TIBCO.EMS.UFO.TopicConnection connection = null;
        TIBCO.EMS.UFO.TopicSession session = null;

        /// <param name="messagingServer">TIBCO EMS Server URL</param>
        /// <param name="credential">TIBCO Credentials</param>
        

        /// <summary>
        /// Sends a message to a TIBCO Topic.
        /// </summary>
        /// <param name="messageChannel">The TIBCO Topic to send to.</param>
        /// <param name="message">The message to send to the Topic.</param>
        public void Send(string messageChannel, FAF.Messaging.Message message) 
        {
            CreateSession();
            //create a topic instance. If it already exists the existing one is returned.
            var topic = session.CreateTopic(messageChannel);
            //create a message publisher
            var publisher = session.CreatePublisher(topic);
            //create message to publish.
            var objMessage = session.CreateObjectMessage(message.MessageContent);
            
            if (message.MessageMetaData.ContainsKey(FAF.Messaging.Constants.MESSAGE_TYPE))
                objMessage.MsgType = message.MessageMetaData[FAF.Messaging.Constants.MESSAGE_TYPE];

            foreach (var item in message.MessageMetaData)
            {
                objMessage.SetStringProperty(item.Key,item.Value);
            }
            // added to publish long properties 
            if (message.MessageMetaDataLong != null && message.MessageMetaDataLong.Count > 0)
                foreach (var item in message.MessageMetaDataLong)
                    objMessage.SetDoubleProperty(item.Key, item.Value); 


            //publish message
              publisher.Publish(objMessage);
            //close session.
            session.Close();
            session = null;

        }

        private void CreateSession()
        {
            var retryCount = 1;

            while (retryCount < 120) // 10 min
            { 
                try
                {
                    if (connection == null)
                    {
                        var credential = new NetworkCredential(TibcoUserName, getPwd());
                        connection = UFOConnection.CreateTopicConnection(TibcoURL, credential);
                        connection.ExceptionListener = this;
                    }

                    session = connection.CreateTopicSession(false, TIBCO.EMS.UFO.Session.AUTO_ACKNOWLEDGE);
                    break;
                }
                catch (System.Exception ex)
                {
                    retryCount++;
                    if (connection != null && !connection.IsClosed)
                    {
                        connection.Close();
                        connection = null;
                    }
                    connection = null;
                    Thread.Sleep(5000); // wait for 5 seconds before retrying.
                }
            }
            
        }

        public void Dispose()
        {
            // Ensure session correctly closed.
            if (session != null)
            {
                session.Close();
                session = null;
            }
        }

        public void OnException(EMSException exception)
        {
            // Recover the connection from available server.
            connection.RecoverConnection();
        }
    }
}
