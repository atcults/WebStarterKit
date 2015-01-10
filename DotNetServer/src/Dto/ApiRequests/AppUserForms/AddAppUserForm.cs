using System;

namespace Dto.ApiRequests.AppUserForms
{
    public class AddAppUserForm : AuditedForm
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Guid? ProfileId { get; set; }
        public string UserStatusValue { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0} - {1}", ToString(), Name);
        }

        public override string GetApiAddress()
        {
            return "AppUser";
        }
    }
}