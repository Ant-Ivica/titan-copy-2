using System.Collections.Generic;

using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
   public interface ITerminalLogInformationMappingservice :Core.IServiceBase
    {
        List<DC.TerminalLogInformationDTO> GetTerminalLogInformationdetails(DC.SearchDetail value);
        int GetLogDetailsCount(DC.SearchDetail value);
        //List<DC.TerminalLogInformationDTO> GetLogDetails();



    }


}
