using System;
using Core.Commands;
using Core.Commands.LeadCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.LeadResponses;

namespace Core.Processors.LeadsProcessors
{
    public class AddLeadsProcessor : ICommandProcessor<AddLead>
    {
        private readonly IRepository<Lead> _leadsRepository;

        public AddLeadsProcessor(IRepository<Lead> leadsRepository)
        {
            _leadsRepository = leadsRepository;
        }

        public void Process(AddLead command, Guid userId, out IWebApiResponse response)
        {
            var leads = new Lead
            {
                Id = command.Id,
                Name = command.Name,
                CompanyName = command.CompanyName,
                Email = command.Email,
                Phone = command.Phone,
                Description = command.Description,
                ImageData = command.ImageData,
                CreatedBy = userId
            };
            _leadsRepository.Add(leads);


            response = new LeadResponse
            {
                Id = command.Id,
                Name = command.Name,
                CompanyName = command.CompanyName,
                Email = command.Email,
                Phone = command.Phone,
                Description = command.Description,
                CreatedBy = userId
            };
        }
    }
}