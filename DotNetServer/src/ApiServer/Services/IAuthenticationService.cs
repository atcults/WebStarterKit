using Core.Views;

namespace WebApp.Services
{
	public interface IAuthenticationService
	{
		bool PasswordMatches(AppUserView user, string password);
	}
}