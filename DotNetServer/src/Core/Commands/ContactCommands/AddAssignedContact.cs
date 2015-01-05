using System;
using Common.Base;
using Common.Enumerations;
using Common.Extensions;

namespace Core.Commands.ContactCommands
{
    public class AddAssignedContact : AuditedCommand
    {
        public Guid ContactId { get; set; }
        public EntityType EntityType { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }

        public override Guid? GetAggregateId()
        {
            return ContactId;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if (ReferenceId.IsEmpty())
                validationResult.AddError("Reference Id", "is missing. Please contact administartor.");

            if (ContactId.IsEmpty())
                validationResult.AddError("Contact Id", "is missing. Please contact administartor.");

            return validationResult;
        }
    }
}