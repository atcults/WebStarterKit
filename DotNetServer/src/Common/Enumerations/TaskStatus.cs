namespace Common.Enumerations
{
    public class TaskStatus : Enumeration<TaskStatus>
    {
        public TaskStatus(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static TaskStatus Queued = new TaskStatus("QD", "Queued", 0);
        public static TaskStatus Processing = new TaskStatus("PR", "Processing", 1);
        public static TaskStatus Finished = new TaskStatus("FN", "Finished", 2);
        public static TaskStatus Failed = new TaskStatus("FL", "Failed", 3);
    }
}