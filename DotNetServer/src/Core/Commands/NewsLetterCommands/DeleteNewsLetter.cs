using System;
using Common.Base;

namespace Core.Commands.NewsLetterCommands
{
    public class DeleteNewsLetter : ICommand
    {
        public DeleteNewsLetter(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }

        public Guid? GetAggregateId()
        {
            return Id;
        }

        public ValidationResult Validate()
        {
            var validationResult = new ValidationResult();
            return validationResult;
        }
    }
}