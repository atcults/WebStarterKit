using System;

namespace Dto.ApiResponses.ContactResponses
{
    public class AssignedContactResponse : AuditedResponse
    {
        public Guid ContactId { get; set; }
        public string EntityTypeValue { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}