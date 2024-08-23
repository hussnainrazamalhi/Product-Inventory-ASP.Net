using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Web.Security;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(WebApplication1.Models.Membership model) 
        {
            using (var context = new OfficeEntities())
            {
                bool isValid = context.User.Any(x => x.UserName == model.UserName && x.Password == model.Password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Employees");
                }

                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new OfficeEntities())
                {
                    bool userExists = context.User.Any(x => x.UserName == model.UserName);
                    if (userExists)
                    {
                        ModelState.AddModelError("", "User is already registered");
                        return View();
                    }

                    context.User.Add(model);
                    context.SaveChanges();

                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Login");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
