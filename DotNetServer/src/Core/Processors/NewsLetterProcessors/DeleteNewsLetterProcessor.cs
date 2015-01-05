using System;
using Core.Commands;
using Core.Commands.NewsLetterCommands;
using Core.Domain.Model;
using Core.ReadWrite;
using Dto.ApiResponses;

namespace Core.Processors.NewsLetterProcessors
{
    public class DeleteNewsLetterProcessor : ICommandProcessor<DeleteNewsLetter>
    {
        private readonly IRepository<NewsLetter> _newsLetterRepository;

        public DeleteNewsLetterProcessor(IRepository<NewsLetter> newsLetterRepository)
        {
            _newsLetterRepository = newsLetterRepository;
        }

        public void Process(DeleteNewsLetter command, Guid userId, out IWebApiResponse response)
        {
            response = new WebApiResponseBase();
            var newsLetter = _newsLetterRepository.GetById(command.Id);
            _newsLetterRepository.Delete(newsLetter.Id);
        }
    }
}