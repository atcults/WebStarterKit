using System;
using Dto.ApiRequests;

namespace NSBus.Dto.Commands
{
    public class GenerateSimpleExportCommand
    {
        public Guid UserId { get; set; }
        public SimpleSearch SearchSpecification { get; set; }
        public string ViewType { get; set; }

        public override string ToString()
        {
            return string.Format("{0} with {1}", ViewType, SearchSpecification);
        }
    }
}
