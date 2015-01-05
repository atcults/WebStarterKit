using System;

namespace NSBus.Dto.Commands
{
    [Serializable]
    public class AddLeadCommand
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
