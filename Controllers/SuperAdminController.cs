using ProjectTracker.Interfaces;
using ProjectTracker.Library;
using ProjectTracker.Models.DB;
using ProjectTracker.Models.EntityManager;
using ProjectTracker.Models.ViewModel;
using ProjectTracker.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectTracker.Controllers
{
    [SuperAdminSessions]
    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin
        private IUserRegistration _IUserRegistration;
        private IUserRole _IUserRole;
        private IApproverResourceMapping _IApproverResourceMapping;
        //private ICacheManager _ICacheManager;
        private IUsers _IUsers;
        private IProject _IProject;


        public SuperAdminController()
        {
            _IUserRegistration = new UserRegistrationManager();
            _IUserRole = new UserRoleManager();
            _IApproverResourceMapping = new ApproverResourceMappingManager();
           // _ICacheManager = new ICacheManager();
            _IUsers = new UsersManager();
            _IProject = new ProjectManager();
        }

        // GET: SuperAdmin
        public ActionResult Dashboard()
        {
            try
            {

            
                    var admincount = _IUsers.GetTotalAdminsCount();
           
                    ViewBag.AdminCount = admincount;
              
                

               
                    var userscount = _IUsers.GetTotalUsersCount();
                   
                    ViewBag.UsersCount = userscount;
               

                
                    var projectcount = _IProject.GetTotalProjectsCounts();
                    
                    ViewBag.ProjectCount = projectcount;
           

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult CreateAdmin()
        {
            return View(new userMaster());
        }

        [HttpPost]
        public ActionResult CreateAdmin(userMaster registration)
        {

            try
            {
                var isUsernameExists = _IUserRegistration.CheckUserNameExists(registration.userName);

                if (isUsernameExists)
                {
                    ModelState.AddModelError("", errorMessage: "Username Already Used try unique one!");
                }
                else
                {
                    registration.createdTimeStamp = DateTime.Now;
                    //registration.userRoleMappings. = _IUserRole.getRolesofUserbyRolename("Admin");
                    registration.password = EncryptionLibrary.EncryptText(registration.password);
                    //registration.ConfirmPassword = EncryptionLibrary.EncryptText(registration.ConfirmPassword);
                    if (_IUserRegistration.AddUser(registration) > 0)
                    {
                        TempData["MessageRegistration"] = "Data Saved Successfully!";
                        return RedirectToAction("CreateAdmin");
                    }
                    else
                    {
                        return View("CreateAdmin", registration);
                    }
                }

                return RedirectToAction("Dashboard");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult AssignRoles()
        {
            try
            {
                ApproverResourceMapping assignRolesModel = new ApproverResourceMapping();
                assignRolesModel.ListofAdmins = _IApproverResourceMapping.ListofAdmins();
                var name = assignRolesModel.ListofAdmins.FirstOrDefault();
                assignRolesModel.ListofUser = _IApproverResourceMapping.GetListofUnAssignedUsers();
                return View(assignRolesModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult AssignRoles(ApproverResourceMapping map)
        {
            try
            {
                if (map.ListofUser == null)
                {
                    TempData["MessageErrorRoles"] = "There are no Users to Assign Roles";
                    map.ListofAdmins = _IApproverResourceMapping.ListofAdmins();
                    map.ListofUser = _IApproverResourceMapping.GetListofUnAssignedUsers();
                    return View(map);
                }


                var SelectedCount = (from User in map.ListofUser
                                     where User.selectedUsers == true
                                     select User).Count();

                if (SelectedCount == 0)
                {
                    TempData["MessageErrorRoles"] = "You have not Selected any User to Assign Roles";
                    map.ListofAdmins = _IApproverResourceMapping.ListofAdmins();
                    map.ListofUser = _IApproverResourceMapping.GetListofUnAssignedUsers();
                    return View(map);
                }

                if (ModelState.IsValid)
                {
                    map.createdBy = Convert.ToString(Session["SuperAdmin"]);
                    _IApproverResourceMapping.SaveAssignedRoles(map);
                    TempData["MessageRoles"] = "Approver Assigned Successfully!";
                }

                map = new ApproverResourceMapping();
                map.ListofAdmins = _IApproverResourceMapping.ListofAdmins();
                map.ListofUser = _IApproverResourceMapping.GetListofUnAssignedUsers();

                return RedirectToAction("AssignRoles");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}