using Common.Helpers;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class AddLeadHandler : MessageHandler<AddLeadCommand>
    {
        private readonly IRepository<Lead> _leadRepository;
        private readonly IRepository<Contact> _contactRepository;

        public AddLeadHandler(IUnitOfWork unitOfWork, IRepository<Lead> leadRepository, IRepository<Contact> contactRepository, IBus bus) : base(unitOfWork, bus)
        {
            _leadRepository = leadRepository;
            _contactRepository = contactRepository;
        }

        public override void HandleMessage(AddLeadCommand command)
        {
            var serverhost = _contactRepository.GetByKey(Property.Of<Contact>(x => x.Name), "serverhost");

            var newsLetter = new Lead
            {
                Id = GuidComb.New(),
                Name = command.Name,
                Email = command.Email,
                Phone = command.Phone,
                CompanyName = command.CompanyName,
                Description = command.Description,
                CreatedBy = serverhost.Id,
                CreatedOn = command.CreatedOn
            };

            _leadRepository.Add(newsLetter);
        }
    }
}

