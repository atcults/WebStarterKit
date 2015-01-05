using System;

namespace NSBus.Dto.Commands
{
    [Serializable]
    public class UserLoginPostCommand
    {
        public Guid UserId { get; set; }
        public bool IsAuthSuccess { get; set; }
        public DateTime LoginDateTime { get; set; }
    }
}