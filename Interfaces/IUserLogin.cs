using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTracker.Models.DB;

namespace ProjectTracker.Interfaces
{
    interface IUserLogin
    {
        userMaster ValidateUser(string emailId, string password);
        bool UpdatePassword(string NewPassword, string userId);
        string GetPasswordbyUserID(string UserID);
    }
}
