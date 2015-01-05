using System;
using System.Data;
using System.Data.SqlClient;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
    [EntityName("NewsLetter")]
    public class NewsLetter : Entity
    {
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime? InsertedDate { get; set; }

        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);
            Email = dataReader.ReadNullSafeString("Email");
            IsActive = dataReader.ReadNullSafeBool("IsActive");
            InsertedDate = dataReader.ReadNullSafeDateTime("InsertedDate");
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 128).Value = (object) Email ?? DBNull.Value;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = (object) IsActive ?? DBNull.Value;
            cmd.Parameters.Add("@InsertedDate", SqlDbType.DateTime).Value = (object) InsertedDate ?? DBNull.Value;
        }
    }
}