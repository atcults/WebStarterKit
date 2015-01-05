using System;
using System.Linq;
using Common.Helpers;
using Core.Commands;
using Core.Commands.LeadCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.LeadResponses;

namespace Core.Processors.LeadsProcessors
{
    public class UpdateLeadsProcessor : ICommandProcessor<UpdateLead>
    {
        private readonly IRepository<Lead> _leadsRepository;

        public UpdateLeadsProcessor(IRepository<Lead> leadsRepository)
        {
            _leadsRepository = leadsRepository;
        }

        public void Process(UpdateLead command, Guid userId, out IWebApiResponse response)
        {
            EnsureSameEmail(command);

            var leads = _leadsRepository.GetById(command.Id);

            leads.Name = command.Name;
            leads.CompanyName = command.CompanyName;
            leads.Email = command.Email;
            leads.Phone = command.Phone;
            leads.ImageData = command.ImageData;
            leads.Description = command.Description;
            leads.ModifiedBy = userId;

            _leadsRepository.Update(leads);

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

        private void EnsureSameEmail(AddLead command)
        {
            var alradyAddedWithSameEmail =
                _leadsRepository.GetAllFor(Property.Of<Lead>(x => x.Email), command.Email).SingleOrDefault();

            if (alradyAddedWithSameEmail == null || alradyAddedWithSameEmail.Id == command.Id) return;
            throw new DomainProcessException(
                string.Format("Email should not be duplicated. Already available contact with this name : {0}",
                    command.Name));
        }
    }
}