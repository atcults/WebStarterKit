namespace Dto.ApiRequests.AppUserForms
{
    public class RegistrationForm : AuditedForm
    {
        public string Email { get; set; }
        public string Mobile { get; set; }

        public override string GetCommandValue()
        {
            return string.Format("{0} - {1} - {2}", base.ToString(), Email, Mobile);
        }

        public override string GetApiAddress()
        {
            return "MemberRegistration";
        }
    }
}