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
    /// Provides operations to publish messages to a TIBCO Events queue.
    /// </summary>
    public class UFOSender : IDisposable, IExceptionListener
    {

        private byte[] s_AccessSecret = null;
        private int pwdlength = 0;
        private const string EMS_QUEUE_EVENTS = "LVIS.EVENTS.QUEUE";
        public UFOSender()
        {
            GetConfig();
        }



        private void GetConfig()
        {
            TibcoUserName = ConfigurationManager.AppSettings["tibco_username"].Decrypt();
            TibcoURL = ConfigurationManager.AppSettings["tibco_url"];
            // this mean we should have other values
            var pwd =ConfigurationManager.AppSettings["tibco_password"].Decrypt();
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

        TIBCO.EMS.UFO.QueueConnection queueconnection = null;
        TIBCO.EMS.UFO.QueueSession queuesession = null;
        /// <param name="messagingServer">TIBCO EMS Server URL</param>
        /// <param name="credential">TIBCO Credentials</param>


        /// <summary>
        /// Sends a message to a TIBCO Queue.
        /// </summary>
        /// <param name="messageChannel">The TIBCO Queue to send to.</param>
        /// <param name="message">The message to send to the Queue.</param>

        public void SendToQueue(string messageChannel, FAF.Messaging.Message message)
        {
            if (!string.IsNullOrWhiteSpace(messageChannel) && messageChannel.EndsWith(EMS_QUEUE_EVENTS))
                TibcoURL = ConfigurationManager.AppSettings["tibco_url_Receiver"];
                CreateSessionQueue(TibcoURL);           
            //create a topic instance. If it already exists the existing one is returned.
            var queue = queuesession.CreateQueue(messageChannel);
            //create a message publisher

            var sender = queuesession.CreateSender(queue);

            //create message to publish.
            var objMessage = queuesession.CreateObjectMessage(message.MessageContent);

            if (message.MessageMetaData.ContainsKey(FAF.Messaging.Constants.MESSAGE_TYPE))
                objMessage.MsgType = message.MessageMetaData[FAF.Messaging.Constants.MESSAGE_TYPE];

            foreach (var item in message.MessageMetaData)
            {
                objMessage.SetStringProperty(item.Key, item.Value);
            }
            // added to publish long properties 
            if (message.MessageMetaDataLong != null && message.MessageMetaDataLong.Count > 0)
                foreach (var item in message.MessageMetaDataLong)
                    objMessage.SetDoubleProperty(item.Key, item.Value);


            //publish message

            sender.Send(objMessage);
            //close session.
            queuesession.Close();
        }

        private void CreateSessionQueue(string tibcoURL)
        {
            var retryCount = 1;

            while (retryCount < 120) // 10 min
            {
                try
                {
                    if (queueconnection == null)
                    {
                        var credential = new NetworkCredential(TibcoUserName, getPwd());
                        queueconnection = UFOConnection.CreateQueueConnection(tibcoURL, credential);
                        queueconnection.ExceptionListener = this;
                    }

                    queuesession = queueconnection.CreateQueueSession(false, TIBCO.EMS.UFO.Session.AUTO_ACKNOWLEDGE);
                    break;
                }
                catch (System.Exception ex)
                {
                    retryCount++;
                    if (queueconnection != null && !queueconnection.IsClosed)
                    {
                        queueconnection.Close();
                    }
                    queueconnection = null;
                    Thread.Sleep(5000); // wait for 5 seconds before retrying.
                }
            }

        }

        public void Dispose()
        {

            if (queuesession != null)
            {
                queuesession.Close();
            }
        }

        public void OnException(EMSException exception)
        {
            // Recover the connection from available server.            
            queueconnection.RecoverConnection();

        }
    }
}
