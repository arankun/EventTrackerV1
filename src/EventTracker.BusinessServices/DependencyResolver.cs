#region directives

using System.ComponentModel.Composition;
using EventTracker.BusinessServices.Authentication;
using EventTracker.BusinessServices.Membership;
using EventTracker.Resolver;

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
            registerComponent.RegisterType<IEventAttendanceServices, EventAttendanceServices>();
            registerComponent.RegisterType<IEventServices, EventServices>();
            registerComponent.RegisterType<IMembershipServices, MembershipServices>();
            registerComponent.RegisterType<IMenuServices, MenuServices>();
            registerComponent.RegisterType<IAuthenticationService, AuthenticationService>();
            registerComponent.RegisterType<IHouseHoldServices, HouseHoldServices>();

        }
    }
}