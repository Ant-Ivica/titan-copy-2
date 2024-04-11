using System.Collections.Generic;
using DC = FA.LVIS.Tower.DataContracts;

namespace FA.LVIS.Tower.Services
{
   public interface ITerminalLogInformationMappingservice12 : Core.IServiceBase
    {
        List<DC.TerminalLogInformationDTO> GetTerminalLogInformationdetails(int TenantId);
    }
}
