using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace DK.Application.Models
{
    /// <summary>
    /// Tài sản
    /// </summary>
    [BsonIgnoreExtraElements]
    public class TaiSan : BaseEntity
    {
        public static int GetCol(string fieldName)
        {
            var property = typeof(TaiSan).GetProperty(fieldName);
            var attribute = (ColumnIndexAttribute)property.GetCustomAttributes(typeof(ColumnIndexAttribute), true).First();
            return attribute.Index;
        }

        [Display(Name = "Mã tài sản")]
        public string Code { get; set; }

        [Display(Name = "Tên tài sản")]
        public string Name { get; set; }

        /// <summary>
        /// Lấy từ bảng chúng loại
        /// </summary>
        [ColumnIndex(3)]
        [Display(Name = "Chủng loại")]
        public string ChungLoai { get; set; }

        [Display(Name = "Serial (số khung/số máy)")]
        public string Serial { get; set; }

        [Display(Name = "Xuất xứ")]
        public string XuatXu { get; set; }

        [Display(Name = "Thuộc hợp đồng")]
        public string ThuocHopDong { get; set; }

        [Display(Name = "Thuộc gói thầu")]
        public string ThuocGoiThau { get; set; }

        [Display(Name = "Nguồn kinh phí")]
        public string NguonKinhPhi { get; set; }

        [Display(Name = "Ngân sách năm")]
        public int NganSachNam { get; set; }

        [Display(Name = "Năm sản xuất")]
        public int NamSanXuat { get; set; }

        [Display(Name = "Năm sử dụng")]
        public int NamSuDung { get; set; }

        [Display(Name = "Số lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Nguyên giá kế toán")]
        public decimal? NguyenGiaKeToan { get; set; }

        [Display(Name = "Nguyên giá kiểm kê")]
        public decimal? NguyenGiaKiemKe { get; set; }

        [Display(Name = "Chất lượng")]
        public string ChatLuong { get; set; }

        [Display(Name = "Người sử dụng (cá nhân)")]
        public string NguoiSuDung { get; set; }

        [Display(Name = "Người quản lý (theo dõi chung)")]
        public string NguoiQuanLy { get; set; }

        [Display(Name = "Phòng/Ban quản lý")]
        public string PhongQuanLy { get; set; }

        /// <summary>
        /// chỉ đối với phương tiện
        /// </summary>
        [Display(Name = "Loại xe")]
        public string LoaiXe { get; set; }

        /// <summary>
        /// Multi select
        /// </summary>
        public List<string> Tags { get; set; }
    }
}
