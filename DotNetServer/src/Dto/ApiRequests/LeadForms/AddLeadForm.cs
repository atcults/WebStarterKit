namespace Dto.ApiRequests.LeadForms
{
    public class AddLeadForm : AuditedForm
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0}-{1}", base.ToString(), Name);
        }

        public override string GetApiAddress()
        {
            return "Lead";
        }
    }
}