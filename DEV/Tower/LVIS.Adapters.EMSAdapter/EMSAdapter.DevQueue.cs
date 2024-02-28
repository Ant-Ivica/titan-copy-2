using FAF.Messaging;
using LVIS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LVIS.Adapters.EMSAdapter
{
    public static class DevQueueSettings
    {
        public const string DevQueue_RootPath = @"c:\cm\devqueue\";
        //Renaming the queue prefic to DEVLOCAL insted of DEV to diff it from the Dev Environment 
        public const string DevQueue_NamePrefix = @"FAF.DEVLOCAL.LVIS";
        public const string DevQueue_NameTrailing = @"QUEUE.txt";
        //Renaming the queue prefic to DEVLOCAL insted of DEV to diff it from the Dev Environment
        public static bool IsDevEnv(string queueName) => queueName.IgnoreCaseContains("faf.devlocal");
    }
    class DevQueuePublisher
    {
        public void Send(string messageChannel, Message message)
        {

            var directory = DevQueueSettings.DevQueue_RootPath;

            System.IO.Directory.CreateDirectory(directory);

            var destQueueName = $"{DevQueueSettings.DevQueue_NamePrefix}.{message.MessageMetaData[Common.Constants.EMS_DESTINATION]}.{DevQueueSettings.DevQueue_NameTrailing}";

            message.MessageMetaData.Add(Common.Constants.EMS_MESSAGECONTENT, message.MessageContent.ToString());

            if (message.MessageMetaDataLong?.Any() ?? false)
            {
                foreach (var item in message.MessageMetaDataLong)
                    message.MessageMetaData.Add(item.Key, item.Value.ToString());
            }

            var listMessage = message.MessageMetaData
                                .Select(k => new SerializableKeyValue<string, string> { Key = k.Key, Value = k.Value })
                                .ToArray();

            var messageString = $"{new Utils().SerializeToString(listMessage)} {Environment.NewLine}";

            System.IO.File.AppendAllText(directory + destQueueName, messageString);
        }
    }

    class DevQueueConsumer
    {
        public Message Receive(string messageChannel, long timeout)
        {
            Message message = null;

            var queueFilePath = DevQueueSettings.DevQueue_RootPath + messageChannel + ".txt";

            if (!System.IO.File.Exists(queueFilePath))
                return null;

            var lines = System.IO.File.ReadLines(queueFilePath).ToList();
            if (lines.Any())
            {
                System.IO.File.WriteAllText(queueFilePath, string.Empty);

                var listMessage = new Utils().DeSerializeToObject<List<SerializableKeyValue<string, string>>>(lines[0]);

                var dictMessage = listMessage.ToDictionary(x => x.Key, x => x.Value);

                message = new Message(dictMessage[Common.Constants.EMS_MESSAGECONTENT], dictMessage);

                message.MessageMetaDataLong = new Dictionary<string, double>();

                if (dictMessage.ContainsKey(Common.Constants.EMS_PUBLISH_DATE))
                {
                    message.MessageMetaDataLong.Add(Common.Constants.EMS_PUBLISH_DATE, Convert.ToDouble(dictMessage[Common.Constants.EMS_PUBLISH_DATE]));
                }
                if (dictMessage.ContainsKey(Common.Constants.EMS_RECEIVE_DATE))
                {
                    message.MessageMetaDataLong.Add(Common.Constants.EMS_RECEIVE_DATE, Convert.ToDouble(dictMessage[Common.Constants.EMS_RECEIVE_DATE]));
                }

                lines.RemoveAt(0);

                System.IO.File.AppendAllLines(queueFilePath, lines);
            }
            return message;
        }
    }

    [Serializable]
    public struct SerializableKeyValue<K, V>
    {
        public K Key { get; set; }
        public V Value { get; set; }
    }
}
