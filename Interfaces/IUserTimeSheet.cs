using ProjectTracker.Models.DB;
using ProjectTracker.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Interfaces
{
    interface IUserTimeSheet
    {
        string AddTimeSheetMaster(TimeSheet TimeSheetMaster);
        string AddTimeSheetDetail(TimeSheet TimeSheetDetails);

        bool CheckIsDateAlreadyUsed(DateTime FromDate, string UserID);
        IQueryable<UserTimeSheetView> ShowTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID);
        List<UserTimeSheetView> TimesheetDetailsbyTimeSheetId(string UserID, string TimeSheetMasterID);
        int DeleteTimesheetByTimeSheetId(string TimeSheetId, string UserID);
        IQueryable<UserTimeSheetView> ShowAllTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID);
        List<UserTimeSheetView> TimesheetDetailsbyTimeSheetMasterID(string TimeSheetMasterID);
        List<GetPeriods> GetPeriodsbyTimeSheetMasterID(string TimeSheetMasterID);
        List<GetProjectNames> GetProjectNamesbyTimeSheetMasterID(string TimeSheetMasterID);
        bool UpdateTimeSheetStatus(TimeSheetApproval timesheetapprovalmodel, string Status);
/*        void InsertTimeSheetAuditLog(TimeSheetAuditTB timesheetaudittb);
*/        int DeleteTimesheetByOnlyTimeSheetMasterID(string TimeSheetMasterID);
/*        int? InsertDescription(DescriptionTB DescriptionTB);
*/        DisplayView GetTimeSheetsCountByAdminID(string AdminID);
        IQueryable<UserTimeSheetView> ShowAllApprovedTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID);
        IQueryable<UserTimeSheetView> ShowAllRejectTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID);
        IQueryable<UserTimeSheetView> ShowAllSubmittedTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID);
        DisplayView GetTimeSheetsCountByUserID(string UserID);
        IQueryable<UserTimeSheetView> ShowTimeSheetStatus(string sortColumn, string sortColumnDir, string Search, string UserID, string TimeSheetStatus);
 //       bool UpdateTimeSheetAuditStatus(int TimeSheetID, string Comment, int Status);
 //       bool IsTimesheetALreadyProcessed(string TimeSheetID);
    }
}
