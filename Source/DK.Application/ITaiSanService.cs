using DK.Application.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DK.Application
{
    public interface ITaiSanService
    {
        Task ExportDataAsync(List<TaiSan> taiSans, string pattern);
        Task ExportReportAsync(List<TaiSan> taiSans, string pattern);
        void Import();
        Task SendToBrowser(Stream OutStream, string MimeType, string FileName);
    }
}