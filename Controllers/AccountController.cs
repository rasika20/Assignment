using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jQueryAjaxInAsp.NETMVC.Models;
using System.Web.Security;
namespace jQueryAjaxInAsp.NETMVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
       
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User Model)
        {
            using(var context = new DBModel())
            {
                bool isValid = context.Users.Any(x=>x.EmailId == Model.EmailId && x.Password == Model.Password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(Model.EmailId, false);
                    return RedirectToAction("Index", "Student");

                }
                ModelState.AddModelError("", "Invalid username and password");
            }
            return View();
        }
        public ActionResult SignUP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(User Model)
        {
            using (var context = new DBModel())
            {
                context.Users.Add(Model);
                context.SaveChanges();
            }
            return RedirectToAction("login");
        }
        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("login");
        }
    }
}