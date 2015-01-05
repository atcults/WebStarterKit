using System;
using Common.Commands;
using Sanelib.Dto;

namespace Common.Commands.AppUserCommands
{
	public class AddAppUser: ICommand
	{
		public Guid Id { get; set; }
		public stringPasswordHash { get; set; }
		public stringPasswordSalt { get; set; }
		public int? FailedAttemptCount { get; set; }
		public Role Role { get; set; }
		public UserStatus UserStatus { get; set; }
		public stringPasswordRetrievalToken { get; set; }
		public DateTime? LastLoginTime { get; set; }
		public DateTime? PasswordRetrievalTokenExpirationDate { get; set; }
		public DateTime? LastPasswordChangedDate { get; set; }

		public Guid GetAggregateId()
		{
			return Id;
		}

		public ErrorResult Validate()
		{
			var validationResult = new ErrorResult();
			return validationResult;
		}
	}

	public class UpdateAppUser: AddAppUser
	{

	}

	public class DeleteAppUser: ICommand
	{
		public DeleteAppUser(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; set; }

		public Guid GetAggregateId()
		{
			return Id;
		}

		public ErrorResult Validate()
		{
			var validationResult = new ErrorResult();
			return validationResult;
		}
	}
}
