using System;
using System.Collections.Generic;
using System.Linq;
using FA.LVIS.Tower.DataContracts;
using System.Data.Linq.SqlClient;
using FA.LVIS.Tower.Common;

namespace FA.LVIS.Tower.Data
{
    public class TerminalLogInformationDataProvider : Core.DataProviderBase, ITerminalLogInformationDataProvider
    {

        Logger sLogger = new Common.Logger(typeof(TerminalLogInformationDataProvider));

        public List<TerminalLogInformationDTO> GetTerminalLogInformationdetails(SearchDetail value)
        {
            DateTime CurrentStartDate = GetDateTime(value.Fromdate, value.StartTime);
            DateTime CurrentEndDate = GetDateTime(value.Fromdate, value.EndTime);
            try
            {

                using (TerminalDBEntities.TerminalLogEntities dbContext = new TerminalDBEntities.TerminalLogEntities())
                {
                    sLogger.Debug(string.Format($"Log@ Values passed {value.currPage} , {value.EndTime},{value.ErrorEnabled},{value.Fromdate},{value.MessageText},{value.Typecodestatus},{value.pageSize},{value.InfoEnabled},{CurrentStartDate},{CurrentEndDate}"));

                    return (dbContext.GetTermianlLog(CurrentStartDate, CurrentEndDate, value.Typecodestatus, value.ErrorEnabled, value.InfoEnabled, value.MessageText?.Replace("'", "").Replace("--", "") ?? "", value.currPage, value.pageSize).Select(x => new TerminalLogInformationDTO
                    {
                        Id = x.Id,
                        Date = x.Date.ToString("yyyy-MM-dd HH:mm:ss.fff tt"),
                        Level = x.Level,
                        logger = x.Logger,
                        Message = ReportingDataProvider.FormatXML(x.Message),
                        Excecption = x.Exception,
                        HostName = x.HostName
                    }).OrderByDescending(s => s.Date).ToList());
                }

            }
            catch (System.Exception ex)
            {
                sLogger.Error(string.Format("Call failed for GetTerminalLogInformationdetails Log@" + ex.ToString()));
                return new List<TerminalLogInformationDTO>();
            }
        }

        private DateTime GetDateTime(string date, string time)
        {
            if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time))
            {

                DateTime currDate = Convert.ToDateTime(date);
                DateTime stime = Convert.ToDateTime(time);
                DateTime dtstart = new DateTime(currDate.Year, currDate.Month, currDate.Day, stime.Hour, stime.Minute, stime.Second);
                return Convert.ToDateTime(dtstart.ToString("dd-MMM-yyyy HH:mm:ss tt"));
            }
            else
            {
                DateTime currDate = Convert.ToDateTime(date);
                DateTime stime = DateTime.Now;
                DateTime dtstart = new DateTime(currDate.Year, currDate.Month, currDate.Day, stime.Hour, stime.Minute, stime.Second);
                return Convert.ToDateTime(dtstart.ToString("dd-MMM-yyyy HH:mm:ss tt"));
            }


        }


         int ITerminalLogInformationDataProvider.GetLogDetailsCount(SearchDetail value)
        {
            string CurrentStartDate = GetDateTime(value.Fromdate, value.StartTime).ToString("yyyy-MM-dd HH:mm:ss");
            string CurrentEndDate = GetDateTime(value.Fromdate, value.EndTime).ToString("yyyy-MM-dd HH:mm:ss");
            using (TerminalDBEntities.TerminalLogEntities dbContext = new TerminalDBEntities.TerminalLogEntities())
            {
                string query = $" select count(*) from  Log where [Date] >= '{CurrentStartDate}' and [Date] <= '{CurrentEndDate}' and 	" +
                 $"(({Convert.ToInt32(value.Typecodestatus)} = 1 and[Level] = 'DEBUG') or " +
                 $"({Convert.ToInt32(value.ErrorEnabled)} = 1 and[Level] = 'ERROR') or " +
                 $"({Convert.ToInt32(value.InfoEnabled)} = 1 and[Level] = 'INFO')) and " +
                $"[Message] like '%{Encode(value.MessageText)?.Replace("'", "").Replace("--", "")}%'";
                sLogger.Debug(string.Format($"Query passed in GetLogDetailsCount Log@:{query}"));
                var count = dbContext.Database.SqlQuery<int>(query).FirstOrDefault();
                return count;
            }
        }

        private string Encode(string input) => string.IsNullOrWhiteSpace(input) ? input : System.Net.WebUtility.HtmlEncode(input);
    }
}
