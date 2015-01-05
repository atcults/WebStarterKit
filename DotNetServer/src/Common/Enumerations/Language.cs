namespace Common.Enumerations
{
    public class Language : Enumeration<Language>
    {
        public Language(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static readonly Language English = new Language("EN", "English", 0);
        public static readonly Language Gujarati = new Language("GU", "Gujarati", 1);
        public static readonly Language Hindi = new Language("HI", "Hindi", 1);
    }
}