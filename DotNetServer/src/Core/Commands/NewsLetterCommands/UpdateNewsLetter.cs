using System;
using Common.Base;
using Common.Extensions;
using Common.Helpers;

namespace Core.Commands.NewsLetterCommands
{
    public class UpdateNewsLetter : ICommand
    {
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public Guid Id { get; set; }


        public Guid? GetAggregateId()
        {
            return Id;
        }

        public ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if (!string.IsNullOrWhiteSpace(Email) && Email.Length > 128)
                validationResult.AddError("Email", " entered exceeds the maximum length 128");

            if (Email.IsEmpty())
            {
                validationResult.AddError("Email", "should not be empty");
            }
            else
            {
                if (Email.IsNotEmpty() && Formatter.EmailId(Email) != true)
                    validationResult.AddError("Email", "should be correct formate. (xxxxx@xxx.xx)");
            }

            return validationResult;
        }
    }
}