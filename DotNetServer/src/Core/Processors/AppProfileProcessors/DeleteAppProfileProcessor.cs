using System;
using Core.Commands;
using Core.Commands.AppProfileCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AppProfileProcessors
{
    public class DeleteAppProfileProcessor : ICommandProcessor<DeleteAppProfile>
    {
        private readonly IRepository<AppProfile> _appProfileRepository;

        public DeleteAppProfileProcessor(IRepository<AppProfile> appProfileRepository)
        {
            _appProfileRepository = appProfileRepository;
        }

        public void Process(DeleteAppProfile command, Guid userId, out IWebApiResponse response)
        {
            response = new WebApiResponseBase();
            var appProfile = _appProfileRepository.GetById(command.Id);
            _appProfileRepository.Delete(appProfile.Id);
        }
    }
}