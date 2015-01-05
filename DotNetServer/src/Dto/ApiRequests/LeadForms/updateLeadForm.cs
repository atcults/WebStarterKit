using System;

namespace Dto.ApiRequests.LeadForms
{
    public class UpdateLeadForm : AddLeadForm
    {
        public Guid Id { get; set; }
    }
}