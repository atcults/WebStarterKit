using System;
using Core.Commands;
using Core.Commands.NoteCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.NoteProcessors
{
    public class DeleteNoteProcessor : ICommandProcessor<DeleteNote>
    {
        private readonly IRepository<Note> _noteRepository;

        public DeleteNoteProcessor(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public void Process(DeleteNote command, Guid userId, out IWebApiResponse response)
        {
            response = new WebApiResponseBase();
            var note = _noteRepository.GetById(command.Id);
            _noteRepository.Delete(note.Id);
        }
    }
}