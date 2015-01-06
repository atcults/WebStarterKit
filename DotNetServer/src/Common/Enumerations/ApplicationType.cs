namespace Common.Enumerations
{
    public class ApplicationType : Enumeration<ApplicationType>
    {
        public ApplicationType(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static ApplicationType JavaScript = new ApplicationType("JS", "JavaScript", 0);
        public static ApplicationType NativeConfidential = new ApplicationType("NC", "Native Confidential", 1);
    }
}