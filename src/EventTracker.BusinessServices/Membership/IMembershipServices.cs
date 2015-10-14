#region directives

using System.Collections.Generic;
using EventTracker.BusinessModel;
using EventTracker.BusinessModel.Common;
using EventTracker.BusinessModel.Criterias;
using EventTracker.BusinessModel.Membership;
using PagedList;

#endregion

namespace EventTracker.BusinessServices.Membership
{
    public interface IMembershipServices
    {
        int AddMember(NewMember newMember);
        int AddSpouseOfMember(int spouseMemberId, NewMember newMember);
        int AddKfcMember(int parentId, NewMember newMember);
        Member GetMember(int memberId);
        IEnumerable<Member> GetMembers(int pageIndex, int pageSize, out int numberOfPages);
        //IEnumerable<Member> GetFamilyMembersByHeadOfFamilyMemberId(int headOfFamilyMemberId);

        IPagedList<Member> GetMembers(int pageIndex, int pageSizes);
        void UpdateMember(Member aMember);
        Common.PagedList GetMembers(SearchParameter searchParam);



        //HouseHold GetHouseHoldViewModel(int houseHoldId);
        //IPagedList<HouseHold> GetHouseHolds(HouseHoldCriteria houseHoldCriteria, PagingInfo pagingInfo);
        //IEnumerable<Member> GetHouseHoldMemers(int houseHoldId);

        //IPagedList<Member> GetHouseHoldMemers(int houseHoldId, int pageNumber, int pageSize);


        IEnumerable<MembershipHistory> GetMembershipHistory(int memberid);
    }
}