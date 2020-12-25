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
using FlexCel.Report;
using FlexCel.XlsAdapter;

namespace DK.Application
{
    public class TaiSanService : ITaiSanService
    {
        protected readonly string TemplateFolder = HttpContext.Current.Server.MapPath("~/ReportTemplates\\");
        private readonly ITaiSanRepository _taiSanRepository;
        private readonly ITypeRepository _typeRepository;
        public TaiSanService(ITaiSanRepository taiSanRepository, ITypeRepository typeRepository)
        {
            _taiSanRepository = taiSanRepository;
            _typeRepository = typeRepository;
            //xlsx.Open(TemplateFolder + "TimeLine.xlsx");
        }

        public void Import()
        {
            var taisans = _taiSanRepository.Find(m => true).ToList().Select(m => new { key = m.Code, value = m }).ToDictionary(x => x.key, x => x.value);
            var types = _typeRepository.Find(m => true).ToList();
            var newTypes = new List<Models.Type>();

            XlsFile xls = new XlsFile(TemplateFolder + "ImportForm.xlsx");
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
                if (!types.Any(m => m.Name == TypeConstant.ChungLoai && m.Title == ts.ChungLoai)) newTypes.Add(new Models.Type { Name = TypeConstant.ChungLoai, Title = ts.ChungLoai });
                ts.DanhMuc = GetCellString(xls, row, nameof(TaiSan.DanhMuc));
                if (!types.Any(m => m.Name == TypeConstant.DanhMuc && m.Title == ts.DanhMuc)) newTypes.Add(new Models.Type { Name = TypeConstant.DanhMuc, Title = ts.DanhMuc });
                ts.NhanHieu = GetCellString(xls, row, nameof(TaiSan.NhanHieu));
                ts.Serial = GetCellString(xls, row, nameof(TaiSan.Serial));
                ts.XuatXu = GetCellString(xls, row, nameof(TaiSan.XuatXu));
                ts.ThuocHopDong = GetCellString(xls, row, nameof(TaiSan.ThuocHopDong));
                ts.ThuocGoiThau = GetCellString(xls, row, nameof(TaiSan.ThuocGoiThau));
                ts.NguonKinhPhi = GetCellString(xls, row, nameof(TaiSan.NguonKinhPhi));
                if (!types.Any(m => m.Name == TypeConstant.NguonKinhPhi && m.Title == ts.NguonKinhPhi)) newTypes.Add(new Models.Type { Name = TypeConstant.NguonKinhPhi, Title = ts.NguonKinhPhi });
                ts.NganSachNam = GetCellInt(xls, row, nameof(TaiSan.NganSachNam));
                ts.NamSanXuat = GetCellInt(xls, row, nameof(TaiSan.NamSanXuat));
                ts.NamSuDung = GetCellInt(xls, row, nameof(TaiSan.NamSuDung));
                ts.NguyenGiaKeToan = GetCellDecimal(xls, row, nameof(TaiSan.NguyenGiaKeToan));
                ts.SoLuong = GetCellInt(xls, row, nameof(TaiSan.SoLuong));
                ts.NguyenGiaKiemKe = GetCellDecimal(xls, row, nameof(TaiSan.NguyenGiaKiemKe));
                ts.SoLuongKiemKe = GetCellInt(xls, row, nameof(TaiSan.SoLuongKiemKe));
                ts.ChatLuong = GetCellString(xls, row, nameof(TaiSan.ChatLuong));
                if (!types.Any(m => m.Name == TypeConstant.ChatLuong && m.Title == ts.ChatLuong)) newTypes.Add(new Models.Type { Name = TypeConstant.ChatLuong, Title = ts.ChatLuong });
                ts.HaoMonLuyKe = GetCellInt(xls, row, nameof(TaiSan.HaoMonLuyKe));
                ts.GiaTriConLai = GetCellString(xls, row, nameof(TaiSan.GiaTriConLai));
                ts.NguoiSuDung = GetCellString(xls, row, nameof(TaiSan.NguoiSuDung));
                ts.NguoiQuanLy = GetCellString(xls, row, nameof(TaiSan.NguoiQuanLy));
                ts.PhongQuanLy = GetCellString(xls, row, nameof(TaiSan.PhongQuanLy));
                if (!types.Any(m => m.Name == TypeConstant.PhongBan && m.Title == ts.PhongQuanLy)) newTypes.Add(new Models.Type { Name = TypeConstant.PhongBan, Title = ts.PhongQuanLy });
                ts.LoaiXe = GetCellString(xls, row, nameof(TaiSan.LoaiXe));
                if (!types.Any(m => m.Name == TypeConstant.LoaiXe && m.Title == ts.LoaiXe)) newTypes.Add(new Models.Type { Name = TypeConstant.LoaiXe, Title = ts.LoaiXe });
                ts.DungTichXiLanh = GetCellInt(xls, row, nameof(TaiSan.DungTichXiLanh));
                ts.SoChoNgoi = GetCellInt(xls, row, nameof(TaiSan.SoChoNgoi));
                ts.SoTang = GetCellInt(xls, row, nameof(TaiSan.SoTang));
                ts.DienTichXayDung = GetCellInt(xls, row, nameof(TaiSan.DienTichXayDung));
                ts.CapCongTrinh = GetCellInt(xls, row, nameof(TaiSan.CapCongTrinh));
                ts.DiaChi = GetCellString(xls, row, nameof(TaiSan.DiaChi));
                ts.DienTichKhuonVien = GetCellInt(xls, row, nameof(TaiSan.DienTichKhuonVien));
                ts.Tags = GetCellListString(xls, row, nameof(TaiSan.Tags));

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

        public Task ExportAsync(List<TaiSan> taiSans)
        {
            var taisans = _taiSanRepository.Find(m => true).Select(m => new { key = m.Code, value = m }).ToDictionary(x => x.key, x => x.value);

            using (FlexCelReport fr = new FlexCelReport(true))
            {
                foreach (var item in taiSans)
                {
                    item.JoinedTags = string.Join("; ", item.Tags);
                }

                fr.AddTable("row", taiSans);
                var xlsx = new XlsFile(true);
                xlsx.Open(TemplateFolder + "ImportForm.xlsx");
                fr.Run(xlsx);
                using (MemoryStream XlsStream = new MemoryStream())
                {
                    xlsx.Save(XlsStream);
                    return SendToBrowser(XlsStream, "application/excel", $"Danh-sach-tai-san.xlsx");
                }
            }
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
