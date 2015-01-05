using System;
using Core.Commands;
using Core.Commands.TaskLogCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.TaskLogProcessors
{
    public class DeleteTaskLogProcessor : ICommandProcessor<DeleteTaskLog>
    {
        private readonly IRepository<TaskLog> _taskLogRepository;

        public DeleteTaskLogProcessor(IRepository<TaskLog> taskLogRepository)
        {
            _taskLogRepository = taskLogRepository;
        }

        public void Process(DeleteTaskLog command, Guid userId, out IWebApiResponse response)
        {
            response = new WebApiResponseBase();

            var taskLog = _taskLogRepository.GetById(command.Id);

            if (taskLog == null) throw new DomainProcessException("Task Log not available");

            _taskLogRepository.Delete(taskLog.Id);
        }
    }
}