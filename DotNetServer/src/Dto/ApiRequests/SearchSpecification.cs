using System;

namespace Dto.ApiRequests
{
    public class SearchSpecification
    {
        public string FilterType { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
        public string SortColumn { get; set; }
        public bool SortReverse { get; set; }
        public string DateColumn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long Page { get; set; }
        public long PageSize { get; set; }

        public override string ToString()
        {
            return string.Format("{1} {0} {2} and {3} between {4} and {5} ", FilterType, ColumnName, ColumnValue, DateColumn, StartDate, EndDate);
        }
    }
}
