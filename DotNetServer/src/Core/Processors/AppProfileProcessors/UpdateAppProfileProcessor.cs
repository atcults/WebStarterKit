using System;
using Core.Commands;
using Core.Commands.AppProfileCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AppProfileProcessors
{
    public class UpdateAppProfileProcessor : ICommandProcessor<UpdateAppProfile>
    {
        private readonly IRepository<AppProfile> _appProfileRepository;

        public UpdateAppProfileProcessor(IRepository<AppProfile> appProfileRepository)
        {
            _appProfileRepository = appProfileRepository;
        }

        public void Process(UpdateAppProfile command, Guid userId, out IWebApiResponse response)
        {

            var appProfile = _appProfileRepository.GetById(command.Id);

            appProfile.Name = command.Name;
            appProfile.IsActive = command.IsActive;

            _appProfileRepository.Update(appProfile);

            response = new WebApiResponseBase();
        }

    }
}