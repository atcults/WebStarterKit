using System;
using Core.Commands;
using Core.Commands.ContactCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.AssignedContactProcessors
{
    public class DeleteAssignedContactProcessor : ICommandProcessor<DeleteAssignedContact>
    {
        private readonly IRepository<AssignedContact> _assignedcontactRepository;

        public DeleteAssignedContactProcessor(IRepository<AssignedContact> assignedcontactRepository)
        {
            _assignedcontactRepository = assignedcontactRepository;
        }

        public void Process(DeleteAssignedContact command, Guid userId, out IWebApiResponse response)
        {
            var assignedcontact = _assignedcontactRepository.GetById(command.Id);

            _assignedcontactRepository.Delete(assignedcontact.Id);

            response = new WebApiResponseBase();
        }
    }
}