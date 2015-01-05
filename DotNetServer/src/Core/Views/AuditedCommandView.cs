using System;
using Core.ViewOnly.Base;

namespace Core.Views
{
    public class AuditedCommandView : View
    {
        public Guid? AgreegateId { get; set; }
        public string CommandName { get; set; }
        public Guid PerformingUserId { get; set; }
        public string PerformingUserName { get; set; }
        public DateTime PerformedOn { get; set; }
        public string CommandText { get; set; }
    }
}