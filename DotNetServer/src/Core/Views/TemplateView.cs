using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;

namespace Core.Views
{
    [ViewName("TemplateView")]
    public class TemplateView : View
    {
        public string Name { get; set; }
        public string MailBody { get; set; }
        public string SmsBody { get; set; }
        
    }
}