using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Collections;

namespace FA.LVIS.Tower.Common
{
    internal class Profiler
    {
        private IProfiler _profiler;
        private ILogger _logger;

        internal Profiler(ILogger logger, IMethodInvocation input)
        {
            _logger = logger;
            if (logger.IsDebugEnabled)
                _profiler = new DebugProfiler(input);
            else if (logger.IsInfoEnabled)
                _profiler = new InfoProfiler(input);
        }

        internal void LogEntryMessage()
        {
            _profiler.LogEntryMessage(_logger);
        }

        internal void LogExitMessage(object returnVal)
        {
            _profiler.LogExitMessage(_logger, returnVal);
        }
    }

    internal static class ProfilerExtensions
    {
        internal static void WriteEntryMessage(this Profiler currentProfiler)
        {
            if (currentProfiler != null)
                currentProfiler.LogEntryMessage();
        }

        internal static void WriteExitMessage(this Profiler currentProfiler, object returnVal)
        {
            if (currentProfiler != null)
                currentProfiler.LogExitMessage(returnVal);
        }
    }

    public interface IProfiler
    {
        void LogEntryMessage(ILogger logger);
        void LogExitMessage(ILogger logger, object returnVal);
    }

    public class InfoProfiler : IProfiler
    {
        private DateTime _startTime;
        private DateTime _endTime;
        private string _className;
        private string _methodName;

        public InfoProfiler(IMethodInvocation input)
        {
            _startTime = DateTime.Now;
            _className = input.Target.GetType().FullName;
            _methodName = input.MethodBase.Name;
        }

        public void LogEntryMessage(ILogger logger)
        {
            logger.Info(string.Format("Entered {0}::{1}", _className, _methodName));
        }

        public void LogExitMessage(ILogger logger, object returnVal)
        {
            _endTime = DateTime.Now;
            logger.Info(string.Format("Exited {0}::{1}, Time Taken: {2}", _className, _methodName, (_endTime - _startTime).TotalMilliseconds));
        }
    }

    public class DebugProfiler : IProfiler
    {
        private DateTime _startTime;
        private DateTime _endTime;
        private string _className;
        private string _methodName;
        private IParameterCollection _params;

        public DebugProfiler(IMethodInvocation input)
        {
            _startTime = DateTime.Now;
            _className = input.Target.GetType().FullName;
            _methodName = input.MethodBase.Name;
            _params = input.Arguments;
        }

        public void LogEntryMessage(ILogger logger)
        {
            string paramInfo = "";
            ParameterCollection x = (ParameterCollection)_params;
            for (int i = 0; i < x.Count; i++)
            {
                paramInfo += x.ParameterName(i) + ":" + x[i] + "; ";
            }
            logger.Debug(string.Format("Entered {0}::{1}, \n\t\tParameters: {2}", _className, _methodName, paramInfo));
        }

        public void LogExitMessage(ILogger logger, object returnVal)
        {
            _endTime = DateTime.Now;
            logger.Debug(string.Format("Exited {0}::{1}, Time Taken: {2}", _className, _methodName, (_endTime - _startTime).TotalMilliseconds));

            //if (returnVal is IEnumerable)
            //{
            //    StringBuilder outputBuffer = new StringBuilder();
            //    PrintCollectionItems((IEnumerable)returnVal, outputBuffer);
            //    logger.Debug(string.Format("Exited {0}::{1}, Time Taken: {2}, \n\t\tReturned: {3}", _className, _methodName, (_endTime - _startTime).TotalMilliseconds, outputBuffer.ToString()));
            //}
            //else
            //{
            //    logger.Debug(string.Format("Exited {0}::{1}, Time Taken: {2}, \n\t\tReturned: {3}", _className, _methodName, (_endTime - _startTime).TotalMilliseconds, returnVal.ToString()));
            //}
        }

        private void PrintCollectionItems(IEnumerable retParam, StringBuilder outputBuffer)
        {
            foreach (var x in retParam)
            {
                outputBuffer.Append(x.ToString());
                outputBuffer.Append(System.Environment.NewLine);
            }
        }
    }
}
