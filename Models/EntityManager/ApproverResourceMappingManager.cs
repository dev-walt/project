using Dapper;
using ProjectTracker.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Linq.Dynamic.Core;
using ProjectTracker.Models.DB;
using ProjectTracker.Interfaces;

namespace ProjectTracker.Models.EntityManager
{
    public class ApproverResourceMappingManager : IApproverResourceMapping
    {
        public List<AdminModel> ListofAdmins()
        {

            DatabaseContext db = new DatabaseContext();
            try
            {
           
                var result = (from a in db.UserMaster
                           join b in db.UserRoleMapping on a.userId equals b.userId
                           join c in db.RoleMaster on b.roleId equals c.roleId
                           where c.roleName == "Admin"
                           select new AdminModel
                           {
                               Name=a.firstName,
                               userId=a.userId,
                           }
                          ).ToList();
                result.Insert(0, new AdminModel { Name = "----Select----", userId = "" });
               
                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<UserModel> ListofUser()
        {

            List<UserModel> lst = new List<UserModel>();

            DatabaseContext db = new DatabaseContext();
            try
            {
                /*var result = (from registration in db.UserMaster
                              where registration.userRoleMappings.FirstOrDefault(a => a.userId == registration.userId).roleMaster.roleName == "AdminUser"
                              select new AdminModel
                              {
                                  Name = registration.firstName,
                                  userId = registration.userId,

                              }).AsList();*/
                // var result = db.UserRoleMapping.All(a => a.roleId == db.RoleMaster.FirstOrDefault(b => b.roleName == "Admin").roleId);
                var result = db.UserMaster.Where(a => a.userId == db.UserRoleMapping.FirstOrDefault(b => b.roleId == db.RoleMaster.FirstOrDefault(c => c.roleName == "User").roleId).userId).Select(a => new UserModel
                {
                    Name = a.firstName,
                    userId = a.userId,

                }).AsList();


                //IEnumerable<AdminModel> admnlst = result.AsEnumerable();
                IEnumerable<UserModel> usrlst = result.Concat(new[] { new UserModel() { Name = "----Select----", userId = "" } });

                return usrlst.ToList();
            }
            catch (Exception)
            {
                throw;
            }



        }

        public int UpdateAssigntoAdmin(string AssignToAdminID, string UserID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ProjectTracker"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@AssignTo", AssignToAdminID);
                    param.Add("@RegistrationID", UserID);
                    var result = con.Execute("Usp_UpdateAssignToUser", param, null, 0, System.Data.CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IQueryable<UserModel> ShowallRoles(string sortColumn, string sortColumnDir, string Search)
        {
            var _context = new DatabaseContext();

            var IQueryabletimesheet = (from AssignedRoles in _context.ApproverResourceMapping
                                       join registration in _context.UserMaster on AssignedRoles.resourceId equals registration.userId
                                       join AssignedRolesAdmin in _context.UserMaster on AssignedRoles.approverId equals AssignedRolesAdmin.userId
                                       select new UserModel
                                       {
                                           Name = registration.firstName,
                                           AssignToAdmin = string.IsNullOrEmpty(AssignedRolesAdmin.firstName) ? "*Not Assigned*" : AssignedRolesAdmin.firstName.ToUpper(),
                                           userId = registration.userId

                                       });

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                IQueryabletimesheet = IQueryabletimesheet.OrderBy(sortColumn + " " + sortColumnDir);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                IQueryabletimesheet = IQueryabletimesheet.Where(m => m.Name == Search);
            }

            return IQueryabletimesheet;

        }

        public bool RemovefromUserRole(string RegistrationID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TimesheetDBEntities"].ToString()))
            {
                con.Open();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@RegistrationID", RegistrationID);
                    var result = con.Execute("Usp_UpdateUserRole", param, null, 0, System.Data.CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<UserModel> GetListofUnAssignedUsers()
        {
            DatabaseContext db = new DatabaseContext();
            try
            {
               
                var result = (from u in db.UserMaster
                              join a in db.UserRoleMapping on u.userId equals a.userId
                              join b in db.RoleMaster on a.roleId equals b.roleId
                              //join c in db.ApproverResourceMapping on u.userId equals c.resourceId into unasgn
                              //from userslst in unasgn.DefaultIfEmpty()
                              where b.roleName=="User"  && !db.ApproverResourceMapping.Any(es=>es.resourceId==u.userId)                     
                             select
                              new UserModel
                              {
                                
                                  Name = u.firstName,
                                  userId =u.userId,
                                  //AssignToAdmin=userslst.approverId
                                 

                              }).ToList();

                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool SaveAssignedRoles(ApproverResourceMapping AssignRolesModel)
        {
            bool result = false;
           
            using (var _context = new DatabaseContext())
            {
                var name = _context.UserMaster.SingleOrDefault(a => a.userId == AssignRolesModel.userId).firstName;
                try
                {
                    for (int i=0; i < AssignRolesModel.ListofUser.Count(); i++)
                    {
                        if (AssignRolesModel.ListofUser[i].selectedUsers)
                        {
                            var id = AssignRolesModel.ListofUser[i].userId;
                            var vendor = _context.UserMaster.SingleOrDefault(a => a.userId == id).vendorName;
                            approverResourceMapping AssignedRoles = new approverResourceMapping
                            {
                                approverId = AssignRolesModel.userId,
                                createdTimeStamp = DateTime.Now,
                                createdBy = AssignRolesModel.createdBy,
                                isActive = true,
                                resourceId = AssignRolesModel.ListofUser[i].userId,
                                resourceName= AssignRolesModel.ListofUser[i].Name,
                                approvarName=name,
                                vendorName=vendor

                            };

                            _context.ApproverResourceMapping.Add(AssignedRoles);
                            _context.SaveChanges();
                        }
                    }

                    result = true;
                }
                catch (Exception)
                {
                    throw;
                }

                return result;
            }
        }

        public bool CheckIsUserAssignedRole(string userId)
        {
            var IsassignCount = 0;
            using (var _context = new DatabaseContext())
            {
                IsassignCount = (from assignUser in _context.ApproverResourceMapping
                                 where assignUser.resourceId == userId
                                 select assignUser).Count();
            }

            if (IsassignCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}