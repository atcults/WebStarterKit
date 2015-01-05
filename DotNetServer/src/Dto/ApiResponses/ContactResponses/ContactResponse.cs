namespace Dto.ApiResponses.ContactResponses
{
    public class ContactResponse : AuditedResponse
    {
        public string Gender { get; set; }
        public string GenderValue { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ContactType { get; set; }
        public string ContactTypeValue { get; set; }
        public string PrimaryLanguage { get; set; }
        public string PrimaryLanguageValue { get; set; }
        public string SecondaryLanguage { get; set; }
        public string SecondaryLanguageValue { get; set; }
    }
}