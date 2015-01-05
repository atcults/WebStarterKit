namespace Dto.ApiRequests
{
    //Equal, ExactEqual, NotEqual, LessThan, GreaterThan, LeassOrEqual, GreaterOrEqual, In, Like, Between
    public class SimpleSearch
    {
        public class SearchItem
        {
            public string FilterType { get; set; }
            public string ColumnName { get; set; }
            public string ColumnValue { get; set; }
        }

        public static SimpleSearch FromDepricated(SearchSpecification specification)
        {
            if (specification == null)
            {
                specification = new SearchSpecification
                {
                    ColumnName = "Name",
                    ColumnValue = "%",
                    FilterType = "like"
                };
            }

            var search = new SimpleSearch
            {
                Page = specification.Page,
                PageSize = specification.PageSize,
                OrderByColumns = specification.SortColumn,
                OrderAsc = !specification.SortReverse,
                SearchItems = new[]
                {
                    new SearchItem
                    {
                        ColumnName = specification.ColumnName,
                        FilterType = specification.FilterType,
                        ColumnValue = specification.ColumnValue
                    }
                }
            };

            return search;
        }

        public SearchItem[] SearchItems { get; set; }

        public string OrderByColumns { get; set; }
        public bool OrderAsc { get; set; }

        public long Page { get; set; }
        public long PageSize { get; set; }
    }
}