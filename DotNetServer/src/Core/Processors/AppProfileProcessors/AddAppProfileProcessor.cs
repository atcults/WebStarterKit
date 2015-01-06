using System;
using Core.Commands;
using Core.Commands.AppProfileCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AppProfileProcessors
{
    public class AddAppProfileProcessor : ICommandProcessor<AddAppProfile>
    {
        private readonly IRepository<AppProfile> _appProfileRepository;

        public AddAppProfileProcessor(IRepository<AppProfile> appProfileRepository)
        {
            _appProfileRepository = appProfileRepository;
        }

        public void Process(AddAppProfile command, Guid userId, out IWebApiResponse response)
        {
            var appProfile = new AppProfile
            {
                Id = command.Id,
                Name = command.Name,
                IsActive = command.IsActive
            };
            _appProfileRepository.Add(appProfile);


            response = new WebApiResponseBase();
        }
    }
}