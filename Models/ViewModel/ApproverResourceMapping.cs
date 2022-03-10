using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ProjectTracker.Models.ViewModel
{
    
    [NotMapped]
    public class ApproverResourceMapping
    {
        public List<AdminModel> ListofAdmins { get; set; }

        [Required(ErrorMessage = "Choose Admin")]
        public string userId { get; set; }
        public List<UserModel> ListofUser { get; set; }
        public string AssignToAdmin { get; set; }
        public string createdBy { get; set; }

    }

    [NotMapped]
    public class AdminModel
    {
        public string userId { get; set; }
        public string Name { get; set; }
    }

    [NotMapped]
    public class UserModel
    {
        public string userId { get; set; }
        public string Name { get; set; }
        public bool selectedUsers { get; set; }
        public string AssignToAdmin { get; set; }

    }
}