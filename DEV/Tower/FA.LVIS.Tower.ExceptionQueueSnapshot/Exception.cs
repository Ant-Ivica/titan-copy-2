//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FA.LVIS.Tower.ExceptionQueueSnapshot
{
    using System;
    using System.Collections.Generic;
    
    public partial class Exception
    {
        public int ExceptionId { get; set; }
        public int ExceptionTypeId { get; set; }
        public Nullable<int> MessageLogId { get; set; }
        public string ExceptionDesc { get; set; }
        public long DocumentObjectId { get; set; }
        public string Comments { get; set; }
        public int TypeCodeId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public int LastModifiedById { get; set; }
    
        public virtual ExceptionType ExceptionType { get; set; }
    }
}
