using System;

namespace Dto.ApiResponses.TemplateResponses
{
    public class TemplateResponse : WebApiResponseBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MailBody { get; set; }
        public string SmsBody { get; set; }
        
    }
}