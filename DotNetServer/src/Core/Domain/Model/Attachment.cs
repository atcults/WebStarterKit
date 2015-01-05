using System;
using System.Data;
using System.Data.SqlClient;
using Common.Enumerations;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
    [EntityName("Attachment")]
    public class Attachment : AuditedEntity
    {
        public string Tags { get; set; }
        public string FileType { get; set; }
        public decimal? FileSize { get; set; }
        public string FileHashCode { get; set; }
        public byte[] FileData { get; set; }
        public EntityType EntityType { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }

        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);
            Tags = dataReader.ReadNullSafeString("Tags");
            FileType = dataReader.ReadNullSafeString("FileType");
            FileSize = dataReader.ReadNullSafeDecimal("FileSize");
            FileHashCode = dataReader.ReadNullSafeString("FileHashCode");
            FileData = dataReader.ReadBytes("FileData");
            EntityType = EntityType.FromValue(dataReader.ReadNullSafeString("EntityTypeValue"));
            ReferenceId = dataReader.ReadUid("ReferenceId");
            ReferenceName = dataReader.ReadNullSafeString("ReferenceName");
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);
            cmd.Parameters.Add("@Tags", SqlDbType.NVarChar).Value = (object) Tags ?? DBNull.Value;
            cmd.Parameters.Add("@FileType", SqlDbType.NVarChar).Value = (object) FileType ?? DBNull.Value;
            cmd.Parameters.Add("@FileSize", SqlDbType.Decimal).Value = (object) FileSize ?? DBNull.Value;
            cmd.Parameters.Add("@FileHashCode", SqlDbType.NVarChar).Value = (object) FileHashCode ?? DBNull.Value;
            cmd.Parameters.Add("@FileData", SqlDbType.Image).Value = (object) FileData ?? DBNull.Value;
            cmd.Parameters.Add("@EntityTypeValue", SqlDbType.Char, 2).Value = BaseEnumeration.GetDbNullSafe(EntityType);
            cmd.Parameters.Add("@ReferenceId", SqlDbType.UniqueIdentifier).Value = ReferenceId;
            cmd.Parameters.Add("@ReferenceName", SqlDbType.VarChar).Value = (object) ReferenceName ?? DBNull.Value;
        }
    }
}