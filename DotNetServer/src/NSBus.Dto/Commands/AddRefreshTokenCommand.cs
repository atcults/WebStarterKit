using System;

namespace NSBus.Dto.Commands
{
    [Serializable]
    public class AddRefreshTokenCommand
    {
        public string Name { get; set; }
        public string TokenHash { get; set; }
        public string ClientId { get; set; }
        public string TicketHash { get; set; }
        public DateTime? IssuedUtc { get; set; }
        public DateTime? ExpiresUtc { get; set; }
    }
}