using DC = FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Common;
using FA.LVIS.Tower.Data;
using System.Collections.Generic;
using System;

namespace FA.LVIS.Tower.Services
{
    public class TerminalLogInformationMappingservice : Core.ServiceBase, ITerminalLogInformationMappingservice
    {
        //public List<DC.TerminalLogInformationDTO> GetLogDetails()
        //{
        //    ITerminalLogInformationDataProvider LogInformations = DataProviderFactory.Resolve<ITerminalLogInformationDataProvider>();
        //    return LogInformations.GetTerminalLogInformationdetails();
        //}
        Logger sLogger = new Common.Logger(typeof(TerminalLogInformationMappingservice));
        public List<DC.TerminalLogInformationDTO> GetTerminalLogInformationdetails(DC.SearchDetail value)
        {
            ITerminalLogInformationDataProvider LogInformations = DataProviderFactory.Resolve<ITerminalLogInformationDataProvider>();
            sLogger.Debug(string.Format($"IN GetTerminalLogInformationdetails Log@ Values passed {value.currPage} , {value.EndTime},{value.ErrorEnabled},{value.Fromdate},{value.MessageText}"));
            return LogInformations.GetTerminalLogInformationdetails(value);
        }

        public int GetLogDetailsCount(DC.SearchDetail value)
        {
            ITerminalLogInformationDataProvider LogInformations = DataProviderFactory.Resolve<ITerminalLogInformationDataProvider>();
            return LogInformations.GetLogDetailsCount(value);


        }



    }
}
