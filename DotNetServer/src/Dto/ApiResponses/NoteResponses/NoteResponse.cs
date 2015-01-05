using System;

namespace Dto.ApiResponses.NoteResponses
{
    public class NoteResponse : AuditedResponse
    {
        public string EntityTypeValue { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }
    }
}