﻿using DK.Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DK.Application
{
    public interface ITaiSanService
    {
        Task ExportTaiSanAsync(List<TaiSan> taiSans, string pattern);
        Task ExportReportDetailsAsync(List<TaiSan> taiSans, string pattern);
        void ImportTaiSan(Stream stream);
        Task SendToBrowser(Stream OutStream, string MimeType, string FileName);

        Task ExportKiemKeAsync(List<KiemKe> kiemKes, string pattern);
        void ImportKiemKe(Stream stream, Guid kiemKeId);
    }
}