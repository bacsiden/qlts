using DK.Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DK.Application
{
    public interface ITaiSanService
    {
        Task ExportDataAsync(List<TaiSan> taiSans, TaiSanSearchModel search);
        Task ExportTaiSanAsync(List<TaiSan> taiSans, TaiSanSearchModel search);
        Task ExportReportDetailsAsync(List<TaiSan> taiSans, TaiSanSearchModel search);
        void ImportNewTaiSan(string userName, Stream stream);
        void ImportUpdateTaiSan(Stream stream);
        Task SendToBrowser(Stream OutStream, string MimeType, string FileName);

        Task ExportKiemKeAsync(List<KiemKe> kiemKes, string pattern, bool preview = false);
        void ImportKiemKe(Stream stream, Guid kiemKeId);
        Task ExportBarCodeAsync(List<TaiSan> taiSans);
        Dictionary<string, int> GetExistingCodes();

        void AddNewType(List<Models.Type> types, List<Models.Type> newTypes, string name, string value);
        void AddNewType(List<Models.Type> types, List<Models.Type> newTypes, string name, List<string> values);
    }
}