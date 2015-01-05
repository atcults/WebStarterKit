using System;

namespace Dto.ApiResponses
{
    public abstract class LineResponse
    {
        public Guid Id { get; set; }
    }

    public abstract class AuditedLineResponse : LineResponse
    {
        public string Name { get; set; }
        public int RevisionNumber { get; set; }
        public Guid? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}