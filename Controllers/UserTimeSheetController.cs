using ProjectTracker.Interfaces;
using ProjectTracker.Models.DB;
using ProjectTracker.Models.EntityManager;
using ProjectTracker.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectTracker.Controllers
{
    [UserSession]
    public class UserTimeSheetController : Controller
    {
        IProject _IProject;
        IUserTimeSheet _ITimeSheet;
        IUsers _IUsers;
        public UserTimeSheetController()
        {
            _IProject = new ProjectManager();
            _ITimeSheet = new UserTimeSheetManager();
            _IUsers = new UsersManager();
        }

        // GET: TimeSheet
        public ActionResult Add()
        {
            // List<SelectListItem> projectlist = new List<SelectListItem>();
            // var projectlist= _IProject.GetListofProjects();
            ViewBag.projectList = new SelectList(_IProject.GetListofProjects().ToList(), "projectId", "projectName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(TimeSheet timesheetmodel)
        {
            try
            {
                if (timesheetmodel == null)
                {
                    ModelState.AddModelError("", "Values Posted Are Not Accurate");
                    return View();
                }
                ViewBag.projectId=ListofProjects();

                //imeSheet timesheetmodel = new TimeSheet();
                timesheetmodel.TimeSheetId = "TS0021";
                timesheetmodel.resourceId = Convert.ToString(Session["UserID"]);
                timesheetmodel.submittedOn = DateTime.Now;
                timesheetmodel.fromTime = timesheetmodel.fromTime;
                timesheetmodel.toTime = timesheetmodel.toTime;
                var totalhour = timesheetmodel.toTime - timesheetmodel.fromTime;
                //timesheetmodel.hours = Convert.ToDecimal(totalhour.TotalHours);
                timesheetmodel.status = "Submitted";
                string TimeSheetMasterID = _ITimeSheet.AddTimeSheetMaster(timesheetmodel);

                /*var count = ProjectSelectCount(timesheetmodel);*/

                /*if (TimeSheetMasterID > 0)
                {
                    Save(timesheetmodel, TimeSheetMasterID);
                    SaveDescription(timesheetmodel, TimeSheetMasterID);
                    _ITimeSheet.InsertTimeSheetAuditLog(InsertTimeSheetAudit(TimeSheetMasterID, 1));
                }*/

                TempData["TimeCardMessage"] = "Data Saved Successfully";

                return RedirectToAction("Add", "UserTimeSheet");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult view()
        {
            using (var context = new ProjectTrackerV2Entities())
            {



                // Return the list of data from the database
                var data = context.TimeSheets.ToList().AsEnumerable();
                return View(data);
            }
        }

        [HttpGet]
        public JsonResult ListofProjects()
        {
            try
            {
                var listofProjects = _IProject.GetListofProjects();
                return Json(listofProjects, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*[NonAction]
        private int? CalculateTotalHours(TimeSheet TimeSheetModel)
        {
            try
            {
                int? Total = 0;
                var val1 = TimeSheetModel.texttotal_p1 == null ? 0 : TimeSheetModel.texttotal_p1;
                var val2 = TimeSheetModel.texttotal_p2 == null ? 0 : TimeSheetModel.texttotal_p2;
                var val3 = TimeSheetModel.texttotal_p3 == null ? 0 : TimeSheetModel.texttotal_p3;
                var val4 = TimeSheetModel.texttotal_p4 == null ? 0 : TimeSheetModel.texttotal_p4;
                var val5 = TimeSheetModel.texttotal_p5 == null ? 0 : TimeSheetModel.texttotal_p5;
                var val6 = TimeSheetModel.texttotal_p6 == null ? 0 : TimeSheetModel.texttotal_p6;
                Total = val1 + val2 + val3 + val4 + val5 + val6;
                return Total;
            }
            catch (Exception)
            {
                throw;
            }
        }*/

       /* [NonAction]
        private void SaveTimeSheetDetail(string DaysofWeek, int? Hours, DateTime? Period, int? ProjectID, int TimeSheetMasterID)
        {
            try
            {
                TimeSheetDetails objtimesheetdetails = new TimeSheetDetails();
                objtimesheetdetails.TimeSheetID = 0;
                objtimesheetdetails.DaysofWeek = DaysofWeek;
                objtimesheetdetails.Hours = Hours == null ? 0 : Hours;
                objtimesheetdetails.Period = Period;
                objtimesheetdetails.ProjectID = ProjectID;
                objtimesheetdetails.UserID = Convert.ToInt32(Session["UserID"]);
                objtimesheetdetails.CreatedOn = DateTime.Now;
                objtimesheetdetails.TimeSheetMasterID = TimeSheetMasterID;
                int TimeSheetID = _ITimeSheet.AddTimeSheetDetail(objtimesheetdetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
*/
        public JsonResult CheckIsDateAlreadyUsed(DateTime FromDate)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(FromDate)))
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }

                var data = _ITimeSheet.CheckIsDateAlreadyUsed(FromDate, Convert.ToString(Session["UserID"]));
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JsonResult Delete(string TimeSheetMasterID)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(TimeSheetMasterID)))
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }

                var data = _ITimeSheet.DeleteTimesheetByTimeSheetId(TimeSheetMasterID, Convert.ToString(Session["UserID"]));

                if (data > 0)
                {
                    return Json(data: true, behavior: JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(data: false, behavior: JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

     

    }
}