using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data;
using System.Collections.Generic;


namespace FA.LVIS.Tower.Services
{
    public class TerminalLogInformationMappingservice12 : Core.ServiceBase, ITerminalLogInformationMappingservice12
    {
        public List<DC.TerminalLogInformationDTO> GetTerminalLogInformationdetails(int TenantId)
        {
            ITerminalLogInformationMappingservice12 Logservice = DataProviderFactory.Resolve<ITerminalLogInformationMappingservice12>();
            return Logservice.GetTerminalLogInformationdetails(TenantId);
        }        
}
}

