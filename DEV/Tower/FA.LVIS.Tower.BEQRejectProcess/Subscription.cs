//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FA.LVIS.Tower.BEQRejectProcess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Subscription
    {
        public int SubscriptionId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public int MessageTypeId { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public int LastModifiedById { get; set; }
        public int TenantId { get; set; }
    
        public virtual Application Application { get; set; }
        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual MessageType MessageType { get; set; }
        public virtual Tenant Tenant { get; set; }
    }
}
