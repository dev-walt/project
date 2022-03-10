using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectTracker.Models.ViewModel
{
        [NotMapped]
        public class RegistrationViewSummaryModel
        {
            public string UserId { get; set; }
            public string FirstName { get; set; }
            public string emailId { get; set; }
            public string UserName { get; set; }
            public string AssignToAdmin { get; set; }

        }
}