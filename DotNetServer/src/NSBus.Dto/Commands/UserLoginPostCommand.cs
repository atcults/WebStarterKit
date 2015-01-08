using System;

namespace NSBus.Dto.Commands
{
    [Serializable]
    public class UserGrantRemoveCommand
    {
        public Guid TokenId { get; set; }
    }
}