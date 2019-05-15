using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AdminSystem model, string returnUrl)
        {

            FinalProjectEntities db = new FinalProjectEntities();
            var user = db.AdminSystems.Where(x => x.UserName == model.UserName && x.UserPassword == model.UserPassword).FirstOrDefault();
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && returnUrl.StartsWith("/\\"))
                {
                    
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.error = "Please check UserName or password!";
                return View();
            }

        }
        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
    }
}