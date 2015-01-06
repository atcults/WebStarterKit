using System;
using System.Data;
using System.Data.SqlClient;
using Common.Enumerations;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
    [EntityName("AppUser")]
    public class AppUser : Entity
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public int FailedAttemptCount { get; set; }
        public Guid? ProfileId { get; set; }
        public UserStatus UserStatus { get; set; }

        public string PasswordRetrievalToken { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? PasswordRetrievalTokenExpirationDate { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }


        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);

            PasswordHash = dataReader.ReadNullSafeString("PasswordHash");
            PasswordSalt = dataReader.ReadNullSafeString("PasswordSalt");
            FailedAttemptCount = dataReader.ReadNullSafeInt("FailedAttemptCount").GetValueOrDefault();
            ProfileId = dataReader.ReadNullSafeUid("ProfileId");
            UserStatus = UserStatus.FromValue(dataReader.ReadNullSafeString("UserStatusValue"));
            PasswordRetrievalToken = dataReader.ReadNullSafeString("PasswordRetrievalToken");
            LastLoginTime = dataReader.ReadNullSafeDateTime("LastLoginTime");
            PasswordRetrievalTokenExpirationDate = dataReader.ReadNullSafeDateTime("PasswordRetrievalTokenExpirationDate");
            LastPasswordChangedDate = dataReader.ReadNullSafeDateTime("LastPasswordChangedDate");
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar).Value = (object) PasswordHash ?? DBNull.Value;
            cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarChar).Value = (object) PasswordSalt ?? DBNull.Value;
            cmd.Parameters.Add("@FailedAttemptCount", SqlDbType.Int).Value = FailedAttemptCount;
            cmd.Parameters.Add("@ProfileId", SqlDbType.UniqueIdentifier).Value = ProfileId.HasValue ? (object) ProfileId.Value : DBNull.Value;
            cmd.Parameters.Add("@UserStatusValue", SqlDbType.Char, 2).Value = BaseEnumeration.GetDbNullSafe(UserStatus) ?? DBNull.Value;
            cmd.Parameters.Add("@PasswordRetrievalToken", SqlDbType.VarChar).Value = (object) PasswordRetrievalToken ?? DBNull.Value;
            cmd.Parameters.Add("@LastLoginTime", SqlDbType.DateTime).Value = (object) LastLoginTime ?? DBNull.Value;
            cmd.Parameters.Add("@PasswordRetrievalTokenExpirationDate", SqlDbType.DateTime).Value = (object) PasswordRetrievalTokenExpirationDate ?? DBNull.Value;
            cmd.Parameters.Add("@LastPasswordChangedDate", SqlDbType.DateTime).Value = (object) LastPasswordChangedDate ?? DBNull.Value;
        }
    }
}