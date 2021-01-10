using DK.Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DK.Application
{
    public interface ITaiSanService
    {
        Task ExportDataAsync(List<TaiSan> taiSans, string pattern, bool includeSub = false);
        Task ExportTaiSanAsync(List<TaiSan> taiSans, string pattern, bool includeSub = false);
        Task ExportReportDetailsAsync(List<TaiSan> taiSans, string pattern);
        void ImportTaiSan(string userName, Stream stream);
        Task SendToBrowser(Stream OutStream, string MimeType, string FileName);

        Task ExportKiemKeAsync(List<KiemKe> kiemKes, string pattern);
        void ImportKiemKe(Stream stream, Guid kiemKeId);
        Task ExportBarCodeAsync(List<TaiSan> taiSans);
    }
}