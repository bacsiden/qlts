using System;
using System.ComponentModel.DataAnnotations;

namespace DK.Application.Models
{
    public class TaiSanSearchModel : TaiSan
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

        public int Page { get; set; } = 1;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}