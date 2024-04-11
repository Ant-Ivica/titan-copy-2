using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.DataContracts;
using LVIS.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FA.LVIS.Tower.Data
{
    public class CustomerRegistrationDataProvider : Core.DataProviderBase, ICustomerRegistrationDataProvider
    {
        public CustomerRegistrationDTO AddCustomerRegistrationDTO(CustomerRegistrationDTO value, int tenantId, int userId)
        {
            using (Entities dbContext = new Entities())
            {
                bool emailFlag = false;
                if (value.CustomerRegistrationId == 0)
                {
                    TerminalDBEntities.CustomerRegistration LocationtoAdd = new TerminalDBEntities.CustomerRegistration();
                    LocationtoAdd.FirstName = value.FirstName;
                    LocationtoAdd.LastName = value.LastName;
                    LocationtoAdd.PhoneNo = value.PhoneNo;
                    LocationtoAdd.EmailId = value.EmailId;
                    LocationtoAdd.CompanyName = value.CompanyName;
                    LocationtoAdd.ProjectName = value.ProjectName;
                    LocationtoAdd.TitleAndSettlement = value.TitleAndSettlement;
                    LocationtoAdd.IneractiveOfficeDirectory = value.IneractiveOfficeDirectory;
                    LocationtoAdd.Other = value.Other;
                    LocationtoAdd.Comments = value.Comments;
                    LocationtoAdd.OtherRequirement = value.OtherRequirement;
                    LocationtoAdd.CustomerStatusId = value.CustomerStatus;
                    LocationtoAdd.CreatedId = userId;
                    LocationtoAdd.CreatedDate = DateTime.Now;
                    LocationtoAdd.LastModifiedBy = userId;
                    LocationtoAdd.LastModifiedDate = DateTime.Now;
                    dbContext.CustomerRegistrations.Add(LocationtoAdd);
                    int Success = AuditLogHelper.SaveChanges(dbContext);
                    value.CustomerRegistrationId = LocationtoAdd.CustomerRegistrationId;
                    value.CustomerStatusName = dbContext.TypeCodes.Where(x => x.TypeCodeId == value.CustomerStatus).Select(x => x.TypeCodeDesc).FirstOrDefault();
                    if (value.CustomerStatus == 5005)
                    {
                        Tuple<string, string> mailBodySubject = GetMailBody(value);
                        SendEmail(mailBodySubject.Item1, mailBodySubject.Item2);
                    }
                }


                else
                {
                    var Update = (from branch in dbContext.CustomerRegistrations
                                  where branch.CustomerRegistrationId == value.CustomerRegistrationId
                                  select branch).FirstOrDefault();
                    if ((Update.CustomerStatusId != value.CustomerStatus) && (value.CustomerStatus == 5004 || value.CustomerStatus == 5005))
                    {
                        emailFlag = true;
                    }

                    if (Update != null)
                    {
                        Update.FirstName = value.FirstName;
                        Update.LastName = value.LastName;
                        Update.PhoneNo = value.PhoneNo;
                        Update.EmailId = value.EmailId;
                        Update.CompanyName = value.CompanyName;
                        Update.ProjectName = value.ProjectName;
                        Update.TitleAndSettlement = value.TitleAndSettlement;
                        Update.IneractiveOfficeDirectory = value.IneractiveOfficeDirectory;
                        Update.Other = value.Other;
                        Update.Comments = value.Comments;
                        Update.OtherRequirement = value.OtherRequirement;
                        Update.CustomerStatusId = value.CustomerStatus;
                        Update.LastModifiedBy = userId;
                        Update.LastModifiedDate = DateTime.Now;
                        dbContext.Entry(Update).State = System.Data.Entity.EntityState.Modified;
                        int Success = AuditLogHelper.SaveChanges(dbContext);
                        value.CustomerStatusName = dbContext.TypeCodes.Where(x => x.TypeCodeId == value.CustomerStatus).Select(x => x.TypeCodeDesc).FirstOrDefault();

                    }
                    if (emailFlag)
                    {
                        Tuple<string, string> mailBodySubject = GetMailBody(value);

                        SendEmail(mailBodySubject.Item1, mailBodySubject.Item2);
                    }

                }




            }
            return value;
        }

        public Tuple<string, string> GetMailBody(CustomerRegistrationDTO value)
        {

            string subject = string.Empty;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat(@"<head>
                                    <meta http-equiv=""Content - Type"" content=""text / html; charset = utf - 8""><style>
                                    .NormalText {{
                                    font - family: Calibri;
                                    font - size:11PT;
                                    }}
                                    </style>
                                    </head><div class=""NormalText"">
                                    Hello LVIS Dev Lead,<br><br>");
            if (value.CustomerStatus == 5005)
            {
                mailBody.AppendFormat("You have a new request to contact and share Open API documents. Below is the information for requesting party.<br>");
                subject = "LVIS API Access Request - Approve";
            }
            else if (value.CustomerStatus == 5004)
            {
                mailBody.AppendFormat("You have a new request to deactivate Open API customer.<br>Below is the Customer that need to be deactivated:<br>");
                subject = "LVIS API Access Request - Deactivate";
            }
            mailBody.AppendFormat("<br>First Name: {0}", value.FirstName);
            mailBody.AppendFormat("<br>Last Name: {0}", value.LastName);
            mailBody.AppendFormat("<br>Email: {0}", value.EmailId);
            if (!string.IsNullOrEmpty(value.PhoneNo))
            {
                mailBody.AppendFormat("<br>Phone: {0}", GetFormattedPhoneNumber(value.PhoneNo));
            }
            mailBody.AppendFormat("<br>Company Name: {0}", value.CompanyName);
            if (!string.IsNullOrEmpty(value.ProjectName)) mailBody.AppendFormat("<br>Project Name: {0}", value.ProjectName);
            if (value.Other) mailBody.AppendFormat("<br>Project's API requirements: {0}", value.Other);
            if (!string.IsNullOrEmpty(value.OtherRequirement)) mailBody.AppendFormat("<br>Other requirements: {0}", value.OtherRequirement);
            if (!string.IsNullOrEmpty(value.Comments)) mailBody.AppendFormat("<br>Comments: {0}", value.Comments);
            mailBody.AppendFormat("<br><br>Thank you,<br>LVIS Support Team");
            Tuple<string, string> mailBodySubject = new Tuple<string, string>(mailBody.ToString(), subject);
            return mailBodySubject;
        }

        public string GetFormattedPhoneNumber(string phoneNo)
        {
            if (phoneNo.Length == 11)
            {
                return Regex.Replace(phoneNo, @"(\d{1})(\d{3})(\d{3})(\d{4})", "$1 ($2) $3-$4");
            }
            return Regex.Replace(phoneNo, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
        }

        public void SendEmail(string mailbody, string subject)
        {

            EmailHelper.SendEmail(ConfigurationManager.AppSettings["SenderMailAddress"], ConfigurationManager.AppSettings["Recipient"], subject, mailbody, null, null, null);
        }

        public List<CustomerRegistrationDTO> GetCustomerRegistrations()
        {
            List<DataContracts.CustomerRegistrationDTO> customers = new List<DataContracts.CustomerRegistrationDTO>();
            using (Entities dbContext = new Entities())
            {

                // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
                var distinctCustomers = dbContext.CustomerRegistrations;

                // convert the entity classes to DTO classes

                foreach (var customer in distinctCustomers)
                    customers.Add(new DataContracts.CustomerRegistrationDTO()
                    {
                        CustomerRegistrationId = customer.CustomerRegistrationId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        PhoneNo = customer.PhoneNo,
                        EmailId = customer.EmailId,
                        CompanyName = customer.CompanyName,
                        ProjectName = customer.ProjectName,
                        TitleAndSettlement = customer.TitleAndSettlement.GetValueOrDefault(false),
                        IneractiveOfficeDirectory = customer.IneractiveOfficeDirectory.GetValueOrDefault(false),
                        Other = customer.Other.GetValueOrDefault(false),
                        Comments = customer.Comments,
                        OtherRequirement = customer.OtherRequirement,
                        CustomerStatus = customer.CustomerStatusId.GetValueOrDefault(5001),
                        CustomerStatusName = dbContext.TypeCodes.Where(x => x.TypeCodeId == customer.CustomerStatusId).Select(x => x.TypeCodeDesc).FirstOrDefault()

                    });

            }
            return customers;
        }


        public List<CustomerRegistrationDTO> GetCustomerRegistrations(string emailid)
        {
            List<DataContracts.CustomerRegistrationDTO> customers = new List<DataContracts.CustomerRegistrationDTO>();
            using (Entities dbContext = new Entities())
            {

                // fetch distinct customers from the PID mapping table. Should create a table to store customer master records
                var distinctCustomers = dbContext.CustomerRegistrations.Where(se => se.EmailId == emailid);

                // convert the entity classes to DTO classes

                foreach (var customer in distinctCustomers)
                    customers.Add(new DataContracts.CustomerRegistrationDTO()
                    {
                        CustomerRegistrationId = customer.CustomerRegistrationId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        PhoneNo = customer.PhoneNo,
                        EmailId = customer.EmailId,
                        CompanyName = customer.CompanyName,
                        ProjectName = customer.ProjectName,
                        TitleAndSettlement = customer.TitleAndSettlement.GetValueOrDefault(false),
                        IneractiveOfficeDirectory = customer.IneractiveOfficeDirectory.GetValueOrDefault(false),
                        Other = customer.Other.GetValueOrDefault(false),
                        Comments = customer.Comments,
                        OtherRequirement = customer.OtherRequirement,
                        CustomerStatus = customer.CustomerStatusId.GetValueOrDefault(5001),
                        CustomerStatusName = dbContext.TypeCodes.Where(x => x.TypeCodeId == customer.CustomerStatusId).Select(x => x.TypeCodeDesc).FirstOrDefault()

                    });

            }
            return customers;
        }

        public IEnumerable<Status> GetStatus()
        {
            using (Entities dbContext = new Entities())
            {
                List<Status> Typecodes = dbContext.TypeCodes.Where(se => se.GroupTypeCode == 5000).Select(sl => new Status
                {
                    ID = sl.TypeCodeId,
                    Name = sl.TypeCodeDesc
                }).ToList();

                return Typecodes;
            }
        }
    }
}
