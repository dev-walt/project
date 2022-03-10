using ProjectTracker.Interfaces;
using ProjectTracker.Library;
using ProjectTracker.Models.DB;
using ProjectTracker.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace ProjectTracker.Controllers
{
    public class UserRegistrationController : Controller
    {
        // GET: UserRegistration
       /* public ActionResult Index()
        {
            return View();
        }
*/
        private IUserRegistration _IRegistration;
        private IUserRole _IRoles;
        public UserRegistrationController()
        {
            _IRegistration = new UserRegistrationManager();
            _IRoles = new UserRoleManager();
        }

        // GET: Registration/Create
        public ActionResult UserRegistration()
        {
            return View(new userMaster());
        }

        // POST: Registration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserRegistration(userMaster registration)
        {
            try
            {
                var isUsernameExists = _IRegistration.CheckUserNameExists(registration.emailId);

                if (isUsernameExists)
                {
                    ModelState.AddModelError("", errorMessage: "Username Already Used try unique one!");
                }
                else
                {
                    registration.createdTimeStamp = DateTime.Now;
/*                    registration.userRoleMappings = _IRoles.getRolesofUserbyRolename("Users");
*/                    
                    registration.password = EncryptionLibrary.EncryptText(registration.password);
/*                    registration.ConfirmPassword = EncryptionLibrary.EncryptText(registration.ConfirmPassword);
*/                    
                    if (_IRegistration.AddUser(registration) > 0)
                    {
                        TempData["MessageRegistration"] = "Data Saved Successfully!";
                        return RedirectToAction("UserRegistration");
                    }
                    else
                    {
                        return View(registration);
                    }
                }
                return RedirectToAction("UserRegistration");
            }
            catch
            {
                return View(registration);
            }
        }

        public JsonResult CheckUserNameExists(string Username)
        {
            try
            {
                var isUsernameExists = false;

                if (Username != null)
                {
                    isUsernameExists = _IRegistration.CheckUserNameExists(Username);
                }

                if (isUsernameExists)
                {
                    return Json(data: true);
                }
                else
                {
                    return Json(data: false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}