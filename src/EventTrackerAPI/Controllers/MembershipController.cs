#region directives

using System.Web.Http;
using EventTracker.BusinessModel.Membership;
using EventTracker.BusinessServices;

#endregion

namespace EventTrackerAPI.Controllers
{
    [RoutePrefix("api/membership")]
    public class MembershipController : ApiController
    {
        private readonly IMembershipServices _services;

        public MembershipController(IMembershipServices services)
        {
            _services = services;
        }

        [HttpGet]
        [Route("{memberId}")]
        public Member GetMember(int memberId)
        {
            return _services.GetMember(memberId);
        }

        [HttpPost]
        public int AddMember([FromBody] NewMember newMember)
        {
            return _services.AddMember(newMember);
        }

        [HttpPost]
        public int AddSpouseOfMember(int spouseMemberId, [FromBody] NewMember newMember)
        {
            return _services.AddSpouseOfMember(spouseMemberId, newMember);
        }

        [HttpPost]
        public int AddKfcMember(int parentId, [FromBody] NewMember newMember)
        {
            return _services.AddKfcMember(parentId,newMember);
        }
    }
}
