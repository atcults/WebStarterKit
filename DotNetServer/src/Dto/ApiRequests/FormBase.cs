namespace Dto.ApiRequests
{
    public abstract class FormBase : IWebApiRequest
    {
        public abstract string GetCommandValue();
        public abstract string GetApiAddress();
    }
}