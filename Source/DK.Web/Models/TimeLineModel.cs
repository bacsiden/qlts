using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DK.Web.Models
{
    public class CreateTimeLineModel
    {
        public int ClassID { get; set; }
        public string ClassTitle { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<int> TimeLineIDs { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<string> NoiDungs { get; set; }
        public List<string> ThoiGians { get; set; }
        public List<string> NguoiPhuTrachs { get; set; }
        public List<string> GiaoViens { get; set; }
        public List<string> DiaDiems { get; set; }
        public List<string> VatChats { get; set; }
    }
}