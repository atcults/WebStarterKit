using System;

namespace Core.ReadWrite.Base
{
    public interface IEntity : IShouldReadDataReader, IShouldPassSqlParameter
    {
        Guid Id { get; set; }
    }
}