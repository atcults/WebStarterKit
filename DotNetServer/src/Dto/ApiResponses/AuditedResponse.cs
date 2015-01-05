using System;

namespace Dto.ApiResponses
{
    public abstract class AuditedResponse : WebApiResponseBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageData { get; set; }
        public int RevisionNumber { get; set; }
        public Guid? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}