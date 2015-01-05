using Common.Enumerations;
using Common.Extensions;
using Common.Helpers;
using Core.Domain.Model;
using NUnit.Framework;
using WebApp.ModelService;

namespace IntegrationTests.ModelServices
{
    public class AttachmentModelServiceTester : IntegrationTestBase
    {
        [Test]
        public void ShouldTestGetDetailById()
        {
            var contact = new Contact
            {
                Id = GuidComb.New(),
                Name = "Priya",
                Gender = Gender.Male,
                ContactType = ContactType.Participant,
                PrimaryLanguage = Language.Gujarati,
                SecondaryLanguage = Language.English,
                Mobile = "mobile",
                Description = "Description",
                ImageData = ImageUtility.NoImageData,
                
            };           

            var attachment = new Attachment
            {
                Id = GuidComb.New(),
                Name = "SanelibAccounts",
                Tags = "hello",
                FileType = "image/png",
                FileSize = 10,
                FileHashCode = "gsdhf35989fsnl1324ywefnj",
                ImageData = ImageUtility.NoImageData,
                EntityType = EntityType.Contact,
                ReferenceId = contact.Id,
                ReferenceName = "AC",
                Description = "This file for Account of sanelib",                
            };

            var attachment1 = new Attachment
            {
                Id = GuidComb.New(),
                Name = "Sailfin",
                Tags = "sanelib",
                FileType = "image/png",
                FileSize = 10,
                FileHashCode = "gsdhf35989fsnl1324ywefnj",
                ImageData = ImageUtility.NoImageData,
                EntityType = EntityType.Contact,
                ReferenceId = contact.Id,
                ReferenceName = "AC",
                Description = "This file for Account of sanelib",                
            };

            var attachment2 = new Attachment
            {
                Id = GuidComb.New(),
                Name = "I-infotechsys",
                Tags = "hello",
                FileType = "image/png",
                FileSize = 10,
                FileHashCode = "gsdhf35989fsnl1324ywefnj",
                ImageData = ImageUtility.NoImageData,
                EntityType = EntityType.Contact,
                ReferenceId = contact.Id,
                ReferenceName = "AC",
                Description = "This file for Account of sanelib",
            };

            var attachment3 = new Attachment
            {
                Id = GuidComb.New(),
                Name = "Account",
                Tags = "hello, sanelib",
                FileType = "image/png",
                FileSize = 10,
                FileHashCode = "gsdhf35989fsnl1324ywefnj",
                ImageData = ImageUtility.NoImageData,
                EntityType = EntityType.Contact,
                ReferenceId = contact.Id,
                ReferenceName = "AC",
                Description = "This file for Account of sanelib",
            };

            Persist( contact, attachment, attachment1, attachment2, attachment3);

            var accountModelService = GetInstance<IAttachmentModelService>();

            var response = accountModelService.GetAttachmentsBySearchValue("Sanelib");
        }
    }
}