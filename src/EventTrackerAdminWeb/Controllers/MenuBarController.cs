#region directives

using System;
using System.Web.Mvc;
using EventTracker.BusinessServices;
using EventTracker.BusinessModel.Menus;

#endregion

namespace EventTrackerAdminWeb.Controllers
{
    public class MenuBarController : BaseController
    {
        private readonly IMenuServices _service;

        public MenuBarController(IMenuServices service)
        {
            _service = service;
        }

        [ChildActionOnly]
        public PartialViewResult GetMenuItems()
        {
            if (User == null)
            {
                var menus = _service.GetUserMenus();
                return PartialView("_MenuBar", menus);
            }
            else
            {
                var  menus = _service.GetUserMenus(User.Roles);
                return PartialView("_MenuBar", menus);
            }
            
        }

        public PartialViewResult GetSubMenuItems(int menucode)
        {
            throw new NotImplementedException();
        }
    }
}