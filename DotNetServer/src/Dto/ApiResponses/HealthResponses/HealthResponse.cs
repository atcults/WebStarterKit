using System;

namespace Dto.ApiResponses.HealthResponses
{
    public class HealthResponse : WebApiResponseBase
    {
        public HealthResponse()
        {
            Value = new HealthLine[0];
        }

        public HealthLine[] Value { get; set; }

        public class HealthLine
        {
            public Guid Id { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
            public DateTime? RecordTime { get; set; }
        }
    }
}