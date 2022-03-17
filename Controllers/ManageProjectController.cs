using ProjectTracker.Models.DB;
using ProjectTracker.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectTracker.Sessions;

namespace ProjectTracker.Controllers
{
    public class ManageProjectController : Controller
    {
        // GET: ManageProject
        public ActionResult Index()
        {
            return View();
        }
        [AdminSession]
        
        [HttpGet]
        public ActionResult AddProject()
        {
            //ViewModel vm = new ViewModel();
            //vm.FPR = PopulateFPRS();
            using (var context = new Models.DB.ProjectTrackerV2Entities())
            {
                ProjectViewModelDetail vm = new ProjectViewModelDetail();



                vm.PVM = context.projectMasters.ToList().AsEnumerable();
                var result = (from UsersList in context.userMasters select UsersList).ToList();
                if (result != null)
                {
                    ViewBag.myUsers = result.Select(x => new SelectListItem { Text = x.userName });
                }
                var re = (from Options in context.projectStatus select Options).ToList();
                if (re != null)
                {
                    ViewBag.Options = re.Select(x => new SelectListItem { Text = x.status });
                }



                //vm.UVM = result
                // .Select(a => new UserViewModel()
                // {
                // UserId = a.UserId,
                // UserName = a.UserName
                // })
                // .ToList();




                return View(vm);
            }
        }
        [AdminSession]
        [HttpPost]
        public ActionResult AddProject(ProjectViewModelDetail model)
        {

            using (var context = new Models.DB.ProjectTrackerV2Entities())
            {
                ProjectViewModelDetail vm = new ProjectViewModelDetail();
                projectRemark p1 = new projectRemark();
                var autoId = context.projectMasters.OrderByDescending(u => u.id).FirstOrDefault();
                projectMaster p = new projectMaster
                {

                    projectId = "P00" + (autoId.id + 1).ToString()
                };
                if (ModelState.IsValid == true)
                {
                    p.CRNumber = model.VM.CRNumber;
                    //p.FPR = PopulateFPRS().ToString() ;
                    //if (model.FPRIDS != null)
                    //{
                    // List<SelectListItem> selectedItems = model.FPR.Where(m1=> model.FPRIDS.Contains(int.Parse(m1.Value))).ToList();
                    //}
                    p.FPR = model.VM.FPR;
                    p.team = model.VM.SPR.ToString();
                    p.projectDescription = model.VM.ProjectDescription;
                    p.expectedDelDate = model.VM.ExpectedDeliveryDate;
                    p.remarks = model.VM.Remark;
                    p.projectName = model.VM.ProjectName;
                    p.status = model.VM.ProjectStatus;
                    p.isActive = model.VM.Active;
                    //p.ModifiedDateTime = DateTime.Now;
                    p.createdTimeStamp = DateTime.Now;
                    //p.ModifiedBy = "Mani";
                    p.createdBy = (string)Session["UserName"];





                    p1.projectId = p.projectId;
                    p1.projectName = p.projectName;
                    p1.remark = p.remarks;
                    p1.createdBy = (string)Session["UserName"];
                    p1.createdTimeStamp = DateTime.Now;



                    // Add data to the particular table
                    context.projectMasters.Add(p);
                    context.projectRemarks.Add(p1);



                    // save the changes



                    context.SaveChanges();
                    ViewBag.AddMsg = "<script>alert('Project Added succesfully') </script>";
                }
                else
                {
                    ViewBag.AddMsg = "<script>alert('Something went wrong') </script>";
                }




            }



            return RedirectToAction("AddProject");
        }






        [HttpGet]
        public ActionResult ViewProject()
        {
            using (var context = new ProjectTrackerV2Entities())
            {



                // Return the list of data from the database
                var data = context.projectMasters.ToList().AsEnumerable();
                return View(data);
            }
        }



        [HttpGet]
        public ActionResult SearchProject(string Projectname)
        {
            using (var context = new ProjectTrackerV2Entities())
            {
                var Projects = from s in context.projectMasters
                               select s;
                if (!string.IsNullOrEmpty(Projectname))
                {
                    Projects = Projects.Where(s => s.projectName == Projectname);
                }
                else
                {
                    ViewBag.Message = "Not found";
                }



                return View(Projects.ToList());
            }




        }

        public ActionResult Remarks(string Id, string Remark)
        {

            using (var context = new ProjectTrackerV2Entities())
            {
                projectRemark pr = new projectRemark();
                var pn = context.projectMasters.Where(u => u.projectId == Id).FirstOrDefault();
                pr.projectName = pn.projectName;
                pr.projectId = Id;
                pr.remark = Remark;
                pr.createdTimeStamp = DateTime.Now;
                pr.createdBy = (string)Session["UserName"]; ;
                context.projectRemarks.Add(pr);
                context.SaveChanges();

                return RedirectToAction("Remarks");
            }
        }



        [HttpGet]
        public ActionResult Remarks(string Id)
        {
            using (var context = new ProjectTrackerV2Entities())
            {
                var Remarks = from s in context.projectRemarks
                              select s;
                if (!string.IsNullOrEmpty(Id))
                {
                    Remarks = Remarks.Where(s => s.projectId == Id);
                }
                else
                {
                    ViewBag.Message = "Not found";
                }



                return View(Remarks.ToList());
            }

        }




        public ActionResult Edit()
        {
            using (var context = new ProjectTrackerV2Entities())
            {
                ProjectViewModel vm = new ProjectViewModel();
                var result = (from UsersList in context.userMasters select UsersList).ToList();
                if (result != null)
                {
                    ViewBag.myUsers = result.Select(x => new SelectListItem { Text = x.userName });
                }
                var re = (from Options in context.projectStatus select Options).ToList();
                if (re != null)
                {
                    ViewBag.Options = re.Select(x => new SelectListItem { Text = x.status });
                }



                return View(vm);
            }
        }
        [HttpPost]
        public ActionResult Edit(ProjectViewModel model, string Id)
        {
            using (var context = new ProjectTrackerV2Entities())
            {
                var Project = context.projectMasters.Where(u => u.projectId == Id).FirstOrDefault();
                Project.projectName = model.ProjectName;
                Project.projectDescription = model.ProjectDescription;
                Project.status = model.ProjectStatus;
                Project.isActive = model.Active;
                Project.modifiedTimeStamp = DateTime.Now;
                Project.FPR = model.FPR;
                Project.team = model.SPR;
                Project.expectedDelDate = model.ExpectedDeliveryDate;
                Project.modifiedBy = (string)Session["UserName"];
                Project.CRNumber = model.CRNumber;



                context.SaveChanges();



                return RedirectToAction("Add");
            }
        }


    }
}