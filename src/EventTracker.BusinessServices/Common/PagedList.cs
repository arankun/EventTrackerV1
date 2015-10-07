#region directives

using System;
using System.Collections;

#endregion

namespace EventTracker.BusinessServices.Common
{
    public class PagedList
    {
        public IList Content { get; set; }

        public Int32 CurrentPage { get; set; }
        public Int32 PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int TotalPages {
            get { return (int)Math.Ceiling((decimal)TotalRecords / PageSize); }
        }
    }
}