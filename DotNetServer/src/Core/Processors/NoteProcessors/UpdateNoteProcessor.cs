using System;
using Core.Commands;
using Core.Commands.NoteCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.NoteProcessors
{
    public class UpdateNoteProcessor : ICommandProcessor<UpdateNote>
    {
        private readonly IRepository<Note> _noteRepository;

        public UpdateNoteProcessor(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public void Process(UpdateNote command, Guid userId, out IWebApiResponse response)
        {
            response = new WebApiResponseBase();

            var note = _noteRepository.GetById(command.Id);
            note.Description = command.Description;
            note.Name = command.Name;
            note.ImageData = command.ImageData;
            note.ReferenceId = command.ReferenceId;
            note.ReferenceName = command.ReferenceName;
            note.EntityType = command.EntityType;
            note.ModifiedBy = userId;

            _noteRepository.Update(note);
        }
    }
}