using System;
using Common.Base;

namespace Core.Commands.AppProfileCommands
{
    public class AddAppProfile : Command
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public override Guid? GetAggregateId()
        {
            return Id;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();
            return validationResult;
        }
    }

}
