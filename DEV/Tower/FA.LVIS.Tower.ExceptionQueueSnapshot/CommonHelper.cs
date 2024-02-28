using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.ExceptionQueueSnapshot
{    
    public enum ExceptionStatusEnum
    {
        New = 201,
        Active,
        Hold,
        Resolved
    }
    
    public enum ExceptionGroupEnum
    {
        BEQ = 1,
        TEQ
    }
    public enum ApplicationEnum
    {
        LVIS = 1,
        TesterApp = 2,
        LenderSimulator = 3,
        RealEC = 4,
        FAST = 5,
        ELS = 6,
        WinTrack = 7,
        FACC = 8
    }
}
