namespace Common.Enumerations
{
    public class UserStatus : Enumeration<UserStatus>
    {
        public UserStatus(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static readonly UserStatus Active = new UserStatus("A", "Active", 0);
        public static readonly UserStatus Disabled = new UserStatus("D", "Disabled", 1);
    }
}