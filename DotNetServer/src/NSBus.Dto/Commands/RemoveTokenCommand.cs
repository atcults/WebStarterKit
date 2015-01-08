using System;

namespace NSBus.Dto.Commands
{
    [Serializable]
    public class RemoveTokenCommand
    {
        public string TokenHash { get; set; }
    }
}