namespace Common.Service
{
    public interface ISmsSender : IServiceCommon
    {
        bool SendShortMessage(string body, string mobile);
    }
}