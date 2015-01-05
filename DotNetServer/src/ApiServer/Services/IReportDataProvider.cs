using System.Data;
using Dto.ApiRequests;

namespace WebApp.Services
{
	public interface IReportDataProvider
	{
        DataTable GetAppUserDetails(SearchSpecification specification);        
	}
}