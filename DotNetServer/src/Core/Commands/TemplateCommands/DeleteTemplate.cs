using System;
using Common.Base;

namespace Core.Commands.TemplateCommands
{
    public class DeleteTemplate : Command
    {
        public DeleteTemplate(Guid id)
        {
            Id = id;
        }

        public override Guid? GetAggregateId()
        {
            return null;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();
            return validationResult;
        }
    }
}