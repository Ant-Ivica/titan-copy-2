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
    
    public partial class FASTBusinessProgramType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FASTBusinessProgramType()
        {
            this.FASTPreferenceBusinessPrograms = new HashSet<FASTPreferenceBusinessProgram>();
        }
    
        public int FASTBusinessProgramTypeId { get; set; }
        public string BusinessProgramName { get; set; }
        public int BusinessProgramId { get; set; }
        public int RegionId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FASTPreferenceBusinessProgram> FASTPreferenceBusinessPrograms { get; set; }
    }
}
