using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
