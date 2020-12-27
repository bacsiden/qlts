using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DK.Application.Models
{
    public class ReportVariables
    {
        public static Dictionary<string, Tuple<string, string>> Templates = new Dictionary<string, Tuple<string, string>> {
            { "data", new Tuple<string, string>("Danh sách tài sản", "ImportForm.xlsx") },

            { "1A", new Tuple<string, string>("Biên bản kiểm kê tài sản đặc biệt", "1ABC.xlsx") },
            { "1B", new Tuple<string, string>("Biên bản kiểm kê tài sản chuyên dùng", "1ABC.xlsx") },
            { "1C", new Tuple<string, string>("Biên bản kiểm kê tài sản phục vụ công tác quản lý", "1ABC.xlsx") },

            { "2A", new Tuple<string, string>("Báo cáo tổng hợp tài sản đặc biệt", "2ABC.xlsx") },
            { "2B", new Tuple<string, string>("Báo cáo tổng hợp tài chuyên dùng", "2ABC.xlsx") },
            { "2C", new Tuple<string, string>("Báo cáo tổng hợp tài sản phục vụ công tác quản lý", "2ABC.xlsx") },

            { "3A", new Tuple<string, string>("Báo cáo chi tiết tài sản đặc biệt", "3AB.xlsx") },
            { "3B", new Tuple<string, string>("Báo cáo chi tiết tài chuyên dùng", "3AB.xlsx") },
            { "3C", new Tuple<string, string>("Báo cáo chi tiết tài sản phục vụ công tác quản lý", "3C.xlsx") },

            { "4A", new Tuple<string, string>("Báo cáo chi tiết tài sản đặc biệt", "4ABC.xlsx") },
            { "4B", new Tuple<string, string>("Báo cáo chi tiết tài chuyên dùng", "4ABC.xlsx") },
            { "4C", new Tuple<string, string>("Báo cáo chi tiết tài sản phục vụ công tác quản lý", "4ABC.xlsx") },

            { "5A", new Tuple<string, string>("Báo cáo chi tiết tài sản đặc biệt", "5ABC.xlsx") },
            { "5B", new Tuple<string, string>("Báo cáo chi tiết tài chuyên dùng", "5ABC.xlsx") },
            { "5C", new Tuple<string, string>("Báo cáo chi tiết tài sản phục vụ công tác quản lý", "5ABC.xlsx") },

            { "6B", new Tuple<string, string>("Báo cáo chi tiết tài chuyên dùng", "6BC.xlsx") },
            { "6C", new Tuple<string, string>("Báo cáo chi tiết tài sản phục vụ công tác quản lý", "6BC.xlsx") },

            { "6A", new Tuple<string, string>("Báo cáo chi tiết tài sản đặc biệt", "6A7BC.xlsx") },
            { "7B", new Tuple<string, string>("Báo cáo chi tiết tài chuyên dùng", "6A7BC.xlsx") },
            { "7C", new Tuple<string, string>("Báo cáo chi tiết tài sản phục vụ công tác quản lý", "6A7BC.xlsx") },

            { "7A", new Tuple<string, string>("Biên bản đánh giá lại tài sản đặc biệt", "7A8BC.xlsx") },
            { "8B", new Tuple<string, string>("Biên bản đánh giá lại tài chuyên dùng", "7A8BC.xlsx") },
            { "8C", new Tuple<string, string>("Biên bản đánh giá lại tài sản phục vụ công tác quản lý", "7A8BC.xlsx") },
        };
    }
}
