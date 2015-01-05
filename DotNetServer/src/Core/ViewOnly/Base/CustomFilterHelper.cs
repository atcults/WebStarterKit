using System;
using System.Linq;

namespace Core.ViewOnly.Base
{
    public static class CustomFilterHelper
    {
        private static void ApplyFilter(this Sql query, string filterType, string columnName, string value)
        {
            var splittedValues = value.Split(',');

            switch (filterType)
            {
                case "like":
                    query.Where("(" + columnName + " like @0)", GetLikeValue(value));
                    break;
                case "exact":
                    query.Where("(" + columnName + " COLLATE Latin1_General_CS_AS = @0 COLLATE Latin1_General_CS_AS)",
                        value);
                    break;
                case "notequal":
                    query.Where("(" + columnName + " <> @0)", value);
                    break;
                case "lessthan":
                    query.Where("(" + columnName + " < @0)", value);
                    break;
                case "lessorequal":
                    query.Where("(" + columnName + " <= @0)", value);
                    break;
                case "greaterthan":
                    query.Where("(" + columnName + " > @0)", value);
                    break;
                case "greaterorequal":
                    query.Where("(" + columnName + " >= @0)", value);
                    break;
                case "in":
                    query.Where("(" + columnName + " in (@tags))", new {tags = value});
                    break;
                case "between":
                    if (splittedValues.Length == 2)
                    {
                        query.Where("(" + columnName + " between @0 and @1)", splittedValues[0], splittedValues[1]);
                    }
                    else
                    {
                        throw new Exception("between query does not pass with left and right values");
                    }
                    break;
                default: //''equal
                    query.Where("(" + columnName + " = @0)", value);
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