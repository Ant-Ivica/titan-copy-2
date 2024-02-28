using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Common;
using FA.LVIS.Tower.Services;

namespace FA.LVIS.Tower.Core
{
    public interface IDataProviderBase
    {
    }

    public abstract class DataProviderBase : IDataProviderBase
    {
        private ILogger _logger;
        //private ServiceContext _serviceContext;

        public ILogger Logger
        {
            get { return _logger; }
        }

        public DataProviderBase()
        {
            _logger = ServiceFactory.Resolve<ILogger>(new Core.InjectionParameter[] { new Core.InjectionParameter("type", typeof(Type), this.GetType()) });

        }
    }
}
