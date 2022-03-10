using ProjectTracker.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Interfaces
{
    interface IApproverResourceMapping
    {
        List<AdminModel> ListofAdmins();
        List<UserModel> ListofUser();
        int UpdateAssigntoAdmin(string AssignToAdminID, string UserID);
        IQueryable<UserModel> ShowallRoles(string sortColumn, string sortColumnDir, string Search);
        bool RemovefromUserRole(string RegistrationID);
        List<UserModel> GetListofUnAssignedUsers();
        bool SaveAssignedRoles(ApproverResourceMapping AssignRolesModel);
        bool CheckIsUserAssignedRole(string userId);
    }
}
