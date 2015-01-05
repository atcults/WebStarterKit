using Common.Base;
using Common.Enumerations;
using Dto.ApiResponses.AttachmentResponses;
using Dto.ApiResponses.ContactResponses;
using Dto.ApiResponses.NoteResponses;

namespace Dto.ApiResponses
{
    public class DetailResponse<T> where T : AuditedResponse
    {
        public DetailResponse()
        {
            ContactLines = new AssignedContactLine[0];
            Notes = new NoteLine[0];
            Attachments = new AttachmentLine[0];
            ContactTypes = ContactType.GetAllPaired();
            Languages = Language.GetAllPaired();
            Genders = Gender.GetAllPaired();
        }

        public T Detail { get; set; }
        public string EntityTypeValue { get; set; }
        public ContactLine[] AllContacts { get; set; }
        public AssignedContactLine[] ContactLines { get; set; }
        public NoteLine[] Notes { get; set; }
        public AttachmentLine[] Attachments { get; set; }
        public IdNamePair[] ContactTypes { get; set; }
        public IdNamePair[] Languages { get; set; }
        public IdNamePair[] Genders { get; set; }
    }
}