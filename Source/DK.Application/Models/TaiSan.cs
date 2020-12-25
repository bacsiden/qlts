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

        public void GenerateCode(int? i = null)
        {
            Code = $"fuck-{i}";
        }

        public bool IsVehicle()
        {
            var value = $"{ChungLoai}{DanhMuc}".ToLower();
            return LoaiXe != null || DungTichXiLanh != null || SoChoNgoi != null || value.Contains("phương tiện giao thông") || value.Contains("ô tô");
        }

        // search chính xác
        [ColumnIndex(2)]
        [Display(Name = "Mã tài sản")]
        public string Code { get; set; }

        // search contains
        [ColumnIndex(3)]
        [Display(Name = "Tên tài sản")]
        public string Name { get; set; }

        // search chính xác
        [ColumnIndex(4)]
        [Display(Name = "Mã nhóm tài sản")]
        public string GroupCode { get; set; }

        // search contains
        [ColumnIndex(5)]
        [Display(Name = "Tên nhóm tài sản")]
        public string GroupName { get; set; }

        /// <summary>
        /// Lấy từ bảng chúng loại
        /// </summary>
        // filter
        [ColumnIndex(6)]
        [Display(Name = "Chủng loại")]
        public string ChungLoai { get; set; }

        // filter
        [ColumnIndex(7)]
        [Display(Name = "Danh mục")]
        public string DanhMuc { get; set; }

        // search contains
        [ColumnIndex(8)]
        [Display(Name = "Nhãn hiệu")]
        public string NhanHieu { get; set; }

        // search chính xác
        [ColumnIndex(9)]
        [Display(Name = "Serial (số khung/số máy)")]
        public string Serial { get; set; }

        // search contains
        [ColumnIndex(10)]
        [Display(Name = "Xuất xứ")]
        public string XuatXu { get; set; }

        // search chính xác
        [ColumnIndex(11)]
        [Display(Name = "Thuộc hợp đồng")]
        public string ThuocHopDong { get; set; }

        // search contains
        [ColumnIndex(12)]
        [Display(Name = "Thuộc gói thầu")]
        public string ThuocGoiThau { get; set; }

        // filter
        [ColumnIndex(13)]
        [Display(Name = "Nguồn kinh phí")]
        public string NguonKinhPhi { get; set; }

        // search chính xác
        [ColumnIndex(14)]
        [Display(Name = "Ngân sách năm")]
        public int? NganSachNam { get; set; }

        // search chính xác
        [ColumnIndex(15)]
        [Display(Name = "Năm sản xuất")]
        public int? NamSanXuat { get; set; }

        // search chính xác
        [ColumnIndex(16)]
        [Display(Name = "Năm sử dụng")]
        public int? NamSuDung { get; set; }

        [ColumnIndex(17)]
        [Display(Name = "Nguyên giá kế toán")]
        public decimal? NguyenGiaKeToan { get; set; }

        [ColumnIndex(18)]
        [Display(Name = "Số lượng kế toán")]
        public int? SoLuong { get; set; }

        [ColumnIndex(19)]
        [Display(Name = "Nguyên giá kiểm kê")]
        public decimal? NguyenGiaKiemKe { get; set; }

        [ColumnIndex(20)]
        [Display(Name = "Số lượng kiểm kê")]
        public int? SoLuongKiemKe { get; set; }

        [ColumnIndex(21)]
        [Display(Name = "Hao mòn lũy kế")]
        public int? HaoMonLuyKe { get; set; }

        [ColumnIndex(22)]
        [Display(Name = "Giá trị còn lại")]
        public string GiaTriConLai { get; set; }

        // filter
        [ColumnIndex(23)]
        [Display(Name = "Chất lượng")]
        public string ChatLuong { get; set; }

        // search contains
        [ColumnIndex(24)]
        [Display(Name = "Người sử dụng (cá nhân)")]
        public string NguoiSuDung { get; set; }

        // search contains
        [ColumnIndex(25)]
        [Display(Name = "Người quản lý (theo dõi chung)")]
        public string NguoiQuanLy { get; set; }

        // filter
        [ColumnIndex(26)]
        [Display(Name = "Phòng/Ban quản lý")]
        public string PhongQuanLy { get; set; }

        /// <summary>
        /// chỉ đối với phương tiện
        /// </summary>
        // filter
        [ColumnIndex(27)]
        [Display(Name = "Loại xe")]
        public string LoaiXe { get; set; }

        /// <summary>
        /// chỉ đối với phương tiện
        /// </summary>
        [Display(Name = "Dung tích xi lanh")]
        [ColumnIndex(28)]
        public int? DungTichXiLanh { get; set; }

        /// <summary>
        /// chỉ đối với phương tiện
        /// </summary>
        [Display(Name = "Số chỗ ngồi")]
        [ColumnIndex(29)]
        public int? SoChoNgoi { get; set; }

        /// <summary>
        /// chỉ đối với nhà đất
        /// </summary>
        [Display(Name = "Số tầng")]
        [ColumnIndex(30)]
        public int? SoTang { get; set; }

        /// <summary>
        /// chỉ đối với nhà đất
        /// </summary>
        [Display(Name = "Diện tích xây dựng")]
        [ColumnIndex(31)]
        public int? DienTichXayDung { get; set; }

        /// <summary>
        /// chỉ đối với nhà đất
        /// </summary>
        [Display(Name = "Cấp công trình")]
        [ColumnIndex(32)]
        public int? CapCongTrinh { get; set; }

        /// <summary>
        /// chỉ đối với nhà đất
        /// </summary>
        [Display(Name = "Địa chỉ")]
        [ColumnIndex(33)]
        public string DiaChi { get; set; }

        /// <summary>
        /// chỉ đối với nhà đất
        /// </summary>
        [Display(Name = "Diện tích khuôn viên")]
        [ColumnIndex(34)]
        public int? DienTichKhuonVien { get; set; }

        /// <summary>
        /// Multi select
        /// </summary>
        [ColumnIndex(35)]
        public List<string> Tags { get; set; }

        public string JoinedTags { get; set; }
    }
}
