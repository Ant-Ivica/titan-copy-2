using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace FA.LVIS.Tower.Core
{
    /// <summary>
    /// This just keeps conventions in one place (namely, requires an IService)
    /// </summary>
    public abstract class ServiceInterceptor : IInterceptionBehavior
    {
        #region IInterceptionBehavior Members
 
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public virtual bool WillExecute
        {
            get { return true; }
        }

        public abstract IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext);

        #endregion
    }

    public static class InterceptionBehaviorExtensionMethods
    {
        public static IMethodReturn GetNext(this GetNextInterceptionBehaviorDelegate behaviorDelegate, IMethodInvocation input)
        {
            return behaviorDelegate()(input, behaviorDelegate);
        }
    }
}
