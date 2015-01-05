using System;

namespace Dto.ApiRequests.ContactForms
{
    public class UpdateAssignedContactForm : AddAssignedContactForm
    {
        public Guid Id { get; set; }

        public override string GetApiAddress()
        {
            return "AssignedContact";
        }
    }
}