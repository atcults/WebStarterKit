using System;

namespace NSBus.Dto.Commands
{
    [Serializable]
    public class UserGrantAccessCommand
    {
        public Guid TokenId { get; set; }
        public string Client { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string AccessTokenHash { get; set; }
        public string ProtectedTicket { get; set; }
        public DateTime TimeUtc { get; set; }
    }
}