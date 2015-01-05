using System;
using System.Data.SqlClient;

namespace Core.ReadWrite
{
    public interface IUnitOfWork : IDisposable
    {
        SqlConnection CurrentConnection { get; }
        void SetCommandTransaction(SqlCommand command);
        void Begin();
        void Commit();
        void RollBack();
    }
}