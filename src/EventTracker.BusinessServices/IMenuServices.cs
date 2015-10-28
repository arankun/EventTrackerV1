#region directives

using System.Collections.Generic;
using EventTracker.BusinessModel.Menus;

#endregion

namespace EventTracker.BusinessServices {
    public interface IMenuServices
    {
        IEnumerable<MenuItem> GetUserMenus();
        IEnumerable<MenuItem> GetUserMenus(List<string> roles);
    }
}
