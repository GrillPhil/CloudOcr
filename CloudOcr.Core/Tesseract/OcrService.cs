using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace CloudOcr.Core.Tesseract
{
    // https://github.com/charlesw/tesseract
    // https://stackoverflow.com/questions/10947399/how-to-implement-and-do-ocr-in-a-c-sharp-project
    // https://github.com/tesseract-ocr/tessdata/blob/master/deu.traineddata

    public class OcrService : IOcrService
    {
        public async Task<string> RecognizeTextAsync(Stream image)
        {
            // There seems to be a bug in Tesseract Engine where the trained data 
            // always has to been in the folder ./tesstdata. 
            // This means having that folder also in the .Web project directory
            using (var engine = new TesseractEngine(@"./tessdata", "deu", EngineMode.Default))
            {
                var ms = new MemoryStream();
                await image.CopyToAsync(ms);

                using (var img = Pix.LoadTiffFromMemory(ms.GetBuffer()))
                {
                    using (var page = engine.Process(img))
                    {
                        return page.GetText();
                    }
                }
            }
        }
    }
}
