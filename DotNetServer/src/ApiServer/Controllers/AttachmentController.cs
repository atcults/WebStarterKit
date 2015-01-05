using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Common.SystemSettings;
using Core.Commands.AttachmentCommands;
using Dto.ApiRequests.AttachmentForms;
using Dto.ApiResponses;
using WebApp.Services;
using System.Net.Http;


namespace WebApp.Controllers
{
    public class AttachmentController : SmartApiController
    {

        public AttachmentController(IUserSession userSession, IMappingEngine mappingEngine) : base(userSession, mappingEngine)
        {
        }

        public HttpResponseMessage Post(AttachmentUploadForm form)
        {
            var response = new WebApiResponseBase();

            var tempFilesFolder = Globals.CreateOrGetCustomPath("Temp\\" + form.FileId);

            File.WriteAllText(tempFilesFolder + "\\" + form.ChunkNumber + ".temp", form.ChunkData);

            if (form.ChunkNumber < Math.Ceiling((double)form.FileSize / 102400)) return Content(response);

            var folderInfo = new DirectoryInfo(tempFilesFolder);
            var totalFiles = folderInfo.GetFiles().Length;

            var sb = new StringBuilder();

            for (var i = 1; i <= totalFiles; i++)
            {
                sb.Append(File.ReadAllText(tempFilesFolder + "\\" + i + ".temp"));
            }

            var base64 = sb.ToString();
            base64 = base64.Substring(base64.IndexOf(',') + 1);
            var fileBytes = Convert.FromBase64String(base64);
            var fileStream = new FileStream(tempFilesFolder + "\\" + form.Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
            fileStream.Write(fileBytes, 0, fileBytes.Length);
            fileStream.Close();

            Directory.Delete(tempFilesFolder, true);

            var md5 = MD5.Create();

            var command = Mapper.Map<AttachmentUploadForm, AddAttachment>(form);
            command.FileData = fileBytes;
            command.FileHashCode = BitConverter.ToString(md5.ComputeHash(fileBytes)).Replace("-", "");

            return ExecuteCommand(command);
        }

        public HttpResponseMessage Put(UpdateAttachmentForm form)
        {
            var command = Mapper.Map<UpdateAttachmentForm, UpdateAttachment>(form);
            return ExecuteCommand(command);
        }

        public HttpResponseMessage Delete(Guid id)
        {
            var command = new DeleteAttachment(id);
            return ExecuteCommand(command);
        }
    }
}
