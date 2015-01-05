using System;

namespace Dto.ApiRequests.ContactForms
{
    public class AddAssignedContactForm : AuditedForm
    {
        public Guid ContactId { get; set; }
        public string EntityTypeValue { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }

        public override string GetCommandValue()
        {
            return "";
        }

        public override string GetApiAddress()
        {
            return "AssignedContact";
        }
    }
}