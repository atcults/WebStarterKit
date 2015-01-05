using System;
using Core.Commands;
using Core.Commands.TaskLogCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.TaskLogResponses;

namespace Core.Processors.TaskLogProcessors
{
    public class AddTaskLogProcessor : ICommandProcessor<AddTaskLog>
    {
        private readonly IRepository<TaskLog> _taskLogRepository;

        public AddTaskLogProcessor(IRepository<TaskLog> taskLogRepository)
        {
            _taskLogRepository = taskLogRepository;
        }

        public void Process(AddTaskLog command, Guid userId, out IWebApiResponse response)
        {
            var taskLog = new TaskLog
            {
                Id = command.Id,
                Name = command.Name,
                TaskStatus = command.TaskStatus,
                CreatedBy = userId
            };
            _taskLogRepository.Add(taskLog);

            response = new TaskLogResponse
            {
                Id = command.Id,
                Name = command.Name
            };
        }
    }
}