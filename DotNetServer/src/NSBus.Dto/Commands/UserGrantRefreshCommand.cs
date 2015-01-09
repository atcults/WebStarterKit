using System;

namespace NSBus.Dto.Commands
{
    [Serializable]
    public class UserGrantRefreshCommand
    {
        public Guid TokenId { get; set; }
        public string Client { get; set; }
        public string RefreshTokenHash { get; set; }
        public string ProtectedTicket { get; set; }
        public DateTime TimeUtc { get; set; }
    }
}