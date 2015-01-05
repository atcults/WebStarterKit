
using System;

namespace NSBus.Dto.Commands
{
    public class AddNewsLetterCommand
    {
        public string Email { get; set; }
        public DateTime InsertedDate { get; set; }
    }
}
