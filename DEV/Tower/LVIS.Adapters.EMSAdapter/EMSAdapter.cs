using FAF.Messaging;
using LVIS.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using FAF.Messaging;
using TibcoMessaging;

namespace LVIS.Adapters.EMSAdapter
{
    public class EMSAdapter : IEMSAdapter
    {
        private long sReceiveTimeout = 100;

        IUtils Utils;
        private bool newConsumerConnection = false;

        public static Func<string> GetTrackingId { get; set; } = () => "";
        private TibcoMessaging.UFOConsumerNew consumerNew = null;

        public EMSAdapter()
        {
            newConsumerConnection = GetConfigValue("NewConsumerConnection");
            consumerNew = new TibcoMessaging.UFOConsumerNew();
            Utils = new Utils();
            if (Utils.IsConfig("tibco_timeout"))
                long.TryParse(Utils.GetConfig("tibco_timeout"), out sReceiveTimeout);
        }

        public EMSAdapter(IUtils utils = null)
        {
            newConsumerConnection = GetConfigValue("NewConsumerConnection");
            consumerNew = new TibcoMessaging.UFOConsumerNew();
            Utils = utils ?? new Utils();
            if (Utils.IsConfig("tibco_timeout"))
                long.TryParse(Utils.GetConfig("tibco_timeout"), out sReceiveTimeout);
        }     

        public void DisposeConnection()
        {
            consumerNew.DisposeObject();
        }

        public Dictionary<string, string> ConsumeMessageWithSearch(string queueName, out string messageBody, string selector)
        {
            return ConsumeMessageInternal(queueName, out messageBody, selector);
        }

        public Dictionary<string, string> ConsumeMessage(string queueName, out string messageBody)
        {
            return ConsumeMessageInternal(queueName, out messageBody);
        }



        public bool PublishMessage(string dest, string source, int serviceRequestId, int messageLogId, string messageData, string externalRefNum = null, DateTime? timeStamp = null,string tagRef = null, string internalRefNum = null)
        {
            var newMessage = CreateEmsMessage(dest, source, messageData,
                                serviceRequestId: serviceRequestId,
                                messageLogId: messageLogId,
                                externalRefNum: externalRefNum,
                                createdatetime: timeStamp == null ? null : timeStamp.Value.ToString("o"),
                                tagref:tagRef,
                                internalrefnumber: internalRefNum);

            return PublishMessage(newMessage);
        }
        public bool PublishMessage(string dest, string source, int serviceRequestId, int messageLogId, string messageData, int Retry)
        {
            var newMessage = CreateEmsMessage(dest, source, messageData,
                                serviceRequestId: serviceRequestId,
                                messageLogId: messageLogId);
            newMessage.Add(Common.Constants.EMS_RetryFlag, Retry);
            return PublishMessage(newMessage);
        }

        public bool PublishMessage(string dest, string source, long docObjId, int? serviceRequestId = null)
        {
            var newMessage = CreateEmsMessage(dest, source, docObjId,
                                    serviceRequestId: serviceRequestId);

            return PublishMessage(newMessage);
        }


        //Added for keeping fast file number for further processing in case of synchronous acknowledgement
        public bool PublishMessage(string dest, string source, long docObjId, string internalRefNumber,string tagRef = null)
        {
            var newMessage = CreateEmsMessage(dest, source, docObjId,
                                    internalrefnumber: internalRefNumber,tagref:tagRef);

            return PublishMessage(newMessage);
        }
        public bool PublishMessageToEventsQueue(string dest,string source,long docObjId,string tagRef,string processName,string processType,string serviceFileProcessID,string orderSourceId,string secondOrderSourceID,string regionID,string objectCD)
        {
            var newMessage = CreateEmsMessage(dest, source, docObjId,tagref: tagRef,objectCD:objectCD,processName:processName,processType:processType,secondOrderSource:secondOrderSourceID,orderSource:orderSourceId,regionId:regionID,serviceFileProcessId:serviceFileProcessID);

            return PublishMessageToEventsQueue(newMessage);
        }
        public bool PublishMessage(string dest, string source, long docObjId, string tagRef, int xRefId)
        {
            var newMessage = CreateEmsMessage(dest, source, docObjId,
                                    FastFileLvisAppXrefId: xRefId,
                                    tagref: tagRef);
            return PublishMessage(newMessage);
        }
        public bool PublishMessage(string dest, string source, string OrderIdentifier, long docObjId, double publishDate, double recevingDate)
        {
            var newMessage = CreateEmsMessage(dest, source, docObjId,
                                    orderidentifier: OrderIdentifier);

            newMessage.MessageMetaDataLong = new Dictionary<string, double>();

            newMessage.MessageMetaDataLong.Add(Common.Constants.EMS_PUBLISH_DATE, publishDate);
            newMessage.MessageMetaDataLong.Add(Common.Constants.EMS_RECEIVE_DATE, recevingDate);

            return PublishMessage(newMessage);
        }


        public Message CreateEmsMessage(string dest, string source, object messageData,
                           object serviceRequestId = null,
                           object messageLogId = null,
                           object internalrefnumber = null,
                           object externalRefNum = null,
                           object createdatetime = null,
                           object orderidentifier = null,
                           object tagref = null,
                           object processName = null,
                           object processType = null,
                           object regionId = null,
                           object objectCD = null,
                           object orderSource = null,
                           object secondOrderSource = null,
                           object serviceFileProcessId = null,
                           object FastFileLvisAppXrefId = null)
        {

            var newMessage = new Message(messageData, new Dictionary<string, string>());
            newMessage.Add(Common.Constants.EMS_DESTINATION, dest);
            newMessage.Add(Common.Constants.EMS_SOURCE, source);
            newMessage.Add(Common.Constants.EMS_SERVICEREQUESTID, serviceRequestId);
            newMessage.Add(Common.Constants.EMS_MESSAGELOGID, messageLogId);
            newMessage.Add(Common.Constants.EMS_INTERNAL_REF_NUMBER, internalrefnumber);
            newMessage.Add(Common.Constants.EMS_EXTERNALREFNUM, externalRefNum);
            newMessage.Add(Common.Constants.EMS_ORDER_IDENTIFER, orderidentifier);
            newMessage.Add(Common.Constants.EMS_CREATED_TIME, createdatetime);
            newMessage.Add(Common.Constants.EMS_TAG_REF, tagref);
            newMessage.Add(Common.Constants.EMS_ORDER_SOURCE, orderSource);
            newMessage.Add(Common.Constants.EMS_SECOND_ORDER_SOURCE, secondOrderSource);
            newMessage.Add(Common.Constants.EMS_PROCESS_NAME, processName);
            newMessage.Add(Common.Constants.EMS_PROCESS_TYPE, processType);
            newMessage.Add(Common.Constants.EMS_REGIONID, regionId);
            newMessage.Add(Common.Constants.EMS_OBJECTCD, objectCD);
            newMessage.Add(Common.Constants.EMS_FASTFILE_LVISAPP_XREF_ID, FastFileLvisAppXrefId);
            newMessage.Add(Common.Constants.EMS_SERVICEFILEPROCESSID, serviceFileProcessId);
            if (!string.IsNullOrEmpty(GetTrackingId()))
                newMessage.Add(Common.Constants.EMS_TRACKINGID, GetTrackingId()); 
            return newMessage;
        }

        public string FullyQulifiedMessageChannel(string queueName)
        {
            const string EMS_QUEUE_EVENTS = "LVIS.EVENTS.QUEUE";
            string FASTE = string.Empty;
            var queueEnv = Utils.IsConfig("tibco_env") ? Utils.GetConfig("tibco_env") : "FAF.DEV";

            if (Utils.IsConfig("tibco_env_Receiver")
                    && !string.IsNullOrWhiteSpace(queueName)
                    && queueName.EndsWith(EMS_QUEUE_EVENTS))
            {
                queueName = Utils.GetConfig("Queue");
                queueEnv = Utils.GetConfig("tibco_env_Receiver");
            }

            return $"{queueEnv}.{queueName}";
        }

        private Dictionary<string, string> ConsumeMessageInternal(string queueName, out string messageBody, string selector = null)
        {
            messageBody = null;
            Message message = null;
            try
            {
                if (newConsumerConnection)
                    message = ConsumeMessageNew(queueName, selector);
                else
                    message = ConsumeMessage(queueName, selector);

                if (message == null)
                    return null;

                var result = new Dictionary<string, string>(message.MessageMetaData);

                if (!result.ContainsKey(Common.Constants.EMS_MESSAGECONTENT))
                {
                    result.Add(Common.Constants.EMS_MESSAGECONTENT, message.MessageContent.ToString());
                };

                messageBody = message.MessageContent.ToString();

               
                return result;
            }
            catch (Exception ex)
            {
                //log error
                throw new LVISErrorException(MessageProcess.GetErrorMessage(ErrorMessage.GenericParameterError, queueName));
            }
        }

        private bool PublishMessage(FAF.Messaging.Message newMessage)
        {
            var messageChannel = FullyQulifiedMessageChannel(Common.Constants.EMS_TOPIC_RECEIVE);
            if (DevQueueSettings.IsDevEnv(messageChannel))
            {
                var devPublisher = new DevQueuePublisher();
                devPublisher.Send(messageChannel, newMessage);
                return true;
            }

            var publisher = new UFOPublisher();
            publisher.Send(messageChannel, newMessage);
            publisher.Dispose();
            return true;
        }
        private bool PublishMessageToEventsQueue(FAF.Messaging.Message newMessage)
        {
            var messageChannel = Utils.GetConfig("eventsQueueName");
            var sender = new UFOSender();
            sender.SendToQueue(messageChannel, newMessage);
            sender.Dispose();
            return true;
        }

        private Message ConsumeMessage(string queueName, string selector = null)
        {
            var messageChannel = FullyQulifiedMessageChannel(queueName);

            if (DevQueueSettings.IsDevEnv(messageChannel))
            {
                return new DevQueueConsumer().Receive(messageChannel, sReceiveTimeout);
            }

            var consumer = new UFOConsumer();
            var message = (selector == null ? consumer.Receive(messageChannel, 2000)
                                : consumer.ReceiveWithSearch(messageChannel, selector, sReceiveTimeout));
            consumer.Dispose();

            return message;
        }

        private Message ConsumeMessageNew(string queueName, string selector = null)
        {

            var messageChannel = FullyQulifiedMessageChannel(queueName);
            if (DevQueueSettings.IsDevEnv(messageChannel))
            {
                return new DevQueueConsumer().Receive(messageChannel, sReceiveTimeout);
            }
            var message = (selector == null ? consumerNew.Receive(messageChannel, 2000)
                                : consumerNew.ReceiveWithSearch(messageChannel, selector, sReceiveTimeout));

            return message;

        }

        private bool GetConfigValue(string key)
        {
            try
            {
                string value = string.Empty;
                value = ConfigurationManager.AppSettings[key];
                if (!string.IsNullOrWhiteSpace(value) && value.ToUpper() == "TRUE")
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
        
    }



    static class ExtensionHelper
    {
        public static Message Add(this Message newMessage, object key, object value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                newMessage.MessageMetaData.Add(key.ToString(), value.ToString());
            return newMessage;
        }
    }
}
