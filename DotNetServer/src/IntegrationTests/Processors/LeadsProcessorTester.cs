using Common.Helpers;
using Core.Commands.LeadCommands;
using Core.Domain.Model;
using Core.ViewOnly;
using Core.Views;
using NUnit.Framework;

namespace IntegrationTests.Processors
{
    public class LeadsProcessorTester : IntegrationTestBase
    {
        [Test]
        public void ShouldAddLeads()
        {
            var admin = GetPersistedSiteUser();

            var command = new AddLead
            {
                Id = GuidComb.New(),
                Name = "Name",
                CompanyName = "Company",
                Email = "temp@temp.com",
                Phone = "0000000000",
                Description = ""
            };
            
            ProcessCommand(command, admin.Id);

            var leadsView = GetInstance<IViewRepository<LeadView>>().GetById(command.Id);

            Assert.AreEqual(command.CompanyName, leadsView.CompanyName);
        }

        [Test]
        public void ShouldUpdateLeads()
        {
            var admin = GetPersistedSiteUser();

            var lead = new Lead
            {
                Id = GuidComb.New(),
                Name = "Name",
                CompanyName = "Company",
                Email = "temp@temp.com",
                Phone = "0000000000",
                Description = "",
                CreatedBy = admin.Id
            };

            Persist(lead);

            var command = new UpdateLead
            {
                Id = lead.Id,
                Name = "Name2",
                CompanyName = "Company2",
                Email = "temp2@temp.com",
                Phone = "1111111111",
                Description = "new description"
            };

            ProcessCommand(command, admin.Id);

            var leadsView = GetInstance<IViewRepository<LeadView>>().GetById(command.Id);

            Assert.AreEqual(leadsView.Id, lead.Id);
            Assert.AreNotEqual(lead.Email, leadsView.Email);
        }

        [Test]
        public void ShouldDeleteLeads()
        {
            var admin = GetPersistedSiteUser();

            var lead = new Lead
            {
                Id = GuidComb.New(),
                Name = "Name",
                CompanyName = "Company",
                Email = "temp@temp.com",
                Phone = "0000000000",
                Description = "",
                CreatedBy = admin.Id
            };

            Persist(lead);

            var command = new DeleteLead(lead.Id);

            ProcessCommand(command, admin.Id);

            var leadsView = GetInstance<IViewRepository<LeadView>>().GetById(command.Id);

            Assert.IsNull(leadsView);
        }
    }
}