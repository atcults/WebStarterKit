using System;
using Common.Base;
using Common.Enumerations;
using Common.Extensions;

namespace Core.Commands.NoteCommands
{
    public class AddNote : AuditedCommand
    {
        public EntityType EntityType { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }

        public override Guid? GetAggregateId()
        {
            return null;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if (EntityType == null)
                validationResult.AddError("Entity Type", "is missing. Please contact administartor.");

            if (ReferenceId.IsEmpty())
                validationResult.AddError("Reference Id", "is missing. Please contact administartor.");

            if (string.IsNullOrWhiteSpace(Description))
                validationResult.AddError("Description", " should not be empty");

            if (!string.IsNullOrWhiteSpace(Description) && Description.Length > 2048)
                validationResult.AddError("Description", " entered exceeds the maximum length 2048");

            return validationResult;
        }
    }
}