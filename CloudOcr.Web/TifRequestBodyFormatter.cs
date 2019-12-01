using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CloudOcr.Web
{
    public class TifRequestBodyFormatter : InputFormatter
    {
        private const string CONTENT_TYPE = "image/tiff";

        public TifRequestBodyFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
        }

        public override Boolean CanRead(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            return context.HttpContext.Request.ContentType == CONTENT_TYPE;
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var contentType = context.HttpContext.Request.ContentType;

            if (contentType == CONTENT_TYPE)
            {
                return await InputFormatterResult.SuccessAsync(request.Body);
            }

            return await InputFormatterResult.FailureAsync();
        }
    }
}
