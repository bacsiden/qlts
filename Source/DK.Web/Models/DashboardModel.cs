using DK.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DK.Web.Models
{
    public class DashboardModel
    {
        public int TongSoDotKiemKe { get; set; }
        public int TongSoTaiSan { get; set; }
        public int TongSoXe { get; set; }
        public decimal TongNguonKinhPhi { get; set; }
        public List<Application.Models.Type> RecentKiemKes{ get; set; }
        public List<string> DanhMuc{ get; set; }
    }
}