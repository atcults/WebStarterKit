using System;
using Common.Helpers;
using Core.Commands;
using Core.Commands.TemplateCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.TemplateResponses;

namespace Core.Processors.TemplateProcessors
{
    public class AddTemplateProcessor : ICommandProcessor<AddTemplate>
    {
        private readonly IRepository<Template> _templateRepository;

        public AddTemplateProcessor(IRepository<Template> templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public void Process(AddTemplate command, Guid userId, out IWebApiResponse response)
        {
            EnsureUniqueness(command);

            var template = new Template
            {
                Id = command.Id,
                Name = command.Name,
                MailBody = command.MailBody,
                SmsBody = command.SmsBody
            };

            _templateRepository.Add(template);

            response = new TemplateResponse
            {
                Id = command.Id,
                Name = command.Name
            };
        }

        private void EnsureUniqueness(AddTemplate command)
        {
            var templateWithSameName = _templateRepository.GetByKey(Property.Of<Template>(entity => entity.Name),
                command.Name);

            if (templateWithSameName != null)
            {
                throw new DomainProcessException("Template with same name exist.");
            }
        }
    }
}