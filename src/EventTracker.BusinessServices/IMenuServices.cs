using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventTracker.BusinessModel.Menus;

namespace EventTracker.BusinessServices {
    public interface IMenuServices
    {
        IEnumerable<MenuItem> GetUserMenus();
    }
}
