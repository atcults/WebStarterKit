namespace Dto.ApiRequests.ConfigForms
{
    public class SendMailForm : FormBase
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0}-{1} [{2}]", base.ToString(), Subject, Body);
        }

        public override string GetApiAddress()
        {
            return "/CoreConfiguration/Email";
        }
    }
}