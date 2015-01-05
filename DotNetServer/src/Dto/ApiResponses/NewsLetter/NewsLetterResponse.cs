using System;

namespace Dto.ApiResponses.NewsLetter
{
    public class NewsLetterResponse : WebApiResponseBase
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}