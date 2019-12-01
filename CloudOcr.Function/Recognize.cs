using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace CloudOcr.Function
{
    public static class Recognize
    {
        [FunctionName("RecognizeTextUsingCognitiveServices")]
        public static async Task<IActionResult> RecognizeTextUsingCognitiveServices(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "recognize/cognitive")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder().SetBasePath(context.FunctionAppDirectory)
                                                   .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                                                   .AddEnvironmentVariables()
                                                   .Build();

            var endpoint = config["csEndpoint"];
            var key = config["csKey"];
            var service = new Core.Cognitive.OcrService(endpoint, key);
            var text = await service.RecognizeTextAsync(req.Body);

            return new OkObjectResult(text);
        }

        [FunctionName("RecognizeTextUsingWindows")]
        public static async Task<IActionResult> RecognizeTextUsingWindows(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "recognize/windows")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var service = new Core.Windows.OcrService();
            var text = await service.RecognizeTextAsync(req.Body);

            return new OkObjectResult(text);
        }

        [FunctionName("RecognizeTextUsingTesseract")]
        public static async Task<IActionResult> RecognizeTextUsingTesseract(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "recognize/tesseract")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var service = new Core.Tesseract.OcrService();
            var text = await service.RecognizeTextAsync(req.Body);

            return new OkObjectResult(text);
        }
    }
}
