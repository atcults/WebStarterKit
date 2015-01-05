using System;
using Common.Base;

namespace Core.Commands.ContactCommands
{
    public class DeleteContact : Command
    {
        public DeleteContact(Guid id)
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