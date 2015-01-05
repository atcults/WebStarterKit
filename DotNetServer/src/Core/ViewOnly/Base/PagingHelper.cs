using System.Text.RegularExpressions;

namespace Core.ViewOnly.Base
{
    internal static class PagingHelper
    {
        public static Regex RxColumns =
            new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b",
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        public static Regex RxOrderBy =
            new Regex(
                @"\bORDER\s+BY\s+(?!.*?(?:\)|\s+)AS\s)(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*",
                RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline |
                RegexOptions.Compiled);

        public static Regex RxDistinct = new Regex(@"\ADISTINCT\s",
            RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        public static bool SplitSql(string sql, out SqlParts parts)
        {
            parts.Sql = sql;
            parts.SqlSelectRemoved = null;
            parts.SqlCount = null;
            parts.SqlOrderBy = null;

            // Extract the columns from "SELECT <whatever> FROM"
            var m = RxColumns.Match(sql);
            if (!m.Success)
                return false;

            // Save column list and replace with COUNT(*)
            var g = m.Groups[1];
            parts.SqlSelectRemoved = sql.Substring(g.Index);

            if (RxDistinct.IsMatch(parts.SqlSelectRemoved))
                parts.SqlCount = sql.Substring(0, g.Index) + "COUNT(" + m.Groups[1].ToString().Trim() + ") " +
                                 sql.Substring(g.Index + g.Length);
            else
                parts.SqlCount = sql.Substring(0, g.Index) + "COUNT(*) " + sql.Substring(g.Index + g.Length);


            // Look for the last "ORDER BY <whatever>" clause not part of a ROW_NUMBER expression
            m = RxOrderBy.Match(parts.SqlCount);

            if (!m.Success) return true;

            g = m.Groups[0];
            parts.SqlOrderBy = g.ToString();
            parts.SqlCount = parts.SqlCount.Substring(0, g.Index) + parts.SqlCount.Substring(g.Index + g.Length);

            return true;
        }

        public struct SqlParts
        {
            public string Sql;
            public string SqlCount;
            public string SqlOrderBy;
            public string SqlSelectRemoved;
        }
    }
}