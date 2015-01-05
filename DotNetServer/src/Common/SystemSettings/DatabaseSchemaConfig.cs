using System;
using System.Collections.Generic;

namespace Common.SystemSettings
{
    [Serializable]
    public class DatabaseSchemaConfig
    {
        public DatabaseSchemaConfig()
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