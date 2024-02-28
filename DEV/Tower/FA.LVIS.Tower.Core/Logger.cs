using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Common
{
    public class Logger : ILogger
    {
        private log4net.ILog logProvider = null;

        #region Constructors
        public Logger()
        {
            logProvider = log4net.LogManager.GetLogger(GetCurrentCallerType());
        }

        /// <summary>
        /// Create logger for a specific Type.
        /// </summary>
        /// <param name="type">The full name of type will used as the name of the logger to retrieve</param>
        public Logger(Type type)
        {
            logProvider = log4net.LogManager.GetLogger(type);
        }

        private static Type GetCurrentCallerType()
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(false);

            Type currentType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
            Type loggerType = currentType;
            int frames = stackTrace.FrameCount;
            // First 2 frames are within this class itself, so do not go through them
            for (int i = 2; i < frames; i++)
            {
                Type declaringType = stackTrace.GetFrame(i).GetMethod().DeclaringType;
                if (declaringType.Equals(currentType) == false)
                {
                    loggerType = declaringType;
                    break;
                }
            }
            return loggerType;
        }

        static Logger()
        {
            string log4netConfigFile = System.Configuration.ConfigurationManager.AppSettings["log4net-config-file"];
            if (!string.IsNullOrEmpty(log4netConfigFile))
            {
                System.IO.FileInfo configFileInfo = new System.IO.FileInfo(log4netConfigFile);
                log4net.Config.XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }
        }
        #endregion

        public bool IsDebugEnabled
        {
            get { return logProvider.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return logProvider.IsErrorEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return logProvider.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return logProvider.IsWarnEnabled; }
        }

        public void Debug(object message)
        {
            logProvider.Debug(message);
        }

        public void Error(object message)
        {
            logProvider.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            logProvider.Error(message, exception);
        }

        public void Info(object message)
        {
            logProvider.Info(message);
        }

        public void Warn(object message)
        {
            logProvider.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            logProvider.Warn(message, exception);
        }
    }
}
