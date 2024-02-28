using System;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Diagnostics;

using System.Reflection;

using FA.LVIS.Tower.Common;

namespace FA.LVIS.Tower.Core
{
    public class LoggingInterceptor : ServiceInterceptor
    {
        public override IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            try
            {
                Profiler svcProfiler = null;

                ServiceBase service = input.Target as ServiceBase;

                if (service != null && (service.Logger.IsInfoEnabled || service.Logger.IsDebugEnabled))
                    svcProfiler = new Profiler(service.Logger, input);

                svcProfiler.WriteEntryMessage();

                var returnVal = getNext.GetNext(input);

                if (returnVal.Exception == null)
                    svcProfiler.WriteExitMessage(returnVal.ReturnValue); // normal return from the service
                else
                    HandleException(service, returnVal);

                return returnVal;
            }
            catch (Exception ex)
            {
                WriteEventLogEntry(ex.ToString());
                throw;
            }
        }

        private void HandleException(ServiceBase service, IMethodReturn returnVal)
        {
            if (service != null)
                service.Logger.Error(returnVal.Exception.InnerException);
            //service.Logger.Error(returnVal.Exception.ToString() + GetCallStack());
            //else
            //{
            //    ILog logger = Logger.GetLogger(typeof(LoggingInterceptor));
            //    logger.Error(returnVal.Exception.ToString() + "\n\t" + GetCallStack());
            //}
        }

        private string GetCallStack()
        {
            string retVal = "";
            StackTrace st = new StackTrace(true);
            StackFrame[] sf = st.GetFrames();
            bool continueBuildingTrace;
            for (int i = 0; i < sf.Length; i++)
            {
                continueBuildingTrace = true;
                string methodCall = GetMethodCall(sf[i], out continueBuildingTrace);
                if (!continueBuildingTrace)
                    break;
                if (!string.IsNullOrEmpty(methodCall))
                    retVal += "\n   " + methodCall;
            }

            return retVal;
        }

        private string GetMethodCall(StackFrame stackFrame, out bool continueBuildingTrace)
        {
            MethodBase mb = stackFrame.GetMethod();
            string ret = null;

            continueBuildingTrace = true;

            if (mb.DeclaringType == typeof(System.AppDomain))
            {
                continueBuildingTrace = false;
                return ret;
            }

            if (mb.DeclaringType == typeof(FA.LVIS.Tower.Core.LoggingInterceptor) ||
                mb.DeclaringType == typeof(FA.LVIS.Tower.Core.InterceptionBehaviorExtensionMethods) ||
                mb.DeclaringType == typeof(Microsoft.Practices.Unity.InterceptionExtension.InterceptionBehaviorPipeline) ||
                mb.DeclaringType.FullName.StartsWith("DynamicModule.ns.Wrapped", StringComparison.CurrentCultureIgnoreCase))
                return ret;

            ret = "at " + mb.DeclaringType.FullName + "." + mb.Name + "(";

            ParameterInfo[] parms = mb.GetParameters();
            if (parms != null && parms.Length > 0)
            {
                ParameterInfo parm = parms[0];
                ret += parm.ParameterType.Name + " " + parm.Name;
                for(int i = 1; i < parms.Length; i++)
                    ret += "," + parm.ParameterType.Name + " " + parm.Name;
            }

            ret += ")";

            string fileName = stackFrame.GetFileName();
            if (!string.IsNullOrEmpty(fileName))
                ret = ret + " in " + fileName + ": line " + stackFrame.GetFileLineNumber().ToString();

            return ret;
        }

        private void WriteEventLogEntry(string message)
        {
            using (EventLog evtLog = new EventLog())
            {
                evtLog.Source = "Application";
                evtLog.WriteEntry(message, EventLogEntryType.Error);
            }
        }
    }
}
