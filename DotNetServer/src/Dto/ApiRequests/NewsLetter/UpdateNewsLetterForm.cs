using System;

namespace Dto.ApiRequests.NewsLetter
{
    public class UpdateNewsLetterForm : AddNewsLetterForm
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
    }
}