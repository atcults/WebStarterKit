namespace Dto.ApiRequests.AppUserForms
{
    public class ChangePasswordForm : FormBase
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0}-{1}", base.ToString(), OldPassword);
        }

        public override string GetApiAddress()
        {
            return "UserProfile";
        }
    }
}