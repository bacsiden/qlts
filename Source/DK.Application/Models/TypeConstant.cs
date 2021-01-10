using System;
using System.Linq;

namespace DK.Application.Models
{
    public static class TypeConstant
    {
        public const string ChungLoai = "Chủng loại";
        public const string DanhMuc = "Danh mục";
        public const string NguonKinhPhi = "Nguồn kinh phí";
        public const string ChatLuong = "Chất lượng";
        public const string LoaiXe = "Loại xe";
        public const string PhongBan = "Phòng, Đơn vị";
        public const string Tags = "Tags";
        public const string KiemKe = "KiemKe";


        public static string GetFirstChars(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            return string.Join(null, input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).
                Select(m => m.FirstOrDefault()).Where(x => char.IsLetterOrDigit(x))).ToUpper();
        }

        public static string SubLastString(this string input, int length)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            if (input.Length <= length) return input;
            return input.Substring(input.Length - length, length);
        }

        public static string AddZeroFrefix(this int input, int length)
        {
            var output = input.ToString();
            while (output.Length < length) output = $"0{output}";
            return output;
        }
    }
}
