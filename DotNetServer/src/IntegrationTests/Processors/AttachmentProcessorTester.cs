using System.Text;
using Common.Enumerations;
using Common.Extensions;
using Common.Helpers;
using Core.Commands.AttachmentCommands;
using Core.Domain.Model;
using Core.ViewOnly;
using Core.Views;
using NUnit.Framework;

namespace IntegrationTests.Processors
{
    public class AttachmentProcessorTester : IntegrationTestBase
    {
        [Test]
        public void ShouldAddAttachment()
        {
            var admin = GetPersistedSiteUser();

            var command = new AddAttachment
            {
                Id = GuidComb.New(),
                Name = "SanelibAccounts",
                FileType = "image/png",
                FileSize = 10,
                FileHashCode = "gsdhf35989fsnl1324ywefnj",
                FileData = Encoding.ASCII.GetBytes(ImageUtility.NoImageData), 
                ImageData = ImageUtility.NoImageData,
                EntityType = EntityType.Contact,
                ReferenceId = admin.Id,
                ReferenceName = "admin",
                Description = ""
            };

            ProcessCommand(command, admin.Id);

            var attachmentView = GetInstance<IViewRepository<AttachmentView>>().GetById(command.Id);

            Assert.AreEqual(command.Name, attachmentView.Name);
        }

        [Test, Explicit]
        public void ShouldUpdateAttachment()
        {
            var admin = GetPersistedSiteUser();

            var attachment = new Attachment
            {
                Id = GuidComb.New(),
                Name = "SanelibAccounts",
                FileType = "image/png",
                FileSize = 10,
                FileHashCode = "gsdhf35989fsnl1324ywefnj",
                FileData = Encoding.ASCII.GetBytes(ImageUtility.NoImageData), 
                ImageData = ImageUtility.NoImageData,
                EntityType = EntityType.Contact,
                ReferenceId = admin.Id,
                ReferenceName = "",
                Description = "",
                CreatedBy = admin.Id
            };
            Persist(attachment);

            var command = new UpdateAttachment
            {
                Id = attachment.Id,
                Name = "SanelibAccounts",
                ImageData = ImageUtility.NoImageData,
                Description = "new description"
            };

            ProcessCommand(command, admin.Id);

            var attachmentView = GetInstance<IViewRepository<AttachmentView>>().GetById(command.Id);

            Assert.AreEqual(attachmentView.Id, attachment.Id);
            Assert.AreNotEqual(attachmentView.Description, attachment.Description);
        }

        [Test]
        public void ShouldDeleteAttachment()
        {
            var admin = GetPersistedSiteUser();

            var attachment = new Attachment
            {
                Id = GuidComb.New(),
                Name = "SanelibAccounts",
                FileType = "image/png",
                FileSize = 10,
                FileHashCode = "gsdhf35989fsnl1324ywefnj",
                FileData = Encoding.ASCII.GetBytes(ImageUtility.NoImageData),
                ImageData = ImageUtility.NoImageData,
                EntityType = EntityType.Contact,
                ReferenceId = admin.Id,
                ReferenceName = "",
                Description = "",
                CreatedBy = admin.Id
            };

            Persist(attachment);

            var command = new DeleteAttachment(attachment.Id);
            ProcessCommand(command, admin.Id);

            var attachmentView = GetInstance<IViewRepository<AttachmentView>>().GetById(command.Id);

            Assert.IsNull(attachmentView);
        }
    }
}