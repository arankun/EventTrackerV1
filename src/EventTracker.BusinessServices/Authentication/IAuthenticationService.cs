#region directives

using EventTracker.BusinessModel.Authentication;

#endregion

namespace EventTracker.BusinessServices.Authentication
{
    public interface IAuthenticationService
    {
        CustomPrincipalSerializeModel AuthenticateUser(string loginId, string password);
    }
}