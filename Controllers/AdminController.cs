//using Microsoft.AspNetCore.Mvc;
using ProjectTracker.Interfaces;
using ProjectTracker.Models.EntityManager;
using ProjectTracker.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectTracker.Controllers
{

        // GET: Admin
        [AdminSession]
        public class AdminController : Controller
        {
            private IUserTimeSheet _ITimeSheet;
            //private IExpense _IExpense;

            public AdminController()
            {
                _ITimeSheet = new UserTimeSheetManager();
                //_IExpense = new ExpenseConcrete();
            }
            // GET: Admin
            [HttpGet]
            public ActionResult Dashboard()
            {
                try
                {
                    var timesheetResult = _ITimeSheet.GetTimeSheetsCountByAdminID(Convert.ToString(Session["AdminUser"]));

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


                    /*var expenseResult = _IExpense.GetExpenseAuditCountByAdminID(Convert.ToString(Session["AdminUser"]));

                    if (expenseResult != null)
                    {
                        ViewBag.SubmittedExpenseCount = expenseResult.SubmittedCount;
                        ViewBag.ApprovedExpenseCount = expenseResult.ApprovedCount;
                        ViewBag.RejectedExpenseCount = expenseResult.RejectedCount;
                    }
                    else
                    {
                        ViewBag.SubmittedExpenseCount = 0;
                        ViewBag.ApprovedExpenseCount = 0;
                        ViewBag.RejectedExpenseCount = 0;
                    }
*/
                    return View();
                }
                catch (Exception)
                {
                    throw;
                }
            }

        public RedirectResult RedirectToAspx()
        {
            var name =Session["userName"];

            /* return Redirect("/ApproveTS.aspx");*/
             return Redirect("/ApproveTS.aspx?Session_ID="+name);
        }

    }
    
}