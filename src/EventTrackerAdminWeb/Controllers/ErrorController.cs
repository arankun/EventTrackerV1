#region directives

using System.Web.Mvc;

#endregion

namespace EventTrackerAdminWeb.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult AccessDenied() {
            return View();
        }
    }
}