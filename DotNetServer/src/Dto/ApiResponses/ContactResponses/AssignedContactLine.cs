using System;

namespace Dto.ApiResponses.ContactResponses
{
    public class AssignedContactLine : AuditedLineResponse
    {
        public Guid ContactId { get; set; }
        public string Gender { get; set; }
        public string GenderValue { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string ContactType { get; set; }
        public string ContactTypeValue { get; set; }
        public string PrimaryLanguage { get; set; }
        public string PrimaryLanguageValue { get; set; }
        public string SecondaryLanguage { get; set; }
        public string SecondaryLanguageValue { get; set; }

        public string EntityTypeValue { get; set; }
        public Guid ReferenceId { get; set; }
        public string ReferenceName { get; set; }
    }
}