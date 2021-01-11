using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using bpac;
using DK.Application.Models;
using DK.Application.Repositories;
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

        public void ImportTaiSan(string userName, Stream stream)
        {
            var taisans = GetExistingCodes();
            var types = _typeRepository.Find(m => true).ToList();
            var newTypes = new List<Models.Type>();

            XlsFile xls = new XlsFile();
            xls.Open(stream);
            var newTaiSans = new List<TaiSan>();
            var cluster = new List<TaiSan>();
            var oldTaiSans = new List<TaiSan>();
            int lastNo = -1;

            xls.ActiveSheet = 1;  //we'll read sheet1. We could loop over the existing sheets by using xls.SheetCount and xls.ActiveSheet 
            for (int row = 3; row <= xls.RowCount; row++)
            {
                var ts = new TaiSan() { CreatedBy = userName };
                var no = GetCellInt(xls, row, nameof(TaiSan.No));
                if (!no.HasValue) throw new Exception($"Lỗi dữ liệu tại dòng {row}. Cột STT phải có dữ liệu");
                ts.No = no.Value;

                ts.Code = GetCellString(xls, row, nameof(TaiSan.Code));
                if (!string.IsNullOrWhiteSpace(ts.Code)) throw new Exception($"Lỗi dữ liệu tại dòng {row}. Không được nhập mã tài sản. Mã tài sản sẽ được tự sinh");

                ts.Name = GetCellString(xls, row, nameof(TaiSan.Name));
                ts.GroupName = GetCellString(xls, row, nameof(TaiSan.GroupName));

                if (!TypeConstant.Groups.Any(m => m.Equals(ts.GroupName, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception($"Lỗi dữ liệu tại dòng {row}. Tên nhóm tài sản phải là: {TypeConstant.GDacBiet}, {TypeConstant.GChuyenDung}, {TypeConstant.GQuanLy}");
                }

                ts.ChungLoai = GetCellString(xls, row, nameof(TaiSan.ChungLoai));
                AddNewType(types, newTypes, TypeConstant.ChungLoai, ts.ChungLoai);
                ts.NhanHieu = GetCellString(xls, row, nameof(TaiSan.NhanHieu));
                ts.Serial = GetCellString(xls, row, nameof(TaiSan.Serial));
                ts.XuatXu = GetCellString(xls, row, nameof(TaiSan.XuatXu));
                ts.ThuocHopDong = GetCellString(xls, row, nameof(TaiSan.ThuocHopDong));
                ts.ThuocGoiThau = GetCellString(xls, row, nameof(TaiSan.ThuocGoiThau));
                ts.NguonKinhPhi = GetCellString(xls, row, nameof(TaiSan.NguonKinhPhi));
                AddNewType(types, newTypes, TypeConstant.NguonKinhPhi, ts.NguonKinhPhi);
                ts.NganSachKhac = GetCellString(xls, row, nameof(TaiSan.NganSachKhac));

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

                ts.LoaiXe = GetCellString(xls, row, nameof(TaiSan.LoaiXe));
                ts.BienSo = GetCellString(xls, row, nameof(TaiSan.BienSo));
                ts.DungTichXiLanh = GetCellInt(xls, row, nameof(TaiSan.DungTichXiLanh));
                ts.SoChoNgoi = GetCellInt(xls, row, nameof(TaiSan.SoChoNgoi));
                ts.SoTang = GetCellInt(xls, row, nameof(TaiSan.SoTang));
                ts.DienTichXayDung = GetCellInt(xls, row, nameof(TaiSan.DienTichXayDung));
                ts.CapCongTrinh = GetCellString(xls, row, nameof(TaiSan.CapCongTrinh));
                ts.DiaChi = GetCellString(xls, row, nameof(TaiSan.DiaChi));
                ts.DienTichKhuonVien = GetCellInt(xls, row, nameof(TaiSan.DienTichKhuonVien));
                ts.Tags = GetCellListString(xls, row, nameof(TaiSan.Tags));
                AddNewType(types, newTypes, TypeConstant.Tags, ts.Tags);

                ts.GenerateCode(taisans);

                if (lastNo == -1 || lastNo == ts.No)
                {
                    cluster.Add(ts);
                }
                else
                {
                    var ts11 = cluster.First();
                    cluster.RemoveAt(0);
                    ts11.Children = cluster;
                    newTaiSans.Add(ts11);

                    cluster = new List<TaiSan> { ts };
                }
                lastNo = ts.No;
            }
            var ts1 = cluster.First();
            cluster.RemoveAt(0);
            ts1.Children = cluster;
            newTaiSans.Add(ts1);

            if (newTaiSans.Any())
                _taiSanRepository.AddRange(newTaiSans);
            if (newTypes.Any())
                _typeRepository.AddRange(newTypes);
        }

        public Task ExportDataAsync(List<TaiSan> taiSans, string pattern, bool includeSub = false)
        {
            if (pattern == "barcode")
                return ExportBarCodeAsync(taiSans);
            var template = ReportVariables.Templates[pattern];
            if (pattern == "data")
                return ExportTaiSanAsync(taiSans, pattern, includeSub);
            else if (template.Item1.Contains("chi tiết") || template.Item1.Contains("đánh giá lại"))
                return ExportReportDetailsAsync(taiSans, pattern);
            else if (template.Item1.Contains("tổng hợp"))
                return ExportReportGroupAsync(taiSans, pattern);
            return Task.CompletedTask;
        }

        public Task ExportTaiSanAsync(List<TaiSan> taiSans, string pattern, bool includeSub = false)
        {
            var template = ReportVariables.Templates[pattern];
            var lst = new List<TaiSan>();
            using (FlexCelReport fr = new FlexCelReport(true))
            {
                var no = 1;
                foreach (var item in taiSans)
                {
                    item.No = no++;
                    item.JoinedTags = string.Join("; ", item.Tags);
                    lst.Add(item);
                    if (includeSub && item.Children.Any())
                    {
                        foreach (var ts in item.Children)
                        {
                            ts.No = item.No;
                            ts.JoinedTags = string.Join("; ", ts.Tags);
                            lst.Add(ts);
                        }
                    }
                }

                fr.AddTable("row", lst);
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

        public Task ExportReportDetailsAsync(List<TaiSan> taiSans, string pattern)
        {
            var template = ReportVariables.Templates[pattern];
            using (FlexCelReport fr = new FlexCelReport(true))
            {
                var no = 1;
                foreach (var item in taiSans)
                {
                    item.No = no++;
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
            foreach (var item in taiSans)
            {
                item.NganSachBo = string.IsNullOrWhiteSpace(item.NguonKinhPhi) ? null : item.NguyenGiaKeToan;
                item.Khac = item.NganSachBo == null ? item.NguyenGiaKeToan : null;
            }
            var template = ReportVariables.Templates[pattern];
            // nhóm theo chủng loại
            var groups = taiSans.Where(m => !string.IsNullOrWhiteSpace(m.ChungLoai)).Select(m => m.ChungLoai).Distinct();
            var reportData = taiSans.Where(m => string.IsNullOrWhiteSpace(m.ChungLoai)).ToList();
            foreach (var item in groups)
            {
                var groupTaiSans = taiSans.Where(m => m.ChungLoai == item).ToList();
                var sum = BuildSumTaiSan(groupTaiSans);
                sum.ChungLoai = item;
                reportData.Add(sum);
            }

            var no = 1;
            var lst = new List<TaiSan>();
            foreach (var item in reportData)
            {
                var ts = BuildSumTaiSan(new List<TaiSan> { item });
                ts.No = no++;
                lst.Add(ts);
            }

            var sumAll = BuildSumTaiSan(lst);
            using (FlexCelReport fr = new FlexCelReport(true))
            {
                fr.AddTable("row", lst);
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
            return $"{pattern} {template.Item1}.xlsx";
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
            sum.GiaTriConLai = sum.TongCong - sum.HaoMonLuyKe;
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
                    item.NguyenGiaChenhLech = KiemKe.GetChenhLech(item.NguyenGiaKeToan, item.NguyenGiaKiemKe);
                    item.GiaTriConLaiChenhLech = KiemKe.GetChenhLech(item.GiaTriConLaiKeToan, item.GiaTriConLaiKiemKe);
                }

                fr.AddTable("row", kiemKes);
                var sum = new KiemKe();
                sum.SoLuongKeToan = kiemKes.Where(m => m.SoLuongKeToan.HasValue).Sum(m => m.SoLuongKeToan);
                sum.SoLuongKiemKe = kiemKes.Where(m => m.SoLuongKiemKe.HasValue).Sum(m => m.SoLuongKiemKe);
                sum.SoLuongChenhLech = KiemKe.GetChenhLech(sum.SoLuongKeToan, sum.SoLuongKiemKe);

                sum.NguyenGiaKeToan = kiemKes.Where(m => m.NguyenGiaKeToan.HasValue).Sum(m => m.NguyenGiaKeToan);
                sum.NguyenGiaKiemKe = kiemKes.Where(m => m.NguyenGiaKiemKe.HasValue).Sum(m => m.NguyenGiaKiemKe);
                sum.NguyenGiaChenhLech = KiemKe.GetChenhLech(sum.NguyenGiaKeToan, sum.NguyenGiaKiemKe);

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
                var firstCell = GetCellInt(xls, row, nameof(KiemKe.No), typeof(KiemKe));
                if (firstCell == null || firstCell.Value == 0) break;

                var kk = new KiemKe();
                kk.Code = GetCellString(xls, row, nameof(KiemKe.Code), typeof(KiemKe));

                if (!taisans.ContainsKey(kk.Code))
                {
                    throw new Exception($"Mã tài sản '{kk.Code}' không tồn tại trong danh mục tài sản");
                }
                kk.KiemKeId = kiemKeId;
                kk.Name = GetCellString(xls, row, nameof(KiemKe.Name), typeof(KiemKe));
                kk.SoLuongKeToan = GetCellInt(xls, row, nameof(KiemKe.SoLuongKeToan), typeof(KiemKe));
                kk.SoLuongKiemKe = GetCellInt(xls, row, nameof(KiemKe.SoLuongKiemKe), typeof(KiemKe));
                kk.NguyenGiaKeToan = GetCellDecimal(xls, row, nameof(KiemKe.NguyenGiaKeToan), typeof(KiemKe));
                kk.NguyenGiaKiemKe = GetCellDecimal(xls, row, nameof(KiemKe.NguyenGiaKiemKe), typeof(KiemKe));
                kk.GiaTriConLaiKeToan = GetCellDecimal(xls, row, nameof(KiemKe.GiaTriConLaiKeToan), typeof(KiemKe));
                kk.GiaTriConLaiKiemKe = GetCellDecimal(xls, row, nameof(KiemKe.GiaTriConLaiKiemKe), typeof(KiemKe));
                kk.GhiChu = GetCellString(xls, row, nameof(KiemKe.GhiChu), typeof(KiemKe));

                newKiemKes.Add(kk);
            }
            if (newKiemKes.Any())
            {
                _kiemKeRepository.DeleteManyAsync(nameof(KiemKe.KiemKeId), kiemKeId).GetAwaiter().GetResult();
                _kiemKeRepository.AddRange(newKiemKes);
            }
        }

        public Task ExportBarCodeAsync(List<TaiSan> taiSans)
        {
            using (FlexCelReport fr = new FlexCelReport(true))
            {
                var no = 1;
                foreach (var item in taiSans)
                {
                    item.No = no++;
                    item.JoinedTags = string.Join("; ", item.Tags);
                }

                fr.AddTable("row", taiSans);
                var xlsx = new XlsFile(true);
                xlsx.Open(TemplateFolder + "ForBarCode.xlsx");
                fr.Run(xlsx);

                bpac.DocumentClass doc = new DocumentClass();
                if (doc.Open(TemplateFolder + "BarCode1.lbx") != false)
                {
                    doc.StartPrint("", PrintOptionConstants.bpoHalfCut);
                    foreach (var item in taiSans)
                    {
                        doc.GetObject("no").Text = item.No.ToString();
                        doc.GetObject("donv").Text = $"Phòng: {item.PhongQuanLy}";
                        doc.GetObject("code").Text = $"Mã tài sản: {item.Code}";
                        doc.GetObject("name").Text = item.Name;
                        doc.GetObject("barcode").Text = $"{item.Code}";
                        doc.PrintOut(1, PrintOptionConstants.bpoCutAtEnd);
                        if (item.Children.Any())
                        {
                            foreach (var sub in item.Children)
                            {
                                doc.GetObject("no").Text = item.No.ToString();
                                doc.GetObject("donv").Text = $"Phòng: {item.PhongQuanLy}";
                                doc.GetObject("code").Text = $"Mã tài sản: {sub.Code} - {item.Code}";
                                doc.GetObject("name").Text = sub.Name;
                                doc.GetObject("barcode").Text = $"{sub.Code}";
                                doc.PrintOut(1, PrintOptionConstants.bpoCutAtEnd);
                            }
                        }
                        //doc.GetObject("no").Text = item.No.ToString();
                        //doc.GetObject("barcode").Text = $"{item.Code}";


                        //doc.SetMediaById(doc.Printer.GetMediaId(), true);

                    }
                    doc.EndPrint();
                    doc.Close();
                }

                using (MemoryStream XlsStream = new MemoryStream())
                {
                    xlsx.Save(XlsStream);
                    return SendToBrowser(XlsStream, "application/excel", "Tài sản dán mã vạch.xlsx");
                }
            }
        }

        public Dictionary<string, int> GetExistingCodes()
        {
            var allTs = _taiSanRepository.Find(m => true).ToList();
            var taisans = allTs.Select(m => new { key = m.Code, value = 1 }).ToDictionary(x => x.key, x => x.value);
            var subTs = allTs.SelectMany(m => m.Children).Select(m => new { key = m.Code, value = 1 }).ToDictionary(x => x.key, x => x.value);

            return taisans.Union(subTs).ToDictionary(k => k.Key, v => v.Value);
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
        private string GetCellString(XlsFile xls, int row, string fieldName, System.Type type = null)
        {
            var colIndex = GetColIndex(type, fieldName);
            object cell = xls.GetCellValue(row, colIndex);
            var value = cell?.ToString()?.Trim();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
        private List<string> GetCellListString(XlsFile xls, int row, string fieldName, System.Type type = null)
        {
            var colIndex = GetColIndex(type, fieldName);
            object cell = xls.GetCellValue(row, colIndex);
            var value = cell?.ToString()?.Trim();
            return string.IsNullOrEmpty(value) ? new List<string>() : value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
        }
        private decimal? GetCellDecimal(XlsFile xls, int row, string fieldName, System.Type type = null)
        {
            var colIndex = GetColIndex(type, fieldName);
            object cell = xls.GetCellValue(row, colIndex);
            if (decimal.TryParse(cell + "", out decimal result))
                return result;
            return null;
        }
        private int? GetCellInt(XlsFile xls, int row, string fieldName, System.Type type = null)
        {
            var colIndex = GetColIndex(type, fieldName);
            object cell = xls.GetCellValue(row, colIndex);
            if (int.TryParse(cell + "", out int result))
                return result;
            return null;
        }
        private int GetColIndex(System.Type type, string fieldName)
        {
            return type == null ? TaiSan.GetCol(fieldName) : KiemKe.GetCol(fieldName);
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
