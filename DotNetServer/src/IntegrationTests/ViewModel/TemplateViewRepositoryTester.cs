using Common.Helpers;
using Core.Domain.Model;
using Core.ViewOnly;
using Core.Views;
using NUnit.Framework;

namespace IntegrationTests.ViewModel
{
    public class TemplateViewRepositoryTester : IntegrationTestBase
    {
        [Test]
        public void ShouldAddTemplateView()
        {
            var template = new Template
            {
                Id = GuidComb.New(),
                Name = "TemplateName",
                MailBody = "TemplateValue",
                SmsBody = "TemplateValue"
            };

            Persist(template);

            var repo = GetInstance<IViewRepository<TemplateView>>();
            var templateView = repo.GetByKey(Property.Of<TemplateView>(x => x.Id), template.Id);

            Assert.AreEqual(templateView.Id, template.Id);
            Assert.AreEqual(templateView.Name, template.Name);
            Assert.AreEqual(templateView.MailBody, template.MailBody);
        }
    }
}