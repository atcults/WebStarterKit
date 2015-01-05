using System;
using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.ContactResponses;

namespace WebApp.ModelService
{
    public interface IContactModelService
    {
        PageResponse<ContactLine> GetPageBySpecification(SearchSpecification specification = null);
        ContactDetailResponse GetContactDetailById(Guid? id = null);
    }
}