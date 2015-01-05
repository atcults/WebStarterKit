using System;
using System.Collections.Generic;
using System.Linq;
using Common.Base;

namespace Dto.ApiResponses
{
    public class WebApiResponseBase : IWebApiResponse
    {
        public static object Lock = new object();

        public WebApiResponseBase()
        {
            ValidationObjects = new List<ValidationObject>();
        }

        public WebApiResponseBase(ValidationResult validationResult)
        {
            ValidationObjects = validationResult.ValidationObjects;
        }

        public List<ValidationObject> ValidationObjects { get; set; }

        public bool IsValid
        {
            get { return ValidationObjects.Count == 0; }
        }

        public string Uri { get; set; }

        public bool RedirectRequired { get; set; }

        public static WebApiResponseBase Create(string uri, bool redirectRequired)
        {
            return new WebApiResponseBase
            {
                Uri = uri,
                RedirectRequired = redirectRequired,
                ValidationObjects = new List<ValidationObject>()
            };
        }

        public void AddError(string key, string value)
        {
            lock (Lock)
            {
                var obj = ValidationObjects.FirstOrDefault(x => x.Key == key);
                if (obj == null)
                {
                    obj = new ValidationObject {Key = key};
                    ValidationObjects.Add(obj);
                }
                obj.Lines.Add(value);
            }
        }

        public override string ToString()
        {
            return ValidationObjects.Aggregate(string.Empty, (current, vo) =>
                current + string.Format("{0}:{1}{2}", vo.Key,
                    vo.Lines.Aggregate(string.Empty, (c, l) => c + l), Environment.NewLine));
        }
    }
}