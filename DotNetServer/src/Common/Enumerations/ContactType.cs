namespace Common.Enumerations
{
    public class ContactType : Enumeration<ContactType>
    {
        public ContactType(string value, string displayName, int displayOrder)
            : base(value, displayName, displayOrder)
        {
        }

        public static readonly ContactType Participant = new ContactType("PC", "Participant", 0);
        public static readonly ContactType Navigator = new ContactType("NV", "Navigator", 1);
    }
}