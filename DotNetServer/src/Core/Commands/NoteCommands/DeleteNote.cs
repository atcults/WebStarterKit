using System;
using Common.Base;

namespace Core.Commands.NoteCommands
{
    public class DeleteNote : Command
    {
        public DeleteNote(Guid id)
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