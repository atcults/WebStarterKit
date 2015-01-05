namespace Common.Enumerations
{
    public class HealthType : Enumeration<HealthType>
    {
        public HealthType(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static readonly HealthType Mr = new HealthType("MM", "Memory", 1);
    }
}