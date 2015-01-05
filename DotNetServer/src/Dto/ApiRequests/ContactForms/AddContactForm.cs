namespace Dto.ApiRequests.ContactForms
{
    public class AddContactForm : AuditedForm
    {
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ContactType { get; set; }
        public string PrimaryLanguage { get; set; }
        public string SecondaryLanguage { get; set; }
        
        public override string GetCommandValue()
        {
            return "";
        }

        public override string GetApiAddress()
        {
            return "Contact";
        }
    }
}