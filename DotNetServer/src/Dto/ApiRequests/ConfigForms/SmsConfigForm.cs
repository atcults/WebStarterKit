namespace Dto.ApiRequests.ConfigForms
{
    public class SmsConfigForm : FormBase
    {
        public string ServiceUrl { get; set; }
        public string SenderName { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0}-{1} [{2}]", base.ToString(), ServiceUrl, SenderName);
        }

        public override string GetApiAddress()
        {
            return "/CoreConfiguration/Sms";
        }
    }
}