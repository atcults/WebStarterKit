using Core.Views;

namespace WebApp.Services
{
	public interface IUserSession
	{
		AppUserView GetCurrentUser();
	}
}