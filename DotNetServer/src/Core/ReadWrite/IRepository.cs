using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Core.ReadWrite.Base;

namespace Core.ReadWrite
{
    public interface IRepository<T> where T : Entity
    {
        T GetById(Guid id);
        T GetByKey(string keyName, object keyVal);
        T Add(T entity);
        T Update(T entity);
        void Delete(Guid id);
        IList<T> GetAllFor(string keyName, object keyVal);
        void ExecuteCommand(string commandSet, Action<SqlCommand> addCommandParams);
        object ExecuteScaler(string storeProcName, Action<SqlCommand> addCommandParams);
    }
}