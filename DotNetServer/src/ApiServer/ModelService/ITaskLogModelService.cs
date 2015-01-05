using System;
using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.TaskLogResponses;

namespace WebApp.ModelService
{
    public interface ITaskLogModelService
    {
        PageResponse<TaskLogLine> GetPageBySpecification(SearchSpecification specification = null);
        TaskLogDetailResponse GetTaskLogDetailById(Guid? id = null);
    }
}