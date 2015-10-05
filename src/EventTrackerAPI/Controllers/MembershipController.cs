#region directives

using System.Security.AccessControl;
using System.Web.Http;
using EventTracker.BusinessModel;
using EventTracker.BusinessModel.Membership;
using EventTracker.BusinessServices;
using EventTracker.BusinessServices.Common;

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


        [HttpGet]
        [Route("getmembers")]
        public PagedList GetMembers(string searchtext = "", int page = 1, int pageSize = 10, string sortBy = "lastname", string sortDirection = "asc")
        {
            var searchParam = new SearchParameter()
            {
                SearchText = searchtext,
                PageNumber = page,
                PageSize = 10,
                SortBy = sortBy,
                SortDirection = sortDirection
            };
            return _services.GetMembers(searchParam);
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
