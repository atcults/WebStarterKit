using System;
using Common.Base;

namespace Core.Commands.AttachmentCommands
{
    public class UpdateAttachment : AuditedCommand
    {
        //TODO: Get ReferenceId
        public string Tags { get; set; }

        public override Guid? GetAggregateId()
        {
            return null;
        }

        public override ValidationResult Validate()
        {
            return new ValidationResult();
        }
    }
}