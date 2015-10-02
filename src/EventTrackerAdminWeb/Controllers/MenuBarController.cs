using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventTracker.BusinessModel.Menus;
using EventTracker.BusinessServices;

namespace EventTrackerAdminWeb.Controllers
{
    public class MenuBarController : Controller
    {
        private readonly IMenuServices _service;

        public MenuBarController(IMenuServices service)
        {
            _service = service;
        }

        [ChildActionOnly]
        public PartialViewResult GetMenuItems()
        {
            var menus = _service.GetUserMenus();

            return PartialView("_MenuBar", menus);
        }

        public PartialViewResult GetSubMenuItems(int menucode)
        {
            throw new NotImplementedException();
        }
    }
}