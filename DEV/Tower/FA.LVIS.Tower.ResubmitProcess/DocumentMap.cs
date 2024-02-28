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
    
    public partial class DocumentMap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DocumentMap()
        {
            this.DocumentLogs = new HashSet<DocumentLog>();
            this.DocumentMessageMaps = new HashSet<DocumentMessageMap>();
        }
    
        public int DocumentMapId { get; set; }
        public string DocumentMapName { get; set; }
        public int DocumentTypeId { get; set; }
        public int ExternalDocTypeId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public bool IsInbound { get; set; }
        public string DocumentMapDesc { get; set; }
        public Nullable<int> StatusTypeCodeId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public int LastModifiedById { get; set; }
        public int TenantId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> CustomerId { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentLog> DocumentLogs { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual DocumentType DocumentType1 { get; set; }
        public virtual Service Service { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual TypeCode TypeCode { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentMessageMap> DocumentMessageMaps { get; set; }
    }
}
