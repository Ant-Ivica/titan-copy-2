//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FA.LVIS.Tower.BEQResubmitProcess
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceRequestInfo
    {
        public int ServiceRequestInfoId { get; set; }
        public int ServiceRequestId { get; set; }
        public Nullable<int> TypeCodeId { get; set; }
        public string Value { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public int LastModifiedById { get; set; }
        public int TenantId { get; set; }
    
        public virtual ServiceRequest ServiceRequest { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual TypeCode TypeCode { get; set; }
    }
}
