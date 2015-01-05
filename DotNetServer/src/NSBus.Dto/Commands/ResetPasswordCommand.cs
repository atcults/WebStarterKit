namespace NSBus.Dto.Commands
{
    public class ResetPasswordCommand
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }      
    }
}