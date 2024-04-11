using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAF.Messaging
{
    public interface IConsumer
    {

        Message Receive(string messageChannel, long timeout);
        
    }
}
