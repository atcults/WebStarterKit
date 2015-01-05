using System;
using Common.Base;
using Common.Enumerations;

namespace Core.Commands.AttachmentCommands
{
    public class AddAttachment : AuditedCommand
    {
        public string FileType { get; set; }
        public decimal FileSize { get; set; }
        public string FileHashCode { get; set; }
        public byte[] FileData { get; set; }
        public EntityType EntityType { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }

        public override Guid? GetAggregateId()
        {
            return Id;
        }

        public override ValidationResult Validate()
        {
            var validationResult = new ValidationResult();
            return validationResult;
        }
    }
}