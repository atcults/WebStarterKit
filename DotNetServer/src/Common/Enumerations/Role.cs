namespace Common.Enumerations
{
    public class Role : Enumeration<Role>
    {
        public Role(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static readonly Role Owner = new Role("OW", "Application Owner", 1);
        public static readonly Role Admin = new Role("AD", "Administrator", 2);
        public static readonly Role Guest = new Role("GU", "Guest User", 3);
        
    }
}