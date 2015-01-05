using System;
using Common.Base;

namespace Core.Commands.AttachmentCommands
{
    public class DeleteAttachment : Command
    {
        public DeleteAttachment(Guid id)
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