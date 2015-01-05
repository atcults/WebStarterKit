using System;
using Core.Commands;
using Core.Commands.AttachmentCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AttachmentProcessors
{
    public class UpdateAttachmentProcessor : ICommandProcessor<UpdateAttachment>
    {
        private readonly IRepository<Attachment> _attachmentRepository;

        public UpdateAttachmentProcessor(IRepository<Attachment> attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public void Process(UpdateAttachment command, Guid userId, out IWebApiResponse response)
        {
            var attachment = _attachmentRepository.GetById(command.Id);

            attachment.Description = command.Description;
            attachment.Tags = command.Tags;
            attachment.ModifiedBy = userId;

            _attachmentRepository.Update(attachment);

            response = new WebApiResponseBase();
        }
    }
}