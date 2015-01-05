namespace Common.Enumerations
{
    public class EntityType : Enumeration<EntityType>
    {
        public EntityType(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static readonly EntityType AppUser = new EntityType("AU", "AppUser", 0);
        public static readonly EntityType Contact = new EntityType("CO", "Contact", 0);
        public static readonly EntityType Lead = new EntityType("LE", "Lead", 1);
        public static readonly EntityType TaskLog = new EntityType("TS", "TaskLog", 2);
        public static readonly EntityType Template = new EntityType("TE", "Template", 3);
    }
}