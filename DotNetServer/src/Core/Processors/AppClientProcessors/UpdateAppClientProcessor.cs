using System;
using Core.Commands;
using Core.Commands.AppClientCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AppClientProcessors
{
    public class UpdateAppClientProcessor : ICommandProcessor<UpdateAppClient>
    {
        private readonly IRepository<AppClient> _appClientRepository;

        public UpdateAppClientProcessor(IRepository<AppClient> appClientRepository)
        {
            _appClientRepository = appClientRepository;
        }

        public void Process(UpdateAppClient command, Guid userId, out IWebApiResponse response)
        {

            var appClient = _appClientRepository.GetById(command.Id);

            appClient.Name = command.Name;
            appClient.AllowedOrigin = command.AllowedOrigin;
            appClient.ApplicationType = command.ApplicationType;
            appClient.RefreshTokenLifeTime = command.RefreshTokenLifeTime;
            appClient.Secret = command.Secret;
            appClient.IsActive = command.IsActive;

            _appClientRepository.Update(appClient);

            response = new WebApiResponseBase();
        }

    }
}