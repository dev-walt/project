//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectTracker.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class roleMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public roleMaster()
        {
            this.userRoleMappings = new HashSet<userRoleMapping>();
        }
    
        public int id { get; set; }
        public string roleId { get; set; }
        public string roleName { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedTimeStamp { get; set; }
        public Nullable<bool> isActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<userRoleMapping> userRoleMappings { get; set; }
    }
}
