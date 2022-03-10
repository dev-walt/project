using ProjectTracker.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.Interfaces
{
    interface IUsers
    {
        IQueryable<RegistrationViewSummaryModel> ShowallUsers(string sortColumn, string sortColumnDir, string Search);

        RegistrationViewSummaryModel GetUserDetailsByRegistrationID(string UserId);
        IQueryable<RegistrationViewSummaryModel> ShowallAdmin(string sortColumn, string sortColumnDir, string Search);

        RegistrationViewSummaryModel GetAdminDetailsByRegistrationID(string UserId);

        IQueryable<RegistrationViewSummaryModel> ShowallUsersUnderAdmin(string sortColumn, string sortColumnDir, string Search, string RegistrationID);

        int GetTotalAdminsCount();
        int GetTotalUsersCount();
        int GetUserIDbyTimesheetID(string TimeSheetId);
        int GetAdminIDbyUserID(string UserId);
    }
}
