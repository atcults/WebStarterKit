using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.ReadWrite.Base
{
    public class AuditedEntity : Entity, IAuditedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageData { get; set; }
        public Guid? CreatedBy { get; set; }
        public int RevisionNumber { get; private set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);
            Name = dataReader.ReadNullSafeString("Name");
            Description = dataReader.ReadNullSafeString("Description");
            ImageData = dataReader.ReadNullSafeString("ImageData");
            RevisionNumber = dataReader.ReadInt("RevisionNumber");
            CreatedBy = dataReader.ReadNullSafeUid("CreatedBy");
            CreatedOn = dataReader.ReadNullSafeDateTime("CreatedOn");
            ModifiedBy = dataReader.ReadNullSafeUid("ModifiedBy");
            ModifiedOn = dataReader.ReadNullSafeDateTime("ModifiedOn");
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);

            object userId = DBNull.Value;

            if (CreatedBy.HasValue)
            {
                userId = CreatedBy.Value;
            }

            if (ModifiedBy.HasValue)
            {
                userId = ModifiedBy.Value;
            }

            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 256).Value = (object) Name ?? DBNull.Value;
            cmd.Parameters.Add("@Description", SqlDbType.VarChar, 2048).Value = (object) Description ?? DBNull.Value;
            cmd.Parameters.Add("@ImageData", SqlDbType.VarChar, -1).Value = (object) ImageData ?? DBNull.Value;
            cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = userId;
        }
    }
}