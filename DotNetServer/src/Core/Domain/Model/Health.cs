using System;
using System.Data;
using System.Data.SqlClient;
using Common.Enumerations;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
    [EntityName("Health")]
    public class Health : Entity
    {
        public HealthType HealthType { get; set; }
        public string Value { get; set; }
        public DateTime? RecordTime { get; set; }

        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);
            HealthType = HealthType.FromValue(dataReader.ReadNullSafeString("HealthTypeValue"));
            Value = dataReader.ReadNullSafeString("Value");
            RecordTime = dataReader.ReadNullSafeDateTime("RecordTime");
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);
            cmd.Parameters.Add("@HealthTypeValue", SqlDbType.Char, 2).Value = BaseEnumeration.GetDbNullSafe(HealthType);
            cmd.Parameters.Add("@Value", SqlDbType.NVarChar).Value = Value;
            cmd.Parameters.Add("@RecordTime", SqlDbType.DateTime).Value = (object) RecordTime ?? DBNull.Value;
        }
    }
}