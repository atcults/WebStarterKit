using System;
using Dto.ApiRequests;
using Dto.ApiResponses;
using Dto.ApiResponses.AttachmentResponses;

namespace WebApp.ModelService
{
    public interface IAttachmentModelService
    {
        PageResponse<AttachmentLine> GetPageBySpecification(SearchSpecification specification = null);
        AttachmentResponse  GetAttachmentById(Guid? id = null);
        AttachmentSearchResponse GetAttachmentsBySearchValue(string value);
    }
}