using System;

namespace Dto.ApiResponses.AttachmentResponses
{
    public class AttachmentLine : AuditedLineResponse
    {
        public string FileType { get; set; }
        public decimal? FileSize { get; set; }
        public string TypeName { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string Tags { get; set; }
        public string EntityTypeValue { get; set; }
        public string EntityTypeName { get; set; }
        public string Description { get; set; }
    }
}