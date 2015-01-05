namespace Common.Enumerations
{
    public class AccountType : Enumeration<AccountType>
    {
        public AccountType(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static readonly AccountType Vendor = new AccountType("VE", "Vendor", 0);
        public static readonly AccountType Client = new AccountType("CL", "Client", 1);
        public static readonly AccountType Individual = new AccountType("IN", "Individual", 2);
    }
}