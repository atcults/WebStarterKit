using Common.Enumerations;
using Common.Extensions;
using Common.Helpers;
using Core.Domain.Model;
using NUnit.Framework;
using WebApp.ModelService;

namespace IntegrationTests.ModelServices
{
    public class ContactModelServiceTester : IntegrationTestBase
    {
        [Test]
        public void ShouldGetById()
        {
            var contact = new Contact
            {
                Id = GuidComb.New(),
                Name = "NikunjBBalar",
                Gender = Gender.Male,
                Mobile = "9099791517",
                Email = "a@b.com",
                Description = "With 1 yeat experiance in c#. net MVC",
                ImageData = ImageUtility.NoImageData
            };

            Persist(contact);

            var contactModelService = GetInstance<IContactModelService>();
            var response = contactModelService.GetContactDetailById(contact.Id);

            Assert.AreEqual(response.Detail.Id, contact.Id);
        }

        [Test]
        public void ShouldGetByIdForEdit()
        {
            var contact = new Contact
            {
                Id = GuidComb.New(),
                Name = "NikunjBBalar",
                Gender = Gender.Male,
                Mobile = "9099791517",
                Email = "a@b.com",
                Description = "With 1 yeat experiance in c#. net MVC",
                ImageData = ImageUtility.NoImageData
            };

            Persist(contact);

            var contacttModelService = GetInstance<IContactModelService>();
            var response = contacttModelService.GetContactDetailById(contact.Id);

            Assert.AreEqual(response.Detail.Id, contact.Id);
        }

        [Test]
        public void ShouldTestGetDetailById()
        {
            var contact = new Contact
            {
                Id = GuidComb.New(),
                Name = "NikunjBBalar",
                Gender = Gender.Male,
                Mobile = "9099791517",
                Email = "a@b.com",
                Description = "With 1 yeat experiance in c#. net MVC",
                ImageData = ImageUtility.NoImageData
            };

            var note = new Note
            {
                Id = GuidComb.New(),
                Name = "Folder Name",
                ImageData = ImageUtility.NoImageData,
                EntityType = EntityType.Contact,
                ReferenceId = contact.Id,
                ReferenceName = contact.Name,
                Description = "Finance Account"
            };

            Persist(contact, note);

            var contactModelService = GetInstance<IContactModelService>();

            var response = contactModelService.GetContactDetailById(contact.Id);

            Assert.AreEqual(response.Detail.Id, contact.Id);
            Assert.AreEqual(response.Notes[0].Id, note.Id);
        }
    }
}