using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Reflection;
using System.IO;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace FA.LVIS.Tower.Common
{
    public abstract class IoCContainer
    {
        private IUnityContainer _unityContainer;

        public IoCContainer()
        {
            _unityContainer = new UnityContainer();
            _unityContainer.AddNewExtension<Interception>();

            if (!_unityContainer.IsRegistered<ILogger>())   // move to actual container implementation
                _unityContainer.RegisterType(typeof(ILogger), typeof(Logger));

            RegisterTypes(); // template method...actual specialized classes should implement. Useful if there are multiple containers in the application

            //_unityContainer.LoadConfiguration(); // TODO: parameterize
        }

        protected abstract void RegisterTypes(); // template method for the actual specialized entities to implement

        #region auto identifaction / registration of types from application
        protected List<Type> GetTypesToRegister(Type superClassType)
        {
            List<Type> typesToRegister = new List<Type>();

            //TODO: make sure the below code can get the current folder information in a consistent fashion (console app, web app, visual studio debugging etc)
            string currentPath = Assembly.GetExecutingAssembly().CodeBase;
            currentPath = (currentPath.IndexOf("file:///") >= 0) ? currentPath.Replace("file:///", "") : currentPath;
            currentPath = currentPath.Substring(0, currentPath.LastIndexOf('/'));

            string[] appAssemblies = Directory.GetFiles(currentPath, "FA.LVIS.Tower.*.dll", SearchOption.TopDirectoryOnly);

            for (int i = 0, noOfAssemblies = appAssemblies.Length; i < noOfAssemblies; i++)
            {
                Assembly assembly = Assembly.LoadFrom(appAssemblies[i]);
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(superClassType))
                        typesToRegister.Add(type);
                }
            }

            return typesToRegister;
        }

        protected void RegisterTypes(List<Type> servicesToRegister, Type rootInterfaceType)
        {
            RegisterTypes(servicesToRegister, rootInterfaceType, null, null);
        }

        protected void RegisterTypes(List<Type> servicesToRegister, Type rootInterfaceType, Interceptor interceptor, InterceptionBehavior behavior)
        {
            foreach (Type serviceToRegister in servicesToRegister)
            {
                Type[] supportedInterfaces = serviceToRegister.GetInterfaces();

                if (supportedInterfaces != null && supportedInterfaces.Length > 0)
                {
                    for (int i = 0; i < supportedInterfaces.Length; i++)
                    {
                        Type interfaceType = supportedInterfaces[i];
                        if (interfaceType == rootInterfaceType)
                            continue;
                        if (!rootInterfaceType.IsAssignableFrom(interfaceType))
                            continue;

                        if (interfaceType.Name.Substring(1) == serviceToRegister.Name)
                        {
                            // default service
                            if (!IsRegistered(interfaceType))
                                RegisterType(interfaceType, serviceToRegister, interceptor, behavior);
                        }
                        else
                        {
                            // register with a different name
                            if (!IsRegistered(interfaceType, serviceToRegister.Name))
                                RegisterType(interfaceType, serviceToRegister, serviceToRegister.Name, interceptor, behavior);
                        }
                    }
                }
            }
        }
        #endregion

        public bool IsRegistered(Type type)
        {
            return _unityContainer.IsRegistered(type);
        }

        public bool IsRegistered(Type type, string name)
        {
            return _unityContainer.IsRegistered(type, name);
        }

        public bool IsRegistered<T>()
        {
            return _unityContainer.IsRegistered<T>();
        }

        public IoCContainer RegisterType(Type from, Type to)
        {
            _unityContainer.RegisterType(from, to);
            return this;
        }

        public IoCContainer RegisterType(Type from, Type to, string name)
        {
            _unityContainer.RegisterType(from, to, name);
            return this;
        }

        public IoCContainer RegisterType(Type from, Type to, Interceptor interceptor, InterceptionBehavior behavior)
        {
            if (interceptor != null && behavior != null)
                _unityContainer.RegisterType(from, to, interceptor, behavior);
            else
                _unityContainer.RegisterType(from, to);

            return this;
        }

        public IoCContainer RegisterType(Type from, Type to, string name, Interceptor interceptor, InterceptionBehavior behavior)
        {
            if (interceptor != null && behavior != null)
                _unityContainer.RegisterType(from, to, name);
            else
                _unityContainer.RegisterType(from, to, name);

            return this;
        }

        public ServiceType Resolve<ServiceType>(params Core.InjectionParameter[] injectionParameters)
        {
            ParameterOverride[] paramOverrides = GetUnityInjectionParameters(injectionParameters);

            if (_unityContainer.IsRegistered<ServiceType>())
            {
                if (paramOverrides == null)
                    return _unityContainer.Resolve<ServiceType>();
                else
                    return _unityContainer.Resolve<ServiceType>(paramOverrides);
            }
            else
                return default(ServiceType);
        }

        public ServiceType Resolve<ServiceType>(string name, params Core.InjectionParameter[] injectionParameters)
        {
            ParameterOverride[] paramOverrides = GetUnityInjectionParameters(injectionParameters);

            if (_unityContainer.IsRegistered<ServiceType>(name))
            {
                if (paramOverrides == null)
                    return _unityContainer.Resolve<ServiceType>(name);
                else
                    return _unityContainer.Resolve<ServiceType>(name, paramOverrides);
            }
            else
                return default(ServiceType);
        }

        private ParameterOverride[] GetUnityInjectionParameters(Core.InjectionParameter[] injectionParameters)
        {
            if (injectionParameters == null || injectionParameters.Length == 0)
                return null;

            List<ParameterOverride> parameterOverrides = new List<ParameterOverride>();
            foreach (Core.InjectionParameter injectParam in injectionParameters)
            {
                parameterOverrides.Add(new ParameterOverride(injectParam.ParameterName,
                    new InjectionParameter(injectParam.ParameterType, injectParam.Parameter)));
            }

            return parameterOverrides.ToArray();
        }
    }
}
