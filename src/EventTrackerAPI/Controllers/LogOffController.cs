#region directives

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventTracker.BusinessServices;
using EventTrackerAPI.ActionFilters;

#endregion

namespace EventTrackerAPI.Controllers
{
    [AuthorizationRequired]
    public class LogOffController : ApiController
    {
        private readonly ITokenServices _tokenServices;

        public LogOffController(ITokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }

        [HttpPost]
        [Route("api/logoff")]
        public IHttpActionResult LogOff()
        {
            var headers = Request.Headers;
            var token = string.Empty;
            if (headers.Contains("Token"))
            {
                token = headers.GetValues("Token").First();
            }
            return Expire(token);
        }

        private IHttpActionResult Expire(string tokenId)
        {
            var deleted = _tokenServices.Kill(tokenId);
            if (deleted)
            {
                var responseMsg = Request.CreateResponse(HttpStatusCode.OK, "Logoff");
                responseMsg.Headers.Add("Token", string.Empty);
                IHttpActionResult actionResult = ResponseMessage(responseMsg);
                return actionResult;
            }
            return NotFound();
        }
    }
}
