using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTracker.Models.ViewModel
{
    public class UserRegistrationView
    {
        public string userId { get; set; }
        public string firstName { get; set; }
        public string LastName { get; set; }
        public string emailId { get; set; }
        public string userName { get; set; }
    }
}