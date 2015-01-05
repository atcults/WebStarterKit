using System.Linq;
using Core.ViewOnly.Base;

namespace Core.ViewOnly.DbType
{
    internal class SqlServerDatabaseType : DatabaseType
    {
        public override string BuildPageQuery(long skip, long take, PagingHelper.SqlParts parts, ref object[] args)
        {
            parts.SqlSelectRemoved = PagingHelper.RxOrderBy.Replace(parts.SqlSelectRemoved, "", 1);
            if (PagingHelper.RxDistinct.IsMatch(parts.SqlSelectRemoved))
            {
                parts.SqlSelectRemoved = "peta_inner.* FROM (SELECT " + parts.SqlSelectRemoved + ") peta_inner";
            }
            var sqlPage =
                string.Format(
                    "SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) peta_rn, {1}) peta_paged WHERE peta_rn>@{2} AND peta_rn<=@{3}",
                    parts.SqlOrderBy ?? "ORDER BY (SELECT NULL)", parts.SqlSelectRemoved, args.Length, args.Length + 1);
            args = args.Concat(new object[] {skip, skip + take}).ToArray();

            return sqlPage;
        }

        public override string GetExistsSql()
        {
            return "IF EXISTS (SELECT 1 FROM {0} WHERE {1}) SELECT 1 ELSE SELECT 0";
        }
    }
}