namespace TibcoMessaging
{
    using System;
    using System.Net;
    using TIBCO.EMS.UFO;

    internal class UFOConnection 
    {
        private static DateTime _topicConnectionCreated = DateTime.Now;
        private static DateTime _queueConnectionCreated = DateTime.Now;

        private static TIBCO.EMS.UFO.TopicConnection _topicConnection = null;
        private static TIBCO.EMS.UFO.QueueConnection _queueConnection = null;
        private static TIBCO.EMS.UFO.QueueConnection _queueConnectionNew = null;

        private static Object _topicLockObj = new Object();
        private static Object _queueLockObj = new Object();
        private static Object _queueLockObjNew = new Object();

        /// <summary>
        /// Creates a TIBCO Topic connection that lives as long as the application process is running.
        /// This is a recommendation from TIBCO. It is implemented using a singleton pattern.
        /// </summary>
        /// <param name="url"></param> 
        /// <param name="credential"></param>
        /// <returns></returns>
        public static TIBCO.EMS.UFO.TopicConnection CreateTopicConnection(string url, NetworkCredential credential) 
        {
            if (_topicConnection == null || _topicConnectionCreated.AddMinutes(15) < DateTime.Now)
            {
                lock (_topicLockObj)
                {
                    if (_topicConnection == null || _topicConnectionCreated.AddMinutes(15) < DateTime.Now)
                    {
                        if (_topicConnection != null)
                        {
                            _topicConnection.Close();
                            _topicConnection = null;
                        }

                        var factory = new TIBCO.EMS.UFO.TopicConnectionFactory(url);
                        // try to connect for up to a minute.
                        factory.SetConnAttemptCount(10);    // 10 attempts 
                        factory.SetConnAttemptDelay(1000);  // 1 second between attempts
                        factory.SetConnAttemptTimeout(5000);// give up attempt after 5 seconds.

                        // try to re-connect for up to 10 minutes
                        factory.SetReconnAttemptCount(30);      // 30 attempts
                        factory.SetReconnAttemptDelay(10000);   // 10 seconds between attempts.
                        factory.SetReconnAttemptTimeout(10000); // give up attempt after 10 secods.

                        _topicConnection = factory.CreateTopicConnection(credential.UserName, credential.Password);
                        _topicConnectionCreated = DateTime.Now;
                    }
                }
            }
            
            return _topicConnection;
        }

        /// <summary>
        /// Creates a TIBCO Queue connection that lives as long as the application process is running.
        /// This is a recommendation from TIBCO. It is implemented using a singleton pattern.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="credential"></param>
        /// <returns></returns>
        public static TIBCO.EMS.UFO.QueueConnection CreateQueueConnection(string url, NetworkCredential credential)
        {
            if (_queueConnection == null || _queueConnectionCreated.AddMinutes(15) < DateTime.Now)
            {
                lock (_queueLockObj)
                {
                    if (_queueConnection == null || _queueConnectionCreated.AddMinutes(15) < DateTime.Now)
                    {
                        if (_queueConnection != null)
                        {
                            _queueConnection.Close();
                            _queueConnection = null;
                        }

                        var factory = new TIBCO.EMS.UFO.QueueConnectionFactory(url);
                        // try to connect for up to a minute.
                        factory.SetConnAttemptCount(10);    // 10 attempts 
                        factory.SetConnAttemptDelay(1000);  // 1 second between attempts
                        factory.SetConnAttemptTimeout(5000);// give up attempt after 5 seconds.

                        // try to re-connect for up to 10 minutes
                        factory.SetReconnAttemptCount(30);      // 30 attempts
                        factory.SetReconnAttemptDelay(10000);   // 10 seconds between attempts.
                        factory.SetReconnAttemptTimeout(10000); // give up attempt after 10 secods.

                        _queueConnection = factory.CreateQueueConnection(credential.UserName, credential.Password);
                        _queueConnectionCreated = DateTime.Now;
                    }
                }
            }
            
            return _queueConnection;
        }

        public static TIBCO.EMS.UFO.QueueConnection CreateQueueConnectionNew(string url, NetworkCredential credential)
        {
                lock (_queueLockObjNew)
                {
                    if (_queueConnectionNew == null)
                    {
                        var factory = new TIBCO.EMS.UFO.QueueConnectionFactory(url);
                        // try to connect for up to a minute.
                        factory.SetConnAttemptCount(10);    // 10 attempts 
                        factory.SetConnAttemptDelay(1000);  // 1 second between attempts
                        factory.SetConnAttemptTimeout(5000);// give up attempt after 5 seconds.

                        // try to re-connect for up to 10 minutes
                        factory.SetReconnAttemptCount(30);      // 30 attempts
                        factory.SetReconnAttemptDelay(10000);   // 10 seconds between attempts.
                        factory.SetReconnAttemptTimeout(10000); // give up attempt after 10 secods.

                    _queueConnectionNew = factory.CreateQueueConnection(credential.UserName, credential.Password);                        
                    }
                }            

            return _queueConnectionNew;
        }
        public static void DisposeConnection()
        {
            if (_queueConnectionNew != null && !_queueConnectionNew.IsClosed)
            {
                _queueConnectionNew.Close();
                _queueConnectionNew = null;
            }
        }

    }
}
