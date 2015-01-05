using System;
using Core.Commands;
using Core.Commands.TemplateCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.TemplateProcessors
{
    public class DeleteTemplateProcessor : ICommandProcessor<DeleteTemplate>
    {
        private readonly IRepository<Template> _templateRepository;

        public DeleteTemplateProcessor(IRepository<Template> templateRepository)
        {
            _templateRepository = templateRepository;
        }

        public void Process(DeleteTemplate command, Guid userId, out IWebApiResponse response)
        {
            response = new WebApiResponseBase();

            var template = _templateRepository.GetById(command.Id);

            if (template == null)
                throw new DomainProcessException("Template not available");

            _templateRepository.Delete(template.Id);
        }
    }
}