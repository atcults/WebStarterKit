using Core.ViewOnly;
using Core.Views;
using NUnit.Framework;

namespace IntegrationTests.ViewModel
{
    public class AppUserViewRepositoryTester : IntegrationTestBase
    {
        [Test]
        public void ShouldGetUserByEmail()
        {
            var member = GetPersistedSiteUser();

            var contact = GetInstance<IViewRepository<ContactView>>().GetById(member.Id);
            Assert.AreEqual("temp@abc.com", contact.Email);
        }
    }
}