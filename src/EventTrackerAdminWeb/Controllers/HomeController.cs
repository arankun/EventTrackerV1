#region directives

using System.Web.Mvc;

#endregion

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