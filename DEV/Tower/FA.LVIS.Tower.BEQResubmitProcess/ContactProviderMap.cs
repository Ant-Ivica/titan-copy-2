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
    
    public partial class ContactProviderMap
    {
        public int ContactProviderMapId { get; set; }
        public int ProviderId { get; set; }
        public int ContactId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public int CustomerId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public int LastModifiedById { get; set; }
        public int TenantId { get; set; }
    
        public virtual Contact Contact { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual Service Service { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
