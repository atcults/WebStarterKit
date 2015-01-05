namespace Common.Enumerations
{
    public class Gender : Enumeration<Gender>
    {
        public Gender(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static readonly Gender Male = new Gender("M", "Male", 1);
        public static readonly Gender Female = new Gender("F", "Female", 2);
    }
}