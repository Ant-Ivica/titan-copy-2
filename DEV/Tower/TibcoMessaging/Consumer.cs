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
using TIBCO.EMS;

namespace TibcoMessaging
{
    public class Consumer: IConsumer
    {

        private byte[] s_AccessSecret = null;
        private int pwdLength = 0;
        public Consumer()
        {
            // Get TIBCO config values from config file.
            GetConfig();
        }
        private string TibcoUserName { get; set;  }
        private string TibcoURL { get; set; }

        private string getPwd()
        {
            if (pwdLength == 0)
                return null;
            ProtectedMemory.Unprotect(s_AccessSecret, MemoryProtectionScope.SameLogon);
            return UnicodeEncoding.ASCII.GetString(s_AccessSecret).Substring(0, pwdLength);
        }

        private void GetConfig()
        {
            // this mean we should have other values
            TibcoUserName = ConfigurationManager.AppSettings["tibco_username"].Decrypt();
            TibcoURL = ConfigurationManager.AppSettings["tibco_url"];
            var pwd = ConfigurationManager.AppSettings["tibco_password"].Decrypt();
            pwdLength = pwd.Length;
                s_AccessSecret = Return16BytesChar(pwd);
                ProtectedMemory.Protect(s_AccessSecret, MemoryProtectionScope.SameLogon);
         }
                  
        QueueConnection connection = null;
        QueueSession session = null;
        TIBCO.EMS.Queue queue = null;

        private byte[] Return16BytesChar(string str)
        {
            int n = 16 - str.Length % 16;
            if (n > 0)
                str += "                ".Substring(0, n);
            return UnicodeEncoding.ASCII.GetBytes(str);
        }

        public FAF.Messaging.Message Receive(string messageChannel, long timeout)
        {
            CreateSession();
            queue = session.CreateQueue(messageChannel);
            var receiver = session.CreateReceiver(queue);
            
            var message = receiver.Receive(timeout);
            session.Close();
            session = null;

            // exit if no message received.
            if (message == null) return null;

            var messageProperties = new Dictionary<string, string>();

            IEnumerator propertyNamesEnumerator = message.PropertyNames;

			if(null != propertyNamesEnumerator)
			{
				while(propertyNamesEnumerator.MoveNext())
				{
                    messageProperties.Add(propertyNamesEnumerator.Current.ToString(), message.GetStringProperty(propertyNamesEnumerator.Current.ToString()));
				}
			}    
      
            var objectMessage = ((ObjectMessage)message);
            return new FAF.Messaging.Message(objectMessage.TheObject, messageProperties);
        }

        public FAF.Messaging.Message ReceiveWithSearch(string messageChannel, string selector, long timeout)
        {
            CreateSession();
            queue = session.CreateQueue(messageChannel);
            var receiver = session.CreateReceiver(queue, selector);

            //var message = receiver.ReceiveNoWait();
            var message = receiver.Receive(timeout);
            session.Close();
            session = null;
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

            var objectMessage = ((ObjectMessage)message);
            return new FAF.Messaging.Message(objectMessage.TheObject, messageProperties);
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
                        connection = Connection.CreateQueueConnection(TibcoURL, credential);
                        connection.Start();
                    }

                    session = connection.CreateQueueSession(false, Session.AUTO_ACKNOWLEDGE);
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
    }
}
