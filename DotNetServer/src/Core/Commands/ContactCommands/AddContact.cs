using System;
using Common.Base;
using Common.Enumerations;
using Common.Helpers;

namespace Core.Commands.ContactCommands
{
    public class AddContact : AuditedCommand
    {
        public Gender Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public ContactType ContactType { get; set; }
        public Language PrimaryLanguage { get; set; }
        public Language SecondaryLanguage { get; set; }
        public override Guid? GetAggregateId()
        {
            return Id;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if (!string.IsNullOrWhiteSpace(Name) && Name.Length > 256)
                validationResult.AddError("Name", " entered exceeds the maximum length 256");

            if (string.IsNullOrWhiteSpace(Name))
                validationResult.AddError("Name", "should not be empty");

            if (!Formatter.MobileNumber(Mobile))
                validationResult.AddError("Mobile", " is not valid number.");

            if (!Formatter.EmailId(Email))
                validationResult.AddError("Email", " is not valid.");

            if (!string.IsNullOrWhiteSpace(Description) && Description.Length > 2048)
                validationResult.AddError("Description", " entered exceeds the maximum length 2048");

            return validationResult;
        }
    }
}