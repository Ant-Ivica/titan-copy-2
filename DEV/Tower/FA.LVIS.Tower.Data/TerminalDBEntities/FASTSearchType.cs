//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FA.LVIS.Tower.Data.TerminalDBEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class FASTSearchType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FASTSearchType()
        {
            this.FASTPreferenceMaps = new HashSet<FASTPreferenceMap>();
        }
    
        public int FASTSearchTypeId { get; set; }
        public string SearchTypeCd { get; set; }
        public string SearchTypeDesc { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public int LastModifiedById { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FASTPreferenceMap> FASTPreferenceMaps { get; set; }
    }
}
