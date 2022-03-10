using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTracker.Models.ViewModel
{
    public class ProjectViewModel
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string CRNumber { get; set; }
        public string FPR { get; set; }
        //public int[] FPRIDS { get; set; }
        public string SPR { get; set; }
        public System.DateTime ExpectedDeliveryDate { get; set; }
        public string Remark { get; set; }
        public string ProjectStatus { get; set; }
        public bool Active { get; set; }

    }
}