using System;
using Common.Enumerations;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;
using Newtonsoft.Json;

namespace Core.Views
{
    [ViewName("TaskLogView")]
    public class TaskLogView : AuditedView
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        [JsonIgnore]
        public TaskStatus TaskStatus { get; set; }

        public string TaskStatusValue
        {
            get { return TaskStatus == null ? "" : TaskStatus.Value; }
            set { TaskStatus = TaskStatus.FromValue(value); }
        }

        public string TaskStatusName
        {
            get { return TaskStatus == null ? "" : TaskStatus.DisplayName; }
            set { TaskStatus = TaskStatus.FromDisplay(value); }
        }
    }
}