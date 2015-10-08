#region directives

using System.Web.Mvc;
using EventTracker.BusinessModel.Authentication;

#endregion

namespace EventTrackerAdminWeb.Controllers
{
    public class BaseController : Controller
    {
        protected virtual new CustomPrincipal User {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}