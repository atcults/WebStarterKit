using Core.Views;
using Dto.ApiRequests.AppUserForms;

namespace WebApp.Services
{
	public interface IUserSession
	{
		AppUserView GetCurrentUser();
	    AppUserView GetAnonymousUser();
	    bool Login(LoginForm request, ref string message);
	    void LogOff();
	}
}