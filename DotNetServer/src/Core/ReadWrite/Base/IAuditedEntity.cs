using System;

namespace Core.ReadWrite.Base
{
    public interface IAuditedEntity : IEntity
    {
        string Name { get; set; }
        string Description { get; set; }
        string ImageData { get; set; }
        Guid? CreatedBy { get; set; }
        int RevisionNumber { get; }
        DateTime? CreatedOn { get; set; }
        Guid? ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}