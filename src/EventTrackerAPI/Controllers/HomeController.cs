#region directives

using System.Web.Mvc;

#endregion

namespace EventTrackerAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
