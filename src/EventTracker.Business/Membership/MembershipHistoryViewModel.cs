using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EventTracker.BusinessModel.Membership {
    public class MembershipHistoryViewModel {
        public IEnumerable<MembershipHistory> MembershipHistory { get; set; }
        public SelectList MembershipOptions { get; set; }
        public int MemberId { get; set; }
        public string MemberOf { get; set; }
    }
}
