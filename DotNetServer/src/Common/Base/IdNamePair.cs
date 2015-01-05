using System;

namespace Common.Base
{
    //NOTE: Rename this to NameValuePair for enumerations
    public class IdNamePair
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class GuidNamePair
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class GuidValuePair
    {
        public Guid Id { get; set; }
        public object Value { get; set; }
    }

    public class NameBoolPair
    {
        public string Name { get; set; }
        public bool Value { get; set; }
    }
}