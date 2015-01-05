using System;
using Common.Base;
using Common.Extensions;
using Common.Helpers;

namespace Core.Commands.AppUserCommands
{
    public class ChangePassword : Command
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public override Guid? GetAggregateId()
        {
            return null;
        }

        public override ValidationResult Validate()
        {
            var validator = new ValidationResult();

            if (OldPassword.IsEmpty()) validator.AddError("Old Password", "is Required");
            if (NewPassword.IsEmpty()) validator.AddError("New Password", "is Required");
            if (ConfirmPassword.IsEmpty()) validator.AddError("Confirm Password", "is Required");
            if (NewPassword != ConfirmPassword)
                validator.AddError("New Password and Confirm New Password", "are not the same");

            if (!validator.IsValid) return validator;

            if (!string.IsNullOrWhiteSpace(NewPassword) && NewPassword.Length < 8)
            {
                validator.AddError("New Password", "Must be 8 characters long");
                return validator;
            }
            if (!Formatter.HasAtLeast1Lowercase(NewPassword))
            {
                validator.AddError("New Password", "Must contains at least one LowerCase letter");
                return validator;
            }
            if (!Formatter.HasAtLeast1Number(NewPassword))
            {
                validator.AddError("New Password", "Must contains at least one Number");
                return validator;
            }
            if (!Formatter.HasAtLeast1SpecialChar(NewPassword))
            {
                validator.AddError("New Password", "Must contains at least one Special Character from  : _ # $ % ");
                return validator;
            }

            if (Formatter.HasAtLeast1Uppercase(NewPassword)) return validator;

            validator.AddError("New Password", "Must contains at least one UpperCase letter");
            return validator;
        }
    }
}