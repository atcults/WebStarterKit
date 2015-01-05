using Common.Helpers;
using Core.Domain.Model;
using Core.ReadWrite;
using NSBus.Dto.Commands;
using NServiceBus;

namespace NSBus.Server.CommandHandlers
{
    public class AddNewsLetterHandler : MessageHandler<AddNewsLetterCommand>
    {
        private readonly IRepository<NewsLetter> _newsLetterRepository;

        public AddNewsLetterHandler(IRepository<NewsLetter> newsLetterRepository, IUnitOfWork unitOfWork, IBus bus) : base(unitOfWork, bus)
        {
            _newsLetterRepository = newsLetterRepository;
        }

        public override void HandleMessage(AddNewsLetterCommand command)
        {
            var newsLetter = new NewsLetter
            {
                Id = GuidComb.New(),
                Email = command.Email,
                IsActive = true,
                InsertedDate = command.InsertedDate
            };
            _newsLetterRepository.Add(newsLetter);
        }
    }
}

