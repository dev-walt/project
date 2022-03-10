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
    
    public partial class projectMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public projectMaster()
        {
            this.expDelDates = new HashSet<expDelDate>();
            this.projectRemarks = new HashSet<projectRemark>();
            this.TimeSheets = new HashSet<TimeSheet>();
        }
    
        public long id { get; set; }
        public string projectId { get; set; }
        public string CRNumber { get; set; }
        public string projectName { get; set; }
        public string projectDescription { get; set; }
        public string FPR { get; set; }
        public string team { get; set; }
        public Nullable<System.DateTime> expectedDelDate { get; set; }
        public string remarks { get; set; }
        public string status { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdTimeStamp { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedTimeStamp { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<expDelDate> expDelDates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<projectRemark> projectRemarks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeSheet> TimeSheets { get; set; }
    }
}
