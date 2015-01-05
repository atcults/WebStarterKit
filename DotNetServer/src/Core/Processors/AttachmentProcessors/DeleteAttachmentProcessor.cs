using System;
using Core.Commands;
using Core.Commands.AttachmentCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AttachmentProcessors
{
    public class DeleteAttachmentProcessor : ICommandProcessor<DeleteAttachment>
    {
        private readonly IRepository<Attachment> _attachmentRepository;

        public DeleteAttachmentProcessor(IRepository<Attachment> attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public void Process(DeleteAttachment command, Guid userId, out IWebApiResponse response)
        {
            var attachment = _attachmentRepository.GetById(command.Id);

            _attachmentRepository.Delete(attachment.Id);

            response = new WebApiResponseBase();
        }
    }
}