using System;

namespace Dto.ApiRequests.AttachmentForms
{
    public class AttachmentUploadForm : AuditedForm
    {
        public string FileId { get; set; }
        public string FileType { get; set; }
        public decimal FileSize { get; set; }
        public int ChunkNumber { get; set; }
        public string ChunkData { get; set; }
        public string EntityTypeValue { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }

        public override string GetCommandValue()
        {
            return Name;
        }

        public override string GetApiAddress()
        {
            return "Attachment";
        }
    }
}