using System;

namespace Core.ViewOnly.Base
{
    public interface IAuditedView : IView
    {
        string Name { get; set; }
        string Description { get; set; }
        string ImageData { get; set; }
        int RevisionNumber { get; set; }
        Guid? CreatedBy { get; set; }
        string CreatedByName { get; set; }
        DateTime? CreatedOn { get; set; }
        Guid? ModifiedBy { get; set; }
        string ModifiedByName { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}