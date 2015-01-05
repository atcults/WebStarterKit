using System;
using Core.Commands;
using Core.Commands.NoteCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.NoteResponses;

namespace Core.Processors.NoteProcessors
{
    public class AddNoteProcessor : ICommandProcessor<AddNote>
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<Note> _noteRepository;

        public AddNoteProcessor(IRepository<Note> noteRepository, IRepository<Contact> contactRepository)
        {
            _noteRepository = noteRepository;
            _contactRepository = contactRepository;
        }

        public void Process(AddNote command, Guid userId, out IWebApiResponse response)
        {
            var user = _contactRepository.GetById(userId);

            var note = new Note
            {
                Id = command.Id,
                Name = "Note",
                EntityType = command.EntityType,
                ReferenceId = command.ReferenceId,
                ReferenceName = command.ReferenceName,
                Description = command.Description,
                ImageData = command.ImageData,
                CreatedBy = userId
            };
            _noteRepository.Add(note);

            response = new NoteResponse
            {
                Id = command.Id,
                CreatedOn = DateTime.Now,
                CreatedByName = user.Name,
                Description = command.Description,
                Name = note.Name
            };
        }
    }
}