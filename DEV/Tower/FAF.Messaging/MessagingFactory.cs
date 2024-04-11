using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;
using System.IO;

namespace FAF.Messaging
{
    public class MessagingFactory
    {
        public IPublisher CreatePublisher() 
        {
            var implementationConfig = ConfigurationManager.AppSettings["messaging_publisher"].Split(',');
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var obj = Assembly.LoadFile(path + "\\" + implementationConfig[0]).CreateInstance(implementationConfig[1]);
            return (IPublisher)(obj);
        }

        public IConsumer CreateConsumer()
        {
            var implementationConfig = ConfigurationManager.AppSettings["messaging_consumer"].Split(',');
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var obj = Assembly.LoadFile(path + "\\" + implementationConfig[0]).CreateInstance(implementationConfig[1]);
            return (IConsumer)(obj);
        }

    }
}
