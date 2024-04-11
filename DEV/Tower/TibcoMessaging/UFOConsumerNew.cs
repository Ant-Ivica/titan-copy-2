using FA.LVIS.CommonHelper;
using FAF.Messaging;

using LVIS.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using TIBCO.EMS.UFO;
using EMSException = TIBCO.EMS.EMSException;
using IExceptionListener = TIBCO.EMS.IExceptionListener;

namespace TibcoMessaging
{
    public class UFOConsumerNew : IConsumer, IExceptionListener
    {
        private byte[] s_AccessSecret = null;
        private int pwdLength = 0;
        private const string EMS_QUEUE_EVENTS = "LVIS.EVENTS.QUEUE";
        static QueueConnection connection = null;
        static QueueSession session = null;
        static QueueReceiver receiver=null;
        static TIBCO.EMS.UFO.Queue queue = null;
        private string TibcoUserName { get; set; }
        private string TibcoURL { get; set; }
        
        public UFOConsumerNew()
        {
          
        }
     
        public void DisposeObject()
        {
            DisposeConnection();
        }
        public FAF.Messaging.Message Receive(string messageChannel, long timeout)
        {
            if (!string.IsNullOrWhiteSpace(messageChannel) && messageChannel.EndsWith(EMS_QUEUE_EVENTS))
                   TibcoURL = ConfigurationManager.AppSettings["tibco_url_Receiver"];           
            if (connection == null)
                CreateConnection(messageChannel);
            return GetMessage(timeout);
        }

        public FAF.Messaging.Message ReceiveWithSearch(string messageChannel, string selector, long timeout)
        {
            if (!string.IsNullOrWhiteSpace(messageChannel) && messageChannel.EndsWith(EMS_QUEUE_EVENTS))
                    TibcoURL = ConfigurationManager.AppSettings["tibco_url_Receiver"];
            if (connection == null)
                CreateConnection(messageChannel, selector);
            return GetMessage(timeout);
        }

        private void CreateConnection(string messageChannel, string selector = null)
        {
            var retryCount = 1;
            while (retryCount < 120)
            {
                try
                {
                    if (connection == null)
                    {
                        GetConfig();
                        var credential = new NetworkCredential(TibcoUserName, GetPwd());
                        connection = UFOConnection.CreateQueueConnectionNew(TibcoURL, credential);
                        connection.ExceptionListener = this;
                        connection.Start();
                        CreateSession(messageChannel, selector);
                    }
                    break;
                }
                catch (System.Exception ex)
                {
                    retryCount++;
                    DisposeConnection();
                    Thread.Sleep(5000);
                }
            }
        }
        private static void CreateSession(string messageChannel, string selector = null)
        {
             DisposeSession();
            session = connection.CreateQueueSession(false, Session.AUTO_ACKNOWLEDGE);
            queue = session.CreateQueue(messageChannel);
            if (!string.IsNullOrWhiteSpace(selector))
                receiver = session.CreateReceiver(queue, selector);
            else
                receiver = session.CreateReceiver(queue);
        }

        private FAF.Messaging.Message GetMessage(long timeout)
        {
            var message = receiver.Receive(timeout);
            if (message == null) return null;
            var messageProperties = new Dictionary<string, string>();
            IEnumerator propertyNamesEnumerator = message.PropertyNames;
            if (null != propertyNamesEnumerator)
            {
                while (propertyNamesEnumerator.MoveNext())
                {
                    messageProperties.Add(propertyNamesEnumerator.Current.ToString(), message.GetStringProperty(propertyNamesEnumerator.Current.ToString()));
                }
            }
            if (message is TIBCO.EMS.UFO.TextMessage)
            {
                var TexMessage = ((TextMessage)message);
                return new FAF.Messaging.Message(TexMessage.Text, messageProperties);
            }
            else
            {
                var objectMessage = ((ObjectMessage)message);
                return new FAF.Messaging.Message(objectMessage.TheObject, messageProperties);
            }
        }
        private string GetPwd()
        {
            if (pwdLength == 0)
                return null;
            ProtectedMemory.Unprotect(s_AccessSecret, MemoryProtectionScope.SameLogon);
            return UnicodeEncoding.ASCII.GetString(s_AccessSecret).Substring(0, pwdLength);
        }
        private void GetConfig()
        {
            TibcoUserName = ConfigurationManager.AppSettings["tibco_username"].Decrypt();
            TibcoURL = ConfigurationManager.AppSettings["tibco_url"];
            // this mean we should have other values
            var pwd = ConfigurationManager.AppSettings["tibco_password"].Decrypt();
            pwdLength = pwd.Length;
            s_AccessSecret = Return16BytesChar(pwd);
            ProtectedMemory.Protect(s_AccessSecret, MemoryProtectionScope.SameLogon);
        }

        private byte[] Return16BytesChar(string str)
        {
            int n = 16 - str.Length % 16;
            if (n > 0)
                str += "                ".Substring(0, n);
            return UnicodeEncoding.ASCII.GetBytes(str);
        }       
        public void OnException(EMSException exception)
        {
            // Recover the connection from available server.
            connection.RecoverConnection(); 
        }
        private static void DisposeSession()
        {
            queue = null;
            if (receiver != null)
            {
                receiver.Close();
                receiver = null;
            }
            if (session != null)
            {
                session.Close();
                session = null;
            }

        }
        private static void DisposeConnection()
        {
            DisposeSession();
            if (connection != null && !connection.IsClosed)
            {
                UFOConnection.DisposeConnection();
                connection.Close();
                connection = null;
            }
            connection = null;
        }
    }
}
