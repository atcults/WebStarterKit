using System;
using System.Linq;
using Common.Helpers;
using Core.Commands;
using Core.Commands.NewsLetterCommands;
using Core.Domain;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;
using Dto.ApiResponses.NewsLetter;

namespace Core.Processors.NewsLetterProcessors
{
    public class AddNewsLetterProcessor : ICommandProcessor<AddNewsLetter>
    {
        private readonly IRepository<NewsLetter> _newsLetterRepository;

        public AddNewsLetterProcessor(IRepository<NewsLetter> newsLetterRepository)
        {
            _newsLetterRepository = newsLetterRepository;
        }

        public void Process(AddNewsLetter command, Guid userId, out IWebApiResponse response)
        {
            EnsureSameEmail(command);

            var newsLetter = new NewsLetter
            {
                Id = command.Id,
                Email = command.Email,
                IsActive = true,
                InsertedDate = DateTime.Now
            };
            _newsLetterRepository.Add(newsLetter);

            response = new NewsLetterResponse
            {
                Id = command.Id,
                Email = command.Email,
                IsActive = newsLetter.IsActive,
                InsertedDate = newsLetter.InsertedDate.GetValueOrDefault()
            };
        }

        private void EnsureSameEmail(AddNewsLetter command)
        {
            var alradyAddedWithSameEmail =
                _newsLetterRepository.GetAllFor(Property.Of<Lead>(x => x.Email), command.Email).SingleOrDefault();

            if (alradyAddedWithSameEmail == null || alradyAddedWithSameEmail.Id == command.Id) return;
            throw new DomainProcessException(
                string.Format("Email should not be duplicated. Already available in system."));
        }
    }
}