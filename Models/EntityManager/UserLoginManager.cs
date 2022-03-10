using ProjectTracker.Interfaces;
using ProjectTracker.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTracker.Models.EntityManager
{
    public class UserLoginManager : IUserLogin
    {
        public userMaster ValidateUser(string emailId, string password)
        {
            try
            {
                using (ProjectTrackerV2Entities _context = new ProjectTrackerV2Entities())
                {
                    var validate = (from user in _context.userMasters
                                    where user.emailId == emailId && user.password == password
                                    select user).SingleOrDefault();

                    return validate;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdatePassword(string NewPassword, string userId)
        {
                using (ProjectTrackerV2Entities db = new ProjectTrackerV2Entities())
                    try
                    {

                        using (var dbContextTransaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                userMaster user = db.userMasters.Find(userId);
                                user.password = NewPassword;
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

            public string GetPasswordbyUserID(string UserID)
        {
            try
            {
                using (ProjectTrackerV2Entities _context = new ProjectTrackerV2Entities())
                {
                    var password = (from temppassword in _context.userMasters
                                    where temppassword.userId == UserID
                                    select temppassword.password).FirstOrDefault();

                    return password;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}