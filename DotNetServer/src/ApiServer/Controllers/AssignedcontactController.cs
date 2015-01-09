using System;
using AutoMapper;
using Core.Commands.ContactCommands;
using Dto.ApiRequests.ContactForms;
using System.Net.Http;


namespace WebApp.Controllers
{
	public class AssignedContactController : SmartApiController
	{
		public HttpResponseMessage Post(AddAssignedContactForm form)
		{
			var command = Mapper.Map<AddAssignedContactForm, AddAssignedContact>(form);
			return ExecuteCommand(command);
		}

		public HttpResponseMessage Put(UpdateAssignedContactForm form)
		{
			var command = Mapper.Map<UpdateAssignedContactForm, UpdateAssignedContact>(form);
			return ExecuteCommand(command);
		}

		public HttpResponseMessage Delete(Guid id)
		{
			var command = new DeleteAssignedContact(id);
			return ExecuteCommand(command);
		}
	}
}
