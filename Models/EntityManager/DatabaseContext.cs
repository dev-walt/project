using ProjectTracker.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectTracker.Models.EntityManager
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
           : base("name=ProjectTrackerV2Entities")
        {
        }
        public DbSet<userMaster> UserMaster { get; set; }
        public DbSet<roleMaster> RoleMaster { get; set; }
        public DbSet<projectMaster> ProjectMaster { get; set; }
        public DbSet<TimeSheet> TimeSheetMaster { get; set; }
        public DbSet<approverResourceMapping> ApproverResourceMapping { get; set; }
        public DbSet<expDelDate> ExpDelDate { get; set; }

        public DbSet<projectRemark>ProjectRemark { get; set; }

        public DbSet<userRoleMapping> UserRoleMapping { get; set; }

        public DbSet<vendorMaster> VendorMaster { get; set; }


    }

}