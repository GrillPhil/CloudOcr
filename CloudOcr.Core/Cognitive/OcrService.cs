using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CloudOcr.Core.Cognitive
{
    public class OcrService : IOcrService
    {
        private readonly string _endpoint;
        private readonly string _key;

        public OcrService(string endpoint, string key)
        {
            _endpoint = endpoint;
            _key = key;
        }

        private async Task<OCRResponse> RecognizeAsync(Stream image)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);

            // Request parameters. 
            // The language parameter doesn't specify a language, so the 
            // method detects it automatically.
            // The detectOrientation parameter is set to true, so the method detects and
            // and corrects text orientation before detecting text.
            var requestParameters = "language=unk&detectOrientation=true";
            var uriBase = _endpoint + "vision/v2.1/ocr";
            var requestUrl = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Add the byte array as an octet stream to the request body.
            using (var content = new StreamContent(image))
            {
                // This example uses the "application/octet-stream" content type.
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(requestUrl, content);
            }

            var contentString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OCRResponse>(contentString);
        }

        public async Task<string> RecognizeTextAsync(Stream image)
        {
            var result = await RecognizeAsync(image);
            return result.ToString();
        }
    }
}
