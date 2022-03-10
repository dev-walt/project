using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTracker.Models.ViewModel
{
    public class UserTimeSheetView
    {
        public long id { get; set; }
        public string TimeSheetId { get; set; }
        public string taskType { get; set; }
        public string projectId { get; set; }
        public string projectName { get; set; }
        public string description { get; set; }
        public string reportDate { get; set; }
        public Nullable<System.TimeSpan> fromTime { get; set; }
        public Nullable<System.TimeSpan> toTime { get; set; }
        public Nullable<decimal> hours { get; set; }
        public string remarks { get; set; }
        public string projectStatus { get; set; }
        public string status { get; set; }
        public string resourceId { get; set; }
        public string resourceName { get; set; }
        public string submittedOn { get; set; }
        public Nullable<System.DateTime> approvedOn { get; set; }
        public string approvedBy { get; set; }
        public string approvalRemark { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public string CRNumber { get; set; }
        public string supportTicketId { get; set; }
        public string BusinessUserName { get; set; }
        public string UserName { get; set; }
        public string SubmittedMonth { get; set; }

    }

    public class MainUserTimeSheetView
    {
        public List<UserTimeSheetView> ListTimeSheetDetails { get; set; }
        public List<GetPeriods> ListofPeriods { get; set; }
        public List<GetProjectNames> ListofProjectNames { get; set; }
        public List<string> ListOfDayofWeek { get; set; }
        public int TimeSheetId { get; set; }
    }


    public class UserTimeSheetApproval
    {
        public int TimeSheetMasterID { get; set; }
        public string Comment { get; set; }
    }

    public class GetPeriods
    {
        public string Period { get; set; } //reportDate
    }

    public class GetProjectNames
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
    }

    public class TimeSheetApproval
    {
        public int TimeSheetMasterID { get; set; }
        public string Comment { get; set; }
    }
}