namespace Dto.ApiResponses.AppUserResponses
{
    public class AppUserLine : AuditedLineResponse
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string RoleName { get; set; }
        public string RoleValue { get; set; }
        public string UserStatusName { get; set; }
        public string UserStatusValue { get; set; }
    }
}