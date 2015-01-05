using System;
using Common.Base;

namespace Core.Commands.LeadCommands
{
    public class DeleteLead : Command
    {
        public DeleteLead(Guid id)
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