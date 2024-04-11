//using Microsoft.SharePoint.Client;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Xml.Linq;
//using SP = Microsoft.SharePoint.Client;
////using SPC = Microsoft.SharePoint.Client;
//using FA.LVIS.Tower.Core;


//namespace FA.LVIS.Tower.Core
//{
//    public class SharePointHelper
//    {
//        public static string GetSharePointUrl()
//        {
//            string environment = "QA";
//            if (string.Equals(environment, "qa", StringComparison.OrdinalIgnoreCase))
//            {
//                return string.Format(ConfigurationManager.AppSettings["SharePointNonProdUrl"], environment);
//            }
//            else
//            {
//                return "";
//            }

//        }

//        public static ClientContext CreateClientContext()
//        {
//            string siteUrl = GetSharePointUrl();

//            return new ClientContext(siteUrl);
//        }

//        public static DateTime GetListItemsByQueue(string queueGroup)
//        {
//            var clientContext = new ClientContext(GetSharePointUrl());

//            List listGroup = clientContext.Web.Lists.GetByTitle(queueGroup);
//            CamlQuery camlQuery = new CamlQuery();
//            camlQuery.ViewXml = @"<View>
//                            <Query>
//                              <OrderBy><FieldRef Name='Created' Ascending='FALSE'/></OrderBy>
//                            </Query><RowLimit>1</RowLimit></View>";

//            ListItemCollection listItems = listGroup.GetItems(camlQuery);
//            clientContext.Load(listItems);
//            clientContext.ExecuteQuery();

//            DateTime date = DateTime.Now;

//            foreach (ListItem listItem in listItems)
//            {
//                date = Convert.ToDateTime(listItem.FieldValues["Created"]).ToLocalTime();
//            }
//            return date;

//        }

//        public static List<DateTime> GetDateTimeByQueue(string queueGroup, DateTime date)
//        {
//            double totalHoursToUpdate = 0;
//            TimeSpan t = new TimeSpan();
//            t = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd h:mm tt")) - Convert.ToDateTime(date.ToString("yyyy-MM-dd h:mm tt"));
//            totalHoursToUpdate = (t.TotalHours >= 1 == true ? t.TotalHours : 12);

//            DateTime date1 = DateTime.Now.AddHours(-totalHoursToUpdate);
//            var clientContext = new ClientContext(GetSharePointUrl());

//            List listGroup = clientContext.Web.Lists.GetByTitle(queueGroup);
//            CamlQuery camlQuery = new CamlQuery();
//            camlQuery.ViewXml = @"<View>" +
//                    "<Query>" +
//                        "<Where>" +
//                        "<Gt>" +
//                            "<FieldRef Name='Created'/>" +
//                            "<Value Type='DateTime'>" + date1.ToString("yyyy-MM-ddTHH:mm:ssZ") + "</Value>" +
//                        "</Gt>" +
//                       "</Where>" +
//                    "</Query>" +
//                    "</View>";

//            ListItemCollection listItems = listGroup.GetItems(camlQuery);
//            clientContext.Load(listItems);
//            clientContext.ExecuteQuery();
//            List<DateTime> dates = new List<DateTime>();

//            foreach (ListItem listItem in listItems)
//            {
//                dates.Add(Convert.ToDateTime(listItem.FieldValues["Created"]).ToLocalTime());
//            }

//            return dates;
//        }

//        public static IList<SP.List> GetExceptionQueues()
//        {
//            ClientContext clientContext = new ClientContext(GetSharePointUrl());
//            Web oWebsite = clientContext.Web;
//            ListCollection collList = oWebsite.Lists;

//            clientContext.Load(oWebsite.Navigation.QuickLaunch, ql => ql.Include(n => n.Title));
//            clientContext.Load(collList);

//            clientContext.ExecuteQuery();

//            string queueGroup = ConfigurationManager.AppSettings["TechnicalExceptionsHeader"];

//            NavigationNode processResolutionNavNode = null;
//            foreach (var navNode in oWebsite.Navigation.QuickLaunch)
//            {
//                if (string.Equals(navNode.Title, queueGroup))
//                {
//                    clientContext.Load(navNode.Children);
//                    clientContext.ExecuteQuery();
//                    processResolutionNavNode = navNode;
//                    break;
//                }
//            }

//            IList<SP.List> filteredLists = new List<SP.List>();
//            int count = 0;
//            if (processResolutionNavNode != null)
//            {
//                foreach (var list in oWebsite.Lists)
//                {
//                    foreach (var child in processResolutionNavNode.Children)
//                    {
//                        if (string.Equals(child.Title, list.Title))
//                        {

//                            filteredLists.Add(list);
//                            count = list.ItemCount;
//                            break;
//                        }
//                    }
//                }
//            }


//            return filteredLists;
//        }

//        public static IList<SP.List> GetArchiveExceptionQueues()
//        {
//            var clientContext = CreateClientContext();

//            Web oWebsite = clientContext.Web;
//            ListCollection collList = oWebsite.Lists;
//            clientContext.Load(oWebsite.Navigation.QuickLaunch, ql => ql.Include(n => n.Title));
//            clientContext.Load(collList);

//            clientContext.ExecuteQuery();

//            string queueGroup = ConfigurationManager.AppSettings["TechnicalExceptionsArchiveHeader"];

//            NavigationNode ArchiveNavNode = null;
//            foreach (var navNode in oWebsite.Navigation.QuickLaunch)
//            {
//                if (string.Equals(navNode.Title, queueGroup))
//                {
//                    clientContext.Load(navNode.Children);
//                    clientContext.ExecuteQuery();
//                    ArchiveNavNode = navNode;
//                    break;
//                }
//            }

//            int count = 0;
//            IList<SP.List> filteredArchiveLists = new List<SP.List>();
//            if (ArchiveNavNode != null)
//            {
//                foreach (var list in oWebsite.Lists)
//                {
//                    foreach (var child in ArchiveNavNode.Children)
//                    {
//                        if (string.Equals(child.Title, list.Title))
//                        {
//                            // splistItemsDt(child.Title);
//                            filteredArchiveLists.Add(list);
//                            count = list.ItemCount;
//                            break;
//                        }

//                    }
//                }

//            }

//            return filteredArchiveLists;
//        }
//    }
//}

