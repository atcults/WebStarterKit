using System;
using Core.Commands;
using Core.Commands.LeadCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.LeadsProcessors
{
    public class DeleteLeadsProcessor : ICommandProcessor<DeleteLead>
    {
        private readonly IRepository<Lead> _leadsRepository;

        public DeleteLeadsProcessor(IRepository<Lead> leadsRepository)
        {
            _leadsRepository = leadsRepository;
        }

        public void Process(DeleteLead command, Guid userId, out IWebApiResponse response)
        {
            response = new WebApiResponseBase();
            var leads = _leadsRepository.GetById(command.Id);
            _leadsRepository.Delete(leads.Id);
        }
    }
}