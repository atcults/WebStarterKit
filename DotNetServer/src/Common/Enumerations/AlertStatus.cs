namespace Common.Enumerations
{
    public class AlertStatus : Enumeration<AlertStatus>
    {
        public AlertStatus(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }
       
        public static AlertStatus Pending = new AlertStatus("PN", "Pending",0);
        public static AlertStatus Processing = new AlertStatus("PR", "Processing",1);
        public static AlertStatus Sent = new AlertStatus("ST", "Sent",2);
        public static AlertStatus Skipped =  new AlertStatus("SK", "Skipped",3);
    }
}