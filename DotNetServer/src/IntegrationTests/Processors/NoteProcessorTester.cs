using Common.Enumerations;
using Common.Extensions;
using Common.Helpers;
using Core.Commands.NoteCommands;
using Core.Domain.Model;
using Core.ViewOnly;
using Core.Views;
using NUnit.Framework;

namespace IntegrationTests.Processors
{
    public class NoteProcessorTester : IntegrationTestBase
    {
        [Test]
        public void ShouldAddNote()
        {
            var admin = GetPersistedSiteUser();

            var command = new AddNote
            {
                Id = GuidComb.New(),
                Name = "Details",
                EntityType = EntityType.Contact,
                ReferenceId = GuidComb.New(),
                ReferenceName = "",
                Description = "Description",
                ImageData = ImageUtility.NoImageData
            };

            ProcessCommand(command, admin.Id);
            var repo = GetInstance<IViewRepository<NoteView>>();
            var noteView = repo.GetById(command.Id);

            Assert.AreEqual(command.Description, noteView.Description);
        }

        [Test]
        public void ShouldUpdateNote()
        {
            var admin = GetPersistedSiteUser();

            var note = new Note
            {
                Id = GuidComb.New(),
                Name = "Details",
                EntityType = EntityType.Contact,
                ReferenceId = GuidComb.New(),
                ReferenceName = "Contact",
                Description = "Description",
                ImageData = ImageUtility.NoImageData
            };

            Persist(note);

            var command = new UpdateNote
            {
                Id = note.Id,
                Name = "Notes",
                EntityType = note.EntityType,
                ReferenceId = note.ReferenceId,
                ReferenceName = note.ReferenceName,
                Description = "new description",
                ImageData = ImageUtility.NoImageData
            };

            ProcessCommand(command, admin.Id);

            var noteView = GetInstance<IViewRepository<NoteView>>().GetById(command.Id);

            Assert.AreEqual(noteView.Id, note.Id);
            Assert.AreNotEqual(command.Description, note.Description);
        }

        [Test]
        public void ShouldDeleteNote()
        {
            var admin = GetPersistedSiteUser();

            var note = new Note
            {
                Id = GuidComb.New(),
                Name = "Details",
                ImageData = ImageUtility.NoImageData,
                EntityType = EntityType.Contact,
                ReferenceId = GuidComb.New(),
                ReferenceName = "Contact",
                Description = "Finance Account"
            };

            Persist(note);

            var command = new DeleteNote(note.Id);
            
            ProcessCommand(command, admin.Id);

            var noteView = GetInstance<IViewRepository<NoteView>>().GetById(command.Id);

            Assert.IsNull(noteView);
        }
    }
}