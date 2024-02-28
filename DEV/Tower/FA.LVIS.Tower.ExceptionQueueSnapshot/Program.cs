using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.LVIS.Tower.ExceptionQueueSnapshot
{
    class Program
    {
        static void Main(string[] args)
        {
            ExceptionSnapShot Snapshot = new ExceptionSnapShot();
            Snapshot.CreateTEQHourlySnapshot();
            Snapshot.CreateBEQHourlySnapshot();
        }
    }
}
