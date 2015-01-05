using System;
using System.Linq;
using System.Text.RegularExpressions;
using Core.ViewOnly.DbType;

namespace Core.ViewOnly.Base
{
    internal static class AutoSelectHelper
    {
        private static readonly Regex RxSelect = new Regex(@"\A\s*(SELECT|EXECUTE|CALL)\s",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private static readonly Regex RxFrom = new Regex(@"\A\s*FROM\s",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public static string AddSelectClause<T>(DatabaseType databaseType, string sql)
        {
            if (sql.StartsWith(";")) return sql.Substring(1);

            if (RxSelect.IsMatch(sql)) return sql;
            var pd = PocoData.ForType(typeof (T));
            var tableName = databaseType.EscapeViewName(pd.ViewInfo.ViewName);
            var cols = pd.Columns.Count != 0
                ? string.Join(", ",
                    (from c in pd.ResultColumns select tableName + "." + databaseType.EscapeSqlIdentifier(c)).ToArray())
                : "NULL";
            sql = !RxFrom.IsMatch(sql)
                ? string.Format("SELECT {0} FROM {1} {2}", cols, tableName, sql)
                : string.Format("SELECT {0} {1}", cols, sql);
            return sql;
        }

        public static bool CanSearchThisProperty<T>(string propertyName)
        {
            var pd = PocoData.ForType(typeof (T));
            return pd.SearchableColumns.Any(s => s.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        }
    }
}