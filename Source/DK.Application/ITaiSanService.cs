using DK.Application.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DK.Application
{
    public interface ITaiSanService
    {
        Task ExportAsync(List<TaiSan> taiSans);
        void Import();
        Task SendToBrowser(Stream OutStream, string MimeType, string FileName);
    }
}