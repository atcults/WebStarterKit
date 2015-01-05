using System;
using Common.Base;
using Common.Extensions;
using Common.Helpers;

namespace Core.Commands.LeadCommands
{
    public class AddLead : AuditedCommand
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override Guid? GetAggregateId()
        {
            return null;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if (!string.IsNullOrWhiteSpace(Name) && Name.Length > 128)
                validationResult.AddError("Name", " entered exceeds the maximum length 128");

            if (string.IsNullOrWhiteSpace(Name))
                validationResult.AddError("Name", "should not empty.");

            if (!string.IsNullOrWhiteSpace(CompanyName) && CompanyName.Length > 128)
                validationResult.AddError("Company Name", " entered exceeds the maximum length 128");

            if (!string.IsNullOrWhiteSpace(Email) && Email.Length > 128)
                validationResult.AddError("Email", " entered exceeds the maximum length 128");

            if (Email.IsEmpty())
                validationResult.AddError("Email", "should not be empty");
            else
            {
                if (Email.IsNotEmpty() && Formatter.EmailId(Email) != true)
                    validationResult.AddError("Email", "should be correct formate. (xxxxx@xxx.xx)");
            }

            return validationResult;
        }
    }
}