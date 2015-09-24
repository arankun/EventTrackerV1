#region directives

using EventTracker.Resolver;
using System.ComponentModel.Composition;

#endregion

namespace EventTracker.BusinessServices
{
    [Export(typeof(IComponent))]
    public class DependencyResolver:IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            //AT: Register Service components here
            registerComponent.RegisterType<IEventServices, EventServices>();
            registerComponent.RegisterType<IUserServices, UserServices>();
            registerComponent.RegisterType<ITokenServices, TokenServices>();
        }
    }
}