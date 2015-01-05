using System;
using Common.Base;

namespace Core.Commands.ContactCommands
{
    public class DeleteAssignedContact : Command
    {
        public DeleteAssignedContact(Guid id)
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