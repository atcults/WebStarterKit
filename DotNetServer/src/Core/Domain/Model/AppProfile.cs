using System;
using System.Data;
using System.Data.SqlClient;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
	[EntityName("AppProfile")]
	public class AppProfile : Entity
	{
		public string Name { get; set; }
		public bool IsActive { get; set; }

		public override void From(IDataReader dataReader)
		{
			base.From(dataReader);
			Id = dataReader.ReadUid("Id");
			Name = dataReader.ReadNullSafeString("Name");
			IsActive = dataReader.ReadNullSafeBool("IsActive");
		}

		public override void To(SqlCommand cmd)
		{
			base.To(cmd);
			cmd.Parameters.Add("@Name", SqlDbType.VarChar, 256).Value = (object)Name ?? DBNull.Value;
			cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
		}
	}
}
