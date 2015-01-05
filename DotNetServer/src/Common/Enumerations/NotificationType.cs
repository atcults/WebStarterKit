namespace Common.Enumerations
{
    public class NotificationType : Enumeration<NotificationType>
    {
        public NotificationType(string value, string displayName, int displayOrder) : base(value, displayName, displayOrder)
        {
        }

        public static readonly NotificationType UserRegistered = new NotificationType("RG", "User Registered", 0);
        public static readonly NotificationType ChangePassword = new NotificationType("CP", "Change Password", 1);
        public static readonly NotificationType ResetPasswordRequest = new NotificationType("RR", "Retrieve Password Request", 2);
        public static readonly NotificationType ResetPasswordSuccess = new NotificationType("RP", "Retrieve Password Success", 3);
        public static readonly NotificationType UserLoginSuccess = new NotificationType("LS", "User Login Success", 4);
        public static readonly NotificationType UserLoginFailed = new NotificationType("LF", "User Login Failed", 5);
        public static readonly NotificationType TaskCompleted = new NotificationType("TC", "Task Completed", 6);
        public static readonly NotificationType TaskFailed = new NotificationType("TF", "Task Failed", 7);
        public static readonly NotificationType ApplicationError = new NotificationType("AE", "Application Error", 8);
    }
}