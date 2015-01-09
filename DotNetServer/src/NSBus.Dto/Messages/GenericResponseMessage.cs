using System;

namespace NSBus.Dto.Messages
{
    [Serializable]
    public class GenericResponseMessage
    {
        public bool IsSuccess { get; set; }
    }
}