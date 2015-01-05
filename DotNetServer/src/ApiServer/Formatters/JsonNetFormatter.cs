using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Dto.ApiResponses;
using Newtonsoft.Json;

namespace WebApp.Formatters
{
    public class JsonNetFormatter : MediaTypeFormatter
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        
        public JsonNetFormatter(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? new JsonSerializerSettings();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }
        
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            // Create a serializer
            var serializer = JsonSerializer.Create(_jsonSerializerSettings);

            // Create task reading the content
            return Task.Factory.StartNew(() =>
            {
                using (var streamReader = new StreamReader(readStream, Encoding.UTF8))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return serializer.Deserialize(jsonTextReader, type);
                    }
                }
            });
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }
        
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            // Create a serializer
            var serializer = JsonSerializer.Create(_jsonSerializerSettings);

            object response;

            //Force all response to derived from WebApiResonseBase
            if (value is IWebApiResponse)
            {
                response = value;
            }
            else
            {

                var error = new WebApiResponseBase();

                if (type == typeof (HttpError))
                {
                    var httpError = (HttpError) value;
                    error.AddError("HttpError", httpError.ExceptionMessage);
                }
                else
                {
                    error.AddError("ResponseType", string.Format("Type {0} should implement IWebApiResponse.", type));
                }
                response = error;
            }

            // Create task writing the serialized content
            return Task.Factory.StartNew(() =>
            {
                using (var streamWriter = new StreamWriter(writeStream, Encoding.UTF8))
                {
                    using (var jsonTextWriter = new JsonTextWriter(streamWriter))
                    {
                        serializer.Serialize(jsonTextWriter, response);
                    }
                }
            });
        }

    }
}