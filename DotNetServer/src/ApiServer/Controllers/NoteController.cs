using System;
using AutoMapper;
using Core.Commands.NoteCommands;
using Dto.ApiRequests.NoteForms;
using WebApp.Services;
using System.Net.Http;


namespace WebApp.Controllers
{
	public class NoteController : SmartApiController
	{
		public NoteController(IUserSession userSession, IMappingEngine mappingEngine) : base(userSession, mappingEngine)
		{
		}

		public HttpResponseMessage Post(AddNoteForm form)
		{
			var command = Mapper.Map<AddNoteForm, AddNote>(form);            
			return ExecuteCommand(command);
		}


		public HttpResponseMessage Put(UpdateNoteForm form)
		{
			var command = Mapper.Map<UpdateNoteForm, UpdateNote>(form);
			return ExecuteCommand(command);
		}


		public HttpResponseMessage Delete(Guid id)
		{
			var command = new DeleteNote(id);
			return ExecuteCommand(command);
		}
	}
}
