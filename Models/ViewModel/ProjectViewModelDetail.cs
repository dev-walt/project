using ProjectTracker.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTracker.Models.ViewModel
{
    public class ProjectViewModelDetail
    {
        public ProjectViewModel VM { get; set; }
        public IEnumerable<projectMaster> PVM { get; set; }
        public IEnumerable<UserRegistrationView> UVM { get; set; }
    }
}