using System;
using Core.Commands;
using Core.Commands.AppClientCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AppClientProcessors
{
    public class AddAppClientProcessor : ICommandProcessor<AddAppClient>
    {
        private readonly IRepository<AppClient> _appClientRepository;

        public AddAppClientProcessor(IRepository<AppClient> appClientRepository)
        {
            _appClientRepository = appClientRepository;
        }

        public void Process(AddAppClient command, Guid userId, out IWebApiResponse response)
        {
            var appClient = new AppClient
            {
                Id = command.Id,
                Name = command.Name,
                AllowedOrigin = command.AllowedOrigin,
                ApplicationType = command.ApplicationType,
                RefreshTokenLifeTime = command.RefreshTokenLifeTime,
                Secret = command.Secret,
                IsActive = command.IsActive
            };
            _appClientRepository.Add(appClient);

            response = new WebApiResponseBase();
        }
    }
}