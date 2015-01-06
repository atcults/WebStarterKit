using System;
using Core.Commands;
using Core.Commands.AppClientCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AppClientProcessors
{
    public class DeleteAppClientProcessor : ICommandProcessor<DeleteAppClient>
    {
        private readonly IRepository<AppClient> _appClientRepository;

        public DeleteAppClientProcessor(IRepository<AppClient> appClientRepository)
        {
            _appClientRepository = appClientRepository;
        }

        public void Process(DeleteAppClient command, Guid userId, out IWebApiResponse response)
        {
            response = new WebApiResponseBase();
            var appClient = _appClientRepository.GetById(command.Id);
            _appClientRepository.Delete(appClient.Id);
        }
    }
}