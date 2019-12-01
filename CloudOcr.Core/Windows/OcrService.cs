using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage.Streams;

using System.Linq;

namespace CloudOcr.Core.Windows
{
    // https://medium.com/@leesonLee/microsoft-ocr-uwp-behind-api-rest-553ac725f6fc
    // https://docs.microsoft.com/fr-fr/windows/apps/desktop/modernize/desktop-to-uwp-enhance
    // https://dev.to/azure/develop-azure-functions-using-net-core-3-0-gcm

    public class OcrService
    {
        private readonly OcrEngine _ocrEngine;

        public OcrService()
        {
            _ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
            if (_ocrEngine == null)
            {
                throw new Exception("Failed to create OcrEngine.");
            }
        }

        private async Task<SoftwareBitmap> GetPictureFromStreamAsync(byte[] image)
        {
            SoftwareBitmap picture = null;

            using (InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream())
            {
                await randomAccessStream.WriteAsync(image.AsBuffer());
                randomAccessStream.Seek(0);

                var decoder = await BitmapDecoder.CreateAsync(randomAccessStream);
                picture = await decoder.GetSoftwareBitmapAsync();
            }

            return picture;
        }

        public async Task<string> RecognizeTextAsync(Stream image)
        {
            var ms = new MemoryStream();
            await image.CopyToAsync(ms);

            var picture = await GetPictureFromStreamAsync(ms.GetBuffer());
            var resultOCR = await _ocrEngine.RecognizeAsync(picture);

            return resultOCR.Text;
        }
    }
}
