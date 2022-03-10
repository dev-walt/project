using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectTracker.Models.ViewModel
{
    public class UserLoginView
    {
        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "EmailId Required")]
        public string emailId { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password Required")]
        public string password { get; set; }
    }
}