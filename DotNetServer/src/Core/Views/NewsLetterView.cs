using System;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;

namespace Core.Views
{
    [ViewName("NewsLetterView")]
    public class NewsLetterView : View
    {
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InsertedDate { get; set; }
    }
}