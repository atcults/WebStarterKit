namespace Dto.ApiRequests.LeadForms
{
    public class AddAnonymousLeadForm : IWebApiRequest
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }

        public string GetCommandValue()
        {
            return string.Format("{0}-{1}", base.ToString(), Name);
        }

        public string GetApiAddress()
        {
            return "Lead";
        }
    }
}