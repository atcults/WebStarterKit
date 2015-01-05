using System;
using Common.Base;
using Common.Enumerations;

namespace Core.Commands.TaskLogCommands
{
    public class AddTaskLog : AuditedCommand
    {
        public TaskStatus TaskStatus { get; set; }

        public override Guid? GetAggregateId()
        {
            return Id;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(Name))
                validationResult.AddError("Task Name", "should not be empty.");

            if (TaskStatus == null)
                validationResult.AddError("Task Status", "should not be empty.");

            return validationResult;
        }
    }
}