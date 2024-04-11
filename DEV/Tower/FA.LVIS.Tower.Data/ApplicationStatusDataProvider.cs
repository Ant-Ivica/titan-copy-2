using System.Collections.Generic;
using System.Linq;
using FA.LVIS.Tower.DataContracts;
using FA.LVIS.Tower.Data.TerminalDBEntities;

namespace FA.LVIS.Tower.Data
{
    public class ApplicationStatusDataProvider : Core.DataProviderBase, IApplicationStatusDataProvider
    {
        Common.Logger sLogger;

        public ApplicationStatusDataProvider()
        {
            sLogger = new Common.Logger(typeof(ApplicationStatusDataProvider));
        }

        public List<EMSQueue> GetApplicationStatus()
        {
            List<EMSQueue> DetailsTobeDisplayed = new List<EMSQueue>();
            try
            {
                using (var dbContext = new Entities())
                {
                    var result = dbContext.GetApplicationHeartbeat();

                    if (result != null)
                    {
                        foreach (var se in result.ToList())
                        {
                            DetailsTobeDisplayed.Add(new EMSQueue()
                            {
                                Active = (se.IsActive == 0 ? false : true),
                                QueueName = se.ApplicationName
                            });
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                sLogger.Error("There was an error in GetConnectorStatus: " + ex.InnerException);
            }

            return DetailsTobeDisplayed;
        }
    }
}
