using System;
using Common.Base;

namespace Core.Commands.TemplateCommands
{
    public class AddTemplate : Command
    {
        public string Name { get; set; }
        public string MailBody { get; set; }
        public string SmsBody { get; set; }

        public override Guid? GetAggregateId()
        {
            return Id;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(MailBody) || string.IsNullOrWhiteSpace(SmsBody))
                validationResult.AddError("Template Name and Template Value", "should not be empty.");

            return validationResult;
        }
    }
}