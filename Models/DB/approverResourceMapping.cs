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
    
    public partial class approverResourceMapping
    {
        public long id { get; set; }
        public string resourceId { get; set; }
        public string resourceName { get; set; }
        public string vendorName { get; set; }
        public string approverId { get; set; }
        public string approvarName { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public string modifiedTimeStamp { get; set; }
    
        public virtual userMaster userMaster { get; set; }
        public virtual userMaster userMaster1 { get; set; }
        public virtual approverResourceMapping approverResourceMapping1 { get; set; }
        public virtual approverResourceMapping approverResourceMapping2 { get; set; }
        public virtual userMaster userMaster2 { get; set; }
    }
}
