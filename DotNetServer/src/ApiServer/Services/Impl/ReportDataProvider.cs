using System.Data;
using Dto.ApiRequests;
using WebApp.ModelService;

namespace WebApp.Services.Impl
{
    public class ReportDataProvider : IReportDataProvider
    {
        private readonly IAppUserModelService _appUserModelService;

        public ReportDataProvider(IAppUserModelService appUserModelService)
        {
            _appUserModelService = appUserModelService;
        }


        public DataTable GetAppUserDetails(SearchSpecification searchSpecification)
        {
            var data = new DataTable();

            data.Columns.Add("Name");
            data.Columns.Add("Email");
            data.Columns.Add("Mobile");

            var users = _appUserModelService.GetPageBySpecification(searchSpecification);
            searchSpecification.PageSize = 1000;
            searchSpecification.Page = 1;

            while (users.Items.Count > 0)
            {
                foreach (var member in users.Items)
                {
                    data.Rows.Add(member.Name, member.Email, member.Mobile);
                }
                searchSpecification.Page ++;
                users = _appUserModelService.GetPageBySpecification(searchSpecification);
            }

            return data;
        }      
    }
}