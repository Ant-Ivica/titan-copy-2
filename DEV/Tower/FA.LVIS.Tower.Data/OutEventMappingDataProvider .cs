using System;
using System.Collections.Generic;
using System.Linq;

namespace FA.LVIS.Tower.Data
{
    public class OutEventMappingDataProvider:Core.DataProviderBase, IOutEventMappingDataProvider
    {
        public List<DataContracts.OutEventMapping> GetLVISOutEvents()
        {
            NewDBEntities.Entities dbContext = new NewDBEntities.Entities();

       

            List<DataContracts.OutEventMapping> events = new List<DataContracts.OutEventMapping>();
            
            return events;
        }

        public List<DataContracts.OutEventMapping> GetLVISLenderOutEvents(string lenderABEID)
        {
            NewDBEntities.Entities dbContext = new NewDBEntities.Entities();



            List<DataContracts.OutEventMapping> events = new List<DataContracts.OutEventMapping>();
         

            return events;
        }

        private string GetServices(string unsbscribedServiceList)
        {
            string allservices = "";

            List<string> services = new List<string>() {
                "ESCROW", "TITLE", "SIGNING"
            };

            if (!string.IsNullOrEmpty(unsbscribedServiceList))
            {
                string[] arr = unsbscribedServiceList.Split(new char[] { ';' });
                var nonIntersecting = arr.Union(services).Except(arr.Intersect(services)).ToString();

                if (arr.Length > 1)
                {
                    foreach (var t in arr)
                    {
                        services.Remove(t.Trim().ToUpper());
                    }

                    foreach (var s in services)
                    {
                        allservices += s + ";";
                    }

                    return allservices.Substring(0, allservices.Length - 1).ToString();
                }
                else
                {
                    allservices = "ALL";
                }
            }
            else
            { allservices = "ALL"; }

            return allservices;
        }
    }
}
