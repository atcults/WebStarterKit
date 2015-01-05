using System;

namespace Dto.ApiRequests.AppUserForms
{
    public class UpdateAppUserForm : AddAppUserForm
    {
        public Guid Id { get; set; }
    }
}