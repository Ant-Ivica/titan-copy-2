using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Core;

namespace FA.LVIS.Tower.Data
{
    public class DataProviderContainer : FA.LVIS.Tower.Common.IoCContainer
    {
        #region Restrict one instance of the data provider container per process/app domain
        private static DataProviderContainer _dataProviderContainer = null;

        private DataProviderContainer()
        {

        }

        private static object syncRoot = new object();

        public static DataProviderContainer Instance
        {
            get
            {
                if (_dataProviderContainer == null)
                {
                    lock (syncRoot)
                    {
                        if (_dataProviderContainer == null)
                            _dataProviderContainer = new DataProviderContainer();
                    }
                }
                return _dataProviderContainer;
            }
        }
        #endregion

        protected override void RegisterTypes()
        {
            List<Type> servicesToRegister = GetTypesToRegister(typeof(DataProviderBase));

            RegisterTypes(servicesToRegister, typeof(IDataProviderBase));
        }
    }

    public static class DataProviderFactory
    {
        public static ServiceType Resolve<ServiceType>()
        {
            return DataProviderContainer.Instance.Resolve<ServiceType>();
        }

        public static ServiceType Resolve<ServiceType>(string name)
        {
            return DataProviderContainer.Instance.Resolve<ServiceType>(name);
        }
    }
}
