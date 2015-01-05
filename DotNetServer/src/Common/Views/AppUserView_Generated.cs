using System;
using PetaPoco;
using Sanelib.DataView;

namespace Common.Views
{
	[TableName("AppUserView")]
	public class AppUserView : AuditedView
	{

		public stringPasswordHash { get; set; }
		public stringPasswordSalt { get; set; }
		public int? FailedAttemptCount { get; set; }

		[JsonIgnore]
		public Role Role { get; set; }
		public string RoleValue { get { return Role == null ? "" : Role.Value; } set { Role = Role.FromValue(value); } }
		public string RoleName { get { return Role == null ? "" : Role.DisplayName; } set { Role = Role.FromDispaly(value); } }


		[JsonIgnore]
		public UserStatus UserStatus { get; set; }
		public string UserStatusValue { get { return UserStatus == null ? "" : UserStatus.Value; } set { UserStatus = UserStatus.FromValue(value); } }
		public string UserStatusName { get { return UserStatus == null ? "" : UserStatus.DisplayName; } set { UserStatus = UserStatus.FromDispaly(value); } }

		public stringPasswordRetrievalToken { get; set; }
		public DateTime? LastLoginTime { get; set; }
		public DateTime? PasswordRetrievalTokenExpirationDate { get; set; }
		public DateTime? LastPasswordChangedDate { get; set; }

	}
}
