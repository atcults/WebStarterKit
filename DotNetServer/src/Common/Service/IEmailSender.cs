namespace Common.Service
{
    public interface IEmailSender : IServiceCommon
    {
        bool SendTextEmail(string subject, string body, string toAddress = null);
    }
}