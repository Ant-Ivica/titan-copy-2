using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FA.LVIS.Tower.ExceptionQueueSnapshot;
using System.Globalization;

namespace FA.LVIS.Tower.ExceptionQueueSnapshot
{
    public  class ExceptionSnapShot
    {

       public void CreateTEQHourlySnapshot()
        {
            using (TerminalEntities dbContext = new TerminalEntities())
            {

                DateTime CurrentDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:00 tt"));
                IEnumerable<ExceptionQueueLog> RemoveDuplicate = dbContext.ExceptionQueueLogs
                     .RemoveRange(dbContext.ExceptionQueueLogs.Where(se => se.CreateDate == CurrentDateTime && se.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ));

                dbContext.SaveChanges();


                foreach (var item in dbContext.Tenants)
                {

                    if (item.TenantId == (int)ApplicationEnum.LVIS)
                    {
                        var QueueItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ && y.TypeCodeId != (int)ExceptionStatusEnum.Resolved).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var ArchiveItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ && y.TypeCodeId == (int)ExceptionStatusEnum.Resolved).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var NewItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ && y.TypeCodeId == (int)ExceptionStatusEnum.New).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var ActiveItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ && y.TypeCodeId == (int)ExceptionStatusEnum.Active).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var HoldItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ && y.TypeCodeId == (int)ExceptionStatusEnum.Hold).Select(mr => new { mr.LastModifiedDate }).ToList();

                        var ArchiveQueue = ArchiveItems.GroupBy(x => x.LastModifiedDate.ToString("hh tt", CultureInfo.InvariantCulture))
                            .Select(g => new { Hour = g.Key, Count = g.Count() });


                        var Graphicalcount = Enumerable.Range(00, 1)
                           .Select(excep => new
                           {
                               Hour = (DateTime.Now.AddHours(-excep)).ToString("hh:00 tt"),
                               QueueCount = QueueItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               NewCount = NewItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               ActiveCount = ActiveItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               HoldCount = HoldItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               ArchiveCount = ArchiveQueue.Where(ie => ie.Hour == (DateTime.Now.AddHours(-excep)).ToString("hh tt")).Select(se => se.Count).FirstOrDefault(),
                               Datetime = DateTime.Today.Date.ToShortDateString()
                           }
                            ).ToList();
                    

                        ExceptionQueueLog TeqLog = new ExceptionQueueLog();
                        var currentlog = Graphicalcount[0];
                        TeqLog.ArchiveCount = currentlog.ArchiveCount;
                        TeqLog.QueueCount = currentlog.QueueCount;
                        TeqLog.NewCount = currentlog.NewCount;
                        TeqLog.ActiveCount = currentlog.ActiveCount;
                        TeqLog.HoldCount = currentlog.HoldCount;
                        TeqLog.ExceptionGroupId = (int)ExceptionGroupEnum.TEQ;
                        TeqLog.CreateDate = CurrentDateTime;
                        TeqLog.Tenantid = item.TenantId;
                        dbContext.ExceptionQueueLogs.Add(TeqLog);
                    }
                    else
                    {
                        var QueueItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                                  && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                  && y.TypeCodeId != (int)ExceptionStatusEnum.Resolved).Select(mr => new { mr.LastModifiedDate }).ToList();

                        var ArchiveItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                                 && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                     && y.TypeCodeId == (int)ExceptionStatusEnum.Resolved).Select(mr => new { mr.LastModifiedDate }).ToList();

                        var NewItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                                 && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                     && y.TypeCodeId == (int)ExceptionStatusEnum.New).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var ActiveItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                                 && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                 && y.TypeCodeId == (int)ExceptionStatusEnum.Active).Select(mr => new { mr.LastModifiedDate }).ToList();

                        var HoldItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.TEQ
                                && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                && y.TypeCodeId == (int)ExceptionStatusEnum.Hold).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var ArchiveQueue = ArchiveItems.GroupBy(x => x.LastModifiedDate.ToString("hh tt", CultureInfo.InvariantCulture))
                    .Select(g => new { Hour = g.Key, Count = g.Count() });

                        var Graphicalcount = Enumerable.Range(00, 1)
                           .Select(excep => new
                           {
                               Hour = (DateTime.Now.AddHours(-excep)).ToString("hh:00 tt"),
                               QueueCount = QueueItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               NewCount = NewItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               ActiveCount = ActiveItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               HoldCount = HoldItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               ArchiveCount = ArchiveQueue.Where(ie => ie.Hour == (DateTime.Now.AddHours(-excep)).ToString("hh tt")).Select(se => se.Count).FirstOrDefault(),
                               Datetime = DateTime.Today.Date.ToShortDateString()
                           }
                             ).ToList();

                        ExceptionQueueLog TeqLog = new ExceptionQueueLog();
                        var currentlog = Graphicalcount[0];
                        TeqLog.ArchiveCount = currentlog.ArchiveCount;
                        TeqLog.QueueCount = currentlog.QueueCount;
                        TeqLog.NewCount = currentlog.NewCount;
                        TeqLog.ActiveCount = currentlog.ActiveCount;
                        TeqLog.HoldCount = currentlog.HoldCount;
                        TeqLog.ExceptionGroupId = (int)ExceptionGroupEnum.TEQ;
                        TeqLog.CreateDate = CurrentDateTime;
                        TeqLog.Tenantid = item.TenantId;
                        dbContext.ExceptionQueueLogs.Add(TeqLog);
                    }

                }
                dbContext.SaveChanges();
            }

        }

        public void CreateBEQHourlySnapshot()
        {
            using (TerminalEntities dbContext = new TerminalEntities())
            {
                DateTime CurrentDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:00 tt"));
                IEnumerable<ExceptionQueueLog> RemoveDuplicate = dbContext.ExceptionQueueLogs
                     .RemoveRange(dbContext.ExceptionQueueLogs.Where(se => se.CreateDate == CurrentDateTime && se.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ));

                dbContext.SaveChanges();

                foreach (var item in dbContext.Tenants)
                {

                    if (item.TenantId == (int)ApplicationEnum.LVIS)
                    {
                        var QueueItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ && y.TypeCodeId != (int)ExceptionStatusEnum.Resolved).Select(mr => new { mr.LastModifiedDate }).ToList();

                        var ArchiveItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ && y.TypeCodeId == (int)ExceptionStatusEnum.Resolved).Select(mr => new { mr.LastModifiedDate }).ToList();

                        var NewItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ && y.TypeCodeId == (int)ExceptionStatusEnum.New).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var ActiveItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ && y.TypeCodeId == (int)ExceptionStatusEnum.Active).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var HoldItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ && y.TypeCodeId == (int)ExceptionStatusEnum.Hold).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var ArchiveQueue = ArchiveItems.GroupBy(x => x.LastModifiedDate.ToString("hh tt", CultureInfo.InvariantCulture))
                                        .Select(g => new { Hour = g.Key, Count = g.Count() });


                        var Graphicalcount = Enumerable.Range(00, 1)
                           .Select(excep => new
                           {
                               Hour = (DateTime.Now.AddHours(-excep)).ToString("hh:00 tt"),
                               QueueCount = QueueItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               NewCount = NewItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               ActiveCount = ActiveItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               HoldCount = HoldItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               ArchiveCount = ArchiveQueue.Where(ie => ie.Hour == (DateTime.Now.AddHours(-excep)).ToString("hh tt")).Select(se => se.Count).FirstOrDefault(),
                               Datetime = DateTime.Today.Date.ToShortDateString()
                           }
                            ).ToList();


                        ExceptionQueueLog BeqLog = new ExceptionQueueLog();
                        var currentlog = Graphicalcount[0];
                        BeqLog.ArchiveCount = currentlog.ArchiveCount;
                        BeqLog.QueueCount = currentlog.QueueCount;
                        BeqLog.NewCount = currentlog.NewCount;
                        BeqLog.ActiveCount = currentlog.ActiveCount;
                        BeqLog.HoldCount = currentlog.HoldCount;
                        BeqLog.ExceptionGroupId = (int)ExceptionGroupEnum.BEQ;
                        BeqLog.CreateDate = CurrentDateTime;
                        BeqLog.Tenantid = item.TenantId;

                        dbContext.ExceptionQueueLogs.Add(BeqLog);
                    
                    }
                    else
                    {
                        var QueueItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                                  && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                  && y.TypeCodeId != (int)ExceptionStatusEnum.Resolved).Select(mr => new { mr.LastModifiedDate }).ToList();

                        var ArchiveItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                                 && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                     && y.TypeCodeId == (int)ExceptionStatusEnum.Resolved).Select(mr => new { mr.LastModifiedDate }).ToList();

                        var NewItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                                 && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                     && y.TypeCodeId == (int)ExceptionStatusEnum.New).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var ActiveItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                                 && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                 && y.TypeCodeId == (int)ExceptionStatusEnum.Active).Select(mr => new { mr.LastModifiedDate }).ToList();

                        var HoldItems = dbContext.Exceptions.Where(y => y.ExceptionType.ExceptionGroupId == (int)ExceptionGroupEnum.BEQ
                                && item.TenantId == (y.MessageLogId != 0 ? dbContext.MessageLogs.Where(ml => ml.MessageLogId == y.MessageLogId).FirstOrDefault().TenantId : 0)
                                && y.TypeCodeId == (int)ExceptionStatusEnum.Hold).Select(mr => new { mr.LastModifiedDate }).ToList();
                        var ArchiveQueue = ArchiveItems.GroupBy(x => x.LastModifiedDate.ToString("hh tt", CultureInfo.InvariantCulture))
                           .Select(g => new { Hour = g.Key, Count = g.Count() });


                        var Graphicalcount = Enumerable.Range(00, 1)
                           .Select(excep => new
                           {
                               Hour = (DateTime.Now.AddHours(-excep)).ToString("hh:00 tt"),
                               QueueCount = QueueItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               NewCount = NewItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               ActiveCount = ActiveItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               HoldCount = HoldItems.Where(ie => ie.LastModifiedDate <= (DateTime.Now.AddHours(-excep))).Count(),
                               ArchiveCount = ArchiveQueue.Where(ie => ie.Hour == (DateTime.Now.AddHours(-excep)).ToString("hh tt")).Select(se => se.Count).FirstOrDefault(),
                               Datetime = DateTime.Today.Date.ToShortDateString()
                           }
                             ).ToList();


                        ExceptionQueueLog BeqLog = new ExceptionQueueLog();
                        var currentlog = Graphicalcount[0];
                        BeqLog.ArchiveCount = currentlog.ArchiveCount;
                        BeqLog.QueueCount = currentlog.QueueCount;
                        BeqLog.NewCount = currentlog.NewCount;
                        BeqLog.ActiveCount = currentlog.ActiveCount;
                        BeqLog.HoldCount = currentlog.HoldCount;
                        BeqLog.ExceptionGroupId = (int)ExceptionGroupEnum.BEQ;
                        BeqLog.CreateDate = CurrentDateTime;
                        BeqLog.Tenantid = item.TenantId;

                        dbContext.ExceptionQueueLogs.Add(BeqLog);
                    }

                }
                dbContext.SaveChanges();
            }


        }
    }
}
