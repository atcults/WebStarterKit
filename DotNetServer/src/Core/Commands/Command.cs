using System;
using Common.Base;

namespace Core.Commands
{
    public abstract class Command : ICommand
    {
        public Guid Id { get; set; }

        public abstract Guid? GetAggregateId();

        public abstract ValidationResult Validate();
    }
}