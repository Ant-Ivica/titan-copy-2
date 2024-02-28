using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Core;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace FA.LVIS.Tower.Services
{
    public class ServicesContainer : FA.LVIS.Tower.Common.IoCContainer
    {
        #region Restrict one instance of the services container per process/app domain
        private static ServicesContainer _servicesContainer = null;

        private ServicesContainer()
        {

        }

        private static object syncRoot = new object();

        public static ServicesContainer Instance
        {
            get
            {
                if (_servicesContainer == null)
                {
                    lock (syncRoot)
                    {
                        if (_servicesContainer == null)
                            _servicesContainer = new ServicesContainer();
                    }
                }
                return _servicesContainer;
            }
        }
        #endregion

        protected override void RegisterTypes()
        {
            Interceptor typeofInterceptor = new Interceptor<InterfaceInterceptor>();
            InterceptionBehavior loggingBehavior = new InterceptionBehavior<LoggingInterceptor>();

            List<Type> servicesToRegister = GetTypesToRegister(typeof(ServiceBase));

            RegisterTypes(servicesToRegister, typeof(IServiceBase), typeofInterceptor, loggingBehavior);
        }
    }

    public static class ServiceFactory
    {
        public static ServiceType Resolve<ServiceType>()
        {
            return ServicesContainer.Instance.Resolve<ServiceType>();
        }

        public static ServiceType Resolve<ServiceType>(string name)
        {
            return ServicesContainer.Instance.Resolve<ServiceType>(name);
        }

        public static ServiceType Resolve<ServiceType>(Core.InjectionParameter[] injectionParameters)
        {
            return ServicesContainer.Instance.Resolve<ServiceType>(injectionParameters);
        }
    }
}
