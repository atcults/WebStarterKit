namespace Dto.ApiResponses.AttachmentResponses
{
    public class AttachmentSearchResponse : WebApiResponseBase
    {
        public AttachmentSearchResponse()
        {
            AttachmentLines = new AttachmentLine[0];
        }

        public AttachmentLine[] AttachmentLines { get; set; }
    }
}