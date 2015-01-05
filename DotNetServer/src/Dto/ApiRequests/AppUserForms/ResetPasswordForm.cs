namespace Dto.ApiRequests.AppUserForms
{
    public class ResetPasswordForm : FormBase
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0}-{1} [{2}]", base.ToString(), Token, NewPassword);
        }

        public override string GetApiAddress()
        {
            return "/Auth/ResetPassword";
        }
    }
}