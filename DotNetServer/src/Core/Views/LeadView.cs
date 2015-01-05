using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;

namespace Core.Views
{
    [ViewName("LeadView")]
    public class LeadView : AuditedView
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}