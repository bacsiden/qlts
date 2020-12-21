using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DK.Web
{
    public static class Functions
    {
        public static string AvatarIcon(string img)
        {
            if (string.IsNullOrEmpty(img))
                return "/Content/images/icon-user.png";
            return img;
        }

        public static String NumberToTextVN(decimal total)
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười").Replace("Mười,", "Mười").Replace(" đồng", null);
            }
            catch
            {
                return "";
            }

        }
    }

    public static class DataTimeExtention
    {
        private static readonly string[] DayOfWeekVNs = { "CN", "HAI", "BA", "TƯ", "NĂM", "SÁU", "BẢY" };
        public static string ToStringVN(this DateTime obj)
        {
            return obj.ToString("dd/MM/yyyy");
        }
        public static string ToStringVN(this DateTime? obj)
        {
            if (!obj.HasValue) return null;

            return obj.Value.ToString("dd/MM/yyyy");
        }
        public static string ToStringForHtml(this DateTime obj)
        {
            return obj.ToString("yyyy-MM-dd");
        }
        public static string ToStringForHtml(this DateTime? obj)
        {
            if (!obj.HasValue) return null;

            return obj.Value.ToString("yyyy-MM-dd");
        }
        public static string DayOfWeekVN(this DateTime obj)
        {
            return DayOfWeekVNs[(int)obj.DayOfWeek];
        }
    }
    public static class StringExtention
    {
        public static List<string> ToList(this string obj, params string[] sep)
        {
            if (string.IsNullOrEmpty(obj))
                return new List<string>();
            if (sep.Length == 0) sep = new string[] { "\n" };
            return obj.Split(sep, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static string TimeLineBr(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return value.Replace("\n", "<br />");
        }
    }
}
