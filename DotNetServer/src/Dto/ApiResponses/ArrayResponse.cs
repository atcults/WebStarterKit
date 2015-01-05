using System;

namespace Dto.ApiResponses
{
    public class ArrayResponse<T> : WebApiResponseBase
    {
        public Guid Id;
        public T[] Items { get; set; }
    }
}