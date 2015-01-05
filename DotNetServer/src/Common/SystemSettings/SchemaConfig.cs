using System;
using System.Collections.Generic;

namespace Common.SystemSettings
{
    [Serializable]
    public class SchemaConfig
    {
        public SchemaConfig()
        {
            SchemaLines = new List<SchemaLine>();
        }

        public List<SchemaLine> SchemaLines { get; set; }
        public class SchemaLine
        {
            public int Version { get; set; }
            public DateTime AppliedOn { get; set; }
            public bool IsSuccess { get; set; }    
        }

        public bool IsDirty { get; set; }
    }
}