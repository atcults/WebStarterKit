using System;

namespace Dto.ApiResponses.AppUserResponses
{
    public class AppUserLine : AuditedLineResponse
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Guid? ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string UserStatusName { get; set; }
        public string UserStatusValue { get; set; }
    }
}