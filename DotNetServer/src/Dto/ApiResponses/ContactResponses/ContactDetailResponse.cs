namespace Dto.ApiResponses.ContactResponses
{
    public class ContactDetailResponse : DetailResponse<ContactResponse>
    {
        public ContactDetailResponse()
        {
            EntityTypeValue = "CO";
        }
    }
}