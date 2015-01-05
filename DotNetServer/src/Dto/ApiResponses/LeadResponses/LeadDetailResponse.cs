namespace Dto.ApiResponses.LeadResponses
{
    public class LeadDetailResponse : DetailResponse<LeadResponse>
    {
        public LeadDetailResponse()
        {
            EntityTypeValue = "LE";
        }
    }
}