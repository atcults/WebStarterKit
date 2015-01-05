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
    public class ContactProcessorTester : IntegrationTestBase
    {
        [Test]
        public void ShouldAddContact()
        {
            var admin = GetPersistedSiteUser();

            var command = new AddContact
            {
                Id = GuidComb.New(),
                Name = "Name",
                Gender = Gender.Male,
                Mobile = "0000000000",
                Email = "a@b.com",
                Description = "",
                ImageData = ImageUtility.NoImageData
            };

            ProcessCommand(command, admin.Id);
            
            var contactView = GetInstance<IViewRepository<ContactView>>().GetById(command.Id);

            Assert.AreEqual(contactView.Name, command.Name);
        }

        [Test, ExpectedException]
        public void ShouldNotAddSameEmailContact()
        {
            var admin = GetPersistedSiteUser();

            var contact = new Contact
            {
                Id = GuidComb.New(),
                Name = "Name",
                Gender = Gender.Male,
                Mobile = "0000000000",
                Email = "a@b.com",
                Description = "",
                ImageData = ImageUtility.NoImageData,
                CreatedBy = admin.Id
            };

            Persist(contact);

            var command = new AddContact
            {
                Id = GuidComb.New(),
                Name = "Name2",
                Gender = Gender.Male,
                Mobile = "0000000000",
                Email = "a@b.com",
                Description = "",
                ImageData = ImageUtility.NoImageData
            };

            ProcessCommand(command, admin.Id);
        }

        [Test]
        public void ShouldUpdateContact()
        {
            var admin = GetPersistedSiteUser();

            var contact = new Contact
            {
                Id = GuidComb.New(),
                Name = "Name",
                Gender = Gender.Male,
                Mobile = "0000000000",
                Email = "a@b.com",
                Description = "",
                ImageData = ImageUtility.NoImageData,
                CreatedBy = admin.Id
            };

            Persist(contact);

            var command = new UpdateContact
            {
                Id = contact.Id,
                Name = "Name2",
                Gender = Gender.Male,
                Mobile = "1111111111",
                Email = "a@b.com",
                Description = "new description",
                ImageData = ImageUtility.NoImageData
            };
            ProcessCommand(command, admin.Id);

            var contactView = GetInstance<IViewRepository<ContactView>>().GetById(contact.Id);

            Assert.AreEqual(contactView.Description, command.Description);
        }

        [Test]
        public void ShouldDeleteContact()
        {
            var admin = GetPersistedSiteUser();

            var contact = new Contact
            {
                Id = GuidComb.New(),
                Name = "Name",
                Gender = Gender.Male,
                Mobile = "0000000000",
                Email = "a@b.com",
                Description = "",
                ImageData = ImageUtility.NoImageData,
                CreatedBy = admin.Id
            };

            Persist(contact);

            var command = new DeleteContact(contact.Id);

            ProcessCommand(command, admin.Id);

            var contactView = GetInstance<IViewRepository<ContactView>>().GetById(command.Id);

            Assert.IsNull(contactView);
        }
    }
}