using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EventTrackerAdminWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Index", "Account");
        }
    }
}