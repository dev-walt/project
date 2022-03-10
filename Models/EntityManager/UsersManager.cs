using ProjectTracker.Interfaces;
using ProjectTracker.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic.Core;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace ProjectTracker.Models.EntityManager
{
    public class UsersManager : IUsers
    {
        public IQueryable<RegistrationViewSummaryModel> ShowallUsers(string sortColumn, string sortColumnDir, string Search)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from registration in _context.UserMaster
                                       join AssignedRoles in _context.ApproverResourceMapping on registration.userId equals AssignedRoles.resourceId
                                       join AssignedRolesAdmin in _context.UserMaster on AssignedRoles.approverId equals AssignedRolesAdmin.userId
                                       where registration.userRoleMappings.SingleOrDefault(a => a.userId == registration.userId).roleMaster.roleName=="User"
                                       select new RegistrationViewSummaryModel
                                       {
                                           FirstName = registration.firstName,
                                           AssignToAdmin = string.IsNullOrEmpty(AssignedRolesAdmin.firstName) ? "*Not Assigned*" : AssignedRolesAdmin.firstName.ToUpper(),
                                           UserId = registration.userId,
                                           emailId = registration.emailId,
                                           UserName = registration.userName
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FirstName == Search);
            }

            return IQueryabletimesheet;

        }

        public RegistrationViewSummaryModel GetUserDetailsByRegistrationID(string RegistrationID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@userId", RegistrationID);
                    return con.Query<RegistrationViewSummaryModel>("Usp_GetUserDetailsByRegistrationID", param, null, true, 0, System.Data.CommandType.StoredProcedure).SingleOrDefault();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IQueryable<RegistrationViewSummaryModel> ShowallAdmin(string sortColumn, string sortColumnDir, string Search)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from registration in _context.UserMaster
                                       where registration.userRoleMappings.SingleOrDefault(a => a.userId == registration.userId).roleMaster.roleName == "AdminUser"
                                       select new RegistrationViewSummaryModel
                                       {
                                           FirstName = registration.firstName,
                                           UserId = registration.userId,
                                           emailId = registration.emailId,
                                         
                                           UserName = registration.userName
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FirstName == Search);
            }

            return IQueryabletimesheet;

        }

        public RegistrationViewSummaryModel GetAdminDetailsByRegistrationID(string RegistrationID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@RegistrationID", RegistrationID);
                    return con.Query<RegistrationViewSummaryModel>("Usp_GetAdminDetailsByRegistrationID", param, null, true, 0, System.Data.CommandType.StoredProcedure).SingleOrDefault();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IQueryable<RegistrationViewSummaryModel> ShowallUsersUnderAdmin(string sortColumn, string sortColumnDir, string Search, string RegistrationID)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from registration in _context.UserMaster
                                       join AssignedRoles in _context.ApproverResourceMapping on registration.userId equals AssignedRoles.resourceId
                                       where registration.userRoleMappings.SingleOrDefault(a => a.userId == registration.userId).roleMaster.roleName == "User" && AssignedRoles.approverId == RegistrationID
                                       select new RegistrationViewSummaryModel
                                       {
                                           FirstName = registration.firstName,
                                           UserId = registration.userId,
                                           emailId = registration.emailId,
                                           UserName = registration.userName
                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.FirstName == Search);
            }

            return IQueryabletimesheet;

        }

        public int GetTotalAdminsCount()
        {
            /* using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
             {
                 var Count = con.Query<int>("Usp_GetAdminCount", null, null, true, 0, System.Data.CommandType.StoredProcedure).FirstOrDefault();
                 if (Count > 0)
                 {
                     return Count;
                 }
                 else
                 {
                     return 0;
                 }
             }*/
            DatabaseContext db = new DatabaseContext();
            var Count = db.UserRoleMapping.Count(a => a.roleId == db.RoleMaster.FirstOrDefault(b => b.roleName == "Admin").roleId);
            if (Count > 0)
            {
                return Count;
            }
            else
            {
                return 0;
            }

        }

        public int GetTotalUsersCount()
        {
            DatabaseContext db = new DatabaseContext();
            var Count = db.UserRoleMapping.Count(a => a.roleId == db.RoleMaster.FirstOrDefault(b => b.roleName == "User").roleId);
            if (Count > 0)
                {
                    return Count;
                }
                else
                {
                    return 0;
                }
        }

        public int GetUserIDbyTimesheetID(string TimeSheetMasterID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                var para = new DynamicParameters();
                para.Add("@TimeSheetId", TimeSheetMasterID);
                var Count = con.Query<int>("GetUserIDbyTimeSheetID", para, null, true, 0, System.Data.CommandType.StoredProcedure).FirstOrDefault();
                if (Count > 0)
                {
                    return Count;
                }
                else
                {
                    return 0;
                }
            }
        }


        public int GetAdminIDbyUserID(string UserID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                var para = new DynamicParameters();
                para.Add("@userId", UserID);
                var Count = con.Query<int>("Usp_GetAdminIDbyUserID", para, null, true, 0, System.Data.CommandType.StoredProcedure).FirstOrDefault();
                if (Count > 0)
                {
                    return Count;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}