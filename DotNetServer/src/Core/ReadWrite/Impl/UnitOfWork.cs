using System;
using System.Data.SqlClient;
using Common.Service.Impl;

namespace Core.ReadWrite.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _begun;
        private bool _disposed;
        private bool _rolledBack;
        private SqlTransaction _transaction;

        public SqlConnection CurrentConnection { get; private set; }

        public void SetCommandTransaction(SqlCommand command)
        {
            command.Transaction = _transaction;
        }

        public void Begin()
        {
            CheckIsDisposed();

            CurrentConnection = new SqlConnection(ConfigProvider.GetDatabaseConfig().GetConnectionString());
            CurrentConnection.Open();

            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            _transaction = CurrentConnection.BeginTransaction();

            _begun = true;
        }

        public void Commit()
        {
            CheckIsDisposed();
            CheckHasBegun();

            if (_transaction.Connection != null && !_rolledBack)
            {
                _transaction.Commit();
            }

            if (_transaction != null)
            {
                _transaction.Dispose();
            }
        }

        public void RollBack()
        {
            CheckIsDisposed();
            CheckHasBegun();

            if (_transaction.Connection != null)
            {
                _transaction.Rollback();
                _rolledBack = true;
            }

            if (_transaction != null)
            {
                _transaction.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_begun || _disposed)
                return;

            if (disposing)
            {
                _transaction.Dispose();
                CurrentConnection.Dispose();
            }

            _disposed = true;
        }

        private void CheckHasBegun()
        {
            if (!_begun)
                throw new InvalidOperationException("Must call Begin() on the unit of work before committing");
        }

        private void CheckIsDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}