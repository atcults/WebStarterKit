using System;
using System.Data;
using System.Data.SqlClient;
using Common.Enumerations;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
    [EntityName("AssignedContact")]
    public class AssignedContact : AuditedEntity
    {
        public Guid ContactId { get; set; }
        public EntityType EntityType { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }

        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);
            ContactId = dataReader.ReadUid("ContactId");
            EntityType = EntityType.FromValue(dataReader.ReadNullSafeString("EntityTypeValue"));
            ReferenceId = dataReader.ReadUid("ReferenceId");
            ReferenceName = dataReader.ReadNullSafeString("ReferenceName");
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);
            cmd.Parameters.Add("@ContactId", SqlDbType.UniqueIdentifier).Value = ContactId;
            cmd.Parameters.Add("@EntityTypeValue", SqlDbType.Char, 2).Value = BaseEnumeration.GetDbNullSafe(EntityType);
            cmd.Parameters.Add("@ReferenceId", SqlDbType.UniqueIdentifier).Value = ReferenceId;
            cmd.Parameters.Add("@ReferenceName", SqlDbType.VarChar).Value = (object) ReferenceName ?? DBNull.Value;
        }
    }
}