using System;
using Core.Commands;
using Core.Commands.AttachmentCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.AttachmentResponses;

namespace Core.Processors.AttachmentProcessors
{
    public class AddAttachmentProcessor : ICommandProcessor<AddAttachment>
    {
        private readonly IRepository<Attachment> _attachmentRepository;

        public AddAttachmentProcessor(IRepository<Attachment> attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public void Process(AddAttachment command, Guid userId, out IWebApiResponse response)
        {
            var attachment = new Attachment
            {
                Id = command.Id,
                Name = command.Name,
                FileType = command.FileType,
                FileSize = command.FileSize,
                FileHashCode = command.FileHashCode,
                FileData = command.FileData,
                EntityType = command.EntityType,
                Tags = string.Empty,
                ReferenceId = command.ReferenceId,
                ReferenceName = command.ReferenceName,
                Description = command.Description,
                ImageData = command.ImageData,
                CreatedBy = userId
            };
            _attachmentRepository.Add(attachment);

            response = new AttachmentResponse
            {
                Id = command.Id,
                Name = command.Name,
                TypeName = command.EntityType.Value,
                FileType = command.FileType,
                FileSize = command.FileSize,
                Description = command.Description,
                CreatedOn = DateTime.Now,
                ReferenceId = command.ReferenceId
            };
        }
    }
}