namespace Dto.ApiResponses.LeadResponses
{
    public class LeadLine : AuditedLineResponse
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
    }
}