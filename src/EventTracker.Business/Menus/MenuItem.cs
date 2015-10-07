#region directives

using System.Collections.Generic;

#endregion

namespace EventTracker.BusinessModel.Menus {
    public class MenuItem {
        public int Order { get; set; }
        public string Name { get; set; }
        public string NavigateUrl { get; set; }

        public IEnumerable<MenuItem> SubMenuItems { get; set; }
        public int Id { get; set; }

        public int ParentId { get; set; }
    }
}
