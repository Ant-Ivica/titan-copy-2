using System;

namespace FA.LVIS.Tower.Common
{
    public interface ILogger
    {
        bool IsDebugEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }

        void Debug(object message);

        void Error(object message);
        void Error(object message, Exception exception);

        void Info(object message);

        void Warn(object message);
        void Warn(object message, Exception exception);
    }
}
