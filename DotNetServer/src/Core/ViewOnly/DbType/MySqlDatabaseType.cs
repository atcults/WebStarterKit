using System;

namespace Core.ViewOnly.DbType
{
    internal class MySqlDatabaseType : DatabaseType
    {
        public override string GetParameterPrefix(string connectionString)
        {
            if (connectionString != null &&
                connectionString.IndexOf("Allow User Variables=true", StringComparison.Ordinal) >= 0)
                return "?";
            return "@";
        }

        public override string EscapeSqlIdentifier(string str)
        {
            return string.Format("`{0}`", str);
        }

        public override string GetExistsSql()
        {
            return "SELECT EXISTS (SELECT 1 FROM {0} WHERE {1})";
        }
    }
}