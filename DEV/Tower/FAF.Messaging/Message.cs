using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAF.Messaging
{
    public class Message
    {
        public object MessageContent { get; set; }
        public Dictionary<string, string> MessageMetaData { get; set; }

        public Dictionary<string, double> MessageMetaDataLong { get; set; }

        public Message(object messageContent, Dictionary<string,string> messageMetaData)
        {
            MessageContent = messageContent;
            MessageMetaData = messageMetaData;
        }
    }
}
