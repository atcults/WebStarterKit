using System;

namespace Dto.ApiRequests.NoteForms
{
    public class UpdateNoteForm : AddNoteForm
    {
        public Guid Id { get; set; }
    }
}