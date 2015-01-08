using System;
using System.Data;
using Common.Enumerations;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
	[EntityName("AppClient")]
	public class AppClient : Entity
	{
		public string Name { get; set; }
        public string Secret { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int AccessTokenLifeTime { get; set; }
		public int RefreshTokenLifeTime { get; set; }
		public string AllowedOrigin { get; set; }
		public bool IsActive { get; set; }

		public override void From(IDataReader dataReader)
		{
			base.From(dataReader);
            Name = dataReader.ReadNullSafeString("Name");
            Secret = dataReader.ReadNullSafeString("Secret");
            ApplicationType = ApplicationType.FromValue(dataReader.ReadNullSafeString("ApplicationTypeValue"));
            AccessTokenLifeTime = dataReader.ReadInt("AccessTokenLifeTime");
            RefreshTokenLifeTime = dataReader.ReadInt("RefreshTokenLifeTime");
			AllowedOrigin = dataReader.ReadNullSafeString("AllowedOrigin");
			IsActive = dataReader.ReadNullSafeBool("IsActive");
		}

		public override void To(System.Data.SqlClient.SqlCommand cmd)
		{
			base.To(cmd);
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 256).Value = (object)Name ?? DBNull.Value;
            cmd.Parameters.Add("@Secret", SqlDbType.VarChar, 256).Value = (object)Secret ?? DBNull.Value;
			cmd.Parameters.Add("@ApplicationTypeValue", SqlDbType.Char, 2).Value = BaseEnumeration.GetDbNullSafe(ApplicationType) ?? DBNull.Value;
            cmd.Parameters.Add("@AccessTokenLifeTime", SqlDbType.Int).Value = AccessTokenLifeTime;
            cmd.Parameters.Add("@RefreshTokenLifeTime", SqlDbType.Int).Value = RefreshTokenLifeTime;
			cmd.Parameters.Add("@AllowedOrigin", SqlDbType.VarChar, 256).Value = (object)AllowedOrigin ?? DBNull.Value;
		    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = IsActive;
		}
	}
}
