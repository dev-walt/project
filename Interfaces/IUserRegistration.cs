using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectTracker.Models.DB;

namespace ProjectTracker.Interfaces
{
    interface IUserRegistration
    {

        bool CheckUserNameExists(string Username);
        int AddUser(userMaster entity);
        IQueryable<userMaster> ListofRegisteredUser(string sortColumn, string sortColumnDir, string Search);
        bool UpdatePassword(string RegistrationID, string Password);


    }
}
