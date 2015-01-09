using System;
using System.Data;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
	[EntityName("TokenStore")]
	public class TokenStore : Entity
	{
        public string ClientName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
	    public string AccessTokenHash { get; set; }
        public string AccessTicket { get; set; }
        public DateTime? AccessTokenIssuedUtc { get; set; }
        public DateTime? AccessTokenExpiresUtc { get; set; }
        public string RefreshTokenHash { get; set; }
        public string RefreshTicket { get; set; }
        public DateTime? RefreshTokenIssuedUtc { get; set; }
        public DateTime? RefreshTokenExpiresUtc { get; set; }
        public int? TimesTokenGiven { get; set; }
        
		public override void From(IDataReader dataReader)
		{
			base.From(dataReader);
			ClientName = dataReader.ReadNullSafeString("ClientName");
		    UserId = dataReader.ReadUid("UserId");
		    UserName = dataReader.ReadNullSafeString("UserName");
		    AccessTokenHash = dataReader.ReadNullSafeString("AccessTokenHash");
		    AccessTicket = dataReader.ReadNullSafeString("AccessTicket");
		    AccessTokenIssuedUtc = dataReader.ReadNullSafeDateTime("AccessTokenIssuedUtc");
		    AccessTokenExpiresUtc = dataReader.ReadNullSafeDateTime("AccessTokenExpiresUtc");
		    RefreshTokenHash = dataReader.ReadNullSafeString("RefreshTokenHash");
		    RefreshTicket = dataReader.ReadNullSafeString("RefreshTicket");
		    RefreshTokenIssuedUtc = dataReader.ReadNullSafeDateTime("RefreshTokenIssuedUtc");
		    RefreshTokenExpiresUtc = dataReader.ReadNullSafeDateTime("RefreshTokenExpiresUtc");
		    TimesTokenGiven = dataReader.ReadNullSafeInt("TimesTokenGiven");
		}

		public override void To(System.Data.SqlClient.SqlCommand cmd)
		{
			base.To(cmd);
		    cmd.Parameters.Add("@ClientName", SqlDbType.VarChar, 256).Value = (object) ClientName ?? DBNull.Value;
		    cmd.Parameters.Add("@UserId", SqlDbType.UniqueIdentifier).Value = UserId;
		    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 256).Value = (object) UserName ?? DBNull.Value;
		    cmd.Parameters.Add("@AccessTokenHash", SqlDbType.VarChar, 256).Value = (object) AccessTokenHash ?? DBNull.Value;
		    cmd.Parameters.Add("@AccessTicket", SqlDbType.VarChar, 1024).Value = (object) AccessTicket ?? DBNull.Value;
		    cmd.Parameters.Add("@AccessTokenIssuedUtc", SqlDbType.DateTime).Value = (object) AccessTokenIssuedUtc ?? DBNull.Value;
		    cmd.Parameters.Add("@AccessTokenExpiresUtc", SqlDbType.DateTime).Value = (object) AccessTokenExpiresUtc ?? DBNull.Value;
		    cmd.Parameters.Add("@RefreshTokenHash", SqlDbType.VarChar, 256).Value = (object) RefreshTokenHash ?? DBNull.Value;
		    cmd.Parameters.Add("@RefreshTicket", SqlDbType.VarChar, 1024).Value = (object) RefreshTicket ?? DBNull.Value;
		    cmd.Parameters.Add("@RefreshTokenIssuedUtc", SqlDbType.DateTime).Value = (object) RefreshTokenIssuedUtc ?? DBNull.Value;
		    cmd.Parameters.Add("@RefreshTokenExpiresUtc", SqlDbType.DateTime).Value = (object) RefreshTokenExpiresUtc ?? DBNull.Value;
		    cmd.Parameters.Add("@TimesTokenGiven", SqlDbType.Int).Value = (object) TimesTokenGiven ?? DBNull.Value;
		}
	}
}
