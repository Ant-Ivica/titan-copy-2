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
    
    public partial class LookupMap
    {
        public int LookupMapId { get; set; }
        public int LookupMapTypeCodeId { get; set; }
        public string LookupMapKey { get; set; }
        public string LookupMapValue { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public int LastModifiedById { get; set; }
    
        public virtual Application Application { get; set; }
        public virtual TypeCode TypeCode { get; set; }
    }
}
