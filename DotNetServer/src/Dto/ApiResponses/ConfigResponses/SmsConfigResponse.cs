namespace Dto.ApiResponses.ConfigResponses
{
    public class SmsConfigResponse : WebApiResponseBase
    {
        public string ServiceUrl { get; set; }
        public string SenderName { get; set; }
    }
}