namespace Dto.ApiResponses.TaskLogResponses
{
    public class TaskLogDetailResponse : DetailResponse<TaskLogResponse>
    {
        public TaskLogDetailResponse()
        {
            EntityTypeValue = "TS";
        }
    }
}