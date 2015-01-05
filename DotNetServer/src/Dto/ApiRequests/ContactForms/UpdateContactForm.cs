using System;

namespace Dto.ApiRequests.ContactForms
{
    public class UpdateContactForm : AddContactForm
    {
        public Guid Id { get; set; }

        public override string GetApiAddress()
        {
            return "Contact";
        }
    }
}