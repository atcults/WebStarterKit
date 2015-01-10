using System;

namespace Dto.ApiResponses.AppUserResponses
{
    public class AppUserResponse : AuditedResponse
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int? FailedAttemptCount { get; set; }
        public Guid? ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string UserStatusName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
    }
}