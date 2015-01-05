using System;
using Common.Base;
using Common.Enumerations;
using Common.Helpers;

namespace Core.Commands.AppUserCommands
{
    public class AddAppUser : AuditedCommand
    {
        public string Mobile { get; set; }
     
        public string Email { get; set; }
        
        public Role Role { get; set; }
        
        public UserStatus UserStatus { get; set; }

        public override Guid? GetAggregateId()
        {
            return Id;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if(!Formatter.MobileNumber(Mobile))
                validationResult.AddError("Mobile", " is not valid number.");

            if (!Formatter.EmailId(Email))
                validationResult.AddError("Email", " is not valid.");
            
            if (Role == null)
                validationResult.AddError("Role", " should be selected.");

            if (UserStatus == null)
                validationResult.AddError("User Status", " should be selected.");

            return validationResult;
        }
    }
}