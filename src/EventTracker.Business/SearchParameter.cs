using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTracker.BusinessModel {
    public class SearchParameter {
        public string SortDirection { get; set; }
        public string SortBy { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchText { get; set; }
    }
}
