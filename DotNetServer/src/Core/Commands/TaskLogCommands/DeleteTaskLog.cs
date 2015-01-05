using System;
using Common.Base;

namespace Core.Commands.TaskLogCommands
{
    public class DeleteTaskLog : Command
    {
        public DeleteTaskLog(Guid id)
        {
            Id = id;
        }

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