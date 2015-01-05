namespace Dto.ApiResponses.LeadResponses
{
    public class LeadResponse : AuditedResponse
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}