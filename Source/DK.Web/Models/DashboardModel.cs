using System.Collections.Generic;

namespace DK.Web.Models
{
    public class DashboardModel
    {
        public int TongSoDotKiemKe { get; set; }
        public int TongSoTaiSan { get; set; }
        public int TongSoXe { get; set; }
        public decimal TongNguonKinhPhi { get; set; }
        public List<Application.Models.Type> RecentKiemKes { get; set; }
        public Dictionary<string, int> DanhMuc { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> DanhMucPercent { get; set; } = new Dictionary<string, int>();
    }
}