using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudOcr.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecognizeController : ControllerBase
    {
        private readonly ILogger<RecognizeController> _logger;
        private readonly CognitiveOptions _options;

        public RecognizeController(ILogger<RecognizeController> logger, IOptions<CognitiveOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        [HttpPost]
        [Route("cognitive")]
        public async Task<string> RecognizeTextUsingCognitiveServices(Stream body)
        {
            var service = new Core.Cognitive.OcrService(_options.Endpoint, _options.Key);
            var text = await service.RecognizeTextAsync(body);
            return text;
        }

        [HttpPost]
        [Route("windows")]
        public async Task<string> RecognizeTextUsingWindows(Stream body)
        {
            var service = new Core.Windows.OcrService();
            var text = await service.RecognizeTextAsync(body);
            return text;
        }

        [HttpPost]
        [Route("tesseract")]
        public async Task<string> RecognizeTextUsingTesseract(Stream body)
        {
            var service = new Core.Tesseract.OcrService();
            var text = await service.RecognizeTextAsync(body);
            return text;
        }
    }
}
