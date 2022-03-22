using ProjectTracker.Interfaces;
using ProjectTracker.Library;
using ProjectTracker.Models.DB;
using ProjectTracker.Models.EntityManager;
using ProjectTracker.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProjectTracker.Controllers
{
    public class UserLoginController : Controller
    {
        // GET: UserLogin
       /* public ActionResult Index()
        {
            return View();
        }*/
        private IUserLogin _ILogin;
        private IApproverResourceMapping _IAssignRoles;
/*        private ICacheManager _ICacheManager;
*/        public UserLoginController()
        {
            _ILogin = new UserLoginManager();
            _IAssignRoles = new ApproverResourceMappingManager();
/*            _ICacheManager = new CacheManager();
*/        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginView loginViewModel)
        {
            try
            {
               /* if (!this.IsCaptchaValid("Captcha is not valid"))
                {
                    ViewBag.errormessage = "Error: captcha entered is not valid.";

                    return View(loginViewModel);
                }*/

                if (!string.IsNullOrEmpty(loginViewModel.emailId) && !string.IsNullOrEmpty(loginViewModel.password))
                {
                    var emailId = loginViewModel.emailId;
//                   var password = EncryptionLibrary.EncryptText(loginViewModel.password);
                   var password = loginViewModel.password;


                    var result = _ILogin.ValidateUser(emailId, password);

                    if (result != null)
                    {
                        if (result.userId == null)
                        {
                            ViewBag.errormessage = "Invalid UserName or Password";
                        }
                        else
                        {
                            DatabaseContext db = new DatabaseContext();
                            string roleName = db.UserRoleMapping.SingleOrDefault(a => a.userId == result.userId).roleMaster.roleName;
                            string roleId = db.RoleMaster.SingleOrDefault(a => a.roleName == roleName).roleId;
                            remove_Anonymous_Cookies(); //Remove Anonymous_Cookies

                            Session["roleId"] = Convert.ToString(roleId);
                            Session["Username"] = Convert.ToString(result.userName);
                            if (roleName == "Admin")
                            {
                                Session["AdminUser"] = Convert.ToString(result.userId);

                                /*if (result.ForceChangePassword == 1)
                                {
                                    return RedirectToAction("ChangePassword", "UserProfile");
                                }*/

                                return RedirectToAction("Dashboard", "Admin");
                            }
                            else if (roleName == "User")
                            {
/*                                if (!_IAssignRoles.CheckIsUserAssignedRole(result.userId))
                                {
                                    ViewBag.errormessage = "Approval Pending";
                                    return View(loginViewModel);
                                }*/

                                Session["UserID"] = Convert.ToString(result.userId);

                                /*if (result.ForceChangePassword == 1)
                                {
                                    return RedirectToAction("ChangePassword", "UserProfile");
                                }*/

                                return RedirectToAction("Dashboard", "User");
                            }
                            else if (roleName == "SuperAdmin")
                            {
                                Session["SuperAdmin"] = Convert.ToString(result.userId);
                                return RedirectToAction("Dashboard", "SuperAdmin");
                            }
                        }
                    }
                    else
                    {
                        ViewBag.errormessage = "Invalid Username and Password";
                        return View(loginViewModel);
                    }
                }
                return View(loginViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {

            try
            {
/*                if (!string.IsNullOrEmpty(Convert.ToString(Session["SuperAdmin"])))
                {
                    _ICacheManager.Clear("AdminCount");
                    _ICacheManager.Clear("UsersCount");
                    _ICacheManager.Clear("ProjectCount");
                }*/

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
                Response.Cache.SetNoStore();

                HttpCookie Cookies = new HttpCookie("WebTime");
                Cookies.Value = "";
                Cookies.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(Cookies);
                HttpContext.Session.Clear();
                Session.Abandon();
                return RedirectToAction("Login", "UserLogin");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [NonAction]
        public void remove_Anonymous_Cookies()
        {
            try
            {

                if (Request.Cookies["WebTime"] != null)
                {
                    var option = new HttpCookie("WebTime");
                    option.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(option);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}