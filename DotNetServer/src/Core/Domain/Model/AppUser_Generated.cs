using System;
using System.Data;
using Sanelib.DataOperation;
using Sanelib.DataOperation.Attribute;
using Sanelib.DataOperation.Base;

namespace Core.Domain.Model
{
	[EntityName("AppUser")]
	public class AppUser : AuditedEntity
	{
		public stringPasswordHash { get; set; }
		public stringPasswordSalt { get; set; }
		public int? FailedAttemptCount { get; set; }
		public Role Role { get; set; }
		public UserStatus UserStatus { get; set; }
		public stringPasswordRetrievalToken { get; set; }
		public DateTime? LastLoginTime { get; set; }
		public DateTime? PasswordRetrievalTokenExpirationDate { get; set; }
		public DateTime? LastPasswordChangedDate { get; set; }

		public override void From(IDataReader dataReader)
		{
			base.From(dataReader);
			PasswordHash = dataReader.ReadNullSafeString("PasswordHash");
			PasswordSalt = dataReader.ReadNullSafeString("PasswordSalt");
			FailedAttemptCount = dataReader.ReadInt("FailedAttemptCount");
			Role = Role.FromValue(dataReader.ReadNullSafeString("RoleValue"));
			UserStatus = UserStatus.FromValue(dataReader.ReadNullSafeString("UserStatusValue"));
			PasswordRetrievalToken = dataReader.ReadNullSafeString("PasswordRetrievalToken");
			LastLoginTime = dataReader.ReadDateTime("LastLoginTime");
			PasswordRetrievalTokenExpirationDate = dataReader.ReadDateTime("PasswordRetrievalTokenExpirationDate");
			LastPasswordChangedDate = dataReader.ReadDateTime("LastPasswordChangedDate");
		}

		public override void To(System.Data.SqlClient.SqlCommand cmd)
		{
			base.To(cmd);
			cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 256).Value = (object)PasswordHash ?? DBNull.Value;
			cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarChar, 256).Value = (object)PasswordSalt ?? DBNull.Value;
			cmd.Parameters.Add("@FailedAttemptCount", SqlDbType.Int).Value = (object)FailedAttemptCount ?? DBNull.Value;
			cmd.Parameters.Add("@RoleValue", SqlDbType.Char, 2).Value = (object)BaseEnumeration.GetDbNullSafe(Role) ?? DBNull.Value;
			cmd.Parameters.Add("@UserStatusValue", SqlDbType.Char, 2).Value = (object)BaseEnumeration.GetDbNullSafe(UserStatus) ?? DBNull.Value;
			cmd.Parameters.Add("@PasswordRetrievalToken", SqlDbType.VarChar, 256).Value = (object)PasswordRetrievalToken ?? DBNull.Value;
			cmd.Parameters.Add("@LastLoginTime", SqlDbType.DateTime).Value = (object)LastLoginTime ?? DBNull.Value;
			cmd.Parameters.Add("@PasswordRetrievalTokenExpirationDate", SqlDbType.DateTime).Value = (object)PasswordRetrievalTokenExpirationDate ?? DBNull.Value;
			cmd.Parameters.Add("@LastPasswordChangedDate", SqlDbType.DateTime).Value = (object)LastPasswordChangedDate ?? DBNull.Value;
		}
	}
}
