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
		public string Name { get; set; }
        public string TokenHash { get; set; }
		public Guid? ClientId { get; set; }
		public string TicketHash { get; set; }
		public DateTime? IssuedUtc { get; set; }
		public DateTime? ExpiresUtc { get; set; }

		public override void From(IDataReader dataReader)
		{
			base.From(dataReader);
			Id = dataReader.ReadUid("Id");
			Name = dataReader.ReadNullSafeString("Name");
		    TokenHash = dataReader.ReadNullSafeString("TokenHash");
			ClientId = dataReader.ReadUid("ClientId");
			TicketHash = dataReader.ReadNullSafeString("TicketHash");
			IssuedUtc = dataReader.ReadNullSafeDateTime("IssuedUtc");
            ExpiresUtc = dataReader.ReadNullSafeDateTime("ExpiresUtc");
		}

		public override void To(System.Data.SqlClient.SqlCommand cmd)
		{
			base.To(cmd);
			cmd.Parameters.Add("@Name", SqlDbType.VarChar, 256).Value = (object)Name ?? DBNull.Value;
			cmd.Parameters.Add("@TokenHash", SqlDbType.VarChar, 256).Value = (object)TokenHash ?? DBNull.Value;
			cmd.Parameters.Add("@ClientId", SqlDbType.UniqueIdentifier).Value = (object)ClientId ?? DBNull.Value;
			cmd.Parameters.Add("@TicketHash", SqlDbType.VarChar, 256).Value = (object)TicketHash ?? DBNull.Value;
			cmd.Parameters.Add("@IssuedUtc", SqlDbType.DateTime).Value = (object)IssuedUtc ?? DBNull.Value;
			cmd.Parameters.Add("@ExpiresUtc", SqlDbType.DateTime).Value = (object)ExpiresUtc ?? DBNull.Value;
		}
	}
}
