using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventTracker.BusinessModel.Membership;

namespace EventTracker.BusinessServices.Membership {
    public static class Extensions {
        public static IQueryable<HouseHold> WithAreaIfNotEmpty(this IQueryable<HouseHold> source,  string value) {
            if (string.IsNullOrEmpty(value))
                return source;
            else
                return source.Where(x => x.Area == value);
        }
    }
}
