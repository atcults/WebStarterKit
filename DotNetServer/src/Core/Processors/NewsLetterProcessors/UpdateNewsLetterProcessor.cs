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
    public class UpdateNewsLetterProcessor : ICommandProcessor<UpdateNewsLetter>
    {
        private readonly IRepository<NewsLetter> _newsLetterRepository;

        public UpdateNewsLetterProcessor(IRepository<NewsLetter> newsLetterRepository)
        {
            _newsLetterRepository = newsLetterRepository;
        }

        public void Process(UpdateNewsLetter command, Guid userId, out IWebApiResponse response)
        {
            EnsureSameEmail(command);

            var newsLetter = _newsLetterRepository.GetById(command.Id);

            newsLetter.Email = command.Email;
            newsLetter.IsActive = command.IsActive;

            _newsLetterRepository.Update(newsLetter);

            response = new NewsLetterResponse
            {
                Id = command.Id,
                Email = command.Email,
                IsActive = command.IsActive
            };
        }

        private void EnsureSameEmail(UpdateNewsLetter command)
        {
            var alradyAddedWithSameEmail =
                _newsLetterRepository.GetAllFor(Property.Of<Lead>(x => x.Email), command.Email).SingleOrDefault();

            if (alradyAddedWithSameEmail == null || alradyAddedWithSameEmail.Id == command.Id) return;
            throw new DomainProcessException(
                string.Format("Email should not be duplicated. Already available in system."));
        }
    }
}