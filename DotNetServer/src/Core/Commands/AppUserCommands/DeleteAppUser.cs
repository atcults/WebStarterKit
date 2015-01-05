using System;
using Common.Base;

namespace Core.Commands.AppUserCommands
{
    public class DeleteAppUser : Command
    {
        public DeleteAppUser(Guid id)
        {
            Id = id;
        }

        public override Guid? GetAggregateId()
        {
            return null;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();
            return validationResult;
        }
    }
}