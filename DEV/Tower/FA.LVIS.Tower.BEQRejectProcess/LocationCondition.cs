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
    
    public partial class LocationCondition
    {
        public int LocationConditionId { get; set; }
        public int ConditionTypeCodeId { get; set; }
        public string ConditionValue { get; set; }
        public Nullable<int> ParentLocationConditionId { get; set; }
        public int FASTPreferenceMapId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public int LastModifiedById { get; set; }
    
        public virtual FASTPreferenceMap FASTPreferenceMap { get; set; }
        public virtual TypeCode TypeCode { get; set; }
    }
}
