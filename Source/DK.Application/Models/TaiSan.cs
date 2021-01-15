using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
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

        public void GenerateCode(Dictionary<string, int> existingCodes)
        {
            var i = 1;
            GenerateCode();
            while (existingCodes.ContainsKey(Code))
            {
                GenerateCode(i++);
            }
            existingCodes.Add(Code, 1);
        }
        public void GenerateCode(int? i = null)
        {
            Code = $"{ChungLoai} {Name}".RemoveDiacritics().GetFirstChars();
            if (Code != null && Code.Length > 7) Code = Code.Substring(0, 7);
            Code += string.IsNullOrWhiteSpace(Serial) ? null : string.Join(null, Serial.RemoveDiacritics().Where(m => char.IsLetterOrDigit(m))).SubLastString(4);
            if (i != null)
                Code = $"{Code}{i.Value.AddZeroFrefix(3)}";
        }

        public bool IsVehicle()
        {
            var value = $"{ChungLoai}".ToLower();
            return LoaiXe != null || DungTichXiLanh != null || SoChoNgoi != null || value.Contains("phương tiện giao thông") || value.Contains("ô tô");
        }

        public string Display(string fieldName)
        {
            var prop = this.GetType().GetProperty(fieldName);
            var value = prop.GetValue(this);
            if (value == null) return null;
            if (value is decimal) return ((decimal?)value).ToMoneyString();
            return value.ToString();
        }

        public string Display<TProperty>(Expression<Func<TaiSan, TProperty>> property)
        {
            return Display(((MemberExpression)property.Body).Member.Name);
        }

        public Guid ParentId { get; set; }

        [BsonIgnore]
        [ColumnIndex(1)]
        public int No { get; set; }

        [ColumnIndex(2)]
        [Display(Name = "Số hiệu")]
        public int? Number { get; set; }

        // search chính xác
        [ColumnIndex(3)]
        [Display(Name = "Mã tài sản")]
        public string Code { get; set; }

        // search contains
        [ColumnIndex(4)]
        [Display(Name = "Tên tài sản")]
        public string Name { get; set; }

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

        // search contains
        [ColumnIndex(7)]
        [Display(Name = "Nhãn hiệu")]
        public string NhanHieu { get; set; }

        // search chính xác
        [ColumnIndex(8)]
        [Display(Name = "Serial (số khung/số máy)")]
        public string Serial { get; set; }

        // search contains
        [ColumnIndex(9)]
        [Display(Name = "Xuất xứ")]
        public string XuatXu { get; set; }

        // search chính xác
        [ColumnIndex(10)]
        [Display(Name = "Thuộc hợp đồng")]
        public string ThuocHopDong { get; set; }

        // search contains
        [ColumnIndex(11)]
        [Display(Name = "Thuộc gói thầu")]
        public string ThuocGoiThau { get; set; }

        // filter
        [ColumnIndex(12)]
        [Display(Name = "Nguồn kinh phí bộ")]
        public string NguonKinhPhi { get; set; }

        // search chính xác
        [ColumnIndex(13)]
        [Display(Name = "Ngân sách khác")]
        public string NganSachKhac { get; set; }

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
        [Display(Name = "Số lượng")]
        public int? SoLuong { get; set; }

        [ColumnIndex(19)]
        [Display(Name = "Khối lượng (kg)")]
        public int? KhoiLuong { get; set; }

        [ColumnIndex(20)]
        [Display(Name = "Hao mòn lũy kế")]
        public decimal? HaoMonLuyKe { get; set; }

        [ColumnIndex(21)]
        [Display(Name = "Giá trị còn lại")]
        public decimal? GiaTriConLai { get; set; }

        // filter
        [ColumnIndex(22)]
        [Display(Name = "Chất lượng")]
        public string ChatLuong { get; set; }

        // search contains
        [ColumnIndex(23)]
        [Display(Name = "Người sử dụng (cá nhân)")]
        public string NguoiSuDung { get; set; }

        // search contains
        [ColumnIndex(24)]
        [Display(Name = "Người quản lý (theo dõi chung)")]
        public string NguoiQuanLy { get; set; }

        // filter
        [ColumnIndex(25)]
        [Display(Name = "Phòng/Ban quản lý")]
        public string PhongQuanLy { get; set; }

        /// <summary>
        /// chỉ đối với phương tiện
        /// </summary>
        // filter
        [ColumnIndex(26)]
        [Display(Name = "Loại xe")]
        public string LoaiXe { get; set; }

        /// <summary>
        /// chỉ đối với phương tiện
        /// </summary>
        // filter
        [ColumnIndex(27)]
        [Display(Name = "Biển số")]
        public string BienSo { get; set; }

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
        [Display(Name = "Diện tích xây dựng (m2)")]
        [ColumnIndex(31)]
        public int? DienTichXayDung { get; set; }

        /// <summary>
        /// chỉ đối với nhà đất
        /// </summary>
        [Display(Name = "Cấp công trình")]
        [ColumnIndex(32)]
        public string CapCongTrinh { get; set; }

        /// <summary>
        /// chỉ đối với nhà đất
        /// </summary>
        [Display(Name = "Địa chỉ")]
        [ColumnIndex(33)]
        public string DiaChi { get; set; }

        /// <summary>
        /// chỉ đối với nhà đất
        /// </summary>
        [Display(Name = "Diện tích khuôn viên (m2)")]
        [ColumnIndex(34)]
        public int? DienTichKhuonVien { get; set; }

        /// <summary>
        /// Multi select
        /// </summary>
        [Display(Name = "Tags")]
        [ColumnIndex(35)]
        public List<string> Tags { get; set; } = new List<string>();

        [BsonIgnore]
        public string GhiChu { get; set; }

        public string JoinedTags { get; set; }

        public decimal? NganSachBo { get; set; }
        public decimal? Khac { get; set; }
        public decimal? TongCong { get; set; }

        public bool IsApproved { get; set; }

        public List<TaiSan> Children = new List<TaiSan>();
    }
}
