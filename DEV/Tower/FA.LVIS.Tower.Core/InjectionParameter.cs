using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.Core
{
    public class InjectionParameter
    {
        public string ParameterName { get; set; }
        public Type ParameterType { get; set; }
        public object Parameter { get; set; }

        public InjectionParameter(string paramName, Type paramType, object param)
        {
            ParameterName = paramName;
            ParameterType = paramType;
            Parameter = param;
        }
    }
}
