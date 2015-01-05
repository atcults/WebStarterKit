using System;

namespace Dto.ApiResponses.TaskLogResponses
{
    public class TaskLogLine : AuditedLineResponse
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TaskStatusName { get; set; }
        public string TaskStatusValue { get; set; }
        public string Description { get; set; }
    }
}