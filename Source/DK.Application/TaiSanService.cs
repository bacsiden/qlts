using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DK.Application.Models;
using DK.Application.Repositories;
using FlexCel.Core;
using FlexCel.Render;
using FlexCel.Report;
using FlexCel.XlsAdapter;

namespace DK.Application
{
    public class TaiSanService : ITaiSanService
    {
        protected readonly string TemplateFolder = HttpContext.Current.Server.MapPath("~/ReportTemplates\\");
        private readonly ITaiSanRepository _taiSanRepository;
        private readonly ITypeRepository _typeRepository;
        private readonly IKiemKeRepository _kiemKeRepository;
        public TaiSanService(ITaiSanRepository taiSanRepository, ITypeRepository typeRepository, IKiemKeRepository kiemKeRepository)
        {
            _taiSanRepository = taiSanRepository;
            _typeRepository = typeRepository;
            _kiemKeRepository = kiemKeRepository;
        }

        public void ImportTaiSan(Stream stream)
        {
            var taisans = _taiSanRepository.Find(m => true).ToList().Select(m => new { key = m.Code, value = m }).ToDictionary(x => x.key, x => x.value);
            var types = _typeRepository.Find(m => true).ToList();
            var newTypes = new List<Models.Type>();

            XlsFile xls = new XlsFile();
            xls.Open(stream);
            var newTaiSans = new List<TaiSan>();
            var oldTaiSans = new List<TaiSan>();

            xls.ActiveSheet = 1;  //we'll read sheet1. We could loop over the existing sheets by using xls.SheetCount and xls.ActiveSheet 
            for (int row = 3; row <= xls.RowCount; row++)
            {
                var ts = new TaiSan();
                ts.Code = GetCellString(xls, row, nameof(TaiSan.Code));
                ts.Name = GetCellString(xls, row, nameof(TaiSan.Name));
                ts.GroupCode = GetCellString(xls, row, nameof(TaiSan.GroupCode));
                ts.GroupName = GetCellString(xls, row, nameof(TaiSan.GroupName));
                ts.ChungLoai = GetCellString(xls, row, nameof(TaiSan.ChungLoai));
                AddNewType(types, newTypes, TypeConstant.ChungLoai, ts.ChungLoai);

                ts.DanhMuc = GetCellString(xls, row, nameof(TaiSan.DanhMuc));
                AddNewType(types, newTypes, TypeConstant.DanhMuc, ts.DanhMuc);

                ts.NhanHieu = GetCellString(xls, row, nameof(TaiSan.NhanHieu));
                ts.Serial = GetCellString(xls, row, nameof(TaiSan.Serial));
                ts.XuatXu = GetCellString(xls, row, nameof(TaiSan.XuatXu));
                ts.ThuocHopDong = GetCellString(xls, row, nameof(TaiSan.ThuocHopDong));
                ts.ThuocGoiThau = GetCellString(xls, row, nameof(TaiSan.ThuocGoiThau));
                ts.NguonKinhPhi = GetCellString(xls, row, nameof(TaiSan.NguonKinhPhi));
                AddNewType(types, newTypes, TypeConstant.NguonKinhPhi, ts.NguonKinhPhi);

                ts.NganSachNam = GetCellInt(xls, row, nameof(TaiSan.NganSachNam));
                ts.NamSanXuat = GetCellInt(xls, row, nameof(TaiSan.NamSanXuat));
                ts.NamSuDung = GetCellInt(xls, row, nameof(TaiSan.NamSuDung));
                ts.NguyenGiaKeToan = GetCellDecimal(xls, row, nameof(TaiSan.NguyenGiaKeToan));
                ts.SoLuong = GetCellInt(xls, row, nameof(TaiSan.SoLuong));
                ts.KhoiLuong = GetCellInt(xls, row, nameof(TaiSan.KhoiLuong));
                ts.ChatLuong = GetCellString(xls, row, nameof(TaiSan.ChatLuong));
                AddNewType(types, newTypes, TypeConstant.ChatLuong, ts.ChatLuong);

                ts.HaoMonLuyKe = GetCellInt(xls, row, nameof(TaiSan.HaoMonLuyKe));
                ts.GiaTriConLai = GetCellDecimal(xls, row, nameof(TaiSan.GiaTriConLai));
                ts.NguoiSuDung = GetCellString(xls, row, nameof(TaiSan.NguoiSuDung));
                ts.NguoiQuanLy = GetCellString(xls, row, nameof(TaiSan.NguoiQuanLy));
                ts.PhongQuanLy = GetCellString(xls, row, nameof(TaiSan.PhongQuanLy));
                AddNewType(types, newTypes, TypeConstant.PhongBan, ts.PhongQuanLy);

                ts.DungTichXiLanh = GetCellInt(xls, row, nameof(TaiSan.DungTichXiLanh));
                ts.SoChoNgoi = GetCellInt(xls, row, nameof(TaiSan.SoChoNgoi));
                ts.SoTang = GetCellInt(xls, row, nameof(TaiSan.SoTang));
                ts.DienTichXayDung = GetCellInt(xls, row, nameof(TaiSan.DienTichXayDung));
                ts.CapCongTrinh = GetCellInt(xls, row, nameof(TaiSan.CapCongTrinh));
                ts.DiaChi = GetCellString(xls, row, nameof(TaiSan.DiaChi));
                ts.DienTichKhuonVien = GetCellInt(xls, row, nameof(TaiSan.DienTichKhuonVien));
                ts.Tags = GetCellListString(xls, row, nameof(TaiSan.Tags));
                AddNewType(types, newTypes, TypeConstant.Tags, ts.Tags);

                if (ts.Code == null)
                {
                    var i = 1;
                    ts.GenerateCode();
                    while (taisans.ContainsKey(ts.Code))
                    {
                        ts.GenerateCode(i++);
                    }

                    newTaiSans.Add(ts);
                    taisans.Add(ts.Code, ts);
                }
                else
                {
                    if (taisans.ContainsKey(ts.Code))
                    {
                        var taisan = taisans[ts.Code];
                        if (taisan.Id != Guid.Empty)
                        {
                            ts.Id = taisan.Id;
                            oldTaiSans.Add(ts);
                        }
                        else throw new Exception($"Dữ liệu bị lỗi tại dòng {row}. Vui lòng không sửa mã tài sản khi import dữ liệu từ file excel");
                    }
                    else if (ts.IsVehicle())
                    {
                        newTaiSans.Add(ts);
                        taisans.Add(ts.Code, ts);
                    }
                    else
                        throw new Exception($"Dữ liệu bị lỗi tại dòng {row}. Vui lòng không nhập mã tài sản khi import dữ liệu từ file excel");
                }
            }
            if (newTaiSans.Any())
                _taiSanRepository.AddRange(newTaiSans);
            foreach (var item in oldTaiSans)
            {
                _taiSanRepository.Update(item);
            }
            if (newTypes.Any())
                _typeRepository.AddRange(newTypes);
        }

        public Task ExportDataAsync(List<TaiSan> taiSans, string pattern)
        {
            var template = ReportVariables.Templates[pattern];
            if (pattern == "data")
                return ExportTaiSanAsync(taiSans, pattern);
            else if (template.Item1.Contains("chi tiết"))
                return ExportReportDetailsAsync(taiSans, pattern);
            else if (template.Item1.Contains("tổng hợp"))
                return ExportReportGroupAsync(taiSans, pattern);
            return Task.CompletedTask;
        }

        public Task ExportTaiSanAsync(List<TaiSan> taiSans, string pattern)
        {
            var template = ReportVariables.Templates[pattern];
            if (pattern.StartsWith("2"))
            {
                ExportReportDetailsAsync(taiSans, pattern);
                return Task.CompletedTask;
            }
            using (FlexCelReport fr = new FlexCelReport(true))
            {
                foreach (var item in taiSans)
                {
                    item.JoinedTags = string.Join("; ", item.Tags);
                }

                fr.AddTable("row", taiSans);
                var xlsx = new XlsFile(true);
                xlsx.Open(TemplateFolder + template.Item2);
                fr.Run(xlsx);
                using (FlexCelPdfExport pdf = new FlexCelPdfExport(xlsx, true))
                {
                    pdf.Export($"{TemplateFolder}result.pdf");
                }
                using (MemoryStream XlsStream = new MemoryStream())
                {
                    xlsx.Save(XlsStream);
                    return SendToBrowser(XlsStream, "application/excel", GetReportName(pattern));
                }
            }
        }

        public Task ExportReportDetailsAsync(List<TaiSan> taiSans, string pattern)
        {
            var template = ReportVariables.Templates[pattern];
            using (FlexCelReport fr = new FlexCelReport(true))
            {
                var no = 1;
                foreach (var item in taiSans)
                {
                    item.JoinedTags = string.Join("; ", item.Tags);

                }

                fr.AddTable("row", taiSans);
                fr.AddTable("sum", new List<TaiSan> { BuildSumTaiSan(taiSans) });
                fr.SetValue("title", template.Item1.ToUpperInvariant());
                fr.SetValue("pattern", pattern);
                var xlsx = new XlsFile(true);
                xlsx.Open(TemplateFolder + template.Item2);
                fr.Run(xlsx);
                using (MemoryStream XlsStream = new MemoryStream())
                {
                    xlsx.Save(XlsStream);
                    return SendToBrowser(XlsStream, "application/excel", GetReportName(pattern));
                }
            }
        }

        public Task ExportReportGroupAsync(List<TaiSan> taiSans, string pattern)
        {
            var template = ReportVariables.Templates[pattern];
            var groups = taiSans.Where(m => !string.IsNullOrWhiteSpace(m.GroupName)).Select(m => m.GroupName).Distinct();
            var reportData = taiSans.Where(m => string.IsNullOrWhiteSpace(m.GroupName)).ToList();
            foreach (var item in groups)
            {
                var groupTaiSans = taiSans.Where(m => m.GroupName == item).ToList();
                var sum = BuildSumTaiSan(groupTaiSans);
                sum.GroupName = item;
                reportData.Add(sum);
            }
            var sumAll = BuildSumTaiSan(reportData);

            var no = 1;
            foreach (var item in reportData)
            {
                item.No = no++;
            }

            using (FlexCelReport fr = new FlexCelReport(true))
            {
                fr.AddTable("row", reportData);
                fr.AddTable("sum", new List<TaiSan> { sumAll });
                fr.SetValue("title", template.Item1.ToUpperInvariant());
                fr.SetValue("pattern", pattern);
                var xlsx = new XlsFile(true);
                xlsx.Open(TemplateFolder + template.Item2);
                fr.Run(xlsx);
                using (MemoryStream XlsStream = new MemoryStream())
                {
                    xlsx.Save(XlsStream);
                    return SendToBrowser(XlsStream, "application/excel", GetReportName(pattern));
                }
            }
        }

        private string GetReportName(string pattern)
        {
            var template = ReportVariables.Templates[pattern];
            return $"{pattern}.xlsx";
        }

        private TaiSan BuildSumTaiSan(List<TaiSan> taiSans)
        {
            var sum = new TaiSan();
            sum.SoLuong = taiSans.Where(m => m.SoLuong.HasValue).Sum(m => m.SoLuong);
            sum.KhoiLuong = taiSans.Where(m => m.KhoiLuong.HasValue).Sum(m => m.KhoiLuong);
            sum.NguyenGiaKeToan = taiSans.Where(m => m.NguyenGiaKeToan.HasValue).Sum(m => m.NguyenGiaKeToan);
            sum.HaoMonLuyKe = taiSans.Where(m => m.HaoMonLuyKe.HasValue).Sum(m => m.HaoMonLuyKe);
            sum.GiaTriConLai = taiSans.Where(m => m.GiaTriConLai.HasValue).Sum(m => m.GiaTriConLai);
            sum.DienTichXayDung = taiSans.Where(m => m.DienTichXayDung.HasValue).Sum(m => m.DienTichXayDung);
            sum.DienTichKhuonVien = taiSans.Where(m => m.DienTichKhuonVien.HasValue).Sum(m => m.DienTichKhuonVien);

            sum.NganSachBo = taiSans.Where(m => m.NganSachBo.HasValue).Sum(m => m.NganSachBo);
            sum.Khac = taiSans.Where(m => m.Khac.HasValue).Sum(m => m.Khac);
            sum.TongCong = sum.NganSachBo + sum.Khac;

            return sum;
        }

        public Task ExportKiemKeAsync(List<KiemKe> kiemKes, string pattern)
        {
            var template = ReportVariables.Templates[pattern];
            using (FlexCelReport fr = new FlexCelReport(true))
            {
                var no = 1;
                foreach (var item in kiemKes)
                {
                    item.No = no++;
                    item.SoLuongChenhLech = KiemKe.GetChenhLech(item.SoLuongKeToan, item.SoLuongKiemKe);
                    item.NguyenGieChenhLech = KiemKe.GetChenhLech(item.NguyenGiaKeToan, item.NguyenGiaKiemKe);
                    item.GiaTriConLaiChenhLech = KiemKe.GetChenhLech(item.GiaTriConLaiKeToan, item.GiaTriConLaiKiemKe);
                }

                fr.AddTable("row", kiemKes);
                var sum = new KiemKe();
                sum.SoLuongKeToan = kiemKes.Where(m => m.SoLuongKeToan.HasValue).Sum(m => m.SoLuongKeToan);
                sum.SoLuongKiemKe = kiemKes.Where(m => m.SoLuongKiemKe.HasValue).Sum(m => m.SoLuongKiemKe);
                sum.SoLuongChenhLech = KiemKe.GetChenhLech(sum.SoLuongKeToan, sum.SoLuongKiemKe);

                sum.NguyenGiaKeToan = kiemKes.Where(m => m.NguyenGiaKeToan.HasValue).Sum(m => m.NguyenGiaKeToan);
                sum.NguyenGiaKiemKe = kiemKes.Where(m => m.NguyenGiaKiemKe.HasValue).Sum(m => m.NguyenGiaKiemKe);
                sum.NguyenGieChenhLech = KiemKe.GetChenhLech(sum.NguyenGiaKeToan, sum.NguyenGiaKiemKe);

                sum.GiaTriConLaiKeToan = kiemKes.Where(m => m.GiaTriConLaiKeToan.HasValue).Sum(m => m.GiaTriConLaiKeToan);
                sum.GiaTriConLaiKiemKe = kiemKes.Where(m => m.GiaTriConLaiKiemKe.HasValue).Sum(m => m.GiaTriConLaiKiemKe);
                sum.GiaTriConLaiChenhLech = KiemKe.GetChenhLech(sum.GiaTriConLaiKeToan, sum.GiaTriConLaiKiemKe);

                fr.AddTable("sum", new List<KiemKe> { sum });
                fr.SetValue("title", template.Item1.ToUpperInvariant());
                fr.SetValue("pattern", pattern);
                var xlsx = new XlsFile(true);
                xlsx.Open(TemplateFolder + template.Item2);
                fr.Run(xlsx);
                using (MemoryStream XlsStream = new MemoryStream())
                {
                    xlsx.Save(XlsStream);
                    return SendToBrowser(XlsStream, "application/excel", $"{pattern}.xlsx");
                }
            }
        }

        public void ImportKiemKe(Stream stream, Guid kiemKeId)
        {
            var taisans = _taiSanRepository.Find(m => true).ToList().Select(m => new { key = m.Code, value = m }).ToDictionary(x => x.key, x => x.value);

            XlsFile xls = new XlsFile();
            xls.Open(stream);

            var newKiemKes = new List<KiemKe>();
            xls.ActiveSheet = 1;
            for (int row = 9; row <= xls.RowCount; row++)
            {
                var firstCell = GetCellInt(xls, row, nameof(KiemKe.No));
                if (firstCell == null || firstCell.Value == 0) break;

                var kk = new KiemKe();
                kk.Code = GetCellString(xls, row, nameof(KiemKe.Code));

                if (!taisans.ContainsKey(kk.Code))
                {
                    throw new Exception($"Mã tài sản '{kk.Code}' không tồn tại trong danh mục tài sản");
                }
                kk.KiemKeId = kiemKeId;
                kk.Name = GetCellString(xls, row, nameof(KiemKe.Name));
                kk.SoLuongKeToan = GetCellInt(xls, row, nameof(KiemKe.SoLuongKeToan));
                kk.SoLuongKiemKe = GetCellInt(xls, row, nameof(KiemKe.SoLuongKiemKe));
                kk.NguyenGiaKeToan = GetCellDecimal(xls, row, nameof(KiemKe.NguyenGiaKeToan));
                kk.NguyenGiaKiemKe = GetCellDecimal(xls, row, nameof(KiemKe.NguyenGiaKiemKe));
                kk.GiaTriConLaiKeToan = GetCellDecimal(xls, row, nameof(KiemKe.GiaTriConLaiKeToan));
                kk.GiaTriConLaiKiemKe = GetCellDecimal(xls, row, nameof(KiemKe.GiaTriConLaiKiemKe));
                kk.GhiChu = GetCellString(xls, row, nameof(KiemKe.GhiChu));

                newKiemKes.Add(kk);
            }
            if (newKiemKes.Any())
            {
                _kiemKeRepository.DeleteManyAsync(nameof(KiemKe.KiemKeId), kiemKeId).GetAwaiter().GetResult();
                _kiemKeRepository.AddRange(newKiemKes);
            }
        }


        private void AddNewType(List<Models.Type> types, List<Models.Type> newTypes, string name, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !types.Any(m => m.Name == name && value.Equals(m.Title, StringComparison.OrdinalIgnoreCase)))
                newTypes.Add(new Models.Type { Name = name, Title = value });
        }
        private void AddNewType(List<Models.Type> types, List<Models.Type> newTypes, string name, List<string> values)
        {
            values.ForEach(x =>
            {
                if (!string.IsNullOrWhiteSpace(x) && !types.Any(m => m.Name == name && x.Equals(m.Title, StringComparison.OrdinalIgnoreCase)))
                    newTypes.Add(new Models.Type { Name = name, Title = x });
            });
        }
        private string GetCellString(XlsFile xls, int row, string fieldName)
        {
            var colIndex = TaiSan.GetCol(fieldName);
            int XF = -1;
            object cell = xls.GetCellValueIndexed(row, colIndex, ref XF);
            var value = cell?.ToString()?.Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
        private List<string> GetCellListString(XlsFile xls, int row, string fieldName)
        {
            var colIndex = TaiSan.GetCol(fieldName);
            int XF = -1;
            object cell = xls.GetCellValueIndexed(row, colIndex, ref XF);
            var value = cell?.ToString()?.Trim();
            return string.IsNullOrEmpty(value) ? new List<string>() : value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
        }
        private decimal? GetCellDecimal(XlsFile xls, int row, string fieldName)
        {
            var colIndex = TaiSan.GetCol(fieldName);
            int XF = -1;
            object cell = xls.GetCellValueIndexed(row, colIndex, ref XF);
            if (decimal.TryParse(cell + "", out decimal result))
                return result;
            return null;
        }
        private int? GetCellInt(XlsFile xls, int row, string fieldName)
        {
            var colIndex = TaiSan.GetCol(fieldName);
            int XF = -1;
            object cell = xls.GetCellValueIndexed(row, colIndex, ref XF);
            if (int.TryParse(cell + "", out int result))
                return result;
            return null;
        }
        public async Task SendToBrowser(Stream OutStream, string MimeType, string FileName)
        {
            var ms = new MemoryStream();
            if (!(OutStream is MemoryStream))
            {
                await OutStream.CopyToAsync(ms);
            }
            else
                ms = OutStream as MemoryStream;
            var response = HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName);
            byte[] MemData = ms.ToArray();
            response.AddHeader("Content-Length", Convert.ToString(MemData.Length, CultureInfo.InvariantCulture));
            response.ContentType = MimeType;
            response.BinaryWrite(MemData);
            response.End();
        }
    }
}
