using System;
using Dto.ApiResponses;

namespace Core.Commands
{
    //NOTE: Do not declare as contravarient. It leads to plug all derived declarations.
    public interface ICommandProcessor<TCommand> where TCommand : ICommand
    {
        void Process(TCommand command, Guid userId, out IWebApiResponse response);
    }
}