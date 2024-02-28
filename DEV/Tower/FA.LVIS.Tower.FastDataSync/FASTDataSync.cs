using System;
using System.Linq;
using System.Text;
using System.Configuration;
using FA.LVIS.Tower.FastDataSync.FastAdminService;
using System.Security.Cryptography;
using LVIS.Common;
using LVIS.Infrastructure.Logging;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using FA.LVIS.CommonHelper;

namespace FA.LVIS.Tower.FastDataSync
{
    public class FASTDataSync
    {
        private const string FAST_ACCESS_USER = "FASTAccessUser";
        private bool s_bAccessUserOverride = false;
        private string s_AccessUserDomain = "fastts";
        private string s_AccessUser = null;
        private byte[] s_AccessSecret = null;
        private int s_AccessPwdLength = 0;
        private StringBuilder queryqueue = new StringBuilder();
        private ILogger sLogger = LoggerFactory.GetLogger(typeof(FASTDataSync));

        public FASTDataSync(ILogger sLogger)
        {
            try
            {
                String Fastuser = ConfigurationManager.AppSettings[FAST_ACCESS_USER];

                if (Fastuser != null)
                {
                    string[] ss = Fastuser.Split(';');
                    s_AccessUser = ss[0].Trim().Decrypt();
                    if (ss.Length > 1)
                    {
                        s_AccessUser = ss[0].Replace('\\', '/');
                        if (s_AccessUser.Contains('/'))
                        {
                            string[] s2 = s_AccessUser.Split('/');
                            s_AccessUser = s2[1].Trim();
                            s_AccessUserDomain = s2[0].Trim();
                        }

                        var s_AccessPwd =ss[1].Trim().Decrypt();
                        s_bAccessUserOverride = true;

                        // Set                                              
                        s_AccessPwdLength = s_AccessPwd.Length;
                        IUtils sUtils = new Utils();
                        s_AccessSecret = sUtils.Return16BytesChar(s_AccessPwd);
                        ProtectedMemory.Protect(s_AccessSecret, MemoryProtectionScope.SameLogon);
                    }
                }
            }
            catch (Exception ex)
            {
                sLogger.Error(ex);
            }
        }

        private FastAdminServiceClient GetFASTAdminService()
        {
            FastAdminServiceClient ws = new FastAdminServiceClient("MainAdminServiceAccess");

            if (s_bAccessUserOverride)
            {
                //Changes incorporate because of Security issue raised by Veracode Scan
                if (s_AccessPwdLength == 0)
                    return null;
                ProtectedMemory.Unprotect(s_AccessSecret, MemoryProtectionScope.SameLogon);

                if (ConfigurationManager.AppSettings["IsBasic"] == "1")
                    ws.Endpoint.EndpointBehaviors.Add(new CustomBehavior());

                ws.ClientCredentials.Windows.ClientCredential.Domain = s_AccessUserDomain.Decrypt();
                ws.ClientCredentials.Windows.ClientCredential.UserName = s_AccessUser;
                ws.ClientCredentials.Windows.ClientCredential.Password = UnicodeEncoding.ASCII.GetString(s_AccessSecret).Substring(0, s_AccessPwdLength);
            }
            return ws;
        }

        public void SyncFastRegions()
        {
            Console.WriteLine("Starting Regions...");  
           sLogger.Info("Starting Regions...");


            FastAdminServiceClient s = GetFASTAdminService();
            sLogger.Info("Created FAST Admin Sercie Client");

            using (TerminalEntities dbcontext = new TerminalEntities())
            {
                using (var dbdatasynctransaction = dbcontext.Database.BeginTransaction())
                {
                    try
                    {
                        sLogger.Info("Started Transcatoin");

                        dbcontext.Configuration.AutoDetectChangesEnabled = false;

                        // Delete Regions and Fastoffices
                        sLogger.Info("Begin DELETE FROM FASTOffice");
                        dbcontext.Database.ExecuteSqlCommand("DELETE FROM FASTOffice");
                        sLogger.Info("End DELETE FROM FASTOffice");
                        sLogger.Info("Begin DELETE FROM FASTRegion");
                        dbcontext.Database.ExecuteSqlCommand("DELETE FROM FASTRegion");
                        sLogger.Info("End DELETE FROM FASTRegion");
                        sLogger.Info("Begin DELETE FROM FastProgramType");
                        dbcontext.Database.ExecuteSqlCommand("DELETE FROM FastProgramType");
                        sLogger.Info("End DELETE FROM FASTRegion");

                        foreach (var item in s.GetRegions(1).BusUnits)
                        {
                            dbcontext.FASTRegions.Add(new FASTRegion() { RegionID = item.BusinessUnitID.Value, Name = item.Name, ApplicationID = 5 });
                            sLogger.Info("Adding new FAST Region Data - RegionID: " + item.BusinessUnitID.Value);

                            BusUnitResponse FastOffices = s.GetOffices(item.BusinessUnitID.Value);
                            Console.WriteLine("Updating Offices for Region  " + item.BusinessUnitID.Value + "...");

                            if (FastOffices != null && FastOffices.BusUnits.Count() > 0)
                            {
                                foreach (var office in FastOffices.BusUnits)
                                {
                                    if (office.BusinessUnitID.HasValue && office.Name.ToUpper().Trim() != "ANY")
                                    {
                                        string State = string.Empty;
                                        string County = string.Empty;

                                        try
                                        {
                                            var Addrofficeinfo = s.GetOfficeAddresses(office.BusinessUnitID.Value);

                                            if (Addrofficeinfo.OfficeAddresses != null && Addrofficeinfo.OfficeAddresses.Count() > 0)
                                            {
                                                var officewithStateCounty = Addrofficeinfo.OfficeAddresses.Where(se => se.State != string.Empty && se.County != string.Empty).FirstOrDefault();
                                                if (officewithStateCounty == null)
                                                    officewithStateCounty = Addrofficeinfo.OfficeAddresses.Where(se => se.State != string.Empty).FirstOrDefault();
                                                State = officewithStateCounty?.State;
                                                County = officewithStateCounty?.County;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            sLogger.Error(ex);
                                        }

                                        FASTOffice Off = new FASTOffice() { RegionID = item.BusinessUnitID.Value, OfficeName = office.Name, OfficeID = office.BusinessUnitID.Value, State = State, County = County };

                                        dbcontext.FASTOffices.Add(Off);
                                        Console.WriteLine("Adding new FAST Offices Data - RegionID: " + item.BusinessUnitID.Value + "...");
                                        sLogger.Debug("Adding new FAST Offices Data - RegionID: " + item.BusinessUnitID.Value + ", OfficeID: " + office.BusinessUnitID.Value);
                                    }
                                }
                            }

                            ProgramTypeResponse FastProgramTypes = s.GetProgramTypes(item.BusinessUnitID.Value);
                            Console.WriteLine("Updating programtypes for Region  " + item.BusinessUnitID.Value + "...");
                            sLogger.Info("Updating programtypes for Region: " + item.BusinessUnitID.Value);

                            if (FastProgramTypes.ProgramTypes != null && FastProgramTypes.ProgramTypes.Count() > 0)
                            {
                                foreach (var pType in FastProgramTypes.ProgramTypes)
                                {
                                    if (pType.ProgramTypeID.HasValue && pType.ProgramTypeName.ToUpper().Trim() != "ANY")
                                    {
                                        FASTProgramType PgmType = new FASTProgramType() { RegionId = item.BusinessUnitID.Value, FASTProgramTypeId = pType.ProgramTypeID.Value, ProgramTypeName = pType.ProgramTypeName, CreatedById = 1, LastModifiedById = 1, CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now };

                                        dbcontext.FASTProgramTypes.Add(PgmType);
                                        sLogger.Debug("Adding new FAST ProgramTypes Data - RegionID: " + item.BusinessUnitID.Value + ", ProgramTypeID: " + pType.ProgramTypeID.Value);

                                    }
                                }
                            }
                        }

                        sLogger.Info("Saving Changes to DB");
                        dbcontext.SaveChanges();
                        dbdatasynctransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sLogger.Error($"Exception occured with exception message : {ex.Message}");

                        dbdatasynctransaction.Rollback();
                        sLogger.Debug($"Rollback Complted");
                        SendEmail(ex);
                        sLogger.Debug($"End Mail sent");
                        EventLog.WriteEntry("FastDataSync", ex.Message + "Exception" + ex.InnerException, EventLogEntryType.Error, 121, short.MaxValue);
                    }
                }

            }

        }

        public void SendEmail(Exception Exception)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {

                    //Get From and To address mailing list
                    mail.To.Add(ConfigurationManager.AppSettings["MailingList"]);
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["FromEmail"]);
                    mail.Subject = ConfigurationManager.AppSettings["Subject"];
                    mail.Priority = MailPriority.High;

                    //Get host name
                    string hostname = "";
                    try
                    { hostname = Dns.GetHostEntry(Dns.GetHostName()).HostName; }
                    catch { hostname = ""; }

                    //Create body message
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<html><body>");
                    sb.AppendLine("<b>Error Message : <br/></b><i>" + Exception.Message + "</i><br/><br/>");
                    sb.AppendLine("<b>Host Name : <br/></b><i>" + hostname + "</i><br/><br/>");
                    sb.AppendLine("<b>Inner Exception : <br/></b><i>" + Exception.InnerException + "</i><br/><br/>");
                    sb.AppendLine("</body></html>");

                    mail.Body = sb.ToString();
                    mail.IsBodyHtml = true;

                    //Send email
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = ConfigurationManager.AppSettings["SMTP"];
                    smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["USESSL"]);
                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                sLogger.Info("Error sending email. " + ex.Message);
            }
        }

    }
}

