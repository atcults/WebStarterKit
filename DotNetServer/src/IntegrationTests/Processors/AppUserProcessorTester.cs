using Common.Enumerations;
using Common.Extensions;
using Common.Helpers;
using Core.Commands.AppUserCommands;
using Core.ViewOnly;
using Core.Views;
using NUnit.Framework;

namespace IntegrationTests.Processors
{
    public class AppUserProcessorTester : IntegrationTestBase
    {
        [Test]
        public void ShouldAddAppUser()
        {
            var admin = GetPersistedSiteUser();
            var command = new AddAppUser
            {
                Id = GuidComb.New(),
                Name = "Name",
                Mobile = "0000000000",
                Email = "a@b.com",
                UserStatus = UserStatus.Active,
                Role = Role.Admin
            };

            ProcessCommand(command, admin.Id);

            var repo = GetInstance<IViewRepository<AppUserView>>();
            var member = repo.GetByKey(Property.Of<AppUserView>(x => x.Id), command.Id);
            Assert.AreEqual(member.Name, command.Name);
        }

        [Test]
        public void ShouldEditAppUser()
        {
            var admin = GetPersistedSiteUser();
            
            var command = new UpdateAppUser
            {
                Id = admin.Id,
                Name = "New Name",
                Email = "mail@gmail.com",
                Mobile = "0000000000",
                Role = Role.Owner,
                UserStatus = UserStatus.Active,
                ImageData = ImageUtility.NoImageData,
            };

            ProcessCommand(command, admin.Id);

            var repo = GetInstance<IViewRepository<AppUserView>>();
            var memberrepo = repo.GetByKey(Property.Of<AppUserView>(x => x.Id), command.Id);
            Assert.AreEqual(command.Email, memberrepo.Email);
        }

        [Test]
        public void ShouldDeleteAppUser()
        {
            var admin = GetPersistedSiteUser();

            var command = new DeleteAppUser(admin.Id);
            ProcessCommand(command, admin.Id);
            var repo = GetInstance<IViewRepository<AppUserView>>();
            var memberView = repo.GetById(admin.Id);
            Assert.IsNull(memberView);
        }
    }
}