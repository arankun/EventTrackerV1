#region directives

using System.Collections.Generic;
using EventTracker.BusinessModel.Menus;

#endregion

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
                    new MenuItem {Name="Current/Future Events", NavigateUrl = "/Events/Index"}
                }
            };

            var memberMenu = new MenuItem() {
                Id = 1,
                Name = "Members",
                NavigateUrl = "/Membership",
                Order = 2,
                SubMenuItems = new List<MenuItem>()
                {
                    new MenuItem {Name="All", NavigateUrl = "/Membership/Members"},
                    new MenuItem {Name="SFC", NavigateUrl = "/Membership/Members?memberOf=SFC"},
                    new MenuItem {Name="YFC", NavigateUrl = "/Membership/Members?memberOf=YFC"},
                    new MenuItem {Name="KFC", NavigateUrl = "/Membership/Members?memberOf=KFC"},
                    new MenuItem {Name="HOLD", NavigateUrl = "/Membership/Members?memberOf=HOLD"},
                    new MenuItem {Name="SOLD", NavigateUrl = "/Membership/Members?memberOf=SOLD"},
                    new MenuItem {Name="Households", NavigateUrl = "/Household/List"}
                }
            };

            var adminMenu = new MenuItem()
            {
                Id = 1,
                Name = "Admin",
                NavigateUrl = "/Admin",
                Order = 2,
                SubMenuItems = new List<MenuItem>()
                {
                    new MenuItem {Name="Members", NavigateUrl = "/Admin/Members"},
                    new MenuItem {Name="Households", NavigateUrl = "/Admin/Households"},
                    new MenuItem {Name="Events", NavigateUrl = "/Admin/Events"},
                    new MenuItem {Name="Members Ang", NavigateUrl =  "#/members"}
                }
            };


            //var memberMenuAngularJs = new MenuItem() {
            //    Id = 1,
            //    Name = "Members Ajs",
            //    NavigateUrl = "#/members",
            //    Order = 2
            //};

            var menuList = new List<MenuItem>() {eventMenu, memberMenu, adminMenu };
            return menuList;
        }
    }
}