using Common.Enumerations;
using Common.Extensions;
using Common.Helpers;
using Core.Commands.ContactCommands;
using Core.Domain.Model;
using Core.ViewOnly;
using Core.Views;
using NUnit.Framework;

namespace IntegrationTests.Processors
{
    public class AssignedContactProcessorTester : IntegrationTestBase
    {
        [Test]
        public void ShouldAddAssignedContact()
        {
            var admin = GetPersistedSiteUser();
            var contact1 = new Contact
            {
                Id = GuidComb.New(),
                Name = "New Contact 1",
                Mobile = "0000000000",
                Description = "Description",
                ImageData = ImageUtility.NoImageData
            };

            var contact2 = new Contact
            {
                Id = GuidComb.New(),
                Name = "New Contact 2",
                Mobile = "1111111111",
                Description = "Description",
                ImageData = ImageUtility.NoImageData
            };

            Persist(contact1, contact2);

            var command = new AddAssignedContact
            {
                Id = GuidComb.New(),
                Name = contact1.Name,
                ContactId = contact1.Id,
                EntityType = EntityType.Contact,
                ReferenceName = contact2.Name,
                ReferenceId = contact2.Id,
                ImageData = ImageUtility.NoImageData,
                Description = "",
            };

            ProcessCommand(command, admin.Id);
            
            var repo = GetInstance<IViewRepository<AssignedContactView>>();
            var assignedContactView = repo.GetById(command.Id);

            Assert.AreEqual(assignedContactView.Name, command.Name);
        }

        [Test]
        public void ShouldUpdateAssignedContact()
        {
            var admin = GetPersistedSiteUser();

            var contact1 = new Contact
            {
                Id = GuidComb.New(),
                Name = "New Contact 1",
                Mobile = "0000000000",
                Description = "Description",
                ImageData = ImageUtility.NoImageData
            };

            var contact2 = new Contact
            {
                Id = GuidComb.New(),
                Name = "New Contact 2",
                Mobile = "1111111111",
                Description = "Description",
                ImageData = ImageUtility.NoImageData
            };

            var assignedContact = new AssignedContact
            {
                Id = GuidComb.New(),
                Name = contact1.Name,
                ContactId = contact1.Id,
                EntityType = EntityType.Contact,
                ReferenceName = contact2.Name,
                ReferenceId = contact2.Id,
                ImageData = ImageUtility.NoImageData,
                Description = "Description",
            };

            Persist(contact1, contact2, assignedContact);

            var command = new UpdateAssignedContact
            {
                Id = assignedContact.Id,
                Name = "New Name",
                ContactId = contact1.Id,
                EntityType = EntityType.Contact,
                ReferenceName = contact2.Name,
                ReferenceId = contact2.Id,
                ImageData = ImageUtility.NoImageData,
                Description = "New description",
            };

            ProcessCommand(command, admin.Id);

            var assignedContactView = GetInstance<IViewRepository<AssignedContactView>>().GetById(command.Id);

            Assert.AreEqual(assignedContactView.Name, assignedContact.Name);
        }

        [Test]
        public void ShouldDeleteAssignedContact()
        {
            var admin = GetPersistedSiteUser();

            var contact1 = new Contact
            {
                Id = GuidComb.New(),
                Name = "New Contact 1",
                Mobile = "0000000000",
                Description = "Description",
                ImageData = ImageUtility.NoImageData
            };

            var contact2 = new Contact
            {
                Id = GuidComb.New(),
                Name = "New Contact 2",
                Mobile = "1111111111",
                Description = "Description",
                ImageData = ImageUtility.NoImageData
            };

            var assignedcontact = new AssignedContact
            {
                Id = GuidComb.New(),
                Name = contact1.Name,
                ContactId = contact1.Id,
                EntityType = EntityType.Contact,
                ReferenceName = contact2.Name,
                ReferenceId = contact2.Id,
                ImageData = ImageUtility.NoImageData,
                Description = "Description",
            };

            Persist(contact1, contact2, assignedcontact);

            var command = new DeleteAssignedContact(assignedcontact.Id);
           
            ProcessCommand(command, admin.Id);

            var contactView = GetInstance<IViewRepository<AssignedContactView>>().GetById(command.Id);

            Assert.IsNull(contactView);
        }
    }
}