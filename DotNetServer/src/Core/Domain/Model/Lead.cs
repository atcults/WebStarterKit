using System;
using System.Data;
using System.Data.SqlClient;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
    [EntityName("Lead")]
    public class Lead : AuditedEntity
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);
            CompanyName = dataReader.ReadNullSafeString("CompanyName");
            Email = dataReader.ReadNullSafeString("Email");
            Phone = dataReader.ReadNullSafeString("Phone");
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);
            cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = (object) CompanyName ?? DBNull.Value;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;
            cmd.Parameters.Add("@Phone", SqlDbType.VarChar).Value = (object) Phone ?? DBNull.Value;
        }
    }
}