using System;

namespace Dto.ApiResponses.TaskLogResponses
{
    public class TaskLogResponse : AuditedResponse
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string TaskStatusName { get; set; }
    }
}