using ProjectTracker.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectTracker.Controllers
{
    public class TimeSheetController : Controller
    {
        // GET: TimeSheet
        public ActionResult view()
        {
            using (var context = new ProjectTrackerV2Entities())
            {



                // Return the list of data from the database
                var data = context.TimeSheets.ToList().AsEnumerable();
                return View(data);
            }
        }
    }
}