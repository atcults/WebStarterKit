using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Common.Service.Impl;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.ReadWrite.Impl
{
    public class Repository<T> : IRepository<T> where T : Entity, new()
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Core

        private static string GetEntityName()
        {
            var type = typeof (T);
            var attrs = type.GetCustomAttributes(typeof (EntityNameAttribute), true);
            var tableNameAttr = attrs[0] as EntityNameAttribute;
            if (tableNameAttr == null)
                throw new CustomAttributeFormatException("Missing EntityNameAttribute in " + type.Name);
            return tableNameAttr.Value;
        }

        private void ExecuteCore(string storeProcName, Action<SqlCommand> addCommandParams, Action<SqlCommand> commandResultAction)
        {
            try
            {
                var connection = _unitOfWork.CurrentConnection;
                var command = connection.CreateCommand();
                _unitOfWork.SetCommandTransaction(command);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storeProcName;

                if (addCommandParams != null)
                    addCommandParams(command);

                if (commandResultAction == null)
                    throw new InvalidOperationException("ExecuteCore called with invalid commandResult action.");

                commandResultAction(command);
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error while executing store proc: " + storeProcName, ex);
            }
        }

        private void Execute(string storeProcName, Action<SqlCommand> addCommandParams)
        {
            ExecuteCore(storeProcName, addCommandParams, command => command.ExecuteNonQuery());
        }

        public IList<T> Load(string storeProcName, Action<SqlCommand> addCommandParams)
        {
            var entity = new List<T>();

            ExecuteCore(storeProcName, addCommandParams, command =>
            {
                var connection = _unitOfWork.CurrentConnection;
                if (string.IsNullOrWhiteSpace(connection.ConnectionString))
                    connection.ConnectionString = ConfigProvider.GetDatabaseConfig().GetConnectionString();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var dataReader = command.ExecuteReader(CommandBehavior.Default);
                while (dataReader.Read())
                {
                    var modelObject = new T();
                    modelObject.From(dataReader);
                    entity.Add(modelObject);
                }
                dataReader.Close();
            });

            return entity;
        }

        #endregion

        public void ExecuteCommand(string commandSet, Action<SqlCommand> addCommandParams)
        {
            try
            {
                var connection = _unitOfWork.CurrentConnection;
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = commandSet;

                if (addCommandParams != null)
                    addCommandParams(command);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Error while executing store proc: " + commandSet, ex);
            }
        }

        public object ExecuteScaler(string storeProcName, Action<SqlCommand> addCommandParams)
        {
            object result = null;
            ExecuteCore(storeProcName, addCommandParams, command => { result = command.ExecuteScalar(); });
            return result;
        }

        public T GetById(Guid id)
        {
            var result = GetAllFor("Id", id);
            if (result.Count > 1) throw new Exception("Result set is not unique. Length is:" + result.Count);
            return result.Count == 0 ? default(T) : result[0];
        }

        public T GetByKey(string keyName, object keyVal)
        {
            var result = GetAllFor(keyName, keyVal);
            if (result.Count > 1) throw new Exception("Result set is not unique. Length is:" + result.Count);
            return result.Count == 0 ? default(T) : result[0];
        }

        public IList<T> GetAllFor(string keyName, object keyVal)
        {
            var result = Load("usp_CustomSelect", cmd =>
            {
                cmd.Parameters.AddWithValue("TableName", GetEntityName());
                cmd.Parameters.AddWithValue("PropertyName", keyName);
                cmd.Parameters.AddWithValue("PropertyVal", keyVal);
            });
            return result;
        }

        public T Add(T entity)
        {
            if (!entity.IsPersistent) throw new Exception("Could not insert data without Id");
            var spName = string.Format("usp_{0}Insert", GetEntityName());
            var result = Load(spName, entity.To);
            switch (result.Count)
            {
                case 0:
                    throw new Exception("Could not insert data");
                case 1:
                    break;
                default:
                    throw new Exception("Result set is not unique. Length is:" + result.Count);
            }
            return result[0];
        }

        public T Update(T entity)
        {
            if (!entity.IsPersistent) throw new Exception("Could not update data without Id");
            var spName = string.Format("usp_{0}Update", GetEntityName());
            var result = Load(spName, entity.To);
            switch (result.Count)
            {
                case 0:
                    throw new Exception("Update failed. No result found to update");
                case 1:
                    break;
                default:
                    throw new Exception("Result set is not unique. Length is:" + result.Count);
            }

            return result[0];
        }

        public void Delete(Guid id)
        {
            var spName = string.Format("usp_{0}Delete", GetEntityName());
            Execute(spName, cmd => { cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id; });
        }
    }
}