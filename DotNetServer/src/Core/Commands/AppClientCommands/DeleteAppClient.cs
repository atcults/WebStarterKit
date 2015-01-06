using System;
using Common.Base;

namespace Core.Commands.AppClientCommands
{
    public class DeleteAppClient : Command
    {
        public DeleteAppClient(Guid id)
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
