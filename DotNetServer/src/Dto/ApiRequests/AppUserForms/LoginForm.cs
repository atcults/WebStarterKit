namespace Dto.ApiRequests.AppUserForms
{
    public class LoginForm : FormBase
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0}-{1} [{2}]", base.ToString(), Username, Password);
        }

        public override string GetApiAddress()
        {
            return "Auth";
        }
    }
}