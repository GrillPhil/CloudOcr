using System.IO;
using System.Threading.Tasks;

namespace CloudOcr.Core
{
    public interface IOcrService
    {
        Task<string> RecognizeTextAsync(Stream tifImage);
    }
}
