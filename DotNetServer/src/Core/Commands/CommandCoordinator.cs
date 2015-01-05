using System;
using Common.Base;

namespace Core.Commands
{
    public interface ICommand
    {
        Guid Id { get; set; }
        Guid? GetAggregateId();
        ValidationResult Validate();
    }
}