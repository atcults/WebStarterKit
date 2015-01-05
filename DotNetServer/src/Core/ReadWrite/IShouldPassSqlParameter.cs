using System.Data.SqlClient;

namespace Core.ReadWrite
{
    public interface IShouldPassSqlParameter
    {
        void To(SqlCommand cmd);
    }
}