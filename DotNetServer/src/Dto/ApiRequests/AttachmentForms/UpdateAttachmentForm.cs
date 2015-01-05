using System;

namespace Dto.ApiRequests.AttachmentForms
{
    public class UpdateAttachmentForm : FormBase
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }

        public override string GetCommandValue()
        {
            return Id.ToString();
        }

        public override string GetApiAddress()
        {
            return "Attachment";
        }
    }
}