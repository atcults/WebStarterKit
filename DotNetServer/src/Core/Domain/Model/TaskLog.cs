using System;
using System.Data;
using System.Data.SqlClient;
using Common.Enumerations;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;

namespace Core.Domain.Model
{
    //TODO: Remove attachmentID from task log. Task log should behave like audited members having Notes, contacts and attachments. 
    //Exported file should be as attachment for that task.
    //If task got an exception then it should log in notes for each retry that task is carried out.
    //Add task type like Report, Email, Sms and TaskData which is serialized object of message type.
    //Add retry 5 times with 10 min delay. Each time it should add note that task is retrying. If error comes then stack trace should be note description.
    [EntityName("TaskLog")]
    public class TaskLog : AuditedEntity
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TaskStatus TaskStatus { get; set; }

        public override void From(IDataReader dataReader)
        {
            base.From(dataReader);
            StartTime = dataReader.ReadNullSafeDateTime("StartTime");
            EndTime = dataReader.ReadNullSafeDateTime("EndTime");
            TaskStatus = TaskStatus.FromValue(dataReader.ReadNullSafeString("TaskStatusValue"));
        }

        public override void To(SqlCommand cmd)
        {
            base.To(cmd);
            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = (object) StartTime ?? DBNull.Value;
            cmd.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = (object) EndTime ?? DBNull.Value;
            cmd.Parameters.Add("@TaskStatusValue", SqlDbType.Char, 2).Value = BaseEnumeration.GetDbNullSafe(TaskStatus) ?? DBNull.Value;
        }
    }
}