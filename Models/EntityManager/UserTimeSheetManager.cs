using ProjectTracker.Interfaces;
using ProjectTracker.Models.DB;
using ProjectTracker.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Linq.Dynamic.Core;


namespace ProjectTracker.Models.EntityManager
{
    public class UserTimeSheetManager: IUserTimeSheet
    {
        public string AddTimeSheetMaster(TimeSheet TimeSheetMaster)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.TimeSheetMaster.Add(TimeSheetMaster);
                    _context.SaveChanges();
                    string id = TimeSheetMaster.TimeSheetId; // Yes it's here
                    return id;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string AddTimeSheetDetail(TimeSheet TimeSheetDetails)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    _context.TimeSheetMaster.Add(TimeSheetDetails);
                    _context.SaveChanges();
                    string id = TimeSheetDetails.TimeSheetId; // Yes it's here
                    return id;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckIsDateAlreadyUsed(DateTime Date, string UserID)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from timesheetdetails in _context.TimeSheetMaster
                                  where timesheetdetails.reportDate == Date && timesheetdetails.resourceId == UserID
                                  select timesheetdetails).Count();

                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<UserTimeSheetView> ShowTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster

                                       where timesheetmaster.resourceId == UserID
                                       select new UserTimeSheetView
                                       {
                                           status = timesheetmaster.status,
                                           remarks = timesheetmaster.remarks,
                                           TimeSheetId = timesheetmaster.TimeSheetId,
                                           reportDate = SqlFunctions.DateName("day", timesheetmaster.reportDate).Trim() + "/" +
                   SqlFunctions.StringConvert((double)timesheetmaster.reportDate.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", timesheetmaster.reportDate),
                                           /*ToDate = SqlFunctions.DateName("day", timesheetmaster.ToDate).Trim() + "/" +
                   SqlFunctions.StringConvert((double)timesheetmaster.ToDate.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", timesheetmaster.ToDate),*/

                                           submittedOn = SqlFunctions.DateName("day", timesheetmaster.submittedOn).Trim() + "/" +
                   SqlFunctions.StringConvert((double)timesheetmaster.submittedOn.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", timesheetmaster.submittedOn),
                                           hours = timesheetmaster.hours
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.reportDate == Search);
            }

            return IQueryabletimesheet;

        }

        public List<UserTimeSheetView> TimesheetDetailsbyTimeSheetId(string UserID, string TimeSheetMasterID)
        {
            using (var _context = new DatabaseContext())
            {
                var data = (from timesheet in _context.TimeSheetMaster
                            join project in _context.ProjectMaster on timesheet.projectId equals project.projectId
                            where timesheet.resourceId == UserID && timesheet.TimeSheetId == TimeSheetMasterID
                            select new UserTimeSheetView
                            {
                                TimeSheetId = timesheet.TimeSheetId,
                                submittedOn = SqlFunctions.DateName("day", timesheet.submittedOn).Trim() + "/" +
                    SqlFunctions.StringConvert((double)timesheet.submittedOn.Value.Month).TrimStart() + "/" +
                    SqlFunctions.DateName("year", timesheet.submittedOn),
                                reportDate = SqlFunctions.DateName("day", timesheet.reportDate).Trim() + "/" +
                    SqlFunctions.StringConvert((double)timesheet.reportDate.Value.Month).TrimStart() + "/" +
                    SqlFunctions.DateName("year", timesheet.reportDate),
 //                               DaysofWeek = timesheet.DaysofWeek,
                                hours = timesheet.hours,
                                projectName = project.projectName,

                            }).ToList();

                return data;
            }
        }

        public List<UserTimeSheetView> TimesheetDetailsbyTimeSheetMasterID(string TimeSheetMasterID)
        {
            using (var _context = new DatabaseContext())
            {
                var data = (from timesheet in _context.TimeSheetMaster
                            join project in _context.ProjectMaster on timesheet.projectId equals project.projectId
                            where timesheet.TimeSheetId == TimeSheetMasterID
                            select new UserTimeSheetView
                            {
                                TimeSheetId = timesheet.TimeSheetId,
                                submittedOn = SqlFunctions.DateName("day", timesheet.submittedOn).Trim() + "/" +
                    SqlFunctions.StringConvert((double)timesheet.submittedOn.Value.Month).TrimStart() + "/" +
                    SqlFunctions.DateName("year", timesheet.submittedOn),
                                reportDate = SqlFunctions.DateName("day", timesheet.reportDate).Trim() + "/" +
                    SqlFunctions.StringConvert((double)timesheet.reportDate.Value.Month).TrimStart() + "/" +
                    SqlFunctions.DateName("year", timesheet.reportDate),
           //                     DaysofWeek = timesheet.DaysofWeek,
                                hours = timesheet.hours,
                                projectName = project.projectName,
                            }).ToList();

                return data;
            }
        }

        public int DeleteTimesheetByTimeSheetId(string TimeSheetMasterID, string UserID)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ProjectTrackerV2Entities"].ToString()))
                {
                    var param = new DynamicParameters();
                    param.Add("@TimeSheetID", TimeSheetMasterID);
                    param.Add("@resourceId", UserID);
                    return con.Execute("Usp_DeleteTimeSheet", param, null, 0, System.Data.CommandType.StoredProcedure);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<UserTimeSheetView> ShowAllTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join registration in _context.UserMaster on timesheetmaster.resourceId equals registration.userId
                                       join AssignedRolesAdmin in _context.ApproverResourceMapping on registration.userId equals AssignedRolesAdmin.resourceId
                                       where AssignedRolesAdmin.approverId == UserID
                                       select new UserTimeSheetView()
                                       {
                                           status = timesheetmaster.status ,
                                           approvalRemark = timesheetmaster.approvalRemark,
                                           TimeSheetId = timesheetmaster.TimeSheetId,
                                           reportDate =
                (
                     System.Data.Entity.DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.reportDate)), 4)

                                            + "-"
                    + System.Data.Entity.DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.reportDate))), 2)
                        + "-"
                        + System.Data.Entity.DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.reportDate))), 2)
                       ).Replace(" ", "0"),



                                           submittedOn = SqlFunctions.DateName("day", timesheetmaster.submittedOn).Trim() + "/" +
                   SqlFunctions.StringConvert((double)timesheetmaster.submittedOn.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", timesheetmaster.submittedOn),
                                           hours = timesheetmaster.hours,
                                           UserName = registration.userName,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.reportDate).ToString()




                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.reportDate == Search || m.UserName == Search);
            }

            return IQueryabletimesheet;

        }

        public List<GetPeriods> GetPeriodsbyTimeSheetMasterID(string TimeSheetMasterID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ProjectTrackerV2Entities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@TimeSheetId", TimeSheetMasterID);
                    var result = con.Query<GetPeriods>("Usp_GetPeriodsbyTimeSheetMasterID", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    if (result.Count > 0)
                    {
                        return result;
                    }
                    else
                    {
                        return new List<GetPeriods>();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<GetProjectNames> GetProjectNamesbyTimeSheetMasterID(string TimeSheetMasterID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ProjectTrackerV2Entities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@TimeSheetId", TimeSheetMasterID);
                    var result = con.Query<GetProjectNames>("Usp_GetProjectNamesbyTimeSheetMasterID", param, null, true, 0, System.Data.CommandType.StoredProcedure).ToList();
                    if (result.Count > 0)
                    {
                        return result;
                    }
                    else
                    {
                        return new List<GetProjectNames>();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public bool UpdateTimeSheetStatus(TimeSheetApproval timesheetapprovalmodel, string Status)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ProjectTrackerV2Entities"].ToString()))
            {
                con.Open();
                SqlTransaction sql = con.BeginTransaction();

                try
                {
                    var param = new DynamicParameters();
                    param.Add("@TimeSheetId", timesheetapprovalmodel.TimeSheetMasterID);
                    param.Add("@approvalRemark", timesheetapprovalmodel.Comment);
                    param.Add("@Status", Status);
                    var result = con.Execute("Usp_UpdateTimeSheetStatus", param, sql, 0, System.Data.CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        sql.Commit();
                        return true;
                    }
                    else
                    {
                        sql.Rollback();
                        return false;
                    }
                }
                catch (Exception)
                {
                    sql.Rollback();
                    throw;
                }
            }
        }


        public int DeleteTimesheetByOnlyTimeSheetMasterID(string TimeSheetMasterID)
        {
            int resultTimeSheetMaster = 0;
            int resultTimeSheetDetails = 0;
            try
            {
                using (var _context = new DatabaseContext())
                {

                    var timesheetcount = (from ex in _context.TimeSheetMaster
                                          where ex.TimeSheetId == TimeSheetMasterID
                                          select ex).Count();

                    if (timesheetcount > 0)
                    {
                        TimeSheet timesheet = (from ex in _context.TimeSheetMaster
                                                     where ex.TimeSheetId == TimeSheetMasterID
                                                     select ex).SingleOrDefault();

                        _context.TimeSheetMaster.Remove(timesheet);
                        resultTimeSheetMaster = _context.SaveChanges();
                    }

                    var timesheetdetailscount = (from ex in _context.TimeSheetMaster
                                                 where ex.TimeSheetId == TimeSheetMasterID
                                                 select ex).Count();

                    if (timesheetdetailscount > 0)
                    {

                        var timesheetdetails = (from ex in _context.TimeSheetMaster
                                                where ex.TimeSheetId == TimeSheetMasterID
                                                select ex).ToList();

                        _context.TimeSheetMaster.RemoveRange(timesheetdetails);
                        resultTimeSheetDetails = _context.SaveChanges();

                    }

                    if (resultTimeSheetMaster > 0 || resultTimeSheetDetails > 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

/*        public int? InsertDescription(DescriptionTB DescriptionTB)
        {
            using (var _context = new DatabaseContext())
            {
                _context.DescriptionTB.Add(DescriptionTB);
                _context.SaveChanges();
                int? id = DescriptionTB.DescriptionID; // Yes it's here
                return id;
            }
        }*/

        public DisplayView GetTimeSheetsCountByAdminID(string AdminID)
        {
            DatabaseContext db = new DatabaseContext();
            var Count = db.UserRoleMapping.Count(a => a.roleId == db.RoleMaster.FirstOrDefault(b => b.roleName == "Admin").roleId);
            DisplayView stat=new DisplayView();
            stat.ApprovalUser = AdminID;
            stat.SubmittedCount= db.TimeSheetMaster.Count(a => a.status == "Submitted");
            stat.ApprovedCount = db.TimeSheetMaster.Count(a => a.status == "Approved");
            stat.RejectedCount= db.TimeSheetMaster.Count(a => a.status == "Rejected");

            /*
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ProjectTrackerV2Entities"].ToString()))
                        {
                            var param = new DynamicParameters();
                            param.Add("@approverId", AdminID);
                            return con.Query<DisplayView>("Usp_GetTimeSheetsCountByAdminID", param, null, true, 0, System.Data.CommandType.StoredProcedure).FirstOrDefault();
                        }*/

            return stat;
        }

        public IQueryable<UserTimeSheetView> ShowAllApprovedTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join registration in _context.UserMaster on timesheetmaster.resourceId equals registration.userId
                                       join AssignedRolesAdmin in _context.ApproverResourceMapping on registration.userId equals AssignedRolesAdmin.resourceId
                                       where AssignedRolesAdmin.approverId == UserID 
                                       select new UserTimeSheetView
                                       {
                                           status = timesheetmaster.status,
                                           approvalRemark = timesheetmaster.approvalRemark,
                                           TimeSheetId = timesheetmaster.TimeSheetId,
                                           reportDate =
                (
                     System.Data.Entity.DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.reportDate)), 4)

                                            + "-"
                    + System.Data.Entity.DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.reportDate))), 2)
                        + "-"
                        + System.Data.Entity.DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.reportDate))), 2)
                       ).Replace(" ", "0"),


                                           submittedOn = SqlFunctions.DateName("day", timesheetmaster.submittedOn).Trim() + "/" +
                   SqlFunctions.StringConvert((double)timesheetmaster.submittedOn.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", timesheetmaster.submittedOn),
                                           hours = timesheetmaster.hours,
                                           UserName = registration.userName,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.reportDate).ToString()




                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.reportDate == Search || m.UserName == Search);
            }

            return IQueryabletimesheet;

        }

        public IQueryable<UserTimeSheetView> ShowAllRejectTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join registration in _context.UserMaster on timesheetmaster.resourceId equals registration.userId
                                       join AssignedRolesAdmin in _context.ApproverResourceMapping on registration.userId equals AssignedRolesAdmin.resourceId
                                       where AssignedRolesAdmin.approverId == UserID
                                       select new UserTimeSheetView
                                       {
                                           status = timesheetmaster.status,
                                           approvalRemark = timesheetmaster.approvalRemark,
                                           TimeSheetId = timesheetmaster.TimeSheetId,
                                           reportDate =
                (
                     System.Data.Entity.DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.reportDate)), 4)

                                            + "-"
                    + System.Data.Entity.DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.reportDate))), 2)
                        + "-"
                        + System.Data.Entity.DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.reportDate))), 2)
                       ).Replace(" ", "0"),


                                          

                                           submittedOn = SqlFunctions.DateName("day", timesheetmaster.submittedOn).Trim() + "/" +
                   SqlFunctions.StringConvert((double)timesheetmaster.submittedOn.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", timesheetmaster.submittedOn),
                                           hours = timesheetmaster.hours,
                                           UserName = registration.userName,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.reportDate).ToString()




                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.reportDate == Search || m.UserName == Search);
            }

            return IQueryabletimesheet;

        }

        public IQueryable<UserTimeSheetView> ShowAllSubmittedTimeSheet(string sortColumn, string sortColumnDir, string Search, string UserID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       join registration in _context.UserMaster on timesheetmaster.resourceId equals registration.userId
                                       join AssignedRolesAdmin in _context.ApproverResourceMapping on registration.userId equals AssignedRolesAdmin.resourceId
                                       where AssignedRolesAdmin.approverId == UserID 
                                       select new UserTimeSheetView
                                       {
                                           status = timesheetmaster.status,
                                           approvalRemark = timesheetmaster.approvalRemark,
                                           TimeSheetId = timesheetmaster.TimeSheetId,
                                           reportDate =
                (
                     System.Data.Entity.DbFunctions.Right(SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("yyyy", timesheetmaster.reportDate)), 4)

                                            + "-"
                    + System.Data.Entity.DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("mm", timesheetmaster.reportDate))), 2)
                        + "-"
                        + System.Data.Entity.DbFunctions.Right(String.Concat(" ", SqlFunctions.StringConvert((double?)SqlFunctions.DatePart("dd", timesheetmaster.reportDate))), 2)
                       ).Replace(" ", "0"),


                                          

                                           submittedOn = SqlFunctions.DateName("day", timesheetmaster.submittedOn).Trim() + "/" +
                   SqlFunctions.StringConvert((double)timesheetmaster.submittedOn.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", timesheetmaster.submittedOn),
                                           hours = timesheetmaster.hours,
                                           UserName = registration.userName,
                                           SubmittedMonth = SqlFunctions.DateName("MONTH", timesheetmaster.reportDate).ToString()




                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.reportDate == Search || m.UserName == Search);
            }

            return IQueryabletimesheet;

        }

        public DisplayView GetTimeSheetsCountByUserID(string UserID)
        {
            DatabaseContext db = new DatabaseContext();
            var Count = db.UserRoleMapping.Count(a => a.roleId == db.RoleMaster.FirstOrDefault(b => b.roleName == "Admin").roleId);
            DisplayView stat = new DisplayView();
            stat.ApprovalUser = UserID;
            stat.SubmittedCount = db.TimeSheetMaster.Count(a => a.status == "Approved");
            stat.ApprovedCount = db.TimeSheetMaster.Count(a => a.status == "Approved");
            stat.RejectedCount = db.TimeSheetMaster.Count(a => a.status == "Rejected");

            /*using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ProjectTrackerV2Entities"].ToString()))
            {
                var param = new DynamicParameters();
                param.Add("@UserID", UserID);
                return con.Query<DisplayView>("Usp_GetTimeSheetsCountByUserID", param, null, true, 0, System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }*/
            return stat;
        }

        public IQueryable<UserTimeSheetView> ShowTimeSheetStatus(string sortColumn, string sortColumnDir, string Search, string UserID, string TimeSheetStatus)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from timesheetmaster in _context.TimeSheetMaster
                                       where timesheetmaster.resourceId == UserID && timesheetmaster.status == TimeSheetStatus
                                       select new UserTimeSheetView
                                       {
                                           status = timesheetmaster.status,
                                           approvalRemark = timesheetmaster.approvalRemark,
                                           TimeSheetId = timesheetmaster.TimeSheetId,
                                           reportDate = SqlFunctions.DateName("day", timesheetmaster.reportDate).Trim() + "/" +
                   SqlFunctions.StringConvert((double)timesheetmaster.reportDate.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", timesheetmaster.reportDate),
                                     

                                           submittedOn = SqlFunctions.DateName("day", timesheetmaster.submittedOn).Trim() + "/" +
                   SqlFunctions.StringConvert((double)timesheetmaster.submittedOn.Value.Month).TrimStart() + "/" +
                   SqlFunctions.DateName("year", timesheetmaster.submittedOn),
                                           hours = timesheetmaster.hours
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.reportDate == Search);
            }

            return IQueryabletimesheet;

        }

       
    }
}