using System.Linq;
using Common.Base;
using Common.Extensions;
using Core.ViewOnly.Base;
using Dto.ApiRequests;

namespace Core.ViewOnly
{
    public static class ViewRepositoryHelper
    {
        //public static void BuildQuery(this SearchSpecification specification, Sql query, string dateColumn)
        //{
        //    if (!string.IsNullOrEmpty(specification.ColumnName) && !string.IsNullOrEmpty(specification.FilterType))
        //    {
        //        specification.ColumnValue = specification.ColumnValue ?? "";
        //        var splittedValues = specification.ColumnValue.Split(',').Select(a => a.Trim()).ToArray();

        //        switch (specification.FilterType.ToLower())
        //        {
        //            case "like":
        //                query.Where("(" + specification.ColumnName + " like @0)",
        //                    GetLikeValue(specification.ColumnValue));
        //                break;
        //            case "exact":
        //                query.Where(
        //                    "(" + specification.ColumnName +
        //                    " COLLATE Latin1_General_CS_AS = @0 COLLATE Latin1_General_CS_AS)",
        //                    specification.ColumnValue);
        //                break;
        //            case "notequal":
        //                query.Where("(" + specification.ColumnName + " <> @0)", specification.ColumnValue);
        //                break;
        //            case "lessthan":
        //                query.Where("(" + specification.ColumnName + " < @0)", specification.ColumnValue);
        //                break;
        //            case "lessorequal":
        //                query.Where("(" + specification.ColumnName + " <= @0)", specification.ColumnValue);
        //                break;
        //            case "greaterthan":
        //                query.Where("(" + specification.ColumnName + " > @0)", specification.ColumnValue);
        //                break;
        //            case "greaterorequal":
        //                query.Where("(" + specification.ColumnName + " >= @0)", specification.ColumnValue);
        //                break;
        //            case "in":
        //                query.Where("(" + specification.ColumnName + " in (@tags))", new {tags = splittedValues});
        //                break;
        //            case "between":
        //                if (splittedValues.Length == 2)
        //                {
        //                    query.Where("(" + specification.ColumnName + " between @0 and @1)", splittedValues[0],
        //                        splittedValues[1]);
        //                }
        //                else
        //                {
        //                    Logger.Log(LogType.Error, typeof (ViewRepositoryHelper),
        //                        "between query does not pass with left and right values");
        //                }
        //                break;
        //            default: //''equal
        //                query.Where("(" + specification.ColumnName + " = @0)", specification.ColumnValue);
        //                break;
        //        }
        //    }

        //    dateColumn = string.IsNullOrEmpty(specification.DateColumn) ? dateColumn : specification.DateColumn;

        //    if (specification.StartDate.HasValue)
        //    {
        //        query.Where("(" + dateColumn + " >= @0)", specification.StartDate);
        //    }

        //    if (specification.EndDate.HasValue)
        //    {
        //        query.Where("(" + dateColumn + " <= @0)", specification.EndDate.GetValueOrDefault().AddDays(1).AddSeconds(-1));
        //    }

        //    if (specification.SortColumn.IsNotEmpty())
        //    {
        //        query.OrderBy(string.Format("{0} {1}", specification.SortColumn,
        //            specification.SortReverse ? "DESC" : "ASC"));
        //    }
        //}

        public static void BuildQuery(this SimpleSearch specification, Sql query)
        {
            foreach (var searchItem in specification.SearchItems)
            {
                AddSearchItem(searchItem, query);
            }

            if (specification.OrderByColumns.IsNotEmpty())
            {
                query.OrderBy(string.Format("{0} {1}", specification.OrderByColumns, specification.OrderAsc ? "ASC" : "DESC"));
            }
        }

        private static void AddSearchItem(SimpleSearch.SearchItem searchItem, Sql query)
        {
            if (string.IsNullOrEmpty(searchItem.ColumnName) || string.IsNullOrEmpty(searchItem.FilterType)) return;
            
            searchItem.ColumnValue = searchItem.ColumnValue ?? "";
            var splittedValues = searchItem.ColumnValue.Split(',').Select(a => a.Trim()).ToArray();

            switch (searchItem.FilterType.ToLower())
            {
                case "like":
                    query.Where("(" + searchItem.ColumnName + " like @0)",
                        GetLikeValue(searchItem.ColumnValue));
                    break;
                case "exact":
                    query.Where(
                        "(" + searchItem.ColumnName +
                        " COLLATE Latin1_General_CS_AS = @0 COLLATE Latin1_General_CS_AS)",
                        searchItem.ColumnValue);
                    break;
                case "notequal":
                    query.Where("(" + searchItem.ColumnName + " <> @0)", searchItem.ColumnValue);
                    break;
                case "lessthan":
                    query.Where("(" + searchItem.ColumnName + " < @0)", searchItem.ColumnValue);
                    break;
                case "lessorequal":
                    query.Where("(" + searchItem.ColumnName + " <= @0)", searchItem.ColumnValue);
                    break;
                case "greaterthan":
                    query.Where("(" + searchItem.ColumnName + " > @0)", searchItem.ColumnValue);
                    break;
                case "greaterorequal":
                    query.Where("(" + searchItem.ColumnName + " >= @0)", searchItem.ColumnValue);
                    break;
                case "in":
                    query.Where("(" + searchItem.ColumnName + " in (@tags))", new { tags = splittedValues });
                    break;
                case "between":
                    if (splittedValues.Length == 2)
                    {
                        query.Where("(" + searchItem.ColumnName + " between @0 and @1)", splittedValues[0],
                            splittedValues[1]);
                    }
                    else
                    {
                        Logger.Log(LogType.Error, typeof(ViewRepositoryHelper),
                            "between query does not pass with left and right values");
                    }
                    break;
                default: //''equal
                    query.Where("(" + searchItem.ColumnName + " = @0)", searchItem.ColumnValue);
                    break;
            }
        }

        public static string GetLikeValue(string value)
        {
            var lastChar = value.Last();
            if (lastChar != '%') value += "%";
            return value;
        }
    }
}

//if (!(specification.ColumnName.HasValue() || 
//      specification.ColumnValue.HasValue() || 
//      specification.DateColumn.HasValue() || 
//      specification.StartDate.HasValue || 
//      specification.EndDate.HasValue ||
//      specification.IsActive.HasValue))
//{
//    query.Where("1 = 0");
//    return;
//}