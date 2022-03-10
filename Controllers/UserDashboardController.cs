using ProjectTracker.Interfaces;
using ProjectTracker.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectTracker.Controllers
{
    public class UserDashboardController : Controller
    {
        // GET: UserDashboard
        /*        public ActionResult Index()
                {
                    return View();
                }*/


        private IUserTimeSheet _ITimeSheet;
        public UserDashboardController()
        {
            _ITimeSheet = new UserTimeSheetManager();
        }
        // GET: UserDashboard
        public ActionResult Dashboard()
        {
            var timesheetResult = _ITimeSheet.GetTimeSheetsCountByUserID(Convert.ToString(Session["UserID"]));

            if (timesheetResult != null)
            {
                ViewBag.SubmittedTimesheetCount = timesheetResult.SubmittedCount;
                ViewBag.ApprovedTimesheetCount = timesheetResult.ApprovedCount;
                ViewBag.RejectedTimesheetCount = timesheetResult.RejectedCount;
            }
            else
            {
                ViewBag.SubmittedTimesheetCount = 0;
                ViewBag.ApprovedTimesheetCount = 0;
                ViewBag.RejectedTimesheetCount = 0;
            }

            return View();
        }
    }
}
