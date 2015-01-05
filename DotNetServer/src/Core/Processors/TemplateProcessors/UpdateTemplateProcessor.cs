using System;
using Core.Commands;
using Core.Commands.TemplateCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.TemplateResponses;

namespace Core.Processors.TemplateProcessors
{
    public class UpdateTemplateProcessor : ICommandProcessor<UpdateTemplate>
    {
        private readonly IRepository<Template> _templateRepository;

        public UpdateTemplateProcessor(IRepository<Template> templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public void Process(UpdateTemplate command, Guid userId, out IWebApiResponse response)
        {
            var template = _templateRepository.GetById(command.Id);

            template.Name = command.Name;
            template.MailBody = command.MailBody;
            template.SmsBody = command.SmsBody;

            _templateRepository.Update(template);

            response = new TemplateResponse
            {
                Id = command.Id,
                Name = command.Name
            };
        }
    }
}