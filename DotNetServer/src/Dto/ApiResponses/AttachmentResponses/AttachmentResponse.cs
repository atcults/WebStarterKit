using System;

namespace Dto.ApiResponses.AttachmentResponses
{
    public class AttachmentResponse : AuditedResponse
    {
        public string FileType { get; set; }
        public decimal? FileSize { get; set; }
        public string TypeName { get; set; }
        public Guid ReferenceId { get; set; }
        public string Tags { get; set; }
    }
}