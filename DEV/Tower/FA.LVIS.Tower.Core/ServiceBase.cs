using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Common;
using FA.LVIS.Tower.Services;

namespace FA.LVIS.Tower.Core
{
    /// <summary>
    /// IService is a marker interface to allow for auto-registration of types in a service library
    /// </summary>
    public interface IServiceBase
    {
    }

    public abstract class ServiceBase : IServiceBase
    {
        private ILogger _logger;
        //private ServiceContext _serviceContext;

        public ILogger Logger
        {
            get { return _logger; }
        }

        public ServiceBase()
        {
            _logger = ServiceFactory.Resolve<ILogger>(new Core.InjectionParameter[] { new Core.InjectionParameter("type", typeof(Type), this.GetType()) });
        }
    }
}
