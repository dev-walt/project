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
    
    public partial class projectRemark
    {
        public long id { get; set; }
        public string projectId { get; set; }
        public string projectName { get; set; }
        public string remark { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedTimeStamp { get; set; }
    
        public virtual projectMaster projectMaster { get; set; }
    }
}
