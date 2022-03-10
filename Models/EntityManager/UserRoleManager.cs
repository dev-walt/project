using ProjectTracker.Interfaces;
using ProjectTracker.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTracker.Models.EntityManager
{
    public class UserRoleManager : IUserRole
    {
        public string getRolesofUserbyRolename(string Rolename)
        {
            using (ProjectTrackerV2Entities _context = new ProjectTrackerV2Entities())
            {
                var roleID = (from role in _context.roleMasters
                              where role.roleName == Rolename
                              select role.roleId).SingleOrDefault();

                return roleID;
            }
        }
    }
}