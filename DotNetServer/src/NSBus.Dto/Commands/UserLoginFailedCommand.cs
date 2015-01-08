using System;

namespace NSBus.Dto.Commands
{
    [Serializable]
    public class UserLoginFailedCommand
    {
        public Guid UserId { get; set; }
    }
}