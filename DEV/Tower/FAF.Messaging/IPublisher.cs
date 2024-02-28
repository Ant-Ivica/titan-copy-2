using System;
namespace FAF.Messaging
{
    public interface IPublisher
    {
        void Send(string messageChannel, Message message);
    }
}
