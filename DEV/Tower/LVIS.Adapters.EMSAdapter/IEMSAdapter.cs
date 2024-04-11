using System;
using System.Collections.Generic;

namespace LVIS.Adapters.EMSAdapter
{
    public interface IEMSAdapter
    {
        Dictionary<string, string> ConsumeMessage(string queueName, out string messageBody);
        Dictionary<string, string> ConsumeMessageWithSearch(string queueName, out string messageBody, string selector);
        bool PublishMessage(string dest, string source, long docObjId, int? serviceRequestId = null);
        bool PublishMessage(string dest, string source, long docObjId, string internalRefNumber,string tagRef = null);
        bool PublishMessage(string dest, string source, int serviceRequestId, int messageLogId, string messageData, string externalRefNum = null, DateTime? timeStamp = default(DateTime?),string tagRef = null, string internalRefNum = null);
        bool PublishMessage(string dest, string source, string identifier, long docObjId, double publishDate, double recevingDate);

      bool  PublishMessage(string dest, string source, int serviceRequestId, int messageLogId, string messageData, int  retry);


        bool PublishMessage(string dest, string source, long docObjId, string tagRef, int xRefId);

        bool PublishMessageToEventsQueue(string dest,string source,long docObjId,string tagRef,string processName,string processType,string serviceFileProcessID,string orderSourceId,string secondOrderSourceID,string regionID,string objectCD);
        void DisposeConnection();
    }
}