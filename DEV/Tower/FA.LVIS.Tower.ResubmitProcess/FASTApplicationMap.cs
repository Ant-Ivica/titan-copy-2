//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FA.LVIS.Tower.ResubmitProcess
{
    using System;
    using System.Collections.Generic;
    
    public partial class FASTApplicationMap
    {
        public int FASTApplicationMapId { get; set; }
        public int ApplicationId { get; set; }
        public int FASTApplicationId { get; set; }
    
        public virtual Application Application { get; set; }
    }
}
