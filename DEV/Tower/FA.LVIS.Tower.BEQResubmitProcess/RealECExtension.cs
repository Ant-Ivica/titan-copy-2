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
    
    public partial class RealECExtension
    {
        public int RealECExtensionId { get; set; }
        public int ServiceRequestId { get; set; }
        public string ElementName { get; set; }
        public string ElementValue { get; set; }
        public string ElementXPath { get; set; }
        public string ElementNamespace { get; set; }
        public string ElementOrder { get; set; }
    
        public virtual ServiceRequest ServiceRequest { get; set; }
    }
}
