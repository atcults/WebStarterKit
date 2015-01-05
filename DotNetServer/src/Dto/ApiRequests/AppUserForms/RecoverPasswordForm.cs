namespace Dto.ApiRequests.AppUserForms
{
    public class RecoverPasswordForm : FormBase
    {
        public string UserNameOrEmailAddress { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0}-{1}", base.ToString(), UserNameOrEmailAddress);
        }

        public override string GetApiAddress()
        {
            return "/Auth/Recover";
        }
    }
}