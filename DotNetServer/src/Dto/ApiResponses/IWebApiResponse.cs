using System.Collections.Generic;
using Common.Base;

namespace Dto.ApiResponses
{
    public interface IWebApiResponse
    {
        string Uri { get; set; }
        bool RedirectRequired { get; set; }

        List<ValidationObject> ValidationObjects { get; set; }

        bool IsValid { get; }

        void AddError(string key, string value);
    }
}