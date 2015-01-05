namespace Dto.ApiRequests.TemplateForms
{
    public class AddTemplateForm : FormBase
    {
        public string Name { get; set; }
        public string MailBody { get; set; }
        public string SmsBody { get; set; }


        public override string GetCommandValue()
        {
            return Name;
        }

        public override string GetApiAddress()
        {
            return "Template";
        }
    }
}