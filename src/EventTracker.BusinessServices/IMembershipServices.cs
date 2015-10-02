#region directives

using System.Collections.Generic;
using EventTracker.BusinessModel.Membership;

#endregion

namespace EventTracker.BusinessServices
{
    public interface IMembershipServices
    {
        int AddMember(NewMember newMember);
        int AddSpouseOfMember(int spouseMemberId, NewMember newMember);
        int AddKfcMember(int parentId, NewMember newMember);
        Member GetMember(int memberId);
        IEnumerable<Member> GetMembers(int pageIndex, int pageSize);
        void UpdateMember(Member aMember);
    }
}