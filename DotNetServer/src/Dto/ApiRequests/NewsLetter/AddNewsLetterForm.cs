namespace Dto.ApiRequests.NewsLetter
{
    public class AddNewsLetterForm : IWebApiRequest
    {
        public string Email { get; set; }

        public string GetCommandValue()
        {
            return string.Format("{0}-{1}", base.ToString(), Email);
        }

        public string GetApiAddress()
        {
            return "NewsLetter";
        }
    }
}