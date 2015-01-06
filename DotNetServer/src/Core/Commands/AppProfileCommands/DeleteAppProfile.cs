using System;
using Common.Base;

namespace Core.Commands.AppProfileCommands
{
    public class DeleteAppProfile : Command
    {
        public DeleteAppProfile(Guid id)
        {
            Id = id;
        }

        public override Guid? GetAggregateId()
        {
            return Id;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();
            return validationResult;
        }
    }
}
