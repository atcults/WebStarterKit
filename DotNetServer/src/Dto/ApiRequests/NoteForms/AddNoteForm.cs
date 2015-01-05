using System;

namespace Dto.ApiRequests.NoteForms
{
    public class AddNoteForm : AuditedForm
    {
        public Guid ReferenceId { get; set; }
        public string EntityTypeValue { get; set; }
        public string ReferenceName { get; set; }

        public override string GetCommandValue()
        {
            return Name;
        }

        public override string GetApiAddress()
        {
            return "Note";
        }
    }
}