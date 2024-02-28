using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FA.LVIS.Tower.Core;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public interface IApplicationStatusDataProvider : IDataProviderBase
    {
        List<DC.EMSQueue> GetApplicationStatus();
    }
}
