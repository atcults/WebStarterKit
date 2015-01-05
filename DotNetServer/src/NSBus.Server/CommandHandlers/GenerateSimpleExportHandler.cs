using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Common.Enumerations;
using Common.Extensions;
using Common.Helpers;
using Common.OpenXml;
using Core.Domain.Model;
using Core.ReadWrite;
using Core.ViewOnly;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class GenerateSimpleExportHandler : MessageHandler<GenerateSimpleExportCommand>
    {
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IRepository<TaskLog> _taskLogRepository;
        private readonly IRepository<Note> _noteRepository;
        private readonly IRepository<Attachment> _attachmentRepository;

        public GenerateSimpleExportHandler(IUnitOfWork unitOfWork,
            IRepository<AppUser> appUserRepository,
            IRepository<TaskLog> taskLogRepository,
            IRepository<Note> noteRepository,
            IRepository<Attachment> attachmentRepository, IBus bus)
            : base(unitOfWork, bus)
        {
            _appUserRepository = appUserRepository;
            _taskLogRepository = taskLogRepository;
            _noteRepository = noteRepository;
            _attachmentRepository = attachmentRepository;
        }

        public override void HandleMessage(GenerateSimpleExportCommand command)
        {
            var view = _appUserRepository.GetById(command.UserId);

            var taskLog = new TaskLog
            {
                Id = GuidComb.New(),
                Name = string.Format("{0} on {1}", command, DateTime.Now),
                TaskStatus = TaskStatus.Processing,
                StartTime = DateTime.Now,
                CreatedBy = view.Id,
                CreatedOn = DateTime.Now
            };

            _taskLogRepository.Add(taskLog);

            try
            {
                var data = new List<object>();

                var viewType = "Core.Views." + EntityType.FromValue(command.ViewType).DisplayName + "View, Core";

                Log.InfoFormat("ViewType : {0}", viewType);

                var openType = typeof(IViewRepository<>); //generic open type

                //modelType is your runtime type
                var type = openType.MakeGenericType(Type.GetType(viewType));

                Log.InfoFormat("Generic Type : {0}", type);

                dynamic viewRepository = NBusServerEndPoint.Container.GetInstance(type); //should get your ConcreteABuilder 

                command.SearchSpecification.Page = 1;
                command.SearchSpecification.PageSize = 1000;

                var firstPage = viewRepository.SearchBySpecification(command.SearchSpecification);

                data.AddRange(firstPage.Items);

                for (var i = 2; i <= firstPage.TotalPages; i++)
                {
                    command.SearchSpecification.Page = i;
                    var page = viewRepository.SearchBySpecification(command.SearchSpecification);
                    data.AddRange(page.Items);
                }

                taskLog.TaskStatus = TaskStatus.Finished;
                taskLog.Description = "Report";
                taskLog.EndTime = DateTime.Now;
                taskLog.ModifiedBy = view.Id;
                taskLog.ModifiedOn = DateTime.Now;
                _taskLogRepository.Update(taskLog);

                ExportFile(taskLog.Id, taskLog.Name, view.Id, data.ToArray());

                Bus.SendLocal(new SendNotificationCommand
                {
                    UserId = command.UserId,
                    NotificationTypeValue = NotificationType.TaskCompleted.Value,
                    ViewDataSingle = new Dictionary<string, object> {{"TaskView", taskLog}}
                });
            }
            catch (Exception e)
            {
                taskLog.TaskStatus = TaskStatus.Failed;
                taskLog.Description = e.StackTrace;
                taskLog.EndTime = DateTime.Now;
                taskLog.ModifiedBy = view.Id;
                taskLog.ModifiedOn = DateTime.Now;
                _taskLogRepository.Update(taskLog);

                _noteRepository.Add(new Note
                {
                    Id = GuidComb.New(),
                    Name = "Task Failed " + DateTime.Now.ToShortDateString(),
                    ReferenceId = taskLog.Id,
                    ReferenceName = taskLog.Name,
                    Description = e.ToString(),
                    EntityType = EntityType.TaskLog,
                    CreatedBy = view.Id
                });

                Bus.SendLocal(new SendNotificationCommand
                {
                    UserId = command.UserId,
                    NotificationTypeValue = NotificationType.TaskFailed.Value,
                    ViewDataSingle = new Dictionary<string, object> { { "TaskView", taskLog } }
                });
            }
        }

        private void ExportFile(Guid taskId, string taskName, Guid userId, ICollection<object> data)
        {
            const string fileName = "Report.xlsx";
            if (data == null || data.Count == 0) return;

            var export = new ExportSimple(fileName, data);
            export.CreatePackage();

            //file Property
            var attachmentFileProperty = new FileInfo(fileName);

            // file bytes
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var binaryReader = new BinaryReader(fileStream);
            var numBytes = attachmentFileProperty.Length;
            var fileData = binaryReader.ReadBytes((int)numBytes);
            fileStream.Close();
            
            // for MD5 hash code
            var md5 = MD5.Create();

            var attachment = new Attachment
            {
                Id = GuidComb.New(),
                Name = attachmentFileProperty.Name,
                FileType = attachmentFileProperty.Extension,
                FileSize = attachmentFileProperty.Length / 1024 / 1024,
                FileHashCode = BitConverter.ToString(md5.ComputeHash(fileData)).Replace("-", ""),
                FileData = fileData,
                EntityType = EntityType.TaskLog,
                ReferenceId = taskId,
                ReferenceName = taskName,
                Description = "Report",
                ImageData = ImageUtility.NoImageData,
                CreatedBy = userId
            };
            
            _attachmentRepository.Add(attachment);

            File.Delete(fileName);
        }
    }
}
