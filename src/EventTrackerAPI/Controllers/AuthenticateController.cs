#region directives

using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventTracker.BusinessServices;
using EventTrackerAPI.Filters;

#endregion

namespace EventTrackerAPI.Controllers
{
    [ApiAuthenticationFilter(true)]
    public class AuthenticateController : ApiController
    {
        private readonly ITokenServices _tokenServices;

        public AuthenticateController(ITokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }

        [HttpPost]
        public IHttpActionResult Authenticate()
        {
            if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var userId = basicAuthenticationIdentity.UserId;
                    return GetAuthToken(userId);
                }
            }
            return Unauthorized();
        }

        private IHttpActionResult GetAuthToken(int userId)
        {
            var token = _tokenServices.GenerateToken(userId);
            var responseMsg = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
            responseMsg.Headers.Add("Token", token.AuthToken);
            responseMsg.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["AuthTokenExpiry"]);
            responseMsg.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
            IHttpActionResult actionResult = ResponseMessage(responseMsg);
            return actionResult;
        }
    }
}
