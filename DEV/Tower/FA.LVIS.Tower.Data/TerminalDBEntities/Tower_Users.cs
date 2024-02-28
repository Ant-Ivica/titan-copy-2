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
    
    public partial class Tower_Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tower_Users()
        {
            this.Tower_UserClaims = new HashSet<Tower_UserClaims>();
            this.Tower_Roles = new HashSet<Tower_Roles>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Nullable<bool> EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<bool> PhoneNumberConfirmed { get; set; }
        public Nullable<bool> TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public Nullable<bool> LockoutEnabled { get; set; }
        public Nullable<int> AccessFailedCount { get; set; }
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int TenantId { get; set; }
        public int UserId { get; set; }
    
        public virtual Tenant Tenant { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tower_UserClaims> Tower_UserClaims { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tower_Roles> Tower_Roles { get; set; }
    }
}
