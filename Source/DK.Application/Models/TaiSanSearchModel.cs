using System;
using System.ComponentModel.DataAnnotations;

namespace DK.Application.Models
{
    public class TaiSanSearchModel : TaiSan, ISearchModel
    {
        [Display(Name = "Ngân sách năm")]
        public int? NganSachNamSearch { get; set; }

        [Display(Name = "Năm sản xuất")]
        public int? NamSanXuatSearch { get; set; }

        [Display(Name = "Năm sử dụng")]
        public int? NamSuDungSearch { get; set; }

        [Display(Name = "Số lượng")]
        public int? SoLuongSearch { get; set; }

        [Display(Name = "Giá kế toán từ")]
        public decimal? GiaKeToanTu { get; set; }

        [Display(Name = "Giá kế toán đến")]
        public decimal? GiaKeToanDen { get; set; }

        [Display(Name = "Giá kiểm kê từ")]
        public decimal? GiaKiemKeTu { get; set; }

        [Display(Name = "Giá kiểm kê đến")]
        public decimal? GiaKiemKeDen { get; set; }
        public string SearchText { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IncludeSub { get; set; }
        public object ToPagingModel()
        {
            return new
            {
                Code,
                Name,
                GroupName,
                ChungLoai,
                NhanHieu,
                Serial,
                XuatXu,
                ThuocHopDong,
                ThuocGoiThau,
                NguonKinhPhi,
                NganSachNamSearch,
                NamSanXuatSearch,
                NamSuDungSearch,
                ChatLuong,
                NguoiSuDung,
                NguoiQuanLy,
                PhongQuanLy,
                LoaiXe,
                PageSize,
                Tags = string.Join(", ", Tags)
            };
        }

        public string pattern { get; set; }
    }
}