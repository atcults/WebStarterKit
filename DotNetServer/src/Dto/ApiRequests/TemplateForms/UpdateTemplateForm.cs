using System;

namespace Dto.ApiRequests.TemplateForms
{
    public class UpdateTemplateForm : AddTemplateForm
    {
        public Guid Id { get; set; }
    }
}