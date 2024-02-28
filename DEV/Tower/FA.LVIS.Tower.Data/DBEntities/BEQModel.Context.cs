﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FA.LVIS.Tower.Data.DBEntities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BEQEntities : DbContext
    {
        public BEQEntities()
            : base("name=BEQEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ExceptionQueue> ExceptionQueues { get; set; }
        public virtual DbSet<ExceptionQueueHistory> ExceptionQueueHistories { get; set; }
    
        public virtual ObjectResult<BEQReporting_ExceptionQueue_Result> BEQReporting_ExceptionQueue(Nullable<System.DateTime> fromCreatedDate)
        {
            var fromCreatedDateParameter = fromCreatedDate.HasValue ?
                new ObjectParameter("FromCreatedDate", fromCreatedDate) :
                new ObjectParameter("FromCreatedDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BEQReporting_ExceptionQueue_Result>("BEQReporting_ExceptionQueue", fromCreatedDateParameter);
        }
    
        public virtual ObjectResult<BEQReporting_ExceptionQueueHistory_Result> BEQReporting_ExceptionQueueHistory(Nullable<System.DateTime> fromCreatedDate)
        {
            var fromCreatedDateParameter = fromCreatedDate.HasValue ?
                new ObjectParameter("FromCreatedDate", fromCreatedDate) :
                new ObjectParameter("FromCreatedDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BEQReporting_ExceptionQueueHistory_Result>("BEQReporting_ExceptionQueueHistory", fromCreatedDateParameter);
        }
    
        public virtual ObjectResult<BEQ_GetExceptionQueueCount_Result> BEQ_GetExceptionQueueCount(Nullable<System.DateTime> fromCreatedDate)
        {
            var fromCreatedDateParameter = fromCreatedDate.HasValue ?
                new ObjectParameter("FromCreatedDate", fromCreatedDate) :
                new ObjectParameter("FromCreatedDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BEQ_GetExceptionQueueCount_Result>("BEQ_GetExceptionQueueCount", fromCreatedDateParameter);
        }
    }
}
