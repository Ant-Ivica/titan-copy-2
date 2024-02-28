using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.DataContracts
{
    public class FastAppInfoDTO
    {
        public FastAppInfoDTO(string appDescription,string fastEnvironment,string applicationName,bool mismatchError=false)
        {
            AppDescription = appDescription;
            FastEnvironment = fastEnvironment;
            ApplicationName = applicationName;
            MismatchError = mismatchError;
        }
        public string AppDescription { get; set; }
        public string FastEnvironment { get; set; }
        public bool MismatchError  { get; set; }
        public string ApplicationName { get; set; }

    }

    public class FastAppDetailsDTO
    {
        public string Server { get; set; }
        public List<FastAppInfoDTO> FastEnvDetails { get; set; } = new List<FastAppInfoDTO>();
        public bool EnableFastInfo { get; set; }
    }
}
