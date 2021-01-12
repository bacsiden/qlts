using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DK.Application.Models
{
    public static class TypeConstant
    {
        public const string ChungLoai = "Chủng loại";
        public const string Group = "Nhóm tài sản";
        public const string NguonKinhPhi = "Nguồn kinh phí";
        public const string ChatLuong = "Chất lượng";
        public const string LoaiXe = "Loại xe";
        public const string PhongBan = "Phòng, Đơn vị";
        public const string Tags = "Tags";
        public const string KiemKe = "KiemKe";

        public const string GQuanLy = "Tài sản phục vụ công tác quản lý";
        public const string GDacBiet = "Tài sản đặc biệt";
        public const string GChuyenDung = "Tài sản chuyên dùng";

        public static List<string> Groups = new List<string> { GQuanLy, GDacBiet, GChuyenDung };
        public static string ToMoneyString(this decimal? input)
        {
            if (!input.HasValue) return null;
            return input.Value.ToString("N0");
        }
        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
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
