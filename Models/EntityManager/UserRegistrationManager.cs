using ProjectTracker.Interfaces;
using ProjectTracker.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic.Core;
using System.Data.SqlClient;
using Dapper;
using System.Configuration;
namespace ProjectTracker.Models.EntityManager
{
    public class UserRegistrationManager : IUserRegistration
    {
        public bool CheckUserNameExists(string emailId)
        {
            try
            {
                using (var _context = new DatabaseContext())
                {
                    var result = (from user in _context.UserMaster
                                  where user.emailId == emailId
                                  select user).Count();
                    var n = result;

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

        public int AddUser(userMaster entity)
        {
            try
            {
                using (ProjectTrackerV2Entities _context = new ProjectTrackerV2Entities())
                {
                    _context.userMasters.Add(entity);
                    return _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<userMaster> ListofRegisteredUser(string sortColumn, string sortColumnDir, string Search)
        {
            try
            {

                ProjectTrackerV2Entities _context = new ProjectTrackerV2Entities();

                    var IQueryableRegistered = (from register in _context.userMasters
                                                select register
                                    );
               

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    IQueryableRegistered = IQueryableRegistered.OrderBy(sortColumn + " " + sortColumnDir);
                }
                if (!string.IsNullOrEmpty(Search))
                {
                    IQueryableRegistered = IQueryableRegistered.Where(m => m.userName.Contains(Search) || m.firstName.Contains(Search));
                }

                return IQueryableRegistered;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePassword(string userId, string password)
        {
            using (ProjectTrackerV2Entities db = new ProjectTrackerV2Entities())
            try
            {
                
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                        try
                        {
                            userMaster user = db.userMasters.Find(userId);
                            user.password = password;
                            user.modifiedBy = userId;
                            user.modifiedTimeStamp = DateTime.Now;
                            db.SaveChanges();
                            dbContextTransaction.Commit();
                            return true;
                        }
                        catch
                        {
                            dbContextTransaction.Rollback();
                            return false;
                        }
           
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}