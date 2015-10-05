using System.Collections.Generic;
using EventTracker.BusinessModel.Menus;

namespace EventTracker.BusinessServices
{
    public class MenuServices:IMenuServices
    {
        public MenuServices()
        {
        }

        public IEnumerable<MenuItem> GetUserMenus()
        {
            var eventMenu = new MenuItem()
            {
                Id = 1,
                Name = "Events",
                NavigateUrl = "/Events",
                Order = 1,
                SubMenuItems = new List<MenuItem>()
                {
                    new MenuItem {Name="Current/Future Events", NavigateUrl = "/Events"}
                }
            };

            var memberMenu = new MenuItem() {
                Id = 1,
                Name = "Members",
                NavigateUrl = "/Membership",
                Order = 2
            };

            var memberMenuAngularJs = new MenuItem() {
                Id = 1,
                Name = "Members Ajs",
                NavigateUrl = "#/members",
                Order = 2
            };

            var menuList = new List<MenuItem>() {eventMenu, memberMenu, memberMenuAngularJs };
            return menuList;
        }

    }
}