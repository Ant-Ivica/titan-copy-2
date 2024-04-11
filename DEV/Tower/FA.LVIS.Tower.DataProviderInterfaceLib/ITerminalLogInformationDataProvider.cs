using FA.LVIS.Tower.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Data
{
    public interface ITerminalLogInformationDataProvider : IDataProviderBase
    {
        List<DC.TerminalLogInformationDTO> GetTerminalLogInformationdetails(DC.SearchDetail value);
        int GetLogDetailsCount(DC.SearchDetail value);
        //List<DC.TerminalLogInformationDTO> GetLogDetails();

    }
}
