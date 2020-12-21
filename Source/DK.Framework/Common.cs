using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace DK.Framework
{
    public class StringHelper
    {
        public static HttpContext HttpContext { get { return HttpContext.Current; } }
        public static string RouteData(string key)
        {
            var obj = HttpContext.Current.Request.RequestContext.RouteData.Values[key];
            return obj == null ? null : obj.ToString();
        }
        private static readonly string[] VietnameseSigns = new string[]
        {
                        "aAeEoOuUiIdDyY",
                        "áàạảãâấầậẩẫăắằặẳẵ",
                        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                        "éèẹẻẽêếềệểễ",
                        "ÉÈẸẺẼÊẾỀỆỂỄ",
                        "óòọỏõôốồộổỗơớờợởỡ",
                        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                        "úùụủũưứừựửữ",
                        "ÚÙỤỦŨƯỨỪỰỬỮ",
                        "íìịỉĩ",
                        "ÍÌỊỈĨ",
                        "đ",
                        "Đ",
                        "ýỳỵỷỹ",
                        "ÝỲỴỶỸ"
        };
        public static string HtmlTypeDate(DateTime? date, bool hasTime = false)
        {
            if (date == null) return null;
            return date.Value.ToString("yyyy-MM-dd") + (hasTime ? date.Value.ToString(" HH:mm") : null);
        }
        public static string Base64Img(byte[] data, string defaultImg = "/Content/images/noimage.jpg")
        {
            if (data == null) return defaultImg;
            try
            {
                return "data:image/png;base64," + System.Convert.ToBase64String(data);
            }
            catch { return null; }
        }
        public static string Base64Img(string data, string defaultImg = "/Content/images/noimage.jpg")
        {
            if (data == null) return defaultImg;
            try
            {
                return "data:image/png;base64," + data;
            }
            catch { return null; }
        }
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
        public static string GetStringFromList(List<object> lst, string sapretor = ";")
        {
            if (lst == null) return null;
            string s = null;
            foreach (var item in lst)
            {
                s += item + sapretor;
            }
            return s;
        }
        public static List<int> GetList(string value, string sapretor = ";")
        {
            var lst = new List<int>();
            if (string.IsNullOrEmpty(value)) return lst;
            foreach (var item in value.Split(new string[] { sapretor }, StringSplitOptions.RemoveEmptyEntries))
            {
                lst.Add(int.Parse(item));
            }
            return lst;
        }
        public static List<string> GetListString(string value, string sapretor = ";")
        {
            var lst = new List<string>();
            if (string.IsNullOrEmpty(value)) return lst;
            foreach (var item in value.Split(new string[] { sapretor }, StringSplitOptions.RemoveEmptyEntries))
            {
                lst.Add(item);
            }
            return lst;
        }
        public static string XMLSerialize(object data)
        {
            var serializer = new XmlSerializer(data.GetType());
            string utf8;
            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, data);
                utf8 = writer.ToString();
                return utf8;
            }


            //XmlSerializer SerializerObj = new XmlSerializer(data.GetType());
            //System.IO.StringWriter sww = new System.IO.StringWriter();
            //XmlWriter writer = XmlWriter.Create(sww);

            //SerializerObj.Serialize(writer, data);
            //return sww.ToString();
        }

        public static T XMLDeserialize<T>(string xml, Type type)
        {
            XmlSerializer mySerializer = new XmlSerializer(type);
            //XmlReader reader = XmlReader.Create(xml);

            XmlTextReader reader = new XmlTextReader(new StringReader(xml));

            return (T)mySerializer.Deserialize(reader);
        }
        public static string RemoveSign4Vietnamese(string str)
        {

            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {

                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;
        }

        public static string RemoveSymbol(string str)
        {
            //Giữ 1 ký tự trắng
            str = string.Join(" ", str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));

            //Loại trừ các ký tự đặc biệt - ngoài các ký tự sau
            string notSymbol =
                        " -[]abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" +
                        "áàạảãâấầậẩẫăắằặẳẵ" +
                        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ" +
                        "éèẹẻẽêếềệểễ" +
                        "ÉÈẸẺẼÊẾỀỆỂỄ" +
                        "óòọỏõôốồộổỗơớờợởỡ" +
                        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ" +
                        "úùụủũưứừựửữ" +
                        "ÚÙỤỦŨƯỨỪỰỬỮ" +
                        "íìịỉĩ" +
                        "ÍÌỊỈĨ" +
                        "đ" +
                        "Đ" +
                        "ýỳỵỷỹ" +
                        "ÝỲỴỶỸ";

            for (int i = 0; i < str.Length; i++)
            {
                if (notSymbol.IndexOf(str[i]) == -1)
                {
                    str = str.Remove(i, 1);
                    i--;
                }
            }
            return str;
        }

        public static string CreateURLParam(int id, string value)
        {
            return string.Format("{0}-{1}", RemoveSymbol(RemoveSign4Vietnamese(value)).Replace(" ", "-").Replace("--", "-"), id);
        }
        public static string CreateURLParam(string value)
        {
            return RemoveSymbol(RemoveSign4Vietnamese(value)).Replace(" ", "-").Replace("--", "-");
        }
        public static int GetIDFromURLParam(string param)
        {
            //Trả về 0 nếu không lọc được ID. Khi gọi, kiểm tra != 0 thì thực hiện tiếp
            int id = 0;
            if (!string.IsNullOrEmpty(param))
            {
                int startIndex = param.LastIndexOf('-');
                int endIndex = param.Length;

                if (startIndex < endIndex)
                {
                    int.TryParse(param.Substring(startIndex + 1, endIndex - startIndex - 1), out id);
                }
            }
            return id;
        }
        public static string getYesNO(bool value)
        {
            if (value)
            {
                return "YES";
            }
            return "NO";
        }
        public static DateTime? ChangeFormatDate(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return null;
            }
            string[] t = date.Trim().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            return new DateTime(int.Parse(t[2]), int.Parse(t[0]), int.Parse(t[1]));
        }
        public static string BuildDropdown(object[] values, object[] texts, object selectedValue, string none = "- none -")
        {
            System.Text.StringBuilder st = new StringBuilder();
            if (none != null)
                st = new StringBuilder("<option value=''>" + none + "</option>");
            string s = selectedValue + "";
            for (int i = 0; i < values.Length; i++)
            {
                if (s.Equals(values[i]))
                    st.Append(string.Format("<option value='{0}' selected='selected'>{1}</option>", values[i], texts[i]));
                else
                    st.Append(string.Format("<option value='{0}'>{1}</option>", values[i], texts[i]));
            }

            return st.ToString();
        }
        public static string BuildDropdown(int[] values, object[] texts, object selectedValue, string none = "- none -")
        {
            System.Text.StringBuilder st = new StringBuilder();
            if (none != null)
                st = new StringBuilder("<option value=''>" + none + "</option>");
            string s = selectedValue + "";
            for (int i = 0; i < values.Length; i++)
            {
                if (s.Equals(values[i]))
                    st.Append(string.Format("<option value='{0}' selected='selected'>{1}</option>", values[i], texts[i]));
                else
                    st.Append(string.Format("<option value='{0}'>{1}</option>", values[i], texts[i]));
            }

            return st.ToString();
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
}