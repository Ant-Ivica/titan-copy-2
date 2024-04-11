using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FA.LVIS.Tower.DataContracts
{
    public class TerminalLogInformationDTO : DataContractBase
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string Level {get ;set ;}

        public string logger { get; set; }

        public string Message { get; set; }

        public string Excecption { get; set; }

        public string displaydate { get; set; }

        public string HostName { get; set; }
    }
}
